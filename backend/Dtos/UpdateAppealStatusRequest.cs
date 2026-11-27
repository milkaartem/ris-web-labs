using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

/// <summary>
/// Входной контракт для изменения статуса жизненного цикла обращения.
/// </summary>
public class UpdateAppealStatusRequest
{
    [Required(ErrorMessage = "Укажите новый статус обращения")]
    public string Status { get; set; } = string.Empty;
}
