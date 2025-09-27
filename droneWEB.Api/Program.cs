using droneWEB.Data.Repository;
using droneWEB.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dapper servisleri
builder.Services.AddScoped<KullaniciRepository>();
builder.Services.AddScoped<KullaniciServisi>();
builder.Services.AddScoped<SmsRepository>();
builder.Services.AddScoped<SmsServisi>();
builder.Services.AddHttpClient<SmsServisi>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7053") // Web UI portunu yaz
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// JWT ve Authentication kaldýrýldý
// app.UseAuthentication();  
// app.UseAuthorization();

app.MapControllers();

app.Run();
