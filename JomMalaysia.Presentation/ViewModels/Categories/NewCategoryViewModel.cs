using System;
using System.ComponentModel.DataAnnotations;

namespace JomMalaysia.Presentation.ViewModels.Categories
{
    public class NewCategoryViewModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "{0} should not exceed 10 characters.")]
        [Display(Name = "Category Code")]
        public string CategoryCode { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Kategori")]
        public string CategoryNameMs { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "类别")]
        public string CategoryNameZh { get; set; }

        [Required]
        [Display(Name = "Category Image")]
        public string CategoryImageUrl { get; set; }

        [Required]
        public string CategoryThumbnailUrl { get; set; }
    }
}