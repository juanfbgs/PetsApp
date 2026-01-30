using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetsApp.Models;

namespace PetsApp.Data.Configurations;

public class PetConfiguration: IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        // Primary Key
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(p => p.Breed)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Age)
            .IsRequired();
        
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(255);
        
        builder.Ignore(p => p.ImageFile);
        
        // Relationship Configuration (One-to-Many)
        builder.HasOne(p => p.ApplicationUser)
            .WithMany(u => u.Pets)
            .HasForeignKey(p => p.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
