namespace PetsApp.Models;

public class Pet
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public int Age { get; set; }

    public required string Breed { get; set; }

    public string? ImageUrl { get; set; }

    public IFormFile? ImageFile { get; set; }

    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
