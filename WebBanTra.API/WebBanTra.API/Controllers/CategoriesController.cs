using DoGiaDung.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanTra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly WebbantraContext _context;
        public CategoriesController(WebbantraContext context)
        {
            _context = context;
        }

        [HttpGet(nameof(GetAllCategories))] 
        public async Task<IActionResult> GetAllCategories()
        {
            var listTrash = await (from c in _context.TblCategories where c.IsDelete == 1 select c).AsNoTracking().ToListAsync();
            int countTrash = listTrash.Count();
            var list = await (from c in _context.TblCategories where c.IsDelete != 1 select c).ToListAsync();
            return Ok(new {count = countTrash, list = list});
        }

        [HttpGet]
        [Route("GetTrash")]
        public async Task<IActionResult> GetTrash()
        {
            var list = await _context.TblCategories.Where(m => m.IsDelete == 1).AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.TblCategories.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return BadRequest(new { MessageStatus = 200, MessageCode = $"Không tồn tại Thể loại có Id = {id}" });
            }
            return Ok(category);
        }

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var list = await _context.TblCategories.Where(p => p.Name.Contains(name)).ToListAsync();
            return Ok(list);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.TblCategories.FindAsync(id);
            if (category == null)
            {
                return BadRequest();
            }
            _context.TblCategories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(new { MessageStatus = 200, MessageCode = "Deleted Successfully"});
        }

        [HttpPost(nameof(CreateCategory))]
        public async Task<IActionResult> CreateCategory(TblCategory category)
        {
            if(ModelState.IsValid)
            {
                String slug = XString.ToAscii(category.Name);
                CheckSlug check = new CheckSlug(_context);
                if(!check.KiemTraSlug("Category", slug, null))
                {
                    return Ok(new { MessageStatus = 200, MessageCode = "Thể loại đã tồn tại" });
                }
                category.Slug = slug;
                category.CreatedDate = DateTime.Now;
                category.UpdatedDate = DateTime.Now;
                category.IsDelete = 0;

                _context.TblCategories.Add(category);
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Add Successfully" });
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> EditCategory(int ID, TblCategory category)
        {
            if (category.Id != ID) 
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                String slug = XString.ToAscii(category.Name);
                CheckSlug check = new CheckSlug(_context);
                if (!check.KiemTraSlug("Category", slug, null))
                {
                    return BadRequest(new { MessageStatus = 200, MessageCode = "Thể loại đã tồn tại" });
                }
                category.Slug = slug;
                category.CreatedDate = (from c in _context.TblCategories where c.Id == ID select c.CreatedDate).FirstOrDefault();
                category.CreatedBy = (from c in _context.TblCategories where c.Id == ID select c.CreatedBy).FirstOrDefault();
                category.UpdatedDate = DateTime.Now;

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Edit Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(DelTrash))]
        public async Task<IActionResult> DelTrash(int ID)
        {
            var category = await _context.TblCategories.FindAsync(ID);
            if (category == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                category.IsDelete = 1;
                category.IsActive = 0;
                category.CreatedDate = (from c in _context.TblCategories where c.Id == ID select c.CreatedDate).FirstOrDefault();
                category.CreatedBy = (from c in _context.TblCategories where c.Id == ID select c.CreatedBy).FirstOrDefault();
                category.UpdatedDate = DateTime.Now;

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ReTrash))]
        public async Task<IActionResult> ReTrash(int ID)
        {
            var category = await _context.TblCategories.FindAsync(ID);
            if (category == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                category.IsDelete = 0;
                category.CreatedDate = (from c in _context.TblCategories where c.Id == ID select c.CreatedDate).FirstOrDefault();
                category.CreatedBy = (from c in _context.TblCategories where c.Id == ID select c.CreatedBy).FirstOrDefault();
                category.UpdatedDate = DateTime.Now;

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ChangeStatus))]
        public async Task<IActionResult> ChangeStatus(int ID)
        {
            var category = await _context.TblCategories.FindAsync(ID);
            if(category == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                category.IsActive = (category.IsActive == 1) ? 0 : 1;
                category.UpdatedDate = DateTime.Now;
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Change Active Successfully" });
            }
            return BadRequest();
        }
    }
}
