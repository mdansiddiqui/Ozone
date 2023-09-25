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
using Ozone.Application.Repository;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
//using static System.Net.Mime.MediaTypeNames;
using System.Web;
//using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Http.Internal;
using Ozone.Application.DTOs.Projects;
using System.Xml.Linq;

namespace Ozone.Infrastructure.Shared.Services
{
    public class AuditReportService : GenericRepositoryAsync<AuditReportDetail>, IAuditReportService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public AuditReportService(
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
        public async Task<string> CreateAuditReport(AuditVisitReportMasterModel input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                AuditReportMaster reportMaster = await Task.Run(() => _dbContext.AuditReportMaster.Where(x => x.Id == input.AuditReportMasterId).FirstOrDefault());

                AuditReportDetail dbreportdetail = await Task.Run(() => _dbContext.AuditReportDetail.Where(x => x.Id == input.Id).FirstOrDefault());
                //AuditReportMaster reportMaster = await Task.Run(() => _dbContext.AuditReportMaster.Where(x => x.Id == input.Id).FirstOrDefault());


                if (input.File != null || dbreportdetail.DocumentFilePath != null)
                {


                    try
                    {
                        //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();
                        //long newmasterid;
                        bool Newrecord = false;
                        if (reportMaster == null)
                        {
                            Newrecord = true;
                            reportMaster = new AuditReportMaster();
                        }

                        reportMaster.ClientAuditVisitId = input.ClientAuditVisitId;
                        reportMaster.ProjectId = input.ProjectId;

                            reportMaster.Minor = input.Minor;
                            reportMaster.Major = input.Major;
                            reportMaster.Critical = input.Critical;
                            reportMaster.TimeBound = input.TimeBound;
                            reportMaster.Observation = input.Observation;



                        if (Newrecord == true)
                        {
                            reportMaster.CreatedDate = DateTime.Now;
                            reportMaster.CreatedById = input.CreatedById;
                            reportMaster.IsDeleted = false;
                            _dbContext.AuditReportMaster.Add(reportMaster);
                        }
                        else
                        {
                            reportMaster.LastModifiedDate = DateTime.Now;
                            reportMaster.LastModifiedById = input.LastModifiedById;
                            _dbContext.Update(reportMaster);
                        }
                        //await base.AddAsync(secuserEntity);
                        //await SecUser.(secuserEntity);
                        // _dbContext.SecUser.Add(secuserEntity);
                        //  ozonedb.Add
                        //  await _secuserRepository.CreateUser(secuserEntity);
                        // await _unitOfWork.SaveChangesAsync();
                        //var result = await _unitOfWork.SaveChangesAsync();

                        // newmasterid = dbreportdetail.Id;

                        //transaction.Commit();


                        long newid;
                        bool New = false;
                        if (dbreportdetail == null)
                        {
                            New = true;
                            dbreportdetail = new AuditReportDetail();
                        }

                    
                        dbreportdetail.AuditDocumentTypeId = input.AuditDocumentTypeId;
                        dbreportdetail.UploadDate = DateTime.Now;



                        if (New == true)
                        {
                            dbreportdetail.CreatedDate = DateTime.Now;
                            dbreportdetail.CreatedById = input.CreatedById;
                            dbreportdetail.IsDeleted = false;
                            reportMaster.AuditReportDetail.Add(dbreportdetail);
                        }
                        else
                        {
                            dbreportdetail.LastModifiedDate = DateTime.Now;
                            dbreportdetail.LastModifiedById = input.LastModifiedById;
                           _dbContext.AuditReportDetail.Update(dbreportdetail);
                        }
                        //await base.AddAsync(secuserEntity);
                        //await SecUser.(secuserEntity);
                        // _dbContext.SecUser.Add(secuserEntity);
                        //  ozonedb.Add
                        //  await _secuserRepository.CreateUser(secuserEntity);
                        // await _unitOfWork.SaveChangesAsync();
                        var result = await _unitOfWork.SaveChangesAsync();

                        newid = dbreportdetail.Id;

                       
                        if (input.File != null)
                        {
                            string filename = Path.GetFileName(input.File.FileName);
                            string ContentType = input.File.ContentType;

                            string reportPath = _configuration["Reporting:AuditPlanPath"];

                            // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                            string newFileName = reportPath + +newid + "_" +Guid.NewGuid() + "_" + filename;
                            // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                            using (var stream = new FileStream(newFileName, FileMode.Create))
                            {
                                await input.File.CopyToAsync(stream);

                            }
                            dbreportdetail.DocumentFilePath = newFileName;
                            dbreportdetail.DocumentContentType = ContentType;
                            await base.UpdateAsync(dbreportdetail);
                            var result2 = await _unitOfWork.SaveChangesAsync();
                        }
                         transaction.Commit();
                        return "Successfully Saved!";



                    }
                    catch (Exception ex)
                    {
                        var Exception = ex;
                        transaction.Rollback();
                        return "Not Inserted!";
                    }



                }
                else
                {
                    return "Please Select Document !";
                }
                //return "User Already Exists!";
            }

        }


