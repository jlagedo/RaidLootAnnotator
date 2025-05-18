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
        //acc
        public int EarsValue { get; set; } = 0;
        public int NeckValue { get; set; } = 0;
        public int WristsValue { get; set; } = 0;
        public int RingValue { get; set; } = 0;
        // gear
        public int WeaponValue { get; set; } = 0;
        public int HeadValue { get; set; } = 0;
        public int BodyValue { get; set; } = 0;
        public int HandsValue { get; set; } = 0;
        public int LegsValue { get; set; } = 0;
        public int FeetValue { get; set; } = 0;

        // upgrades
        public int WeaponTokenValue { get; set; } = 0;
        public int WeaponUpgradeValue { get; set; } = 0;
        public int AccUpgradeValue { get; set; } = 0;
        public int GearUpgradeValue { get; set; } = 0;
    }
}
