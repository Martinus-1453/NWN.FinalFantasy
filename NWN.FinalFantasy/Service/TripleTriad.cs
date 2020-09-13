using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service.TripleTriadService;
using Pipelines.Sockets.Unofficial.Arenas;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Location = NWN.FinalFantasy.Core.Location;

namespace NWN.FinalFantasy.Service
{
    public static class TripleTriad
    {
        private const string DefaultCardTexture = "CardBack";
        private const string ArenaResref = "tt_arena";
        private const string CardResref = "tt_card";

        private static Dictionary<CardType, Card> _availableCards;

        /// <summary>
        /// Static constructor initializes all of the card data.
        /// </summary>
        static TripleTriad()
        {
            _availableCards = new Dictionary<CardType, Card>()
            {
                {CardType.Geezard, new Card("Geezard", "Card_Geezard", 1, 1, 5, 4)},
                {CardType.Funguar, new Card("Funguar", "Card_Funguar", 5, 1, 3, 1)},
                {CardType.BiteBug, new Card("Bite Bug", "Card_BiteBug", 1, 3, 5, 3)},
                {CardType.RedBat, new Card("Red Bat", "Card_RedBat", 6, 1, 2, 1)},
                {CardType.Blobra, new Card("Blobra", "Card_Blobra", 2, 1, 5, 3)},
            };
        }

        /// <summary>
        /// Creates an arena instance and stores all necessary locations on the instance.
        /// </summary>
        /// <returns>The instanced arena area.</returns>
        private static uint CreateArena()
        {
            Location GetInstanceLocation(uint instanceArea, Location originalLocation)
            {
                var position = GetPositionFromLocation(originalLocation);
                var facing = GetFacingFromLocation(originalLocation);
                var instanceLocation = Location(instanceArea, position, facing);

                return instanceLocation;
            }

            // Copy the original arena area.
            var original = Cache.GetAreaByResref(ArenaResref);
            var instance = CopyArea(original);
            
            // Store player start locations
            SetLocalLocation(instance, "PLAYER_1_START", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_START"))));
            SetLocalLocation(instance, "PLAYER_2_START", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_START"))));

            // Store player 1 hand locations
            SetLocalLocation(instance, "PLAYER_1_HAND_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_1"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_2"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_3", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_3"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_4", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_4"))));
            SetLocalLocation(instance, "PLAYER_1_HAND_5", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P1_CARD_5"))));

            // Store player 2 hand locations
            SetLocalLocation(instance, "PLAYER_2_HAND_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_1"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_2"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_3", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_3"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_4", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_4"))));
            SetLocalLocation(instance, "PLAYER_2_HAND_5", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_P2_CARD_5"))));

            // Store board locations
            SetLocalLocation(instance, "BOARD_0_0", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_0_0"))));
            SetLocalLocation(instance, "BOARD_0_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_0_1"))));
            SetLocalLocation(instance, "BOARD_0_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_0_2"))));
            SetLocalLocation(instance, "BOARD_1_0", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_1_0"))));
            SetLocalLocation(instance, "BOARD_1_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_1_1"))));
            SetLocalLocation(instance, "BOARD_1_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_1_2"))));
            SetLocalLocation(instance, "BOARD_2_0", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_2_0"))));
            SetLocalLocation(instance, "BOARD_2_1", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_2_1"))));
            SetLocalLocation(instance, "BOARD_2_2", GetInstanceLocation(instance, GetLocation(GetWaypointByTag("TT_BOARD_2_2"))));

            return instance;
        }


        public static void SimulateStart(uint player)
        {
            var area = CreateArena();

            var startLocation = GetLocalLocation(area, "PLAYER_1_START");
            AssignCommand(player, () => ActionJumpToLocation(startLocation));

            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    var location = GetLocalLocation(area, $"BOARD_{x}_{y}");
                    var placeable = CreateObject(ObjectType.Placeable, CardResref, location);
                    var randomId = Random.Next(1, 5);

                    var card = _availableCards[(CardType)randomId];
                    SetName(placeable, card.Name);
                    ReplaceObjectTexture(placeable, DefaultCardTexture, card.Texture);
                    SetObjectVisualTransform(placeable, ObjectVisualTransform.Scale, 2.75f);
                }
            }

        }

    }
}
