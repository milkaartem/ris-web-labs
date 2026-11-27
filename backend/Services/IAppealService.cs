using Backend.Dtos;

namespace Backend.Services;

public enum AppealOperationStatus
{
    Success,
    NotFound,
    ValidationError,
    Conflict
}

public record AppealOperationResult<T>(
    AppealOperationStatus Status,
    T? Data = default,
    string? ErrorMessage = null);

/// <summary>
/// Контракт сервиса управления обращениями студентов.
/// </summary>
public interface IAppealService
{
    IReadOnlyList<AppealResponse> GetAll(string? status = null, int? serviceTypeId = null);
    AppealResponse? GetById(int id);
    AppealResponse Create(CreateAppealRequest request, int createdByUserId);
    AppealOperationResult<AppealResponse> Assign(int id, int assigneeUserId);
    AppealOperationResult<AppealResponse> UpdateStatus(int id, string nextStatus, int? actorUserId);
}
