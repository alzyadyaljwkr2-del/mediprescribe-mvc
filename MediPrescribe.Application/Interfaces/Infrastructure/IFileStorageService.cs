using System.IO;

namespace MediPrescribe.Application.Interfaces.Infrastructure
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName);
        void DeleteFile(string fileUrl);
    }
}
