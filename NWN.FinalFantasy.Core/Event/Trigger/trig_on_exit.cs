﻿using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Trigger;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class trig_on_exit
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnExit);
        }
    }
}