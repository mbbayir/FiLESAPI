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
        public ResultDto CreateFolder(FolderDto dto)
        {
            var result = new ResultDto();

            if (_context.Folders.Any(f => f.FolderName == dto.FolderName))
            {
                result.Status = false;
                result.Message = "Girilen dosya adı kayıtlıdır";
                return result;
            }

            var folder = new Folder
            {
                FolderName = dto.FolderName,
                Updated = DateTime.Now,
                Created = DateTime.Now
            };

            _context.Folders.Add(folder);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Klasör eklendi";
            return result;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]

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
            folder.FolderName = dto.FolderName;
            folder.Updated = DateTime.Now;

            _context.Folders.Update(folder);
            _context.SaveChanges();

            result.Status = true;
            result.Message = "Klasör düzenlendi";
            return result;
        }

        [HttpDelete]
        
        [Authorize(Roles = "Admin")]
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





    }
}
