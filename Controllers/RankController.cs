using System.Linq;
using System.Threading.Tasks;
using GreenMaps.Areas.Identity.Data;
using GreenMaps.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenMaps.Controllers
{
    public class RankController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public RankController([FromServices] ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Recupera todos os usuários
            var usuarios = await _context.Users.Include(x => x.PontoColeta).OrderByDescending(x => x.PontoColeta.Count).ToListAsync();
            return View(usuarios);
        }
    }
}
