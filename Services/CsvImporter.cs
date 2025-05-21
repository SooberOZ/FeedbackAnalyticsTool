using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using System.Data;
using System.Globalization;

namespace FeedbackAnalyticsTool.Services
{
    class CsvImporter
    {
        public static void ImportFeedbackFromCsv(IDbConnection db, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл CSV не знайдено.");
                return;
            }

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            });

            var records = csv.GetRecords<dynamic>();
            foreach (var record in records)
            {
                string projectName = record.ProjectName;
                string employeeName = record.EmployeeName;
                int rating = int.Parse(record.Rating);
                string comment = record.Comment;

                var projectId = db.ExecuteScalar<int?>("SELECT Id FROM Projects WHERE Name = @Name", new { Name = projectName });

                if (!projectId.HasValue)
                {
                    db.Execute("INSERT INTO Projects (Name) VALUES (@Name)", new { Name = projectName });
                    projectId = db.ExecuteScalar<int?>("SELECT Id FROM Projects WHERE Name = @Name", new { Name = projectName });
                }

                db.Execute(@"
                    INSERT INTO Feedbacks (ProjectId, EmployeeName, Rating, Comment)
                    VALUES (@ProjectId, @EmployeeName, @Rating, @Comment)",
                    new { ProjectId = projectId.Value, EmployeeName = employeeName, Rating = rating, Comment = comment });
            }
        }
    }
}