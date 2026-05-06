using System.ComponentModel.DataAnnotations;

namespace Day3WebApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be 1-255 characters")]
        public string Title { get; set; } = string.Empty;

        public List<Author> Authors { get; set; } = [];

        [StringLength(80)]
        public string Genre { get; set; } = string.Empty;

        [Range(1000, 2030, ErrorMessage = "Year must be between 1000 and 2030")]
        public int Year { get; set; }

        public bool Available { get; set; } = true;

        [Url(ErrorMessage = "CoverImageUrl must be a valid URL")]
        public string? CoverImageUrl { get; set; }
    }
}
