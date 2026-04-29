using ApiAzureExamenAGC.Data;
using ApiAzureExamenAGC.Helpers;
using ApiAzureExamenAGC.Repositories;
using ApiAzureExamenAGC.Services;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("AzureKeys"));
});
builder.Services.AddHttpContextAccessor();
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
HelperCryptography.Initialize(builder.Configuration, secretClient);
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration, secretClient);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticationSchema()).AddJwtBearer(helper.GetJwtBearerOptions());
KeyVaultSecret secret = await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secret.Value;
builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddDbContext<ContextCubo>(
    options => options.UseSqlServer(connectionString));
KeyVaultSecret storageSecret = await secretClient.GetSecretAsync("storageexamenapi");
string storage = storageSecret.Value;
BlobServiceClient blobServiceClient = new BlobServiceClient(storage);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);
builder.Services.AddTransient<ServicesBlobStorage>();
builder.Services.AddTransient<HelperUsuarioToken>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
