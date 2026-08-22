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
    return Results.Ok(todos);
});
app.MapGet("/todos/{id}", handler: (int id) =>
{
    var todoItem = todos.FirstOrDefault(todo => todo.Id == id);
    return todoItem == null ? Results.NotFound() : Results.Ok(todoItem);
});

// app.MapGet("/michal", () => 
// {
//     return new { FirstName = "Michal", LastName = "Szczepanski" };
// });
app.MapPost("/todos", (CreateTodoDTO dto) =>
{
    if(string.IsNullOrWhiteSpace(dto.Text))
    {
        return Results.BadRequest("Text cannot be empty");
    }
    var id = todos.Count == 0 ? 1 : todos.Max(todo => todo.Id) + 1; 
    var todo = new TodoItem()
    {
        Id = id,
        Text = dto.Text,
        IsDone = false,
    };
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", (int id, UpdateTodoDto dto) =>
{
    var todoItem = todos.FirstOrDefault(todo => todo.Id == id);
    if (todoItem == null)
    {
        return Results.NotFound();
    }
    if(string.IsNullOrWhiteSpace(dto.Text))
    {
        return Results.BadRequest("Text cannot be empty");
    }
    todoItem.Text = dto.Text;
    todoItem.IsDone = dto.IsDone;
    return Results.Ok(todoItem);
});

app.MapDelete("/todos/{id}", (int id) =>
{
    var todoItem = todos.FirstOrDefault(todo => todo.Id == id);
    if (todoItem == null)
    {
        return Results.NotFound();
    }
    todos.Remove(todoItem);
    return Results.NoContent();
});

app.MapPatch("/todos/{id}", (int id, PatchTodoDto dto) =>
{
    var todo = todos.FirstOrDefault(todo => todo.Id == id);

    if (todo == null)
    {
        return Results.NotFound();
    }

    if (dto.Text != null)
    {
        todo.Text = dto.Text;
    }

    if (dto.IsDone != null)
    {
        todo.IsDone = dto.IsDone.Value;
    }

    return Results.Ok(todo);
});

// app.MapGet("/todos", () => TodoService.GetAll());
// app.MapGet("/michal", () => TodoService.GetMichal());


app.Run();