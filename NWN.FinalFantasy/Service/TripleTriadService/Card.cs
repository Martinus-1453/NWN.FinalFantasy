namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public class Card
    {
        public Card(string name, string texture, int topPower, int bottomPower, int leftPower, int rightPower, CardElementType element = CardElementType.None)
        {
            Name = name;
            Texture = texture;
            TopPower = topPower;
            BottomPower = bottomPower;
            LeftPower = leftPower;
            RightPower = rightPower;
            Element = element;
        }

        public string Name { get; set; }
        public string Texture { get; set; }
        public int TopPower { get; set; }
        public int BottomPower { get; set; }
        public int LeftPower { get; set; }
        public int RightPower { get; set; }
        public CardElementType Element { get; set; }
    }
}
