using Microsoft.EntityFrameworkCore;
using pr11;
using pr11.Services;
using pr11.Models;

var builder = WebApplication.CreateBuilder(args);

// Підключення до бази даних
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;"));

// Сервіс
builder.Services.AddScoped<BookService>();

// Контролери
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();


// ===== Додавання стартових даних =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    if (!context.Authors.Any())
    {
        context.Authors.AddRange(
            new Author { Name = "J.K. Rowling" },
            new Author { Name = "Robert Martin" }
        );
        context.SaveChanges();
    }

    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Name = "Fantasy" },
            new Category { Name = "Programming" }
        );
        context.SaveChanges();
    }

    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book
            {
                Title = "Harry Potter",
                Year = 2001,
                AuthorId = 1,
                CategoryId = 1
            },
            new Book
            {
                Title = "Clean Code",
                Year = 2008,
                AuthorId = 2,
                CategoryId = 2
            }
        );

        context.SaveChanges();
    }
}

app.Run();