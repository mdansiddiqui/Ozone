using AutoMapper;
using Ozone.Application;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Security;


//using Ozone.Domain.Entities;
using Ozone.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ozone.Infrastructure.Persistence.Models;
using Ozone.Application.Interfaces.Service;
using Ozone.Application.Parameters;
using Ozone.Application.Interfaces;
//using Ozone.Infrastructure.Persistence.Repository;
using Ozone.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ozone.Application.Repository;
using System.IO;
using Microsoft.Extensions.Configuration;
using Ozone.Application.DTOs.Reports;
using Ozone.Application.DTOs.Projects;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Ozone.Application.Interfaces.Setup;



////using Ozone.Domain.Entities;
//using Ozone.Infrastructure.Persistence;

//using Ozone.Application.Interfaces.Service;
//using Ozone.Application.Parameters;

////using Ozone.Infrastructure.Persistence.Repository;
//using Ozone.Infrastructure.Persistence.Entities;
//using Microsoft.EntityFrameworkCore;

//using System.IO;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Http;
//using static System.Net.Mime.MediaTypeNames;
////using static System.Net.Mime.MediaTypeNames;
//using System.Web;
////using System.IO;
//using System.Drawing;
//using Microsoft.AspNetCore.Http.Internal;

namespace Ozone.Infrastructure.Shared.Services
{
    public class DocumentTypeService : GenericRepositoryAsync<DocumentType>, IDocumentTypeService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentTypeService(
             IUnitOfWork unitOfWork,
         OzoneContext dbContext,
        //IDataShapeHelper<Library> dataShaper,
        IMapper mapper,
        IUserSessionHelper userSession) : base(dbContext)
        {
            this._unitOfWork = unitOfWork;
            _dbContext = dbContext;
            //  _user = dbContext.Set<Library>();
            //_dataShaper = dataShaper;
            this._mapper = mapper;
            this._userSession = userSession;
            //_mockData = mockData;
        }

        public async Task<string> Create(DocumentsTypeModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                DocumentType DbDocument = await Task.Run(() => _dbContext.DocumentType.Where(x => x.IsDeleteed == false && x.Id == input.Id).FirstOrDefault());

                //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


                try
                {
                    long newid;
                    bool New = false;
                    if (DbDocument == null)
                    {
                        New = true;
                        DbDocument = new DocumentType();
                    }
                    //DbModules.Id = input.Id;
                    DbDocument.Id = input.Id;
                    DbDocument.Name = input.Name;
                    DbDocument.IsActive = input.IsActive;
                    DbDocument.Description = input.Description;

                    DbDocument.Code = input.Code;


                    if (New == true)
                    {
                        DbDocument.IsDeleteed = false;
                        await base.AddAsync(DbDocument);
                    }
                    else
                    {
                        await base.UpdateAsync(DbDocument);
                    }
                    //await base.AddAsync(secuserEntity);
                    //await SecUser.(secuserEntity);
                    // _dbContext.SecUser.Add(secuserEntity);
                    //  ozonedb.Add
                    //  await _secuserRepository.CreateUser(secuserEntity);
                    // await _unitOfWork.SaveChangesAsync();
                    var result = await _unitOfWork.SaveChangesAsync();

                    newid = DbDocument.Id;

                    transaction.Commit();
                    return "Successfully Saved!";

                }

                catch
                {
                    transaction.Rollback();
                    return "Not Inserted!";
                }



            }

        }

        public async Task<string> DocumentTypeDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                DocumentType DbDocument = _dbContext.DocumentType.Where(u => u.Id == id).FirstOrDefault();


                if (DbDocument != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        DbDocument.IsDeleteed = true;
                        await base.UpdateAsync(DbDocument);
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

        public async Task<string> DelMSWD(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                MappingDocumentsWithStandard DbDocument = _dbContext.MappingDocumentsWithStandard.Where(u => u.Id == id).FirstOrDefault();


                if (DbDocument != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        DbDocument.IsDeleted = true;
                        _dbContext.MappingDocumentsWithStandard.Update(DbDocument);
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

        public async Task<string> createMSWD(MappingDocumentsWithStandardModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                MappingDocumentsWithStandard DbStandard = await Task.Run(() => _dbContext.MappingDocumentsWithStandard.Where(x => x.IsDeleted == false && x.Id == input.Id).FirstOrDefault());
                try
                {
                    long newid;
                    bool New = false;
                    if (DbStandard == null)
                    {
                        New = true;
                        DbStandard = new MappingDocumentsWithStandard();
                    }
                    //DbModules.Id = input.Id;
                    DbStandard.Id = input.Id;
                    DbStandard.StandardId = input.StandardId;
                    DbStandard.VisitLevelId = input.VisitLevelId;
                    DbStandard.DocumentTypeId = input.DocumentTypeId;
                    DbStandard.IsActive = input.IsActive;
                    DbStandard.IsRequired = input.IsRequired;
                   // DbStandard.DocumentForReviewer = input.DocumentForReviewer;
                    DbStandard.DocumentAssignId = input.DocumentAssignId;
                    if (New == true)
                    {
                        DbStandard.IsDeleted = false;
                        await _dbContext.MappingDocumentsWithStandard.AddAsync(DbStandard);
                    }
                    else
                    {
                        _dbContext.MappingDocumentsWithStandard.Update(DbStandard);
                    }

                    var result = await _unitOfWork.SaveChangesAsync();

                    newid = DbStandard.Id;

                    transaction.Commit();
                    return "Successfully Saved!";

                }

                catch
                {
                    transaction.Rollback();
                    return "Not Inserted!";
                }



            }

        }
        public async Task<List<MappingDocumentsWithStandardModel>> GetMSWD(PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedDocumentsTypeModel();
                var productDenoList = new List<MappingDocumentsWithStandardModel>();

                var list = await Task.Run(() => _dbContext.MappingDocumentsWithStandard.Include(x => x.Standard).Include(x => x.DocumentType).Include(x => x.VisitLevel).Include(x=>x.DocumentAssign).Where(x => x.IsDeleted == false).ToList());
                productDenoList = _mapper.Map<List<MappingDocumentsWithStandardModel>>(list);
                return productDenoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DocumentsTypeModel> GetDocumentsTypeBYId(long id)
        {
            var result = new DocumentsTypeModel();
            var DbDocument = await Task.Run(() => _dbContext.DocumentType.Where(x => x.Id == id).FirstOrDefault());
            result = _mapper.Map<DocumentsTypeModel>(DbDocument);
            return result;
        }

        private List<DocumentsTypeModel> GetPage(List<DocumentsTypeModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedDocumentsTypeModel> GetPagedDocumentsTypeResponse(PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedDocumentsTypeModel();
                var productDenoList = new List<DocumentsTypeModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;

                var list = await Task.Run(() => _dbContext.DocumentType.Where(x => x.IsDeleteed == false).ToList());
                productDenoList = _mapper.Map<List<DocumentsTypeModel>>(list);
                //}


                //else
                //{
                //    var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //                 (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //                x.Code.ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //    productDenoList = _mapper.Map<List<ModulesModel>>(list);
                //    // return list;
                //}
                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.DocumentsTypeModel = GetPage(productDenoList, model.Page, model.PageSize);
                result.TotalCount = productDenoList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
