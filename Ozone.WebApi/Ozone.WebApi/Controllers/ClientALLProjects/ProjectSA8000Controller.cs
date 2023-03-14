﻿
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

namespace Ozone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectSA8000Controller : BaseApiController
    { 
        private readonly IConfiguration _configuration;

        IAllDropdownService _AllDropdownService;
        ISecUserSessionService _userSessionService;
    IProjectSA8000Service _ProjectSA8000Service;
    private readonly IJwtAuthManager _jwtAuthManager;


    //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
    public ProjectSA8000Controller(
        // UserService userService,
        IConfiguration configuration,

        ISecUserSessionService userSessionService,
        IProjectSA8000Service libraryResourceService,
        IJwtAuthManager jwtAuthManager,
        IAllDropdownService allDropdownService
        )
    {
        //this.userManager = userManager;
        // this._userService = userService;
        _configuration = configuration;
        this._AllDropdownService = allDropdownService;
        this._userSessionService = userSessionService;
        this._ProjectSA8000Service = libraryResourceService;
        this._jwtAuthManager = jwtAuthManager;
    }


        //User Academic

        [Route("ProjectSA8000CreateWithFile")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProjectSA8000CreateWithFile([FromForm] ProjectSA8000CreateModel input)
        {

            var result = await _ProjectSA8000Service.Create(input);
            return Ok(new Response { Status = result, Message = result });



        }




        [Route("SA8000ChangeRequest")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SA8000ChangeRequest([FromForm] ProjectSA8000Model input)
        {

            var result = await _ProjectSA8000Service.SA8000ChangeRequest(input);
            return Ok(new Response { Status = result, Message = result });



        }


        [Route("ApprovedProjectChange")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> ApprovedProjectChange(ActivityLogModel input)
        {

            var result = await _ProjectSA8000Service.ApprovedProjectChange(input);
            return Ok(new Response { Status = result, Message = result });


        }


        [Route("GetProjectSA8000RemarksDataById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetProjectSA8000RemarksDataById(long id)
        {
            var list = await _ProjectSA8000Service.GetProjectSA8000RemarksDataById(id);
            return new JsonResult(list);


        }

        [Route("GetAllProjectChangeRequest")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetAllProjectChangeRequest(long id, PagedResponseModel model)


        {
            var list = await _ProjectSA8000Service.GetAllProjectChangeRequest(id, model);
            //var list1 = await _ClientSitesService.GetPagedAllProjects(id, model);
            return new JsonResult(list);
        }




        [Route("GetProjectChangeDataById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetProjectChangeDataById(long id, PagedResponseModel model)
        {
            var list = await _ProjectSA8000Service.GetProjectChangeDataById(id, model);
            return new JsonResult(list);


        }



        [Route("GetPagedProjectSA8000")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetPagedProjectSA8000(PagedResponseModel model)
        {
            var list = await _ProjectSA8000Service.GetPagedProjectSA8000(model);
            return new JsonResult(list);
        }
        [Route("GetProjectSA8000BYId")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProjectSA8000BYId(long id)
        {
            var list = await _ProjectSA8000Service.GetProjectSA8000BYId(id);
            return new JsonResult(list);


        }


        [HttpPost]
        [Route("ProjectSA8000DeleteById")]
        [Authorize]
        //  [Authorize]
        public async Task<IActionResult> ProjectSA8000DeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _ProjectSA8000Service.ProjectSA8000DeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadApplicationForm")]
        [Authorize]
        public async Task<IActionResult> DownloadApplicationForm(long id)
        {

            var result = await _ProjectSA8000Service.DownloadFile(id);
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
        [Route("GetAllConsultantList")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllConsultantList(long id)
        {
            var List = await _AllDropdownService.GetAllConsultantList(id);
            return new JsonResult(List);
        }
        [Route("GetAllAccreditation")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAccreditation()
        {
            var List = await _AllDropdownService.GetAllAccreditation();
            return new JsonResult(List);
        }
        [Route("SurveillanceVisitFrequencyList")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SurveillanceVisitFrequencyList()
        {
            var List = await _AllDropdownService.SurveillanceVisitFrequencyList();
            return new JsonResult(List);
        }
        
        [Route("GetAllClientSites")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllClientSites(long id)
        {
            var List = await _AllDropdownService.GetAllClientSites(id);
            return new JsonResult(List);
        }
        [Route("GetAllRisk")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRisk()
        {
            var List = await _AllDropdownService.GetAllRisk();
            return new JsonResult(List);
        }

        [Route("GetAllRiskByNaceCode")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRiskByNaceCode(long id)
        {
            var List = await _AllDropdownService.GetAllRiskByNaceCode(id);
            return new JsonResult(List);
        }
        [Route("GetALlSurveillanceMethod")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetALlSurveillanceMethod()
        {
            var List = await _AllDropdownService.GetALlSurveillanceMethod();
            return new JsonResult(List);
        }
        [Route("GetALlExpences")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetALlExpences()
        {
            var List = await _AllDropdownService.GetALlExpences();
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
            var result = await _ProjectSA8000Service.Approval(input);
            return Ok(new Response { Status = result, Message = result });

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
        public async Task<IActionResult> SubmitForReview(long id,long loginUserId)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _ProjectSA8000Service.SubmitForReview(id, loginUserId);
            return Ok(new Response { Status = result, Message = result });

        }

        [Route("ContractSubmit")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ContractSubmit([FromForm] ClientProjectModel input)
        {

            var result = await _ProjectSA8000Service.ContractSubmit(input);
            return Ok(new Response { Status = result, Message = result });



        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadContract")]
        [Authorize]
        public async Task<IActionResult> DownloadContract(long id)
        {

            var result = await _ProjectSA8000Service.downloadContract(id);
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
    }
}
