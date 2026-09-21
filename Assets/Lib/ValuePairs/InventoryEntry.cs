using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Lib.ValuePairs
{
    [System.Serializable]
    public struct InventoryEntry
    {
        public GoodsId key;
        public int value;
    }
}
