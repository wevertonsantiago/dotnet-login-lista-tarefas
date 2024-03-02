using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace entities;

public class UserEntity
{
    [Key]
    [Column("id")]
    [JsonIgnore]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    [EmailAddress]
    [Column("email")]
    [Required]
    public string? Email { get; set; }

    [Column("password")]
    [Required]
    public string? Password { get; set; }

    [Column("date_create")]
    [JsonIgnore]
    public DateTime DateCreate { get; set; }

    [Column("date_upgrade")]
    [JsonIgnore]
    public DateTime DateUpgrade { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpirytime { get; set; }

    public ICollection<TaskEntity> Tasks { get; set; } = new Collection<TaskEntity>();

}
