
static public class Menu
{
    static public void MainMenu()
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
                    if (selected == 0) BlackjackGame.PlayGame(); // Play
                    else if (selected == 1) Quit(); // Quit
                    break;
                case ConsoleKey.Escape:
                    Quit();
                    break;
            }
        }
    }

    static private void DrawHeader()
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

    static private void DrawMenu(string[] options, int selected)
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

    static public void Quit()
    {
        Console.Clear();
        Console.CursorVisible = true; // Not reset when you exit for some reason
        Environment.Exit(0);
    }

    // Clears the console and draws the header again
    static public void ResetView()
    {
        Console.Clear();
        DrawHeader();
    }
}
