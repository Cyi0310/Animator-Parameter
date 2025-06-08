using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GameTools.AnimatorParameter
{
    [CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
    public class AnimatorParameterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var animatorAttr = (AnimatorParameterAttribute)attribute;
            var target = property.serializedObject.targetObject;
            var fieldInfo = GetFieldInfo(target, animatorAttr.AnimatorFieldName);
            var parameterNames = GetParameterNames(fieldInfo, target);

            if (parameterNames.Length == 0)
            {
                var emptyMsg = LocalizationManager.Get("EmptyMsg");
                EditorGUI.LabelField(position, label.text, emptyMsg);
                return;
            }
            DrawDropdown(position, property, label, parameterNames);
        }

        private FieldInfo GetFieldInfo(UnityEngine.Object target, string fieldName)
        {
            var targetType = target.GetType();
            const BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            return targetType.GetField(fieldName, bindingAttr);
        }

        private string[] GetParameterNames(FieldInfo fieldInfo, UnityEngine.Object target)
        {
            var animator = fieldInfo?.GetValue(target) as Animator;
            if (animator?.runtimeAnimatorController is AnimatorController controller)
            {
                return controller.parameters.Select(p => p.name).ToArray();
            }

            return (animator != null && animator.runtimeAnimatorController != null)
                    ? animator.parameters.Select(p => p.name).ToArray()
                    : Array.Empty<string>();
        }

        private void DrawDropdown(Rect position, SerializedProperty property, GUIContent label, string[] parameterNames)
        {
            int selectedIndex = Array.IndexOf(parameterNames, property.stringValue);
            if (selectedIndex == -1)
            {
                selectedIndex = 0;
            }

            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, parameterNames);
            property.stringValue = parameterNames[selectedIndex];
        }
    }
}
