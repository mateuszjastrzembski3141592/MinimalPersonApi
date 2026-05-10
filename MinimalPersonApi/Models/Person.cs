namespace MinimalPersonApi.Models;

public class Person
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Address { get; set; }
}
