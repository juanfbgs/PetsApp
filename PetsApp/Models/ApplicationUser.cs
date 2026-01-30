using Microsoft.AspNetCore.Identity;

namespace PetsApp.Models;

public class ApplicationUser: IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName {get; set;}

    public ICollection<Pet>? Pets { get; set; }
}
