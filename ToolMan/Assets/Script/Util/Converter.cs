using System.Collections;
using System.Collections.Generic;

namespace ToolMan.Util
{
    public static class Converter
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
    }
}