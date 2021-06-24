using System.Linq;
using System.Threading.Tasks;
using GreenMaps.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenMaps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Recupera todos os pontos de coleta cadastrados
            var pontosDeColeta = await _context.PontoColeta
                .Include(p => p.TipoPonto)
                .Include(p => p.TipoLixos)
                .ToListAsync();

            return View(pontosDeColeta);
        }
    }
}
