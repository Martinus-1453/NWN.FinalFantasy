using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public static class CardList
    {
        public static Dictionary<CardType, Card> Create()
        {
            return new Dictionary<CardType, Card>()
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
                {CardType.Propagator, new Card("Propagator", "Card_P_Propagato", 8, 4, 8, 4)},
                {CardType.JumboCactuar, new Card("Jumbo Cactuar", "Card_P_JumboCact", 8, 4, 4, 8)},
                {CardType.TriPoint, new Card("Tri-Point", "Card_P_TriPoint", 8, 2, 8, 5, CardElementType.Lightning)},
                {CardType.Gargantua, new Card("Gargantua", "Card_P_Gargantua", 5, 6, 8, 6)},
                {CardType.MobileType8, new Card("Mobile Type 8", "Card_P_Mobiletyp", 8, 7, 3, 6)},
                {CardType.Sphinxara, new Card("Sphinxara", "Card_P_Sphinxara", 8, 5, 8, 3)},
                {CardType.Tiamat, new Card("Tiamat", "Card_P_Tiamat", 8, 5, 4, 8)},
                {CardType.BGH251F2, new Card("BGH251F2", "Card_P_BGH251F2", 5, 8, 5, 7)},
                {CardType.RedGiant, new Card("Red Giant", "Card_P_RedGiant", 6, 4, 7, 8)},
                {CardType.Catoblepas, new Card("Catoblepas", "Card_P_Catoblepa", 1, 7, 7, 8)},
                {CardType.UltimaWeapon, new Card("Ultima Weapon", "Card_P_UltimaWea", 7, 2, 8, 7)},

                // Level 8 Cards
                {CardType.ChubbyChocobo, new Card("Chubby Chocobo", "Card_P_ChubbyCho", 4, 8, 9, 4)},
                {CardType.Angelo, new Card("Angelo", "Card_P_Angelo", 9, 7, 3, 6)},
                {CardType.Gilgamesh, new Card("Gilgamesh", "Card_P_Gilgamesh", 3, 9, 6, 7)},
                {CardType.MiniMog, new Card("MiniMog", "Card_P_MiniMog", 9, 9, 2, 3)},
                {CardType.Chicobo, new Card("Chicobo", "Card_P_Chicobo", 9, 8, 4, 4)},
                {CardType.Quezacotl, new Card("Quezacotl", "Card_P_Quezacotl", 2, 9, 4, 9)},
                {CardType.Shiva, new Card("Shiva", "Card_P_Shiva", 6, 4, 9, 7)},
                {CardType.Ifrit, new Card("Ifrit", "Card_P_Ifrit", 9, 2, 8, 6, CardElementType.Fire)},
                {CardType.Siren, new Card("Siren", "Card_P_Siren", 8, 6, 2, 9)},
                {CardType.Sacred, new Card("Sacred", "Card_P_Sacred", 5, 9, 9, 1)},
                {CardType.Minotaur, new Card("Minotaur", "Card_P_Minotaur", 9, 2, 9, 5)},

                // Level 9 Cards
                {CardType.Carbuncle, new Card("Carbuncle", "Card_P_Carbuncle", 8, 10, 4, 4)},
                {CardType.Diablos, new Card("Diablos", "Card_P_Diablos", 5, 8, 3, 10)},
                {CardType.Leviathan, new Card("Leviathan", "Card_P_Leviathan", 7, 1, 7, 10, CardElementType.Water)},
                {CardType.Odin, new Card("Odin", "Card_P_Odin", 8, 3, 5, 10)},
                {CardType.Pandemona, new Card("Pandemona", "Card_P_Pandemona", 10, 7, 7, 1, CardElementType.Wind)},
                {CardType.Cerberus, new Card("Cerberus", "Card_P_Cerberus", 7, 6, 10, 4)},
                {CardType.Alexander, new Card("Alexander", "Card_P_Alexander", 9, 4, 2, 10)},
                {CardType.Phoenix, new Card("Phoenix", "Card_P_Phoenix", 7, 7, 10, 2, CardElementType.Fire)},
                {CardType.Bahamut, new Card("Bahamut", "Card_P_Bahamut", 10, 2, 6, 8)},
                {CardType.Doomtrain, new Card("Doomtrain", "Card_P_Doomtrain", 3, 10, 10, 1)},
                {CardType.Eden, new Card("Eden", "Card_P_Eden", 4, 9, 10, 4)},

                // Level 10 Cards
                {CardType.Ward, new Card("Ward", "Card_P_Ward", 10, 2, 8, 7)},
                {CardType.Kiros, new Card("Kiros", "Card_P_Kiros", 6, 6, 10, 7)},
                {CardType.Laguna, new Card("Laguna", "Card_P_Laguna", 5, 3, 9, 10)},
                {CardType.Selphie, new Card("Selphie", "Card_P_Selphie", 10, 6, 4, 8)},
                {CardType.Quistis, new Card("Quistis", "Card_P_Quistis", 9, 10, 2, 6)},
                {CardType.Irvine, new Card("Irvine", "Card_P_Irvine", 2, 9, 10, 6)},
                {CardType.Zell, new Card("Zell", "Card_P_Zell", 8, 10, 6, 5)},
                {CardType.Rinoa, new Card("Rinoa", "Card_P_Rinoa", 4, 2, 10, 10)},
                {CardType.Edea, new Card("Edea", "Card_P_Edea", 10, 3, 3, 10)},
                {CardType.Seifer, new Card("Seifer", "Card_P_Seifer", 6, 10, 4, 9)},
                {CardType.Squall, new Card("Squall", "Card_P_Squall", 10, 6, 9, 4)}
            };
        }
    }
}
