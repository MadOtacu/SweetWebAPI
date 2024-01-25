using Microsoft.EntityFrameworkCore;
using SweetWebAPI;
using SweetWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

string connection = "Server=(localdb)\\mssqllocaldb;Database=SweetWebAPIDB;Trusted_Connection=True;";
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/stuff", async (Stuff stuff, ApplicationContext db) => 
{
    await db.AllStuff.AddAsync(stuff);
    await db.SaveChangesAsync();
    return stuff;
});

app.MapDelete("/api/stuff/{id:int}", async (int id, ApplicationContext db) =>
{
    Stuff? stuff = await db.AllStuff.FirstOrDefaultAsync(x => x.Id == id);

    if (stuff == null) return Results.NotFound(new {message = "Товар не найден!"});

    db.AllStuff.Remove(stuff);
    await db.SaveChangesAsync();
    return Results.Json(stuff);
});

app.MapGet("/api/stuff", async (ApplicationContext db) => await db.AllStuff.ToListAsync());

app.MapPost("/api/order", async (Order order, ApplicationContext db) =>
{
    int stuffId = order.StuffId;
    Stuff? stuff = await db.AllStuff.FirstOrDefaultAsync(x => x.Id == stuffId);
    if (stuff == null) return Results.NotFound(new { message = "Товар не найден!" });
    await db.Orders.AddAsync(order);
    await db.SaveChangesAsync();
    return Results.Json(order);
});

app.MapDelete("/api/order/{id:int}", async (int id, ApplicationContext db) =>
{
    Order? order = await db.Orders.FirstOrDefaultAsync(x => x.Id == id);

    if (order == null) return Results.NotFound(new {message = "Заказ не найден!"});

    db.Orders.Remove(order);
    await db.SaveChangesAsync();
    return Results.Json(order);
});

app.MapGet("api/order", async (ApplicationContext db) => await db.Orders.ToListAsync());

app.Run();
