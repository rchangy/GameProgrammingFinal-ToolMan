using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolMan.Util
{
    public static class LayerMaskUtil
    {
        public static int LayerBitMaskToLayerNumber(int bitmask)
        {
            int result = bitmask > 0 ? 0 : 31;
            while (bitmask > 1)
            {
                bitmask = bitmask >> 1;
                result++;
            }
            return result;
        }
        public static bool IsInLayerMask(this LayerMask mask, int layer)
        {
            return ((mask.value & (1 << layer)) > 0);
        }
    }
}