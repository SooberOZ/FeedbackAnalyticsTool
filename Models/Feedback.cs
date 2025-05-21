namespace FeedbackAnalyticsTool.Models
{
    class Feedback
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}