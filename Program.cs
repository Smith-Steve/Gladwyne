using Gladwyne.API.Interfaces;
using Gladwyne.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors((options) => 
{
    options.AddPolicy("DevCors", (corsBuilder) => 
    {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.AddPolicy("ProdCors", (corsBuilder) => 
    {
        corsBuilder.WithOrigins("https://Gladwyne.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

//This is linking our IUserRepository Interface as a scoped connection to UserRepository.
//This gives us access to the methods in UserRepository without actually using UserRepository.
builder.Services.AddScoped<IUserRepository, UserRepository>();

var application = builder.Build();

// Configure the HTTP request pipeline.
if (application.Environment.IsDevelopment())
{
    application.UseCors("DevCors");
    application.UseSwagger();
    application.UseSwaggerUI();
}
else
{
    application.UseCors("ProdCors");
    application.UseHttpsRedirection();
}

application.MapControllers();

application.Run();