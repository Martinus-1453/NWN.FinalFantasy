using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public class CardGameState
    {
        public uint Player1 { get; set; }
        public uint Player2 { get; set; }
        public Card[,] Board { get; set; }
        public CardGamePlayer CurrentPlayerTurn { get; set; }
        public CardRuleType ActiveRule { get; set; }

        public List<Card> Player1Hand { get; set; }
        public List<Card> Player2Hand { get; set; }

        public int CalculatePoints(CardGamePlayer player)
        {
            if (player == CardGamePlayer.Invalid) return 0;

            var hand = player == CardGamePlayer.Player1 ? Player1Hand : Player2Hand;
            
            // Cards in hands count for one point each.
            var points = hand.Count;

            // Iterate over the board and count up the number of cards owned by this player.
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    // Make sure there's a card in this position.
                    if (Board[x, y] != null)
                    {
                        var card = Board[x, y];

                        if (card.CurrentOwner == player)
                        {
                            points++;
                        }
                    }
                }
            }

            return points;
        }

        public CardGameState(uint player1, uint player2)
        {
            Player1 = player1;
            Player2 = player2;

            Board = new Card[3,3];
            Player1Hand = new List<Card>();
            Player2Hand = new List<Card>();
        }
    }
}
