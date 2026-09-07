using MediPrescribe.Application.Interfaces.Infrastructure;

namespace MediPrescribe.Infrastructure.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _webRootPath;

        public FileStorageService()
        {
            _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName)
        {
            var safeFolder = Path.GetFileNameWithoutExtension(folderName.Trim('/', '\\'));
            var uploadsDir = Path.Combine(_webRootPath, "uploads", safeFolder);

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var ext = Path.GetExtension(fileName);
            var storedName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsDir, storedName);

            await using (var target = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(target);
            }

            return $"/uploads/{safeFolder}/{storedName}";
        }

        public void DeleteFile(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                return;
            }

            var normalized = fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_webRootPath, normalized);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}