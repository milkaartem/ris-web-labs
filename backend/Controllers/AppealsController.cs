using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppealsController : ControllerBase
{
    private readonly IAppealService _appealService;
    private readonly ILogger<AppealsController> _logger;
    private readonly IWebHostEnvironment _env;

    public AppealsController(
        IAppealService appealService,
        ILogger<AppealsController> logger,
        IWebHostEnvironment env)
    {
        _appealService = appealService;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Получить список обращений (при отсутствии записей возвращает 200 и пустой массив []).
    /// </summary>
    [HttpGet]
    public ActionResult<IReadOnlyList<AppealResponse>> GetAll(
        [FromQuery] string? status = null,
        [FromQuery] int? serviceTypeId = null)
    {
        return Ok(_appealService.GetAll(status, serviceTypeId));
    }

    /// <summary>
    /// Получить одну карточку обращения по идентификатору.
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<AppealResponse> GetById(int id)
    {
        var appeal = _appealService.GetById(id);
        if (appeal is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Обращение не найдено",
                detail: $"Обращение с id={id} не существует.");
        }

        return Ok(appeal);
    }

    /// <summary>
    /// Создать новое обращение студента (всегда со статусом New).
    /// </summary>
    [HttpPost]
    public ActionResult<AppealResponse> Create(
        [FromBody] CreateAppealRequest request,
        [FromHeader(Name = "X-User-Id")] int? userId = null)
    {
        try
        {
            if (_env.IsDevelopment() &&
                string.Equals(Request.Headers["X-Simulate-Error"], "true", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Учебная симуляция сбоя после валидации");
            }

            var actorId = userId is > 0 ? userId.Value : 1;
            var created = _appealService.Create(request, actorId);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "CreateAppeal failed; serviceTypeId={ServiceTypeId}; error={ErrorType}",
                request.ServiceTypeId,
                ex.GetType().Name);

            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Не удалось обработать запрос");
        }
    }

    /// <summary>
    /// Назначить ответственного сотрудника деканата отдельным действием (статус не меняется).
    /// </summary>
    [HttpPatch("{id:int}/assignee")]
    public ActionResult<AppealResponse> Assign(int id, [FromBody] AssignAppealRequest request)
    {
        var result = _appealService.Assign(id, request.AssigneeUserId);
        return ToActionResult(result);
    }

    /// <summary>
    /// Изменить статус жизненного цикла обращения.
    /// </summary>
    [HttpPatch("{id:int}/status")]
    public ActionResult<AppealResponse> UpdateStatus(
        int id,
        [FromBody] UpdateAppealStatusRequest request,
        [FromHeader(Name = "X-User-Id")] int? userId = null)
    {
        var result = _appealService.UpdateStatus(id, request.Status, userId);
        return ToActionResult(result);
    }

    private ActionResult<AppealResponse> ToActionResult(AppealOperationResult<AppealResponse> result)
    {
        return result.Status switch
        {
            AppealOperationStatus.Success => Ok(result.Data),
            AppealOperationStatus.NotFound => Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Обращение не найдено",
                detail: result.ErrorMessage),
            AppealOperationStatus.ValidationError => Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Некорректные данные запроса",
                detail: result.ErrorMessage),
            AppealOperationStatus.Conflict => Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Нарушение правил процесса",
                detail: result.ErrorMessage),
            _ => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Не удалось обработать запрос")
        };
    }
}
