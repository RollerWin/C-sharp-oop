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
    static private int s_currentId = 1;

    public Book(string name, string author, int year)
    {
        Id = s_currentId++;
        Name = name;
        Author = author;
        Year = year;
    }

    public int Id {get; private set;}
    public string Name {get; private set;}
    public string Author {get; private set;}
    public int Year {get; private set;}
}

class Storage
{
    private List<Book> _books;

    public Storage() => _books = new List<Book>();

    public List<Book> Books => new List<Book>(_books);

    public void AddBook(Book book) => _books.Add(book);

    public void RemoveBook(Book book) => _books.Remove(book);

    public void GetBooks()
    {
        Console.WriteLine("Id  Название\tАвтор\tГод выпуска");

        foreach(var book in _books)
            Console.WriteLine($"{book.Id}   {book.Name}\t\t{book.Author}\t\t{book.Year}");
    }

    public bool TryGetBook(int bookId, out Book book)
    {
        bool isFound = false;
        book = null;

        foreach(var storedBook in _books)
        {
            if(bookId == storedBook.Id)
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
    const string CommandGetBook = "find";
    const string CommandGetBooks = "show";
    const string CommandExit = "exit";
    const string CommandGetBookByName = "name";
    const string CommandGetBookByAuthor = "author";
    const string CommandGetBookByYear = "year";

    private Storage _storage;

    public MenuHandler(Storage storage) => _storage = storage;

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
                    SearchBook();
                break;

                case CommandGetBooks:
                    _storage.GetBooks();
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
        Console.Write("Название книги: ");
        string bookName = Console.ReadLine();

        Console.Write("\nАвтор книги: ");
        string bookAuthor = Console.ReadLine();

        Console.Write("\nГод выпуска книги: ");
        int bookYear = ReadPositiveNumber();

        Book book = new Book(bookName, bookAuthor, bookYear);
        _storage.AddBook(book);
    }

    private void RemoveBook()
    {
        Console.Write("Введите Id книги для удаления: ");
        int bookId = ReadPositiveNumber();
        Console.WriteLine();
        
        if(_storage.TryGetBook(bookId, out Book book))
        {
            _storage.RemoveBook(book);
        }
        else
        {
            Console.WriteLine("Такой книги нет!");
        }
    }

    private void SearchBook()
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

        foreach(var book in _storage.Books)
            if(book.Name == bookName)
                Console.WriteLine($"{book.Name} {book.Author} {book.Year}");
    }

    private void GetBookByAuthor()
    {
        Console.Write("Введите автора: ");
        string bookAuthor = Console.ReadLine();

        Console.WriteLine($"Найдены следующие книги за авторством {bookAuthor}:");

        foreach(var book in _storage.Books)
            if(book.Author == bookAuthor)
                Console.WriteLine($"{book.Name} {book.Author} {book.Year}");
    }

    private void GetBookByYear()
    {
        Console.Write("Введите год выпуска: ");
        int bookYear = ReadPositiveNumber();

        Console.WriteLine($"Найдены следующие книги года выпуска {bookYear}:");

        foreach(var book in _storage.Books)
            if(book.Year == bookYear)
                Console.WriteLine($"{book.Name} {book.Author} {book.Year}");
    }
}