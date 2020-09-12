﻿

namespace NWN.FinalFantasy.Core.NWNX.Enum
{
    public class EffectUnpacked
    {
        public int EffectID { get; set; }
        public int Type { get; set; }
        public int SubType { get; set; }

        public float Duration { get; set; }
        public int ExpiryCalendarDay { get; set; }
        public int ExpiryTimeOfDay { get; set; }

        public uint? Creator { get; set; }
        public int SpellID { get; set; }
        public int Expose { get; set; }
        public int ShowIcon { get; set; }
        public int CasterLevel { get; set; }

        public Core.Effect LinkLeft { get; set; }
        public int LinkLeftValid { get; set; }
        public Core.Effect LinkRight { get; set; }
        public int LinkRightValid { get; set; }

        public int NumIntegers { get; set; }
        public int nParam0 { get; set; }
        public int nParam1 { get; set; }
        public int nParam2 { get; set; }
        public int nParam3 { get; set; }
        public int nParam4 { get; set; }
        public int nParam5 { get; set; }
        public int nParam6 { get; set; }
        public int nParam7 { get; set; }
        public float fParam0 { get; set; }
        public float fParam1 { get; set; }
        public float fParam2 { get; set; }
        public float fParam3 { get; set; }
        public string sParam0 { get; set; }
        public string sParam1 { get; set; }
        public string sParam2 { get; set; }
        public string sParam3 { get; set; }
        public string sParam4 { get; set; }
        public string sParam5 { get; set; }
        public uint? oParam0 { get; set; }
        public uint? oParam1 { get; set; }
        public uint? oParam2 { get; set; }
        public uint? oParam3 { get; set; }

        public string Tag { get; set; }
    }
}