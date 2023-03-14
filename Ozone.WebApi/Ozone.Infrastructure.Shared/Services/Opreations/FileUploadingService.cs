using Ozone.Application.DTOs;
using Ozone.Application.Interfaces.Setup;
using Ozone.Application.Repository;
using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using AutoMapper;
using Ozone.Application;
using Ozone.Application.Interfaces;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Ozone.Application.DTOs.Projects;
using System.ComponentModel;
using Ozone.Application.DTOs.Setup;

namespace Ozone.Infrastructure.Shared.Services.Opreations
{
    public class FileUploadingService : GenericRepositoryAsync<FileUploading>, IFileUploadingService
    {


        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public FileUploadingService(
             IUnitOfWork unitOfWork,
         OzoneContext dbContext,
        //IDataShapeHelper<Library> dataShaper,
        IMapper mapper,
        IUserSessionHelper userSession, IConfiguration configuration) : base(dbContext)
        {
            this._unitOfWork = unitOfWork;
            _dbContext = dbContext;
            //  _user = dbContext.Set<Library>();
            //_dataShaper = dataShaper;
            this._mapper = mapper;
            this._userSession = userSession;
            //_mockData = mockData;
            this._configuration = configuration;
        }
        public async Task<string> Create(FileUploadingModel input)
        {

            long newid;
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                
               
                    try
                    {
                        
                            FileUploading fileUpload = new FileUploading();
                            //fileUpload.Id = input.Id;
                            fileUpload.AgencyId = input.AgencyId;
                            fileUpload.FromUserId = input.FromUserId;
                            fileUpload.ToUserId = input.ToUserId;
                            fileUpload.CreatedById= input.FromUserId;
                            fileUpload.CreatedDate = DateTime.Now;
                            fileUpload.IsClosed =false;
                            fileUpload.IsDeleted = false;
                            fileUpload.Description = input.Description;

                            //secuserEntity.StatusId = input.StatusId;
                            //secuserEntity.IsSubmitted = input.IsSubmitted;

                            await base.AddAsync(fileUpload);
                    
                             await _unitOfWork.SaveChangesAsync();

                        

                        



                        newid = fileUpload.Id;

                   

                    if (input.File != null)
                        {
                            string filename = Path.GetFileName(input.File.FileName);
                            string ContentType = input.File.ContentType;

                            string reportPath = _configuration["Reporting:FileUploadingDocumentsPath"];

                            // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                            string newFileName = reportPath + +newid + "_" + Guid.NewGuid() + "_" + filename;
                            // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                            using (var stream = new FileStream(newFileName, FileMode.Create))
                            {
                                await input.File.CopyToAsync(stream);

                            }
                        fileUpload.FilePath = newFileName;
                        fileUpload.FileContentType = ContentType;

                          await base.UpdateAsync(fileUpload);
                            await _unitOfWork.SaveChangesAsync();

                        }

                    transaction.Commit();
                    return "Successfully Inserted!";
                       
                    }
                    catch (Exception ex)
                    {
                        var Exception = ex;
                        transaction.Rollback();
                        return "Not Inserted!";
                    }



                }
               
            }


        public async Task<GetPagedFileUploadingModel> GetPagedFiles(long id, PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedFileUploadingModel();
                var fileList = new List<FileUploadingModel>();


                var files = await Task.Run(() => _dbContext.FileUploading.Include(x => x.Agency).Include(x => x.FromUser).Include(x => x.ToUser).Where(x => (x.FromUserId == id || x.ToUserId == id) && x.IsDeleted==false).ToListAsync());

                fileList = _mapper.Map<List<FileUploadingModel>>(files);

               



                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.FileUploadingModel = GetPage(fileList, model.Page, model.PageSize);
                result.TotalCount = fileList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<FileUploadingModel> DownloadClientChange(long id)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                FileUploading file = await Task.Run(() => _dbContext.FileUploading.Where(x => x.Id == id).FirstOrDefault());

                FileUploadingModel cli = new FileUploadingModel();
                if (file != null)
                {
                    cli.FilePath = file.FilePath;
                    cli.FileContentType = file.FileContentType;

                }
                return cli;
            }

        }

        private List<FileUploadingModel> GetPage(List<FileUploadingModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }


        public async Task<string> FileDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                FileUploading Dbresult = _dbContext.FileUploading.Where(u => u.Id == id).FirstOrDefault();


                if (Dbresult != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        Dbresult.IsDeleted = true;
                        await base.UpdateAsync(Dbresult);
                        await _unitOfWork.SaveChangesAsync();





                        transaction.Commit();


                        return "Successfully Deleted!";

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return "Not Deleted!";
                    }
                }
                else
                {
                    return "Client not Exists!";
                }


            }
            // return "User Already Exists!";
        }
    }
}
