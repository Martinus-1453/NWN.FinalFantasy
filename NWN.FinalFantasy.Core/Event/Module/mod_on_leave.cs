﻿using NWN.FinalFantasy.Core.Event;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class mod_on_leave
    {
        public static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, "ON_CLIENT_EXIT_");
        }
    }
}