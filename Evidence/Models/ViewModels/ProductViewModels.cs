using Evidence.CustomValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Evidence.Models.ViewModels
{
    public class ProductViewModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required!!!")]
        [StringLength(50)]
        [Display(Name = "Product Name")]
        //[RegularExpression(@"^^([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$", ErrorMessage =("Product Name Start with a uppercase letter!!!"))]
        //[RegularExpression(@"^([A-z][A-Za-z]*\s*[A-Za-z]*)$", ErrorMessage =("Product Name Start with a uppercase letter!!!"))]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Model is required!!!")]
        [StringLength(30)]
        [Display(Name = "Product Model")]
        public string ModelName { get; set; }
        [Required(ErrorMessage = "Manufacture Date is required")]
        [Column(TypeName = "date")]
        [Display(Name = "Manufacture Date")]
        [CustomMfgDate(ErrorMessage = "Manufacture Date should be less than or equal Today!!!")]
        public DateTime MfgDate { get; set; }
        [Required(ErrorMessage = "Product price is required")]
        [Column(TypeName = "money")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }

        [Display(Name = "Product Image")]
        public string ImagePath { get; set; }
        [Display(Name = "Product Image")]
        public IFormFile Image { get; set; }
        [Display(Name = "Warranty Status")]
        public bool? WarrantyStatus { get; set; }
    }
}
