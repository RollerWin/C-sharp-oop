﻿class Program
{
    static void Main(string[] args)
    {
        PlayerDataBase Users = new PlayerDataBase();
        MenuHandler menuHandler = new MenuHandler(Users);
        
        menuHandler.RunMenu();
    }
}

class Player
{
    static private int _currentId = 1;
    public Player(string inputName, int inputLevel)
    {
        Id = _currentId++;
        NickName = inputName;
        Level = inputLevel;
        isBanned = false;
    }

    public int Id {get; private set;}
    public string NickName {get; private set;}
    public int Level {get; private set;}
    public bool isBanned {get; private set;}

    public void BanPlayer() => isBanned = true;

    public void UnbanPlayer() => isBanned = false;
}

class PlayerDataBase
{
    public PlayerDataBase() => Users = new List<Player>();

    public List<Player> Users {get; set;} 

    public void AddPlayer(Player player) => Users.Add(player);

    public void BanPlayerById(int userId)
    {
        foreach(var user in Users)
            if(user.Id == userId)
                user.BanPlayer();
    }

    public void UnbanPlayerById(int userId)
    {
        foreach(var user in Users)
            if(user.Id == userId)
                user.UnbanPlayer();
    }

    public void DeletePlayerById(int userId)
    {
        Player playerToDelete = null;

        foreach(var user in Users)
            if(user.Id == userId)
                playerToDelete = user;

        if(Users.Contains(playerToDelete))
            Users.Remove(playerToDelete);
        else
        {
            Console.WriteLine("Пользователь не найден!");
        }
    }

    public void ShowUsers()
    {
        foreach(var user in Users)
            Console.WriteLine(user.Id + " - " + user.NickName + " " + user.Level + " " + user.isBanned);
    }
}

class MenuHandler
{
    const string CommandAddPlayer = "add";
    const string CommandBanPlayer = "ban";
    const string CommandUnbanPlayer = "unban";
    const string CommandDeletePlayer = "delete";
    const string CommandShowPlayers = "show";
    const string CommandExit = "exit";

    private PlayerDataBase _users;

    public MenuHandler(PlayerDataBase users) => _users = users;

    public void ShowMenu()
    {
        Console.WriteLine
        (
            $"{CommandAddPlayer} - Добавить игрока\n" + 
            $"{CommandBanPlayer} - Забанить пользователя по Id\n" + 
            $"{CommandUnbanPlayer} - Разбанить игрока по Id" + 
            $"{CommandDeletePlayer} - Удалить игрока\n" + 
            $"{CommandShowPlayers} - Показать всех игроков\n" + 
            $"{CommandExit} - Выход"
        );
    }

    public void RunMenu()
    {
        bool isRun = true;

        while(isRun)
        {
            ShowMenu();

            switch(Console.ReadLine())
            {
                case CommandAddPlayer:
                    AddPlayer();
                break;

                case CommandBanPlayer:
                    BanPlayer();
                break;

                case CommandUnbanPlayer:
                    UnbanPlayer();
                break;

                case CommandDeletePlayer:
                    DeletePlayer();
                break;

                case CommandShowPlayers:
                    ShowUsers();
                break;

                case CommandExit:
                    isRun = false;
                break;

                default:
                    Console.WriteLine("Неизвестная команда!");
                break;
            }

            Console.ReadKey();
            Console.Clear();
        }
    }

    private void AddPlayer()
    {
        Console.Write("Ник: ");
        string inputName = Console.ReadLine();

        Console.Write("\nУровень: ");
        int inputLevel = ReadPositiveNumber();
        Console.WriteLine();

        Player player = new Player(inputName, inputLevel);
        _users.AddPlayer(player);
    }

    private void BanPlayer()
    {
        Console.Write("Введите Id: ");
        int userId = ReadPositiveNumber();
        Console.WriteLine();

        _users.BanPlayerById(userId);
    }

    private void UnbanPlayer()
    {
        Console.Write("Введите Id: ");
        int userId = ReadPositiveNumber();
        Console.WriteLine();

        _users.UnbanPlayerById(userId);
    }

    private void DeletePlayer()
    {
        Console.Write("Введите Id: ");
        int userId = ReadPositiveNumber();
        Console.WriteLine();

        _users.DeletePlayerById(userId);
    }

    private void ShowUsers() => _users.ShowUsers();

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