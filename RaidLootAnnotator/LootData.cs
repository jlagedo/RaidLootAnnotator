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
        public void ResetAllValues()
        {
            EarsValue = 0;
            NeckValue = 0;
            WristsValue = 0;
            RingValue = 0;
            WeaponValue = 0;
            HeadValue = 0;
            BodyValue = 0;
            HandsValue = 0;
            LegsValue = 0;
            FeetValue = 0;
            WeaponTokenValue = 0;
            WeaponUpgradeValue = 0;
            AccUpgradeValue = 0;
            GearUpgradeValue = 0;
        }
    }
}
