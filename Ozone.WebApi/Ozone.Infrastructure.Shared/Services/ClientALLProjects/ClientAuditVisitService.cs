﻿using AutoMapper;
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
using Ozone.Application.Repository;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace Ozone.Infrastructure.Shared.Services
{
  public  class ClientAuditVisitService : GenericRepositoryAsync<ClientAuditVisit>, IClientAuditVisitService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public ClientAuditVisitService(
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

        public async Task<string> Create(ClientAuditVisitModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                if (input.Id > 0)
                {

                    List<ClientAuditVisit> AuditVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Where(x => x.Id!=input.Id  && x.ProjectId == input.ProjectId && x.VisitLevelId == input.VisitLevelId && x.IsDeleted == false).ToList());

                    if (AuditVisit.Count > 0)
                    {
                        return "Visit Level already exit";
                    }
                }
                else
                {
                    List<ClientAuditVisit> AuditVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Where(x => x.ProjectId == input.ProjectId && x.VisitLevelId == input.VisitLevelId && x.IsDeleted == false).ToList());
                
                    if (AuditVisit.Count > 0)
                    {
                        return "Visit Level already exit";
                    }
                }
            var DbdddVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Auditor1).Include(x => x.Auditor2).Include(x => x.Auditor3).Include(x => x.Auditor4).Include(x => x.Auditor5).Include(x => x.LeadAuditor).Include(x => x.JustifiedPerson).Include(x => x.TechnicalExpert).Where(x => x.Id != input.Id && x.IsDeleted == false).FirstOrDefault());
               List<ClientAuditVisitModel> mod = new List<ClientAuditVisitModel>();
                var DbVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Auditor1).Include(x => x.Auditor2).Include(x => x.Auditor3).Include(x => x.Auditor4).Include(x => x.Auditor5).Include(x => x.LeadAuditor).Include(x => x.JustifiedPerson).Include(x => x.TechnicalExpert).Where(x => x.Id != input.Id && x.StartDate <=input.StartDate && x.EndDate >=input.StartDate &&  x.IsDeleted == false && x.OrganizationId==input.OrganizationId).ToList());
                mod = _mapper.Map<List<ClientAuditVisitModel>>(DbVisit);
                //ClientAuditVisit DbVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Where(x => x.Id != input.Id && x.StartDate >= input.StartDate && x.EndDate <= input.EndDate && x.LeadAuditorId == input.LeadAuditorId && x.IsDeleted==false).FirstOrDefault());
                if (mod != null)

                {    

                    var msg = String.Empty;


                    foreach (var Visit in mod)
                    {

                        //if(mod.TechnicalExpertId==input.TechnicalExpertId  && mod.TechnicalExpertId!=null)
                        //{
                        //    // Lead Audit Assigned to an other Audit
                        //    msg += "Technical Expert "+ mod.TechnicalExpertName+ " assigned to another Audit,   ";
                        //}
                        if (Visit.LeadAuditorId > 0)
                        {
                            if (Visit.LeadAuditorId == input.LeadAuditorId || Visit.LeadAuditorId == input.Auditor1Id || Visit.LeadAuditorId == input.Auditor2Id || Visit.LeadAuditorId == input.Auditor3Id || Visit.LeadAuditorId == input.Auditor4Id || Visit.LeadAuditorId == input.Auditor5Id)
                            {

                                // Lead Audit Assigned to an other Audit
                                msg += "Lead Auditor " + Visit.LeadAuditorName + " assigned to another Audit,  ";
                            }
                        }
                        if (Visit.Auditor1Id > 0)
                        {
                            if (Visit.Auditor1Id == input.Auditor1Id || Visit.Auditor1Id == input.LeadAuditorId || Visit.Auditor1Id == input.Auditor2Id || Visit.Auditor1Id == input.Auditor3Id || Visit.Auditor1Id == input.Auditor4Id || Visit.Auditor1Id == input.Auditor5Id)
                            {
                                // Lead Audit Assigned to an other Audit
                                msg += "Auditor_1 " + Visit.Auditor1Name + " assigned to another Audit,   ";
                            }
                        }
                        if (Visit.Auditor2Id > 0)
                        {
                            if (Visit.Auditor2Id == input.Auditor2Id || Visit.Auditor2Id == input.LeadAuditorId || Visit.Auditor2Id == input.Auditor1Id || Visit.Auditor2Id == input.Auditor3Id || Visit.Auditor2Id == input.Auditor4Id || Visit.Auditor2Id == input.Auditor5Id)
                            {
                                // Lead Audit Assigned to an other Audit
                                msg += "Auditor_2 " + Visit.Auditor2Name + " assigned to another Audit,   ";
                            }
                        }
                        if (Visit.Auditor3Id > 0)
                        {
                            if (Visit.Auditor3Id == input.Auditor3Id || Visit.Auditor3Id == input.LeadAuditorId || Visit.Auditor3Id == input.Auditor1Id || Visit.Auditor3Id == input.Auditor2Id || Visit.Auditor3Id == input.Auditor4Id || Visit.Auditor3Id == input.Auditor5Id)
                            {
                                // Lead Audit Assigned to an other Audit
                                msg += "Auditor_3 " + Visit.Auditor3Name + " assigned to another Audit,   ";
                            }
                        }
                        if (Visit.Auditor4Id > 0)
                        {
                            if (Visit.Auditor4Id == input.Auditor4Id || Visit.Auditor4Id == input.LeadAuditorId || Visit.Auditor4Id == input.Auditor1Id || Visit.Auditor4Id == input.Auditor2Id || Visit.Auditor4Id == input.Auditor3Id || Visit.Auditor4Id == input.Auditor5Id)
                            {
                                // Lead Audit Assigned to an other Audit
                                msg += "Auditor_4 " + Visit.Auditor4Name + " assigned to another Audit,   ";
                            }
                        }

                        if (Visit.Auditor5Id > 0)
                        {
                            if (Visit.Auditor5Id == input.Auditor5Id || Visit.Auditor5Id == input.LeadAuditorId || Visit.Auditor5Id == input.Auditor1Id || Visit.Auditor5Id == input.Auditor2Id || Visit.Auditor5Id == input.Auditor3Id || Visit.Auditor5Id == input.Auditor4Id)
                            {
                                // Lead Audit Assigned to an other Audit
                                msg += "Auditor_5 " + Visit.Auditor5Name + " assigned to another Audit,   ";
                            }
                        }
                        if (msg != null && msg != string.Empty)
                        {
                            return msg;
                        }
                    }

                }
               
                ClientAuditVisit DbResilt= await Task.Run(() => _dbContext.ClientAuditVisit.Where(x=>x.Id == input.Id).FirstOrDefault());

                //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


                try
                {
                    
                    long newid;
                    bool New = false;
                    if (DbResilt == null)
                    {
                        New = true;
                        DbResilt = new ClientAuditVisit();
                        DbResilt.CreatedDate = DateTime.Now;
                    }
                    //DbModules.Id = input.Id;
                   // DbResilt.Id = input.Id;
                    DbResilt.ProjectId = input.ProjectId;
                    //DbResilt.VisitTypeId = input.VisitTypeId;
                    DbResilt.VisitStatusId = 5;
                    if (input.JustifiedPersonId > 0)
                    {
                        DbResilt.JustifiedPersonId = input.JustifiedPersonId;
                    }
                    else 
                    {
                        DbResilt.JustifiedPersonId = null;
                    }

                    if (input.TechnicalExpertId > 0)
                    {

                        DbResilt.TechnicalExpertId = input.TechnicalExpertId;
                    }
                    DbResilt.Duration = input.Duration;
                    DbResilt.VisitDate = input.VisitDate;
                    DbResilt.StartDate = input.StartDate;
                    DbResilt.EndDate = input.EndDate;

                    DbResilt.LeadAuditorId = input.LeadAuditorId;
                    if (input.Auditor1Id > 0)
                    {
                        DbResilt.Auditor1Id = input.Auditor1Id;
                    }
                    if (input.Auditor2Id > 0)
                    {
                        DbResilt.Auditor2Id = input.Auditor2Id;
                    }
                    if (input.Auditor3Id > 0)
                    {
                        DbResilt.Auditor3Id = input.Auditor3Id;
                    }
                    if (input.Auditor4Id > 0)
                    {
                        DbResilt.Auditor4Id = input.Auditor4Id;
                    }
                    if (input.Auditor5Id > 0)
                    {
                        DbResilt.Auditor5Id = input.Auditor5Id;
                    }
                    DbResilt.ReviewerId = input.ReviewerId;
                    DbResilt.VerificationLevel = input.VerificationLevel;
                    DbResilt.OrganizationId = input.OrganizationId;
                    DbResilt.VisitLevelId = input.VisitLevelId;
                    //var reviewerdate=input.EndDate.Add
                    // DateTime today = DateTime.Now;
                    DateTime today = Convert.ToDateTime(input.EndDate);
                    DateTime ReviewDate = today.AddDays(5);

                    //DateTime fdate = DateTime.Now.AddDays(10);
                    DateTime fdate = ReviewDate;
                    //DateTime sdate = DateTime.Now.AddDays(-1);
                    DateTime sdate = today;
                    TimeSpan ts = fdate - sdate;
                    var sundays = ((ts.TotalDays / 7) + (sdate.DayOfWeek == DayOfWeek.Sunday || fdate.DayOfWeek == DayOfWeek.Sunday || fdate.DayOfWeek > sdate.DayOfWeek ? 1 : 0));

                    sundays = Math.Round(sundays + .5, MidpointRounding.AwayFromZero);
                    var newReviewdate = ReviewDate.AddDays(sundays);
                    var Holiday = await Task.Run(() => _dbContext.HolidayCalendar.Where(x => x.Date>= input.EndDate && x.Date<= newReviewdate && x.OrganizationId== input.OrganizationId).ToList());
                    int holidaycount = Holiday.Count();
                    newReviewdate = newReviewdate.AddDays(holidaycount);
                    DbResilt.ReviewDate = newReviewdate;

                    DateTime SubmisionDate = newReviewdate.AddDays(3);

                  

                    DateTime SSdate = newReviewdate;
                    //DateTime sdate = DateTime.Now.AddDays(-1);
                    DateTime Fdate = Convert.ToDateTime(SubmisionDate);
                    TimeSpan TSS = Fdate-SSdate;
                    var sssundays = ((TSS.TotalDays / 7) +( SSdate.DayOfWeek == DayOfWeek.Sunday || Fdate.DayOfWeek == DayOfWeek.Sunday || Fdate.DayOfWeek > SSdate.DayOfWeek ? 1 : 0));

                    var SundayForSubmission = Math.Round(sssundays + .5, MidpointRounding.AwayFromZero);
                    SubmisionDate = SubmisionDate.AddDays(SundayForSubmission);

                    var SubmissionDaysCount = await Task.Run(() => _dbContext.HolidayCalendar.Where(x => x.Date >= newReviewdate && x.Date <= SubmisionDate && x.OrganizationId == input.OrganizationId).ToList());

                    var SubmissionDays =SubmissionDaysCount.Count();


                   // DbResilt.ReviewDate = newReviewdate;
                    DbResilt.SubmisionDate = SubmisionDate.AddDays(SubmissionDays);

                    if (New == true)
                    {

                        DbResilt.IsDeleted = false;
                        await base.AddAsync(DbResilt);
                    }
                    else
                    {
                        await base.UpdateAsync(DbResilt);
                    }
                    //await base.AddAsync(secuserEntity);
                    //await SecUser.(secuserEntity);
                    // _dbContext.SecUser.Add(secuserEntity);
                    //  ozonedb.Add
                    //  await _secuserRepository.CreateUser(secuserEntity);
                    // await _unitOfWork.SaveChangesAsync();
                    var result = await _unitOfWork.SaveChangesAsync();
                    newid = DbResilt.Id;
                    //newid = DbAccreditation.Id;

                  
                    if (input.File != null)
                    {
                        string filename = Path.GetFileName(input.File.FileName);
                        string ContentType = input.File.ContentType;

                        string reportPath = _configuration["Reporting:AuditPlanPath"];

                        // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                        string newFileName = reportPath + newid + "_" + Guid.NewGuid() + "_" + filename;
                        // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                        using (var stream = new FileStream(newFileName, FileMode.Create))
                        {
                            await input.File.CopyToAsync(stream);

                        }
                        DbResilt.AuditPlanFilePath = newFileName;
                        DbResilt.AuditPlanContentType = ContentType;
                        await base.UpdateAsync(DbResilt);
                        var result2 = await _unitOfWork.SaveChangesAsync();
                    }
                    transaction.Commit();
                    return "1";

                }
                catch (Exception ex)
                {
                    var Exception = ex;
                    //throw ex;
                    transaction.Rollback();
                    return "0";
                }
              



            }

        }

        public async Task<string> ClientAuditVisitDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                ClientAuditVisit DbResult = _dbContext.ClientAuditVisit.Where(u => u.Id == id).FirstOrDefault();


                if (DbResult != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        DbResult.IsDeleted = true;
                        await base.UpdateAsync(DbResult);
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

        public async Task<ClientAuditVisitModel> GetClientAuditVisitBYId(long id)
        {
            var result = new ClientAuditVisitModel();
            var DbData = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x=>x.Project).Include(x=>x.VisitStatus).Include(x => x.VisitType).Include(x=>x.VisitLevel) .Include(x=>x.Auditor1).Include(x => x.Auditor2).Include(x => x.Auditor3).Include(x => x.Auditor4).Include(x => x.Auditor5).Include(x => x.LeadAuditor).Include(x => x.JustifiedPerson).Include(x => x.TechnicalExpert).Include(x=>x.Reviewer).Where(x => x.Id == id).OrderByDescending(x=>x.Id).FirstOrDefault());
            result = _mapper.Map<ClientAuditVisitModel>(DbData);
            return result;
        }

        private List<ClientAuditVisitModel> GetPage(List<ClientAuditVisitModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedClientAuditVisitModel> GetPagedClientAuditVisitResponse(long ProjectId, PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedClientAuditVisitModel();
                var List = new List<ClientAuditVisitModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;

                var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x=>x.Project).Include(x=>x.VisitStatus).Include(x=>x.VisitType).Include(x=>x.VisitLevel).Where(x => x.IsDeleted == false && x.ProjectId== ProjectId).ToList());
                List = _mapper.Map<List<ClientAuditVisitModel>>(list);
               
                // }


                //else
                //{
                //    var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //                 (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //                x.Code.ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //    productDenoList = _mapper.Map<List<ModulesModel>>(list);
                //    // return list;
                //}
                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.ClientAuditModel = GetPage(List, model.Page, model.PageSize);
                result.TotalCount = List.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GetPagedClientAuditVisitModel> AuditPlan(long id, PagedResponseModel model)
        {


            var list111 = await Task.Run(() => _dbContext.ClientProjects.Include(x => x.ClientAuditVisit).Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList());
            var result = new GetPagedClientAuditVisitModel();
            var List = new List<ClientAuditVisitModel>();
            try
            {

               

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;

            var Userdata= await Task.Run(() => _dbContext.SecUser.Where(x => x.Id == id).FirstOrDefault());
                //if (roleId == 6 || roleId==2)
                //{
                //    var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.VisitStatus).Include(x => x.VisitType).Include(x=>x.Project.Standard).Where(x => x.IsDeleted == false).OrderByDescending(x=>x.Id).ToList());
                //    List = _mapper.Map<List<ClientAuditVisitModel>>(list);

                //}

                //else
                //{
                //    var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.VisitStatus).Include(x => x.VisitType).Include(x=>x.Project.Standard).Where(x => x.IsDeleted == false && x.JustifiedPersonId == id || x.LeadAuditorId == id || x.TechnicalExpertId == id || x.Auditor1Id == id || x.Auditor2Id == id || x.Auditor3Id == id || x.Auditor4Id == id || x.Auditor5Id == id || x.CreatedById == id).OrderByDescending(x=>x.Id).ToList());
                //    List = _mapper.Map<List<ClientAuditVisitModel>>(list);
                //}

                if (Userdata.RoleId == 2 || Userdata.RoleId == 19 || Userdata.RoleId == 21)
                {

                    ///if (model.ViewAllRecord == true)
                    //{
                    var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.VisitStatus).Include(x => x.VisitType).Include(x => x.VisitLevel).Include(x => x.Project.Standard).Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList());
                    List = _mapper.Map<List<ClientAuditVisitModel>>(list);
                    //}
                }
                else if (Userdata.RoleId == 6 || Userdata.RoleId == 20)
                {

                    ///if (model.ViewAllRecord == true)
                    //{
                    var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.VisitStatus).Include(x => x.VisitType).Include(x => x.VisitLevel).Include(x => x.Project.Standard).Where(x => x.IsDeleted == false && x.OrganizationId == Userdata.OrganizationId).OrderByDescending(x => x.Id).ToList());
                    List = _mapper.Map<List<ClientAuditVisitModel>>(list);
                    //}
                }
                else if (Userdata.RoleId == 12)
                {
                    var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.VisitStatus).Include(x => x.VisitType).Include(x => x.VisitLevel).Include(x => x.Project.Standard).Where(x => x.IsDeleted == false && x.OrganizationId == Userdata.OrganizationId).OrderByDescending(x => x.Id).ToList());

                    list = list.Where(x => x.JustifiedPersonId == id || x.LeadAuditorId == id || x.TechnicalExpertId == id || x.Auditor1Id == id || x.Auditor2Id == id || x.Auditor3Id == id || x.Auditor4Id == id || x.Auditor5Id == id || x.CreatedById == id).ToList();
                    List = _mapper.Map<List<ClientAuditVisitModel>>(list);
                }
                else {

                    var list = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.VisitStatus).Include(x => x.VisitType).Include(x => x.VisitLevel).Include(x => x.Project.Standard).Where(x => x.IsDeleted == false && x.OrganizationId == Userdata.OrganizationId).OrderByDescending(x => x.Id).ToList());

                    list = list.Where(x => x.JustifiedPersonId == id || x.LeadAuditorId == id || x.TechnicalExpertId == id || x.Auditor1Id == id || x.Auditor2Id == id || x.Auditor3Id == id || x.Auditor4Id == id || x.Auditor5Id == id || x.CreatedById == id || x.ReviewerId == id).ToList();
                    List = _mapper.Map<List<ClientAuditVisitModel>>(list);

                }


                // }


                //else
                //{
                //    var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //                 (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //                x.Code.ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //    productDenoList = _mapper.Map<List<ModulesModel>>(list);
                //    // return list;
                //}
                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.ClientAuditModel = GetPage(List, model.Page, model.PageSize);
                result.TotalCount = List.Count();
                return result;
            }
            catch (Exception ex)
            {
                var exp = ex;
                return result;
                //throw ex;
            }
        }
        public async Task<ClientAuditVisitModel> DownloadAuditPlan(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ClientAuditVisit Db = await Task.Run(() => _dbContext.ClientAuditVisit.Where(x => x.Id == id && x.IsDeleted==false).OrderByDescending(x=>x.Id).FirstOrDefault());

                ClientAuditVisitModel mod = new ClientAuditVisitModel();
                if (Db != null)
                {
                    mod.AuditPlanFilePath = Db.AuditPlanFilePath;
                    mod.AuditPlanContentType = Db.AuditPlanContentType;

                    //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();
                }
                return mod;
            }
        }

        
       public async Task<string> AuditComplete(ClientAuditVisitModel input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ProjectLedger projectLedgerDbmod = new ProjectLedger();
                ClientAuditVisit Ca = _dbContext.ClientAuditVisit.Where(u => u.Id == input.Id && u.IsDeleted==false).FirstOrDefault();

                if (Ca != null)
                {
                 ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == Ca.ProjectId).FirstOrDefault();
                    Ca.VisitStatusId = 7;

                    Ca.AuditConductedDate = DateTime.Now;
                    await base.UpdateAsync(Ca);
                    await _unitOfWork.SaveChangesAsync();
                    transaction.Commit();
                    return "1";
                }
                else
                {
                    transaction.Rollback();
                    return "0";
                }
            }

        }


        //        public async Task<string> AuditPlaneSubmitForReview(long id, long loginUserId)
        //{
        //    // OzoneContext ozonedb = new OzoneContext();
        //    using (var transaction = _unitOfWork.BeginTransaction())
        //    {
        //        ProjectRemarksHistory history = new ProjectRemarksHistory();
        //        ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == id).FirstOrDefault();
        //        ProjectSa8000 Sa = _dbContext.ProjectSa8000.Where(u => u.ClientProjectId == id).FirstOrDefault();
        //        // ProjectRemarksHistory history = await Task.Run(() => _dbContext.ProjectRemarksHistory.Where(x => x.ProjectId == id).FirstOrDefault());
        //        if (ClientProject != null)
        //        {


        //            if (ClientProject.ApprovalStatusId != 6)
        //            {
        //                //Sa.ApprovalStatusId = 2;

        //                history.ApprovalStatusId = 2;
        //                ClientProject.ApprovalStatusId = 2;
        //            }
        //            else
        //            {
        //                //Sa.ApprovalStatusId = 4;

        //                history.ApprovalStatusId = 4;
        //                ClientProject.ApprovalStatusId = 4;
        //            }
        //            history.Remarks = ClientProject.Remarks;
        //            history.RemarksById = ClientProject.LastModifiedById;
        //            history.RemarksDate = DateTime.Now;
        //            history.ProjectId = id;

        //            history.IsDeleted = false;

        //            ClientProject.ProjectRemarksHistory.Add(history);
        //            //ClientProject.ProjectSa8000.Add(Sa);
        //            await base.UpdateAsync(ClientProject);
        //            await _unitOfWork.SaveChangesAsync();

        //            transaction.Commit();
        //            return "Successfully Record Send For Review !";
        //        }
        //        else
        //        {
        //            return "Reques Not Send For Approval";
        //        }
        //    }

        //}

        public async Task<GetPagedProjectStatusModel> ProjectStatus(long id, PagedResponseModel Pagemodel)
        {
            try
            {
                PagedResponseModel model = Pagemodel;
                var result = new GetPagedProjectStatusModel();

                var Project = new List<ProjectDashboardStatusModel>();
                var Contract = new List<ContractDashboardStatusModel>();
                var Audit = new List<AuditsDashboardStatusModel>();
                var clientproject = new List<ClientProjects>();
                var activityLog = new List<ActivityLog>();
                var clientDashboardStatus = new List<ClientDashboardStatusModel>();
                var projectDashboardStatus = new List<ProjectRequestDashboardStatusModel>();
                var clientSitetRequestDashboardStatus = new List<ClientSitetRequestDashboardStatusModel>();
                List<long> Ids = new List<long>() { 22, 10037 , 37 };

                var clientAuditVisit = new List<ClientAuditVisit>();
                long roleId = id;
              

                int ClientApprovedRecordCount = 0;
                int ClientForReviewRecordCount = 0;
                int ClientOSPActionRecordCount = 0;
                int ClientResubmitedRecordCount = 0;


                // Project Change Request

                int projectApprovedRecordCount = 0;
                int projectForReviewRecordCount = 0;
                int projectOSPActionRecordCount = 0;
                int projectResubmitedRecordCount = 0;


                // Client Site Change Request

                int clientSiteApprovedRecordCount = 0;
                int clientSiteForReviewRecordCount = 0;
                int clientSiteOSPActionRecordCount = 0;
                int clientSiteResubmitedRecordCount = 0;

                if (model.organizationId == 1)
                {
                    clientproject = await Task.Run(() => _dbContext.ClientProjects.Where(x => x.IsDeleted == false).ToList());
                    clientAuditVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Where(x => x.IsDeleted == false).ToList());

                    //For Client Change Request
                    var AL_ClientChanegRequest = new List<ActivityLog>();
                    AL_ClientChanegRequest = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.FormId==12).ToList());
                    var query = AL_ClientChanegRequest


                              .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId })
                              .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();

                    ClientForReviewRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 1).Count();
                    ClientApprovedRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 2).Count();
                    ClientOSPActionRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 3).Count();
                    ClientResubmitedRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 10002).Count();

                    // Project Change Request
                    var AL_ProjectChanegRequest = new List<ActivityLog>();
                    AL_ProjectChanegRequest = await Task.Run(() => _dbContext.ActivityLog.Where(x => Ids.Contains(x.FormId)).ToList());
                    var query1 = AL_ProjectChanegRequest

                              .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId ,x.FormId})
                              .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();
                    projectForReviewRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 1).Count();
                    projectApprovedRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 2).Count();
                    projectOSPActionRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 3).Count();
                    projectResubmitedRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 10002).Count();


                    // Client Site Change Request
                    var AL_ClientSiteChanegRequest = new List<ActivityLog>();
                    AL_ClientSiteChanegRequest = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.FormId == 10066).ToList());
                    var query2 = AL_ClientSiteChanegRequest

                              .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId })
                              .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();
                    clientSiteForReviewRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 1).Count();
                    clientSiteApprovedRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 2).Count();
                    clientSiteOSPActionRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 3).Count();
                    clientSiteResubmitedRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 10002).Count();

                }
                else
                {
                    clientproject = await Task.Run(() => _dbContext.ClientProjects.Include(x => x.Client).Where(x => x.IsDeleted == false && x.Client.OrganizationId == model.organizationId).ToListAsync());
                    if (roleId == 12)
                    {

                        clientAuditVisit   = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.OrganizationId == model.organizationId ).OrderByDescending(x => x.Id).ToListAsync());

                        clientAuditVisit = clientAuditVisit.Where(x => x.TechnicalExpertId == model.userId || x.JustifiedPersonId == model.userId || x.LeadAuditorId == model.userId || x.Auditor1Id == model.userId || x.Auditor2Id == model.userId || x.Auditor3Id == model.userId || x.Auditor4Id == model.userId || x.Auditor5Id == model.userId).ToList();
                        //clientAuditVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.Client.OrganizationId == model.organizationId && x.ReviewerId == model.userId).ToList());
                    }
                    else 
                    {
                      
                        clientAuditVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.OrganizationId == model.organizationId).ToList());


                        // Client Request
                        var AL_ClientChanegRequest = new List<ActivityLog>();
                        AL_ClientChanegRequest = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.FormId == 12 && x.OrganizationId == model.organizationId).ToList());
                        var query = AL_ClientChanegRequest


                           .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId })
                           .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();

                        ClientForReviewRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 1).Count();
                        ClientApprovedRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 2).Count();
                        ClientOSPActionRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 3).Count();
                        ClientResubmitedRecordCount = query.Where(x => x.Peo.ApprovalStatusId == 10002).Count();

                        // Project Change Request
                        var AL_ProjectChanegRequest = new List<ActivityLog>();
                        AL_ProjectChanegRequest = await Task.Run(() => _dbContext.ActivityLog.Where(x => Ids.Contains(x.FormId) && x.OrganizationId == model.organizationId).ToList());

                        var rejectProj = AL_ProjectChanegRequest.Where(x => x.ApprovalStatusId == 3).ToList();
                        var query1 = AL_ProjectChanegRequest

                                  .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId, x.FormId })
                                  .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();
                        projectForReviewRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 1).Count();
                        projectApprovedRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 2).Count();
                        projectOSPActionRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 3).Count();
                        projectResubmitedRecordCount = query1.Where(x => x.Peo.ApprovalStatusId == 10002).Count();


                        // Client Site Change Request
                        var AL_ClientSiteChanegRequest = new List<ActivityLog>();
                        AL_ClientSiteChanegRequest = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.FormId == 10066 && x.OrganizationId == model.organizationId).ToList());
                        var query2 = AL_ClientSiteChanegRequest

                                  .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId })
                                  .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();
                        clientSiteForReviewRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 1).Count();
                        clientSiteApprovedRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 2).Count();
                        clientSiteOSPActionRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 3).Count();
                        clientSiteResubmitedRecordCount = query2.Where(x => x.Peo.ApprovalStatusId == 10002).Count();

                    }
                }
               
                //List<UserStandards> Userstandard = new List<UserStandards>();
                if (roleId == 19 || roleId == 21)
                {
                    //Userstandard = await Task.Run(() => _dbContext.UserStandards.Where(x => x.IsDeleted == false && x.UserId == model.userId && x.AuditorTypeId == 4).ToList());

                    var Userstandard = (from m in _dbContext.UserStandards
                                         .Where(x => x.IsDeleted == false && x.UserId == model.userId && x.AuditorTypeId == 4)
                                        group m by m.StandardId into g
                                        select new
                                        {
                                            StandardId = g.Key,

                                        }).ToList();
                    //var idList = new int[4, 5];
                    // var idList = new long?[4,5];
                    //var tt =_dbContext.UserStandards
                    //           .Where(t => idList.Contains(t.AuditorTypeId));
                    //var userProfiles = _dbContext.UserStandards
                    //           .Where(t => idList.Contains(t.AuditorTypeId));

                    //For project Approal
                    int ApprovedRecordCount = 0;
                    int ForReviewRecordCount = 0;
                    int OSPActionRecordCount = 0;
                    int ResubmitedRecordCount = 0;

                    //for Contract Approval
                    int ApprovedContract = 0;
                    int ForReviewContract = 0;
                    int OSPActionContract = 0;
                    int ResubmitedContract = 0;

                    //Audit
                    int ApprovedAudit = 0;
                    int ForReviewAudit = 0;
                    int OSPActionAudit = 0;
                    int ResubmitedAudit = 0;
                    int Booked = 0;
                    int AuditConducted = 0;

                    foreach (var standard in Userstandard)
                    {  // for Application
                        ApprovedRecordCount = ApprovedRecordCount + clientproject.Where(x => x.ApprovalStatusId == 1 && x.StandardId == standard.StandardId).Count();
                        ForReviewRecordCount = ForReviewRecordCount + clientproject.Where(x => x.ApprovalStatusId == 4 && x.StandardId == standard.StandardId).Count();
                        OSPActionRecordCount = OSPActionRecordCount + clientproject.Where(x => x.ApprovalStatusId == 5 && x.StandardId == standard.StandardId).Count();
                        ResubmitedRecordCount = ResubmitedRecordCount + clientproject.Where(x => x.ApprovalStatusId == 2 && x.StandardId == standard.StandardId).Count();

                        //for Contract 
                        ApprovedContract = ApprovedContract + clientproject.Where(x => x.ApprovalStatusId == 7 && x.StandardId == standard.StandardId).Count();
                        ForReviewContract = ForReviewContract + clientproject.Where(x => x.ApprovalStatusId == 9 && x.StandardId == standard.StandardId).Count();
                        OSPActionContract = OSPActionContract + clientproject.Where(x => x.ApprovalStatusId == 8 && x.StandardId == standard.StandardId).Count();
                        ResubmitedContract = ResubmitedContract + clientproject.Where(x => x.ApprovalStatusId == 10 && x.StandardId == standard.StandardId).Count();



                        Booked = Booked + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 5 && x.ReviewerId == model.userId).Count());
                        ApprovedAudit = ApprovedAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 2 && x.ReviewerId == model.userId).Count());
                        ForReviewAudit = ForReviewAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 1 && x.ReviewerId == model.userId).Count());
                        OSPActionAudit = OSPActionAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 3 && x.ReviewerId == model.userId).Count());
                        ResubmitedAudit = ResubmitedAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 4 && x.ReviewerId == model.userId).Count());
                        AuditConducted = AuditConducted + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 7 && x.ReviewerId == model.userId).Count());


                        //if (roleId == 21)
                        //{
                        //    Booked = Booked + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 5 && x.ReviewerId == model.userId).Count());
                        //    //ApprovedAudit = ApprovedAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 2 && x.ReviewerId == model.userId).Count());
                        //    ForReviewAudit = ForReviewAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 1 && x.ReviewerId == model.userId).Count());
                        //    OSPActionAudit = OSPActionAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 3 && x.ReviewerId == model.userId).Count());
                        //    ResubmitedAudit = ResubmitedAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 4 && x.ReviewerId == model.userId).Count());


                        //}
                        //else
                        //{
                        //    Booked = Booked + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 5).Count());
                        //    // ApprovedAudit = ApprovedAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 2).Count());
                        //    ForReviewAudit = ForReviewAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 1).Count());
                        //    OSPActionAudit = OSPActionAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 3).Count());
                        //    ResubmitedAudit = ResubmitedAudit + await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Where(x => x.IsDeleted == false && x.Project.StandardId == standard.StandardId && x.VisitStatusId == 4).Count());
                        //}
                    }

                    ProjectDashboardStatusModel PS = new ProjectDashboardStatusModel();
                    PS.Id = 1;
                    PS.Name = "Approved";
                    //PS.Count = clientproject.Where(x => x.ApprovalStatusId == 1).Count();
                    PS.Count = ApprovedRecordCount;
                    Project.Add(PS);

                    PS = new ProjectDashboardStatusModel();
                    PS.Id = 4;
                    PS.Name = "Submited For Review";
                    // PS.Count = clientproject.Where(x => x.ApprovalStatusId == 4 ).Count();
                    PS.Count = ForReviewRecordCount;
                    Project.Add(PS);

                    PS = new ProjectDashboardStatusModel();
                    PS.Id = 5;
                    PS.Name = "Rejected";
                    //PS.Count = clientproject.Where(x => x.ApprovalStatusId == 5).Count();
                    PS.Count = OSPActionRecordCount;
                    Project.Add(PS);

                    PS = new ProjectDashboardStatusModel();
                    PS.Id = 2;
                    PS.Name = "Resubmited";
                    //PS.Count = clientproject.Where(x => x.ApprovalStatusId == 2).Count();
                    PS.Count = ResubmitedRecordCount;
                    Project.Add(PS);

                    result.ProjectDashboardStatusModel = Project;



                    //contract

                    ContractDashboardStatusModel ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 7;
                    ContractDashboard.Name = "Approved";
                    // ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 7).Count();
                    ContractDashboard.Count = ApprovedContract;
                    Contract.Add(ContractDashboard);

                    ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 9;
                    ContractDashboard.Name = "Submited For Review";
                    // ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 9).Count();
                    ContractDashboard.Count = ForReviewContract;
                    Contract.Add(ContractDashboard);

                    ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 8;
                    ContractDashboard.Name = "Rejected";
                    //ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 8).Count();
                    ContractDashboard.Count = OSPActionContract;
                    Contract.Add(ContractDashboard);


                    ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 10;
                    ContractDashboard.Name = "Resubmited";
                    //ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 10).Count();
                    ContractDashboard.Count = ResubmitedContract;
                    Contract.Add(ContractDashboard);

                    result.ContractDashboardStatusModel = Contract;


                    //Audit
                    AuditsDashboardStatusModel AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 2;
                    AuditDashboard.Name = "Approved";
                    //AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    AuditDashboard.Count = ApprovedAudit;
                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 5;
                    AuditDashboard.Name = "Booked";
                    //AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    AuditDashboard.Count = Booked;
                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 7;
                    AuditDashboard.Name = "Audit Conducted";
                    //AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    AuditDashboard.Count = AuditConducted;
                    Audit.Add(AuditDashboard);


                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 1;
                    AuditDashboard.Name = "Submited For QC";
                    //AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 1).Count();
                    AuditDashboard.Count = ForReviewAudit;
                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 3;
                    AuditDashboard.Name = "Rejected";
                    // AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 3).Count();
                    AuditDashboard.Count = OSPActionAudit;
                    Audit.Add(AuditDashboard);


                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 4;
                    AuditDashboard.Name = "Resubmited";
                    // AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 3).Count();
                    AuditDashboard.Count = ResubmitedAudit;
                    Audit.Add(AuditDashboard);
                    result.AuditsDashboardStatusModel = Audit;



                    //Client Change Request
                    ClientDashboardStatusModel ClientDashboard = new ClientDashboardStatusModel();
                    //AuditDashboard.Id = 2;
                    //AuditDashboard.Name = "Approved";
                    ////AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    //AuditDashboard.Count = ApprovedAudit;
                    //Audit.Add(AuditDashboard);

                    ClientDashboard.Id = 2;
                    ClientDashboard.Name = "Approved";
                    //AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    ClientDashboard.Count = ClientApprovedRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);

                    ClientDashboard = new ClientDashboardStatusModel();
                    ClientDashboard.Id = 1;
                    ClientDashboard.Name = "Submited For Review";
                    ClientDashboard.Count = ClientForReviewRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);

                    ClientDashboard = new ClientDashboardStatusModel();
                    ClientDashboard.Id = 3;
                    ClientDashboard.Name = "Rejected";
                    ClientDashboard.Count = ClientOSPActionRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);


                    ClientDashboard = new ClientDashboardStatusModel();
                    ClientDashboard.Id = 10002;
                    ClientDashboard.Name = "Resubmited";
                    ClientDashboard.Count = ClientResubmitedRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);
                    result.ClientDashboardStatusModel = clientDashboardStatus;



                    //Project Change Request
                    ProjectRequestDashboardStatusModel projectDashboard = new ProjectRequestDashboardStatusModel();
                 
                    projectDashboard.Id = 2;
                    projectDashboard.Name = "Approved";
                    projectDashboard.Count = projectApprovedRecordCount;
                    projectDashboardStatus.Add(projectDashboard);

                    projectDashboard = new ProjectRequestDashboardStatusModel();
                    projectDashboard.Id = 1;
                    projectDashboard.Name = "Submited For Review";
                    projectDashboard.Count = projectForReviewRecordCount;
                    projectDashboardStatus.Add(projectDashboard);

                    projectDashboard = new ProjectRequestDashboardStatusModel();
                    projectDashboard.Id = 3;
                    projectDashboard.Name = "Rejected";
                    projectDashboard.Count = projectOSPActionRecordCount;
                    projectDashboardStatus.Add(projectDashboard);


                    projectDashboard = new ProjectRequestDashboardStatusModel();
                    projectDashboard.Id = 10002;
                    projectDashboard.Name = "Resubmited";
                    projectDashboard.Count = projectResubmitedRecordCount;
                    projectDashboardStatus.Add(projectDashboard);

                    result.ProjectRequestDashboardStatusModel = projectDashboardStatus;


                    //Client Site Change Request
                    ClientSitetRequestDashboardStatusModel clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();

                    clientSiteDashboard.Id = 2;
                    clientSiteDashboard.Name = "Approved";
                    clientSiteDashboard.Count = clientSiteApprovedRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);

                    clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();
                    clientSiteDashboard.Id = 1;
                    clientSiteDashboard.Name = "Submited For Review";
                    clientSiteDashboard.Count = clientSiteForReviewRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);

                    clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();
                    clientSiteDashboard.Id = 3;
                    clientSiteDashboard.Name = "Rejected";
                    clientSiteDashboard.Count = clientSiteOSPActionRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);


                    clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();
                    clientSiteDashboard.Id = 10002;
                    clientSiteDashboard.Name = "Resubmited";
                    clientSiteDashboard.Count = clientSiteResubmitedRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);

                    result.ClientSitetRequestDashboardStatusModel = clientSitetRequestDashboardStatus;


                }

                else
                {

                    ProjectDashboardStatusModel PS = new ProjectDashboardStatusModel();


                    PS.Id = 1;
                    PS.Name = "Approved";
                    PS.Count = clientproject.Where(x => x.ApprovalStatusId == 1).Count();

                    Project.Add(PS);
                    PS = new ProjectDashboardStatusModel();
                    PS.Id = 4;
                    PS.Name = "Submited For Review";
                    PS.Count = clientproject.Where(x => x.ApprovalStatusId == 4).Count();

                    Project.Add(PS);

                    PS = new ProjectDashboardStatusModel();
                    PS.Id = 5;
                    PS.Name = "Rejected";
                    PS.Count = clientproject.Where(x => x.ApprovalStatusId == 5).Count();

                    Project.Add(PS);

                    PS = new ProjectDashboardStatusModel();
                    PS.Id = 2;
                    PS.Name = "Resubmited";
                    PS.Count = clientproject.Where(x => x.ApprovalStatusId == 2).Count();

                    Project.Add(PS);

                    result.ProjectDashboardStatusModel = Project;



                    // Contact
                    ContractDashboardStatusModel ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 7;
                    ContractDashboard.Name = "Approved";
                    ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 7).Count();

                    Contract.Add(ContractDashboard);

                    ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 9;
                    ContractDashboard.Name = "Submited For Review";
                    ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 9).Count();

                    Contract.Add(ContractDashboard);

                    ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 8;
                    ContractDashboard.Name = "Rejected";
                    ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 8).Count();

                    Contract.Add(ContractDashboard);
                    ContractDashboard = new ContractDashboardStatusModel();
                    ContractDashboard.Id = 10;
                    ContractDashboard.Name = "Resubmited";
                    ContractDashboard.Count = clientproject.Where(x => x.ApprovalStatusId == 10).Count();

                    Contract.Add(ContractDashboard);

                    result.ContractDashboardStatusModel = Contract;


                    //Audit

                    AuditsDashboardStatusModel AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 2;
                    AuditDashboard.Name = "Approved";
                    AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 5;
                    AuditDashboard.Name = "Booked";
                    AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 5).Count();
                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 7;
                    AuditDashboard.Name = "Audit Conducted";
                    AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 7).Count();
                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 1;
                    AuditDashboard.Name = "Submited For QC";
                    AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 1).Count();

                    Audit.Add(AuditDashboard);

                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 3;
                    AuditDashboard.Name = "Rejected";
                    AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 3).Count();

                    Audit.Add(AuditDashboard);


                    AuditDashboard = new AuditsDashboardStatusModel();
                    AuditDashboard.Id = 4;
                    AuditDashboard.Name = "Resubmited";
                    AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 4).Count();

                    Audit.Add(AuditDashboard);

                    result.AuditsDashboardStatusModel = Audit;


                    //Client Change Request
                    ClientDashboardStatusModel ClientDashboard = new ClientDashboardStatusModel();
                    //AuditDashboard.Id = 2;
                    //AuditDashboard.Name = "Approved";
                    ////AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    //AuditDashboard.Count = ApprovedAudit;
                    //Audit.Add(AuditDashboard);

                    ClientDashboard.Id = 2;
                    ClientDashboard.Name = "Approved";
                    //AuditDashboard.Count = clientAuditVisit.Where(x => x.VisitStatusId == 2).Count();
                    ClientDashboard.Count = ClientApprovedRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);

                    ClientDashboard = new ClientDashboardStatusModel();
                    ClientDashboard.Id = 1;
                    ClientDashboard.Name = "Submited For Review";
                    ClientDashboard.Count = ClientForReviewRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);

                    ClientDashboard = new ClientDashboardStatusModel();
                    ClientDashboard.Id = 3;
                    ClientDashboard.Name = "Rejected";
                    ClientDashboard.Count = ClientOSPActionRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);


                    ClientDashboard = new ClientDashboardStatusModel();
                    ClientDashboard.Id = 10002;
                    ClientDashboard.Name = "Resubmited";
                    ClientDashboard.Count = ClientResubmitedRecordCount;
                    clientDashboardStatus.Add(ClientDashboard);

                    result.ClientDashboardStatusModel = clientDashboardStatus;



                    //Project Change Request
                    ProjectRequestDashboardStatusModel projectDashboard = new ProjectRequestDashboardStatusModel();

                    projectDashboard.Id = 2;
                    projectDashboard.Name = "Approved";
                    projectDashboard.Count = projectApprovedRecordCount;
                    projectDashboardStatus.Add(projectDashboard);

                    projectDashboard = new ProjectRequestDashboardStatusModel();
                    projectDashboard.Id = 1;
                    projectDashboard.Name = "Submited For Review";
                    projectDashboard.Count = projectForReviewRecordCount;
                    projectDashboardStatus.Add(projectDashboard);

                    projectDashboard = new ProjectRequestDashboardStatusModel();
                    projectDashboard.Id = 3;
                    projectDashboard.Name = "Rejected";
                    projectDashboard.Count = projectOSPActionRecordCount;
                    projectDashboardStatus.Add(projectDashboard);


                    projectDashboard = new ProjectRequestDashboardStatusModel();
                    projectDashboard.Id = 10002;
                    projectDashboard.Name = "Resubmited";
                    projectDashboard.Count = projectResubmitedRecordCount;
                    projectDashboardStatus.Add(projectDashboard);

                    result.ProjectRequestDashboardStatusModel = projectDashboardStatus;


                    //Client Site Change Request
                    ClientSitetRequestDashboardStatusModel clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();

                    clientSiteDashboard.Id = 2;
                    clientSiteDashboard.Name = "Approved";
                    clientSiteDashboard.Count = clientSiteApprovedRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);

                    clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();
                    clientSiteDashboard.Id = 1;
                    clientSiteDashboard.Name = "Submited For Review";
                    clientSiteDashboard.Count = clientSiteForReviewRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);

                    clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();
                    clientSiteDashboard.Id = 3;
                    clientSiteDashboard.Name = "Rejected";
                    clientSiteDashboard.Count = clientSiteOSPActionRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);


                    clientSiteDashboard = new ClientSitetRequestDashboardStatusModel();
                    clientSiteDashboard.Id = 10002;
                    clientSiteDashboard.Name = "Resubmited";
                    clientSiteDashboard.Count = clientSiteResubmitedRecordCount;
                    clientSitetRequestDashboardStatus.Add(clientSiteDashboard);

                    result.ClientSitetRequestDashboardStatusModel = clientSitetRequestDashboardStatus;
                }





                //PS.Count

                //result.ClientAuditModel = GetPage(List, model.Page, model.PageSize);
                //result.TotalCount = List.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}