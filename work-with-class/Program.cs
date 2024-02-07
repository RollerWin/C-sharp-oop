class Program
{
    static void Main(string[] args)
    {
        Console.Write("Дайте игроку имя: ");
        string userName = Console.ReadLine();

        Console.Write("\nСколько у него жизней: ");
        int userHealth = ReadPositiveNumber();

        Console.Write("\nСколько у него силы: ");
        int userPower = ReadPositiveNumber();

        Player player = new Player(userName, userHealth, userPower);
        player.ShowInfo();

        Player defaultPlayer = new Player();
        Console.WriteLine("\nА вот так выглядел бы пользователь, по умолчанию: ");
        defaultPlayer.ShowInfo();
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

class Player
{
    string _name;
    int _health;
    int _power;

    public Player()
    {
        _name = "Newbie";
        _health = 100;
        _power = 10;
    }

    public Player(string name, int health, int power)
    {
        _name = name;
        _health = health;
        _power = power;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Персонаж имеет следующие характеристики: \nИмя - {_name}\nЗдоровье - {_health}\nСила - {_power}");
    }
}