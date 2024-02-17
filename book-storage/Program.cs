class Program
{
    static void Main(string[] agrs)
    {
        Storage storage = new Storage();
        MenuHandler menuHandler = new MenuHandler(storage);
        menuHandler.RunMenu();
    }
}

class Book
{
    public Book(string inputName, string inputAuthor, int inputYear)
    {
        Name = inputName;
        Author = inputAuthor;
        Year = inputYear;
    }

    public string Name {get; private set;}
    public string Author {get; private set;}
    public int Year {get; private set;}
}

class Storage
{
    public Storage() => Books = new List<Book>();

    public List<Book> Books {get; private set;}

    public void AddBook(Book book) => Books.Add(book);

    public void RemoveBook(Book book) => Books.Remove(book);

    public void GetBooks()
    {
        Console.WriteLine("Название\tАвтор\tГод выпуска");

        foreach(var book in Books)
            Console.WriteLine($"{book.Name}\t\t{book.Author}\t\t{book.Year}");
    }

    public bool TryGetBook(string bookName, string bookAuthor, int bookYear, out Book book)
    {
        bool isFound = false;
        book = null;

        foreach(var storedBook in Books)
        {
            if(storedBook.Name == bookName && storedBook.Author == bookAuthor && storedBook.Year == bookYear)
            {
                book = storedBook;
                isFound = true;
            }
        }
        
        return isFound;
    }
}

class MenuHandler
{
    const string CommandAddBook = "add";
    const string CommandRemoveBook = "remove";
    const string CommandGetBook = "get";
    const string CommandGetBooks = "show";
    const string CommandExit = "exit";
    const string CommandGetBookByName = "name";
    const string CommandGetBookByAuthor = "author";
    const string CommandGetBookByYear = "year";

    private Storage _books;

    public MenuHandler(Storage inputBooks) => _books = inputBooks;

    public void ShowMenu() => Console.WriteLine
    (
        $"{CommandAddBook} - Добавить книгу\n" + 
        $"{CommandRemoveBook} - Убрать книгу\n" + 
        $"{CommandGetBook} - Найти книгу\n" +
        $"{CommandGetBooks} - Вывести все книги\n" + 
        $"{CommandExit} - Выход"
    );

    public void RunMenu()
    {
        bool isRun = true;

        while(isRun)
        {
            ShowMenu();

            switch(Console.ReadLine())
            {
                case CommandAddBook:
                    AddBook();
                break;

                case CommandRemoveBook:
                    RemoveBook();
                break;

                case CommandGetBook:
                    GetBook();
                break;

                case CommandGetBooks:
                    _books.GetBooks();
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

    private void InputBookInfo(out string bookName, out string bookAuthor, out int bookYear)
    {
        Console.Write("Название книги: ");
        bookName = Console.ReadLine();

        Console.Write("\nАвтор книги: ");
        bookAuthor = Console.ReadLine();

        Console.Write("\nГод выпуска книги: ");
        bookYear = ReadPositiveNumber();
    }

    private int ReadPositiveNumber()
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

    private void AddBook()
    {
        InputBookInfo(out string bookName, out string bookAuthor, out int bookYear);
        Book book = new Book(bookName, bookAuthor, bookYear);
        _books.AddBook(book);
    }

    private void RemoveBook()
    {
        InputBookInfo(out string bookName, out string bookAuthor, out int bookYear);

        if(_books.TryGetBook(bookName, bookAuthor, bookYear, out Book book))
        {
            _books.RemoveBook(book);
        }
        else
        {
            Console.WriteLine("Такой книги нет!");
        }
    }

    private void GetBook()
    {
        ShowGetBookMenu();

        switch(Console.ReadLine())
        {
            case CommandGetBookByName:
                GetBookByName();
            break;

            case CommandGetBookByAuthor:
                GetBookByAuthor();
            break;

            case CommandGetBookByYear:
                GetBookByYear();
            break;

            default:
                Console.WriteLine("Неверная команда!");
            break;
        }
    }

    private void ShowGetBookMenu() => Console.WriteLine
    (
        $"{CommandGetBookByName} - Найти по имени\n" + 
        $"{CommandGetBookByAuthor} - Найти по автору\n" + 
        $"{CommandGetBookByYear} - Найти по году"
    );

    private void GetBookByName()
    {
        Console.Write("Введите название книги: ");
        string bookName = Console.ReadLine();

        Console.WriteLine($"Найдены следующие книги с названием {bookName}:");

        foreach(var book in _books.Books)
            if(book.Name == bookName)
                Console.WriteLine($"{book.Name} {book.Author} {book.Year}");
    }

    private void GetBookByAuthor()
    {
        Console.Write("Введите автора: ");
        string bookAuthor = Console.ReadLine();

        Console.WriteLine($"Найдены следующие книги за авторством {bookAuthor}:");

        foreach(var book in _books.Books)
            if(book.Author == bookAuthor)
                Console.WriteLine($"{book.Name} {book.Author} {book.Year}");
    }

    private void GetBookByYear()
    {
        Console.Write("Введите год выпуска: ");
        int bookYear = ReadPositiveNumber();

        Console.WriteLine($"Найдены следующие книги года выпуска {bookYear}:");

        foreach(var book in _books.Books)
            if(book.Year == bookYear)
                Console.WriteLine($"{book.Name} {book.Author} {book.Year}");
    }
}