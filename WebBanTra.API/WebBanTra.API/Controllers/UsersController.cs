﻿using DoGiaDung.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanTra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebbantraContext _context;
        public UsersController(WebbantraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var countTrash = await _context.TblUsers.Where(m => m.IsDelete == 1).CountAsync();
            var list = await _context.TblUsers.Where(m => m.IsDelete != 1).ToListAsync();
            return Ok(new { countTrash, list });
        }

        [HttpGet]
        [Route("GetTrash")]
        public async Task<IActionResult> GetTrash()
        {
            var list = await _context.TblUsers.Where(m => m.IsDelete == 1).ToListAsync();
            return Ok(list);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.TblUsers.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest($"Không tồn tại Tài khoản có Id = {id}");
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var list = await _context.TblUsers.Where(p => p.FullName.Contains(name)).ToListAsync();
            return Ok(list);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.TblUsers.FindAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            string fileName = user.Image;
            if(fileName != null)
            {
                System.IO.File.Delete("E:\\CNTTVA\\Nam4Ki1\\CNPM\\BTL\\WebBanTraAngular\\src\\assets\\admin-img\\user" + fileName);
            }
            _context.TblUsers.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { MessageStatus = 200, MessageCode = "Deleted Successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UserImage u)
        {
            if(ModelState.IsValid)
            {
                String avatar = XString.ToAscii(u.FullName);
                var user = new TblUser
                {
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Password = u.Password,
                    Role = u.Role,
                    Address = u.Address,
                    Email = u.Email,
                    Phone = u.Phone,
                    CreatedDate = DateTime.Now,
                    CreatedBy = u.CreatedBy,
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = u.UpdatedBy,
                    IsDelete = 0,
                    IsActive = u.IsActive,
                };
                if(u.FileImage != null)
                {
                    String fileName = avatar + u.FileImage.FileName.Substring(u.FileImage.FileName.LastIndexOf('.'));
                    var path = Path.Combine("E:\\CNTTVA\\Nam4Ki1\\CNPM\\BTL\\WebBanTraAngular\\src\\assets\\admin-img\\user", fileName);
                    using (var stream = System.IO.File.Create(path)) 
                    {
                        await u.FileImage.CopyToAsync(stream);
                    }
                    user.Image = fileName;
                }
                else
                {
                    user.Image = null;
                }
                _context.TblUsers.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Add Successfully" });
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> EditUser(int ID, [FromForm] UserImage u)
        {
            if (ModelState.IsValid)
            {
                String avatar = XString.ToAscii(u.FullName);
                var user = new TblUser
                {
                    Id = ID,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Password = u.Password,
                    Role = u.Role,
                    Address = u.Address,
                    Email = u.Email,
                    Phone = u.Phone,
                    CreatedDate = (from c in _context.TblUsers where c.Id == ID select c.CreatedDate).FirstOrDefault(),
                    CreatedBy = (from c in _context.TblUsers where c.Id == ID select c.CreatedBy).FirstOrDefault(),
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = u.UpdatedBy,
                    IsDelete = 0,
                    IsActive = u.IsActive,
                };
                if (u.FileImage != null)
                {
                    String fileName = avatar + u.FileImage.FileName.Substring(u.FileImage.FileName.LastIndexOf('.'));
                    var path = Path.Combine("E:\\CNTTVA\\Nam4Ki1\\CNPM\\BTL\\WebBanTraAngular\\src\\assets\\admin-img\\user", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await u.FileImage.CopyToAsync(stream);
                    }
                    user.Image = fileName;
                }
                else
                {
                    user.Image = (from c in _context.TblUsers where c.Id == ID select c.Image).FirstOrDefault();
                }

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Edit Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(DelTrash))]
        public async Task<IActionResult> DelTrash(int ID)
        {
            var user = await _context.TblUsers.FindAsync(ID);
            if (user == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                user.IsDelete = 1;
                user.IsActive = 0;
                user.CreatedDate = (from c in _context.TblUsers where c.Id == ID select c.CreatedDate).FirstOrDefault();
                user.CreatedBy = (from c in _context.TblUsers where c.Id == ID select c.CreatedBy).FirstOrDefault();
                user.UpdatedDate = DateTime.Now;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ReTrash))]
        public async Task<IActionResult> ReTrash(int ID)
        {
            var user = await _context.TblUsers.FindAsync(ID);
            if (user == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                user.IsDelete = 0;
                user.CreatedDate = (from c in _context.TblUsers where c.Id == ID select c.CreatedDate).FirstOrDefault();
                user.CreatedBy = (from c in _context.TblUsers where c.Id == ID select c.CreatedBy).FirstOrDefault();
                user.UpdatedDate = DateTime.Now;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ChangeStatus))]
        public async Task<IActionResult> ChangeStatus(int ID)
        {
            var user = await _context.TblUsers.FindAsync(ID);
            if (user == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                user.IsActive = (user.IsActive == 1) ? 0 : 1;
                user.UpdatedDate = DateTime.Now;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Change Active Successfully" });
            }
            return BadRequest();
        }
    }
}
