using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace КТ12.Models;

public class UserProfile
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Введите имя")]
    [StringLength(50)]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите фамилию")]
    [StringLength(50)]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Дата рождения")]
    public DateTime? DateOfBirth { get; set; }

    [Phone]
    [StringLength(20)]
    [Display(Name = "Телефон")]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    [Display(Name = "Адрес")]
    public string? Address { get; set; }

    [StringLength(1000)]
    [Display(Name = "О себе")]
    public string? Bio { get; set; }

    [Url]
    [StringLength(300)]
    [Display(Name = "Ссылка на аватар")]
    public string? AvatarUrl { get; set; }

    [NotMapped]
    public string FullName => $"{LastName} {FirstName}".Trim();

    public User User { get; set; } = null!;
}
