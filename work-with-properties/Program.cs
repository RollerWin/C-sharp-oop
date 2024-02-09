class Program
{
    static void Main(string[] args)
    {
        int positionX = ReadPositiveNumber();
        int positionY = ReadPositiveNumber();

        Player player = new Player(positionX, positionY);
        Renderer renderer = new Renderer();

        renderer.DrawPlayer(player.X, player.Y, player.Character);
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
    int _positionX;
    int _positionY;
    char _playerSymbol = '@';

    public int X
    {
        get
        {
            return _positionX;
        }
        private set
        {
            _positionX = value;
        }
    }

    public int Y
    {
        get
        {
            return _positionY;
        }
        private set
        {
            _positionY = value;
        }
    }

    public char Character
    {
        get
        {
            return _playerSymbol;
        }
    }

    public Player(int x, int y)
    {
        _positionX = x;
        _positionY = y;
    }
}