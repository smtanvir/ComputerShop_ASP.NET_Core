using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Evidence.Models;
using Evidence.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Evidence.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ProductDbContext context;
        //private readonly IHostingEnvironment he;
        private readonly IWebHostEnvironment he;
        public ProductsController(ProductDbContext _context, IWebHostEnvironment _he)
        {
            this.context = _context;
            this.he = _he;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var productDbContext = context.Products.Include(p => p.Category);
            return View(await productDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,ModelName,MfgDate,Price,CategoryId,Image,WarrantyStatus")] ProductViewModels pvm)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(pvm);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            
            Product p = new Product();
            if (ModelState.IsValid)
            {
                if (pvm.Image != null)
                {
                    string webroot = he.WebRootPath;
                    string folder = "Images";
                    string imageFileName = Guid.NewGuid() + "_" + Path.GetFileName(pvm.Image.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imageFileName);
                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await pvm.Image.CopyToAsync(stream);
                    }

                    p.ProductName = pvm.ProductName;
                    p.ModelName = pvm.ModelName;
                    p.MfgDate = pvm.MfgDate;
                    p.Price = pvm.Price;
                    p.CategoryId = pvm.CategoryId;
                    p.ProductImage = imageFileName;
                    p.WarrantyStatus = pvm.WarrantyStatus;

                    context.Add(p);
                    await context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return PartialView("_Success");
                }
                else
                {
                    return PartialView("_Fail");
                }
                
            }
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "CategoryName", pvm.CategoryId);
            return View(pvm);
        }

        // Get: Update Products 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModels pvm = new ProductViewModels
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ModelName = product.ModelName,
                MfgDate = product.MfgDate,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ImagePath = product.ProductImage,
                WarrantyStatus = product.WarrantyStatus
            };
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "CategoryName", pvm.CategoryId);
            return View(pvm);
        }

        // Update Products
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,ModelName,MfgDate,Price,CategoryId,Image,WarrantyStatus")] ProductViewModels pvm)
        {
            var products = await context.Products.FindAsync(id);

            if (id != pvm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (pvm.Image != null)
                    {

                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", products.ProductImage);
                        System.IO.File.Delete(filepath);

                        string webroot = he.WebRootPath;
                        string folder = "Images";
                        string imageFileName = Guid.NewGuid() + "_" + Path.GetFileName(pvm.Image.FileName);
                        string fileToWrite = Path.Combine(webroot, folder, imageFileName);
                        using (var stream = new FileStream(fileToWrite, FileMode.Create))
                        {
                            await pvm.Image.CopyToAsync(stream);
                        }
                        products.Id = pvm.Id;
                        products.ProductName = pvm.ProductName;
                        products.ModelName = pvm.ModelName;
                        products.MfgDate = pvm.MfgDate;
                        products.Price = pvm.Price;
                        products.CategoryId = pvm.CategoryId;
                        products.ProductImage = imageFileName;
                        products.WarrantyStatus = pvm.WarrantyStatus;

                        context.Update(products);
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        products.Id = pvm.Id;
                        products.ProductName = pvm.ProductName;
                        products.ModelName = pvm.ModelName;
                        products.MfgDate = pvm.MfgDate;
                        products.Price = pvm.Price;
                        products.CategoryId = pvm.CategoryId;
                        products.ProductImage = products.ProductImage;
                        products.WarrantyStatus = pvm.WarrantyStatus;
                        context.Update(products);
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(pvm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "CategoryName", pvm.CategoryId);
            return View(pvm);
        }

        // Delete Products
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = context.Products.Find(id);
            if (product.ProductImage != null)
            {
                //For Image Delete Form Folder
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images", product.ProductImage);

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                //Delete Data
                var filedel = (from Product in context.Products where Product.Id == id select Product).FirstOrDefault();
                context.Products.Remove(filedel);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            else
            {
                //Delete Data
                var filedel = (from Product in context.Products where Product.Id == id select Product).FirstOrDefault();
                context.Products.Remove(filedel);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await context.Products.FindAsync(id);
        //    context.Products.Remove(product);
        //    await context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ProductExists(int id)
        {
            return context.Products.Any(e => e.Id == id);
        }
    }
}
