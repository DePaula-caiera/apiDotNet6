using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProdutoRepository.Init(configuration);

// QUANDO REINICIAR O PROJETO, SALVAR OS ITENS NOVAMENTE (REALIZAR O POST)
// CREATE
app.MapPost("/produtos", (ProdutoRequest produtoRequest, ApplicationDbContext context) => {
    var categoria = context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).First();
    var produto = new Produto{
        Codigo = produtoRequest.Codigo,
        Nome = produtoRequest.Nome,
        Descricao = produtoRequest.Descricao,
        Categoria = categoria
    };

    if(produtoRequest.Tags != null)
    {
        produto.Tags = new List<Tag>();
        foreach(var item in produtoRequest.Tags)
        {
            produto.Tags.Add(new Tag{ Nome = item });
        }
    }
    context.Produtos?.Add(produto);
    context.SaveChanges();
    return Results.Created($"/produtos/{produto.Id}", produto.Id);
});


//SELECT/ID
//api.app.com/user/{code}
app.MapGet("/produtos/{id}", ([FromRoute] int id, ApplicationDbContext context) =>{
    var produto = context.Produtos?
    .Include(p => p.Categoria)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    if(produto != null)
    {    
        return Results.Ok(produto);
    }
    return Results.NotFound();
});

//UPDATE/ID
app.MapPut("/produtos/{id}", ([FromRoute] int id, ProdutoRequest produtoRequest, ApplicationDbContext context) => {
    var produto = context.Produtos?
        .Include(p => p.Tags)
        .Where(p => p.Id == id).First();
    var categoria = context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).First();    
   
   produto.Codigo = produtoRequest.Codigo;
   produto.Nome = produtoRequest.Nome;
   produto.Descricao = produtoRequest.Descricao;
   produto.Categoria = categoria;
   produto.Tags = new List<Tag>();

    if(produtoRequest.Tags != null)
    {
        produto.Tags = new List<Tag>();
        foreach(var item in produtoRequest.Tags)
        {
            produto.Tags.Add(new Tag{ Nome = item });
        }
    }

   context.SaveChanges(); 
   return Results.Ok();
});

//DELETE/id
app.MapDelete("/produtos/{id}", ([FromRoute] int id, ApplicationDbContext context) => {
    var produto = context.Produtos.Where(p => p.Id == id).First();
    context.Produtos.Remove(produto);  

    context.SaveChanges();    
    return Results.Ok();
});

app.MapGet("/configuration/database", (IConfiguration configuration) => {
    return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");

});

app.Run();

