using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Ozone.Application.DTOs;
using Ozone.Application.Interfaces.Service;
using Ozone.Application.Interfaces.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ozone.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Ozone.WebApi.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : BaseApiController
    {
        private readonly IConfiguration _configuration;


        ISecUserSessionService _userSessionService;
        ICityService _CityService;
        private readonly IJwtAuthManager _jwtAuthManager;
        IAllDropdownService _AllDropdownService;
        //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
        public CityController(
            // UserService userService,
            IConfiguration configuration,

            ISecUserSessionService userSessionService,
            ICityService CityService,
            IJwtAuthManager jwtAuthManager,
            IAllDropdownService allDropdownService
            )
        {
            //this.userManager = userManager;
            // this._userService = userService;
            _configuration = configuration;
            // this._authService = authService;
            this._userSessionService = userSessionService;
            this._CityService = CityService;
            this._jwtAuthManager = jwtAuthManager;
            this._AllDropdownService = allDropdownService;
        }
        [Route("Create")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(CityModel input)
        {

            var result = await _CityService.Create(input);
            return Ok(new Response { Status = result, Message = result });



        }

        [Route("GetPagedCity")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedCity(PagedResponseModel model)
        {
            var list = await _CityService.GetPagedCityResponse(model);
            return new JsonResult(list);
        }

        [Route("GetCityDataById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetCityDataById(int id)
        {
            var list = await _CityService.GetCityBYId(id);
            return new JsonResult(list);


        }

        [HttpPost]
        [Route("CityDeleteById")]
        [Authorize]

        public async Task<IActionResult> CityDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _CityService.CityDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }
        [Route("GetState")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetState(long id)
        {
            var List = await _AllDropdownService.GetAllStateBycountry(id);
            return new JsonResult(List);
        }


        [Route("GetCities")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetCities(long id)
        {
            var List = await _AllDropdownService.GetAllCityByState(id);
            return new JsonResult(List);
        }
    }
}
