using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P225Allup.DAL;
using P225Allup.Interfaces;
using P225Allup.Models;
using P225Allup.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225Allup.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BasketVM>> GetBasketAsync()
        {
            string cookie = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrEmpty(cookie))
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

        public async Task<IDictionary<string,string>> GetSettingsAsync()
        {
            IDictionary<string, string> setting = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);
            return setting;
        }

        
    }
}
