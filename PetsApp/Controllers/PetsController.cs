using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApp.Data;
using PetsApp.Dtos;
using PetsApp.Models;

namespace PetsApp.Controllers;

[Authorize]
public class PetsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public PetsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    // Helper to get Current User ID
    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: Pets
    public async Task<IActionResult> Index()
    {
        var myPets = await _context.Pets
            .Where(p => p.ApplicationUserId == CurrentUserId)
            .ToListAsync();

        return View(myPets);
    }

    // GET: Pets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var pet = await _context.Pets
            .FirstOrDefaultAsync(m => m.Id == id && m.ApplicationUserId == CurrentUserId);

        if (pet == null) return NotFound();

        return View(pet);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PetCreateDto petDto)
    {
        if (ModelState.IsValid)
        {
            var pet = new Pet
            {
                Name = petDto.Name,
                Description = petDto.Description,
                Age = petDto.Age,
                Breed = petDto.Breed,
                ApplicationUserId = CurrentUserId
            };

            if (petDto.ImageFile != null)
            {
                pet.ImageUrl = await SaveImage(petDto.ImageFile);
            }

            _context.Add(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(petDto);
    }

    // GET: Pets/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == CurrentUserId);
        if (pet == null) return NotFound();

        var petDto = new PetCreateDto(pet.Name, pet.Description, pet.Age, pet.Breed, null);
        ViewBag.ExistingImageUrl = pet.ImageUrl;
        return View(petDto);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PetCreateDto petDto)
    {
        var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == CurrentUserId);
        if (pet == null) return NotFound();

        if (ModelState.IsValid)
        {
            pet.Name = petDto.Name;
            pet.Description = petDto.Description;
            pet.Age = petDto.Age;
            pet.Breed = petDto.Breed;

            if (petDto.ImageFile != null)
            {
                DeleteImage(pet.ImageUrl);
                pet.ImageUrl = await SaveImage(petDto.ImageFile);
            }

            _context.Update(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(petDto);
    }

    // GET: Pets/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var pet = await _context.Pets.FirstOrDefaultAsync(m => m.Id == id && m.ApplicationUserId == CurrentUserId);
        if (pet == null) return NotFound();

        return View(pet);
    }

    // POST: Pets/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == CurrentUserId);

        if (pet != null)
        {
            DeleteImage(pet.ImageUrl);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // --- Helper Methods (Private) ---

    private async Task<string> SaveImage(IFormFile imageFile)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images", "pets");

        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        string filePath = Path.Combine(uploadPath, fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return "/images/pets/" + fileName;
    }

    private void DeleteImage(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        string filePath = Path.Combine(_hostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}