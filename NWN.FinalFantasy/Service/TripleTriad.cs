using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWNX.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Feature;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Location = NWN.FinalFantasy.Core.Location;
using Object = NWN.FinalFantasy.Core.NWNX.Object;
using Player = NWN.FinalFantasy.Core.NWNX.Player;

namespace NWN.FinalFantasy.Service
{
    public static class TripleTriad
    {
        // Default card graphic texture
        private const string DefaultCardTexture = "Card_Back";

        // Element texture
        private const string ElementTexture = "card_elem";

        // Determines the texture names used for each power slot
        private const string CardPowerPlayer1Top = "Card_Pwr_B_2";
        private const string CardPowerPlayer1Bottom = "Card_Pwr_B_3";
        private const string CardPowerPlayer1Left = "Card_Pwr_B_1";
        private const string CardPowerPlayer1Right = "Card_Pwr_B_4";

        private const string CardPowerPlayer2Top = "Card_Pwr_R_2";
        private const string CardPowerPlayer2Bottom = "Card_Pwr_R_3";
        private const string CardPowerPlayer2Left = "Card_Pwr_R_4";
        private const string CardPowerPlayer2Right = "Card_Pwr_R_1"; 

        // Texture used for the face-down cards on power textures
        private const string EmptyTexture = "Card_None";

        // Resref of the base arena area
        private const string ArenaResref = "tt_arena";

        // Resref of all card placeables
        private const string CardResref = "tt_card";

        // Determines how big to scale the cards placed on the board
        private const float BoardCardScale = 2.00f;

        // Determines how big to scale cards held in player hands
        private const float HandCardScale = 1.25f;

        // Resref of the selection shafts of light for each player
        private const string Player1SelectionResref = "plc_solblue";
        private const string Player2SelectionResref = "plc_solred";

        // Tracks all of the registered cards
        private static Dictionary<CardType, Card> AvailableCards { get; set; }

        // Tracks all of the available NPC deck choices
        private static Dictionary<int, CardDeck> NPCDeckTypes { get; set; } = CardNPCDecks.CreateNPCDecks();

        // Tracks all of the active game states
        private static Dictionary<string, CardGameState> GameStates { get; set; } = new Dictionary<string, CardGameState>();

