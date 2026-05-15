var builder = WebApplication.CreateBuilder(args);

// Разрешаем фронтенду обращаться к API (CORS)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();

// Данные о команде
var team = new[]
{
    new { name = "Иванов Иван", role = "Tech Lead", fact = "Люблю C#" },
    new { name = "Петрова Мария", role = "Developer", fact = "Пишу на C# с 1 курса" },
    new { name = "Сидоров Алексей", role = "QA", fact = "Нахожу баги быстрее всех" }
};

// GET /api/team — вернуть список участников
app.MapGet("/api/team", () => Results.Ok(team));

// GET /api/team/{name} — вернуть одного участника
app.MapGet("/api/team/{name}", (string name) =>
{
    var member = team.FirstOrDefault(m =>
        m.name.Contains(name, StringComparison.OrdinalIgnoreCase));
    return member is not null
        ? Results.Ok(member)
        : Results.NotFound(new { error = "Участник не найден" });
});

// GET /api/ping – проверка что API живой
app.MapGet("/api/ping", () => Results.Ok(new { 
    status = "ok", 
    time = DateTime.Now.ToString("HH:mm:ss"), 
    team = "DevTeam" 
}));

app.Run();