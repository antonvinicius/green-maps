using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GreenMaps.Areas.Identity.Data;
using GreenMaps.Data;
using GreenMaps.Models;
using GreenMaps.Models.ViewModels;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GreenMaps.Controllers
{
    [Authorize]
    public class PontoColetaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<Usuario> _userManager;

        public PontoColetaController([FromServices] ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<Usuario> userManager)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Criar()
        {
            await SetDataCriarViewAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(PontoColetaViewModel model)
        {
            // Verifica se o usuário marcou o ponto de coleta no mapa
            if (model.Latitude == null || model.Longitude == null)
            {
                ModelState.AddModelError("lat_lng", "Você deve clicar no mapa para marcar a localização do ponto de coleta");
            }

            // Verifica se a view model está válida
            if (ModelState.IsValid)
            {
                // Salva o arquivo no diretório de Imagens
                var nomeUnicoArquivo = UploadedFile(model);

                // Recupera o Tipo de Ponto
                var tipoPonto = _context.TipoPonto.Find(model.TipoPontoId);

                // Cria um ponto de coleta
                var pontoColeta = new PontoColeta()
                {
                    Imagem = nomeUnicoArquivo,
                    Latitude = double.Parse(model.Latitude, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(model.Longitude, CultureInfo.InvariantCulture),
                    TipoPonto = tipoPonto,
                    Nome = model.Nome
                };

                // Vincula o usuário logado com o ponto de coleta
                var usuario = await _userManager.GetUserAsync(User);
                pontoColeta.Usuario = usuario;

                // Armazena os tipos de lixos vinculados ao ponto de coleta
                var tiposLixos = _context.TipoLixo.Where(t => model.TipoLixoIds.Contains(t.Id)).ToList();
                pontoColeta.TipoLixos = tiposLixos;

                // Salva o ponto de coleta no banco de dados e direciona para index
                _context.PontoColeta.Add(pontoColeta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            await SetDataCriarViewAsync();
            return View();
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            // Recupera o ponto de coleta do banco
            var pontoColeta = await _context.PontoColeta
                .Include(p => p.TipoLixos)
                .Include(p => p.TipoPonto)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pontoColeta == null)
                return NotFound();

            return View(pontoColeta);
        }

        private string UploadedFile(PontoColetaViewModel model)
        {
            string nomeUnicoArquivo = null;
            // Verifica se a imagem está vazia
            if (model.Imagem != null)
            {
                var optimizer = new ImageOptimizer();
                // Pega o caminho relativo do diretório de imagens
                string pastaFotos = Path.Combine(webHostEnvironment.WebRootPath, "Imagens");
                // Cria um nome único para a imagem
                nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + model.Imagem.FileName;
                // Combina o caminho relativo com o nome
                string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
                // Salva o arquivo no diretório
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    optimizer.Compress(fileStream);
                    model.Imagem.CopyTo(fileStream);
                }
            }
            return nomeUnicoArquivo;
        }

        private async Task SetDataCriarViewAsync()
        {
            // Recupera os tipos de lixo e os tipos de ponto
            var tiposLixos = await _context.TipoLixo.ToListAsync();
            var tiposPontos = await _context.TipoPonto.ToListAsync();

            // Seta a view bag para preencher os drop downs
            ViewBag.TiposLixo = new MultiSelectList(tiposLixos, "Id", "Nome");
            ViewBag.TiposPonto = new SelectList(tiposPontos, "Id", "Nome"); ;
        }
    }
}
