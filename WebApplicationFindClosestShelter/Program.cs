using Bll_Services;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Dal;
using Dal.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShelteredPlacesDb>(option=>
option.UseSqlServer(builder.Configuration.GetConnectionString("ShelteredPlacesDb")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IDalAddress, DalAddress>();
builder.Services.AddScoped<IDalOpinion, DalOpinion>();
builder.Services.AddScoped<IDalShelter, DalShelter>();

builder.Services.AddScoped<IBllAddress, BllAddress>();
builder.Services.AddScoped<IBllOpinion, BllOpinion>();
builder.Services.AddScoped<IBllShelter, BllShelter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
