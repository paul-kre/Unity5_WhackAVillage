using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns true if the mask contains the given layer.
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return ((mask.value & (1 << layer)) > 0);
        }

    }
}