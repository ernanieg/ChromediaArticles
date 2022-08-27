using ChromediaArticles.DataAccess;
using ChromediaArticles.Models;
using System;
using System.Collections.Generic;

namespace ChromediaArticles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the number of articles to return:");
            int topArticle = Convert.ToInt32(Console.ReadLine());

            ArticleDataAccess articleDA = new ArticleDataAccess();
            List<Article> articles = articleDA.GetTopArticles(topArticle).Result;

            foreach (Article article in articles)
            {
                Console.WriteLine(article.title);
            }

            Console.ReadKey();
        }


    }
}
