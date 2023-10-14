using DoGiaDung.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBanTra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly WebbantraContext _context;
        public ContactsController(WebbantraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var countTrash = _context.TblContacts.Where(m => m.IsDelete == 1).Count();
            var list = await _context.TblContacts.Where(m => m.IsDelete != 1).ToListAsync();
            return Ok(new { countTrash, list });
        }

        [HttpGet]
        [Route("Trash")]
        public async Task<IActionResult> GetTrash()
        {
            var list = await _context.TblContacts.Where(m => m.IsDelete == 1).ToListAsync();
            return Ok(list);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _context.TblContacts.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (contact == null)
            {
                return BadRequest(new { MessageStatus = 400, MessageCode = $"Không tồn tại Liên hệ có Id = {id}" });
            }
            return Ok(contact);
        }

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<IActionResult> GetContactByName(string name)
        {
            var list = await _context.TblContacts.Where(p => p.FullName.Contains(name)).ToListAsync();
            return Ok(list);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.TblContacts.FindAsync(id);
            if (contact == null)
            {
                return BadRequest();
            }
            _context.TblContacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok(new { MessageStatus = 200, MessageCode = "Deleted Successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(TblContact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedDate = DateTime.Now;
                contact.IsReply = 0;
                contact.IsDelete = 0;

                _context.TblContacts.Add(contact);
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Send Successfully" });
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> ReplyContact(int ID, TblContact contact)
        {
            if(ID != contact.Id)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                contact.CreatedDate = (from c in _context.TblContacts where c.Id == ID select c.CreatedDate).FirstOrDefault();
                contact.IsDelete = 0;
                contact.IsReply = 1;
                contact.RepliedDate = DateTime.Now;

                _context.Entry(contact).State= EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "Rely Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(DelTrash))]
        public async Task<IActionResult> DelTrash(int ID)
        {
            var contact = await _context.TblContacts.FindAsync(ID);
            if (contact == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                contact.IsDelete = 1;
                contact.CreatedDate = (from c in _context.TblContacts where c.Id == ID select c.CreatedDate).FirstOrDefault();
                contact.RepliedDate = DateTime.Now;

                _context.Entry(contact).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }

        [HttpGet(nameof(ReTrash))]
        public async Task<IActionResult> ReTrash(int ID)
        {
            var contact = await _context.TblContacts.FindAsync(ID);
            if (contact == null)
            {
                return BadRequest();
            }
            else if (ModelState.IsValid)
            {
                contact.IsDelete = 0;
                contact.CreatedDate = (from c in _context.TblContacts where c.Id == ID select c.CreatedDate).FirstOrDefault();
                contact.RepliedDate = DateTime.Now;

                _context.Entry(contact).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { MessageStatus = 200, MessageCode = "DelTrash Successfully" });
            }
            return BadRequest();
        }
    }
}
