using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Frontend.Interfaces;
using Frontend.Models;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _service;
        public HomeController(IUserService service)
        {
            _service = service;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register() => PartialView("_Register");

        [HttpPost]
        public async Task<DataResult> Register(UserRegister model)
        {
            if (!ModelState.IsValid)
                return new DataResult()
                { Success = false, Message = "Preencha o formulário corretamente.." };

            UserResult result = await _service.Register(model);

            if (result.Success == false)              
                return new DataResult()
                { Success = false, Message = result.Message };            

            return new DataResult()
            { Success = true, Message = string.Empty };
        }



        public IActionResult ConfirmedRegister() => PartialView("_Registered");

        public IActionResult Login() => PartialView("_Login");

        [HttpPost]
        public async Task<DataResult> Login(UserLogin model)
        {
            if (!ModelState.IsValid)
                return new DataResult()
                { Success = false, Message = "Preencha o formulário corretamente.." };

            UserResult result = await _service.Login(model);
            if (result.Success == false)                           
                return new DataResult()
                { Success = false, Message = result.Message };            

            RegistrarCookies(result);

            return new DataResult()
            { Success = true, Message = string.Empty };
        }




        public async void RegistrarCookies(UserResult result)
        {
            //cria coockie
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, result.Object.Username),
                new Claim(ClaimTypes.Role, result.Object.Role),
                new Claim("Token", result.Token),
            };

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20), // <-- tempo expiracao cookie
                IsPersistent = false,       // <-- Se o cookie permanece após fechar browser ou nao
                IssuedUtc = DateTime.Now,   // <-- Data/hora de persistencia do cookie                                
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
