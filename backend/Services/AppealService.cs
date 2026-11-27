namespace Backend.Services;

/// <summary>
/// Реализация сервиса-заготовки (регистрируется через DI).
/// </summary>
public class AppealService : IAppealService
{
    public object GetStubStatus()
    {
        return new
        {
            service = nameof(AppealService),
            status = "stub",
            message = "Каркас сервиса обращений (ЛР7). Создание и работа с заявками появятся в ЛР8."
        };
    }
}
