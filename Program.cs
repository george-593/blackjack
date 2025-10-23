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
    Console.WriteLine($"╚{border}╝");
    Console.ResetColor();

    Console.WriteLine("Use ↑/↓ to navigate, Enter to select, Esc to quit.\n");
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

void Quit() {
    Console.Clear();
    Console.CursorVisible = true; // Not reset when you exit for some reason
    Environment.Exit(0);
}

void PlayGame()
{
    Console.Clear();
    Console.WriteLine("Starting new game...\n");
    Console.CursorVisible = true;

    int playerChips = 1000;
    while (playerChips > 0) {
        Console.Write($"Enter your bet, you have {playerChips} chips available: ");
        int bet = int.Parse(Console.ReadLine()!);

        int result = PlayRound(bet);
    }
}

int PlayRound(int bet)
{
    List<Card> dealersCards = new List<Card>();
    List<Card> playersCards = new List<Card>();

    // Clear the game start bet message
    Console.Clear();

    // Give the player an extra card at the start of the round
    Card playerFirstCard = GenerateRandomCard();
    playersCards.Add(playerFirstCard);
    Console.WriteLine($"\nYour Card: {playerFirstCard.cardNum} of {playerFirstCard.suit}");

    bool playerStood = false;

    // Variables to know when the round is over
    bool playerFinished = false;
    bool dealerFinished = false;

    bool firstLoop = true;
    char playerAction = 'h';

    // Loop round until someone wins
    while (true)
    {
        // Clear the console on each new iteration, except first
        if (!firstLoop) Console.Clear();

        // Get totals for player and dealer
        int dealerTotal = SumCardList(dealersCards);
        int playerTotal = SumCardList(playersCards);

        if (playerTotal > 21) {
            if (!playerFinished) playerFinished = true;

            Console.WriteLine($"You have gone bust on {playerTotal}\n");
        } else if (playerAction == 'h')
        {
            Card lastPlayerCard = GenerateRandomCard();
            playersCards.Add(lastPlayerCard);

            playerTotal = SumCardList(playersCards);
            Console.WriteLine($"Your Card: {lastPlayerCard.cardNum} of {lastPlayerCard.suit}\nYour total value: {playerTotal}\n");
        }
        else if (playerAction == 's' || playerStood)
        {
            if (!playerStood) playerStood = true;
            if (!playerFinished) playerFinished = true;

            Console.WriteLine($"You have stood on {playerTotal}\n");
        }

        // Dealer stands on 17
        if (dealerTotal < 17)
        {
            // Deal the cards out
            Card lastDealerCard = GenerateRandomCard();
            dealersCards.Add(lastDealerCard);

            dealerTotal = SumCardList(dealersCards);
            Console.WriteLine($"Dealer's Card: {lastDealerCard.cardNum} of {lastDealerCard.suit}\nDealer's total value: {dealerTotal}");
        }
        else if (dealerTotal > 21)
        {
            if (!dealerFinished) dealerFinished = true;

            Console.WriteLine($"Dealer has gone bust on {dealerTotal}!");
        }
        else
        {
            if (!dealerFinished) dealerFinished = true;

            dealerTotal = SumCardList(dealersCards);
            Console.WriteLine($"Dealer has stood on {dealerTotal}");
        }
        
        if (playerFinished && dealerFinished) break;

        if (!playerFinished) {
            Console.WriteLine("\nWhat action would you like to do?\nH = Hit, S = Stand");
            playerAction = Console.ReadKey(true).KeyChar;
        } else {
            Console.Write("\nPress any key to continue: ");
            Console.ReadLine();
        }

        if (firstLoop) firstLoop = false;
    }
    Console.WriteLine("\nRound Over!!");

    return 1;
}


int SumCardList(List<Card> cardList) {
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