        //public async Task<string> Create(AuditReportModel input)
        //{
        //    // OzoneContext ozonedb = new OzoneContext();
        //    using (var transaction = _unitOfWork.BeginTransaction())
        //    {
        //        AuditReport dbreportdetail = await Task.Run(() => _dbContext.AuditReport.Where(x => x.Id == input.Id).FirstOrDefault());


        //        if (input.File != null || dbreportdetail.DocumentFilePath != null)
        //        {
        //            //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


        //            try
        //            {
        //                long newid;
        //                bool New = false;
        //                if (dbreportdetail == null)
        //                {
        //                    New = true;
        //                    dbreportdetail = new AuditReport();
        //                }

        //                dbreportdetail.AuditVisitId =input.AuditVisitId;
        //                dbreportdetail.ProjectId = input.ProjectId;
        //                dbreportdetail.AuditDocumentTypeId = input.AuditDocumentTypeId;
        //                dbreportdetail.UploadDate = DateTime.Now;
                      


        //                if (New == true)
        //                {
        //                    dbreportdetail.CreatedDate = DateTime.Now;
        //                    dbreportdetail.CreatedById = input.CreatedById;
        //                    dbreportdetail.IsDeleted = false;
        //                    await base.AddAsync(dbreportdetail);
        //                }
        //                else
        //                {
        //                    dbreportdetail.LastModifiedDate = DateTime.Now;
        //                    dbreportdetail.LastModifiedById = input.LastModifiedById;
        //                    await base.UpdateAsync(dbreportdetail);
        //                }
        //                //await base.AddAsync(secuserEntity);
        //                //await SecUser.(secuserEntity);
        //                // _dbContext.SecUser.Add(secuserEntity);
        //                //  ozonedb.Add
        //                //  await _secuserRepository.CreateUser(secuserEntity);
        //                // await _unitOfWork.SaveChangesAsync();
        //                var result = await _unitOfWork.SaveChangesAsync();

        //                newid = dbreportdetail.Id;

        //                transaction.Commit();
        //                if (input.File != null)
        //                {
        //                    string filename = Path.GetFileName(input.File.FileName);
        //                    string ContentType = input.File.ContentType;

        //                    string reportPath = _configuration["Reporting:AuditPlanPath"];

        //                    // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

        //                    string newFileName = reportPath + +newid + "_" + filename;
        //                    // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
        //                    using (var stream = new FileStream(newFileName, FileMode.Create))
        //                    {
        //                        await input.File.CopyToAsync(stream);

        //                    }
        //                    dbreportdetail.DocumentFilePath = newFileName;
        //                    dbreportdetail.DocumentContentType = ContentType;
        //                    await base.UpdateAsync(dbreportdetail);
        //                    var result2 = await _unitOfWork.SaveChangesAsync();
        //                }
        //                return "Successfully Saved!";



        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                return "Not Inserted!";
        //            }



        //        }
        //        else
        //        {
        //            return "Please Select Document !";
        //        }
        //        //return "User Already Exists!";
        //    }

        //}

        public async Task<string> AuditReportDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                AuditReportDetail Db = _dbContext.AuditReportDetail.Where(u => u.Id == id).FirstOrDefault();


