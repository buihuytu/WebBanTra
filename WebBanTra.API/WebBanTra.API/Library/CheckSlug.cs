using WebBanTra.API.Models;

namespace DoGiaDung.Library
{
    public class CheckSlug
    {
        private readonly WebbantraContext _context;
        public CheckSlug(WebbantraContext context)
        {
            _context = context;
        }

        public bool KiemTraSlug(String Table, String Slug, int? id)
        {
            switch(Table)
            {
                case "Category":
                    if(id != null)
                    {
                        if (_context.TblCategories.Where(m => m.Slug == Slug && m.Id == id).Count() > 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (_context.TblCategories.Where(m => m.Slug == Slug).Count() > 0)
                            return false;
                    }
                    break;
                case "News":
                    break;
                case "Product":
                    break;
            }
            return true;
        }
    }
}
