using System;
using System.Collections.Generic;
using System.Text;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public static class CardNPCDecks
    {
        public static Dictionary<int, CardDeck> CreateNPCDecks()
        {
            var npcDecks = new Dictionary<int, CardDeck>();

            npcDecks.Add(1, new CardDeck
            {
                Name = "Test Deck",
                Card1 = CardType.Geezard,
                Card2 = CardType.Funguar,
                Card3 = CardType.BiteBug,
                Card4 = CardType.RedBat,
                Card5 = CardType.Blobra
            });


            return npcDecks;
        }
    }
}
