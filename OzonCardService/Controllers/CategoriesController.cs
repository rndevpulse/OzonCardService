using Microsoft.AspNetCore.Mvc;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [Route("all")]
        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }
    }
}
