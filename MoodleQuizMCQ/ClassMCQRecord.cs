namespace MoodleQuizMCQ
{
    internal class ClassMCQRecord // MCQ record class
    {

        // Headers for csv file must match names
        public string Topic { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
