﻿using Ozone.Application.DTOs;
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

namespace Ozone.Infrastructure.Shared.Services
{
   public class UserCPDService : GenericRepositoryAsync<UserCpd>, IUserCPDService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;

        public UserCPDService(
             IUnitOfWork unitOfWork,
         OzoneContext dbContext,
        //IDataShapeHelper<Library> dataShaper,
        IMapper mapper,
        IUserSessionHelper userSession,
         IConfiguration configuration) : base(dbContext)
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
        public async Task<string> Create(UserCPDModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                UserCpd DbResult = await Task.Run(() => _dbContext.UserCpd.Where(x => x.Id == input.Id).FirstOrDefault());

                //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


                try
                {
                    var message = "";
                    long newid;
                    bool New = false;
                    if (DbResult == null)
                    {
                        New = true;
                        DbResult = new UserCpd();
                    }
                    DbResult.UserId = input.UserId;
                    DbResult.Course = input.Course;
                    DbResult.Organization = input.Organization;
                    DbResult.Details = input.Details;
                    DbResult.Year = input.Year;
                    DbResult.TypeId = input.TypeId;
                    DbResult.Hours = input.Hours;
                    DbResult.StandardId = input.StandardId;
                   




                    if (New == true)
                    {
                        DbResult.CreatedBy = input.CreatedBy;
                        DbResult.CreatedDate = DateTime.Now;
                        DbResult.IsDeleted = false;
                        await base.AddAsync(DbResult);
                        message = "Successfully Inserted!";
                    }
                    else
                    {
                        //DbResult.LastModifiedBy = input.LastModifiedBy;
                        //DbResult.LastModifiedDate = input.LastModifiedDate;
                        await base.UpdateAsync(DbResult);
                        message = "Successfully Updated!";
                    }
                    //await base.AddAsync(secuserEntity);
                    //await SecUser.(secuserEntity);
                    // _dbContext.SecUser.Add(secuserEntity);
                    //  ozonedb.Add
                    //  await _secuserRepository.CreateUser(secuserEntity);
                    // await _unitOfWork.SaveChangesAsync();
                    var result = await _unitOfWork.SaveChangesAsync();
                    newid = DbResult.Id;
                    if (input.DocumentFile != null)
                    {
                        string filename = Path.GetFileName(input.DocumentFile.FileName);
                        string ContentType = input.DocumentFile.ContentType;

                        string reportPath = _configuration["Reporting:UserCPDPath"];

                        // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                        string newFileName = reportPath + newid + "_" + Guid.NewGuid() + "_" + filename;
                        // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                        using (var stream = new FileStream(newFileName, FileMode.Create))
                        {
                            await input.DocumentFile.CopyToAsync(stream);

                        }
                        DbResult.DocumentsFilePath = newFileName;
                        DbResult.DocumentsContentType = ContentType;
                        await base.UpdateAsync(DbResult);
                        var result2 = await _unitOfWork.SaveChangesAsync();

                    }


                    transaction.Commit();
                    return message;

                }

                catch
                {
                    transaction.Rollback();
                    return "Not Inserted!";
                }



            }

        }

        public async Task<UserCPDModel> GetUserCPDBYId(long id)
        {
            var result = new UserCPDModel();
            var Dbresult = await Task.Run(() => _dbContext.UserCpd.Where(x => x.Id == id).FirstOrDefault());
            result = _mapper.Map<UserCPDModel>(Dbresult);
            return result;
        }

        private List<UserCPDModel> GetPage(List<UserCPDModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedUserCPDModel> GetPagedUserCPDResponse(PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedUserCPDModel();
                var UserStdList = new List<UserCPDModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Certification.Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();

                var list = await _dbContext.UserCpd.Include(x=>x.Standard).Where(x=>x.IsDeleted==false && x.UserId.ToString()==model.Keyword).ToListAsync();
                UserStdList = _mapper.Map<List<UserCPDModel>>(list);
                // return list;
                //}


                //else
                //{
                //    var list = await _dbContext.Certification.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //                 (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //                x.Code.ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //    productDenoList = _mapper.Map<List<StandardModel>>(list);
                //    // return list;
                //}
                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.UserCPDModel = GetPage(UserStdList, model.Page, model.PageSize);
                result.TotalCount = UserStdList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> UserCPDDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                UserCpd dbresult = _dbContext.UserCpd.Where(u => u.Id == id).FirstOrDefault();


                if (dbresult != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        dbresult.IsDeleted = true;
                        await base.UpdateAsync(dbresult);
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
                    return "User Academic not Exists!";
                }


            }
            // return "User Already Exists!";
        }
        public async Task<UserCPDModel> DownloadFile(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                UserCpd DbLibrary = await Task.Run(() => _dbContext.UserCpd.Where(x => x.Id == id).FirstOrDefault());

                UserCPDModel Li = new UserCPDModel();
                if (DbLibrary != null)
                {
                    Li.DocumentsFilePath = DbLibrary.DocumentsFilePath;
                    Li.DocumentsContentType = DbLibrary.DocumentsContentType;

                    //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();
                }
                return Li;
            }

        }

       

    }
}