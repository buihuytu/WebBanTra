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

        [HttpGet] 
        public async Task<IActionResult> GetAllCategories()
        {
            var listTrash = await (from c in _context.TblCategories where c.IsDelete == 1 select c).AsNoTracking().ToListAsync();
            int countTrash = listTrash.Count();
            var list = await (from c in _context.TblCategories where c.IsDelete != 1 select c).ToListAsync();
            return Ok(new {countTrash, list});
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
                return BadRequest($"Không tồn tại Thể loại có Id = {id}");
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
            return Ok("Deleted Successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(TblCategory category)
        {
            if(ModelState.IsValid)
            {
                String slug = XString.ToAscii(category.Name);
                CheckSlug check = new CheckSlug(_context);
                if(!check.KiemTraSlug("Category", slug, null))
                {
                    return BadRequest("Thể loại đã tồn tại");
                }
                category.Slug = slug;
                category.CreatedDate = DateTime.Now;
                category.UpdatedDate = DateTime.Now;
                category.IsDelete = 0;

                _context.TblCategories.Add(category);
                await _context.SaveChangesAsync();
                return Ok("Add Successfully");
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
                    return BadRequest("Thể loại đã tồn tại");
                }
                category.Slug = slug;
                category.CreatedDate = (from c in _context.TblCategories where c.Id == ID select c.CreatedDate).FirstOrDefault();
                category.CreatedBy = (from c in _context.TblCategories where c.Id == ID select c.CreatedBy).FirstOrDefault();
                category.UpdatedDate = DateTime.Now;

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Edit Successfully");
            }
            return BadRequest();
        }
    }
}
