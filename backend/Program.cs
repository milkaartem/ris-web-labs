using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAppealService, AppealService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/info", (IWebHostEnvironment env) => Results.Ok(new
{
    app = "ElectronicDeaneryApi",
    title = "Электронная приёмная деканата ИЦТЭ КГЭУ",
    environment = env.EnvironmentName,
    version = "0.1.0-lab07",
    note = "ЛР7: каркас Web API (создание обращений появится в ЛР8)"
}));

app.MapControllers();

app.Run();
