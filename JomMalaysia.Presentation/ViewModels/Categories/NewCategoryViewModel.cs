using System.ComponentModel.DataAnnotations;
using JomMalaysia.Presentation.Models.Categories;

namespace JomMalaysia.Presentation.ViewModels.Categories
{
    public class NewCategoryViewModel
    {
        [StringLength(10, ErrorMessage = "{0} should not exceed 10 characters.")]
        [Display(Name = "Category Code (optional)")]
        public string CategoryCode { get; set; }

        [Required]
        [Display(Name = "Category Type")]
        public CategoryType CategoryType { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Category Name (EN)")]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Category Name (BM)")]
        public string CategoryNameMs { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Category Name (中文)")]
        public string CategoryNameZh { get; set; }


        [Display(Name = "Category Logo")] public string CategoryImageUrl { get; set; }


        public string CategoryThumbnailUrl { get; set; }
    }
}