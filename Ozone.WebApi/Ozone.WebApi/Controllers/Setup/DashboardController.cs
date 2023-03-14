using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Authorization;

using Ozone.Infrastructure.Shared.Services;

using System.Threading.Tasks;

using Ozone.Application.DTOs;

using Ozone.Application.Interfaces.Service;

using System.IO;
using Ozone.Infrastructure;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.Interfaces;

namespace Ozone.WebApi.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {

        //IClientAuditVisitService _clientAuditService;
        //IDashboardService _dashboardService;



        //public DashboardController(IClientAuditVisitService clientAuditVisitService)
        //{
        //    this._clientAuditService = clientAuditVisitService;

        //}
       

     
            private readonly IConfiguration _configuration;

            IAllDropdownService _AllDropdownService;
            ISecUserSessionService _userSessionService;
            IProjectSA8000Service _ProjectSA8000Service;
            private readonly IJwtAuthManager _jwtAuthManager;
        IClientAuditVisitService _clientAuditService;
        IDashboardService _dashboardService;


        //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
        public DashboardController(
                // UserService userService,
                IConfiguration configuration,

                ISecUserSessionService userSessionService,
                IProjectSA8000Service libraryResourceService,
                IJwtAuthManager jwtAuthManager,
                IAllDropdownService allDropdownService,
                 IClientAuditVisitService clientAuditService,
                 IDashboardService dashboardService
                )
            {
                //this.userManager = userManager;
                // this._userService = userService;
                _configuration = configuration;
                this._AllDropdownService = allDropdownService;
                this._userSessionService = userSessionService;
                this._ProjectSA8000Service = libraryResourceService;
                this._jwtAuthManager = jwtAuthManager;
                 this._clientAuditService = clientAuditService;
            this._dashboardService = dashboardService;
        }


        [Route("GetDashboardData")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetDashboardData(long id, PagedResponseModel model)
        {
            var list = await _clientAuditService.ProjectStatus(id,model);
            return new JsonResult(list);


        }


        [HttpGet, DisableRequestSizeLimit]
        [Route("ProjectChangeRequestDownloadfile")]
        [Authorize]

        public async Task<IActionResult> ProjectChangeRequestDownloadfile(long id)
        {

            var result = await _dashboardService.ProjectChangeRequestDownloadfile(id);

            var fileName = result.FilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contenpe = result.ContentType;
            var fileNM = Path.GetFileName(fileName);

            return File(memory, contenpe, fileNM);
        }




        [HttpGet, DisableRequestSizeLimit]
        [Route("oldValuesDownloadfile")]
        [Authorize]

        public async Task<IActionResult> oldValuesDownloadfile(long id)
        {

            var result = await _dashboardService.oldValuesDownloadfile(id);

            var fileName = result.FilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contenpe = result.ContentType;
            var fileNM = Path.GetFileName(fileName);

            return File(memory, contenpe, fileNM);
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("NewValuesDownloadfile")]
        [Authorize]

        public async Task<IActionResult> NewValuesDownloadfile(long id)
        {

            var result = await _dashboardService.NewValuesDownloadfile(id);

            var fileName = result.FilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contenpe = result.ContentType;
            var fileNM = Path.GetFileName(fileName);

            return File(memory, contenpe, fileNM);
        }


    }
}
