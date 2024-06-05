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
    public class EFolderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EFolderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetFolderContents")]
        public IActionResult GetFolderContents()
        {
            
            var folder = _context.Folders
                .Include(f => f.Files) // Files ilişkisel verilerini yükle
                .Include(f => f.SubFolders) // SubFolders ilişkisel verilerini yükle
                .ToList();

            
          
            return Ok(folder);
        }



        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ResultDto { Status = false, Message = "Please select a file." });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new ResultDto { Status = true, Message = "File uploaded successfully." });
        }

        [HttpGet("Download/{id}")]
        public IActionResult Download(int id)
        {
            var folder = _context.Folders.SingleOrDefault(f => f.Id == id);
            if (folder == null)
            {
                return NotFound(new ResultDto { Status = false, Message = "Folder not found" });
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", folder.FolderName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new ResultDto { Status = false, Message = "File not found" });
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", Path.GetFileName(filePath));
        }

        [HttpPut("Rename/{id}")]
        public async Task<IActionResult> Rename(int id, [FromBody] RenameDto renameDto)
        {
            var folder = _context.Folders.SingleOrDefault(f => f.Id == id);
            if (folder == null)
            {
                return NotFound(new ResultDto { Status = false, Message = "Folder not found" });
            }

            folder.FolderName = renameDto.NewName;
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();

            return Ok(new ResultDto { Status = true, Message = "Folder renamed successfully" });
        }

        [HttpPut("Move/{id}")]
        public async Task<IActionResult> Move(int id, [FromBody] MoveDto moveDto)
        {
            var folder = await _context.Folders.SingleOrDefaultAsync(f => f.Id == id);
            if (folder == null)
            {
                return NotFound(new ResultDto { Status = false, Message = "Folder not found" });
            }

            folder.ParentFolderId = moveDto.NewParentFolderId;
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();

            return Ok(new ResultDto { Status = true, Message = "Folder moved successfully" });
        }

        [HttpGet("Search")]
        public IActionResult Search([FromQuery] SearchDto searchDto)
        {
            if (string.IsNullOrWhiteSpace(searchDto.Query))
            {
                return BadRequest(new ResultDto { Status = false, Message = "Search query cannot be empty" });
            }

            var matchingFolders = new List<Folder>();
            var matchingFiles = new List<Fillies>();

            if (searchDto.IncludeFolders)
            {
                matchingFolders = _context.Folders
                    .Include(f => f.Files)
                    .Include(f => f.SubFolders)
                    .Where(f => EF.Functions.Like(f.FolderName, $"%{searchDto.Query}%"))
                    .ToList();
            }

            if (searchDto.IncludeFiles)
            {
                matchingFiles = _context.Files
                    .Where(f => EF.Functions.Like(f.FileName, $"%{searchDto.Query}%"))
                    .ToList();
            }

            var searchResults = new
            {
                Folders = matchingFolders,
                Files = matchingFiles
            };
            return Ok(searchResults);
        }


    }
}
