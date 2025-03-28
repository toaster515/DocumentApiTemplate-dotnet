using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Storage;
public class FileStorageFactory : IFileStorageFactory
{
    private readonly IServiceProvider _services;

    public FileStorageFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IFileStorageService Get(StorageProvider provider)
    {
        return provider switch
        {
            StorageProvider.Aws => _services.GetRequiredService<AwsStorageService>(),
            StorageProvider.Azure => _services.GetRequiredService<AzureBlobStorageService>(),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), $"Unsupported provider: {provider}")
        };
    }
}
