using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text.Json.Serialization;
using FacilityLeasing;
using FacilityLeasing.Interfaces;
using FacilityLeasing.Middleware;
using FacilityLeasing.Options;
using FacilityLeasing.Common.Interfaces;
using FacilityLeasing.Common.Services;
using FacilityLeasing.Extensions;
using FacilityLeasing.Data;
using FacilityLeasing.Services;
using FacilityLeasing.MappingProfiles;
using FluentValidation.AspNetCore;
using FluentValidation;
using FacilityLeasing.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Facility Leasing API", Version = "v1" });

    // Add API Key Authentication
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = Constants.ApiKeyHeaderName,
        Type = SecuritySchemeType.ApiKey,
        Description = "API key needed to access the endpoints. Use 'X-Api-Key: {your_api_key}'."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddDbContext<FacilityManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FacilityManagementConnection"));
});

builder.Services.AddAutoMapper(typeof(FacilityManagementProfile));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IEquipmentPlacementContractService, EquipmentPlacementContractService>();

// Add Azure Blob Storage
builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));
builder.Services.AddSingleton<IAzureStorageService, AzureStorageService>();

// Add PDF Generator
builder.Services.AddSingleton<IPdfGenerator, PdfGenerator>();
QuestPDF.Settings.License = LicenseType.Community;

// Add Background Service
builder.Services.AddSingleton<PdfBackgroundService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<PdfBackgroundService>());

builder.Services.Configure<ApiKeySettings>(builder.Configuration.GetSection(ApiKeySettings.ApiKeySettingsSectionName));

builder.Services.AddValidatorsFromAssemblyContaining<EquipmentPlacementContractValidator>(ServiceLifetime.Transient);
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GenericExceptionHandler>();
app.UseMiddleware<ApiKeyMiddleware>();

await app.SeedProductionFacilities();
await app.SeedProcessEquipmentTypes();

app.UseCors(opt => opt
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
