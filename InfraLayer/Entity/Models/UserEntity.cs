using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraLayer.Entity.Models;

[Table("User")]
public class UserEntity
{
    [Key]
    public required string Id { get; set; }

    [Required]
    public required string Account { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required DateTime CreatedAt { get; set; }
}
