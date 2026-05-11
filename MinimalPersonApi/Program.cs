using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalPersonApi.Data;
using MinimalPersonApi.DTOs;
using MinimalPersonApi.Models;
using System.Net;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PersonDbContext>(options => options.UseInMemoryDatabase("PersonsBase"));

var app = builder.Build();

app.MapGet("/persons", async (PersonDbContext personDbContext) =>
{
    var db = personDbContext.Persons;

    var persons = await db
        .Select(p => new PersonResponse()
        {
            Id = p.Id,
            Name = p.Name,
            Surname = p.Surname,
            DateOfBirth = p.DateOfBirth,
            Address = p.Address
        })
        .ToListAsync();

    return Results.Ok(persons);
});

app.MapGet("/persons/{id}", async (int id, PersonDbContext personDbContext) =>
{
    var db = personDbContext.Persons;

    var person = await db
        .Where(p => p.Id == id)
        .Select(p => new PersonResponse()
        {
            Id = p.Id,
            Name = p.Name,
            Surname = p.Surname,
            DateOfBirth = p.DateOfBirth,
            Address = p.Address
        })
        .FirstOrDefaultAsync();

    if (person is null)
    {
        return Results.NotFound($"Person with id: {id} does not exist.");
    }

    return Results.Ok(person);

});

app.MapPost("/persons", async (CreatePerson newPerson, PersonDbContext personDbContext) =>
{
    var db = personDbContext.Persons;

    if (string.IsNullOrWhiteSpace(newPerson.Name) ||
        string.IsNullOrWhiteSpace(newPerson.Surname) ||
        string.IsNullOrWhiteSpace(newPerson.Address))
    {
        return Results.BadRequest("Name, Surname, and Address fields cannot be empty or consist of only white spaces.");
    }

    if (newPerson.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
    {
        return Results.BadRequest("Date of birth can't be a future date.");
    }

    newPerson.Name = newPerson.Name.Trim();
    newPerson.Surname = newPerson.Surname.Trim();
    newPerson.Address = newPerson.Address.Trim();

    if (await db.AnyAsync(p =>
        p.Name == newPerson.Name &&
        p.Surname == newPerson.Surname &&
        p.DateOfBirth == newPerson.DateOfBirth))
    {
        return Results.Conflict("This person already exists in the database.");
    }

    var person = new Person()
    {
        Name = newPerson.Name,
        Surname = newPerson.Surname,
        DateOfBirth = newPerson.DateOfBirth,
        Address = newPerson.Address
    };

    db.Add(person);
    await personDbContext.SaveChangesAsync();

    var createdPerson = new PersonResponse()
    {
        Id = person.Id,
        Name = person.Name,
        Surname = person.Surname,
        DateOfBirth = person.DateOfBirth,
        Address = person.Address
    };

    return Results.Created($"/persons/{createdPerson.Id}", createdPerson);
});

app.Run();
