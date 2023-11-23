using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Ozone.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

using Ozone.Application.Interfaces.Service;
using Ozone.Application.Interfaces.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ozone.Application.DTOs.Setup;
using Ozone.Infrastructure.Shared.Services;

namespace Ozone.WebApi.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditDocumentTypeController : BaseApiController
    {
       // private readonly IConfiguration _configuration;

       
        IAuditDocumentTypeService _auditDocumentTypeService;
        //private object _AuditDocumentTypeService;
        //private readonly IJwtAuthManager _jwtAuthManager;

        public AuditDocumentTypeController(
            IAuditDocumentTypeService auditDocumentTypeService
        )
        {
           // this._configuration = configuration;
          
            this._auditDocumentTypeService = auditDocumentTypeService;
           // this._jwtAuthManager = jwtAuthManager;
        }
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(AuditDocumetTypeModel input)
        {
            var result = await _auditDocumentTypeService.Create(input);
            return Ok(new Response { Status = result, Message = result });
        }

        [Route("GetPagedAuditDocumentType")]
        [HttpPost]

        public async Task<IActionResult> GetPagedAuditDocumentType(PagedResponseModel model)
        {
            var list = await _auditDocumentTypeService.GetPagedAuditDocumentTypeResponse(model);
            return new JsonResult(list);
        }
        [Route("GetAuditDocumentTypeDataById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAuditDocumentTypeDataById(int id)
        {
            var list = await _auditDocumentTypeService.GetAuditDocumetTypeBYId(id);
            return new JsonResult(list);


        }

        [HttpPost]
        [Route("AuditDocumentTypeDeleteById")]
        [Authorize]

        public async Task<IActionResult> AuditDocumentTypeDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditDocumentTypeService.AuditDocumetTypeDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }

    }
}