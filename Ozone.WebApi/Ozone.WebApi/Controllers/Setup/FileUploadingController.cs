using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.DTOs.Setup;
using Ozone.Application.Interfaces;
using Ozone.Application.Interfaces.Setup;
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
    public class FileUploadingController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        IAllDropdownService _AllDropdownService;
        private readonly IMapper _mapper;
        IClientService _ClientService;
        IClientSitesService _ClientSitesService;
        private IJwtAuthManager _jwtAuthManager;
        private readonly IJwtAuthManager _jwtAuthManage;


        IFileUploadingService _fileUploadingService;

       
           

            public FileUploadingController(IMapper mapper,
            // UserService userService,
            IConfiguration configuration,

            IAllDropdownService allDropdownService,
            IClientService clientService,
                IClientSitesService ClientSitesService,
            IJwtAuthManager jwtAuthManager,
             IFileUploadingService fileUploadingService
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
            this._fileUploadingService = fileUploadingService;
        }



        [Route("Create")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] FileUploadingModel input)
        {

            var result = await _fileUploadingService.Create(input);
            return Ok(new Response { Status = result, Message = result });



        }
        [Route("GetPagedFiles")]
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> GetPagedFiles(long id, PagedResponseModel model)
        {
            var list = await _fileUploadingService.GetPagedFiles(id, model);
            return new JsonResult(list);
        }



        [HttpGet, DisableRequestSizeLimit]
        [Route("download")]
        [Authorize]

        public async Task<IActionResult> Download(long id)
        {

            var result = await _fileUploadingService.DownloadClientChange(id);

            var fileName = result.FilePath;
            var memory = new MemoryStream();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contenpe = result.FileContentType;
            var fileNM = Path.GetFileName(fileName);

            return File(memory, contenpe, fileNM);
        }

        [HttpPost]
        [Route("FileDeleteById")]
        [Authorize]

        public async Task<IActionResult> FileDeleteById(long id)
        {
            // SecUserService secuserservice = new SecUserService();
            var result = await _fileUploadingService.FileDeleteById(id);
            return Ok(new Response { Status = result, Message = result });

        }
    }
}
