<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
    // Create a sample book with reviews
    Book book = new Book
    {
        Title = "Sample Book",
        Author = "John Doe",
        Reviews = new List<Review>
            {
                new Review { Reviewer = "Alice", Comment = "Great book!" },
                new Review { Reviewer = "Bob", Comment = "Interesting read." }
            }
    };

    // Configure dependencies and run the program
    IFileHandler<Book> fileHandler = new FileHandler<Book>();
    IBookRepository bookRepository = new BookRepository(fileHandler);

    bookRepository.Save(book);
    Book loadedBook = bookRepository.Load();

    // Display the loaded book details
    Console.WriteLine("Book Details:");
    Console.WriteLine("Title: " + loadedBook.Title);
    Console.WriteLine("Author: " + loadedBook.Author);
    Console.WriteLine("Reviews:");
    foreach (Review review in loadedBook.Reviews)
    {
        Console.WriteLine("Reviewer: " + review.Reviewer);
        Console.WriteLine("Comment: " + review.Comment);
        Console.WriteLine();
    }
}

// Interfaces
public interface IFileHandler<T>
{
    void SaveToFile(T obj, string filePath);
    T LoadFromFile(string filePath);
}

public interface IBookRepository
{
    void Save(Book book);
    Book Load();
}

// Classes
public class FileHandler<T> : IFileHandler<T>
{
    public void SaveToFile(T obj, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, obj);
        }
    }

    public T LoadFromFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(reader);
        }
    }
}

public class BookRepository : IBookRepository
{
    private readonly IFileHandler<Book> _fileHandler;
    private const string FilePath = "book.xml";

    public BookRepository(IFileHandler<Book> fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public void Save(Book book)
    {
        _fileHandler.SaveToFile(book, FilePath);
    }

    public Book Load()
    {
        return _fileHandler.LoadFromFile(FilePath);
    }
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public List<Review> Reviews { get; set; }
}

public class Review
{
    public string Reviewer { get; set; }
    public string Comment { get; set; }
}