                if (Db != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        Db.IsDeleted = true;
                        await base.UpdateAsync(Db);
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
                    return "Audit Report not Exists!";
                }


            }
            // return "User Already Exists!";
        }

        public async Task<AuditVisitReportMasterModel> AuditReportBYId(long id)
        {
            var result = new AuditVisitReportMasterModel();
            var Db = await Task.Run(() => _dbContext.AuditReportDetail.Where(x => x.Id == id).FirstOrDefault());
            result = _mapper.Map<AuditVisitReportMasterModel>(Db);
            return result;
        }

        private List<AuditReportModel> GetPage(List<AuditReportModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedAuditReportModel> GetPagedAuditReportResponse(PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedAuditReportModel();
                var AuditReportList = new List<AuditReportModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;

                var list = await Task.Run(() => _dbContext.AuditReport.Include(x=>x.AuditDocumentType).Where(x => x.IsDeleted == false).ToList());
                AuditReportList = _mapper.Map<List<AuditReportModel>>(list);
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

                result.AuditReportModel = GetPage(AuditReportList, model.Page, model.PageSize);
                result.TotalCount = AuditReportList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetPagedAuditReportModel> GetPagedAuditReportResponseById(long id,PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedAuditReportModel();
                var AuditReportList = new List<AuditReportModel>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;

                var list = await Task.Run(() => _dbContext.AuditReport.Include(x => x.AuditDocumentType).Where(x => x.IsDeleted == false && x.AuditVisitId==id).ToList());
                AuditReportList = _mapper.Map<List<AuditReportModel>>(list);
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

                result.AuditReportModel = GetPage(AuditReportList, model.Page, model.PageSize);
                result.TotalCount = AuditReportList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AuditVisitReportMasterModel> DownloadAuditReport(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                AuditReportDetail Db = await Task.Run(() => _dbContext.AuditReportDetail.Where(x => x.Id == id && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault());

                AuditVisitReportMasterModel mod = new AuditVisitReportMasterModel();
                if (Db != null)
                {
                    mod.DocumentFilePath = Db.DocumentFilePath;
                    mod.DocumentContentType = Db.DocumentContentType;

                    //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();
                }
                return mod;
            }
        }
        public async Task<GetPagedAuditVisitReportMasterModel> GetPagedAuditReportDetailById(long id, PagedResponseModel model)
        {
            try
            {
                                                
                var result = new GetPagedAuditVisitReportMasterModel();
                var AuditReportList = new List<AuditVisitReportMasterModel>();
                var auditreportMSmodel= new AuditReportMSModel();
                var DocumentRequiredListModel = new List<MappingDocumentsWithStandardModel>();


                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Module.Include("Module").Include("Status").Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();
                //productDenoList = _mapper.Map<List<ModulesModel>>(list);
                // return list;
             

                var listMaster = await Task.Run(() => _dbContext.AuditReportMaster.Include(x=>x.Project).Include(x=>x.ApprovalStatus).Where(x=> x.IsDeleted == false && x.ClientAuditVisitId == id).FirstOrDefault());
                auditreportMSmodel = _mapper.Map<AuditReportMSModel>(listMaster);

                var list = await Task.Run(() => _dbContext.AuditReportDetail.Include(x=>x.AuditReportMaster).Include(x => x.AuditDocumentType).Include(x=>x.AuditReportMaster.Project).Include(x=>x.AuditReportMaster.ClientAuditVisit).Include(x => x.AuditReportMaster.ClientAuditVisit.VisitLevel).Where(x => x.IsDeleted == false && x.AuditReportMaster.ClientAuditVisitId == id).ToList());
                
                var clientvisit = _dbContext.ClientAuditVisit.Where(x => x.Id == id).FirstOrDefault();
                if (clientvisit.VisitLevelId == 8)
                {
                    var stage1 = _dbContext.ClientAuditVisit.Where(x => x.ProjectId == clientvisit.ProjectId && x.VisitLevelId==7 && x.IsDeleted==false).FirstOrDefault();
                    if (stage1 != null)
                    {
                        var statge1Report = await Task.Run(() => _dbContext.AuditReportDetail.Include(x => x.AuditReportMaster).Include(x => x.AuditDocumentType).Include(x => x.AuditReportMaster.Project).Include(x => x.AuditReportMaster.ClientAuditVisit).Include(x => x.AuditReportMaster.ClientAuditVisit.VisitLevel).Where(x => x.IsDeleted == false && x.AuditReportMaster.ClientAuditVisitId == stage1.Id).ToList());
                        list.AddRange(statge1Report);
                    }

                }


                AuditReportList = _mapper.Map<List<AuditVisitReportMasterModel>>(list);
                foreach (var auditlist in AuditReportList)
                {
                    if (auditlist.DocumentFilePath != null)
                    {
                        string[] subs = auditlist.DocumentFilePath.Split('\\');
                       var FileName= subs.LastOrDefault();
                        //string[] sub = auditlist.DocumentFilePath.Split('_');
                        var filename2 = FileName.Split(new char[] { '_' }, 3);
                        string reportname = filename2.LastOrDefault();
                        auditlist.DocumentFilePath = reportname;


                    }
                }

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

                //result.AuditVisitReportMasterModel = GetPage(AuditReportList, model.Page, model.PageSize);

                result.AuditVisitReportMasterModel = AuditReportList;
                result.TotalCount = AuditReportList.Count();
                result.AuditVisitReportModel = auditreportMSmodel;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<AuditVisitReportMasterModel> GetPage(List<AuditVisitReportMasterModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }


        public async Task<GetPagedQCDocumentsListModel> QCDocumentsList(long id,PagedResponseModel model)

        {
            var result2 = new GetPagedQCDocumentsListModel();
            var result = new List<QCDocumentsListModel>();
           // var clientproject = await Task.Run(() => _dbContext.ClientProjects.Where(x => x.Id==model.projectId ).FirstOrDefault());
            if (model.standardId>0)
            {

                var qcCommentsMaster = new List<QcCommentsMasterModel>();
                var Db = await Task.Run(() => _dbContext.QcdocumentsList.Where(x =>x.StandardId == model.standardId && x.IsActive == true && x.IsDeleted == false).ToList());
                var QcMaster = await Task.Run(() => _dbContext.QcMasterComments.Include(x => x.QcStatus).Where(x => x.ProjectId == model.projectId && x.ClientAuditVisitId==id).ToList());
                result = _mapper.Map<List<QCDocumentsListModel>>(Db);
                qcCommentsMaster = _mapper.Map<List<QcCommentsMasterModel>>(QcMaster);
                ClientAuditVisitModel ClientVisitModel = new ClientAuditVisitModel();
                var ClientModel = await Task.Run(() => _dbContext.ClientAuditVisit.Where(x => x.ProjectId == model.projectId && x.IsDeleted == false && x.Id==id ).FirstOrDefault());
                ClientVisitModel = _mapper.Map<ClientAuditVisitModel>(ClientModel);
                foreach (var res in result)
                {


                    var qcC = qcCommentsMaster.Where(x => x.QcDocumentsId == res.Id).FirstOrDefault();
                    if (qcC != null)
                    {
                        res.StatusId = qcC.QcStatusId;
                        res.StatusName = qcC.StatusName;
                    }
                    else
                    {
                        res.StatusId = 5;
                        res.StatusName = "Pending";

                    }


                }
                result2.QCDocumentsListModel = result;
                result2.ClientAuditVisitModel = ClientVisitModel;
            }
            return result2;
        }
        public async Task<List<QCHistoryModel>> QcHistory(long AuditVisitId)
        {
            var result = new List<QCHistoryModel>();
            var Db = await Task.Run(() => _dbContext.QcHistory.Include(x=>x.QcStatus).Include(x=>x.RemarksBy).Where(x => x.ClientAuditVisitId == AuditVisitId && x.IsDeleted==false).ToList());
            result = _mapper.Map<List<QCHistoryModel>>(Db);
            return result;
        }

        public async Task<string> AddComment(QCHistoryModel input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                QcHistory QCHistoryDb = new QcHistory();
                QcMasterComments QcComments = new QcMasterComments();
                QcComments = _dbContext.QcMasterComments.Where(u => u.ProjectId == input.ProjectId && u.QcDocumentsId==input.QcdocumentsListId).FirstOrDefault();
                QCHistoryDb = _dbContext.QcHistory.Where(u => u.Id == input.Id).FirstOrDefault();
                try
                {

                    if (QcComments != null && QcComments.Id > 0)
                    {
                        QcComments.LastModifiedById = input.RemarksById;
                        QcComments.LastModifiedDate = DateTime.Now;
                        QcComments.QcStatusId = input.QcStatusId;
                        _dbContext.QcMasterComments.Update(QcComments);



                    }
                    else
                    {
                        QcComments.ProjectId = input.ProjectId;
                        QcComments.QcDocumentsId = input.QcdocumentsListId;
                        QcComments.QcStatusId = input.QcStatusId;
                        QcComments.IsActive = true;
                        QcComments.CreatedById = input.RemarksById;
                        QcComments.CreatedDate = DateTime.Now;
                        _dbContext.QcMasterComments.Add(QcComments);
                    }
                    if (QCHistoryDb == null && input.QcdocumentsListId > 0 && input.Remarks != null)
                    {   
                       QCHistoryDb = new QcHistory();
                        QcComments = new QcMasterComments();
                        QCHistoryDb.QcdocumentsListId = input.QcdocumentsListId;
                        QCHistoryDb.Remarks = input.Remarks;
                        QCHistoryDb.RemarksById = input.RemarksById;
                        QCHistoryDb.RemarksDate = DateTime.Now;
                        QCHistoryDb.ProjectId = input.ProjectId;
                        QCHistoryDb.IsDeleted = false;
                     

                       

                        _dbContext.QcHistory.Add(QCHistoryDb);
                   



                    }

                    else 
                    {

                              //QCHistoryDb.RemarksById = input.RemarksById;
                              //QCHistoryDb.RemarksDate = DateTime.Now;
                       QCHistoryDb.Remarks = input.Remarks;
                       QCHistoryDb.RemarksUpdateById = input.RemarksById;
                       QCHistoryDb.RenarksUpdateDate = DateTime.Now;
                        _dbContext.QcHistory.Update(QCHistoryDb);

                       


                    }

                    await _unitOfWork.SaveChangesAsync();
                   transaction.Commit();
                    return "Successfully Saved!";
                 

                }
                catch (Exception ex)
                {
                    var Exception = ex;
                    transaction.Rollback();
                return "Not Inserted!";
            }
        }

        }

        public async Task<string> QCCommentsDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                QcHistory QcHistory = _dbContext.QcHistory.Where(u => u.Id == id).FirstOrDefault();


                if (QcHistory != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        QcHistory.IsDeleted = true;
                       _dbContext.QcHistory.Update(QcHistory);
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

        
        public async Task<List<QcStatus>> GetAllQcStatus()
        {
           

            var list = await Task.Run(() => _dbContext.QcStatus.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
           
            return list;
        }
        public async Task<string> AddCommentList(List<QCHistoryModel> input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                QcHistory QCHistoryDb = new QcHistory();
                QcMasterComments QcComments = new QcMasterComments();
 
                 
                    try
                    {
                   // string mesage = "";
                    
                        foreach (var NewComments in input)
                        {
                            QcComments = _dbContext.QcMasterComments.Where(u => u.ProjectId == NewComments.ProjectId && u.QcDocumentsId == NewComments.QcdocumentsListId && u.ClientAuditVisitId == NewComments.clientAuditVisitId).FirstOrDefault();
                            QCHistoryDb = _dbContext.QcHistory.Where(u => u.Id == NewComments.Id).FirstOrDefault();


                            if (QcComments != null && QcComments.Id > 0)
                            {
                                QcComments.LastModifiedById = NewComments.RemarksById;
                                QcComments.LastModifiedDate = DateTime.Now;
                                QcComments.QcStatusId = NewComments.QcStatusId;
                                _dbContext.QcMasterComments.Update(QcComments);



                            }
                            else
                            {
                                QcComments = new QcMasterComments();
                                QcComments.ProjectId = NewComments.ProjectId;
                                QcComments.ClientAuditVisitId = NewComments.clientAuditVisitId;
                                QcComments.QcDocumentsId = NewComments.QcdocumentsListId;
                                QcComments.QcStatusId = NewComments.QcStatusId;
                                QcComments.IsActive = true;
                                QcComments.CreatedById = NewComments.RemarksById;
                                QcComments.CreatedDate = DateTime.Now;
                                _dbContext.QcMasterComments.Add(QcComments);
                            }
                            if (QCHistoryDb == null && NewComments.QcdocumentsListId > 0 && NewComments.Remarks != null)
                            {
                                QCHistoryDb = new QcHistory();
                               
                                QCHistoryDb.QcdocumentsListId = NewComments.QcdocumentsListId;
                                QCHistoryDb.Remarks = NewComments.Remarks;
                                QCHistoryDb.RemarksById = NewComments.RemarksById;
                                QCHistoryDb.RemarksDate = DateTime.Now;
                                QCHistoryDb.ProjectId = NewComments.ProjectId;
                                QCHistoryDb.ClientAuditVisitId = NewComments.clientAuditVisitId;
                                QCHistoryDb.IsDeleted = false;
                                QCHistoryDb.DocumentContent = NewComments.qcRemarksFile;
                                QCHistoryDb.DocumentContentType = NewComments.DocumentContentType;
                                QCHistoryDb.DocumentName = NewComments.DocumentName;
                                QCHistoryDb.QcStatusId =NewComments.QcStatusId;

                            _dbContext.QcHistory.Add(QCHistoryDb);

                           


                            }

                            else
                            {

                                //QCHistoryDb.RemarksById = input.RemarksById;
                                //QCHistoryDb.RemarksDate = DateTime.Now;
                                QCHistoryDb.Remarks = NewComments.Remarks;
                                QCHistoryDb.RemarksUpdateById = NewComments.RemarksById;
                                QCHistoryDb.RenarksUpdateDate = DateTime.Now;
                                QCHistoryDb.QcStatusId = NewComments.QcStatusId;
                              _dbContext.QcHistory.Update(QCHistoryDb);




                            }

                       
                        //return "Successfully Saved!";
                    }
                    if (input != null && input.Count > 0)
                    {
                          var CommentsList = input.Where(u => u.ProjectId == input[0].ProjectId && u.clientAuditVisitId==input[0].clientAuditVisitId).ToList();
                       // var CommentsList = _dbContext.QcMasterComments.Where(u => u.ProjectId == input[0].ProjectId && u.ClientAuditVisitId==input[0].clientAuditVisitId).ToList();
                       var forcount = CommentsList.Where(x => x.QcStatusId != 1).ToList();
                        var resubmitcount = CommentsList.Where(x => x.QcStatusId == 8).ToList();
                        AuditReportMaster AuditMaster = _dbContext.AuditReportMaster.Where(u => u.ProjectId == input[0].ProjectId && u.ClientAuditVisitId == input[0].clientAuditVisitId).FirstOrDefault();
                        ClientAuditVisit ClientAudit = _dbContext.ClientAuditVisit.Where(u => u.ProjectId == input[0].ProjectId && u.Id == input[0].clientAuditVisitId).FirstOrDefault();
                        AuditReportHistory AuditReportHistoryMod = new AuditReportHistory();
                        if (forcount != null && forcount.Count() > 0)
                        {
                            
                            if (resubmitcount.Count() > 0 && resubmitcount != null)
                            {
                                ClientAudit.VisitStatusId = 4;
                                AuditMaster.ApprovalStatusId = 10002;

                                AuditReportHistoryMod.ApprovalStatusId = 10002;
                                AuditReportHistoryMod.Remarks = "Resubmited QC";
                            }
                            else { 
                                AuditMaster.ApprovalStatusId = 3;
                                ClientAudit.VisitStatusId = 3;
                                AuditReportHistoryMod.ApprovalStatusId = 3;
                                AuditReportHistoryMod.Remarks = "Reject QC";
                            }
                            _dbContext.AuditReportMaster.Update(AuditMaster);
                            _dbContext.ClientAuditVisit.Update(ClientAudit);


                           
                            
                              
                              

                            AuditReportHistoryMod.ClientAuditVisitId = ClientAudit.Id;
                            AuditReportHistoryMod.AuditReportId = AuditMaster.Id;

                            // AuditReportMod.Remarks = NewComments.QcdocumentsListId;
                            AuditReportHistoryMod.RemarksById = input[0].RemarksById;
                            AuditReportHistoryMod.RemarksDate = DateTime.Now;
                            AuditReportHistoryMod.IsDeleted = false;

                            _dbContext.AuditReportHistory.Add(AuditReportHistoryMod);


                        }
                        else 
                        {
                        

                            ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == input[0].ProjectId).FirstOrDefault();

                            var reviewer = await _dbContext.AuditReviewerDocuments.Where(x => x.ClientAuditVisitId == input[0].clientAuditVisitId).ToListAsync();

                            var DocumentList = await Task.Run(() => _dbContext.MappingDocumentsWithStandard.Include(x => x.DocumentType).Where(x => x.IsDeleted == false && x.IsActive == true && x.StandardId == ClientProject.StandardId && x.VisitLevelId == ClientAudit.VisitLevelId && x.DocumentAssignId == 2 && x.IsRequired==true).ToList());
                            foreach (var mapdocuments in DocumentList)
                            {
                                int documentCount=reviewer.Where(x => x.AuditDocumentTypeId == mapdocuments.DocumentTypeId).Count();

                                if (!(documentCount > 0))
                                {
                                    transaction.Rollback();
                                    return "Please Upload all required Reviewer documents";
                                }
                            }


                            ProjectLedger projectLedgerDbmod = new ProjectLedger();
                            projectLedgerDbmod = _dbContext.ProjectLedger.Where(u => u.ProjectId == input[0].ProjectId).FirstOrDefault();




                            var dbprojectAmount = await Task.Run(() => _dbContext.ProjectAmount.Where(x => x.StandardId == ClientProject.StandardId && x.IsDeleted == false && x.OrganizationId == ClientAudit.OrganizationId).OrderByDescending(x => x.Id).FirstOrDefault());

                            if (dbprojectAmount != null)
                            {
                                ClientProject.ProjectAmountId = dbprojectAmount.Id;
                            }
                            else
                            {
                                transaction.Rollback();
                                return "Project Amount Not Set in  Project Amount Form of this Branch Office";
                            }
                            if (projectLedgerDbmod == null)
                            {
                                projectLedgerDbmod = new ProjectLedger();
                                projectLedgerDbmod.Amount = dbprojectAmount.Amount;
                                projectLedgerDbmod.ProjectId = input[0].ProjectId;
                                projectLedgerDbmod.CreatedById = ClientProject.CreatedById;
                                projectLedgerDbmod.LastModifiedById = ClientProject.LastModifiedById;

                                ClientProject.ProjectLedger.Add(projectLedgerDbmod);
                                _dbContext.ClientProjects.Update(ClientProject);
                            }

                            if (resubmitcount.Count() > 0 && resubmitcount != null)
                            {
                                ClientAudit.VisitStatusId = 4;
                                AuditMaster.ApprovalStatusId = 10002;

                                AuditReportHistoryMod.ApprovalStatusId = 10002;
                                AuditReportHistoryMod.Remarks = "Resubmited QC";

                            }
                            else
                            {
                                AuditMaster.ApprovalStatusId = 2;

                                ClientAudit.VisitStatusId = 2;
                                ClientAudit.ApprovedDate = DateTime.Now;
                                ClientAudit.AuditCompletedDate = DateTime.Now;
                                ClientAudit.ApprovedById = input[0].RemarksById;
                                AuditReportHistoryMod.ApprovalStatusId = 2;
                                AuditReportHistoryMod.Remarks = "Approved QC";
                                if (ClientAudit.VisitLevelId == 8 || ClientAudit.VisitLevelId == 21 || ClientAudit.VisitLevelId == 23) 
                                {
                                
                                  ClientProjects ClientProject2 = _dbContext.ClientProjects.Where(u => u.Id == input[0].ProjectId).FirstOrDefault();
                                  ClientProject2.ApprovalStatusId = 3;
                                  ClientProject2.CertificateIssueDate = DateTime.Now;
                                  ClientProject2.CertificationExpiryDate = DateTime.Now.AddYears(3);


                                    var standard=_dbContext.Certification.Where(x => x.Id == ClientProject2.StandardId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                    string Standardcode=standard.Code;
                                    DateTime Date = System.DateTime.Now;
                                    string year = Date.ToString("yyyy");
                                    string Month = Date.ToString("MM");
                                    string Day = Date.ToString("dd");
                                    int? ProjectCount = _dbContext.ClientProjects.Where(x => x.CertificateIssueDate != null && x.IsDeleted == false).Count();
                                    ProjectCount = ProjectCount + 1;

                                    //Random generator = new Random();
                                    //String r = generator.Next(0, 1000000).ToString("D6");
                                    //string RegistrationNo = Standardcode + "-" + r + "-" + ProjectCount;
                                     string RegistrationNo = await registrationno(ProjectCount,Standardcode);



                                    int? cycleCount = _dbContext.ClientProjects.Where(x => x.CertificateIssueDate != null && x.RegistrationNo!=null && x.StandardId==ClientProject2.StandardId && x.ClientId==ClientProject2.ClientId && x.IsDeleted == false).Count();
                                    cycleCount = cycleCount + 1;


                                  
                                    //System.Console.WriteLine(r);

                                    string cycleCode = Standardcode + "-" + year + "-" + cycleCount;
                                    ClientProject2.CycleCode = cycleCode;

                                    ClientProject2.RegistrationNo = RegistrationNo;
                                    _dbContext.ClientProjects.Update(ClientProject2);

                                    ProjectRemarksHistory history = new ProjectRemarksHistory();

                                    history.ApprovalStatusId = 3;

                                    history.Remarks = "QC Approved";
                                    history.RemarksById = input[0].RemarksById;

                                    history.RemarksDate = DateTime.Now;
                                    history.ProjectId = input[0].ProjectId;

                                    history.IsDeleted = false;

                                    _dbContext.ProjectRemarksHistory.Add(history);
                                }
                                //var  projectmaster= _dbContext.ClientProjects.Where(u => u.Id == input[0].ProjectId).FirstOrDefault();
                                //projectmaster.ApprovalStatusId=3;
                                //_dbContext.ClientProjects.Update(projectmaster);

                            }
                            AuditReportHistoryMod.ClientAuditVisitId = ClientAudit.Id;
                            AuditReportHistoryMod.AuditReportId = AuditMaster.Id;

                            // AuditReportMod.Remarks = NewComments.QcdocumentsListId;
                            AuditReportHistoryMod.RemarksById = input[0].RemarksById;
                            AuditReportHistoryMod.RemarksDate = DateTime.Now;
                            AuditReportHistoryMod.IsDeleted = false;

                            _dbContext.AuditReportHistory.Add(AuditReportHistoryMod);
                            //_dbContext.AuditReportHistory.Add(AuditReportHistoryMod);
                            _dbContext.AuditReportMaster.Update(AuditMaster);
                            _dbContext.ClientAuditVisit.Update(ClientAudit);
                        }
                       
                        await _unitOfWork.SaveChangesAsync();
                        transaction.Commit();
                        return "Successfully Saved!";
                    }
                    else {
                        transaction.Rollback();
                        return "Record is Empty!";
                       
                    }
                    //await _unitOfWork.SaveChangesAsync();
                    //transaction.Commit();


                }

                    catch (Exception ex)
                {
                    var Exception = ex;
                    transaction.Rollback();
                        return "Not Inserted!";
                    }
                
            }

        }
        public async Task<string> registrationno(int? ProjectCount,string Standardcode) 
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            string RegistrationNo = Standardcode + "-" + r + "-" + ProjectCount;

            int? db = _dbContext.ClientProjects.Where(x => x.RegistrationNo == RegistrationNo).Count();

            if (db > 0)
            {
                ProjectCount = ProjectCount + 1;
               await registrationno(ProjectCount, Standardcode);
            }

            return RegistrationNo;
        }

        public async Task<string> SubmitForReview(IDictionary<string, long> keyValuePairs)
        {
            try
            {
                long? ReportMasterid = 0;
                long? userId = 0;

                if (keyValuePairs.ContainsKey("reportMasterid"))
                {
                    ReportMasterid = Convert.ToInt64(keyValuePairs["reportMasterid"]);
                }
                if (keyValuePairs.ContainsKey("userId"))
                {
                    userId = Convert.ToInt64(keyValuePairs["userId"]);
                }


               long minor=Convert.ToInt64(keyValuePairs["minor"]);
               long major=Convert.ToInt64(keyValuePairs["major"]);
                long critical = Convert.ToInt64(keyValuePairs["critical"]);
                long timeBound = Convert.ToInt64(keyValuePairs["timeBound"]);
                long observation = Convert.ToInt64(keyValuePairs["observation"]);



                // OzoneContext ozonedb = new OzoneContext();
                using (var transaction = _unitOfWork.BeginTransaction())
                {

                    AuditReportMaster ReportMaster = _dbContext.AuditReportMaster.Where(u => u.Id == ReportMasterid).FirstOrDefault();

                    ClientAuditVisit ClientAuditVisit = _dbContext.ClientAuditVisit.Where(u => u.Id == ReportMaster.ClientAuditVisitId).FirstOrDefault();

                    // ProjectRemarksHistory history = await Task.Run(() => _dbContext.ProjectRemarksHistory.Where(x => x.ProjectId == id).FirstOrDefault());
                    if (ReportMaster != null)
                    {

                        AuditReportHistory AuditReportHistoryMod = new AuditReportHistory();
                        if (ReportMaster.ApprovalStatusId > 0)
                        {
                            ReportMaster.ApprovalStatusId = 10002;
                            ClientAuditVisit.VisitStatusId = 4;
                            AuditReportHistoryMod.ApprovalStatusId = 10002;
                            AuditReportHistoryMod.Remarks = "Resubmitted For QC";
                           
                        }
                        else
                        {

                            AuditReportHistoryMod.ApprovalStatusId = 1;
                            AuditReportHistoryMod.Remarks = "Submit For QC";
                            ReportMaster.ApprovalStatusId = 1;
                            ClientAuditVisit.VisitStatusId = 1;
                            

                        }
                        AuditReportHistoryMod.ClientAuditVisitId = ClientAuditVisit.Id;
                        AuditReportHistoryMod.AuditReportId = ReportMasterid;

                        // AuditReportMod.Remarks = NewComments.QcdocumentsListId;
                        AuditReportHistoryMod.RemarksById = userId;
                        AuditReportHistoryMod.RemarksDate = DateTime.Now;
                        AuditReportHistoryMod.IsDeleted = false;


                        ReportMaster.Minor = minor;
                        ReportMaster.Major = major;
                        ReportMaster.Critical = critical;
                        ReportMaster.TimeBound = timeBound;
                        ReportMaster.Observation = observation;

                        _dbContext.AuditReportHistory.Add(AuditReportHistoryMod);

                        _dbContext.AuditReportMaster.Update(ReportMaster);
                        _dbContext.ClientAuditVisit.Update(ClientAuditVisit);





                        await _unitOfWork.SaveChangesAsync();

                        transaction.Commit();
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return ex;

            }

            //return null;
        }

        public async Task<string> onStageOne(IDictionary<string, long> keyValuePairs)
        {
            try
            {
                long? ReportMasterid = 0;
                long? userId = 0;

                if (keyValuePairs.ContainsKey("reportMasterid"))
                    ReportMasterid = Convert.ToInt64(keyValuePairs["reportMasterid"]);
                if (keyValuePairs.ContainsKey("userId"))
                    userId = Convert.ToInt64(keyValuePairs["userId"]);
                // OzoneContext ozonedb = new OzoneContext();
                using (var transaction = _unitOfWork.BeginTransaction())
                {

                    AuditReportMaster ReportMaster = _dbContext.AuditReportMaster.Where(u => u.Id == ReportMasterid).FirstOrDefault();

                    ClientAuditVisit ClientAuditVisit = _dbContext.ClientAuditVisit.Where(u => u.Id == ReportMaster.ClientAuditVisitId).FirstOrDefault();

                    // ProjectRemarksHistory history = await Task.Run(() => _dbContext.ProjectRemarksHistory.Where(x => x.ProjectId == id).FirstOrDefault());
                    if (ReportMaster != null)
                    {

                        AuditReportHistory AuditReportHistoryMod = new AuditReportHistory();
                        if (ReportMaster.ApprovalStatusId > 0)
                        {
                            ReportMaster.ApprovalStatusId = 2;
                            ClientAuditVisit.VisitStatusId = 2;
                            AuditReportHistoryMod.ApprovalStatusId = 2;
                            AuditReportHistoryMod.Remarks = "Stage O1 Completed";

                        }
                        else
                        {

                            AuditReportHistoryMod.ApprovalStatusId = 2;
                            AuditReportHistoryMod.Remarks = "Stage O1 Completed";
                            ReportMaster.ApprovalStatusId = 2;
                            ClientAuditVisit.VisitStatusId = 2;


                        }
                        AuditReportHistoryMod.ClientAuditVisitId = ClientAuditVisit.Id;
                        AuditReportHistoryMod.AuditReportId = ReportMasterid;

                        // AuditReportMod.Remarks = NewComments.QcdocumentsListId;
                        AuditReportHistoryMod.RemarksById = userId;
                        AuditReportHistoryMod.RemarksDate = DateTime.Now;
                        AuditReportHistoryMod.IsDeleted = false;

                        _dbContext.AuditReportHistory.Add(AuditReportHistoryMod);

                        _dbContext.AuditReportMaster.Update(ReportMaster);
                        _dbContext.ClientAuditVisit.Update(ClientAuditVisit);
                        await _unitOfWork.SaveChangesAsync();

                        transaction.Commit();
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return ex;

            }

            //return null;
        }


    }
}
