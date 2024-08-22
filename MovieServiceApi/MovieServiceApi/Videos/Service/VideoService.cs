using Microsoft.IdentityModel.Tokens;
using MovieServiceApi.DataBase.Context;
using MovieServiceApi.DataBase.Entities;
using MovieServiceApi.Videos.DTO;

namespace MovieServiceApi.Videos.Service
{
    public class VideoService(ILogger<VideoService> logger, MovieServiceContext db)
    {
        private const string videoFolderPath = "C:\\Users\\dimon\\OneDrive\\Рабочий стол\\Study\\Универ\\Курс 3\\Семестр 6\\БД\\Курсовая\\Data\\";

        public async Task<(FileStream? file, bool isPartitial)?> GetVideoFile(string path, HttpContext context)
        {
            return await Task.Run(() =>
            {
                var response = context.Response;
                var request = context.Request;
                var filePath = Path.Combine(videoFolderPath, path);

                if (!File.Exists(filePath))
                    return (null!, false);

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileLength = fileStream.Length;
                var rangeHeader = request.Headers["Range"].ToString();


                if (string.IsNullOrEmpty(rangeHeader))
                {
                    return (fileStream, false);
                }

                var range = rangeHeader.Replace("bytes=", "").Split('-');
                var start = long.Parse(range[0]);
                var end = range.Length > 1 && !string.IsNullOrEmpty(range[1]) ? long.Parse(range[1]) : fileLength - 1;

                if (start >= fileLength || end >= fileLength)
                    return (null!, false);

                fileStream.Seek(start, SeekOrigin.Begin);

                var contentLength = end - start + 1;
                response.StatusCode = 206;
                response.Headers["Content-Range"] = $"bytes {start}-{end}/{fileLength}";
                response.Headers.ContentLength = contentLength;
                return (fileStream, true);
            });
        }

        public async Task<bool> AddIntoDataBase(VideoInsertDTO dto)
        {
            try
            {
                var media = new Medium();
                if (!dto.TrailerPath.IsNullOrEmpty()) media.MedTrailerPath = dto.TrailerPath;
                if (!dto.FilmPath.IsNullOrEmpty()) media.MedFilmPath = dto.FilmPath;
                await db.Media
                    .AddAsync(media);

                var numberOfInserted = await db.SaveChangesAsync();
                if (numberOfInserted == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during addition film video into data base : {ex}", ex.Message);
                return false;
            }
        }
    }
}
