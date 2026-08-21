using ToDoApiAndWebClient.Model;
using ToDoApiAndWebClient.Model.DTOs;
using ToDoApiAndWebClient.Model.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer(); // crucial for minimal API to discover endtpoins
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var todos = new List<TodoItem>
{
    new() { Id = 1, Text = "Handle melk", IsDone = false },
    new() { Id = 2, Text = "Svare på e-post", IsDone = true }
};
app.MapGet("/todos", () =>
{
    return todos;
});
app.MapGet("/todos/{id}", (int id) =>
{
    return todos.FirstOrDefault(todos => todos.Id == id);
});
app.MapGet("/michal", () => 
{git init

    return new { FirstName = "Michal", LastName = "Szczepanski" };
});
app.MapPost("/todos", (CreateTodoDTO createTodo) =>
{
    var id = todos.Count == 0 ? 1 : todos.Max(todo => todo.Id) + 1; 
    var todoItem = new TodoItem()
    {
        Id = id,
        Text = createTodo.Text,
        IsDone = false,
    };
    todos.Add(todoItem);
    return todoItem;
});

// app.MapGet("/todos", () => TodoService.GetAll());
// app.MapGet("/michal", () => TodoService.GetMichal());


app.Run();