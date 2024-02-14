using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

using Ozone.Infrastructure.Shared.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ozone.Infrastructure.Persistence;
using Ozone.Application.DTOs;
//using Ozone.Application.Interfaces.Service;
using Ozone.Infrastructure.Shared;
using Ozone.Infrastructure;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
//using Microsoft.AspNetCore.Authorization;
using Ozone.Infrastructure.Persistence.Models;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using Ozone.Application.Interfaces.Service;
using Ozone.Application.Interfaces;
using Ozone.Application;
using System.IO;
using AutoMapper;
using Ozone.Application.Interfaces.Setup;
using Ozone.Application.DTOs.Projects;

namespace Ozone.WebApi.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainClauseController : BaseApiController
    {



     
        IMainClause _mainClause;


        public MainClauseController(
             IMainClause mainClause
            )
        {
            
             this._mainClause = mainClause;

        }

        [Route("Create")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(MainClauseModel input)
        {
            var result = await _mainClause.Create(input);
            return Ok(new Response { Status = result, Message = result });
        }

        [Route("GetPagedMainClause")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedMainClause(PagedResponseModel model)
        {
            var result = await _mainClause.GetPagedMainClause(model);
            return new JsonResult(result);
        }


        [Route("MainClauseDeleteById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> MainClauseDeleteById(long id)
        {
            var result = await _mainClause.MainClauseDeleteById(id);
            return Ok(new Response { Status =result, Message = result });
        }


        //[Route("GetPagedAgency")]
        //[HttpPost]
        //[Authorize]

        //public async Task<IActionResult> GetPagedAgency(PagedResponseModel model)
        //{
        //    var list = await _AgensyService.GetPagedAgency(model);
        //    return new JsonResult(list);
        //}


    }
}
