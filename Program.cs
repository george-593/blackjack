using System.Security.Cryptography;

void MainMenu()
{
    Console.CursorVisible = false;
    var options = new[] { "Play", "Quit" };
    int selected = 0;

    while (true)
    {
        Console.Clear();
        DrawHeader();
        DrawMenu(options, selected);
        Console.WriteLine("Use ↑/↓ to navigate, Enter to select, Esc to quit.\n");

        var key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                selected = (selected - 1 + options.Length) % options.Length;
                break;
            case ConsoleKey.DownArrow:
                selected = (selected + 1) % options.Length;
                break;
            case ConsoleKey.Enter:
                if (selected == 0) PlayGame(); // Play
                else if (selected == 1) Quit(); // Quit
                break;
            case ConsoleKey.Escape:
                Quit();
                break;
        }
    }
}

void DrawHeader()
{
    var title = "BLACKJACK";
    int innerWidth = title.Length + 10;
    string border = new string('═', innerWidth);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"╔{border}╗");
    Console.Write("║   ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("♠ ");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(title);
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write(" ♥");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("   ║");
    Console.WriteLine($"╚{border}╝\n");
    Console.ResetColor();
}

void DrawMenu(string[] options, int selected)
{
    for (int i = 0; i < options.Length; i++)
    {
        if (i == selected)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"> {options[i]}");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"  {options[i]}");
        }
    }
}

void Quit()
{
    Console.Clear();
    Console.CursorVisible = true; // Not reset when you exit for some reason
    Environment.Exit(0);
}

// Clears the console and draws the header again
void ResetView()
{
    Console.Clear();
    DrawHeader();
}

void PlayGame()
{
    int playerChips = 1000;
    while (playerChips > 0)
    {
        ResetView();
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
        ResetView();
        Console.CursorVisible = false;

        int result = PlayRound(bet);
        playerChips += result;
    }

    ResetView();
    Console.WriteLine("You have gone bankrupt, better luck next time!");
    Console.Write("\nPress any key to continue: ");
    Console.ReadKey();
}

int PlayRound(double bet)
{
    List<Card> dealersCards = new List<Card>();
    List<Card> playersCards = new List<Card>();

    // Deal dealer's first card
    Card firstDealerCard = GenerateRandomCard();
    dealersCards.Add(firstDealerCard);
    Console.WriteLine($"Dealer's Card: {firstDealerCard.cardNum} of {firstDealerCard.suit}\n");

    // Deal the players first card
    Card playerFirstCard = GenerateRandomCard();
    playersCards.Add(playerFirstCard);
    Console.WriteLine($"Your Card: {playerFirstCard.cardNum} of {playerFirstCard.suit}");

    // Variables to know when the round is over
    bool playerFinished = false;
    bool dealerFinished = false;

    // Get totals for player and dealer
    int dealerTotal = SumCardList(dealersCards);
    int playerTotal = SumCardList(playersCards);

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
            Card lastPlayerCard = GenerateRandomCard();
            playersCards.Add(lastPlayerCard);

            playerTotal = SumCardList(playersCards);
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

        ResetView();
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

            dealerTotal = SumCardList(dealersCards);
            Console.WriteLine($"Dealer has stood on {dealerTotal}");
        }
        // Generate a new card for the dealer
        else
        {
            Card lastDealerCard = GenerateRandomCard();
            dealersCards.Add(lastDealerCard);

            dealerTotal = SumCardList(dealersCards);
            Console.WriteLine($"Dealer's Card: {lastDealerCard.cardNum} of {lastDealerCard.suit}\nDealer's total value: {dealerTotal}");
        }

        Console.Write("\nPress any key to continue: ");
        Console.ReadKey();

        ResetView();
    }
    Console.WriteLine("Round Over!");

    Console.WriteLine($"Player's Total: {playerTotal}\nDealer's Total: {dealerTotal}\n");

    if (playerTotal == 21 && playersCards.Count == 2 && !(dealerTotal == 21 && dealersCards.Count == 2))
    {
        Console.WriteLine("Result: Blackjack Win");
        bet *= 2.5;
    }
    if (playerTotal > 21)
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


int SumCardList(List<Card> cardList)
{
    int sum = 0;
    foreach (Card card in cardList)
    {
        sum += card.cardNum;
    }
    return sum;
}

Card GenerateRandomCard()
{
    // Get a random card from the suits
    string[] suits = { "Spades", "Hearts", "Diamonds", "Clubs" };
    int suitNum = RandomNumberGenerator.GetInt32(0, suits.Length);
    string suit = suits[suitNum];

    // Random card number from the blackjack deck
    int cardNum = RandomNumberGenerator.GetInt32(1, 13);
    // Set the value to a max of 10 (still need 13 to simulate full deck)
    if (cardNum > 10) cardNum = 10;

    return new Card(suit, cardNum);
}

MainMenu();