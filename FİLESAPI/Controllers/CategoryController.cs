using AutoMapper;
using FİLESAPI.Dtos;
using FİLESAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FİLESAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(AppDbContext context, ILogger<CategoryController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        ResultDto result = new ResultDto();
        [HttpGet]
        public ActionResult<List<CategoryDto>> CategoryList()
        {
            var categories = _context.Categories.ToList();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        public ResultDto Post(CategoryDto dto)
        {
            var result = new ResultDto();

            if (_context.Categories.Count(c => c.Name == dto.Name) > 0)
            {
                result.Status = false;
                result.Message = "Girilen Kategori Kayıtlıdır";
                return result;
            }
            var category = _mapper.Map<Category>(dto);
            category.Updated = DateTime.Now;
            category.Created = DateTime.Now;

            _context.Categories.Add(category);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Kategori Eklendi";
            return result;
        }

        [HttpPut]
        public ResultDto Put(CategoryDto dto)
        {
            var result = new ResultDto();

            var category = _context.Categories.Where(s => s.Name == dto.Name).SingleOrDefault();
            if (category == null)
            {
                result.Status = false;
                result.Message = "Kategori Bulunamadı";
                return result;
            }
            category.Name = dto.Name;
            category.IsActive = dto.IsActive;
            category.Updated = DateTime.Now;
            _context.Categories.Update(category);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Kategori Düzenlendi";
            return result;
        }

        [HttpDelete("{id}")]
        public ResultDto Delete(int id)
        {
            var result = new ResultDto();

            var category = _context.Categories.SingleOrDefault(s => s.Id == id);
            if (category == null)
            {
                result.Status = false;
                result.Message = "Kategori Bulunamadı";
                return result;
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Kategori Silindi";
            return result;
        }

    }
}
