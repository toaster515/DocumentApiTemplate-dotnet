using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Api.Services;
using System.Reflection;
using MediatR;
using Infrastructure;
using Infrastructure.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Application.Commands;
using Application.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
});
builder.Services.AddHealthChecks();
// builder.Services.AddScoped<IUser, CurrentUser>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateFileRecordCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UploadFileCommandHandler).Assembly); 
    cfg.RegisterServicesFromAssembly(typeof(GetFileQueryHandler).Assembly); 
    cfg.RegisterServicesFromAssembly(typeof(GetRecordQueryHandler).Assembly); 
});

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
}
app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
