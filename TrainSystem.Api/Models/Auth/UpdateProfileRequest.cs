using System.ComponentModel.DataAnnotations;

namespace TrainSystem.Api.Models.Auth;

public class UpdateProfileRequest
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
}
