using RestApiCore.Controllers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// add this code for CORS
builder.Services.AddCors(p => p.AddPolicy(name: "YourNamePolicy", policy =>
    policy.WithMethods().WithOrigins("http://hello.com").AllowAnyHeader()
)) ;



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

// add this code
app.UseStaticFiles();
app.UseCors("YourNamePolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
