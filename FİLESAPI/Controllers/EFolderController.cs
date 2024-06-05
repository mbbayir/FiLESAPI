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

        ResultDto result = new ResultDto();

        public EFolderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetFolderContents/{id}")]
        public IActionResult GetFolderContents(int id)
        {
            var folder = _context.Folders.Include(f => f.Files).FirstOrDefault(f => f.Id == id);
            if (folder == null)
            {
                return NotFound(new ResultDto { Status = false, Message = "Folder not found" });
            }

            var folderContents = new FolderContentsDto
            {
                FolderName = folder.FolderName,
                Files = _mapper.Map<List<FilliesDto>>(folder.Files),
                SubFolders = _mapper.Map<List<FolderDto>>(_context.Folders.Where(f => f.ParentFolderId == id).ToList())
            };

            return Ok(folderContents);
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

    }
}

