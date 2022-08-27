namespace ChromediaArticles.Models
{
    public class Article
    {
        public string title { get; set; }
        public string url { get; set; }
        public string author { get; set; }
        public int? num_comments { get; set; }
        public int? story_id { get; set; }
        public string story_title { get; set; }
        public string story_url { get; set; }
        public int? parent_id { get; set; }
        public int created_at { get; set; }
    }
}
