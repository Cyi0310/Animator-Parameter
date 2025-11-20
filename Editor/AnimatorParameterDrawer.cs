using System;
using System.Collections.Generic;
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
        private GUIStyle errorShow;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            errorShow ??= new GUIStyle(EditorStyles.popup) { normal = { textColor = Color.red } };

            var animatorAttr = (AnimatorParameterAttribute)attribute;
            var target = property.serializedObject.targetObject;
            var fieldInfo = GetFieldInfo(target, animatorAttr.AnimatorFieldName);
            if (fieldInfo == null)
            {
                ShowError(position, label, $"Field '{animatorAttr.AnimatorFieldName}' not found!");
                return;
            }

            var parameterNames = GetParameterNames(fieldInfo, target);
            if (parameterNames.Length == 0 || parameterNames == null)
            {
                object rawValue = fieldInfo.GetValue(target);
                var unityObject = rawValue as UnityEngine.Object;

                if (unityObject == null)
                {
                    ShowError(position, label, $"Assign {animatorAttr.AnimatorFieldName} first!");
                }
                else
                {
                    ShowError(position, label, "Animator has no parameters");
                }
                return;
            }

            DrawDropdown(position, property, label, parameterNames);
        }

        private FieldInfo GetFieldInfo(UnityEngine.Object target, string fieldName)
        {
            var targetType = target.GetType();
            const BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            FieldInfo field = targetType.GetField(fieldName, bindingAttr);
            while (field == null && targetType.BaseType != null)
            {
                targetType = targetType.BaseType;
                field = targetType.GetField(fieldName, bindingAttr);
            }

            return field;
        }

        private string[] GetParameterNames(FieldInfo fieldInfo, UnityEngine.Object target)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();
            object value = fieldInfo?.GetValue(target);
            var unityObject = value as UnityEngine.Object;

            if (unityObject == null)
            { }
            else if (value is Animator animator)
            {
                result = GetAnimatorParameterNames(animator);
            }
            else if (value is AnimatorController animatorController)
            {
                result = animatorController.parameters
                                           .Select(p => p.name);
            }
            return result.ToArray();
        }

        private IEnumerable<string> GetAnimatorParameterNames(Animator animator)
        {
            if (animator?.runtimeAnimatorController is AnimatorController controller)
            {
                return controller.parameters.Select(p => p.name);
            }

            return (animator != null && animator.runtimeAnimatorController != null)
                    ? animator.parameters.Select(p => p.name)
                    : Enumerable.Empty<string>();
        }

        private void DrawDropdown(Rect position, SerializedProperty property, GUIContent label, string[] parameterNames)
        {
            string currentStringValue = property.stringValue;
            int selectedIndex = Array.IndexOf(parameterNames, property.stringValue);

            bool isMissing = false;
            if (selectedIndex == -1)
            {
                if (!string.IsNullOrEmpty(currentStringValue))
                {
                    isMissing = true;
                }

                selectedIndex = 0;
            }

            if (isMissing)
            {
                var displayOptions = new List<string>(parameterNames)
                {
                    $"<Missing: {currentStringValue}>"
                };

                int missingIndex = displayOptions.Count - 1;

                ShowError(position, label, label.text);

                int newIndex;
                using (new ColorScope(Color.red))
                {
                    newIndex = EditorGUI.Popup(position, label.text, missingIndex, displayOptions.ToArray());
                }

                if (newIndex != missingIndex)
                {
                    property.stringValue = parameterNames[newIndex];
                }
                return;
            }

            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, parameterNames);
            property.stringValue = parameterNames[selectedIndex];
        }

        private void ShowError(Rect position, GUIContent label, string message)
        {
            using (new ColorScope(Color.red))
            {
                EditorGUI.LabelField(position, label, new GUIContent(message));
            }
        }

        private readonly struct ColorScope : IDisposable
        {
            private readonly Color originalColor;
            public ColorScope(Color color)
            {
                originalColor = GUI.color;
                GUI.color = color;
            }
            public void Dispose() => GUI.color = originalColor;
        }
    }
}
