using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

/// <summary>
/// Входной контракт для назначения ответственного сотрудника деканата.
/// </summary>
public class AssignAppealRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Укажите положительный идентификатор исполнителя (assigneeUserId)")]
    public int AssigneeUserId { get; set; }
}
