using System.ComponentModel.DataAnnotations;

namespace PetsApp.Dtos;

public record PetCreateDto(
    [Required][StringLength(100)] string Name,
    [Required] string Description,
    [Range(0, 100)] int Age,
    [Required] string Breed,
    [Display(Name = "Pet Photo")] IFormFile? ImageFile
);