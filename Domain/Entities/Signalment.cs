using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public record Signalment(
    string? Species,
    string? Breed, 
    string? Gender, 
    bool? Sterilize, 
    string? Age, 
    string? Weight
);