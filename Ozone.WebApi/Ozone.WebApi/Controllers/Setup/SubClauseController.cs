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
    public class SubClauseController : BaseApiController
    {


        ISubClause _subClause;


        public SubClauseController(
             ISubClause subClause
            )
        {

            this._subClause = subClause;

        }

        [Route("Create")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(SubClauseModel input)
        {
            var result = await _subClause.Create(input);
            return Ok(new Response { Status = result, Message = result });
        }


        [Route("GetPagedSubClause")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedSubClause(PagedResponseModel model)
        {
            var result = await _subClause.GetPagedSubClause(model);
            return new JsonResult(result);
        }

        [Route("SubClauseDeleteById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> SubClauseDeleteById(long id)
        {
            var result = await _subClause.SubClauseDeleteById(id);
            return Ok(new Response { Status = result, Message = result });
        }

        [Route("GetMainClause")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetMainClause(long id)
        {
            var List = await _subClause.GetAllClauseByStandard(id);
            return new JsonResult(List);
        }



    }
}
