using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaidLootAnnotator
{
    [Serializable]
    public class LootData
    {
        public int EarsValue { get; set; } = 0;
        public int NeckValue { get; set; } = 0;
        public int WristsValue { get; set; } = 0;
        public int RingValue { get; set; } = 0;
    }
}
