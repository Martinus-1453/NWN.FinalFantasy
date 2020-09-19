using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Service.TripleTriadService;

namespace NWN.FinalFantasy.Entity
{
    public class PlayerTripleTriad: EntityBase
    {
        public PlayerTripleTriad()
        {
            AvailableCards = new Dictionary<CardType, DateTime>();
            Decks = new Dictionary<int, CardDeck>();
        }

        public override string KeyPrefix => "PlayerTripleTriad";
        public Dictionary<CardType, DateTime> AvailableCards { get; set; }
        public Dictionary<int, CardDeck> Decks { get; set; }

        public uint Wins { get; set; }
        public uint Losses { get; set; }
        public uint Draws { get; set; }
        public uint TriadPoints { get; set; }
    }
}
