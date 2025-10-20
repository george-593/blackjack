using System.Security.Cryptography;

Card GenerateRandomCard()
{
    // Get a random card from the suits
    string[] suits = { "Spades", "Hearts", "Diamonds", "Clubs" };
    int suitNum = RandomNumberGenerator.GetInt32(0, suits.Length);
    string suit = suits[suitNum];

    // Random card number from the blackjack deck
    int cardNum = RandomNumberGenerator.GetInt32(1, 13);

    return new Card(suit, cardNum);
}

Console.WriteLine(GenerateRandomCard().cardNum);
Console.WriteLine(GenerateRandomCard().suit);