        /// <summary>
        /// Static constructor initializes all of the card data.
        /// </summary>
        static TripleTriad()
        {
            AvailableCards = new Dictionary<CardType, Card>()
            {
                // Special card types used by the system
                {CardType.Invalid, new Card(string.Empty, "Card_None", -1, -1, -1, -1)},
                {CardType.FaceDown, new Card(string.Empty, "Card_Back", -1, -1, -1, -1)},

                // Level 1 Cards
                {CardType.Geezard, new Card("Geezard", "Card_P_Geezard", 1, 1, 5, 4)},
                {CardType.Funguar, new Card("Funguar", "Card_P_Funguar", 5, 1, 3, 1)},
                {CardType.BiteBug, new Card("Bite Bug", "Card_P_BiteBug", 1, 3, 5, 3)},
                {CardType.RedBat, new Card("Red Bat", "Card_P_RedBat", 6, 1, 2, 1)},
                {CardType.Blobra, new Card("Blobra", "Card_P_Blobra", 2, 1, 5, 3)},
                {CardType.Gayla, new Card("Gayla", "Card_P_Gayla", 2, 4, 4, 1, CardElementType.Lightning)},
                {CardType.Gesper, new Card("Gesper", "Card_P_Gesper", 1, 4, 1, 5)},
                {CardType.FastitocalonF, new Card("Fastitocalon-F", "Card_P_Fastitoca", 3, 2, 1, 5, CardElementType.Earth)},
                {CardType.BloodSoul, new Card("Blood Soul", "Card_P_BloodSoul", 2, 6, 1, 1)},
                {CardType.Caterchipillar, new Card("Caterchipillar", "Card_P_Caterchip", 4, 4, 3, 2)},
                {CardType.Cockatrice, new Card("Cockatrice", "Card_P_Cockatric", 2, 2, 6, 1)},

                // Level 2 Cards
                {CardType.Grat, new Card("Grat", "Card_P_Grat", 7, 3, 1, 1)},
                {CardType.Buel, new Card("Buel", "Card_P_Buel", 6, 2, 3, 2)},
                {CardType.Mesmerize, new Card("Mesmerize", "Card_P_Mesmerize", 5, 3, 4, 3)},
                {CardType.GlacialEye, new Card("Glacial Eye", "Card_P_GlacialE", 6, 4, 3, 1, CardElementType.Ice)},
                {CardType.Belhelmel, new Card("Belhelmel", "Card_P_Belhelmel", 3, 5, 3, 4)},
                {CardType.Thrustaevis, new Card("Thrustaevis", "Card_P_Thrustaev", 5, 2, 5, 3, CardElementType.Wind)},
                {CardType.Anacondaur, new Card("Anacondaur", "Card_P_Anacondau", 5, 3, 5, 1)},
                {CardType.Creeps, new Card("Creeps", "Card_P_Creeps", 5, 5, 2, 2)},
                {CardType.Grendel, new Card("Grendel", "Card_P_Grendel", 4, 5, 2, 4)},
                {CardType.Jelleye, new Card("Jelleye", "Card_P_Jelleye", 3, 1, 7, 2)},
                {CardType.GrandMantis, new Card("Grand Mantis", "Card_P_GrandMant", 5, 5, 3, 2)},

                // Level 3 Cards
                {CardType.Forbidden, new Card("Forbidden", "Card_P_Forbidden", 6, 3, 2, 6)},
                {CardType.Armadodo, new Card("Armadodo", "Card_P_Armadodo", 6, 1, 6, 3)},
                {CardType.TriFace, new Card("Tri-Face", "Card_P_Tri-Face", 3, 5, 5, 5)},
                {CardType.Fastitocalon, new Card("Fastitocalon", "Card_P_Fast1", 7, 1, 3, 5)},
                {CardType.SnowLion, new Card("Snow Lion", "Card_P_SnowLion", 7, 5, 3, 1)},
                {CardType.Ochu, new Card("Ochu", "Card_P_Ochu", 5, 3, 3, 6)},
                {CardType.Sam08G, new Card("SAM08G", "Card_P_SAM08G", 5, 2, 4, 6)},
                {CardType.DeathClaw, new Card("Death Claw", "Card_P_DeathClaw", 4, 7, 2, 4)},
                {CardType.Cactuar, new Card("Cactuar", "Card_P_Cactuar", 6, 3, 2, 6)},
                {CardType.Tonberry, new Card("Tonberry", "Card_P_Tonberry", 3, 4, 4, 6)},
                {CardType.AbyssWorm, new Card("Abyss Worm", "Card_P_AbyssWorm", 7, 3, 5, 2)},

                // Level 4 Cards
                {CardType.Turtapod, new Card("Turtapod", "Card_P_Turtapod", 2, 6, 7, 3)},
                {CardType.Vysage, new Card("Vysage", "Card_P_Vysage", 6, 4, 5, 5)},
                {CardType.TRexaur, new Card("T-Rexaur", "Card_P_TRexaur", 4, 2, 7, 6)},
                {CardType.Bomb, new Card("Bomb", "Card_P_Bomb", 2, 6, 3, 7)},
                {CardType.Blitz, new Card("Blitz", "Card_P_Blitz", 1, 4, 7, 6)},
                {CardType.Wendigo, new Card("Wendigo", "Card_P_Wendigo", 7, 1, 6, 3)},
                {CardType.Torama, new Card("Torama", "Card_P_Torama", 7, 4, 4, 4)},
                {CardType.Imp, new Card("Imp", "Card_P_Imp", 6, 3, 2, 6)},
                {CardType.BlueDragon, new Card("Blue Dragon", "Card_P_BlueDrago", 6, 7, 3, 2)},
                {CardType.Adamantoise, new Card("Adamantoise", "Card_P_Adamantoi", 4, 5, 6, 5)},
                {CardType.Hexadragon, new Card("Hexadragon", "Card_P_Hexadrago", 7, 4, 3, 5)},

                // Level 5 Cards
                {CardType.IronGiant, new Card("Iron Giant", "Card_P_IronGiant", 6, 6, 5, 5)},
                {CardType.Behemoth, new Card("Behemoth", "Card_P_Behemoth", 3, 5, 7, 6)},
                {CardType.Chimera, new Card("Chimera", "Card_P_Chimera", 6, 3, 2, 6)},
                {CardType.PuPu, new Card("PuPu", "Card_P_PuPu", 3, 2, 1, 10)},
                {CardType.Elastoid, new Card("Elastoid", "Card_P_Elastoid", 6, 6, 7, 2)},
                {CardType.GIM47N, new Card("GIM47N", "Card_P_GIM47N", 5, 7, 4, 5)},
                {CardType.Malboro, new Card("Malboro", "Card_P_Malboro", 7, 4, 2, 7, CardElementType.Poison)},
                {CardType.RubyDragon, new Card("Ruby Dragon", "Card_P_RubyDrago", 7, 7, 4, 2)},
                {CardType.Elnoyle, new Card("Elnoyle", "Card_P_Elnoyle", 5, 7, 6, 3)},
                {CardType.TonberryKing, new Card("Tonberry King", "Card_P_TonberryK", 4, 7, 4, 6)},
                {CardType.BiggsWedge, new Card("Biggs, Wedge", "Card_P_BiggsWedg", 6, 2, 7, 6)},

                // Level 6 Cards
                {CardType.FujinRaijin, new Card("Fujin, Raijin", "Card_P_FujinRaij", 2, 8, 4, 8)},
                {CardType.Elvoret, new Card("Elvoret", "Card_P_Elvoret", 7, 3, 4, 8)},
                {CardType.XATM092, new Card("X-ATM092", "Card_P_XATM092", 4, 7, 3, 8)},
                {CardType.Granaldo, new Card("Granaldo", "Card_P_Granaldo", 7, 8, 5, 2)},
                {CardType.Gerogero, new Card("Gerogero", "Card_P_Gerogero", 1, 8, 3, 8, CardElementType.Poison)},
                {CardType.Iguion, new Card("Iguion", "Card_P_Iguion", 8, 8, 2, 2)},
                {CardType.Abadon, new Card("Abadon", "Card_P_Abadon", 6, 4, 5, 8)},
                {CardType.Trauma, new Card("Trauma", "Card_P_Trauma", 4, 5, 6, 8)},
                {CardType.Oilboyle, new Card("Oilboyle", "Card_P_Oilboyle", 1, 4, 8, 8)},
                {CardType.ShumiTribe, new Card("Shumi Tribe", "Card_P_ShumiTrib", 6, 8, 4, 5)},
                {CardType.Krysta, new Card("Krysta", "Card_P_Krysta", 7, 8, 1, 5)},

                // Level 7 Cards

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
            SetEventScript(instance, EventScript.Area_OnHeartbeat, "tt_area_hb");
            
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

        /// <summary>
        /// Retrieves the game Id from an arena area.
        /// </summary>
        /// <param name="arenaArea">The arena area</param>
        /// <returns>A string representing the game state Id</returns>
        private static string GetGameId(uint arenaArea)
        {
            return GetLocalString(arenaArea, "TRIPLE_TRIAD_GAME_ID");
        }

        /// <summary>
        /// Retrieves the player and slot of a given placeable card.
        /// Only use this on card placeables in hands, not on the board.
        /// </summary>
        /// <param name="placeable">The hand card placeable</param>
        /// <returns>The card game player and the index of the card in the hand.</returns>
        private static (CardGamePlayer, int) GetCardHandDetails(uint placeable)
        {
            var playerNumber = GetLocalInt(placeable, "TRIPLE_TRIAD_CARD_OWNER");
            var cardGamePlayer = playerNumber == 1 ? CardGamePlayer.Player1 : CardGamePlayer.Player2;
            var handSlot = GetLocalInt(placeable, "TRIPLE_TRIAD_CARD_HAND_SLOT");

            return (cardGamePlayer, handSlot);
        }

        /// <summary>
        /// Starts a new Triple Triad game. At least one player must be a PC or the system won't function properly.
        /// </summary>
        /// <param name="player1">The first player</param>
        /// <param name="player1DeckId">The first player's deck Id.</param>
        /// <param name="player2">The second player</param>
        /// <param name="player2DeckId">The second player's deck Id.</param>
        public static void StartGame(
            uint player1, 
            int player1DeckId,
            uint player2,
            int player2DeckId)
        {
            var arenaArea = CreateArena();
            CardDeck player1Deck;
            CardDeck player2Deck;

            if (GetIsPC(player1))
            {
                var player1Id = GetObjectUUID(player1);
                var dbPlayer1TT = DB.Get<PlayerTripleTriad>(player1Id);
                player1Deck = dbPlayer1TT.Decks[player1DeckId];
            }
            else
            {
                player1Deck = NPCDeckTypes[player1DeckId];
            }

            if (GetIsPC(player2))
            {
                var player2Id = GetObjectUUID(player2);
                var dbPlayer2TT = DB.Get<PlayerTripleTriad>(player2Id);
                player2Deck = dbPlayer2TT.Decks[player2DeckId];
            }
            else
            {
                player2Deck = NPCDeckTypes[player2DeckId];
            }


            // Register the game state
            var state = new CardGameState(arenaArea, player1, player2);
            var gameId = Guid.NewGuid().ToString();
            GameStates[gameId] = state;
            SetLocalString(arenaArea, "TRIPLE_TRIAD_GAME_ID", gameId);

            // Register player 1's hand
            state.Player1Hand[1].Type = player1Deck.Card1;
            state.Player1Hand[2].Type = player1Deck.Card2;
            state.Player1Hand[3].Type = player1Deck.Card3;
            state.Player1Hand[4].Type = player1Deck.Card4;
            state.Player1Hand[5].Type = player1Deck.Card5;

            // Register player 2's hand
            state.Player2Hand[1].Type = player2Deck.Card1;
            state.Player2Hand[2].Type = player2Deck.Card2;
            state.Player2Hand[3].Type = player2Deck.Card3;
            state.Player2Hand[4].Type = player2Deck.Card4;
            state.Player2Hand[5].Type = player2Deck.Card5;

            if (GetIsPC(player1))
            {
                PersistentLocation.SaveLocation(player1);

                var playerStartLocation1 = GetLocalLocation(arenaArea, "PLAYER_1_START");
                AssignCommand(player1, () => ActionJumpToLocation(playerStartLocation1));
            }

            if (GetIsPC(player2))
            {
                PersistentLocation.SaveLocation(player1);

                var playerStartLocation2 = GetLocalLocation(arenaArea, "PLAYER_2_START");
                AssignCommand(player2, () => ActionJumpToLocation(playerStartLocation2));
            }
        }

        /// <summary>
        /// Handles the game's logic processing.
        /// This handles initialization of the game, handling idle players, and other checks which must happen.
        /// </summary>
        [NWNEventHandler("tt_area_hb")]
        public static void GameLogicProcessing()
        {
            var area = OBJECT_SELF;
            var gameId = GetGameId(area);

            // Only Triple Triad arena areas with active games should be processed.
            if (string.IsNullOrWhiteSpace(gameId)) return;

            var state = GameStates[gameId];

            // Game hasn't initialized yet.
            if (!state.HasInitialized)
            {
                InitializeGame(gameId);
            }
            else if(state.HasInitialized)
            {
                // Is game ending normally?
                if (state.IsGameEnding)
                {
                    state.GameEndingTicks++;
                    SendMessageToPC(state.Player1, "Game will be ending soon...");
                    SendMessageToPC(state.Player2, "Game will be ending soon...");

                    if (state.GameEndingTicks >= 3) // Roughly 18 seconds after ending, the game will clean up.
                    {
                        // Send the players back to their saved locations if they're still in the arena.
                        if (GetArea(state.Player1) == state.ArenaArea)
                        {
                            PersistentLocation.LoadLocation(state.Player1);
                        }

                        if (GetArea(state.Player2) == state.ArenaArea)
                        {
                            PersistentLocation.LoadLocation(state.Player2);
                        }

                        // Clear game state and area.
                        DelayCommand(30f, () =>
                        {
                            DestroyArea(state.ArenaArea);
                            GameStates.Remove(gameId);
                        });

                        state.IsGameEnding = false;
                    }
                }
                // Otherwise handle player disconnection logic.
                else
                {
                    // One or more player has left the area or disconnected.
                    if (GetArea(state.Player1) != state.ArenaArea ||
                        GetArea(state.Player2) != state.ArenaArea)
                    {
                        state.DisconnectionCheckCounter++;
                    }
                    // Both players are in the arena, reset the counter.
                    else
                    {
                        state.DisconnectionCheckCounter = 0;
                    }

                    // Counter has reached limit. End the game prematurely.
                    if (state.DisconnectionCheckCounter >= 10)
                    {
                        SendMessageToPC(state.Player1, "One or more players have left the Triple Triad arena. The game will end now.");
                        SendMessageToPC(state.Player2, "One or more players have left the Triple Triad arena. The game will end now.");

                        EndGame(gameId, true);
                    }

                }


            }
        }

        /// <summary>
        /// Initializes the game by spawning each player's hands 
        /// </summary>
        /// <param name="gameId">The game's state Id</param>
        private static void InitializeGame(string gameId)
        {
            var state = GameStates[gameId];

            // Both players must physically be in the arena to initialize.
            if (GetArea(state.Player1) != state.ArenaArea ||
                GetArea(state.Player2) != state.ArenaArea)
                return;

            // Spawn player 1's hand
            for (var index = 1; index <= 5; index++)
            {
                var cardType = state.Player1Hand[index].Type;
                var placeable = SpawnCard(gameId, $"PLAYER_1_HAND_{index}", cardType, HandCardScale);
                state.Player1Hand[index].Placeable = placeable;
                state.Player1Hand[index].Type = cardType;
                SetLocalInt(placeable, "TRIPLE_TRIAD_CARD_HAND_SLOT", index);
                SetLocalInt(placeable, "TRIPLE_TRIAD_CARD_OWNER", 1);
                SetEventScript(placeable, EventScript.Placeable_OnLeftClick, "tt_card_select");
                AssignCommand(placeable, () => ActionPlayAnimation(Animation.PlaceableDeactivate));

                // Disable usability on card only if player isn't playing themselves (debugging purposes)
                if (state.Player1 != state.Player2)
                {
                    Player.SetPlaceableUseable(state.Player2, placeable, false);
                }
            }

            // Spawn player 2's hand
            for (var index = 1; index <= 5; index++)
            {
                var cardType = state.Player2Hand[index].Type;
                var placeable = SpawnCard(gameId, $"PLAYER_2_HAND_{index}", cardType, HandCardScale);
                state.Player2Hand[index].Placeable = placeable;
                state.Player2Hand[index].Type = cardType;
                SetLocalInt(placeable, "TRIPLE_TRIAD_CARD_HAND_SLOT", index);
                SetLocalInt(placeable, "TRIPLE_TRIAD_CARD_OWNER", 2);
                SetEventScript(placeable, EventScript.Placeable_OnLeftClick, "tt_card_select");
                AssignCommand(placeable, () => ActionPlayAnimation(Animation.PlaceableActivate));

                // Disable usability on card only if player isn't playing themselves (debugging purposes)
                if (state.Player1 != state.Player2)
                {
                    Player.SetPlaceableUseable(state.Player1, placeable, false);
                }
            }

            // Spawn blank cards on the board
            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    var locationId = $"BOARD_{x}_{y}";
                    var placeable = SpawnCard(gameId, locationId, CardType.Invalid);
                    SetName(placeable, $"({x}, {y})");
                    SetEventScript(placeable, EventScript.Placeable_OnLeftClick, "tt_card_place");
                    SetLocalInt(placeable, "TRIPLE_TRIAD_X", x);
                    SetLocalInt(placeable, "TRIPLE_TRIAD_Y", y);

                    state.Board[x, y].Placeable = placeable;
                    ReplaceObjectTexture(placeable, "Card_Bkg_Red", EmptyTexture);
                    ReplaceObjectTexture(placeable, "Card_Board", EmptyTexture);
                }
            }

            if (Random.D100(1) <= 50)
            {
                state.CurrentPlayerTurn = CardGamePlayer.Player1;
                SendMessageToPC(state.Player1, "Player 1 won the coin toss and goes first.");
                SendMessageToPC(state.Player2, "Player 1 won the coin toss and goes first.");
            }
            else
            {
                state.CurrentPlayerTurn = CardGamePlayer.Player2;
                SendMessageToPC(state.Player1, "Player 2 won the coin toss and goes first.");
                SendMessageToPC(state.Player2, "Player 2 won the coin toss and goes first.");
            }


            state.HasInitialized = true;
        }

