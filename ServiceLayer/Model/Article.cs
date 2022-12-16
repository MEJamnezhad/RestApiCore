using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ServiceLayer
{
    public class Article : BaseEntity<Article>, INonDeletable, INonActive
    {

        [Required]
        public string Title { get; set; } = String.Empty;
        [Required]
        public string Text { get; set; } = String.Empty;

        public string Picture { get; set; } = String.Empty;
       
        public string DocumentPath { get; set; } = String.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public bool IsDeleted { get; set; } = false;

        public int Visit { get; set; } = 0;

        public int CategoryId { get; set; }

        // If you don't want Category data to appear in the resulting JSON
        [JsonIgnore]
        public virtual Category Category { get; set; }

        public int WriterId { get; set; }

        // If you don't want User data to appear in the resulting JSON
        [JsonIgnore]
        public virtual User Writer { get; set; }


        #region Helper

        public static async Task<List<Article>> GetActiveArticles(int categoryId)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    return await context.Articles.Where(p => p.CategoryId == categoryId && p.IsDeleted == false && p.IsActive == true).ToListAsync();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public static async Task<List<Article>> GetTop(int categoryId = 0, int top = 0)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    List<Article> articles;
                    if (categoryId == 0)
                    {
                        articles = await context.Articles
                                .Where(p => p.IsDeleted == false && p.IsActive == true)
                                .OrderByDescending(p => p.Id).ToListAsync();
                    }
                    else
                    {
                        articles = await context.Articles
                            .Where(p => p.CategoryId == categoryId && p.IsDeleted == false && p.IsActive == true)
                            .OrderByDescending(p => p.Id).ToListAsync();
                    }

                    if (top == 0)
                        return articles.ToList();

                    return articles.Take(top).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion




    }
}
