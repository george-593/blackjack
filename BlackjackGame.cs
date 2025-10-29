static public class BlackjackGame
{
    static public void PlayGame()
    {
        int playerChips = 1000;
        while (playerChips > 0)
        {
            Menu.ResetView();
            Console.CursorVisible = true;

            Console.Write($"Enter your bet, you have {playerChips} chips available: ");
            int bet = int.Parse(Console.ReadLine()!);

            if (bet > playerChips)
            {
                Console.WriteLine("You cannot afford that bet, exiting to menu");

                Console.Write("\nPress any key to continue: ");
                Console.ReadKey();
                return;
            }
            playerChips -= bet;

            // Clear the game start bet message
            Menu.ResetView();
            Console.CursorVisible = false;

            int result = PlayRound(bet);
            playerChips += result;
        }

        Menu.ResetView();
        Console.WriteLine("You have gone bankrupt, better luck next time!");
        Console.Write("\nPress any key to continue: ");
        Console.ReadKey();
    }

    static private int PlayRound(double bet)
    {
        List<Card> dealersCards = new List<Card>();
        List<Card> playersCards = new List<Card>();

        // Deal dealer's first card
        Card firstDealerCard = Utils.GenerateRandomCard();
        dealersCards.Add(firstDealerCard);
        Console.WriteLine($"Dealer's Card: {firstDealerCard.cardNum} of {firstDealerCard.suit}\n");

        // Deal the players first card
        Card playerFirstCard = Utils.GenerateRandomCard();
        playersCards.Add(playerFirstCard);
        Console.WriteLine($"Your Card: {playerFirstCard.cardNum} of {playerFirstCard.suit}");

        // Variables to know when the round is over
        bool playerFinished = false;
        bool dealerFinished = false;

        // Get totals for player and dealer
        int dealerTotal = Utils.SumCardList(dealersCards);
        int playerTotal = Utils.SumCardList(playersCards);

        // The player's action (hit/stand)
        char playerAction = 'h';
        while (!playerFinished)
        {
            // Check if the player has gone bust
            if (playerTotal > 21)
            {
                playerFinished = true;

                Console.WriteLine($"You have gone bust on {playerTotal}\n");
            }
            // If the player hit, generate them another card
            else if (playerAction == 'h')
            {
                Card lastPlayerCard = Utils.GenerateRandomCard();
                playersCards.Add(lastPlayerCard);

                playerTotal = Utils.SumCardList(playersCards);
                Console.WriteLine($"Your Card: {lastPlayerCard.cardNum} of {lastPlayerCard.suit}\nYour total value: {playerTotal}");
            }
            // If the player stood, end their turn
            else if (playerAction == 's')
            {
                playerFinished = true;

                Console.WriteLine($"You have stood on {playerTotal}\n");
            }

            // Input management
            if (!playerFinished)
            {
                Console.WriteLine("\nWhat action would you like to do?\nH = Hit, S = Stand");
                playerAction = Console.ReadKey(true).KeyChar;
            }
            else
            {
                Console.Write("\nPress any key to continue: ");
                Console.ReadKey();
            }

            Menu.ResetView();
        }


        while (!dealerFinished)
        {
            // Check if the dealer has gone bust
            if (dealerTotal > 21)
            {
                dealerFinished = true;

                Console.WriteLine($"Dealer has gone bust on {dealerTotal}!");
            }
            //  Check if the dealer has stood (on 17)
            else if (dealerTotal >= 17)
            {
                dealerFinished = true;

                dealerTotal = Utils.SumCardList(dealersCards);
                Console.WriteLine($"Dealer has stood on {dealerTotal}");
            }
            // Generate a new card for the dealer
            else
            {
                Card lastDealerCard = Utils.GenerateRandomCard();
                dealersCards.Add(lastDealerCard);

                dealerTotal = Utils.SumCardList(dealersCards);
                Console.WriteLine($"Dealer's Card: {lastDealerCard.cardNum} of {lastDealerCard.suit}\nDealer's total value: {dealerTotal}");
            }

            Console.Write("\nPress any key to continue: ");
            Console.ReadKey();

            Menu.ResetView();
        }
        Console.WriteLine("Round Over!");

        Console.WriteLine($"Player's Total: {playerTotal}\nDealer's Total: {dealerTotal}\n");

        if (playerTotal == 21 && playersCards.Count == 2 && !(dealerTotal == 21 && dealersCards.Count == 2))
        {
            Console.WriteLine("Result: Blackjack Win");
            bet *= 2.5;
        }
        else if (playerTotal > 21)
        {
            Console.WriteLine("Result: Loss");
            bet = 0;
        }
        else if (dealerTotal > 21)
        {
            Console.WriteLine("Result: Win");
            bet *= 2;
        }
        else if (playerTotal > dealerTotal)
        {
            Console.WriteLine("Result: Win");
            bet *= 2;
        }
        else if (playerTotal == dealerTotal)
        {
            Console.WriteLine("Result: Push");
        }
        else
        {
            Console.WriteLine("Result: Loss");
            bet = 0;
        }


        Console.WriteLine($"You receive: {bet} credits\n");
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        return (int)Math.Floor(bet);
    }
}