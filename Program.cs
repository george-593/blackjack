Card GenerateRandomCard()
{
    Random random = new Random();
    string[] suits = { "Spades", "Hearts", "Diamonds", "Clubs" };
    int suitNum = random.Next(0, suits.Length);
    string suit = suits[suitNum];

    int cardNum = random.Next(1, 14);

    return new Card(suit, cardNum);
}