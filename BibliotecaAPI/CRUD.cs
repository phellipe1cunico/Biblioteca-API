using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("LivrosDB"));
var app = builder.Build();

app.MapGet("/livros/{id}", async (int id, AppDbContext db) =>
{
    var livro = await db.Livros.FindAsync(id);
    return livro is not null ? Results.Ok(livro) : Results.NotFound("Livro não encontrado!");
});

app.MapPost("/livros", async (Livro livro, AppDbContext db) =>
{
    db.Livros.Add(livro);
    await db.SaveChangesAsync();
    return Results.Created($"/livros/{livro.Id}", livro);
});

app.MapPut("/livros/{id}", async (int id, Livro updatedLivro, AppDbContext db) =>
{
    var existing = await db.Livros.FindAsync(id);
    if (existing == null)
        return Results.NotFound("Livro não encontrado!");

    existing.Titulo = updatedLivro.Titulo;
    existing.Autor = updatedLivro.Autor;
    existing.Ano = updatedLivro.Ano;

    await db.SaveChangesAsync();
    return Results.Ok(existing);
});

app.MapDelete("/livros/{id}", async (int id, AppDbContext db) =>
{
    var livro = await db.Livros.FindAsync(id);
    if (livro == null)
        return Results.NotFound("Livro não encontrado!");

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();
    return Results.Ok("Livro deletado com sucesso!");
});

app.Run();
