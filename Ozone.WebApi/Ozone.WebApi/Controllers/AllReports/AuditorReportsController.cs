using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Ozone.Infrastructure.Shared.Services;
using System.Threading.Tasks;
using Ozone.Application.DTOs;
using Ozone.Application.Interfaces.Service;
using System.IO;
using Ozone.Infrastructure;
using System.Collections.Generic;
using Ozone.Application.Interfaces;
using System;

namespace Ozone.WebApi.Controllers.AllReports
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditorReportsController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        IAllDropdownService _AllDropdownService;
        ISecUserSessionService _userSessionService;
        IClientAuditVisitService _clientAuditVisitService;
        IAuditorReportsService _auditorReportsService;
        private readonly IJwtAuthManager _jwtAuthManager;


        //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
        public AuditorReportsController(
            // UserService userService,
            IConfiguration configuration,

            ISecUserSessionService userSessionService,
            IAuditorReportsService auditorReportsService,
       
            IJwtAuthManager jwtAuthManager,
            IAllDropdownService allDropdownService
            )
        {
            //this.userManager = userManager;
            // this._userService = userService;
            _configuration = configuration;
            this._AllDropdownService = allDropdownService;
            this._userSessionService = userSessionService;
            this._auditorReportsService = auditorReportsService;
            this._jwtAuthManager = jwtAuthManager;
           
        }

        //[Route("GetPagedAuditorReportsResponse")]
        //[HttpPost]
        //[Authorize]

        //public async Task<IActionResult> GetPagedAuditorReportsResponse(long id, PagedResponseModel model)
        //{
        //    var list = await _auditorReportsService.GetPagedLeadAuditorReportsModel(model);
        //    return new JsonResult(list);
        //}


        [Route("GetAuditorReports")]
        [HttpPost]
        [Authorize]
       
        public async Task<IActionResult> GetAuditorReports(IDictionary<string, string> keyValuePairs)
        {

            //string fromdate = null;
            //string todate = null;
            //string OrganizationId =null;
            //DateTime frmdate = Convert.ToDateTime(fromdate);
            //if (keyValuePairs.ContainsKey("fromdate"))
            //    fromdate = keyValuePairs["fromdate"].Split("T")[0];
            //if (keyValuePairs.ContainsKey("todate"))
            //    todate = keyValuePairs["todate"];
            //if (keyValuePairs.ContainsKey("OrganizationId"))
            //{ 
            //        OrganizationId = keyValuePairs["OrganizationId"];
            //}

            //var list = await _auditorReportsService.GetPagedLeadAuditorReportsModel(keyValuePairs);
            var list = await _auditorReportsService.AuditorReports(keyValuePairs);
            //var list = await _auditorReportsService.AuditorReports(fromdate, todate, OrganizationId);
            return new JsonResult(list);
        }



        [Route("GetClientProjectReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetClientProjectReport(IDictionary<string, string> keyValuePairs)
        {

            var list = await _auditorReportsService.GetClientProjectReport(keyValuePairs);
       
            return new JsonResult(list);
        }



        [Route("GetAuditorListReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetAuditorListReport(IDictionary<string, string> keyValuePairs)
        {

            var list = await _auditorReportsService.GetAuditorListReport(keyValuePairs);

            return new JsonResult(list);
        }

        [Route("GetAuditorScheduleReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetAuditorScheduleReport(IDictionary<string, string> keyValuePairs)
        {

            var list = await _auditorReportsService.GetAuditorScheduleReport(keyValuePairs);

            return new JsonResult(list);
        }


        [Route("GetScheduleOfAuditReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetScheduleOfAuditReport(IDictionary<string, string> keyValuePairs)
        {

            var list = await _auditorReportsService.GetScheduleOfAuditReport(keyValuePairs);

            return new JsonResult(list);
        }


        [Route("GetImpartialityReviewReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetImpartialityReviewReport(IDictionary<string, string> keyValuePairs)
        {

            var list = await _auditorReportsService.GetImpartialityReviewReport(keyValuePairs);

            return new JsonResult(list);
        }


        [Route("AuditorReportsHistory")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> AuditorReportsHistory(int id)
        {


            var list = await _auditorReportsService.AuditReportHistory(id);

            return new JsonResult(list);
        }
    

    [Route("GetAuditorReportsDetails")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetAuditorReportsDetails(IDictionary<string, string> keyValuePairs)
        {

           
            var list = await _auditorReportsService.AuditorReportsDetail(keyValuePairs);
  
            return new JsonResult(list);
        }


        [Route("GetCertifiedClientReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetCertifiedClientReport(IDictionary<string, string> keyValuePairs)
        {


            var list = await _auditorReportsService.GetScheduleOfAuditWithWindowPeriod(keyValuePairs);

            return new JsonResult(list);
        }
    }
}
