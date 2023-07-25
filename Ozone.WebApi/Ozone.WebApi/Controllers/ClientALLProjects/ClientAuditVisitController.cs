
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
using Ozone.Application.Interfaces.Security;
using Ozone.Application.Interfaces.Setup;
using Ozone.Application.Interfaces;

namespace Ozone.WebApi.Controllers.ClientALLProjects
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAuditVisitController : BaseApiController
    {
        private readonly IConfiguration _configuration;

    IAllDropdownService _AllDropdownService;
    ISecUserSessionService _userSessionService;
        IClientAuditVisitService _clientAuditVisitService;
        IAuditReportService _auditReportService;
        private readonly IJwtAuthManager _jwtAuthManager;


    //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
    public ClientAuditVisitController(
        // UserService userService,
        IConfiguration configuration,

        ISecUserSessionService userSessionService,
        IClientAuditVisitService clientAuditVisitService,
        IAuditReportService auditReportService,
        IJwtAuthManager jwtAuthManager,
        IAllDropdownService allDropdownService
        )
    {
        //this.userManager = userManager;
        // this._userService = userService;
        _configuration = configuration;
        this._AllDropdownService = allDropdownService;
        this._userSessionService = userSessionService;
        this._clientAuditVisitService = clientAuditVisitService;
        this._jwtAuthManager = jwtAuthManager;
            this._auditReportService = auditReportService;
    }



        [Route("Create")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create([FromForm] ClientAuditVisitModel input)
        {

            var result = await _clientAuditVisitService.Create(input);
            return Ok(new Response { Status = result, Message = result });



        }

        [Route("GetPagedClientAuditVisitResponse")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedClientAuditVisitResponse(long id,PagedResponseModel model)
        {
            var list = await _clientAuditVisitService.GetPagedClientAuditVisitResponse(id,model);
            return new JsonResult(list);
        }

        [Route("GetClientAuditVisitBYId")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetClientAuditVisitBYId(int id)
        {
            var list = await _clientAuditVisitService.GetClientAuditVisitBYId(id);
            return new JsonResult(list);


        }

        [Route("GetAllRequiredDocumentBYId")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllRequiredDocumentBYId(int id)
        {
            var list = await _clientAuditVisitService.GetAllRequiredDocumentBYId(id);
            return new JsonResult(list);


        }


        [Route("GetAllDocumentsTypeWSVLAS")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllDocumentsTypeWSVLAS(int id)
        {
            var list = await _clientAuditVisitService.GetAllDocumentsTypeWSVLAS(id);
            return new JsonResult(list);


        }

        [Route("GetClientAuditVisitByOrganizationId")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetClientAuditVisitByOrganizationId(int id)
        {
            var list = await _clientAuditVisitService.GetClientAuditVisitByOrganizationId(id);
            return new JsonResult(list);


        }


        [Route("GetClientAuditVisitBySearch")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetClientAuditVisitBySearch(IDictionary<string, string> keyValuePairs)
        {

            var list = await _clientAuditVisitService.GetClientAuditVisitBySearch(keyValuePairs);

            return new JsonResult(list);
        }



        [HttpPost]
        [Route("ClientAuditVisitDeleteById")]
        [Authorize]

        public async Task<IActionResult> ClientAuditVisitDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _clientAuditVisitService.ClientAuditVisitDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }


        [Route("GetALLVisitType")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetALLVisitType()
        {
            var List = await _AllDropdownService.GetALLVisitType();
            return new JsonResult(List);
        }
        [Route("GetALLVisitStatus")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetALLVisitStatus()
        {
            var List = await _AllDropdownService.GetALLVisitStatus();
            return new JsonResult(List);
        }
        [Route("GetAllProjectCode")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllProjectCode()
        {
            var List = await _AllDropdownService.GetAllProjectCode();
            return new JsonResult(List);
        }
        [Route("GetProjectCodeById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetProjectCodeById(long id)
        {
            var List = await _AllDropdownService.GetProjectCodeById(id);
            return new JsonResult(List);
        }
        [Route("AuditPlan")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AuditPlan(long id,PagedResponseModel model)
        {
            var list = await _clientAuditVisitService.AuditPlan(id,model);
            return new JsonResult(list);
        }
        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadAuditPlan")]
        [Authorize]

        public async Task<IActionResult> DownloadAuditPlan(long id)
        {

            var result = await _clientAuditVisitService.DownloadAuditPlan(id);
            //  var fileName = @"G:/OzoneDocuments/LibraryDocument/10_AD Requirement.txt";
            var fileName = result.AuditPlanFilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            // var contenpe = "application/pdf";
            var contenpe = result.AuditPlanContentType;
            var fileNM = Path.GetFileName(fileName);
            //   var net = new System.Net.WebClient();
            //  var data = net.DownloadData(@"D:/Update work/OT Booking.pdf");
            // var data = net.DownloadData(fname);
            // var content = new System.IO.MemoryStream(data);
            //  var contentType = "application/pdf";
            //var fileName = "OT Booking.pdf";
            return File(memory, contenpe, fileNM);
        }
        [Route("CreateAuditReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateAuditReport([FromForm] AuditVisitReportMasterModel input)
        {

            var result = await _auditReportService.CreateAuditReport(input);
            return Ok(new Response { Status = result, Message = result });



        }

        [Route("GetPagedAuditReport")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedAuditReport(PagedResponseModel model)
        {
            var list = await _auditReportService.GetPagedAuditReportResponse(model);
            return new JsonResult(list);
        }
        //[Route("GetPagedAuditReportById")]
        //[HttpPost]
        //public async Task<IActionResult> GetPagedAuditReportById(long id,PagedResponseModel model)
        //{
        //    var list = await _auditReportService.GetPagedAuditReportResponseById(id,model);
        //    return new JsonResult(list);
        //}
        [Route("GetPagedAuditReportById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedAuditReportById(long id, PagedResponseModel model)
        {
            var list = await _auditReportService.GetPagedAuditReportDetailById(id, model);
            return new JsonResult(list);
        }
        [Route("GetAuditReportById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAuditReportById(int id)
        {
            var list = await _auditReportService.AuditReportBYId(id);
            return new JsonResult(list);


        }

        [HttpPost]
        [Route("AuditReportDeleteById")]
        [Authorize]

        public async Task<IActionResult> AuditReportDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditReportService.AuditReportDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadAuditReport")]
        [Authorize]

        public async Task<IActionResult> DownloadAuditReport(long id)
        {

            var result = await _auditReportService.DownloadAuditReport(id);
            //  var fileName = @"G:/OzoneDocuments/LibraryDocument/10_AD Requirement.txt";
            var fileName = result.DocumentFilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            // var contenpe = "application/pdf";
            var contenpe = result.DocumentContentType;
            var fileNM = Path.GetFileName(fileName);
            //   var net = new System.Net.WebClient();
            //  var data = net.DownloadData(@"D:/Update work/OT Booking.pdf");
            // var data = net.DownloadData(fname);
            // var content = new System.IO.MemoryStream(data);
            //  var contentType = "application/pdf";
            //var fileName = "OT Booking.pdf";
            return File(memory, contenpe, fileNM);
        }

        [Route("GetALLAuditDoucmentsType")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetALLAuditDoucmentsType()
        {
            var List = await _AllDropdownService.GetAllAuditDocumentsType();
            return new JsonResult(List);
        }

        [Route("GetPagedAuditReportDetail")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedAuditReportDetail(long id,PagedResponseModel model)
        {
            var list = await _auditReportService.GetPagedAuditReportDetailById(id,model);
            return new JsonResult(list);
        }

        //[Route("AuditComplete")]
        //[HttpPost]
        //public async Task<IActionResult> AuditComplete(long id)
        //{
        //    var result = await _clientAuditVisitService.AuditComplete(id);
        //    return Ok(new Response { Status = result, Message = result });
        //}

        [Route("AuditComplete")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AuditComplete(ClientAuditVisitModel input)
        {
            var result = await _clientAuditVisitService.AuditComplete(input);
            return Ok(new Response { Status = result, Message = result });
        }


        //[HttpPost]
        //[Route("SubmitForReview")]
        ////  [Authorize]
        //public async Task<IActionResult> SubmitForReview(long id, long loginUserId)
        //{
        //    // SecUserService secuserservice = new SecUserService();
        //    var result = await _clientAuditVisitService.SubmitForReview(id, loginUserId);
        //    return Ok(new Response { Status = result, Message = result });

        //}

        [HttpPost]
        [Route("QCDocumentsList")]
        [Authorize]

        public async Task<IActionResult> QCDocumentsList(long id,PagedResponseModel model)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditReportService.QCDocumentsList(id,model);
            return new JsonResult(result);

        }
        [HttpGet]
        [Route("QCHostory")]
        [Authorize]

        public async Task<IActionResult> QCHostory(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditReportService.QcHistory(id);
            return new JsonResult(result);

        }


        //[Route("AddCommentList")]
        //[HttpPost]
        //public async Task<IActionResult> AddCommentList([FromForm]List<QCHistoryModelTow> QCHistoryModel)
        //{
        //   List< QCHistoryModel> QC = new List<QCHistoryModel>();
        //    var result = await _auditReportService.AddCommentList(QC);
        //    return Ok(new Response { Status = result, Message = result });



        //}

        [Route("AddCommentList")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AddCommentList(List<QCHistoryModel> QC)
        {
            //List<QCHistoryModel> QC = new List<QCHistoryModel>();
            var result = await _auditReportService.AddCommentList(QC);
            return Ok(new Response { Status = result, Message = result });



        }


        //[Route("AddMasterCommentList")]
        //[HttpPost]
        //[Authorize]

        //public async Task<IActionResult> AddMasterCommentList(QCDocumentsListModel input)
        //{
        //    var result="Farooq";
        //    //var result = await _auditReportService.AddCommentList(input);
        //    return Ok(new Response { Status = result, Message = result });



        //}
        [Route("AddComment")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AddComment(QCHistoryModel input)
        {

            var result = await _auditReportService.AddComment(input);
            return Ok(new Response { Status = result, Message = result });



        }

        [HttpPost]
        [Route("QCCommentsDeleteById")]
        [Authorize]

        public async Task<IActionResult> QCCommentsDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditReportService.QCCommentsDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }
        [Route("GetAllQcStatus")]
        [HttpGet]
        [Authorize]
        
        public async Task<IActionResult> GetAllQcStatus()
        {
            var List = await _auditReportService.GetAllQcStatus();
            return new JsonResult(List);
        }

        [HttpPost]
        [Route("AuditSubmitForReview")]
        [Authorize]

        //  [Authorize]
        public async Task<IActionResult> AuditSubmitForReview(IDictionary<string, long> keyValuePairs)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditReportService.SubmitForReview(keyValuePairs);
            return Ok(new Response { Status = result, Message = result });

        }

        [HttpPost]
        [Route("onStageOne")]
        [Authorize]

        //  [Authorize]
        public async Task<IActionResult> onStageOne(IDictionary<string, long> keyValuePairs)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _auditReportService.onStageOne(keyValuePairs);
            return Ok(new Response { Status = result, Message = result });

        }


        [Route("GetAllAdminList")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllAdminList()
        
        {
            var List = await _AllDropdownService.GetAllAdminList();
            return new JsonResult(List);
        }
        [Route("GetAllTechnicalExpert")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllTechnicalExpert(long id)

        {
            var List = await _AllDropdownService.GetAllTechnicalExpert(id);
            return new JsonResult(List);
        }

        [Route("GetAllJustifiedPerson")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllJustifiedPerson(long id)

        {
            var List = await _AllDropdownService.GetAllJustifiedPerson(id);
            return new JsonResult(List);
        }

        [Route("GetALLVisitLevel")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetALLVisitLevel()
        {
            var List = await _AllDropdownService.GetALLVisitLevel();
            return new JsonResult(List);
        }

        [Route("CreateVisitLevel")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateVisitLevel(VisitLevelModel input)
        {

            var result = await _AllDropdownService.CreateVisitLevel(input);
            return Ok(new Response { Status = result, Message = result });

        }


        [HttpPost]
        [Route("DeleteVisitLevelById")]
        [Authorize]

        public async Task<IActionResult> DeleteVisitLevelById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _AllDropdownService.DeleteVisitLevelById(id);
            return Ok(new Response { Status = result, Message = result });

        }

        [Route("GetAllAgencywithHeadOffice")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllAgencywithHeadOffice()
        {

            var list = await _AllDropdownService.GetAllAgencywithHeadOffice();

            return new JsonResult(list);
        }

        //[HttpPost]
        //[Route("AddVisitLevelUser")]
        //[Authorize]
        //public async Task<IActionResult> AddVisitLevelUser(VisitLevelModel input)
        //{
        //    var result = await _AllDropdownService.CreateUserVisitLevel(input);
        //    return Ok(new Response { Status = result, Message = result });
        //}


        [HttpPost]
        [Route("CreateAuditReviewerDocument")]
        [Authorize]

        public async Task<IActionResult> CreateAuditReviewerDocument([FromForm] AuditReviewerDocumentModel input)
        {
           
            var result = await _clientAuditVisitService.AuditReviewerDocumentCreate(input);
            return Ok(new Response { Status = result, Message = result });

        }




        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadAuditReviewerDocuments")]
        [Authorize]

        public async Task<IActionResult> DownloadAuditReviewerDocuments(long id)
        {

            var result = await _clientAuditVisitService.DownloadAuditReviewerDocuments(id);
            //  var fileName = @"G:/OzoneDocuments/LibraryDocument/10_AD Requirement.txt";
            var fileName = result.DocumentPath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
     
            var contenpe = result.DocumentContentType;
            var fileNM = Path.GetFileName(fileName);
          
            return File(memory, contenpe, fileNM);
        }

        [Route("GetAllAuditReviewerDocuments")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAuditReviewerDocuments(long id)
        {

            var list = await _clientAuditVisitService.GetAllAuditReviewerDocuments(id);

            return new JsonResult(list);
        }

        [HttpPost]
        [Route("AuditReviewerDocumentDeleteById")]
        [Authorize]

        public async Task<IActionResult> AuditReviewerDocumentDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _clientAuditVisitService.AuditReviewerDocumentDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }
        [Route("GetAllReviewerDocumentsType")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllReviewerDocumentsType(int id)
        {
            var list = await _clientAuditVisitService.GetAllDocumentsTypeforReviewer(id);
            return new JsonResult(list);


        }
        [Route("GetAllManagerDocumentsType")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllManagerDocumentsType(int id)
        {
            var list = await _clientAuditVisitService.GetAllManagerDocumentsType(id);
            return new JsonResult(list);


        }


        [Route("GetAllAuditManagerDocuments")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAuditManagerDocuments(long id)
        {

            var list = await _clientAuditVisitService.GetAllAuditManagerDocuments(id);

            return new JsonResult(list);
        }

        [HttpPost]
        [Route("AuditManagerDocumentDeactiveById")]
        [Authorize]

        public async Task<IActionResult> AuditManagerDocumentDeactiveById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _clientAuditVisitService.AuditManagerDocumentDeactiveById(id);
            return Ok(new Response { Status = result, Message = result });

        }

        [HttpPost]
        [Route("AuditManagerDocumentCreate")]
        [Authorize]

        public async Task<IActionResult> AuditManagerDocumentCreate([FromForm] AuditMangerDocumentModel input)
        {

            var result = await _clientAuditVisitService.AuditManagerDocumentCreate(input);
            return Ok(new Response { Status = result, Message = result });

        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("DownloadAuditManagerDocuments")]
        [Authorize]

        public async Task<IActionResult> DownloadAuditManagerDocuments(long id)
        {

            var result = await _clientAuditVisitService.DownloadAudiorManagerDocuments(id);
            //  var fileName = @"G:/OzoneDocuments/LibraryDocument/10_AD Requirement.txt";
            var fileName = result.DocumentPath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contenpe = result.DocumentContentType;
            var fileNM = Path.GetFileName(fileName);

            return File(memory, contenpe, fileNM);
        }
    }
}
