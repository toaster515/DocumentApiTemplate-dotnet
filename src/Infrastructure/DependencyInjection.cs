using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Abstractions;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using Amazon;
using Amazon.Runtime;
using Azure.Storage.Blobs;
using Infrastructure.Queueing;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileStorageService, AwsStorageService>();
        services.AddScoped<IFileStorageFactory, FileStorageFactory>();
        services.AddScoped<IFileRecordRepository, FileRecordRepository>();

        services.AddScoped<AwsStorageService>();

        var awsSection = configuration.GetSection("Storage:Aws");
        var awsOptions = awsSection.Get<AWSOptions>() ?? new AWSOptions();

        awsOptions.Credentials = new BasicAWSCredentials(
            awsSection["AccessKey"],
            awsSection["SecretKey"]
        );

        awsOptions.Region = RegionEndpoint.GetBySystemName(awsSection["Region"] ?? "us-east-1");

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();

        var azureConnection = configuration["Storage:Azure:ConnectionString"];
        services.AddSingleton(new BlobServiceClient(azureConnection));

        services.AddScoped<AzureBlobStorageService>();

        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<MetadataWorker>();


        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}
