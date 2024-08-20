using MovieServiceApi.DataBase.Context;
using MovieServiceApi.Images.DTO;
using System.IO;
using System.Net.Security;

namespace MovieServiceApi.Images.Service
{
    public class ImageService(ILogger<ImageService> logger, MovieServiceContext db)
    {
        private static string imageFolderPath = "C:\\Users\\dimon\\OneDrive\\Рабочий стол\\Study\\Универ\\Курс 3\\Семестр 6\\БД\\Курсовая\\Data";
        public async Task<FileStream?> GetImage(string path)
        {
            return await Task.Run(() =>
            {
                var fullPath = Path.Combine(imageFolderPath, path);
                try
                {
                    if (!File.Exists(fullPath)) return null;

                    return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                }
                catch (Exception ex)
                {
                    logger.LogError("Exception occured during image getting with path: {path} and exception: {ex}", path, ex.Message);
                    return null;
                }
            });
        }

        public async Task<bool> UploadImage(UploadeImageDTO dto)
        {
            try
            {
                var fullPath = Path.Combine(imageFolderPath, dto.PathInImages, dto.File.FileName);
                using var filStream = new FileStream(fullPath, FileMode.Create);
                await dto.File.CopyToAsync(filStream);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during image uploading with name: {name} and exception: {ex}", dto.File.FileName, ex.Message);
                return false;
            }
        }
    }
}
