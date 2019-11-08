﻿using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Module;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class mod_on_death
    {
        internal static void Main()
        {
            Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerDeath);
        }
    }
}