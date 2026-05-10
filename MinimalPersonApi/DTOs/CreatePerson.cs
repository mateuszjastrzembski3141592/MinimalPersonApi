namespace MinimalPersonApi.DTOs;

public class CreatePerson
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Address { get; set; }
}