        private static void ChangeTurn(string gameId, CardGamePlayer playerTurn)
        {
            var state = GameStates[gameId];

            state.CurrentPlayerTurn = playerTurn;

            if (playerTurn == CardGamePlayer.Player1)
            {
                var message = $"It is {GetName(state.Player1)}'s turn.";
                SendMessageToPC(state.Player1, message);
                SendMessageToPC(state.Player2, message);
            }
            else
            {
                var message = $"It is {GetName(state.Player2)}'s turn.";
                SendMessageToPC(state.Player1, message);
                SendMessageToPC(state.Player2, message);
            }
        }

        /// <summary>
        /// Spawns a card at a specific location Id.
        /// </summary>
        /// <param name="gameId">The game's state Id</param>
        /// <param name="locationId">The location Id</param>
        /// <param name="cardType">The type of card to spawn</param>
        /// <param name="scale">Scale of the card placeable</param>
        /// <returns>The placeable spawned.</returns>
        private static uint SpawnCard(string gameId, string locationId, CardType cardType, float scale = BoardCardScale)
        {
            var state = GameStates[gameId];
            var area = state.ArenaArea;
            var card = AvailableCards[cardType];

            var location = GetLocalLocation(area, locationId);
            var placeable = CreateObject(ObjectType.Placeable, CardResref, location);
            SetName(placeable, card.Name);

            // Set the card graphic
            ReplaceObjectTexture(placeable, DefaultCardTexture, card.Texture);

            // Set power levels
            ReplaceObjectTexture(placeable, CardPowerPlayer1Right, GetPowerTexture(card.RightPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer1Top, GetPowerTexture(card.TopPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer1Bottom, GetPowerTexture(card.BottomPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer1Left, GetPowerTexture(card.LeftPower));

            ReplaceObjectTexture(placeable, CardPowerPlayer2Right, GetPowerTexture(card.RightPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer2Top, GetPowerTexture(card.TopPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer2Bottom, GetPowerTexture(card.BottomPower));
            ReplaceObjectTexture(placeable, CardPowerPlayer2Left, GetPowerTexture(card.LeftPower));

            // Set the element texture
            ReplaceObjectTexture(placeable, ElementTexture, GetElementTexture(card.Element));

            // Scale the card to fit the board
            SetObjectVisualTransform(placeable, ObjectVisualTransform.Scale, scale);

            return placeable;
        }

        /// <summary>
        /// Retrieves the texture used for a specific power level.
        /// Returns an empty texture if value is outside range of 0-10
        /// </summary>
        /// <param name="power">The power level</param>
        /// <returns>A texture with a matching power level.</returns>
        private static string GetPowerTexture(int power)
        {
            if (power >= 0 && power <= 10)
            {
                return $"Card_{power}";
            }

            return EmptyTexture;
        }

        private static void FlipBoardCard(string gameId, uint placeable)
        {
            var state = GameStates[gameId];
            var x = GetLocalInt(placeable, "TRIPLE_TRIAD_X");
            var y = GetLocalInt(placeable, "TRIPLE_TRIAD_Y");
            var boardCard = state.Board[x, y];

            if (boardCard.CurrentOwner == CardGamePlayer.Player1)
            {
                AssignCommand(boardCard.Placeable, () => ActionPlayAnimation(Animation.PlaceableDeactivate));
            }
            else if (boardCard.CurrentOwner == CardGamePlayer.Player2)
            {
                AssignCommand(boardCard.Placeable, () => ActionPlayAnimation(Animation.PlaceableActivate));
            }
        }

        private static string GetElementTexture(CardElementType elementType)
        {
            switch (elementType)
            {
                // todo: make these textures
                case CardElementType.Earth:
                    break;
                case CardElementType.Fire:
                    break;
                case CardElementType.Water:
                    break;
                case CardElementType.Poison:
                    break;
                case CardElementType.Holy:
                    break;
                case CardElementType.Lightning:
                    break;
                case CardElementType.Wind:
                    break;
                case CardElementType.Ice:
                    break;
            }

            return EmptyTexture;
        }

        /// <summary>
        /// When a card is clicked, select the card.
        /// </summary>
        [NWNEventHandler("tt_card_select")]
        public static void ClickCard()
        {
            var player = GetPlaceableLastClickedBy();
            var placeable = OBJECT_SELF;
            var area = GetArea(placeable);
            var (owner, cardHandId) = GetCardHandDetails(placeable);
            var gameId = GetGameId(area);
            var state = GameStates[gameId];

            AssignCommand(player, () => ClearAllActions());

            if (player != state.Player1 && owner == CardGamePlayer.Player1 ||
                player != state.Player2 && owner == CardGamePlayer.Player2)
            {
                SendMessageToPC(player, "You do not own that card.");
                return;
            }

            if (owner == CardGamePlayer.Player1)
            {
                if (state.Player1Selection.Placeable != null)
                {
                    Object.SetPosition((uint)state.Player1Selection.Placeable, GetPosition(placeable));
                }
                else
                {
                    state.Player1Selection.Placeable = CreateObject(ObjectType.Placeable, Player1SelectionResref, GetLocation(placeable));

                    // Do this check just in case the player is playing against himself.
                    if (state.Player1 != state.Player2)
                    {
                        Visibility.SetVisibilityOverride(state.Player1, placeable, VisibilityType.Visible);
                        Visibility.SetVisibilityOverride(state.Player2, placeable, VisibilityType.Hidden);
                    }
                }

                state.Player1Selection.CardHandId = cardHandId;
            }
            else if (owner == CardGamePlayer.Player2)
            {
                if (state.Player2Selection.Placeable != null)
                {
                    Object.SetPosition((uint) state.Player2Selection.Placeable, GetPosition(placeable));
                }
                else
                {
                    state.Player2Selection.Placeable = CreateObject(ObjectType.Placeable, Player2SelectionResref, GetLocation(placeable));

                    // Do this check just in case the player is playing against himself.
                    if (state.Player1 != state.Player2)
                    {
                        Visibility.SetVisibilityOverride(state.Player1, placeable, VisibilityType.Hidden);
                        Visibility.SetVisibilityOverride(state.Player2, placeable, VisibilityType.Visible);
                    }
                }

                state.Player2Selection.CardHandId = cardHandId;
            }
        }

        /// <summary>
        /// When a player clicks on a card on the board, attempt to place the selected card.
        /// </summary>
        [NWNEventHandler("tt_card_place")]
        public static void PlaceCard()
        {
            var player = GetPlaceableLastClickedBy();
            var placeable = OBJECT_SELF;
            var area = GetArea(placeable);
            var gameId = GetGameId(area);
            var state = GameStates[gameId];
            var x = GetLocalInt(placeable, "TRIPLE_TRIAD_X");
            var y = GetLocalInt(placeable, "TRIPLE_TRIAD_Y");

            AssignCommand(player, () => ClearAllActions());

            // Is it this the player's turn?
            if (state.CurrentPlayerTurn == CardGamePlayer.Player1 &&
                state.Player1 != player)
            {
                SendMessageToPC(player, "It is currently player 2's turn. Please wait your turn.");
                return;
            }
            else if (state.CurrentPlayerTurn == CardGamePlayer.Player2 &&
                     state.Player2 != player)
            {
                SendMessageToPC(player, "It is currently player 1's turn. Please wait your turn.");
                return;
            }

            var selection = state.CurrentPlayerTurn == CardGamePlayer.Player1
                ? state.Player1Selection
                : state.Player2Selection;

            // Player hasn't selected a card yet.
            if (selection.Placeable == null)
            {
                SendMessageToPC(player, "Select a card from your hand first.");
                return;
            }

            // Sanity check to make sure the player hasn't picked a location that already has a card.
            if (state.Board[x, y].CardType != CardType.Invalid)
            {
                return;
            }

            // We've got a valid location. Update the selected card in this position, remove the card from the player's hand, and run game rules on this change.
            var hand = state.CurrentPlayerTurn == CardGamePlayer.Player1 
                ? state.Player1Hand 
                : state.Player2Hand;

            // Remove the card from the player's hand
            var handCard = hand[selection.CardHandId];
            if (handCard.Placeable != null)
                DestroyObject((uint) handCard.Placeable);

            // Destroy the selection placeable
            if (selection.Placeable != null)
            {
                DestroyObject((uint) selection.Placeable);
                selection.Placeable = null;
            }

            // Destroy the blank card on the board
            DestroyObject(placeable);

            // Spawn a new card onto the board in the same position.
            placeable = SpawnCard(gameId, $"BOARD_{x}_{y}", handCard.Type);
            SetUseableFlag(placeable, false);
            SetLocalInt(placeable, "TRIPLE_TRIAD_X", x);
            SetLocalInt(placeable, "TRIPLE_TRIAD_Y", y);

            // Update the board state
            state.Board[x, y].Placeable = placeable;
            state.Board[x, y].CardType = handCard.Type;
            state.Board[x, y].CurrentOwner = state.CurrentPlayerTurn;

            // Handle flipping the card to match the owner.
            FlipBoardCard(gameId, placeable);

            // Remove the card from the player's hand.
            hand.Remove(selection.CardHandId);

            // Change turns
            var nextTurn = state.CurrentPlayerTurn == CardGamePlayer.Player1
                ? CardGamePlayer.Player2
                : CardGamePlayer.Player1;

            // Handle card fighting
            CardFight(gameId, x, y);

            // Have all cards been placed?
            if (CheckForEndCondition(gameId))
            {
                EndGame(gameId, false);
            }
            // Game is still going, change turns.
            else
            {
                ChangeTurn(gameId, nextTurn);
            }
        }

        /// <summary>
        /// Handles fighting cards in adjacent positions.
        /// </summary>
        /// <param name="gameId">The game Id</param>
        /// <param name="x">The X position of the card that was just placed</param>
        /// <param name="y">The Y position of the card that was just placed</param>
        private static void CardFight(string gameId, int x, int y)
        {
            int DeterminePower(CardType cardType, CardDirection direction)
            {
                var card = AvailableCards[cardType];

                switch (direction)
                {
                    case CardDirection.Top:
                        return card.TopPower;
                    case CardDirection.Bottom:
                        return card.BottomPower;
                    case CardDirection.Left:
                        return card.LeftPower;
                    case CardDirection.Right:
                        return card.RightPower;
                }

                return 0;
            }

            void DoFight(
                CardBoardPosition attacker, 
                CardBoardPosition defender, 
                CardDirection attackerDirection, 
                CardDirection defenderDirection)
            {
                // No defender card. Skip out of this as there's nothing to fight.
                if (defender.CardType == CardType.Invalid) return;
                // Same owner, no sense running the logic.
                if (attacker.CurrentOwner == defender.CurrentOwner) return;

                var attackerPower = DeterminePower(attacker.CardType, attackerDirection);
                var defenderPower = DeterminePower(defender.CardType, defenderDirection);

                if (attackerPower > defenderPower)
                {
                    Console.WriteLine($"Attacker ({attackerDirection}) / {attackerPower} vs Defender ({defenderDirection}) / {defenderPower}"); // todo debug

                    defender.CurrentOwner = attacker.CurrentOwner;
                }

                // todo: "Same" rule would go here. If same power, attacker gets control

                // todo: "Combo" rule would go here.


                FlipBoardCard(gameId, attacker.Placeable);
                FlipBoardCard(gameId, defender.Placeable);
            }

            var state = GameStates[gameId];
            var boardCard = state.Board[x, y];

            // Row 1
            if (x == 0 && y == 0)
            {
                DoFight(boardCard, state.Board[0, 1], CardDirection.Right, CardDirection.Left);
                DoFight(boardCard, state.Board[1, 0], CardDirection.Bottom, CardDirection.Top);
            }
            else if (x == 0 && y == 1)
            {
                DoFight(boardCard, state.Board[0, 0], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[0, 2], CardDirection.Right, CardDirection.Left);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Bottom, CardDirection.Top);
            }
            else if (x == 0 && y == 2)
            {
                DoFight(boardCard, state.Board[0, 1], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[1, 2], CardDirection.Bottom, CardDirection.Top);
            }

            // Row 2
            else if (x == 1 && y == 0)
            {
                DoFight(boardCard, state.Board[0, 0], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Right, CardDirection.Left);
                DoFight(boardCard, state.Board[2, 0], CardDirection.Bottom, CardDirection.Top);
            }
            else if (x == 1 && y == 1)
            {
                DoFight(boardCard, state.Board[0, 1], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[1, 0], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[2, 1], CardDirection.Bottom, CardDirection.Top);
                DoFight(boardCard, state.Board[1, 2], CardDirection.Right, CardDirection.Left);
            }
            else if (x == 1 && y == 2)
            {
                DoFight(boardCard, state.Board[0, 2], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[2, 2], CardDirection.Bottom, CardDirection.Top);
            }

            // Row 3
            else if (x == 2 && y == 0)
            {
                DoFight(boardCard, state.Board[1, 0], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[2, 1], CardDirection.Right, CardDirection.Left);
            }
            else if (x == 2 && y == 1)
            {
                DoFight(boardCard, state.Board[2, 0], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[1, 1], CardDirection.Top, CardDirection.Bottom);
                DoFight(boardCard, state.Board[2, 2], CardDirection.Right, CardDirection.Left);
            }
            else if (x == 2 && y == 2)
            {
                DoFight(boardCard, state.Board[2, 1], CardDirection.Left, CardDirection.Right);
                DoFight(boardCard, state.Board[1, 2], CardDirection.Top, CardDirection.Bottom);
            }
        }

        /// <summary>
        /// Checks to see if there's a player-placed card in every position on the board.
        /// If there is, the game is presumed to be finished.
        /// </summary>
        /// <param name="gameId">The game state Id</param>
        /// <returns>true if the game is finished, false otherwise</returns>
        private static bool CheckForEndCondition(string gameId)
        {
            var state = GameStates[gameId];

            // Check every position in the board for a non-invalid card.
            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 2; y++)
                {
                    if (state.Board[x, y].CardType == CardType.Invalid ||
                        state.Board[x, y].CardType == CardType.FaceDown)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Ends the game. Player stats are recorded and all players are returned to their last saved position.
        /// </summary>
        /// <param name="gameId">The game Id</param>
        /// <param name="endedPrematurely">If true, a player disconnected or otherwise left the arena. No rewards will be given to either player in this situation.</param>
        private static void EndGame(string gameId, bool endedPrematurely)
        {
            void GrantRewards(uint player, bool? isWinner, bool opponentWasPlayer)
            {
                // No need to reward NPCs or DMs.
                if (!GetIsPC(player) || GetIsDM(player)) return;
                var playerId = GetObjectUUID(player);
                var dbTripleTriadPlayer = DB.Get<PlayerTripleTriad>(playerId) ?? new PlayerTripleTriad();

                // Won
                if (isWinner == true)
                {
                    if (opponentWasPlayer)
                    {
                        dbTripleTriadPlayer.WinsAgainstPlayers++;
                    }
                    else
                    {
                        dbTripleTriadPlayer.WinsAgainstNPCs++;
                    }

                    dbTripleTriadPlayer.TriadPoints += 2;
                    SendMessageToPC(player, $"Triad Points: {dbTripleTriadPlayer.TriadPoints} (+2)");
                }
                // Lost
                else if (isWinner == false)
                {
                    if (opponentWasPlayer)
                    {
                        dbTripleTriadPlayer.LossesAgainstPlayers++;
                    }
                    else
                    {
                        dbTripleTriadPlayer.LossesAgainstNPCs++;
                    }

                    dbTripleTriadPlayer.TriadPoints++;
                    SendMessageToPC(player, $"Triad Points: {dbTripleTriadPlayer.TriadPoints} (+1)");
                }
                // Draw
                else
                {
                    if (opponentWasPlayer)
                    {
                        dbTripleTriadPlayer.DrawsAgainstPlayers++;
                    }
                    else
                    {
                        dbTripleTriadPlayer.DrawsAgainstNPCs++;
                    }

                    // No Triad Points granted
                    SendMessageToPC(player, $"Triad Points: {dbTripleTriadPlayer.TriadPoints} (+0)");
                }

                DB.Set(playerId, dbTripleTriadPlayer);
            }

            var state = GameStates[gameId];
            var (player1Points, player2Points) = state.CalculatePoints();

            // Player 1 wins
            if (player1Points > player2Points)
            {
                var message = $"{GetName(state.Player1)} has won the game!";
                SendMessageToPC(state.Player1, message);
                SendMessageToPC(state.Player2, message);

                if (!endedPrematurely)
                {
                    GrantRewards(state.Player1, true, GetIsPC(state.Player2));
                    GrantRewards(state.Player2, false, GetIsPC(state.Player1));
                }
            }
            // Player 2 wins
            else if (player2Points > player1Points)
            {
                var message = $"{GetName(state.Player2)} has won the game!";
                SendMessageToPC(state.Player1, message);
                SendMessageToPC(state.Player2, message);

                if (!endedPrematurely)
                {
                    GrantRewards(state.Player1, false, GetIsPC(state.Player2));
                    GrantRewards(state.Player2, true, GetIsPC(state.Player1));
                }
            }
            // Draw
            else
            {
                var message = $"The game ended in a draw!";
                SendMessageToPC(state.Player1, message);
                SendMessageToPC(state.Player2, message);

                if (!endedPrematurely)
                {
                    GrantRewards(state.Player1, null, GetIsPC(state.Player2));
                    GrantRewards(state.Player2, null, GetIsPC(state.Player1));
                }
            }

            // Mark the game as ending. This flag will be picked up in the heartbeat processor.
            // We don't want to immediately end the game as it will create a jarring experience for the player.
            // So instead, we leave the game active for about 18 seconds before cleaning up.
            state.IsGameEnding = true;
        }
    }
}
