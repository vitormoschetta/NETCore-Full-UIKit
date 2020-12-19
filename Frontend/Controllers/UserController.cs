using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Frontend.Interfaces;
using Frontend.Models;
using Frontend.Utils;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        public async Task<IActionResult> LiberarAcesso(int? pageNumber)
        {
            ViewBag.Mensagem = TempData.Get<string>("mensagem");

            var lista = await _service.GetInactivesFirstAccess();
            var itensPorPagina = 5;
            PaginatedList<User> listaPaginada = PaginatedList<User>.Create(lista, pageNumber ?? 1, itensPorPagina);

            return View(listaPaginada);
        }

        public async Task<IActionResult> LiberarAcessoConfirmar(Guid id)
        {
            UserResult result = await _service.ActivateFirstAccess(id);
            TempDataUtil.Put(TempData, "mensagem", result.Message);
            return RedirectToAction("LiberarAcesso");
        }

        public async Task<IActionResult> ExcluirPedidoAcesso(Guid id)
        {
            UserResult result = result = await _service.Delete(id);
            TempDataUtil.Put(TempData, "mensagem", result.Message);
            return RedirectToAction("LiberarAcesso");
        }


        public async Task<IActionResult> ExcluirUsuario(Guid id)
        {
            var user = await _service.GetById(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirUsuario(User model)
        {
            UserResult result = result = await _service.Delete(model.Id);
            TempDataUtil.Put(TempData, "mensagem", result.Message);
            return RedirectToAction("ListaUsuarios");
        }


        public async Task<IActionResult> ListaUsuarios(int? pageNumber)
        {
            ViewBag.Mensagem = TempData.Get<string>("mensagem");

            var lista = await _service.GetAll();
            var itensPorPagina = 5;
            PaginatedList<User> listaPaginada = PaginatedList<User>.Create(lista, pageNumber ?? 1, itensPorPagina);

            return View(listaPaginada);
        }

        public async Task<IActionResult> EditarAcessoUsuario(Guid id)
        {
            var user = await _service.GetById(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditarAcessoUsuario(User model)
        {
            UserResult result = await _service.UpdateRoleActive(model);
            if (result.Success == false)
            {                
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempDataUtil.Put(TempData, "mensagem", result.Message);
            return RedirectToAction("ListaUsuarios");
        }


        public IActionResult AdicionarUsuario() => View();

        [HttpPost]
        public async Task<IActionResult> AdicionarUsuario(UserRegister model)
        {
            if (!ModelState.IsValid) return View(model);

            UserResult result = await _service.RegisterAdmin(model);
            if (result.Success == false)
            {                
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempDataUtil.Put(TempData, "mensagem", result.Message);
            return RedirectToAction("ListaUsuarios");
        }


        public async Task<IActionResult> PaginationSearch(string filter, int? pageNumber)
        {
            if (filter == "" || filter == null) filter = "empty";
            var lista = await _service.Search(filter);
            var itensPorPagina = 5;
            PaginatedList<User> listaPaginada = PaginatedList<User>.Create(lista, pageNumber ?? 1, itensPorPagina);
            return PartialView("_TabelaUsuarios", listaPaginada);
        }

        public async Task<IActionResult> PaginationSearchRequestAccess(string filter, int? pageNumber)
        {
            if (filter == "" || filter == null) filter = "empty";
            var lista = await _service.SearchRequestAccess(filter);
            var itensPorPagina = 5;
            PaginatedList<User> listaPaginada = PaginatedList<User>.Create(lista, pageNumber ?? 1, itensPorPagina);
            return PartialView("_TabelaLiberarAcesso", listaPaginada);
        }
    }
}