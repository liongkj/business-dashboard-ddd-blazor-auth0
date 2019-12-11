using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.Models.Categories
{
    public class Category
    {
        public string CategoryId { get; set; }


        [StringLength(10, ErrorMessage = "{0} should not exceed 10 characters.")]
        [Display(Name = "Category Code")]
        public string CategoryCode { get; set; }

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

        [Required]
        [Display(Name = "Category Logo")]
        public Image CategoryThumbnail { get; set; }

        public CategoryType CategoryType { get; set; } = CategoryType.Private;
        public CategoryPath CategoryPath { get; set; }
        public List<Category> LstSubCategory { get; set; }

        public bool IsCategory()
        {
            return CategoryPath.Subcategory == null;
        }

        public bool IsDeleted { get; set; }
        public bool IsSelected { get; set; }
    }

    public enum CategoryType
    {
        Professional,
        Government,
        Private,
        Nonprofit,
        Attraction
    }
}