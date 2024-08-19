using Microsoft.EntityFrameworkCore;
using MovieServiceApi.DataBase.Context;
using MovieServiceApi.Feedbacks.DTO;

namespace MovieServiceApi.Feedbacks.Service
{
    public class FeedbackService(ILogger<FeedbackService> logger, MovieServiceContext db)
    {
        public async Task<bool> CreateFeedBack(FeedbackParamsDTO dto)
        {
            try
            {
                var numberOfInserted = await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO [feedback] ([fbk_film], [fbk_user], [fbk_text], [fbk_mark]) VALUES ({dto.FilmId}, {dto.UserId}, {dto.Text}, {dto.Rating})");
                if (numberOfInserted == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during feedback creation with: {dto}, with message: {ex}", dto, ex.Message);
                return false;
            }
        }

        public async Task<List<FeedbackDTO>?> GetFeedbackForFilm(int filmId)
        {
            try
            {
                var feedback = await db.Feedbacks
                    .Where(fbk => fbk.FbkFilm == filmId)
                    .Include(fbk => fbk.FbkUserNavigation)
                    .Select(fbk => new FeedbackDTO()
                    {
                        Mark = fbk.FbkMark,
                        Text = fbk.FbkText,
                        UserName = fbk.FbkUserNavigation.UsrName
                    })
                    .ToListAsync();

                return feedback;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during feedback getting for film: {filmID} , with message: {ex}", filmId, ex.Message);
                return null;
            }
        }

        public async Task<List<FeedbackDTO>?> GetFeedbackForUser(int userId)
        {
            try
            {
                var feedback = await db.Feedbacks
                    .Where(fbk => fbk.FbkUser == userId)
                    .Include(fbk => fbk.FbkUserNavigation)
                    .Select(fbk => new FeedbackDTO()
                    {
                        Mark = fbk.FbkMark,
                        Text = fbk.FbkText,
                        UserName = fbk.FbkUserNavigation.UsrName
                    })
                    .ToListAsync();

                return feedback;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during feedback getting for user: {userId} , with message: {ex}", userId, ex.Message);
                return null;
            }
        }
        public async Task<bool> DeleteFeedback(int filmId, int userId)
        {
            try
            {
                var numberOfDeleted = await db.Feedbacks
                    .Where(fbk => fbk.FbkUser == userId && fbk.FbkFilm == filmId)
                    .ExecuteDeleteAsync();

                if (numberOfDeleted == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during feedback deletion with key (user: {userId}, film: {filmID}) , with message: {ex}", userId, filmId, ex.Message);
                return false;
            }
        }

        //Forbidden
        public async Task<bool> UpdateFeedback(FeedbackUpdateDTO dto)
        {
            try
            {
                //Complete it later
                var numberOfUpdated = await db.Feedbacks
                    .Where(f => f.FbkUser == dto.UserId && f.FbkFilm == dto.FilmId)
                    .ExecuteUpdateAsync(f => f
                        .SetProperty(p => p.FbkMark, p => dto.NewRating ?? p.FbkMark)
                        .SetProperty(p => p.FbkText, p => dto.NewText ?? p.FbkText));

                if (numberOfUpdated == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured during feedback update with key (user: {userId}, film: {filmID}) , with message: {ex}", dto.UserId, dto.FilmId, ex.Message);
                return false;
            }
        }

    }
}
