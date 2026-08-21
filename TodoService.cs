using ToDoApiAndWebClient.Model.Model;

namespace ToDoApiAndWebClient.Model;

public class TodoService
{
    private static List<TodoItem> Todos = new List<TodoItem>
    {
        new() { Id = 1, Text = "Handle melk", IsDone = false },
        new() { Id = 2, Text = "Svare på e-post", IsDone = true }
    };

    public static List<TodoItem> GetAll()
    {
        return Todos;
    }

    public static object GetMichal()
    {
        return new { FirstName = "Michal", LastName = "Szczepanski" };
    }
    
}