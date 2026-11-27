using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

/// <summary>
/// Входной контракт для создания обращения.
/// Поля id, number, status и assigneeUserId задаются только сервером.
/// </summary>
public class CreateAppealRequest : IValidatableObject
{
    [Required(ErrorMessage = "Укажите тему обращения")]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Тема: от 5 до 80 символов")]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Укажите положительный номер категории услуги (serviceTypeId)")]
    public int ServiceTypeId { get; set; }

    [Required(ErrorMessage = "Укажите описание обращения")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание: от 10 до 500 символов")]
    public string Description { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var trimmedTitle = Title?.Trim() ?? string.Empty;
        if (trimmedTitle.Length < 5 || trimmedTitle.Length > 80)
        {
            yield return new ValidationResult(
                "Тема после обрезки пробелов должна содержать от 5 до 80 символов",
                new[] { nameof(Title) });
        }

        var trimmedDescription = Description?.Trim() ?? string.Empty;
        if (trimmedDescription.Length < 10 || trimmedDescription.Length > 500)
        {
            yield return new ValidationResult(
                "Описание после обрезки пробелов должно содержать от 10 до 500 символов",
                new[] { nameof(Description) });
        }
    }
}
