﻿class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите координаты X и Y");
        int positionX = ReadPositiveNumber();
        int positionY = ReadPositiveNumber();

        Player player = new Player(positionX, positionY);
        Renderer renderer = new Renderer();

        renderer.DrawPlayer(player);
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
    public void DrawPlayer(Player player)
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.SetCursorPosition(player.PositionX, player.PositionY);
        Console.Write(player.PlayerSymbol);
        Console.ReadKey(true);
    }
}

class Player
{
    public Player(int inputX, int inputY, char playerSymbol = '@')
    {
        PositionX = inputX;
        PositionY = inputY;
        PlayerSymbol = playerSymbol;
    }

    public int PositionX {get; private set;}
    public int PositionY {get; private set;}
    public char PlayerSymbol {get; private set;}
}