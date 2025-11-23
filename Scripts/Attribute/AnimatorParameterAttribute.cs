using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTools.AnimatorParameter
{
    public class AnimatorParameterAttribute : PropertyAttribute
    {
        public readonly string AnimatorFieldName;
        public AnimatorParameterAttribute(string animatorFieldName)
        {
            AnimatorFieldName = animatorFieldName;
        }
    }
}
