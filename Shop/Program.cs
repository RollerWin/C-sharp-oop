class Program
{
    static void Main(string[] args)
    {
        Seller seller = new Seller();
        
        Console.Write($"Сколько денег у игрока: ");
        int playerMoney = ReadPositiveNumber();
        Player player = new Player(playerMoney);

        MenuHandler menuHandler = new MenuHandler(seller, player);
        menuHandler.RunMenu();
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

class Item
{
    public Item(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public string Name {get; private set;}
    public int Price {get; private set;}
}

class Human
{
    protected List<Item> _inventory;

    public Human() => _inventory = new List<Item>();

    public void ShowInventory()
    {
        foreach(var item in _inventory)
            Console.WriteLine($"Предмет: {item.Name}\tЦена: {item.Price}");
    } 
}

class Seller : Human
{
    public Seller() : base()
    {
        _inventory.Add(new Item("health potion", 10));
        _inventory.Add(new Item("mana potion", 15));
        _inventory.Add(new Item("silver sword", 20));
        _inventory.Add(new Item("iron armor", 30));
    }

    public bool TryGetItem(string itemName, out Item sellItem)
    {
        bool isFound = false;
        sellItem = null;

        foreach(var item  in _inventory)
        {
            if(itemName == item.Name)
            {
                isFound = true;
                sellItem = item;
            }
        }

        return isFound;
    }

    public void SellProduct(Item item) => _inventory.Remove(item);
}

class Player : Human
{
    public Player(int money) : base()
    {
        Money = money;
    }

    public int Money {get; private set;}

    public bool CanPay(int price) 
    {
        return Money >= price;
    }

    public void BuyProduct(Item purchasedItem)
    {
        _inventory.Add(purchasedItem);
        Money -= purchasedItem.Price;
    }
}

class MenuHandler
{
    const string CommandShowSellerItems = "showcase";
    const string CommandShowPlayerItems = "inventory";
    const string CommandBuyItem = "buy";
    const string CommandExit = "exit";

    private Seller _seller;
    private Player _player;

    public MenuHandler(Seller seller, Player player)
    {
        _seller = seller;
        _player = player;
    }

    public void ShowMenu() => Console.WriteLine
    (
        $"{CommandShowSellerItems} - Посмотреть предметы на продажу\n" +
        $"{CommandShowPlayerItems} - Посмотреть свои предметы\n" +
        $"{CommandBuyItem} - Купить продукт\n" +
        $"{CommandExit} - Выход\n"
    );

    public void RunMenu()
    {
        bool isRun = true;

        while(isRun)
        {
            ShowMenu();            

            switch(Console.ReadLine())
            {
                case CommandShowSellerItems:
                    _seller.ShowInventory();
                break;

                case CommandShowPlayerItems:
                    _player.ShowInventory();
                break;

                case CommandBuyItem:
                    BuyItem();
                break;

                case CommandExit:
                    isRun = false;
                break;

                default:
                    Console.WriteLine("Неверная команда!");
                break;
            }

            Console.ReadKey();
            Console.Clear();
        }
    }

    private void BuyItem()
    {
        _seller.ShowInventory();
        Console.WriteLine($"\nСейчас у вас {_player.Money} монет");
        Console.Write("\nВведите название предмета, который хотите купить: ");
        string itemName = Console.ReadLine();
        Console.WriteLine();

        if(_seller.TryGetItem(itemName, out Item item) && _player.CanPay(item.Price))
        {
            _seller.SellProduct(item);
            _player.BuyProduct(item);
            Console.WriteLine("Сделка состоялась");
        }
        else
        {
            Console.WriteLine("Неверное название предмета или у вас недостаточно денег!");
        }
    }
}