using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

namespace GameTools.AnimatorParameter
{
    [CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
    public class AnimatorParameterDrawer : PropertyDrawer
    {
        private GUIStyle mainStyle;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            mainStyle ??= new GUIStyle(EditorStyles.label) { wordWrap = true, fontStyle = FontStyle.Bold };

            if (property.propertyType != SerializedPropertyType.String)
            {
                ShowError(position, label, Utility.GetLocalizedText("TypeError", property.propertyType));
                return;
            }

            var animatorAttr = (AnimatorParameterAttribute)attribute;
            var target = property.serializedObject.targetObject;
            var fieldInfo = GetFieldInfo(target, animatorAttr.AnimatorFieldName);
            if (fieldInfo == null)
            {
                ShowError(position, label, Utility.GetLocalizedText("FieldNotFound", animatorAttr.AnimatorFieldName));
                return;
            }

            var parameterNames = GetParameterNames(fieldInfo, target);
            if (parameterNames == null || parameterNames.Length == 0)
            {
                ShowMsg(position, label, Utility.GetLocalizedText("NoParametersMsg"), Color.clear, false);
                property.stringValue = string.Empty;
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
            object rawValue = fieldInfo?.GetValue(target);
            if (Utility.ObjectIsNull(rawValue))
            {
                return null;
            }
            return rawValue switch
            {
                Animator animator => GetAnimatorParameterNames(animator).ToArray(),
                AnimatorController controller => controller.parameters.Select(p => p.name).ToArray(),
                _ => null
            };
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

            bool isMissing = selectedIndex == -1 && !string.IsNullOrEmpty(currentStringValue);
            if (isMissing)
            {
                var displayOptions = new List<string>(parameterNames)
                {
                    Utility.GetLocalizedText("MissingValue", currentStringValue)
                };

                int missingIndex = displayOptions.Count - 1;
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

            if (selectedIndex == -1)
                selectedIndex = 0;

            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, parameterNames);
            property.stringValue = parameterNames[selectedIndex];
        }

        private void ShowError(Rect position, GUIContent label, string message)
        {
            ShowMsg(position, label, message, Color.red, true);
        }

        private void ShowMsg(Rect position, GUIContent label, string message, Color color, bool drawBackground)
        {
            Rect fullRect = EditorGUI.IndentedRect(position);
            fullRect.height = EditorGUIUtility.singleLineHeight;

            if (drawBackground)
            {
                Color backgroundColor = new Color(color.r, color.g, color.b, 0.3f);
                EditorGUI.DrawRect(fullRect, backgroundColor);
            }

            using (new ColorScope(Color.black))
            {
                EditorGUI.LabelField(position, label, new GUIContent(message));
            }
        }
    }
}
