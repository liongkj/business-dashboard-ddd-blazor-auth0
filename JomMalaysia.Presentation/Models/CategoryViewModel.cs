using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Category;

namespace JomMalaysia.Presentation.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel() { }

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

        [StringLength(500)]
        [Display(Name = "Display Image")]
        public Image CategoryImage { get; set; }

        public CategoryPathViewModel CategoryPath { get; set; }
        public List<CategoryViewModel> LstSubCategory { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsSelected { get; set; }

    }
}
