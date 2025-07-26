using ClosetMuseBackend.Config;
using ClosetMuseBackend.Repositories;
using ClosetMuseBackend.Repositories.Interfaces;
using ClosetMuseBackend.Services;
using ClosetMuseBackend.Services.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load Firebase configuration
var firebaseConfig = builder.Configuration.GetSection("Firebase");
var serviceAccountPath = firebaseConfig["ServiceAccountPath"];
var projectId = firebaseConfig["ProjectId"];
var bucketName = firebaseConfig["BucketName"];

// Set GOOGLE_APPLICATION_CREDENTIALS environment variable
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", serviceAccountPath);

// Firestore
var firestore = FirebaseConfig.InitializeFirestore(serviceAccountPath, projectId);
builder.Services.AddSingleton(firestore);

// Storage
builder.Services.AddSingleton(StorageClient.Create());

// Register repositories & services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(provider => new UserService(
    provider.GetRequiredService<IUserRepository>(),
    provider.GetRequiredService<FirebaseAuthService>(),
    provider.GetRequiredService<StorageClient>(),
    firebaseConfig["BucketName"]! // Explicitly pass the bucket name
));
builder.Services.AddScoped<IOutfitRepository, OutfitRepository>();
builder.Services.AddScoped(provider => new OutfitService(
    provider.GetRequiredService<IOutfitRepository>(),
    provider.GetRequiredService<StorageClient>(),
    firebaseConfig["BucketName"]! // Explicitly pass the bucket name
));
builder.Services.AddScoped<FirebaseAuthService>();

// Register OutfitService with bucketName explicitly
builder.Services.AddScoped<IOutfitService>(provider =>
{
    var outfitRepository = provider.GetRequiredService<IOutfitRepository>();
    var storageClient = provider.GetRequiredService<StorageClient>();
    var bucketName = builder.Configuration["Firebase:BucketName"]; // Ensure this matches your configuration
    return new OutfitService(outfitRepository, storageClient, bucketName);
});

// Register PlanService as usual
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClosetMuse API",
        Version = "v1",
        Description = "API documentation for ClosetMuse Backend"
    });
});

builder.Services.AddControllers();
var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClosetMuse API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();