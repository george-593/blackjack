using System.Security.Cryptography;

static public class Utils
{
    static public int SumCardList(List<Card> cardList)
    {
        int sum = 0;
        foreach (Card card in cardList)
        {
            sum += card.cardNum;
        }
        return sum;
    }

    static public Card GenerateRandomCard()
    {
        // Get a random card from the suits
        string[] suits = { "Spades", "Hearts", "Diamonds", "Clubs" };
        int suitNum = RandomNumberGenerator.GetInt32(0, suits.Length);
        string suit = suits[suitNum];

        // Random card number from the blackjack deck
        int cardNum = RandomNumberGenerator.GetInt32(1, 14);
        // Set the value to a max of 10 (still need 13 to simulate full deck)
        if (cardNum > 10) cardNum = 10;

        return new Card(suit, cardNum);
    }
}