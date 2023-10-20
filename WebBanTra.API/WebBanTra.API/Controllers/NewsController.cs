using DoGiaDung.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace WebBanTra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly WebbantraContext _context;
        public NewsController(WebbantraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var countTrash = await _context.TblNews.Where(m => m.IsDelete == 1).CountAsync();
            var list = await _context.TblNews.Where(m => m.IsDelete != 1).ToListAsync();
            return Ok(new { countTrash, list });
        }

        [HttpGet]
        [Route("GetNewsList/pageNo/pageSize")]
        public async Task<IActionResult> GetAllNewsUI(int pageNo, int pageSize)
        {
            var countTrash = await _context.TblNews.Where(m => m.IsDelete == 1).CountAsync();
            var list = _context.TblNews.Where(m => m.IsDelete != 1);
            int totalNews = list.Count();
            list = list.Skip((pageNo - 1) * pageSize).Take(pageSize);
            await list.ToListAsync();
            return Ok(new { countTrash = countTrash, totalNews = totalNews, list = list });
        }

        [HttpGet]
        [Route("GetTrash")]
        public async Task<IActionResult> GetTrash()
        {
            var list = await _context.TblNews.Where(m => m.IsDelete == 1).ToListAsync();
            return Ok(list);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetNewById(int id)
        {
            var post = await _context.TblNews.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (post == null)
            {
                return BadRequest($"Không tồn tại Tin tức có Id = {id}");
            }
            return Ok(post);
        }

        [HttpGet]
        [Route("GetNews/{slug}")]
        public async Task<IActionResult> GetNewBySlug(string slug)
        {
            var post = await _context.TblNews.Where(p => p.Slug == slug).FirstOrDefaultAsync();
            if (post == null)
            {
                return BadRequest($"Không tồn tại Tin tức có Slug = {slug}");
            }
            return Ok(post);
        }

        [HttpGet]
        [Route("GetOtherPost/{id}")]
        public async Task<IActionResult> GetOtherPost(int id)
        {
            var list = await _context.TblNews.Where(p => p.IsActive == 1 && p.Id != id).OrderByDescending(p => p.CreatedDate).Take(5).ToListAsync();
            return Ok(list);
        }

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<IActionResult> GetNewByName(string name)
        {
            var list = await _context.TblNews.Where(p => p.Name.Contains(name)).ToListAsync();
            return Ok(list);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteNew(int id)
        {
            var post = await _context.TblNews.FindAsync(id);
            if (post == null)
            {
                return BadRequest($"Không tồn tại Tin tức có Id = {id}");
            }
            string fileName = post.Image;
            if (fileName != null)
            {
                System.IO.File.Delete("D:\\2023-2024\\CDCNPM\\WebBanTraAngular\\src\\assets\\news\\" + fileName);
            }
            _context.TblNews.Remove(post);
            await _context.SaveChangesAsync();
            return Ok(new { MessageStatus = 200, MessageCode = "Delete Successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNew([FromForm] NewImage n)
        {
            if(ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(n.Name);
                var post = new TblNew
                {
                    Name = n.Name,
                    Detail = n.Detail,
                    Summary = n.Summary,
                    Slug = strSlug,
                    MetaTitle = n.MetaTitle,
                    MetaKey = n.MetaKey,
                    MetaDesc = n.MetaDesc,
                    CreatedDate = DateTime.Now,
                    CreatedBy = n.CreatedBy,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = n.UpdatedBy,
                    IsDelete = 0,
                    IsActive = n.IsActive,
                };
                if(n.FileImage != null)
                {
                    String fileName = strSlug + n.FileImage.FileName.Substring(n.FileImage.FileName.LastIndexOf('.'));
                    var path = Path.Combine("D:\\2023-2024\\CDCNPM\\WebBanTraAngular\\src\\assets\\news", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await n.FileImage.CopyToAsync(stream);
                    }
                    post.Image = fileName;
                }
                else
                {
                    post.Image = null;
                }
                _context.TblNews.Add(post);
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Add Successfully" });
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> EditNew(int id, [FromForm] NewImage n)
        {
            if(ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(n.Name);
                var post = new TblNew
                {
                    Id = id,
                    Name = n.Name,
                    Detail = n.Detail,
                    Summary = n.Summary,
                    Slug = strSlug,
                    MetaTitle = n.MetaTitle,
                    MetaKey = n.MetaKey,
                    MetaDesc = n.MetaDesc,
                    CreatedDate = (from c in _context.TblNews where c.Id == id select c.CreatedDate).FirstOrDefault(),
                    CreatedBy = (from c in _context.TblNews where c.Id == id select c.CreatedBy).FirstOrDefault(),
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = n.UpdatedBy,
                    IsDelete = 0,
                    IsActive = n.IsActive,
                };
                if (n.FileImage != null)
                {
                    String fileName = strSlug + n.FileImage.FileName.Substring(n.FileImage.FileName.LastIndexOf('.'));
                    var path = Path.Combine("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\news", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await n.FileImage.CopyToAsync(stream);
                    }
                    post.Image = fileName;
                }
                else
                {
                    post.Image = (from c in _context.TblNews where c.Id == id select c.Image).FirstOrDefault();
                }
                _context.Entry(post).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Edit Successfully");
            }
            return BadRequest();
        }

        [HttpGet(nameof(DelTrash))]
        public async Task<IActionResult> DelTrash(int ID)
        {
            var n = await _context.TblNews.FindAsync(ID);
            if (n == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                n.IsDelete = 1;
                n.IsActive = 0;
                n.CreatedDate = (from c in _context.TblNews where c.Id == ID select c.CreatedDate).FirstOrDefault();
                n.CreatedBy = (from c in _context.TblNews where c.Id == ID select c.CreatedBy).FirstOrDefault();
                n.UpdatedDate = DateTime.Now;

                _context.Entry(n).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ReTrash))]
        public async Task<IActionResult> ReTrash(int ID)
        {
            var p = await _context.TblNews.FindAsync(ID);
            if (p == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                p.IsDelete = 0;
                p.CreatedDate = (from c in _context.TblNews where c.Id == ID select c.CreatedDate).FirstOrDefault();
                p.CreatedBy = (from c in _context.TblNews where c.Id == ID select c.CreatedBy).FirstOrDefault();
                p.UpdatedDate = DateTime.Now;

                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ChangeStatus))]
        public async Task<IActionResult> ChangeStatus(int ID)
        {
            var p = await _context.TblNews.FindAsync(ID);
            if (p == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                p.IsActive = (p.IsActive == 1) ? 0 : 1;
                p.UpdatedDate = DateTime.Now;
                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Change Active Successfully" });
            }
            return BadRequest();
        }
    }
}
