
namespace GameTools.AnimatorParameter
{
    public static class Utility
    {
        public static string GetLocalizedText(string key, params object[] args)
        {
            string format = LocalizationManager.Get(key);
            return args.Length > 0 ? string.Format(format, args) : format;
        }

        public static bool ObjectIsNull(object source)
        {
            if (source is UnityEngine.Object unityObject)
            {
                return unityObject == null;
            }
            return source == null;
        }
    }
}