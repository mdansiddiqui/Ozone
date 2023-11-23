using AutoMapper;
using Ozone.Application;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Setup;
using Ozone.Application.Interfaces;
using Ozone.Application.Interfaces.Setup;
using Ozone.Application.Repository;
using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Infrastructure.Shared.Services
{
    public class AuditDocumentTypeService : GenericRepositoryAsync<AuditDocumentsType>, IAuditDocumentTypeService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;

        //public object DbAuditDocumentsType { get; private set; }
        //public AuditDocumentsType DbAuditDocumentType { get; private set; }

        public AuditDocumentTypeService(
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

        public async Task<string> Create(AuditDocumetTypeModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                AuditDocumentsType DbAuditDocumentType = await Task.Run(() => _dbContext.AuditDocumentsType.Where(x => x.IsDeleted == false && x.Id == input.Id).FirstOrDefault());

                //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


                try
                {

                    bool New = false;
                    if (DbAuditDocumentType == null)
                    {
                        New = true;
                        DbAuditDocumentType = new AuditDocumentsType();
                    }
                    //DbModules.Id = input.Id;
                    DbAuditDocumentType.Id = input.Id;
                    DbAuditDocumentType.Name = input.Name;
                    DbAuditDocumentType.IsActive = input.IsActive;
                    DbAuditDocumentType.Description = input.Description;

                    DbAuditDocumentType.Code = input.Code;
                    DbAuditDocumentType.CreatedById = input.CreatedById;


                    if (New == true)
                    {
                        DbAuditDocumentType.IsDeleted = false;
                        await base.AddAsync(DbAuditDocumentType);
                    }
                    else
                    {
                        await base.UpdateAsync(DbAuditDocumentType);
                    }
                    //await base.AddAsync(secuserEntity);
                    //await SecUser.(secuserEntity);
                    // _dbContext.SecUser.Add(secuserEntity);
                    //  ozonedb.Add
                    //  await _secuserRepository.CreateUser(secuserEntity);
                    // await _unitOfWork.SaveChangesAsync();
                    var result = await _unitOfWork.SaveChangesAsync();

                    //newid = DbAccreditation.Id;

                    transaction.Commit();
                    return "Successfully Saved!";

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Not Inserted!";
                }




            }

        }

        public async Task<string> AuditDocumetTypeDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                AuditDocumentsType DbAuditDocumentsType = _dbContext.AuditDocumentsType.Where(u => u.Id == id).FirstOrDefault();


                if (DbAuditDocumentsType != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        DbAuditDocumentsType.IsDeleted = true;
                        await base.UpdateAsync(DbAuditDocumentsType);
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

        public async Task<AuditDocumetTypeModel> GetAuditDocumetTypeBYId(long id)
        {
            var result = new AuditDocumetTypeModel();
            var DbAuditDocumentType = await Task.Run(() => _dbContext.AuditDocumentsType.Where(x => x.Id == id).FirstOrDefault());
            result = _mapper.Map<AuditDocumetTypeModel>(DbAuditDocumentType);
            return result;
        }

        private List<AuditDocumetTypeModel> GetPage(List<AuditDocumetTypeModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedAuditDocumentTypeModel> GetPagedAuditDocumentTypeResponse(PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedAuditDocumentTypeModel();
                var productDenoList = new List<AuditDocumetTypeModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;

                var list = await Task.Run(() => _dbContext.AuditDocumentsType.Where(x => x.IsDeleted == false).ToList());
                productDenoList = _mapper.Map<List<AuditDocumetTypeModel>>(list);
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

                result.AuditDocumetTypeModel = GetPage(productDenoList, model.Page, model.PageSize);
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
