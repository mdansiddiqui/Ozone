using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.Interfaces;
//using Ozone.Application.Interfaces.Service;
using Ozone.Infrastructure;
using Ozone.Infrastructure.Shared.Services;
using System.IO;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;


namespace Ozone.WebApi.Controllers.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        IAllDropdownService _AllDropdownService;
        private readonly IMapper _mapper;
        IClientService _ClientService;
        IClientSitesService _ClientSitesService;
        private readonly IJwtAuthManager _jwtAuthManager;

        //public AuthenticateController(UserManager<SecUser> userManager, IConfiguration configuration)
        public ClientController(IMapper mapper,
            // UserService userService,
            IConfiguration configuration,

            IAllDropdownService allDropdownService,
            IClientService clientService,
                IClientSitesService ClientSitesService,
            IJwtAuthManager jwtAuthManager
            )
        {
            this._mapper = mapper;
            //this.userManager = userManager;
            // this._userService = userService;
            _configuration = configuration;
            // this._authService = authService;
            this._ClientService = clientService;
            this._ClientSitesService = ClientSitesService;
            this._jwtAuthManager = jwtAuthManager;
            this._AllDropdownService = allDropdownService;
        }
        [Route("GetPagedClient")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedClient(long id, PagedResponseModel model)
        {
            var list = await _ClientService.GetPagedClient(id, model);
            return new JsonResult(list);
        }

        [Route("GetClientDataById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetClientDataById(int id)
        {
            var list = await _ClientService.GetClientBYId(id);
            return new JsonResult(list);


        }

       

        [Route("CreateClient")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateClient(ClientModel input)
        {

            var result = await _ClientService.CreateClient(input);
            return Ok(new Response { Status = result, Message = result });


        }


        //[Route("ChangeRequestCreateWithFile")]
        //[HttpPost]
        //[Authorize]

        //public async Task<IActionResult> ChangeRequestCreateWithFile([FromForm] ClientModel input)
        //{

        //    var result = await _ClientService.CreateChangeClient(input);
        //    return Ok(new Response { Status = result, Message = result });



        //}

        [Route("CreateChangeClient")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateChangeClient([FromForm] ClientModel input)
        {

            var result = await _ClientService.CreateChangeClient(input);
            return Ok(new Response { Status = result, Message = result });


        }



        [Route("CreateChangeClienSite")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateChangeClienSite([FromForm] ClientSitesModel input)
        {

            var result = await _ClientService.CreateChangeClienSite(input);
            return Ok(new Response { Status = result, Message = result });


        }



        [HttpGet, DisableRequestSizeLimit]
        [Route("download")]
        [Authorize]

        public async Task<IActionResult> Download(long id)
        {

            var result = await _ClientService.DownloadClientChange(id);
        
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
        [Route("ClientSitesdownloadFile")]
        [Authorize]

        public async Task<IActionResult> ClientSitesdownloadFile(long id)
        {

            var result = await _ClientService.ClientSitesdownloadFile(id);

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




        [HttpPost]
        [Route("ClientDeleteById")]
        [Authorize]

        //  [Authorize]
        public async Task<IActionResult> ClientDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _ClientService.ClientDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }



        [Route("CreateClientSites")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateClientSites(ClientSitesModel input)
        {

            var result = await _ClientSitesService.CreateClientSites(input);
            return Ok(new Response { Status = result, Message = result });


        }


        [Route("ApprovedClientSitesChange")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> ApprovedClientSitesChange(ActivityLogModel input)
        {

            var result = await _ClientSitesService.ApprovedClientSitesChange(input);
            return Ok(new Response { Status = result, Message = result });


        }



        [Route("ApprovedClientChange")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> ApprovedClientChange(ActivityLogModel input)
        {

            var result = await _ClientSitesService.ApprovedClientChange(input);
            return Ok(new Response { Status = result, Message = result });


        }


        [Route("GetPagedClientSites")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedClientSites(PagedResponseModel model)
        {
            var list = await _ClientSitesService.GetPagedClientSites(model);
            return new JsonResult(list);
        }

        [Route("GetPagedClientAllProject")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedClientAllProject(long id,PagedResponseModel model)
        {
            var list = await _ClientSitesService.GetPagedClientProjects(id,model);
            return new JsonResult(list);
        }

        [Route("GetPagedAllProject")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedAllProject(long id, PagedResponseModel model)


        {
            var list = await _ClientSitesService.GetPagedAllProjects(id, model);
            //var list1 = await _ClientSitesService.GetPagedAllProjects(id, model);
            return new JsonResult(list);
        }

        [Route("GetPagedAllAudits")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedAllAudits(long id, PagedResponseModel model)


        {
            var list = await _ClientSitesService.GetPagedAllAudits(id, model);
            //var list1 = await _ClientSitesService.GetPagedAllProjects(id, model);
            return new JsonResult(list);
        }


        [Route("GetPagedAllClientChange")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedAllClientChange(long id, PagedResponseModel model)


        {
            var list = await _ClientSitesService.GetPagedAllClientChange(id, model);
            //var list1 = await _ClientSitesService.GetPagedAllProjects(id, model);
            return new JsonResult(list);
        }


        [Route("GetAllClientSitesChangeRequest")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetAllClientSitesChangeRequest(long id, PagedResponseModel model)


        {
            var list = await _ClientSitesService.GetAllClientSitesChangeRequest(id, model);
            //var list1 = await _ClientSitesService.GetPagedAllProjects(id, model);
            return new JsonResult(list);
        }


        [Route("GetClientChangeDataById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetClientChangeDataById(long id,PagedResponseModel model)
        {
            var list = await _ClientSitesService.GetClientChangeDataById(id,model);
            return new JsonResult(list);


        }


        [Route("GetClientSitesChangeDataById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetClientSitesChangeDataById(long id, PagedResponseModel model)
        {
            var list = await _ClientSitesService.GetClientSitesChangeDataById(id, model);
            return new JsonResult(list);


        }



        [Route("GetClientChangeRemarksDataById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetClientChangeRemarksDataById(long id)
        {
            var list = await _ClientSitesService.GetClientChangeRemarksDataById(id);
            return new JsonResult(list);


        }



        [Route("GetClientSitesChangeRemarksDataById")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetClientSitesChangeRemarksDataById(long id)
        {
            var list = await _ClientSitesService.GetClientSitesChangeRemarksDataById(id);
            return new JsonResult(list);


        }



        [Route("GetProjectFormUrlById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetProjectFormUrlById(long id)
        {
            var list = await _ClientSitesService.GetProjectUrlBYId(id);
            return new JsonResult(list);


        }

        [Route("GetSiteById")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetSiteById(long id)
        {
            var list = await _ClientSitesService.GetClientSitesBYId(id);
            return new JsonResult(list);


        }
        [Route("GetALLClients")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetALLClients(long id)
        {
            var list = await _AllDropdownService.GetALLClients(id);
            return new JsonResult(list);


        }

        
        [Route("GetAuditorByStandardId")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetAuditorByStandardId(UserStandardModel input)
        {
            var list = await _ClientService.GetAuditorByStandardId(input.Id, input.OrganizationId);
            return new JsonResult(list);


        }


        [Route("GetLeadAuditorByStandardId")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetLeadAuditorByStandardId(UserStandardModel input)
        {
            var list = await _ClientService.GetLeadAuditorByStandardId(input.Id, input.OrganizationId);
            return new JsonResult(list);


        }
        [Route("GetReviewerByStandardId")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetReviewerByStandardId(UserStandardModel input)
        {
            var list = await _ClientService.GetReviewerByStandardId(input.Id);
            return new JsonResult(list);


        }
        [HttpPost]
        [Route("ClientSitesDeleteById")]
        [Authorize]

        //  [Authorize]
        public async Task<IActionResult> ClientSitesDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _ClientSitesService.ClientSitesDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }
        //[Route("GetClientfullDataById")]
        //[HttpGet]
        //public async Task<IActionResult> GetClientfullDataById(int id)
        //{
        //    var list = await _ClientService.GetClientBYId(id);
        //    return new JsonResult(list);


        //}

        [Route("GetReviewerByStandard")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetReviewerByStandard(UserStandardModel input)
        {
           var list = await _ClientService.GetReviewerByStandard(input.StandardId, input.UserId);
            return new JsonResult(list);


        }



       


    }
}
