class Program
{
    static void Main(string[] args)
    {
        Supermarket supermarket = new Supermarket();
        supermarket.WorkSupermarket();
    }
}

class Client
{
    private List<Product> _products;

    public Client(int money, List<Product> products)
    {
        Money = money;
        _products = products;
    } 

    public int Money {get; private set;}
    
    public int GetAmountOfProducts()
    {
        int amountOfProducts = 0;

        foreach(var product in _products)
            amountOfProducts += product.Price;

        return amountOfProducts;
    }

    public int GetNumberOfProducts() => _products.Count;

    public void RemoveProduct(int removedIndex) => _products.RemoveAt(removedIndex);

    public void PayProduct(int amountOfProducts) => Money -= amountOfProducts;
}

class Product
{
    public Product(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public string Name {get; private set;}
    public int Price {get; private set;}
}

class Supermarket
{
    private Queue<Client> _clients;

    public Supermarket()
    {
        _clients  = new Queue<Client>();

        _clients.Enqueue
        (
            new Client
            (
                500, 
                new List<Product>
                {
                    new Product("a", 50),
                    new Product("b", 100),
                    new Product("c", 40)
                }
            )
        );

        _clients.Enqueue
        (
            new Client
            (
                100,
                new List<Product>
                {
                    new Product("a", 20),
                    new Product("b", 100),
                    new Product("c", 40)
                }
            )
        );
    }

    public void WorkSupermarket()
    {
        while(_clients.Count > 0)
        {
            Client client = _clients.Dequeue();
            int amountOfProducts = client.GetAmountOfProducts();

            Console.WriteLine($"Клиент пришёл на кассу\nОбщая сумма его товаров: {amountOfProducts}");

            if(client.Money >= amountOfProducts)
            {
                client.PayProduct(amountOfProducts);
            }
            else
            {
                while(client.Money <= amountOfProducts)
                {
                    Random random = new Random();

                    int minProductIndex = 0;
                    int maxProductIndex = client.GetNumberOfProducts();
                    
                    client.RemoveProduct(random.Next(minProductIndex, maxProductIndex));
                    amountOfProducts = client.GetAmountOfProducts();

                    Console.WriteLine($"Теперь товаров на сумму {amountOfProducts}");
                }

                client.PayProduct(amountOfProducts);
            }

            Console.WriteLine("Все товары оплачены!");
        }
    }
}