class Program
{
    static void Main(string[] args)
    {
        Deck defaultDeck = new Deck();
        defaultDeck.ShuffleDeck();

        foreach(var card in defaultDeck.DeckOfCards)
        {
            Console.WriteLine(card.Number + " " + card.Suit);
        }
    }
}

class Player
{
    public Player() => playerDeck = new List<Card>();

    public List<Card> playerDeck {get; private set;}

    public void AddCardToDeck(Card newCard) => playerDeck.Add(newCard);
}

class Deck
{
    private List<string> _suits = new List<string>{"Червы", "Бубны", "Трефы", "Пики"};

    const int MinCardValue = 1;
    const int MaxCardValue = 10;

    public Deck()
    {
        DeckOfCards = new List<Card>();

        for(int i = MinCardValue; i <= MaxCardValue; i++)
        {
            foreach(var suit in _suits)
            {
                Card card = new Card(suit, i);
                DeckOfCards.Add(card);
            }
        }
    }

    public List<Card> DeckOfCards {get; private set;}

    public void ShuffleDeck()
    {
        Random random = new Random();

        for (int i = 0; i < DeckOfCards.Count - 1; i++)
        {
            int j = random.Next(i + 1, DeckOfCards.Count);

            Card temp = DeckOfCards[i];
            DeckOfCards[i] = DeckOfCards[j];
            DeckOfCards[j] = temp;
        }
    }

    public Card ExtractCard()
    {
        Card extractractedCard = DeckOfCards[0];
        DeckOfCards.RemoveAt(0);
        
        return extractractedCard;
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