using RelatedEntities.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>());

//DbContext
builder.Services.AddDbContext<DataContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("ConexionMySql"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql")
));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();//importamos para utilizar lo controladores
app.Run();


