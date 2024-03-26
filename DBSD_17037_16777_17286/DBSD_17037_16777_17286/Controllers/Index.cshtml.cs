using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DBSD_17037_16777_17286.DAL.Infrastructure;
using DBSD_17037_16777_17286.DAL.Models;

namespace DBSD_17037_16777_17286.Controllers
{
    public class IndexModel : PageModel
    {
        private readonly DBSD_17037_16777_17286.DAL.Infrastructure.MacroDbContext _context;

        public IndexModel(DBSD_17037_16777_17286.DAL.Infrastructure.MacroDbContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Customers != null)
            {
                Customer = await _context.Customers
                .Include(c => c.Person).ToListAsync();
            }
        }
    }
}
