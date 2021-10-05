using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Evidence.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required!!!")]
        [StringLength(50)]
        public string CategoryName { get; set; }
        //Navigation
        public ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name is required!!!")]
        [StringLength(50)]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage ="Product Model is required!!!")]
        [StringLength(30)]
        [Display(Name = "Product Model")]
        public string ModelName { get; set; }
        [Required(ErrorMessage = "Manufacture Date is required")]
        [Column(TypeName ="date")]
        [Display(Name = "Manufacture Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MfgDate { get; set; }
        [Required(ErrorMessage = "Product price is required")]
        [Column(TypeName = "money")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [ForeignKey("Category")]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }

        [Display(Name = "Product Image")]
        public string ProductImage { get; set; }
        [Display(Name = "Warranty Status")]
        public bool? WarrantyStatus { get; set; }
        //Navigation
        public Category Category { get; set; }
    }

    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
