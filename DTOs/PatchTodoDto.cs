namespace ToDoApiAndWebClient.Model.DTOs;

public class PatchTodoDto
{
    public string? Text { get; set; }
    public bool? IsDone { get; set; }
}