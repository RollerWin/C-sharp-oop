class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите координаты X и Y");
        int positionX = ReadPositiveNumber();
        int positionY = ReadPositiveNumber();

        Player player = new Player(positionX, positionY);
        Renderer renderer = new Renderer();

        renderer.DrawPlayer(player.PositionX, player.PositionY, player.PlayerSymbol);
    }

    static int ReadPositiveNumber()
    {
        string userInput;
        bool isCorrect = false;
        int correctNumber = 0;

        while(isCorrect != true)
        {
            userInput = Console.ReadLine();

            if(int.TryParse(userInput, out correctNumber) && correctNumber > 0)
            {
                isCorrect = true;
            }
            else
            {
                Console.WriteLine("Некорректный ввод! Попробуйте ещё раз");
                Console.ReadKey();
                Console.Clear();
            }
        }

        return correctNumber;
    }
}

class Renderer
{
    public void DrawPlayer(int x, int y, char playerSymbol)
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.SetCursorPosition(x, y);
        Console.Write(playerSymbol);
        Console.ReadKey(true);
    }
}

class Player
{
    public int PositionX {get; private set;}
    public int PositionY {get; private set;}
    public char PlayerSymbol {get; private set;}

    public Player(int x, int y, char playerSymbol = '@')
    {
        PositionX = x;
        PositionY = y;
        PlayerSymbol = playerSymbol;
    }
}