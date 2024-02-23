class Program
{
    static void Main(string[] args)
    {
        Supermarket supermarket = new Supermarket();
        supermarket.Work();
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
    public int GetNumberOfProducts => _products.Count;

    public int GetAmountOfProducts()
    {
        int amountOfProducts = 0;

        foreach(var product in _products)
            amountOfProducts += product.Price;

        return amountOfProducts;
    }

    public void RemoveRandomProduct()
    {
        Random random = new Random();
        int minProductIndex = 0;
        int maxProductIndex = _products.Count;
            
        _products.RemoveAt(random.Next(minProductIndex, maxProductIndex));
    }

    public bool CanPay() => Money >= GetAmountOfProducts();

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
    private List<Product> _shelvesWithProducts;

    public Supermarket()
    {
        _clients  = new Queue<Client>();
        _shelvesWithProducts = new List<Product>();
        
        FillShelves();
        AddClients();
    }

    public void Work()
    {
        while(_clients.Count > 0)
        {
            Client client = _clients.Dequeue();
            int amountOfProducts = client.GetAmountOfProducts();
            Console.WriteLine($"Клиент пришёл на кассу\nОбщая сумма его товаров: {amountOfProducts}\tДенег у клиента: {client.Money}");

            while(client.CanPay() == false)
            {
                client.RemoveRandomProduct();

                amountOfProducts = client.GetAmountOfProducts();
                Console.WriteLine($"Теперь товаров на сумму {amountOfProducts}");
            }

            client.PayProduct(amountOfProducts);
            Console.WriteLine("Все товары оплачены!\n");
        }
    }

    private void FillShelves()
    {
        _shelvesWithProducts.Add(new Product("фарш", 100));
        _shelvesWithProducts.Add(new Product("молоко", 50));
        _shelvesWithProducts.Add(new Product("кола", 40));
        _shelvesWithProducts.Add(new Product("сыр", 150));
        _shelvesWithProducts.Add(new Product("корм для собак", 60));
        _shelvesWithProducts.Add(new Product("сырок", 30));
        _shelvesWithProducts.Add(new Product("чипсы", 65));
        _shelvesWithProducts.Add(new Product("пиво", 20));
        _shelvesWithProducts.Add(new Product("квас", 25));
    }

    private void AddClients()
    {
        int minNumberOfClients = 1;
        int maxNumberOfClients = 10;

        int minClientMoney = 0;
        int maxClientMoney = 500;

        Random random = new Random();
        int numberOfClients = random.Next(minNumberOfClients, maxNumberOfClients + 1);
        int clientMoney;
        
        for(int i = 0; i < numberOfClients; i++)
        {
            clientMoney = random.Next(minClientMoney, maxClientMoney + 1);
            
            _clients.Enqueue(new Client(clientMoney, FormClientCart(random)));
        }
    }

    private List<Product> FormClientCart(Random random)
    {
        List<Product> clientCart = new List<Product>();

        int minRandomProductIndex = 0;
        int maxRandomProductIndex = _shelvesWithProducts.Count;
        
        int minNumberOfProducts = 1;
        int maxNumberOfProducts = 5;
        
        int numberOfProducts = random.Next(minNumberOfProducts, maxNumberOfProducts + 1);

        for(int j = 0; j < numberOfProducts; j++)
        {
            int randomProductIndex = random.Next(minRandomProductIndex, maxRandomProductIndex);
            Product randomProduct = _shelvesWithProducts[randomProductIndex];
            clientCart.Add(new Product(randomProduct.Name, randomProduct.Price));
        }

        return clientCart;
    }
}