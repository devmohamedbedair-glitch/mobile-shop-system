using Microsoft.AspNetCore.Mvc;
using MobileShop.Data;
using MobileShop.Models;
using System;
using System.Linq;

namespace MobileShop.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 عرض صفحة إنشاء فاتورة
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // 🟢 حفظ الفاتورة (أكتر من منتج)
        [HttpPost]
        public IActionResult Create(int[] productIds, int[] quantities)
        {
            if (productIds == null || quantities == null || productIds.Length == 0)
            {
                return Content("❌ لازم تختار منتج واحد على الأقل");
            }

            var invoice = new Invoice
            {
                Date = DateTime.Now,
                TotalAmount = 0
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            decimal total = 0;

            for (int i = 0; i < productIds.Length; i++)
            {
                var product = _context.Products.Find(productIds[i]);

                if (product == null) continue;

                var qty = quantities[i];

                // ❌ منع إدخال كمية غلط
                if (qty <= 0) continue;

                // ❌ منع البيع لو الكمية مش كفاية
                if (product.Quantity < qty)
                {
                    return Content($"❌ الكمية غير كافية للمنتج: {product.Name}");
                }

                var item = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    Quantity = qty,
                    Price = product.Price
                };

                _context.InvoiceItems.Add(item);

                total += product.Price * qty;

                // تقليل المخزون
                product.Quantity -= qty;
            }

            invoice.TotalAmount = total;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 🟢 عرض كل الفواتير
        public IActionResult Index()
        {
            var invoices = _context.Invoices.ToList();
            return View(invoices);
        }


        public IActionResult Details(int id)
        {
            var invoice = _context.Invoices
                .Where(i => i.Id == id)
                .Select(i => new
                {
                    i.Id,
                    i.Date,
                    i.TotalAmount,
                    Items = _context.InvoiceItems
                        .Where(ii => ii.InvoiceId == i.Id)
                        .Select(ii => new
                        {
                            ProductName = ii.Product.Name,
                            ii.Quantity,
                            ii.Price
                        }).ToList()
                })
                .FirstOrDefault();

            return View(invoice);
        }












    }
}