using System;
using UnityEngine;

namespace GameTools.AnimatorParameter
{
    public readonly struct ColorScope : IDisposable
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