using System.ComponentModel.DataAnnotations;

namespace КТ12.ViewModels;

public class UserFormViewModel
{
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

    [Required(ErrorMessage = "Введите имя")]
    [StringLength(50)]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите фамилию")]
    [StringLength(50)]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Дата рождения")]
    public DateTime? DateOfBirth { get; set; }

    [Phone(ErrorMessage = "Некорректный номер телефона")]
    [StringLength(20)]
    [Display(Name = "Телефон")]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    [Display(Name = "Адрес")]
    public string? Address { get; set; }

    [StringLength(1000)]
    [Display(Name = "О себе")]
    public string? Bio { get; set; }

    [Url(ErrorMessage = "Некорректная ссылка")]
    [StringLength(300)]
    [Display(Name = "Ссылка на аватар")]
    public string? AvatarUrl { get; set; }
}
