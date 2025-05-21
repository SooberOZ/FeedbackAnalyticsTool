using Dapper;
using System.Data;

namespace FeedbackAnalyticsTool.Data
{
    class DbInitializer
    {
        public static void Initialize(IDbConnection db)
        {
            db.Execute(@"
            CREATE TABLE IF NOT EXISTS Projects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Feedbacks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProjectId INTEGER,
                EmployeeName TEXT NOT NULL,
                Rating INTEGER,
                Comment TEXT,
                FOREIGN KEY(ProjectId) REFERENCES Projects(Id)
            );
        ");
        }
    }
}