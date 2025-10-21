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

}

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

MainMenu();