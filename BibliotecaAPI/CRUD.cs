using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("LivrosDB"));
var app = builder.Build();

// GET por ID
app.MapGet("/livros/{id}", async (int id, AppDbContext db) =>
{
    var livro = await db.Livros.FindAsync(id);
    return livro is not null ? Results.Ok(livro) : Results.NotFound("Livro não encontrado!");
});

// POST (cadastrar livro)
app.MapPost("/livros", async (Livro livro, AppDbContext db) =>
{
    db.Livros.Add(livro);
    await db.SaveChangesAsync();
    return Results.Created($"/livros/{livro.Id}", livro);
});

app.Run();
