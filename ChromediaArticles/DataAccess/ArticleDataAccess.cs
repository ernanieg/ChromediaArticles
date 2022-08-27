using ChromediaArticles.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChromediaArticles.DataAccess
{
    public class ArticleDataAccess
    {
        static readonly HttpClient client = new HttpClient();

        private async Task<List<Article>> GetAllArticles()
        {
            List<Article> articles = Enumerable.Empty<Article>().ToList();

            // get the number of pages first
            int totalPages = 0;
            QueryResult result = await GetResult(1);


            articles.AddRange(result.data);

            totalPages = result.total_pages;

            for (int i = 2; i <= totalPages; i++)
            {
                QueryResult succeedingResult = await GetResult(i);
                articles.AddRange(succeedingResult.data);
            }

            return articles;
        }

        private async Task<QueryResult> GetResult(int limit)
        {
            HttpResponseMessage response = await client.GetAsync(String.Format("{0}{1}", Config.Constant.BaseURL, limit));
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<QueryResult>(responseBody);
            return result;
        }
        public async Task<List<Article>> GetTopArticles(int limit)
        {
            List<Article> articles = Enumerable.Empty<Article>().ToList();
            articles = await GetAllArticles();

            // If the title field is not null, use title.
            // Otherwise, if the story_title field is not null, use story_title. 
            // If both fields are null, ignore the article.
            List<Article> articlesWithName = articles
                .Where(x => !string.IsNullOrEmpty(x.title) ||
                            !string.IsNullOrEmpty(x.story_title)
                      ).ToList();


            // loop all article to replace empty title with story title
            foreach (Article article in articlesWithName)
            {
                if (string.IsNullOrEmpty(article.title))
                {
                    article.title = article.story_title;
                }
            }

            // Sort the titles decreasing by comment count, then decreasing alphabetically by article name if there is a tie in comments count
            List<Article> sortedArticles = articlesWithName.OrderByDescending(x => x.num_comments).ThenByDescending(x => x.title).Take(limit).ToList();

            return sortedArticles;
        }
    }
}
