class Program
{
    static void Main(string[] args)
    {
        Player player = new Player();

        Deck deck = new Deck();
        deck.ShuffleDeck();

        MenuHandler menuHandler = new MenuHandler(deck, player);
        menuHandler.RunMenu();
    }
}

class Player
{
    public Player() => playerCards = new List<Card>();

    public List<Card> playerCards {get; private set;}

    public void AddCardToDeck(Card newCard) => playerCards.Add(newCard);

    public void ShowDeck()
    {
        foreach(var card in playerCards)
            Console.WriteLine($"{card.Number} {card.Suit}");
    }
}

class Deck
{
    private List<string> _suits = new List<string>{"Червы", "Бубны", "Трефы", "Пики"};

    const int MinCardValue = 1;
    const int MaxCardValue = 10;

    public Deck()
    {
        CardsInDeck = new List<Card>();

        for(int i = MinCardValue; i <= MaxCardValue; i++)
        {
            foreach(var suit in _suits)
            {
                Card card = new Card(suit, i);
                CardsInDeck.Add(card);
            }
        }
    }

    public List<Card> CardsInDeck {get; private set;}

    public void ShuffleDeck()
    {
        Random random = new Random();

        for (int i = 0; i < CardsInDeck.Count - 1; i++)
        {
            int j = random.Next(i + 1, CardsInDeck.Count);

            Card temp = CardsInDeck[i];
            CardsInDeck[i] = CardsInDeck[j];
            CardsInDeck[j] = temp;
        }
    }

    public bool TryGetCard(out Card card)
    {
        bool isFound = false;
        card = null;

        if(CardsInDeck.Count != 0)
        {
            card = CardsInDeck[0];
            CardsInDeck.RemoveAt(0);
            isFound = true;
        }

        return isFound;
    }
}

class Card
{
    public Card(string inputSuit, int inputNumber)
    {
        Suit = inputSuit;
        Number = inputNumber;
    }

    public string Suit {get; private set;}
    public int Number {get; private set;}
}

class MenuHandler
{
    const string CommandAddCard = "add";
    const string CommandExit = "exit";
    const string CommandShuffleDeck = "shuffle";
    const string CommandStop = "stop";

    private Deck _deck;
    private Player _player;

    public MenuHandler(Deck deck, Player player)
    {
        _deck = deck;
        _player = player;
    }

    public void ShowMenu()
    {
        Console.WriteLine
        (
            $"{CommandAddCard} - Взять 1 карту\n" + 
            $"{CommandShuffleDeck} - перемешать коллоду\n" + 
            $"{CommandStop} - прекратить брать карты\n" + 
            $"{CommandExit} - Выход\n"
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
                case CommandAddCard:
                    AddCard();
                break;

                case CommandShuffleDeck:
                    ShuffleDeck();
                break;

                case CommandStop:
                    StopGame(ref isRun);
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

    private void AddCard()
    {
        if(_deck.TryGetCard(out Card card))
        {
            _player.AddCardToDeck(card);
            Console.WriteLine("Карта добавлена!");
        }
        else
        {
            Console.WriteLine("Карты закончились!");
        }
    }

    private void ShuffleDeck() => _deck.ShuffleDeck();

    private void StopGame(ref bool isRun)
    {
        isRun = false;
        Console.WriteLine("Всего карт в вашей коллоде:");
        _player.ShowDeck();
    }
}