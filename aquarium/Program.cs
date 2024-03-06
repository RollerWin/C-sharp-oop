class Program
{
    static void Main(string[]args)
    {
        const string CommandAddFish = "add";
        const string CommandRemoveFish = "remove";
        const string CommandRemoveAllDeadFishes = "remove dead";
        const string CommandShowFishes = "show";
        const string CommandExit = "exit";

        Aquarium aquarium = new Aquarium();
    
        bool isRun = true;

        while(isRun)
        {
            Console.WriteLine
            (
                $"{CommandAddFish} - Добавить рыбу\n" +
                $"{CommandRemoveFish} - добавить рыбу\n" +
                $"{CommandRemoveAllDeadFishes} - убрать всех мёртвых рыб\n" +
                $"{CommandShowFishes} - показать рыб и их состояние\n" +
                $"{CommandExit} - выход" 
            );

            switch(Console.ReadLine())
            {
                case CommandAddFish:
                    aquarium.AddFish();
                break;

                case CommandRemoveFish:
                    aquarium.RemoveFish();
                break;

                case CommandRemoveAllDeadFishes:
                    aquarium.RemoveAllDeadFishes();
                break;

                case CommandShowFishes:
                    aquarium.ShowInfo();
                break;

                case CommandExit:
                    isRun = false;
                break;

                default:
                    Console.WriteLine("Некорректная программа!");
                break;
            }

            Console.ReadKey();
            Console.Clear();

            aquarium.PassTime();
        }
    }
}

class Aquarium
{
    private const int MaxNumberOfFishes = 10;

    private List<Fish> _fishes;

    public Aquarium() => _fishes = new List<Fish>();

    public void ShowInfo()
    {
        for(int i = 0 ; i<_fishes.Count; i++)
        {
            Console.Write($"{i + 1} - ");
            _fishes[i].ShowInfo();
        }
    }

    public void AddFish()
    {
        if(_fishes.Count < MaxNumberOfFishes)
        {
            Console.Write("Введите имя рыбы:");
            string fishName = Console.ReadLine();
            int fishAge = ReadCorrectFishAge();

            _fishes.Add(new Fish(fishName, fishAge));
        }
        else
        {
            Console.WriteLine("аквариум переполнен!");
        }
    }

    public void RemoveFish()
    {
        ShowInfo();
        Console.Write("Введите индекс рыбы, которую хотите убрать: ");
        int userInput = ReadCorrectIndex();
        _fishes.RemoveAt(userInput - 1);
    }

    public void RemoveAllDeadFishes()
    {
        for(int i = _fishes.Count - 1; i >= 0; i--)
            if(_fishes[i].IsAlive() == false)
                _fishes.RemoveAt(i);
    }

    public void PassTime()
    {
        foreach(var fish in _fishes)
            fish.BecomeOlderYear();
    }

    private int ReadCorrectFishAge() => ReadCorrectValue("\nВведите возраст рыбы: ", Fish.GetMaxAge());

    private int ReadCorrectIndex() => ReadCorrectValue("\nВведите индекс: ", _fishes.Count);
    
    private int ReadCorrectValue(string prompt, int limit)
    {
        int correctValue = 0;
        bool isCorrect = false;

        while (!isCorrect)
        {
            Console.Write(prompt);
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out correctValue) && correctValue > 0 && correctValue <= limit)
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

        return correctValue;
    }
}

class Fish
{
    private const int MaxAge = 10;

    public Fish(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string Name {get; private set;}
    public int Age {get; private set;}

    public static int GetMaxAge() => MaxAge;

    public void BecomeOlderYear() => Age += 1;

    public bool IsAlive() => Age < MaxAge;

    public void ShowInfo()
    {
        Console.Write($"Имя: {Name}\tВозраст: {Age}\tСостояние: ");

        if(IsAlive())
            Console.WriteLine("жива");
        else
            Console.WriteLine("мертва");
    }
}