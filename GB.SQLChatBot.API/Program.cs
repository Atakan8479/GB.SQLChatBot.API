// GB.SQLChatBot.API/Program.cs (Dosya adı aynı kalıyor)
using GB.SQLChatBot.Data.EF;
using GB.SQLChatBot.Data.Repositories;
using GB.SQLChatBot.Business.Services; // OpenAIService için
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
// Microsoft.Extensions.Configuration artık gerekmiyor, kaldırabilirsiniz.
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using GB.SQLChatBot.Business.Leave.Handlers;
using GB.SQLChatBot.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DbContext ve Repository Kaydı
builder.Services.AddDbContext<LeaveContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonAnnualLeaveActionRepository, PersonAnnualLeaveActionRepository>();

// MediatR Kaydı
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ChatbotQueryParserService).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ExecuteRawSqlQueryHandler).Assembly));


// HttpClient ve OpenAIService Kaydı (Artık Ollama'yı temsil ediyor)
// SSL doğrulama devre dışı bırakma kodu kaldırıldı, çünkü HTTP kullanıyoruz.
//builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IChatbotQueryParserService, ChatbotQueryParserService>();

builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<ISqlExecutionService, SqlExecutionService>();


// ChatbotQueryParserService'in kendisini kaydet
//builder.Services.AddScoped<ChatbotQueryParserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "https://localhost:4200") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();