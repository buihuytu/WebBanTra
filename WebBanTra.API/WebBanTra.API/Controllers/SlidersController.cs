using DoGiaDung.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanTra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly WebbantraContext _context;
        public SlidersController(WebbantraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSliders()
        {
            var countTrash = _context.TblSliders.Where(m => m.IsDelete == 1).Count();
            var list = await _context.TblSliders.Where(m => m.IsDelete != 1).ToListAsync();
            return Ok(new { countTrash, list });
        }

        [HttpGet]
        [Route("Trash")]
        public async Task<IActionResult> GetTrash()
        {
            var list = await _context.TblSliders.Where(m => m.IsDelete == 1).ToListAsync();
            return Ok(list);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetSliderById(int id)
        {
            var contact = await _context.TblSliders.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (contact == null)
            {
                return BadRequest($"Không tồn tại Slider có Id = {id}");
            }
            return Ok(contact);
        }

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<IActionResult> GetSliderByName(string name)
        {
            var list = await _context.TblSliders.Where(p => p.Name.Contains(name)).ToListAsync();
            return Ok(list);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var slider = await _context.TblSliders.FindAsync(id);
            if (slider == null)
            {
                return BadRequest();
            }
            string fileName = slider.Image;
            System.IO.File.Delete("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\sliders\\" + fileName);
            _context.TblSliders.Remove(slider);
            await _context.SaveChangesAsync();
            return Ok("Deleted Successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlider([FromForm] SliderImage s)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(s.Name);
                var slider = new TblSlider
                {
                    Name = s.Name,
                    Url = strSlug,
                    Description = s.Description,
                    Position = s.Position,
                    CreatedDate = DateTime.Now,
                    CreatedBy = s.CreatedBy,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = s.UpdatedBy,
                    IsDelete = 0,
                    IsActive = s.IsActive,
                };
                if(s.FileImage != null)
                {
                    String fileName = strSlug + s.FileImage.FileName.Substring(s.FileImage.FileName.LastIndexOf("."));
                    var path = Path.Combine("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\sliders", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await s.FileImage.CopyToAsync(stream);
                    }
                    slider.Image = fileName;
                }
                else
                {
                    slider.Image = null;
                }
                _context.TblSliders.Add(slider);
                await _context.SaveChangesAsync();
                return Ok("Add Successfully");
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> EidtSlider(int ID, [FromForm] SliderImage s)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(s.Name);
                var slider = new TblSlider
                {
                    Id = ID,
                    Name = s.Name,
                    Url = strSlug,
                    Description = s.Description,
                    Position = s.Position,
                    CreatedDate = (from c in _context.TblSliders where c.Id == ID select c.CreatedDate).FirstOrDefault(),
                    CreatedBy = (from c in _context.TblSliders where c.Id == ID select c.CreatedBy).FirstOrDefault(),
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = s.UpdatedBy,
                    IsDelete = 0,
                    IsActive = s.IsActive,
                };
                if (s.FileImage != null)
                {
                    String fileName = strSlug + s.FileImage.FileName.Substring(s.FileImage.FileName.LastIndexOf("."));
                    var path = Path.Combine("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\sliders", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await s.FileImage.CopyToAsync(stream);
                    }
                    slider.Image = fileName;
                }
                else
                {
                    slider.Image = (from c in _context.TblSliders where c.Id == ID select c.Image).FirstOrDefault();
                }
                _context.Entry(slider).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Edit Successfully");
            }
            return BadRequest();
        }

    }
}
