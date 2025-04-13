using UnityEngine;

namespace GameTools.AnimatorParameter
{
    public class UseParameterSimple : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        [SerializeField, AnimatorParameter(nameof(anim))]
        private string parameter;
    }
}
