using System;
using System.IO;
using System.Threading.Tasks;
using GreenMaps.Data;
using GreenMaps.Models;
using GreenMaps.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenMaps.Controllers
{
    public class PontoColetaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PontoColetaController([FromServices] ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            // Recupera a lista de pontos de coletas e mostra na tela
            var pontoColetas = await _context.PontoColeta.ToListAsync();
            return View(pontoColetas);
        }

        public async Task<IActionResult> Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(PontoColetaViewModel model)
        {
            // Verifica se a view model está válida
            if (ModelState.IsValid)
            {
                // Salva o arquivo no diretório de Imagens
                var nomeUnicoArquivo = UploadedFile(model);

                // Cria um ponto de coleta
                var pontoColeta = new PontoColeta()
                {
                    Imagem = nomeUnicoArquivo,
                    Latitude = double.Parse(model.Latitude.Replace('.', ',')),
                    Longitude = double.Parse(model.Longitude.Replace('.', ',')),
                    Nome = model.Nome
                };

                // Salva o ponto de coleta no banco de dados e direciona para index
                _context.PontoColeta.Add(pontoColeta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(PontoColetaViewModel model)
        {
            string nomeUnicoArquivo = null;
            // Verifica se a imagem está vazia
            if (model.Imagem != null)
            {
                // Pega o caminho relativo do diretório de imagens
                string pastaFotos = Path.Combine(webHostEnvironment.WebRootPath, "Imagens");
                // Cria um nome único para a imagem
                nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + model.Imagem.FileName;
                // Combina o caminho relativo com o nome
                string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
                // Salva o arquivo no diretório
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    model.Imagem.CopyTo(fileStream);
                }
            }
            return nomeUnicoArquivo;
        }
    }
}
