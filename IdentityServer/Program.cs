using IdentityServer.ConfigSample;
using IdentityServer.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddDALDependencyGroup(builder.Configuration) // DAL
                .AddMicrosoftIdentityServices() // Microsoft Identity
                .AddTokenConfigs(builder.Configuration) // JWT
                ;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
