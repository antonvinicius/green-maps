using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GreenMaps.Areas.Identity.Data;
using GreenMaps.Data;
using GreenMaps.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GreenMaps.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }
        public int TotalPontosColetaCadastrados { get; set; }
        public ICollection<PontoColeta> PontosColeta { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefone")]
            public string PhoneNumber { get; set; }
            public string Logradouro { get; set; }
            public string Bairro { get; set; }
            public string Cep { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            // Recupera os pontos de coleta cadastrados pelo usuário
            var pontosColeta = await _context.PontoColeta.Include(x => x.TipoPonto).Where(p => p.Usuario.Id == user.Id).ToListAsync();
            PontosColeta = pontosColeta;

            TotalPontosColetaCadastrados = pontosColeta.Count;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Logradouro = user.Logradouro,
                Bairro = user.Bairro,
                Cep = user.Cep,
                Cidade = user.Cidade,
                Estado = user.Estado
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.Logradouro = Input.Logradouro;
            user.Bairro = Input.Bairro;
            user.Cep = Input.Cep;
            user.Cidade = Input.Cidade;
            user.Estado = Input.Estado;

            var atualizar = await _userManager.UpdateAsync(user);
            if (!atualizar.Succeeded)
            {
                StatusMessage = "Ocorreu um erro ao tentar atualizar o usuário.";
                return RedirectToPage();
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Seu perfil foi atualizado com sucesso!";
            return RedirectToPage();
        }
    }
}
