using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace entities;

public class TaskEntity
{
    [Key]
    [Column("id")]
    [JsonIgnore]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    [Column("title")]
    [Required]
    [StringLength(100)]
    public string? Title { get; set; }

    [Column("description")]
    [Required]
    public string? Description { get; set; }

    [Column("date_create")]
    [JsonIgnore]
    public DateTime DateCreate { get; set; }

    [Column("date_upgrade")]
    [JsonIgnore]
    public DateTime DateUpgrade { get; set; }


    [ForeignKey("UserId")]
    [JsonIgnore]
    public string UserId { get; init; }
    [JsonIgnore]
    public UserEntity? User { get; set; }

}
