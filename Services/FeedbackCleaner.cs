using Dapper;
using System.Data;

namespace FeedbackAnalyticsTool.Services
{
    class FeedbackCleaner
    {
        public static int RemoveNegativeFeedback(IDbConnection db, int minRating = 4)
        {
            var count = db.Execute("DELETE FROM Feedbacks WHERE Rating < @MinRating", new { MinRating = minRating });
            return count;
        }
    }
}