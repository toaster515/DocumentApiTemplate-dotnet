using Domain.Models;
namespace Application.Interfaces;
public interface IFileStorageFactory
{
    IFileStorageService Get(StorageProvider provider);
}
