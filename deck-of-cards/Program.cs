class Program
{
    static void Main(string[] args)
    {

    }
}

class Player
{

}

class Deck
{
    public Deck()
    {
        DeckOfCards = new List<Card>();
    }

    public List<Card> DeckOfCards {get; private set;} 
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