using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

/// <summary>
/// Реализация правил процесса и хранения обращений в оперативной памяти (Singleton).
/// </summary>
public class AppealService : IAppealService
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.Ordinal)
    {
        "New",
        "InProgress",
        "Closed",
        "Cancelled"
    };

    private readonly List<Appeal> _appeals = new();
    private readonly object _lock = new();
    private int _nextId = 1;

    public IReadOnlyList<AppealResponse> GetAll(string? status = null, int? serviceTypeId = null)
    {
        lock (_lock)
        {
            IEnumerable<Appeal> query = _appeals;

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => string.Equals(a.Status, status.Trim(), StringComparison.Ordinal));
            }

            if (serviceTypeId.HasValue)
            {
                query = query.Where(a => a.ServiceTypeId == serviceTypeId.Value);
            }

            return query.Select(ToResponse).ToList();
        }
    }

    public AppealResponse? GetById(int id)
    {
        lock (_lock)
        {
            var appeal = _appeals.FirstOrDefault(a => a.Id == id);
            return appeal is null ? null : ToResponse(appeal);
        }
    }

    public AppealResponse Create(CreateAppealRequest request, int createdByUserId)
    {
        lock (_lock)
        {
            var id = _nextId++;
            var now = DateTime.UtcNow;

            var appeal = new Appeal
            {
                Id = id,
                Number = $"ОБР-2026-{id:D4}",
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                ServiceTypeId = request.ServiceTypeId,
                Status = "New",
                CreatedByUserId = createdByUserId,
                AssigneeUserId = null,
                CreatedAt = now,
                UpdatedAt = now
            };

            _appeals.Add(appeal);
            return ToResponse(appeal);
        }
    }

    public AppealOperationResult<AppealResponse> Assign(int id, int assigneeUserId)
    {
        lock (_lock)
        {
            var appeal = _appeals.FirstOrDefault(a => a.Id == id);
            if (appeal is null)
            {
                return new AppealOperationResult<AppealResponse>(
                    AppealOperationStatus.NotFound,
                    ErrorMessage: $"Обращение с id={id} не найдено.");
            }

            if (appeal.Status is "Closed" or "Cancelled")
            {
                return new AppealOperationResult<AppealResponse>(
                    AppealOperationStatus.Conflict,
                    ErrorMessage: "Нельзя назначить исполнителя для завершённого или отменённого обращения.");
            }

            appeal.AssigneeUserId = assigneeUserId;
            appeal.UpdatedAt = DateTime.UtcNow;

            return new AppealOperationResult<AppealResponse>(
                AppealOperationStatus.Success,
                Data: ToResponse(appeal));
        }
    }

    public AppealOperationResult<AppealResponse> UpdateStatus(int id, string nextStatus, int? actorUserId)
    {
        var normalizedStatus = nextStatus?.Trim() ?? string.Empty;
        if (!AllowedStatuses.Contains(normalizedStatus))
        {
            return new AppealOperationResult<AppealResponse>(
                AppealOperationStatus.ValidationError,
                ErrorMessage: "Недопустимый код статуса. Разрешены только: New, InProgress, Closed, Cancelled.");
        }

        lock (_lock)
        {
            var appeal = _appeals.FirstOrDefault(a => a.Id == id);
            if (appeal is null)
            {
                return new AppealOperationResult<AppealResponse>(
                    AppealOperationStatus.NotFound,
                    ErrorMessage: $"Обращение с id={id} не найдено.");
            }

            if (appeal.Status is "Closed" or "Cancelled")
            {
                return new AppealOperationResult<AppealResponse>(
                    AppealOperationStatus.Conflict,
                    ErrorMessage: "Нельзя изменить статус завершённого или отменённого обращения.");
            }

            if (normalizedStatus == "New")
            {
                return new AppealOperationResult<AppealResponse>(
                    AppealOperationStatus.Conflict,
                    ErrorMessage: "Нельзя вернуть обращение обратно в статус New.");
            }

            if (normalizedStatus == "InProgress")
            {
                if (appeal.Status != "New")
                {
                    return new AppealOperationResult<AppealResponse>(
                        AppealOperationStatus.Conflict,
                        ErrorMessage: "Перевести в InProgress можно только обращение в статусе New.");
                }

                if (!appeal.AssigneeUserId.HasValue)
                {
                    return new AppealOperationResult<AppealResponse>(
                        AppealOperationStatus.Conflict,
                        ErrorMessage: "Нельзя перевести обращение в работу, пока не назначен исполнитель.");
                }

                if (!actorUserId.HasValue || actorUserId.Value != appeal.AssigneeUserId.Value)
                {
                    return new AppealOperationResult<AppealResponse>(
                        AppealOperationStatus.Conflict,
                        ErrorMessage: "Перевести обращение в работу может только назначенный исполнитель (заголовок X-User-Id).");
                }
            }

            if (normalizedStatus == "Closed")
            {
                if (appeal.Status != "InProgress")
                {
                    return new AppealOperationResult<AppealResponse>(
                        AppealOperationStatus.Conflict,
                        ErrorMessage: "Закрыть можно только обращение, находящееся в статусе InProgress.");
                }

                if (!appeal.AssigneeUserId.HasValue || !actorUserId.HasValue || actorUserId.Value != appeal.AssigneeUserId.Value)
                {
                    return new AppealOperationResult<AppealResponse>(
                        AppealOperationStatus.Conflict,
                        ErrorMessage: "Закрыть обращение может только назначенный исполнитель (заголовок X-User-Id).");
                }
            }

            if (normalizedStatus == "Cancelled")
            {
                var isAssignedStaff = appeal.AssigneeUserId.HasValue && actorUserId == appeal.AssigneeUserId.Value;
                var isDispatcherOrAuthor = actorUserId is 1 || actorUserId == appeal.CreatedByUserId;
                if (!isAssignedStaff && !isDispatcherOrAuthor)
                {
                    return new AppealOperationResult<AppealResponse>(
                        AppealOperationStatus.Conflict,
                        ErrorMessage: "Отменить обращение может только назначенный исполнитель, автор или диспетчер (заголовок X-User-Id).");
                }
            }

            appeal.Status = normalizedStatus;
            appeal.UpdatedAt = DateTime.UtcNow;

            return new AppealOperationResult<AppealResponse>(
                AppealOperationStatus.Success,
                Data: ToResponse(appeal));
        }
    }

    private static AppealResponse ToResponse(Appeal appeal) => new()
    {
        Id = appeal.Id,
        Number = appeal.Number,
        Title = appeal.Title,
        Description = appeal.Description,
        ServiceTypeId = appeal.ServiceTypeId,
        Status = appeal.Status,
        CreatedByUserId = appeal.CreatedByUserId,
        AssigneeUserId = appeal.AssigneeUserId,
        CreatedAt = appeal.CreatedAt,
        UpdatedAt = appeal.UpdatedAt
    };
}
