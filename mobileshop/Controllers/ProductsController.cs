using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;
using System.Linq;

namespace MobileShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Delete
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        // POST: Delete
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            var item = _context.Products.Find(product.Id);
            _context.Products.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }






    }







}

