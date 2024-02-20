class Program
{
    static void Main(string[] args)
    {
        const string CommandCreateFlight = "create";
        const string CommandExit = "exit";

        List<Flight> flights = new List<Flight>();
        bool isRun = true;

        while(isRun)
        {
            Console.WriteLine("Текущие рейсы:");

            foreach(var flight in flights)
                Console.WriteLine
                (
                    $"Отправление: {flight.Route.DepartureCity} Прибытие: {flight.Route.ArrivalCity}\n" +
                    $"Количество пассажиров: {flight.NumberOfTickets}\n" +
                    $"Время отправления: {flight.DepartureTime}"
                );
            
            Console.WriteLine
            (
                $"\n{CommandCreateFlight} - создать новый рейс\n" +
                $"{CommandExit} - Выход"
            );

            switch(Console.ReadLine())
            {
                case CommandCreateFlight:
                    CreateFlight(flights);
                break;

                case CommandExit:
                    isRun = false;
                break;

                default:
                    Console.WriteLine("Неверная команда!");
                break;
            }
        }
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

    static void CreateFlight(List<Flight> flights)
    {
        Console.Write("Введите город отбытия: ");
        string departureCity = Console.ReadLine();

        Console.Write("\nВведите город прибытия: ");
        string arrivalCity = Console.ReadLine();

        Route route = new Route(departureCity, arrivalCity);
        Flight flight = new Flight(route, DateTime.Now);

        Console.WriteLine($"Рейс создан!\nБилетов куплено: {flight.NumberOfTickets}");

        while(flight.NumberOfTickets > flight.GetTrainTotalCapacity())
        {
            Console.WriteLine($"На данный момент вместимость поезда: {flight.GetTrainTotalCapacity()}/{flight.NumberOfTickets}");
            Console.Write("Введите вместимость нового поезда: ");
            
            int wagonCapacity = ReadPositiveNumber();
            flight.AddWagonToTrain(wagonCapacity);
        }

        Console.WriteLine("Состав сформирован!");
        Console.ReadKey();

        flights.Add(flight);
        Console.WriteLine("Рейс отправлен!");
    }
}

class Flight
{
    private const int MinCapacityValue = 1;
    private const int MaxCapacityValue = 100; 
    private Train _train;

    public Flight(Route route, DateTime departureTime)
    {
        Route = route;
        _train = new Train();
        DepartureTime = departureTime;

        Random random = new Random();
        NumberOfTickets = random.Next(MinCapacityValue, MaxCapacityValue + 1);
    }

    public Route Route {get; private set;}
    public int NumberOfTickets {get; private set;}
    public DateTime DepartureTime {get; private set;}

    public void AddWagonToTrain(int capacity) => _train.AddWagon(new Wagon(capacity));
    public int GetTrainTotalCapacity() => _train.GetTotalCapacity();
}

class Route
{
    public Route(string departureCity, string arrivalCity)
    {
        DepartureCity = departureCity;
        ArrivalCity = arrivalCity;
    }

    public string DepartureCity {get; private set;}
    public string ArrivalCity {get; private set;}
}

class Train
{
    private List<Wagon> _wagons;

    public Train() => _wagons = new List<Wagon>();

    public void AddWagon(Wagon wagon) => _wagons.Add(wagon);

    public int GetTotalCapacity()
    {
        int totalCapacity = 0;

        foreach(var wagon in _wagons)
            totalCapacity += wagon.Capacity;
        
        return totalCapacity;
    }
}

class Wagon
{
    public Wagon(int capacity) => Capacity = capacity;

    public int Capacity {get; private set;}
}