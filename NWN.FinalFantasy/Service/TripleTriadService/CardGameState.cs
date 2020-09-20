using System.Collections.Generic;
using Pipelines.Sockets.Unofficial.Arenas;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public class CardGameState
    {
        public uint ArenaArea { get; set; }

        public bool HasInitialized { get; set; }
        public uint Player1 { get; set; }
        public uint Player2 { get; set; }
        public CardBoardPosition[,] Board { get; set; }
        public CardGamePlayer CurrentPlayerTurn { get; set; }
        public CardRuleType ActiveRule { get; set; }

        public Dictionary<int, CardHand> Player1Hand { get; set; }
        public Dictionary<int, CardHand> Player2Hand { get; set; }

        public CardSelection Player1Selection { get; set; }
        public CardSelection Player2Selection { get; set; }

        public int DisconnectionCheckCounter { get; set; }

        /// <summary>
        /// Calculates the points for each player.
        /// Cards in hands count for 1 point for their owner.
        /// Cards on the board count for 1 point for whoever has claimed them.
        /// </summary>
        /// <returns>The number of points for each player.</returns>
        public (int, int) CalculatePoints()
        {
            // Cards in hands count for one point each.
            var player1Points = Player1Hand.Count;
            var player2Points = Player2Hand.Count;

            // Iterate over the board and count up the number of cards owned by this player.
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    // Make sure there's a card in this position.
                    if (Board[x, y] != null)
                    {
                        var card = Board[x, y];

                        if (card.CurrentOwner == CardGamePlayer.Player1)
                        {
                            player1Points++;
                        }
                        else if (card.CurrentOwner == CardGamePlayer.Player2)
                        {
                            player2Points++;
                        }
                    }
                }
            }

            return (player1Points, player2Points);
        }

        public CardGameState(uint arenaArea, uint player1, uint player2)
        {
            ArenaArea = arenaArea;

            Player1 = player1;
            Player2 = player2;

            Board = new CardBoardPosition[3,3];
            Board[0, 0] = new CardBoardPosition();
            Board[0, 1] = new CardBoardPosition();
            Board[0, 2] = new CardBoardPosition();

            Board[1, 0] = new CardBoardPosition();
            Board[1, 1] = new CardBoardPosition();
            Board[1, 2] = new CardBoardPosition();

            Board[2, 0] = new CardBoardPosition();
            Board[2, 1] = new CardBoardPosition();
            Board[2, 2] = new CardBoardPosition();

            Player1Hand = new Dictionary<int, CardHand>
            {
                {1, new CardHand()},
                {2, new CardHand()},
                {3, new CardHand()},
                {4, new CardHand()},
                {5, new CardHand()},
            };
            Player2Hand = new Dictionary<int, CardHand>
            {
                {1, new CardHand()},
                {2, new CardHand()},
                {3, new CardHand()},
                {4, new CardHand()},
                {5, new CardHand()},
            };

            Player1Selection = new CardSelection();
            Player2Selection = new CardSelection();
        }
    }
}
