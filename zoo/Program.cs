class Program
{
    static void Main(string[] args)
    {
        Zoo zoo = new Zoo();
        zoo.OpenGates();
    }
}

class Cage
{
    private List<Animal> _animals;

    public Cage(string familyName)
    {
        NameOfAnimalFamily = familyName;
        _animals = new List<Animal>();
    }

    public string NameOfAnimalFamily {get; private set;}

    public void ShowInfo()
    {
        Console.WriteLine($"В этом вольере обитают {NameOfAnimalFamily}");
        Console.WriteLine($"Здесь живут {_animals.Count} животных");
        foreach(var animal in _animals)
            animal.ShowInfo();
    } 

    public void AddAnimal(string name, int age, string gender, string sound) => _animals.Add(new Animal(name, age, gender, sound));
}

class Animal
{
    public Animal(string name, int age, string gender, string sound)
    {
        Name = name;
        Age = age;
        Gender = gender;
        Sound = sound;
    }

    public string Name {get; private set;}
    public int Age {get; private set;}
    public string Gender {get; private set;}
    public string Sound {get; private set;}

    public void ShowInfo() => Console.WriteLine($"Имя: {Name}\tВозраст: {Age}\tПол: {Gender}\tИздаёт звук: {Sound}");
}

class Zoo
{
    private List<Cage> _cages;

    public Zoo()
    {
        _cages = new List<Cage>();
        AddCages();
        AddAnimalsToCage();
    }
    public void OpenGates()
    {
        const int MaxNumberOfVisits = 3;

        for(int i = 0; i < MaxNumberOfVisits; i++)
        {
            ShowCages();
            int userInput = ReadCorrectIndex(_cages.Count);
            _cages[userInput - 1].ShowInfo();

            Console.ReadKey();
            Console.Clear();
        }

        Console.WriteLine("Зоопарк закрывается! Всего хорошего!");
    }

    private void AddCages()
    {
        _cages.Add(new Cage("Cats"));
        _cages.Add(new Cage("Crocodiles"));
        _cages.Add(new Cage("Bears"));
        _cages.Add(new Cage("Penguins"));
    }

    private void AddAnimalsToCage()
    {
        _cages[0].AddAnimal("Bagira", 10, "Female", "RRR");
        _cages[0].AddAnimal("Simba", 2, "Male", "BRRR");
        _cages[1].AddAnimal("Dandy", 10, "Male", "aaa");
        _cages[2].AddAnimal("Masha", 20, "Female", "AAA");
        _cages[3].AddAnimal("Skipper", 13, "Male", "kowalski analysis");
        _cages[3].AddAnimal("kowalski", 12, "Male", "analysis");
        _cages[3].AddAnimal("Rico", 10, "Male", "GrBdJ%#!");
        _cages[3].AddAnimal("Private", 5, "Male", "pi-pi-pi");
    }

    private void ShowCages()
    {
        Console.WriteLine($"Перед вами {_cages.Count} вольеров\nВыберите номер, к какому подойти:");
        
        for(int i = 0; i < _cages.Count; i++)
            Console.WriteLine($"{i + 1}: Подойти к вольеру с {_cages[i].NameOfAnimalFamily}");
            
    }

    private int ReadCorrectIndex(int limit)
    {
        int correctValue = 0;
        bool isCorrect = false;

        while (isCorrect == false)
        {
            Console.WriteLine("Выберите номер вольера:");
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