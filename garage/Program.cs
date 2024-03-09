class Program
{
    static void Main(string[] args)
    {
        CarService carService = new CarService();
        Queue<Client> clients = new Queue<Client>();

        Console.Write("Введите количество клиентов: ");
        int userInput = ReadCorrectValue();
        
        for(int i = 0; i < userInput; i++)
            clients.Enqueue(new Client());

        while(clients.Count > 0 && carService.Money >= 0)
            carService.ServeClient(clients.Dequeue());

        if(clients.Count == 0)
            Console.WriteLine("Клиенты закончились! Ура!");
        else if(carService.Money < 0)
            Console.WriteLine("Денег нет! Игра окончена :(");
    }

    static int ReadCorrectValue()
    {
        int correctValue = 0;
        bool isCorrect = false;

        while (isCorrect == false)
        {
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out correctValue) && correctValue > 0)
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

class CarService
{
    private const int PenaltyFee = 100;
    private const int PriceForRepair = 50;

    private StoreHouse _storeHouse;

    public CarService()
    {
        int minServiceMoney = 20;
        int maxServiceMoney = 400;
        Money = UserUtils.GenerateRandomValue(minServiceMoney, maxServiceMoney);
        _storeHouse = new StoreHouse();
    }

    public int Money {get; private set;}

    public void ServeClient(Client client)
    {
        Console.WriteLine($"У клиента поломка {client.BrokenDetail.Name}");
        Console.WriteLine("Мы имеем детали:");
        _storeHouse.ShowDetails();

        if(_storeHouse.IsDetailExist(client.BrokenDetail.Name))
        {
            if(client.Money >= client.BrokenDetail.Price + PriceForRepair)
            {
                int userInput = ReadCorrectIndex(_storeHouse.GetNumberOfDetails());
                Detail usedDetail = _storeHouse.GetDetail(userInput - 1);

                if(usedDetail.Name == client.BrokenDetail.Name)
                {
                    int totalPrice = usedDetail.Price + PriceForRepair;
                    Console.WriteLine($"Клиент доволен! Оплата: {totalPrice}");
                    client.PayForService(totalPrice);
                    Money += PriceForRepair;
                }
                else
                {
                    Console.WriteLine($"Деталь не та! Штраф: {PenaltyFee}");
                    Money -= PenaltyFee;
                }
            }
            else
            {
                Console.WriteLine("Клиент не может заплатить за ремонт!");
            }
        }
        else
        {
            Console.WriteLine($"Деталь отсутствует! Штраф: {PenaltyFee}");
            Money -= PenaltyFee;
        }

        Console.WriteLine($"Итоговая сумма: {Money}");
    }

    private int ReadCorrectIndex(int limit)
    {
        int correctValue = 0;
        bool isCorrect = false;

        while (isCorrect == false)
        {
            Console.WriteLine("Выберите номер детали:");
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

class Client
{
    public Client()
    {
        int minClientMoney = 50;
        int maxClientMoney = 400;
        Money = UserUtils.GenerateRandomValue(minClientMoney, maxClientMoney);
        BrokenDetail = DetailsTemplate.GetRandomDetail();
    }

    public int Money {get; private set;}
    public Detail BrokenDetail {get; private set;}

    public void PayForService(int Price) => Money -= Price;
}

class Detail
{
    public Detail(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public string Name {get; private set;}
    public int Price {get; private set;}
}

class StoreHouse
{
    private List<Detail> _details;

    public StoreHouse()
    {
        _details = new List<Detail>();
        AddDetails();
    }

    public void ShowDetails()
    {
        for(int i = 0; i < _details.Count; i++)
            Console.WriteLine($"{i + 1}. {_details[i].Name}, Цена: {_details[i].Price}");
    }

    public Detail GetDetail(int index)
    {
        Detail tempDetail = _details[index];
        _details.RemoveAt(index);

        return new Detail(tempDetail.Name, tempDetail.Price);
    }

    public bool IsDetailExist(string detailName)
    {
        bool isFound = false;

        foreach(var detail in _details)
            if(detailName == detail.Name)
                isFound = true;

        return isFound;
    }

    public int GetNumberOfDetails() => _details.Count;

    private void AddDetails()
    {
        int minNumberOfDetails = 5;
        int maxNumberOfDetails = 15;
        int numbersOfDetails = UserUtils.GenerateRandomValue(minNumberOfDetails, maxNumberOfDetails + 1);

        for(int i = 0; i < numbersOfDetails; i++)
            _details.Add(DetailsTemplate.GetRandomDetail());
    }
}

static class DetailsTemplate
{
    private static List<Detail> s_details;

    static DetailsTemplate()
    {
        s_details = new List<Detail>();
        AddDetails();
    }

    static public Detail GetRandomDetail()
    {
        Detail tempDetail = s_details[UserUtils.GenerateRandomValue(0, s_details.Count - 1)];

        return new Detail(tempDetail.Name, tempDetail.Price);
    }

    static public int GetNumberOfDetails() => s_details.Count;

    static private void AddDetails()
    {
        s_details.Add(new Detail("Мотор", 200));
        s_details.Add(new Detail("Тормоза", 100));
        s_details.Add(new Detail("Фара", 25));
        s_details.Add(new Detail("Колесо", 50));
        s_details.Add(new Detail("Коробка передач", 75));
        s_details.Add(new Detail("Бампер", 30));
    }
}

static class UserUtils
{
    private static Random s_random;

    static UserUtils() => s_random = new Random();

    static public int GenerateRandomValue(int min, int max)
    {
        return s_random.Next(min, max);
    }
}