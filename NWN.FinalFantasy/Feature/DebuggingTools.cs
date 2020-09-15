﻿using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Feature.DialogDefinition;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Dialog = NWN.FinalFantasy.Service.Dialog;
using Skill = NWN.FinalFantasy.Service.Skill;

namespace NWN.FinalFantasy.Feature
{
    public static class DebuggingTools
    {
        [NWNEventHandler("test1")]
        public static void DebugGiveQuest()
        {
            var player = GetLastUsedBy();
            Quest.AcceptQuest(player, "testQuest");
        }

        [NWNEventHandler("test2")]
        public static void DebugSpawnCreature()
        {
            var location = GetLocation(GetWaypointByTag("DEATH_DEFAULT_RESPAWN_POINT"));
            var spawn = CreateObject(ObjectType.Creature, "test_zombie", location);

            SetLocalInt(spawn, "QUEST_NPC_GROUP_ID", 1);
        }

        [NWNEventHandler("test4")]
        public static void DebugGiveXP()
        {
            var player = GetLastUsedBy();
            Skill.GiveSkillXP(player, SkillType.Longsword, 5000);
        }

        [NWNEventHandler("test6")]
        public static void IncreaseEnmityOnBoy()
        {
            var player = GetLastUsedBy();
            var boy = GetObjectByTag("ENMITY_TARGET");
            var lastAttacker = GetLastAttacker(player);

            Enmity.ModifyEnmity(boy, lastAttacker, 999);
        }

        [NWNEventHandler("test7")]
        public static void GiveEffect()
        {
            var player = GetLastUsedBy();
            StatusEffect.Apply(player, player, StatusEffectType.Invincible, 30.0f);
        }

        [NWNEventHandler("test8")]
        public static void MakeIP()
        {
            Console.WriteLine("firing");

            var itemprop = ItemPropertyAttackBonus(1);

            Console.WriteLine("Unpacking");
            var unpacked = Core.NWNX.ItemProperty.UnpackIP(itemprop);

            Console.WriteLine("Packing");
            var packed = Core.NWNX.ItemProperty.PackIP(unpacked);

            Console.WriteLine("Done");
        }

        [NWNEventHandler("test9")]
        public static void OpenHomePurchaseMenu()
        {
            var player = GetLastUsedBy();

            Creature.AddFeatByLevel(player, Feat.PropertyTool, 1);

            Dialog.StartConversation(player, OBJECT_SELF, nameof(PlayerHouseDialog));
        }

        [NWNEventHandler("test10")]
        public static void SpawnGold()
        {
            var player = GetLastUsedBy();
            GiveGoldToCreature(player, 5000);
        }

        [NWNEventHandler("test11")]
        public static void SimulateTripleTriad()
        {
            var player = GetLastUsedBy();
            TripleTriad.SimulateStart(player);
        }

        [NWNEventHandler("test12")]
        public static void UseCard()
        {
            var card = OBJECT_SELF;

            var val = GetLocalBool(card, "USED");

            if (val)
            {
                AssignCommand(card, () => ClearAllActions());
                AssignCommand(card, () => ActionPlayAnimation(Animation.PlaceableDeactivate, 1f, 99999f));
            }
            else
            {
                AssignCommand(card, () => ClearAllActions());
                AssignCommand(card, () => ActionPlayAnimation(Animation.PlaceableActivate, 1f, 99999f));
            }
        }

    }
}
