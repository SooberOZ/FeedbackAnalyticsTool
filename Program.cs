using Dapper;
using FeedbackAnalyticsTool.Data;
using FeedbackAnalyticsTool.Models;
using FeedbackAnalyticsTool.Services;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Batteries.Init();

var connectionString = "Data Source=feedback.db";
using IDbConnection db = new SqliteConnection(connectionString);
DbInitializer.Initialize(db);

Console.WriteLine("📥 Імпорт даних із CSV...");
CsvImporter.ImportFeedbackFromCsv(db, "feedback.csv"); // Потрібно вказувати повний шлях файлу

Console.WriteLine("Введіть назву проекту для розрахунку середньої оцінки: ");
var input = Console.ReadLine();
var targetProjectName = db.QueryFirstOrDefault<Project>(
    "SELECT * FROM Projects WHERE Name = @Name", new { Name = input });

if (targetProjectName != null)
{
    var avg = db.ExecuteScalar<double>(
        "SELECT AVG(Rating) FROM Feedbacks WHERE ProjectId = @ProjectId", new { ProjectId = targetProjectName.Id });

    Console.WriteLine($"Середня оцінка по проекту \"{input}\" = {avg:F2}");
}
else
{
    Console.WriteLine("Проект не знайдено.");
}

Console.WriteLine("\nБажаєш видалити всі негативні відгуки (оцінка < 4)? (y/n): ");
var confirm = Console.ReadLine();

if (confirm?.ToLower() == "y")
{
    int removed = FeedbackCleaner.RemoveNegativeFeedback(db);
    Console.WriteLine($"Видалено {removed} негативних відгуків.");
}
else
{
    Console.WriteLine("Негативні відгуки залишено.");
}