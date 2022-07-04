using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P225Allup.DAL;
using P225Allup.Models;
using P225Allup.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225Allup.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }


            return  View(await _getBasketAsync(cookie));
        }
        public async Task<IActionResult> AddToBasket(int? id, int count =1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            List<BasketVM> basketVMs = null;

            string cookie = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrEmpty(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
                if (basketVMs.Exists(p => p.Id == id))
                {
                    basketVMs.Find(p => p.Id == id).Count++;
                }
                else
                {
                   
                    BasketVM basketVM = new BasketVM
                    {
                        Id = (int)id,
                        Count = count
                    };
                    basketVMs.Add(basketVM);
                }
            }
            else
            {
                basketVMs = new List<BasketVM>();
                BasketVM basketVM = new BasketVM
                {
                    Id = (int)id,
                    Count=count
                };
                basketVMs.Add(basketVM);
            }
            string item = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", item);

            return PartialView("_BasketPartial", await _getBasketAsync(item));
        }
        private async Task<List<BasketVM>> _getBasketAsync(string cookie)
        {

            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.Id);

                basketVM.Image = product.MainImage;
                basketVM.Name = product.Title;
                basketVM.Price = product.DiscountPrice > 0 ? product.DiscountPrice : product.Price;

            }

            return basketVMs;
        }

        public async Task<IActionResult> RemoveItem( int? id)
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            
            string items = JsonConvert.SerializeObject(basketVMs.FindAll(x => x.Id != id));
            HttpContext.Response.Cookies.Append("basket", items);

            return PartialView("_BasketPartial", await    _getBasketAsync(items));
        }

    }
}
