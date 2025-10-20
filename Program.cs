using System.Security.Cryptography;

Card GenerateRandomCard()
{
    Random random = new Random();
    string[] suits = { "Spades", "Hearts", "Diamonds", "Clubs" };
    int suitNum = RandomNumberGenerator.GetInt32(0, suits.Length);
    string suit = suits[suitNum];

    int cardNum = RandomNumberGenerator.GetInt32(1, 13);

    return new Card(suit, cardNum);
}

Console.WriteLine(GenerateRandomCard().cardNum);
Console.WriteLine(GenerateRandomCard().suit);