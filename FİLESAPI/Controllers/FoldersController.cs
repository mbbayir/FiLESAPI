using AutoMapper;
using FİLESAPI.Dtos;
using FİLESAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FİLESAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        ResultDto result = new ResultDto();

        public FoldersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public List<FolderDto> GetFoldersList()
        {
            var Folders = _context.Folders.ToList();
            var FolderDto = _mapper.Map<List<FolderDto>>(Folders);
            return FolderDto;
        }


        [HttpGet("{id}")]
       
        public FolderDto GetFolder(int id)
        {
            var folder = _context.Folders.Where(f => f.Id == id).SingleOrDefault();
            var folderDto = _mapper.Map<FolderDto>(folder);
            return folderDto;
        }

        [HttpGet]
        [Route("GetFolderFiles")]
        public List<Folderfilles> GetFolderfilles() {
            var floder = _context.Folders.Include(x => x.Files).ToList();
            var folderdtos = _mapper.Map<List<Folderfilles>>(floder);
            return folderdtos;
        
        }
        [HttpPost]
        public async Task<string> CreateFolder(IFormFile FolderName)
        {
            if (FolderName != null && FolderName.Length > 0)
            {
                var guid = Guid.NewGuid();

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", guid + FolderName.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FolderName.CopyToAsync(stream);
                }

                var filebudur = guid + FolderName.FileName;
                var folder = new Folder { FolderName = filebudur, Created = DateTime.Now, Updated = DateTime.Now }; // Veritabanına eklenecek dosya adıyla yeni bir FolderDto oluşturuluyor
                _context.Folders.Add(folder);
                await _context.SaveChangesAsync(); // Değişikliklerin veritabanına kaydedilmesi

                result.Status = true;
                result.Message = "Dosya başarıyla eklendi";
                return result.Message;
            }

            return "hata";

        }
       
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public ResultDto UpdateFolder(FolderDto dto)
        {
            var result = new ResultDto();

            var folder = _context.Folders.SingleOrDefault(f => f.Id == dto.Id);

            if (folder == null)
            {
                result.Status = false;
                result.Message = "Klasör Bulunamadı!";
                return result;
            }
            //folder.FolderName = dto.FolderName;
            folder.Updated = DateTime.Now;

            _context.Folders.Update(folder);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Klasör düzenlendi";
            return result;
        }

        [HttpDelete("{id}")]
        
        //[Authorize(Roles = "Admin")]
        public ResultDto DeleteFolder(int id)
        {
            var result = new ResultDto();

            var folder = _context.Folders.SingleOrDefault(f => f.Id == id);
            if (folder == null)
            {
                result.Status = false;
                result.Message = "Klasör bulunamadı";
                return result;
            }
            _context.Folders.Remove(folder);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Klasör Silindi";
            return result;
        }

        [HttpGet("Download/{id}")]
        public IActionResult DownloadFile(int id)
        {
            var folder = _context.Folders.SingleOrDefault(f => f.Id == id);
            if (folder == null)
            {
                return NotFound("Klasör bulunamadı");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", folder.FolderName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Dosya bulunamadı");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", Path.GetFileName(filePath));
        }




    }
}
