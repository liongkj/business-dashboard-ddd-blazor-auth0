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
        public String CategoryId { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} should not exceed 10 characters.")]
        [Display(Name = "Category Code")]
        public string CategoryCode { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Name Kategori")]
        public string CategoryNameMs { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "名称")]
        public string CategoryNameZh { get; set; }

        [Required]
        [Display(Name = "Category Image")]
        public string CategoryImageUrl { get; set; }

        [Required]
        public string CategoryThumbnailUrl { get; set; }

        public CategoryPath CategoryPath { get; set; }
        public List<Category> LstSubCategory { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsSelected { get; set; }

    }
}
