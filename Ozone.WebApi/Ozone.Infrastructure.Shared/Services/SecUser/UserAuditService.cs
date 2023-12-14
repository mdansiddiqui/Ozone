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
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ozone.Infrastructure.Shared.Services
{
   public class UserAuditService : GenericRepositoryAsync<UserAudit>, IUserAuditService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;

        public UserAuditService(
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
        public async Task<string> Create(UserAuditModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                UserAudit DbResult = await Task.Run(() => _dbContext.UserAudit.Where(x => x.Id == input.Id).FirstOrDefault());

                //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


                try
                {
                    var message = "";
                    //long newid;
                    bool New = false;
                    if (DbResult == null)
                    {
                        New = true;
                        DbResult = new UserAudit();
                    }
                    DbResult.UserId = input.UserId;
                    DbResult.Organization = input.Organization;
                    DbResult.StandardId = input.StandardId;
                    DbResult.EacodeId = input.EacodeId;
                    DbResult.NaceCodeId = input.NaceCodeId;
                    DbResult.AuditLevel = input.AuditLevel;
                    DbResult.AuditTypeId = input.AuditTypeId;
                    DbResult.Duration = input.Duration;
                    DbResult.Year = input.Year;
                    DbResult.CertificationBodyId = input.CertificationBodyId;


                    if (New == true)
                    {
                        DbResult.CreatedBy = input.CreatedBy;
                        DbResult.CreatedDate = input.CreatedDate;
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

        public async Task<UserAuditModel> GetUserAuditBYId(long id)
        {
            var result = new UserAuditModel();
            var Dbresult = await Task.Run(() => _dbContext.UserAudit.Where(x => x.Id == id).FirstOrDefault());
            result = _mapper.Map<UserAuditModel>(Dbresult);
            return result;
        }

        private List<UserAuditModel> GetPage(List<UserAuditModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedUserAuditModel> GetPagedUserAuditResponse(PagedResponseModel model)
         {
            try
            {

                var result = new GetPagedUserAuditModel();
                var UserStdList = new List<UserAuditModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Certification.Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();

                var list = await _dbContext.UserAudit.Include(x=>x.Standard).Include(x=>x.NaceCode).Include(x=>x.Eacode).Include(x=>x.AuditType).Include(x=>x.CertificationBody).Where(x => x.IsDeleted == false && x.UserId.ToString() == model.Keyword).ToListAsync();
                UserStdList = _mapper.Map<List<UserAuditModel>>(list);
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
                List<ClientAuditVisitModel> clientAuditsList = new List<ClientAuditVisitModel>();
                clientAuditsList = await GetClientAuditVisitBySearch(model.Keyword);
                foreach (var clientaudit in clientAuditsList)
                {
                    UserAuditModel Us = new UserAuditModel();
                    Us.Id = 00;
                    Us.Organization = clientaudit.ClientName;
                    Us.StandardName = clientaudit.StandardName;
                    Us.Duration =Convert.ToDecimal(clientaudit.Duration);
                    Us.Year = Convert.ToDateTime(clientaudit.StartDate).Year;
                    Us.EacodeName = clientaudit.EACode;
                    Us.NacecodeName = clientaudit.NaceCode;
                    Us.CertificationBodyName = "Ozone";
                    Us.AuditTypeName = "3rd Party";
                    Us.AuditLevel = clientaudit.VisitLevelName;

                    UserStdList.Add(Us);


                }
                //UserStdList.AddRange(_mapper.Map<List<UserAuditModel>>(clientAuditsList));
                result.UserAuditModel =UserStdList;
                //result.UserAuditModel = GetPage(UserStdList, model.Page, model.PageSize);
                result.TotalCount = UserStdList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ClientAuditVisitModel>> GetClientAuditVisitBySearch(string Id)
        {

            try
            {

                string StandardId = "";

                ClientAuditVisitModel Search_Auditor = new ClientAuditVisitModel();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;
                string LeadAuditorId = Id.ToString();
                string VisitLevelId = null;
                string VisitStatusId = null;
                DateTime frmdate = Convert.ToDateTime(fromdate);
             
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<ClientAuditVisitModel> CPRLIst = new List<ClientAuditVisitModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_All_ClientVisitList";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",null),
                    new SqlParameter("@ToDate",null),
                    new SqlParameter("@StandardId",( StandardId == "" ? null :StandardId)),
                    new SqlParameter("@LeadAuditorId",(LeadAuditorId == "" ? null : LeadAuditorId)),
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null :OrganizationId)),
                    new SqlParameter("@VisitStatusId",(VisitStatusId == "" ? null :VisitStatusId)),
                    new SqlParameter("@VisitLevelId",(VisitLevelId== "" ? null :VisitLevelId)),




                    });

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                ClientAuditVisitModel CPR = new ClientAuditVisitModel();

                                //AVGTAT_Auditor.ClientProjectReportModel.Add(new ClientProjectReportModel()
                                //{ClientId
                                CPR.S_No = i++;
                                CPR.Id = (!(dataReader["Id"] is DBNull)) ? Convert.ToInt64(dataReader["Id"]) : 0;
                                CPR.ProjectId = (!(dataReader["ProjectId"] is DBNull)) ? Convert.ToInt64(dataReader["ProjectId"]) : 0;
                                CPR.StandardId = (!(dataReader["StandardId"] is DBNull)) ? Convert.ToInt64(dataReader["StandardId"]) : 0;
                                CPR.ClientId = (!(dataReader["ClientId"] is DBNull)) ? Convert.ToInt64(dataReader["ClientId"]) : 0;
                                CPR.ProjectCode = (!(dataReader["ProjectCode"] is DBNull)) ? dataReader["ProjectCode"].ToString() : null;
                                CPR.ClientName = (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null;
                                CPR.ClientSiteName = (!(dataReader["ClientSiteName"] is DBNull)) ? dataReader["ClientSiteName"].ToString() : null;
                                CPR.VisitStatusName = (!(dataReader["VisitStatusName"] is DBNull)) ? dataReader["VisitStatusName"].ToString() : null;
                                CPR.VisitLevelName = (!(dataReader["VisitLevelName"] is DBNull)) ? dataReader["VisitLevelName"].ToString() : null;
                                CPR.JustifiedPersonName = (!(dataReader["JustifiedPersonName"] is DBNull)) ? dataReader["JustifiedPersonName"].ToString() : null;
                                CPR.TechnicalExpertName = (!(dataReader["TechnicalExpertName"] is DBNull)) ? dataReader["TechnicalExpertName"].ToString() : null;
                                CPR.Duration = (!(dataReader["Duration"] is DBNull)) ? dataReader["Duration"].ToString() : null;
                                CPR.VisitDate = (!(dataReader["VisitDate"] is DBNull)) ? Convert.ToDateTime(dataReader["VisitDate"]) : (DateTime?)null;
                                CPR.StartDate = (!(dataReader["StartDate"] is DBNull)) ? Convert.ToDateTime(dataReader["StartDate"]) : (DateTime?)null;
                                CPR.EndDate = (!(dataReader["EndDate"] is DBNull)) ? Convert.ToDateTime(dataReader["EndDate"]) : (DateTime?)null;
                                CPR.LeadAuditorName = (!(dataReader["LeadAuditorName"] is DBNull)) ? dataReader["LeadAuditorName"].ToString() : null;
                                CPR.Auditor1Name = (!(dataReader["Auditor1Name"] is DBNull)) ? dataReader["Auditor1Name"].ToString() : null;
                                CPR.Auditor2Name = (!(dataReader["Auditor2Name"] is DBNull)) ? dataReader["Auditor2Name"].ToString() : null;
                                CPR.Auditor3Name = (!(dataReader["Auditor3Name"] is DBNull)) ? dataReader["Auditor3Name"].ToString() : null;
                                CPR.Auditor4Name = (!(dataReader["Auditor4Name"] is DBNull)) ? dataReader["Auditor4Name"].ToString() : null;
                                CPR.Auditor5Name = (!(dataReader["Auditor5Name"] is DBNull)) ? dataReader["Auditor5Name"].ToString() : null;
                                CPR.ReviewerName = (!(dataReader["ReviewerName"] is DBNull)) ? dataReader["ReviewerName"].ToString() : null;
                                CPR.ReviewDate = (!(dataReader["ReviewDate"] is DBNull)) ? Convert.ToDateTime(dataReader["ReviewDate"]) : (DateTime?)null;
                                CPR.SubmisionDate = (!(dataReader["SubmisionDate"] is DBNull)) ? Convert.ToDateTime(dataReader["SubmisionDate"]) : (DateTime?)null;
                                CPR.StandardName = (!(dataReader["StandardName"] is DBNull)) ? dataReader["StandardName"].ToString() : null;
                                CPR.NaceCode = (!(dataReader["NaceCode"] is DBNull)) ? dataReader["NaceCode"].ToString() : null;
                                CPR.EACode = (!(dataReader["EACode"] is DBNull)) ? dataReader["EACode"].ToString() : null;
                                CPRLIst.Add(CPR);
                                //}); ;
                            }
                        }
                    }
                }






                return CPRLIst.OrderByDescending(x => x.StartDate).ToList();


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }


        private async Task EnsureConnectionOpenAsync()
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }
        public async Task<string> UserAuditDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                UserAudit dbresult = _dbContext.UserAudit.Where(u => u.Id == id).FirstOrDefault();


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
                        var Exception = ex;
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

        public async Task<List<CertificationBodyModel>> GetAllCertificationBody()
        {
            var result = new List<CertificationBodyModel>();

            var list = await Task.Run(() => _dbContext.CertificationBody.Where(x => x.IsDeleted == false).ToList());
            result = _mapper.Map<List<CertificationBodyModel>>(list);
            return result;
        }
     

    }
}

