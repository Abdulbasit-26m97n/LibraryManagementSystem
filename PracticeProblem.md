Great! Here's a **practice problem** to help you implement **Clean Architecture** using your current approach (**Core instead of Domain, without CQRS/MediatR**).  

---

## **📝 Practice Problem: "Library Management System"**
### **Scenario**  
You are building a **Library Management System** where librarians can **add, update, delete, and search for books**. Each book has the following details:  
- **Title**  
- **Author**  
- **ISBN (Unique)**  
- **Publication Year**  
- **Availability Status (Available/Borrowed)**  

Librarians should be able to:  
✅ **Register a new book** in the system.  
✅ **Update book details** (e.g., change title, author, or availability status).  
✅ **Delete a book** from the system.  
✅ **Search books** by title, author, or ISBN.  

---

## **📂 Solution Structure (Without CQRS & MediatR)**
```
📂 LibraryManagementSystem.sln
   ├── 📂 LibraryManagement.Presentation (API)
   │        → Contains Controllers (e.g., `BookController.cs`)
   │
   ├── 📂 LibraryManagement.Application (Business Logic)
   │        → Contains Services (e.g., `BookService.cs`)
   │
   ├── 📂 LibraryManagement.Core (Entities & Interfaces)
   │        → Contains Entities (e.g., `Book.cs`)
   │        → Contains Interfaces (e.g., `IBookRepository.cs`)
   │
   ├── 📂 LibraryManagement.Infrastructure (Database & Repositories)
   │        → Implements `IBookRepository` (e.g., `BookRepository.cs`)
   │        → Uses EF Core for database access
```

---

## **🛠️ Steps to Implement**
### **1️⃣ Define `Book` Entity in Core**
```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; } // Unique
    public int PublicationYear { get; set; }
    public bool IsAvailable { get; set; } = true;
}
```

---

### **2️⃣ Define `IBookRepository` Interface in Core**
```csharp
public interface IBookRepository
{
    Task<Book> GetByISBNAsync(string isbn);
    Task<IEnumerable<Book>> SearchBooksAsync(string query);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
}
```

---

### **3️⃣ Implement `BookRepository` in Infrastructure**
```csharp
public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Book> _books;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
        _books = _context.Set<Book>();
    }

    public async Task<Book> GetByISBNAsync(string isbn) => 
        await _books.FirstOrDefaultAsync(b => b.ISBN == isbn);

    public async Task<IEnumerable<Book>> SearchBooksAsync(string query) =>
        await _books.Where(b => b.Title.Contains(query) || b.Author.Contains(query)).ToListAsync();

    public async Task AddAsync(Book book)
    {
        _books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _books.FindAsync(id);
        if (book != null)
        {
            _books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
```

---

### **4️⃣ Implement `BookService` in Application**
```csharp
public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task AddBookAsync(Book book)
    {
        var existingBook = await _bookRepository.GetByISBNAsync(book.ISBN);
        if (existingBook != null)
            throw new InvalidOperationException("A book with this ISBN already exists.");
        
        await _bookRepository.AddAsync(book);
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string query) => 
        await _bookRepository.SearchBooksAsync(query);
}
```

---

### **5️⃣ Create `BookController` in Presentation**
```csharp
[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] Book book)
    {
        await _bookService.AddBookAsync(book);
        return Ok("Book added successfully.");
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchBooks([FromQuery] string query)
    {
        var books = await _bookService.SearchBooksAsync(query);
        return Ok(books);
    }
}
```

---

### **6️⃣ Register Dependencies in `Program.cs`**
```csharp
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookService>();
```

---

## **🔹 Your Task:**
1️⃣ Implement all CRUD operations (**Add, Update, Delete, Search Books**).  
2️⃣ Use **Clean Architecture** principles (**Infrastructure does not reference Application**).  
3️⃣ Write a **unit test** for `BookService` using **Moq** (optional).  

Would you like me to **review your implementation** when you're done? 🚀