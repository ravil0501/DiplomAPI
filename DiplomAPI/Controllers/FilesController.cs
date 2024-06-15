using DiplomAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using File = DiplomAPI.Models.File;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly DiplomContext _context;

    public FilesController(DiplomContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileExport>>> GetFiles()
    {
        return await (from file in _context.Files
                      join user in _context.Users on file.Iduser equals user.UserId
                      select new FileExport { FileName = file.FileName, CreationDate = file.CreationDate, FileData = file.FileData, UserLogin = user.Login, GroupNumber = user.GroupNumber }).ToListAsync();
;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<File>> GetFile(int id)
    {
        var file = await _context.Files.FindAsync(id);

        if (file == null)
        {
            return NotFound();
        }

        return file;
    }

    [HttpGet("user/{userid}")]
    public async Task<ActionResult<IEnumerable<FileExport>>> GetFileByUserId(int userid)
    {
        var files = await (
            from file in _context.Files
            join user in _context.Users on file.Iduser equals user.UserId
            where file.Iduser == userid
            select new FileExport { FileName = file.FileName, FileId = file.FileId, CreationDate = file.CreationDate, FileData = file.FileData, UserLogin = user.Login, GroupNumber = user.GroupNumber }
        ).ToListAsync();

        if (files == null)
        {
            return NotFound();
        }

        return files;
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IEnumerable<FileExport>>> GetFilesByGroup(int groupId)
    {
        var files = await (
            from file in _context.Files
            join user in _context.Users on file.Iduser equals user.UserId
            where user.GroupNumber == groupId
            select new FileExport{FileName = file.FileName, FileId = file.FileId, CreationDate = file.CreationDate, FileData = file.FileData, UserLogin = user.Login, GroupNumber = user.GroupNumber}
        ).ToListAsync();
         
        if (files == null)
        {
            return NotFound();
        }
        return files;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFile(int id, File file)
    {
        if (id != file.FileId)
        {
            return BadRequest();
        }

        _context.Entry(file).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FileExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<File>> PostFile(File file)
    {
        _context.Files.Add(file);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFile", new { id = file.FileId }, file);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var file = await _context.Files.FindAsync(id);
        if (file == null)
        {
            return NotFound();
        }

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FileExists(int id)
    {
        return _context.Files.Any(e => e.FileId == id);
    }
}

