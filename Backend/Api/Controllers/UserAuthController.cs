using System;
using System.Collections.Generic;
using Api.Services;
using Domain;
using Domain.SubDomains.Authentication.Commands;
using Domain.SubDomains.Authentication.Contracts.Handlers;
using Domain.SubDomains.Authentication.Contracts.Repositories;
using Domain.SubDomains.Authentication.Entities;
using Domains.Authentication.Commands.UserAuthCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserAuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserAuthRepository _repository;
        private readonly IUserAuthHandler _handler;        
        public UserAuthController(TokenService tokenService, IUserAuthRepository repository, IUserAuthHandler handler)
        {
            _tokenService = tokenService;
            _repository = repository;
            _handler = handler;            
        }


        [HttpPost]
        public CommandResult Register(RegisterUserAuthCommand command)
        {
            CommandResult result = _handler.Register(command);
            return result;
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public CommandResult RegisterAdmin(RegisterAdminUserAuthCommand command)
        {
            CommandResult result = _handler.RegisterAdmin(command, User.Identity.Name);
            return result;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<UserAuth> GetInactivesFirstAccess()
        {
            var users = _repository.GetInactivesFirstAccess();
            return users;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public CommandResult ActivateFirstAccess(ActivateUserCommand command)
        {            
            CommandResult result = _handler.ActivateFirstAccess(command, User.Identity.Name);
            return result;
        }


        [HttpPost]
        public CommandResultToken Login(LoginUserAuthCommand command)
        {
            CommandResultToken result = _handler.Login(command.Username, command.Password);
            if (result.Success)            
                 result.Token = _tokenService.GenerateToken((UserAuth)result.Object);                          
               
            return result;
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public CommandResult UpdateRoleActive(UpdateRoleActiveCommand command)
        {
            CommandResult result = _handler.UpdateRoleActive(command, User.Identity.Name);            
            return result;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public CommandResult Delete(Guid id)
        {
            CommandResult result = _handler.Delete(id, User.Identity.Name);            
            return result;
        }


        [HttpGet]
        [Authorize(Roles="Admin")]
        public List<UserAuth> GetAll()
        {
            var users = _repository.GetAll();
            return users;
        }

        [HttpGet("{id}")]
        [Authorize]
        public UserAuth GetById(Guid id)
        {
            var user = _repository.GetById(id);
            return user;
        }

        [HttpGet("{name}")]
        [Authorize]
        public UserAuth GetByName(string name)
        {
            var user = _repository.GetByName(name);
            return user;
        }



        [HttpPost]
        [Authorize]
        public CommandResult UpdatePassword(UpdatePasswordUserCommand command)
        {
            CommandResult result = _handler.UpdatePassword(command, User.Identity.Name);           
            return result;
        }


        [HttpGet("{filter}")]
        [Authorize(Roles="Admin")]
        public List<UserAuth> Search(string filter = "")
        {            
            if (filter == "empty") filter = "";
            var result = _repository.Search(filter);
            return result;
        }

        

    }
}