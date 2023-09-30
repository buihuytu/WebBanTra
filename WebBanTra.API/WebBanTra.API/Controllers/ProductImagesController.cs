using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using WebBanTra.API.Models;

namespace WebBanTra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly WebbantraContext _context;
        public ProductImagesController(WebbantraContext context)
        {
            _context = context;
        }

        [HttpGet("productId")]
        public async Task<IActionResult> GetListImageByProductId(int productId)
        {
            var list = from p in _context.TblProducts
                      join i in _context.TblProductImages
                      on p.Id equals i.IdProduct
                      where i.IdProduct == productId
                      select new Image()
                      {
                          Id = i.Id,
                          Name = i.Name,
                          IdProduct = i.IdProduct,
                          NameProduct = p.Name
                      };
            await list.ToListAsync();
            return Ok(list);
        }

        [HttpDelete("productId/imageId")]
        public async Task<IActionResult> DeleteImageByProductId(int productId, int imageId)
        {
            var image = await _context.TblProductImages.Where(i => i.IdProduct == productId && i.Id == imageId).FirstOrDefaultAsync();
            if (image == null)
            {
                return BadRequest($"Không tồn tại Hình ảnh có Id = {imageId}");
            }
            string fileName = image.Name;
            if (fileName != null)
            {
                System.IO.File.Delete("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\product-images\\" + fileName);
            }
            _context.TblProductImages.Remove(image);
            await _context.SaveChangesAsync();
            return Ok("Delete Successfully");
        }

        [HttpPost]
        [Route("upload-anh")]
        public async Task<IActionResult> CreateSingleImage([FromForm] ImgProduct i)
        {
            if(i.FileImage != null)
            {
                int index;
                var strSlug = _context.TblProducts.Where(p => p.Id == i.IdProduct).First().Slug;
                int countImg = _context.TblProductImages.Where(pi => pi.IdProduct == i.IdProduct).Count();
                
                if(countImg != 0)
                {
                    string maxName = _context.TblProductImages.Where(pi => pi.IdProduct == i.IdProduct).OrderByDescending(pi => pi.Id).Take(1).First().Name;
                    string[] lstCharac = maxName.Split("-");
                    string[] lastCharac = (lstCharac[lstCharac.Length - 1]).Split(".");
                    index = Int16.Parse(lastCharac[0]);
                }
                else
                {
                    index = 0;
                }
                var productImage = new TblProductImage()
                {
                    IdProduct = i.IdProduct,
                };
                String fileName = strSlug + '-' + (index + 1).ToString() + i.FileImage[0].FileName.Substring(i.FileImage[0].FileName.LastIndexOf('.'));
                var path = Path.Combine("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\product-images", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await i.FileImage[0].CopyToAsync(stream);
                }
                productImage.Name = fileName;
                _context.TblProductImages.Add(productImage);
                await _context.SaveChangesAsync();
                return Ok(new { strSlug, fileName });
            }
            return BadRequest("Không có ảnh được gửi lên");
        }

        [HttpPost]
        [Route("upload-nhieu-anh")]
        public async Task<IActionResult> CreateListImage([FromForm] ImgProduct i)
        {
            if(i.FileImage.Length > 0)
            {
                int index;
                var strSlug = _context.TblProducts.Where(p => p.Id == i.IdProduct).First().Slug;
                int countImg = _context.TblProductImages.Where(pi => pi.IdProduct == i.IdProduct).Count();
                if (countImg != 0)
                {
                    string maxName = _context.TblProductImages.Where(pi => pi.IdProduct == i.IdProduct).OrderByDescending(pi => pi.Id).Take(1).First().Name;
                    string[] lstCharac = maxName.Split("-");
                    string[] lastCharac = (lstCharac[lstCharac.Length - 1]).Split(".");
                    index = Int16.Parse(lastCharac[0]);
                }
                else
                {
                    index = 0;
                }
                int stt = 1;
                foreach(var file in i.FileImage)
                {
                    var productImage = new TblProductImage()
                    {
                        IdProduct = i.IdProduct,
                    };
                    String fileName = strSlug + '-' + (index + stt).ToString() + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    var path = Path.Combine("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\product-images", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }
                    productImage.Name = fileName;
                    _context.TblProductImages.Add(productImage);
                    await _context.SaveChangesAsync();
                    stt++;
                }
                return Ok("Add successfully");
            }
            return BadRequest("Không có ảnh được gửi lên");
        }

        [HttpPut("id")]
        public async Task<IActionResult> EditImage(int ID, [FromForm] ImgProduct i)
        {
            if (i.FileImage != null)
            {
                
                String fileName = _context.TblProductImages.Where(p => p.Id == ID && p.IdProduct == i.IdProduct).First().Name;
                var productImage = new TblProductImage()
                {
                    Id = ID,
                    IdProduct = i.IdProduct,
                    Name = fileName,
                };
                var path = Path.Combine("D:\\Ki_2-Nam_3\\ThucTapChuyenNganh\\WebBanTra\\WebBanTra\\Public\\Admin\\Pictures\\product-images", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await i.FileImage[0].CopyToAsync(stream);
                }

                _context.Entry(productImage).State = EntityState.Detached;
                await _context.SaveChangesAsync();
                return Ok("Edit Successfully");
            }
            return BadRequest("Không có ảnh được gửi lên");
        }
    }
}
