using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.Interfaces.Service;
using Ozone.Infrastructure;
using Ozone.Infrastructure.Shared.Services;
using Ozone.Infrastructure.Shared.Services.ClientALLProjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace Ozone.WebApi.Controllers.ClientALLProjects
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectGeneralFormController : Controller
    {
        private readonly IConfiguration _configuration;

        IAllDropdownService _AllDropdownService;
        ISecUserSessionService _userSessionService;
        IProjectGeneralFormService _projectGeneralFormService;
        private readonly IJwtAuthManager _jwtAuthManager;


        //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
        public ProjectGeneralFormController(
            // UserService userService,
            IConfiguration configuration,

            ISecUserSessionService userSessionService,
            IProjectGeneralFormService projectGeneralFormService,
            IJwtAuthManager jwtAuthManager,
            IAllDropdownService allDropdownService
            )
        {
            //this.userManager = userManager;
            // this._userService = userService;
            _configuration = configuration;
            this._AllDropdownService = allDropdownService;
            this._userSessionService = userSessionService;
            this._projectGeneralFormService = projectGeneralFormService;
            this._jwtAuthManager = jwtAuthManager;
        }

        //[Route("ProjectGeneralFormCreateWithFile")]
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> ProjectGeneralFormCreateWithFile([FromForm] ProjectGeneralFormModel input)
        //{

        //    var result = await _projectGeneralFormService.Create(input);
        //    return Ok(new Response { Status = result, Message = result });



        //}

        //[Route("GetPagedProjectSA8000")]
        //[HttpPost]
        //public async Task<IActionResult> GetPagedProjectSA8000(PagedResponseModel model)
        //{
        //    var list = await _projectSlcpService.getp(model);
        //    return new JsonResult(list);
        //}
        [Route("GetProjectGeneralFormBYId")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProjectGeneralFormBYId(long id)
        {
            var list = await _projectGeneralFormService.GetProjectGeneralFormBYId(id);
            return new JsonResult(list);


        }


        [HttpPost]
        [Route("ProjectGeneralFormDeleteById")]
        [Authorize]
        //  [Authorize]
        public async Task<IActionResult> ProjectGeneralFormDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _projectGeneralFormService.ProjectGeneralFormDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadApplicationForm")]
        [Authorize]
        public async Task<IActionResult> DownloadApplicationForm(long id)
        {

            var result = await _projectGeneralFormService.DownloadFile(id);
            //  var fileName = @"G:/OzoneDocuments/LibraryDocument/10_AD Requirement.txt";
            var fileName = result.ApplicationFormPath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            // var contenpe = "application/pdf";
            var contenpe = result.ApplicationContentType;
            var fileNM = Path.GetFileName(fileName);
            //   var net = new System.Net.WebClient();
            //  var data = net.DownloadData(@"D:/Update work/OT Booking.pdf");
            // var data = net.DownloadData(fname);
            // var content = new System.IO.MemoryStream(data);
            //  var contentType = "application/pdf";
            //var fileName = "OT Booking.pdf";
            return File(memory, contenpe, fileNM);
        }


        [Route("GetAllVerificationType")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllVerificationType()
        {
            var List = await _AllDropdownService.GetAllVerificationType();
            return new JsonResult(List);
        }
        [Route("GetAllProjectType")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProjectType()
        {
            var List = await _AllDropdownService.GetAllProjectType();
            return new JsonResult(List);
        }
        [Route("GetALLServicesType")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLServicesType()
        {
            var List = await _AllDropdownService.GetALLServicesType();
            return new JsonResult(List);
        }
        [Route("GetALLMethodology")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLMethodology()
        {
            var List = await _AllDropdownService.GetALLMethodology();
            return new JsonResult(List);
        }
        [Route("GetALLAssessmentCompleted")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLAssessmentCompleted()
        {
            var List = await _AllDropdownService.GetALLAssessmentCompleted();
            return new JsonResult(List);
        }

        [Route("GetALLCompletedModule")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLCompletedModule()
        {
            var List = await _AllDropdownService.GetALLCompletedModule();
            return new JsonResult(List);
        }
        [Route("GetALLAccreditation")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLAccreditation()
        {
            var List = await _AllDropdownService.GetAllAccreditation();
            return new JsonResult(List);
        }
        [Route("GetALLModuleVersion")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLModuleVersion(long id)
        {
            var List = await _AllDropdownService.GetALLModuleVersion(id);
            return new JsonResult(List);
        }
        [Route("GetALLModuleShare")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLModuleShare()
        {
            var List = await _AllDropdownService.GetALLModuleShare();
            return new JsonResult(List);
        }

        [Route("GetPagedPrjectRemarks")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetPagedPrjectRemarks(PagedResponseModel model)
        {
            var list = await _AllDropdownService.GetPagedProjectRemarks(model);
            return new JsonResult(list);
        }

        [HttpPost]
        [Route("CreateApproval")]
        [Authorize]
        public async Task<IActionResult> CreateApproval(ProjectRemarksHistoryModel input)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _projectGeneralFormService.Approval(input);
            return Ok(new Response { Status = result, Message = result });

        }
        [Route("GetALLRequestOfSite")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLRequestOfSite()
        {
            var List = await _AllDropdownService.GetALLRequestOfSite();
            return new JsonResult(List);
        }
        [Route("GetALLCompletedSetup")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLCompletedSetup()
        {
            var List = await _AllDropdownService.GetALLCompletedSetup();
            return new JsonResult(List);
        }

        [Route("GetALLProjectStatus")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALLProjectStatus(long id)
        {
            var List = await _AllDropdownService.GetALLProjectStatus(id);
            return new JsonResult(List);
        }
        [HttpPost]
        [Route("SubmitForReview")]
        [Authorize]
        //  [Authorize]
        public async Task<IActionResult> SubmitForReview(long id, long loginUserId)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _projectGeneralFormService.SubmitForReview(id, loginUserId);
            return Ok(new Response { Status = result, Message = result });

        }

        [Route("GetAllClientSites")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllClientSites(long id)
        {
            var List = await _AllDropdownService.GetAllClientSites(id);
            return new JsonResult(List);
        }


        [Route("ContractSubmit")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ContractSubmit([FromForm] ClientProjectModel input)
        {

            var result = await _projectGeneralFormService.ContractSubmit(input);
            return Ok(new Response { Status = result, Message = result });



        }
        [Route("ContractApproval")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ContractApproval([FromForm] ClientProjectModel input)
        {

            var result = await _projectGeneralFormService.ContractApproval(input);
            return Ok(new Response { Status = result, Message = result });



        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadContract")]
        [Authorize]
        public async Task<IActionResult> DownloadContract(long id)
        {

            var result = await _projectGeneralFormService.downloadContract(id);
            //  var fileName = @"G:/OzoneDocuments/LibraryDocument/10_AD Requirement.txt";
            var fileName = result.ContractFilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            // var contenpe = "application/pdf";
            var contenpe = result.ContractFileContent;
            var fileNM = Path.GetFileName(fileName);
            //   var net = new System.Net.WebClient();
            //  var data = net.DownloadData(@"D:/Update work/OT Booking.pdf");
            // var data = net.DownloadData(fname);
            // var content = new System.IO.MemoryStream(data);
            //  var contentType = "application/pdf";
            //var fileName = "OT Booking.pdf";
            return File(memory, contenpe, fileNM);
        }
        [Route("GetAllSmetaAuditPillars")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllSmetaAuditPillars()
        {
            var List = await _projectGeneralFormService.GetAllSmetaAuditPillars();
            return new JsonResult(List);
        }
    }
}
