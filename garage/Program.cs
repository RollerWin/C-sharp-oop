class Program
{
    static void Main(string[] args)
    {

    }
}

class CarService
{
    public double Money {get; private set;}
}

class Detail
{
    public Detail(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public string Name {get; private set;}
    public double Price {get; private set;}
}

class DetailsTemplate
{
    private List<Detail> _details;

    public DetailsTemplate()
    {
        _details = new List<Detail>();
        AddDetails();
    }

    public Detail GetRandomDetail() => _details[UserUtils.GenerateRandomValue(0, _details.Count - 1)];

    private void AddDetails()
    {
        _details.Add(new Detail("Мотор", 200));
        _details.Add(new Detail("Тормоза", 100));
        _details.Add(new Detail("Фара", 25));
        _details.Add(new Detail("Колесо", 50));
        _details.Add(new Detail("Коробка передач", 75));
        _details.Add(new Detail("Бампер", 30));
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