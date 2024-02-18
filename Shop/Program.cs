using System.Data.SqlTypes;
using System.Dynamic;

class Program
{
    static void Main(string[] args)
    {
        Seller seller = new Seller();
        
        Console.Write($"Сколько денег у игрока: ");
        int playerMoney = ReadPositiveNumber();
        Player player = new Player(playerMoney);

        Shop Shop = new Shop(seller, player);
        Shop.RunMenu();
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
    protected List<Item> Inventory;

    public Human()
    {
        Inventory = new List<Item>();
        Money = 0;
    } 

    public int Money {get; protected set;}

    public void ShowInventory()
    {
        foreach(var item in Inventory)
            Console.WriteLine($"Предмет: {item.Name}\tЦена: {item.Price}");
    } 
}

class Seller : Human
{
    public Seller() : base()
    {
        Inventory.Add(new Item("health potion", 10));
        Inventory.Add(new Item("mana potion", 15));
        Inventory.Add(new Item("silver sword", 20));
        Inventory.Add(new Item("iron armor", 30));
    }

    public bool TryGetItem(string itemName, out Item sellItem)
    {
        bool isFound = false;
        sellItem = null;

        foreach(var item  in Inventory)
        {
            if(itemName == item.Name)
            {
                isFound = true;
                sellItem = item;
            }
        }

        return isFound;
    }

    public void SellProduct(Item item)
    {
        Inventory.Remove(item);
        Money += item.Price;
    }
}

class Player : Human
{
    public Player(int money) : base()
    {
        Money = money;
    }

    public bool CanPay(int price) 
    {
        return Money >= price;
    }

    public void BuyProduct(Item purchasedItem)
    {
        Inventory.Add(purchasedItem);
        Money -= purchasedItem.Price;
    }
}

class Shop
{
    const string SellerItems = "showcase";
    const string PlayerItems = "inventory";
    const string MakeDeal = "buy";
    const string Exit = "exit";

    private Seller _seller;
    private Player _player;

    public Shop(Seller seller, Player player)
    {
        _seller = seller;
        _player = player;
    }

    public void ShowMenu() => Console.WriteLine
    (
        $"{SellerItems} - Посмотреть предметы на продажу\n" +
        $"{PlayerItems} - Посмотреть свои предметы\n" +
        $"{MakeDeal} - Купить продукт\n" +
        $"{Exit} - Выход\n"
    );

    public void RunMenu()
    {
        bool isRun = true;

        while(isRun)
        {
            ShowMenu();            

            switch(Console.ReadLine())
            {
                case SellerItems:
                    _seller.ShowInventory();
                break;

                case PlayerItems:
                    _player.ShowInventory();
                break;

                case MakeDeal:
                    Trade();
                break;

                case Exit:
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

    private void Trade()
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