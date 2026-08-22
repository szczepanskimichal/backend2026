namespace ToDoApiAndWebClient.Model.DTOs;

public class UpdateTodoDto
{
    public string Text { get; set; } = "";
    public bool IsDone { get; set; }
}