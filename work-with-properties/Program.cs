class Program
{
    static void Main(string[] args)
    {
        Player player = new Player();
        Field field = new Field();
        Renderer renderer = new Renderer();

        while(true)
        {
            field.DrawField();
            renderer.UpdateScreen(player.X, player.Y, player.Symbol);
            ConsoleKeyInfo userInput = Console.ReadKey(true);
            player.HandleInput(userInput);
            Console.Clear();
        }
    }
}

class Player
{
    int _positionX;
    int _positionY;
    char _playerSymbol = '@';

    public Player()
    {
        _positionX = Field.MinPositionX + 1;
        _positionY = Field.MinPositionY + 1;
    }

    public int X
    {
        get
        {
            return _positionX;
        }
        private set
        {
            if(_positionX + value > Field.MinPositionX && _positionX + value < Field.MaxPositionX - 1)
            {
                _positionX += value;
            }
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
            if(_positionY + value > Field.MinPositionY && _positionY + value < Field.MaxPositionY - 1)
            {
                _positionY += value;
            }
        }
    }

    public char Symbol 
    {
        get 
        {
            return _playerSymbol;
        }
    }

    public void HandleInput(ConsoleKeyInfo input)
    {
        const ConsoleKey СommandShiftLeftArrowawMap = ConsoleKey.LeftArrow;
        const ConsoleKey СommandShiftLeftBoard = ConsoleKey.A;

        const ConsoleKey СommandShiftRightArrow = ConsoleKey.RightArrow;
        const ConsoleKey СommandShiftRightBoard = ConsoleKey.D;

        const ConsoleKey СommandShiftUpArrow = ConsoleKey.UpArrow;
        const ConsoleKey СommandShiftUpBoard = ConsoleKey.W;

        const ConsoleKey СommandShiftDownArrow = ConsoleKey.DownArrow;
        const ConsoleKey СommandShiftDownBoard = ConsoleKey.S;

        switch(input.Key)
        {
            case СommandShiftLeftArrowawMap:
            case СommandShiftLeftBoard:
                X = -1;
            break;

            case СommandShiftRightArrow:
            case СommandShiftRightBoard:
                X = 1;
            break;

            case СommandShiftUpArrow:
            case СommandShiftUpBoard:
                Y = -1;
            break;

            case СommandShiftDownArrow:
            case СommandShiftDownBoard:
                Y = 1;
            break;
        }
    }
}

class Field
{
    public const int MinPositionX = 0;
    public const int MinPositionY = 0;
    public const int MaxPositionX = 10;
    public const int MaxPositionY = 10;
    public const char BorderSymbol = '#';

    // private char[] _fieldMap;
    public void DrawField()
    {
        for(int i = MinPositionY; i < MaxPositionY; i++)
        {
            for(int j = MinPositionX; j < MaxPositionX; j++)
            {
                Console.Write(BorderSymbol);
            }

            Console.WriteLine();
        }
    }
}

class Renderer
{
    public void UpdateScreen(int x, int y, char playerSymbol)
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(x, y);
        Console.Write(playerSymbol);  
    }
}