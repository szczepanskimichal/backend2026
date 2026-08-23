using System.Text.Json;
using ToDoApiAndWebClient.Model.Model;
using ToDoApiAndWebClient.Model.DTOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var filename = "todos.json";

app.MapGet("/todos", async () =>
{
   if (!File.Exists(filename))
       return Results.Ok(Array.Empty<TodoItem>());

   var json = await File.ReadAllTextAsync(filename);
   var todos = JsonSerializer.Deserialize<TodoItem[]>(json) ?? Array.Empty<TodoItem>();
   return Results.Ok(todos);
});

app.MapGet("/todos/{id}", async (int id) =>
{
   if (!File.Exists(filename))
       return Results.NotFound();

   var json = await File.ReadAllTextAsync(filename);
   var todos = JsonSerializer.Deserialize<TodoItem[]>(json) ?? Array.Empty<TodoItem>();
   var todoItem = todos.FirstOrDefault(todo => todo.Id == id);

   return todoItem == null ? Results.NotFound() : Results.Ok(todoItem);
});

app.MapPost("/todos", async (CreateTodoDTO dto) =>
{
   if (string.IsNullOrWhiteSpace(dto.Text))
       return Results.BadRequest("Text cannot be empty.");

   var todos = new List<TodoItem>();

   if (File.Exists(filename))
    {
       var json = await File.ReadAllTextAsync(filename);
       var existingTodos = JsonSerializer.Deserialize<List<TodoItem>>(json);
       if (existingTodos != null)
           todos = existingTodos;
   }

   var id = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;
   var todo = new TodoItem
   {
       Id = id,
       Text = dto.Text,
       IsDone = false
   };

   todos.Add(todo);
   await File.WriteAllTextAsync(filename, JsonSerializer.Serialize(todos));

   return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", async (int id, UpdateTodoDto dto) =>
{
   if (string.IsNullOrWhiteSpace(dto.Text))
       return Results.BadRequest("Text cannot be empty.");

   if (!File.Exists(filename))
       return Results.NotFound();

   var json = await File.ReadAllTextAsync(filename);
   var todos = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
   var todoItem = todos.FirstOrDefault(todo => todo.Id == id);

   if (todoItem == null)
       return Results.NotFound();

   todoItem.Text = dto.Text;
   todoItem.IsDone = dto.IsDone;

   await File.WriteAllTextAsync(filename, JsonSerializer.Serialize(todos));
   return Results.Ok(todoItem);
});

app.MapDelete("/todos/{id}", async (int id) =>
{
   if (!File.Exists(filename))
       return Results.NotFound();

   var json = await File.ReadAllTextAsync(filename);
   var todos = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
   var todoItem = todos.FirstOrDefault(todo => todo.Id == id);

   if (todoItem == null)
       return Results.NotFound();

   todos.Remove(todoItem);
   await File.WriteAllTextAsync(filename, JsonSerializer.Serialize(todos));
   return Results.NoContent();
});

app.MapPatch("/todos/{id}", async (int id, PatchTodoDto dto) =>
{
   if (!File.Exists(filename))
       return Results.NotFound();

   var json = await File.ReadAllTextAsync(filename);
   var todos = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
   var todo = todos.FirstOrDefault(todo => todo.Id == id);

   if (todo == null)
       return Results.NotFound();

   if (!string.IsNullOrWhiteSpace(dto.Text))
       todo.Text = dto.Text;

   if (dto.IsDone.HasValue)
       todo.IsDone = dto.IsDone.Value;

   await File.WriteAllTextAsync(filename, JsonSerializer.Serialize(todos));
   return Results.Ok(todo);
});

app.Run();
