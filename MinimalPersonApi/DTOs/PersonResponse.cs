namespace MinimalPersonApi.DTOs;

public class PersonResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string Address { get; init; }
}
