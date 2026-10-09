using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace КТ12.Models;

[Index(nameof(Email), IsUnique = true)]   
public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите имя пользователя")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя пользователя: от 3 до 50 символов")]
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Дата регистрации")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserProfile? Profile { get; set; }
}
