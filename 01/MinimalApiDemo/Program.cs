using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "MinimalApi",
        Version = "v1"
    });
});


builder.Services.AddSingleton<IDateTimeService, DateTimeService>();

var app = builder.Build();

app.MapGet("helloworld", () => "Hello world!");

app.MapPost("hello", (string name) => "hello " + name);


app.MapPost("/Special/{age:int}", (int age, string name, Person p, HttpRequest request, [FromHeader] string accept, IDateTimeService service) => new
{
    age = age,
    name = name,
    Person = p,
    QueryString = request.Query,
    accept = accept,
    now = service.Now()
});


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalApi v1"));

app.Run();



class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
} 



interface IDateTimeService
{
    DateTime Now();
}

class DateTimeService : IDateTimeService
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}

