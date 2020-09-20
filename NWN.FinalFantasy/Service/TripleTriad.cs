using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWNX.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Location = NWN.FinalFantasy.Core.Location;
using Object = NWN.FinalFantasy.Core.NWNX.Object;

namespace NWN.FinalFantasy.Service
{
    public static class TripleTriad
    {
        // Default card graphic texture
        private const string DefaultCardTexture = "Card_Back";

        // Element texture
        private const string ElementTexture = "card_elem";

        // Determines the texture names used for each power slot
        private const string CardPowerRight = "Card_Pwr_Slot_1";
        private const string CardPowerTop = "Card_Pwr_Slot_2";
        private const string CardPowerBottom = "Card_Pwr_Slot_3";
        private const string CardPowerLeft = "Card_Pwr_Slot_4"; 

        // Texture used for the face-down cards on power textures
        private const string EmptyTexture = "Card_None";

        // Texture names of all number textures
        private const string Power0Texture = "Card_0";
        private const string Power1Texture = "Card_1";
        private const string Power2Texture = "Card_2";
        private const string Power3Texture = "Card_3";
        private const string Power4Texture = "Card_4";
        private const string Power5Texture = "Card_5";
        private const string Power6Texture = "Card_6";
        private const string Power7Texture = "Card_7";
        private const string Power8Texture = "Card_8";
        private const string Power9Texture = "Card_9";
        private const string PowerATexture = "Card_10";

        // Background textures
        private const string BackgroundBlueTexture = "card_bkg_blue";
        private const string BackgroundRedTexture = "card_bkg_red";

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

                // Player card options
                {CardType.Geezard, new Card("Geezard", "Card_P_Geezard", 1, 1, 5, 4)},
                {CardType.Funguar, new Card("Funguar", "Card_P_Funguar", 5, 1, 3, 1)},
                {CardType.BiteBug, new Card("Bite Bug", "Card_P_BiteBug", 1, 3, 5, 3)},
                {CardType.RedBat, new Card("Red Bat", "Card_P_RedBat", 6, 1, 2, 1)},
                {CardType.Blobra, new Card("Blobra", "Card_P_Blobra", 2, 1, 5, 3)},
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
                var playerStartLocation1 = GetLocalLocation(arenaArea, "PLAYER_1_START");
                AssignCommand(player1, () => ActionJumpToLocation(playerStartLocation1));
            }

            if (GetIsPC(player2))
            {
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

                ReplaceObjectTexture(placeable, "Card_Bkg_Red", BackgroundBlueTexture);
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

                ReplaceObjectTexture(placeable, "Card_Bkg_Red", BackgroundRedTexture);
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
                    ReplaceObjectTexture(placeable, "Card_Bkg_Red", EmptyTexture);
                    SetLocalInt(placeable, "TRIPLE_TRIAD_X", x);
                    SetLocalInt(placeable, "TRIPLE_TRIAD_Y", y);

                    state.Board[x, y].Placeable = placeable;
                }
            }

            state.HasInitialized = true;
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
            ReplaceObjectTexture(placeable, CardPowerRight, GetPowerTexture(card.RightPower));
            ReplaceObjectTexture(placeable, CardPowerTop, GetPowerTexture(card.TopPower));
            ReplaceObjectTexture(placeable, CardPowerBottom, GetPowerTexture(card.BottomPower));
            ReplaceObjectTexture(placeable, CardPowerLeft, GetPowerTexture(card.LeftPower));

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
            switch (power)
            {
                case 0:
                    return Power0Texture;
                case 1:
                    return Power1Texture;
                case 2:
                    return Power2Texture;
                case 3:
                    return Power3Texture;
                case 4:
                    return Power4Texture;
                case 5:
                    return Power5Texture;
                case 6:
                    return Power6Texture;
                case 7:
                    return Power7Texture;
                case 8:
                    return Power8Texture;
                case 9:
                    return Power9Texture;
                case 10:
                    return PowerATexture;
            }

            return EmptyTexture;
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

            var handCard = hand[selection.CardHandId];

            if (handCard.Placeable != null)
                DestroyObject((uint) handCard.Placeable);

            if (selection.Placeable != null)
                DestroyObject((uint) selection.Placeable);

            DestroyObject(placeable);

            selection.Placeable = null;

            placeable = SpawnCard(gameId, $"BOARD_{x}_{y}", handCard.Type);
            SetUseableFlag(placeable, false);

        }

    }
}
