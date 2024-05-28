using AutoMapper;
using FİLESAPI.Dtos;
using FİLESAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FİLESAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        ResultDto result = new ResultDto();

        public FilesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public List<FilliesDto> GetAllFiles()
        {

            var files = _context.Files.ToList();
            var FilliesDtos = _mapper.Map<List<FilliesDto>>(files);

            return FilliesDtos;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<FilliesDto> GetFile(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == id);
            if (file == null)
            {
                return NotFound("Dosya Bulunamadı");
            }
            var FilliesDto = _mapper.Map<FilliesDto>(file);
            return FilliesDto;
        }

        [HttpPost]
        public ActionResult<ResultDto> UploadFile(FilliesDto dto)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == dto.Id);
            if (file != null)
            {
                result.Status = false;
                result.Message = "Girilen dosya ID'si zaten mevcut.";
                return result;
            }


            var mapper = _mapper.Map<Fillies>(dto);
            mapper.Created = dto.Created;
            mapper.Updated = dto.Updated;
       

            _context.Files.Add(mapper);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Dosya başarıyla eklendi.";
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ResultDto> UpdateFile(FilliesDto dto)
        {

            var file = await _context.Files.FirstOrDefaultAsync(f => f.Id == dto.Id);
            if (file == null)
            {
                result.Status = false;
                result.Message = "Dosya Bulunamadı";
                return result;
            }

            _mapper.Map(dto, file);
            file.Updated = DateTime.Now;
            await _context.SaveChangesAsync();

            result.Status = true;
            result.Message = "Dosya başarıyla güncellendi.";
            return result;
        }

        [HttpDelete("{id}")]
        public ActionResult<ResultDto> DeleteFile(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == id);
            if (file == null)
            {
                return NotFound("Dosya Bulunamadı");
            }
            _context.Files.Remove(file);
            _context.SaveChanges();

            var result = new ResultDto
            {
                Status = true,
                Message = "Dosya başarıyla silindi."
            };
            return result;
        }
    }
}
