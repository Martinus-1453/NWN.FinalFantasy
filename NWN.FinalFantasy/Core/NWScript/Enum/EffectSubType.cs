using System;

namespace NWN.FinalFantasy.Core.NWScript.Enum
{
    [Flags]
    public enum EffectSubType
    {
        Magical = 8,
        Supernatural = 16,
        Extraordinary = 24,
        Mask = 0x18
    }
}