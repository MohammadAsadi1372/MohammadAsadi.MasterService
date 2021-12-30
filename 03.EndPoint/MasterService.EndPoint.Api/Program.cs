using MasterService.Core.ApplicationService.Query;
using MasterService.Core.Domain.Repo;
using MasterService.EndPoint.Api.Helper;
using MasterService.EndPoint.Api.Middleware;
using MasterService.Infra.Data.Sql;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly())
    .AddTransient<IRequestHandler<FetchQuery, List<object>>, FetchQueryHandler>();
builder.Services.AddScoped<IQueryRepository, QueryRepository>();
builder.Services.AddSingleton<IConfiguration>(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuthenticationMiddleWare>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
