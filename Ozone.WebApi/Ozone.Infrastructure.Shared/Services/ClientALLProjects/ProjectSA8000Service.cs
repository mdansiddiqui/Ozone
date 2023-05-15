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
using System.ComponentModel;
using Ozone.Application.DTOs.Projects;

namespace Ozone.Infrastructure.Shared.Services
{
  public  class ProjectSA8000Service : GenericRepositoryAsync<ClientProjects>, IProjectSA8000Service
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;

        public ProjectSA8000Service(
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
        private static bool newproject=true;
        public async Task<string> Create(ProjectSA8000CreateModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ProjectRemarksHistory  history= new ProjectRemarksHistory();
                ClientProjects Dbproject = null;
                ProjectSa8000 DbResult = null;
                 Dbproject = await Task.Run(() => _dbContext.ClientProjects.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault());
                if (Dbproject != null)
                { 
                    DbResult = await Task.Run(() => _dbContext.ProjectSa8000.Where(x => x.ClientProjectId == Dbproject.Id && x.IsDeleted == false).FirstOrDefault()); 
                }

                //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();


                try
                {
                    var message = "";
                    long newid;
                    bool New = false;
                    if ( Dbproject==null)
                    {
                        New = true;
                        Dbproject = new ClientProjects();
                        DbResult = new ProjectSa8000();
                        Dbproject.Date = DateTime.Now;

                        Dbproject.StandardId = input.StandardId;
                        Dbproject.ApprovalStatusId = 6;
                        Dbproject.ClientId = input.ClientId;
                        Dbproject.ProjectCode= GetProgectCode(input.ClientSiteId, Dbproject.StandardId,input.ClientId);
                        if (newproject == true)
                        {
                            if (input.ProjectTypeId != null && input.ProjectTypeId > 0)
                            {
                                Dbproject.ProjectTypeId = input.ProjectTypeId;

                            }
                            else
                            {
                                Dbproject.ProjectTypeId = 3;
                            }
                        }
                        else
                        {
                            Dbproject.ProjectTypeId = 2;
                        }
                    }
                    ///// ClientProject
                    if (input.ClientSiteId > 0)
                    {
                        Dbproject.ClientSiteId = input.ClientSiteId;
                    }
                    // Dbproject.StandardId = 7;
                    if (input.VerificationTypeId > 0)
                    {
                        Dbproject.VerificationTypeId = input.VerificationTypeId;
                    }


                    Dbproject.ConsultantId = input.ConsultantId;
                    Dbproject.Scope = input.Scope;
                    Dbproject.Remarks = input.Remarks;
                    Dbproject.SurveillanceVisitFrequencyId = input.SurveillanceVisitFrequencyId;


                    DbResult.Code = input.Code;
                   // DbResult.ProjectTypeId = input.ProjectTypeId;
                    DbResult.RiskId = input.RiskId;
                    DbResult.EacodeId = input.EacodeId;
                    DbResult.NaceCodeId = input.NaceCodeId;
                    DbResult.ConsultantId = input.ConsultantId;
                    DbResult.AccreditationId = input.AccreditationId;
                    DbResult.Scope = input.Scope;
                    DbResult.PreAuditDuration = input.PreAuditDuration;
                    DbResult.DurationStage1 = input.DurationStage1;
                    DbResult.DurationStage2 = input.DurationStage2;
                    DbResult.DurationSurvVisit = input.DurationSurvVisit;

                    DbResult.SurveillanceMethodId = input.SurveillanceMethodId;
                    DbResult.SurveillanceVisitFrequencyId = input.SurveillanceVisitFrequencyId;
                    DbResult.NoOfSurveillanceVisits = input.NoOfSurveillanceVisits;
                    DbResult.Applicatonfee = input.Applicatonfee;
                    DbResult.PreAuditfee = input.PreAuditfee;
                    DbResult.Assessmentfee = input.Assessmentfee;
                    DbResult.Survfee = input.Survfee;
                    DbResult.ExpensesId = input.ExpensesId;
                  
                    DbResult.Remarks = input.Remarks;
                    




                    if (New == true)
                    {                                            
                        DbResult.CreatedById = input.CreatedById;
                        DbResult.CreatedDate = DateTime.Now;

                        Dbproject.CreatedById = input.CreatedById;
                        Dbproject.CreatedDate = DateTime.Now;
                        DbResult.IsDeleted = false;  
                       
                        Dbproject.IsActive = true;
                        Dbproject.IsDeleted = false;
                        Dbproject.ProjectSa8000.Add(DbResult);
                        await base.AddAsync(Dbproject);
                        //message = "Successfully Inserted!";
                        message = "1";
                    }
                    else
                    {
                        DbResult.LastModifiedById = input.LastModifiedById;
                        DbResult.LastModifiedDate =DateTime.Now;
                        Dbproject.LastModifiedById = input.LastModifiedById;
                        Dbproject.LastModifiedDate = DateTime.Now;
                        if (Dbproject.ProjectTypeId == 2 && input.ProjectTypeId == 0)
                        {
                            Dbproject.ProjectTypeId = 2;
                        }
                        else if (Dbproject.ProjectTypeId == 2 && input.ProjectTypeId == 1)
                        {
                            Dbproject.ProjectTypeId = 2;
                        }
                        else if (input.ProjectTypeId == 0)
                        {
                            Dbproject.ProjectTypeId = 3;
                        }
                        else if (input.ProjectTypeId == 1)
                        {
                            Dbproject.ProjectTypeId = 1;
                        }

                        
                        await base.UpdateAsync(Dbproject);
                      
                        message = "2";
                    }
                    //await base.AddAsync(secuserEntity);
                    //await SecUser.(secuserEntity);
                    // _dbContext.SecUser.Add(secuserEntity);
                    //  ozonedb.Add
                    //  await _secuserRepository.CreateUser(secuserEntity);
                    // await _unitOfWork.SaveChangesAsync();
                    var result = await _unitOfWork.SaveChangesAsync();
                    newid = DbResult.Id;
                    if (input.ApplicationForm != null)
                    {
                        string filename = Path.GetFileName(input.ApplicationForm.FileName);
                        string ContentType = input.ApplicationForm.ContentType;

                        string reportPath = _configuration["Reporting:ProjectsSA8000Path"];

                        // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                        string newFileName = reportPath + newid +"_" +Guid.NewGuid() + "_" + filename;
                        // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                        using (var stream = new FileStream(newFileName, FileMode.Create))
                        {
                            await input.ApplicationForm.CopyToAsync(stream);

                        }
                        DbResult.ApplicationFormPath = newFileName;
                        DbResult.ApplicationContentType = ContentType;
                        _dbContext.Update(DbResult);
                        var result2 = await _unitOfWork.SaveChangesAsync();

                    }


                    transaction.Commit();
                    return message;

                }

                catch (Exception ex)
                {
                    var Exception = ex;
                    transaction.Rollback();
                    return "0";
                }



            }

        }


        //public async Task<string> ProjectSA8000ProjectStatusChange(ClientProjectModel input)
        //{
        //    // OzoneContext ozonedb = new OzoneContext();
        //    using (var transaction = _unitOfWork.BeginTransaction())
        //    {
        //        ProjectRemarksHistory history = new ProjectRemarksHistory();
        //        ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == input.Id).FirstOrDefault();
        //        var secuser = _dbContext.SecUser.Where(u => u.Id == ClientProject.CreatedById).FirstOrDefault();

        //        // ProjectRemarksHistory history = await Task.Run(() => _dbContext.ProjectRemarksHistory.Where(x => x.ProjectId == id).FirstOrDefault());
        //        if (ClientProject != null)
        //        {



        //            history.ApprovalStatusId = input.ApprovalStatusId;
        //            ClientProject.ApprovalStatusId = input.ApprovalStatusId;
        //            ClientProject.Remarks = input.Remarks;


        //            history.Remarks = input.Remarks;
        //            history.RemarksById = input.LastModifiedById;

        //            history.RemarksDate = DateTime.Now;
        //            history.ProjectId = input.Id;

        //            history.IsDeleted = false;

        //            _dbContext.ProjectRemarksHistory.Add(history);

        //            await base.UpdateAsync(ClientProject);
        //            await _unitOfWork.SaveChangesAsync();

        //            transaction.Commit();
        //            // return "Successfully Saved !";
        //            return "1";
        //        }
        //        else
        //        {
        //            // return "Not Submited!";
        //            return "0";
        //        }
        //    }

        //}

        public async Task<string> SA8000ChangeRequest(ProjectSA8000Model input)
        {
            var SecFormData = await Task.Run(() => _dbContext.SecForm.Where(x => x.Name == input.FormName && x.IsClosed == false).FirstOrDefault());

            var newcode = "";
            string Message = "";
            long newid;



          
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var Dbs8000 = new ProjectSA8000Model();
                if (input.Id > 0)
                {
                    try
                    {

                        var s8000Result = await Task.Run(() => _dbContext.ProjectSa8000.Include(x=>x.ClientProject).Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault());
                        Dbs8000 = _mapper.Map<ProjectSA8000Model>(s8000Result);

                        var result = new ProjectSA8000Model();
                        var properties = TypeDescriptor.GetProperties(typeof(ProjectSA8000Model));

                        foreach (PropertyDescriptor property in properties)
                        {
                            var currentValue = property.GetValue(input);
                            var dbdata = property.GetValue(Dbs8000);
                            var name = property.Name.ToString();
                            if (property.Name.ToString() != "CreatedById" && property.Name.ToString() != "OrganizationId" && property.Name.ToString()!= "FormName")
                            {

                                var dbdataValue = (dbdata == null ? "" : dbdata.ToString());
                                 var NewValue = (currentValue == null ? "" : currentValue.ToString());
                                // var dbdataValue = (currentValue == null ? "" : dbdata.ToString());
                                // var test = (!(dbdata is DBNull)) ? Convert.ToInt64(dataReader["Id"]) : 0;
                              
                                if (NewValue != "" && NewValue != null && NewValue != dbdataValue)
                                {
                                    ActivityLog activityLog = new ActivityLog();
                                    property.SetValue(result, currentValue);

                                    activityLog.UserId = input.CreatedById;
                                    activityLog.FormId = SecFormData.Id;
                                    activityLog.TableName = "ProjectSA8000";
                                    activityLog.TableRowId = Dbs8000.Id;
                                    activityLog.OldValues = dbdataValue;
                                    activityLog.NewValues = NewValue;
                                    activityLog.AffectedColumns = property.Name;
                                    activityLog.ApprovalStatusId = 1;
                                    activityLog.CreatedDate = DateTime.Now;
                                    activityLog.OrganizationId = input.OrganizationId;
                                    //activityLog.FilePath = input.FilePath;
                                    //activityLog.ContentType = input.ContentType;


                                    _dbContext.ActivityLog.Add(activityLog);
                                    await _unitOfWork.SaveChangesAsync();



                                    newid = activityLog.Id;



                                    if (input.File != null)
                                    {
                                        string filename = Path.GetFileName(input.File.FileName);
                                        string ContentType = input.File.ContentType;

                                        string reportPath = _configuration["Reporting:Sa8000DocumentsPath"];

                                //    // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                                        string newFileName = reportPath + Dbs8000+"_"+Guid.NewGuid() + "_" + filename;
                                        // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                                        using (var stream = new FileStream(newFileName, FileMode.Create))
                                        {
                                            await input.File.CopyToAsync(stream);

                                        }
                                        activityLog.FilePath = newFileName;
                                        activityLog.ContentType = ContentType;
                                        _dbContext.ActivityLog.Update(activityLog);
                                        await _unitOfWork.SaveChangesAsync();

                                    }



                                }
                            }
                        }

                        //date = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");


                        if (input.ContractForm != null && Dbs8000.ContractFilePath != null && Dbs8000.ContractFileContentType != null)
                        {
                            string filename = Path.GetFileName(input.ContractForm.FileName);
                            string ContentType = input.ContractForm.ContentType;

                            string reportPath = _configuration["Reporting:Sa8000DocumentsPath"];

                            //string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                            string newFileName = reportPath + Dbs8000.Id+ "_"+Guid.NewGuid() + "_" + filename;

                            //string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                            using (var stream = new FileStream(newFileName, FileMode.Create))
                            {
                                await input.ContractForm.CopyToAsync(stream);

                            }
                            string OldfileContractPathwithType = Dbs8000.ContractFilePath;

                            string NewfileContractPathwithType = "FilePath_" + newFileName + "_ContentType_" + ContentType;

                            ActivityLog activityLog = new ActivityLog();


                            activityLog.UserId = input.CreatedById;
                            activityLog.FormId = SecFormData.Id;
                            activityLog.TableName = "ProjectSA8000";
                            activityLog.TableRowId = Dbs8000.Id;
                            activityLog.OldValues = OldfileContractPathwithType;
                            activityLog.NewValues = NewfileContractPathwithType;
                            activityLog.AffectedColumns = "ContractFilePath";
                            activityLog.ApprovalStatusId = 1;
                            activityLog.CreatedDate = DateTime.Now;
                            activityLog.OrganizationId = input.OrganizationId;



                            _dbContext.ActivityLog.Add(activityLog);
                            await _unitOfWork.SaveChangesAsync();


                        }

                        transaction.Commit();
                        Message = "1";

                    }
                    catch (Exception ex)
                    {
                        var Exception = ex;
                        transaction.Rollback();
                        return "Not Inserted!";
                    }


                }


                return Message;


            }




        }


        public async Task<List<ActivityLogModel>> GetProjectChangeDataById(long id, PagedResponseModel model)
        {
            try
            {


                //List<ActivityLog> List = new List<ActivityLog>();
                List<ActivityLogModel> AuditList = new List<ActivityLogModel>();
                List<ActivityLogModel> activityLogsMod = new List<ActivityLogModel>();


               var List = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.TableRowId == id && x.ApprovalStatusId == model.statusId && x.TableName == model.Keyword).OrderByDescending(x => x.Id).ToList());
                activityLogsMod = _mapper.Map<List<ActivityLogModel>>(List);

                ProjectSA8000Model saMod = new ProjectSA8000Model();
                ProjectSLCPModel SlcpMod = new ProjectSLCPModel();
                ProjectHiggModel higgMod = new ProjectHiggModel();
                foreach (var activityLog in activityLogsMod)
                {
                    ActivityLogModel Acmodel = new ActivityLogModel();
                 
                    //var client = await _dbContext.Client.Where(x => x.Id == activityLog.TableRowId).FirstOrDefaultAsync();

                    string ClientName = "";
                    if (activityLog.TableName == "ProjectSA8000")
                    {
                        var Prosa = await _dbContext.ProjectSa8000.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == activityLog.TableRowId).FirstOrDefaultAsync();
                        saMod = _mapper.Map<ProjectSA8000Model>(Prosa);
                        ClientName = saMod.ClientName;
                    }
                    if (activityLog.TableName == "ProjectSLCP")
                    {
                        var Proslcp = await _dbContext.ProjectSlcp.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == activityLog.TableRowId).FirstOrDefaultAsync();
                        SlcpMod = _mapper.Map<ProjectSLCPModel>(Proslcp);
                        ClientName = SlcpMod.ClientName;
                    }
                    if (activityLog.TableName == "ProjectHigg")
                    {
                        var Prohigg = await _dbContext.ProjectHigg.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == activityLog.TableRowId).FirstOrDefaultAsync();
                        higgMod = _mapper.Map<ProjectHiggModel>(Prohigg);
                        ClientName = higgMod.ClientName;
                    }
                    Acmodel.ClientName = ClientName;
                   
                    Acmodel.AffectedColumns = activityLog.AffectedColumns;
                    Acmodel.OldValues = activityLog.OldValues;
                    Acmodel.NewValues = activityLog.NewValues;
                    Acmodel.TableName = activityLog.TableName;
                    Acmodel.CreatedDate = activityLog.CreatedDate;
                    Acmodel.FormId = activityLog.FormId;

                    Acmodel.Id = activityLog.Id;
                   Acmodel.FilePath = activityLog.FilePath;

                    //----------------------------- SA8000 ---------------------------------------
                    if (activityLog.TableName == "ProjectSA8000")
                    {
                        if (Acmodel.AffectedColumns == "SurveillanceMethodId")
                        {
                            var SurveillanceMethodold = await _dbContext.SurveillanceMethod.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var SurveillanceMethodnew = await _dbContext.SurveillanceMethod.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = SurveillanceMethodold.Name;
                            Acmodel.NewValues = SurveillanceMethodnew.Name;

                        }

                        if (Acmodel.AffectedColumns == "SurveillanceVisitFrequencyId")
                        {
                            var SurveillanceVisitFrequencyold = await _dbContext.SurveillanceVisitFrequency.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var SurveillanceVisitFrequencynew = await _dbContext.SurveillanceVisitFrequency.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = SurveillanceVisitFrequencyold.Name;
                            Acmodel.NewValues = SurveillanceVisitFrequencynew.Name;

                        }
                    }

                    //----------------------------- SLCP ---------------------------------------
                    if (activityLog.TableName == "ProjectSLCP")
                    {
                        if (Acmodel.AffectedColumns == "AssessmentCompletedId")
                        {
                            var AssessmentCompletedOld = await _dbContext.AssessmentCompleted.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var SAssessmentCompletedNew = await _dbContext.AssessmentCompleted.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = AssessmentCompletedOld.Name;
                            Acmodel.NewValues = SAssessmentCompletedNew.Name;

                        }

                        if (Acmodel.AffectedColumns == "AccreditationId")
                        {
                            var AccreditationOld = await _dbContext.Accreditation.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var AccreditationNew = await _dbContext.Accreditation.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = AccreditationOld.Name;
                            Acmodel.NewValues = AccreditationNew.Name;

                        }
                        if (Acmodel.AffectedColumns == "CompletedStepId")
                        {
                            var CompletedStepold = await _dbContext.CompletedSteps.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var CompletedStepnew = await _dbContext.CompletedSteps.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = CompletedStepold.Name;
                            Acmodel.NewValues = CompletedStepnew.Name;

                        }
                        if (Acmodel.AffectedColumns == "RequestOfSiteId")
                        {
                            var RequestOfSiteold = await _dbContext.RequestOfSite.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var RequestOfSitenew = await _dbContext.RequestOfSite.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = RequestOfSiteold.Name;
                            Acmodel.NewValues = RequestOfSitenew.Name;

                        }
                        if (Acmodel.AffectedColumns == "ModuleVersionId")
                        {
                            var ModuleVersionold = await _dbContext.ModuleVersion.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var ModuleVersionnew = await _dbContext.ModuleVersion.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = ModuleVersionold.Name;
                            Acmodel.NewValues = ModuleVersionnew.Name;

                        }
                        if (Acmodel.AffectedColumns == "ModuleShareId")
                        {
                            var ModuleShareold = await _dbContext.ModuleShare.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var ModuleSharenew = await _dbContext.ModuleShare.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = ModuleShareold.Name;
                            Acmodel.NewValues = ModuleSharenew.Name;

                        }
                        if (Acmodel.AffectedColumns == "ServicesTypeId")
                        {
                            var ServicesTypeold = await _dbContext.ServiceType.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var ServicesTypenew = await _dbContext.ServiceType.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = ServicesTypeold.Name;
                            Acmodel.NewValues = ServicesTypenew.Name;

                        }
                        if (Acmodel.AffectedColumns == "MethodologyId")
                        {
                            var Methodologyold = await _dbContext.Methodology.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var Methodologynew = await _dbContext.Methodology.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = Methodologyold.Name;
                            Acmodel.NewValues = Methodologynew.Name;

                        }
                    }

                    //----------------------------- HIGG ---------------------------------------
                    if (activityLog.TableName == "ProjectHigg")
                    {
                        if (Acmodel.AffectedColumns == "AssessmentCompletedId")
                        {
                            var AssessmentCompletedOld = await _dbContext.AssessmentCompleted.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var AssessmentCompletedNew = await _dbContext.AssessmentCompleted.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = AssessmentCompletedOld.Name;
                            Acmodel.NewValues = AssessmentCompletedNew.Name;

                        }

                        if (Acmodel.AffectedColumns == "CompletedModuleId")
                        {
                            var CompletedModuleOld = await _dbContext.CompletedModule.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var CompletedModuleNew = await _dbContext.CompletedModule.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = CompletedModuleOld.Name;
                            Acmodel.NewValues = CompletedModuleNew.Name;

                        }
                        if (Acmodel.AffectedColumns == "EffluentTreatmentPlantId")
                        {
                            var EffluentTreatmentPlantold = await _dbContext.EffluentTreatmentPlant.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var EffluentTreatmentPlantnew = await _dbContext.EffluentTreatmentPlant.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = EffluentTreatmentPlantold.Name;
                            Acmodel.NewValues = EffluentTreatmentPlantnew.Name;

                        }

                        if (Acmodel.AffectedColumns == "ServicesTypeId")
                        {
                            var ServicesTypeold = await _dbContext.ServiceType.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var ServicesTypenew = await _dbContext.ServiceType.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = ServicesTypeold.Name;
                            Acmodel.NewValues = ServicesTypenew.Name;

                        }
                        if (Acmodel.AffectedColumns == "MethodologyId")
                        {
                            var Methodologyold = await _dbContext.Methodology.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var Methodologynew = await _dbContext.Methodology.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = Methodologyold.Name;
                            Acmodel.NewValues = Methodologynew.Name;

                        }
                        if (Acmodel.AffectedColumns == "RequestOfSiteId")
                        {
                            var RequestOfSiteold = await _dbContext.RequestOfSite.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var RequestOfSitenew = await _dbContext.RequestOfSite.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = RequestOfSiteold.Name;
                            Acmodel.NewValues = RequestOfSitenew.Name;

                        }
                        if (Acmodel.AffectedColumns == "ModuleVersionId")
                        {
                            var ModuleVersionold = await _dbContext.ModuleVersion.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var ModuleVersionnew = await _dbContext.ModuleVersion.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = ModuleVersionold.Name;
                            Acmodel.NewValues = ModuleVersionnew.Name;

                        }
                        if (Acmodel.AffectedColumns == "ModuleShareId")
                        {
                            var ModuleShareold = await _dbContext.ModuleShare.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                            var ModuleSharenew = await _dbContext.ModuleShare.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                            Acmodel.OldValues = ModuleShareold.Name;
                            Acmodel.NewValues = ModuleSharenew.Name;

                        }
                    }

                    AuditList.Add(Acmodel);

                    //Acmodel.
                }



                return AuditList;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<string> ApprovedProjectChange(ActivityLogModel input)
        {
          

            string Message = "";



            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var DbClientChangeRemarks = new ActivityLogModel();
                if (input.Id > 0)
                {
                    try
                    {

                        ActivityLog activityLog = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.Id == input.Id).FirstOrDefault());


                        if (activityLog != null)
                        {

                            activityLog.ApprovalStatusId = input.ApprovalStatusId;
                            //activityLog.Remarks = input.Remarks;
                            activityLog.LastModifiedBy = input.CreatedBy;
                            activityLog.LastModifiedDate = DateTime.Now;

                            if (input.NewValues != null && input.NewValues != string.Empty)
                            {

                                activityLog.NewValues = input.NewValues;

                            }

                            _dbContext.ActivityLog.Update(activityLog);


                            ActivityLogRemarksHistory activitylogRemarksHistory = new ActivityLogRemarksHistory();
                            activitylogRemarksHistory.ActivityLogId = input.Id;
                            activitylogRemarksHistory.Remarks = input.Remarks;
                            activitylogRemarksHistory.ApprovalStatusId = input.ApprovalStatusId;
                            activitylogRemarksHistory.RemarksById = input.CreatedBy;
                            activitylogRemarksHistory.RemarksDate = DateTime.Now;

                            _dbContext.ActivityLogRemarksHistory.Add(activitylogRemarksHistory);
                            //if (input.ApprovalStatusId) { }

     
                            if (input.ApprovalStatusId == 2)
                            {
                                if (activityLog.FormId == 22) { 

                                string columnName = activityLog.AffectedColumns;


                                    if (columnName == "ContractFilePath") {

                                   
                                       var s8000Result = await Task.Run(() => _dbContext.ProjectSa8000.Include(x => x.ClientProject).Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());
                              


                                        ClientProjects clientpro= await Task.Run(() => _dbContext.ClientProjects.Where(x => x.Id == s8000Result.ClientProjectId).FirstOrDefault());

                                        string fullfilepath = activityLog.NewValues;
                                        var a = fullfilepath.Split("_ContentType_");
                                        var filepath = a[0].Split("FilePath_");


                                        clientpro.ContractFilePath = filepath[1];
                                        clientpro.ContractFileContent = a[1];


                                        _dbContext.ClientProjects.Update(clientpro);
                                    }
                                    else { 

                                    ProjectSa8000 projectSa8000 = await Task.Run(() => _dbContext.ProjectSa8000.Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());


                                if (columnName == "Scope")
                                {

                                    projectSa8000.Scope = activityLog.NewValues;
                                }

                                if (columnName == "DurationStage2")
                                {

                                    projectSa8000.DurationStage2 = activityLog.NewValues;
                                }

                                if (columnName == "SurveillanceVisitFrequencyId")
                                {

                                    projectSa8000.SurveillanceVisitFrequencyId = Convert.ToInt64(activityLog.NewValues);
                                }
                                if (columnName == "SurveillanceMethodId")
                                {

                                    projectSa8000.SurveillanceMethodId = Convert.ToInt64(activityLog.NewValues);
                                }
                                if (columnName == "NoOfSurveillanceVisits")
                                {

                                    projectSa8000.NoOfSurveillanceVisits = activityLog.NewValues;
                                }

                                if (columnName == "Assessmentfee")
                                {

                                    projectSa8000.Assessmentfee =Convert.ToDouble(activityLog.NewValues);
                                }

                                if (columnName == "Survfee")
                                {

                                    projectSa8000.Survfee = Convert.ToDouble(activityLog.NewValues);
                                }
                                if (columnName == "DurationSurvVisit")
                                {

                                    projectSa8000.DurationSurvVisit = Convert.ToDecimal(activityLog.NewValues);
                                }

                                        _dbContext.ProjectSa8000.Update(projectSa8000);
                                }
                                }


                                if (activityLog.FormId == 10037)
                                {
                                    string columnName = activityLog.AffectedColumns;
                                    
                                    if (columnName == "ContractFilePath")
                                    {


                                        var slcpResult = await Task.Run(() => _dbContext.ProjectSlcp.Include(x => x.ClientProject).Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());



                                        ClientProjects clientpro = await Task.Run(() => _dbContext.ClientProjects.Where(x => x.Id == slcpResult.ClientProjectId).FirstOrDefault());

                                        string fullfilepath = activityLog.NewValues;
                                        var a = fullfilepath.Split("_ContentType_");
                                        var filepath = a[0].Split("FilePath_");


                                        clientpro.ContractFilePath = filepath[1];
                                        clientpro.ContractFileContent = a[1];


                                        _dbContext.ClientProjects.Update(clientpro);
                                    }
                                    else
                                    {

                            

                                    ProjectSlcp projectSlcp = await Task.Run(() => _dbContext.ProjectSlcp.Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());



                                    if (columnName == "AssessmentCompletedId")
                                    {

                                        projectSlcp.AssessmentCompletedId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "AccreditationId")
                                    {

                                        projectSlcp.AccreditationId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "CompletedStepId")
                                    {

                                        projectSlcp.CompletedStepId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "NoOfMd")
                                    {

                                        projectSlcp.NoOfMd = Convert.ToDecimal(activityLog.NewValues);
                                    }
                                    if (columnName == "RequestOfSiteId")
                                    {

                                        projectSlcp.RequestOfSiteId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "ModuleVersionId")
                                    {

                                        projectSlcp.ModuleVersionId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "BodySelectDate")
                                    {

                                        projectSlcp.BodySelectDate = Convert.ToDateTime(activityLog.NewValues);
                                    }
                                    if (columnName == "ModuleShareId")
                                    {

                                        projectSlcp.ModuleShareId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "ModuleYear")
                                    {

                                        projectSlcp.ModuleYear = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "TentativeDate")
                                    {

                                        projectSlcp.TentativeDate = Convert.ToDateTime(activityLog.NewValues);
                                    }
                                    if (columnName == "ServicesTypeId")
                                    {

                                        projectSlcp.ServicesTypeId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "MethodologyId")
                                    {

                                        projectSlcp.MethodologyId = Convert.ToInt64(activityLog.NewValues);
                                    }
                                    if (columnName == "FromDate")
                                    {

                                        projectSlcp.FromDate = Convert.ToDateTime(activityLog.NewValues);
                                    }
                                    if (columnName == "ToDate")
                                    {

                                        projectSlcp.ToDate = Convert.ToDateTime(activityLog.NewValues);
                                    }
                                    if (columnName == "ApplicationDate")
                                    {

                                        projectSlcp.ApplicationDate = Convert.ToDateTime(activityLog.NewValues);
                                    }

                                        _dbContext.ProjectSlcp.Update(projectSlcp);

                                    }
                                }




                                if (activityLog.FormId == 37)
                                {
                                    string columnName = activityLog.AffectedColumns;
                                    if (columnName == "ContractFilePath")
                                    {


                                        var higgResult = await Task.Run(() => _dbContext.ProjectHigg.Include(x => x.ClientProject).Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());



                                        ClientProjects clientpro = await Task.Run(() => _dbContext.ClientProjects.Where(x => x.Id == higgResult.ClientProjectId).FirstOrDefault());

                                        string fullfilepath = activityLog.NewValues;
                                        var a = fullfilepath.Split("_ContentType_");
                                        var filepath = a[0].Split("FilePath_");


                                        clientpro.ContractFilePath = filepath[1];
                                        clientpro.ContractFileContent = a[1];


                                        _dbContext.ClientProjects.Update(clientpro);
                                    }
                                    else
                                    {

                                        ProjectHigg projectHigg = await Task.Run(() => _dbContext.ProjectHigg.Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());

                                        if (columnName == "AssessmentCompletedId")
                                        {

                                            projectHigg.AssessmentCompletedId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "CompletedModuleId")
                                        {

                                            projectHigg.CompletedModuleId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "ManufacturingProcessess")
                                        {

                                            projectHigg.ManufacturingProcessess = activityLog.NewValues;
                                        }
                                        if (columnName == "EffluentTreatmentPlantId")
                                        {

                                            projectHigg.EffluentTreatmentPlantId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "ServicesTypeId")
                                        {

                                            projectHigg.ServicesTypeId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "MethodologyId")
                                        {

                                            projectHigg.MethodologyId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "FromDate")
                                        {

                                            projectHigg.FromDate = Convert.ToDateTime(activityLog.NewValues);
                                        }
                                        if (columnName == "ToDate")
                                        {

                                            projectHigg.ToDate = Convert.ToDateTime(activityLog.NewValues);
                                        }
                                        if (columnName == "ApplicationDate")
                                        {

                                            projectHigg.ApplicationDate = Convert.ToDateTime(activityLog.NewValues);
                                        }
                                        if (columnName == "NoOfMd")
                                        {

                                            projectHigg.NoOfMd = Convert.ToDecimal(activityLog.NewValues);
                                        }
                                        if (columnName == "RequestOfSiteId")
                                        {

                                            projectHigg.RequestOfSiteId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "ModuleVersionId")
                                        {

                                            projectHigg.ModuleVersionId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "BodySelectDate")
                                        {

                                            projectHigg.BodySelectDate = Convert.ToDateTime(activityLog.NewValues);
                                        }
                                        if (columnName == "ModuleShareId")
                                        {

                                            projectHigg.ModuleShareId = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        if (columnName == "ModuleYear")
                                        {

                                            projectHigg.ModuleYear = Convert.ToInt64(activityLog.NewValues);
                                        }
                                        _dbContext.ProjectHigg.Update(projectHigg);

                                    }
                                }

                            }

                            await _unitOfWork.SaveChangesAsync();




                            transaction.Commit();

                            Message = "Successfully Submited..";

                        }
                        else
                        {
                            Message = "Not Inserted.!";
                        }




                    }
                    catch (Exception ex)
                    {
                        var Exception = ex;
                        transaction.Rollback();
                        Message = "Error";

                    }


                }


                return Message;


            }
        }


        public async Task<List<ActivityLogRemarksHistoryModel>> GetProjectSA8000RemarksDataById(long id)
        {
            try
            {
                //var result = new ActivityLogRemarksHistoryModel();
                List<ActivityLogRemarksHistory> List = new List<ActivityLogRemarksHistory>();
                List<ActivityLogRemarksHistoryModel> AuditList = new List<ActivityLogRemarksHistoryModel>();

                List = await _dbContext.ActivityLogRemarksHistory.Include(x => x.ApprovalStatus).Include(x => x.RemarksBy).Where(x => x.ActivityLogId == id).OrderByDescending(x => x.Id).ToListAsync();


                AuditList = _mapper.Map<List<ActivityLogRemarksHistoryModel>>(List);


                return AuditList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<GetPagedProjectsModel> GetAllProjectChangeRequest(long id, PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedProjectsModel();
                List<ActivityLog> List = new List<ActivityLog>();
                List<ActivityLogModel> AuditList = new List<ActivityLogModel>();
                List<string> projects = new List<string> { "ProjectSA8000", "ProjectSLCP", "ProjectHigg" };



                if (model.organizationId > 1)
                {
                    List = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && (projects.Contains(x.TableName) && x.OrganizationId == model.organizationId)).OrderByDescending(x => x.Id).ToListAsync();
                    var query = List


                           .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId, x.OrganizationId ,x.TableName})
                           .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();


                    foreach (var activityLog in query)
                    {
                        ActivityLogModel Acmodel = new ActivityLogModel();

                        long tbrowId = activityLog.Peo.TableRowId;
                        var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                        //var client = await _dbContext.Client.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                        ProjectSa8000 Projectsa = null;

                        ProjectSA8000Model saMod = new ProjectSA8000Model();
                        ProjectSLCPModel SlcpMod = new ProjectSLCPModel();
                        ProjectHiggModel higgMod = new ProjectHiggModel();
                        string ClientName = "";
                        if (activityLog.Peo.TableName == "ProjectSA8000")
                        {
                            var Prosa = await _dbContext.ProjectSa8000.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == tbrowId).FirstOrDefaultAsync();
                            saMod = _mapper.Map<ProjectSA8000Model>(Prosa);
                            ClientName = saMod.ClientName;
                        }
                        if (activityLog.Peo.TableName == "ProjectSLCP")
                        {
                            var Proslcp = await _dbContext.ProjectSlcp.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == tbrowId).FirstOrDefaultAsync();
                            SlcpMod = _mapper.Map<ProjectSLCPModel>(Proslcp);
                            ClientName = SlcpMod.ClientName;
                        }
                        if (activityLog.Peo.TableName == "ProjectHigg")
                        {
                            var Prohigg = await _dbContext.ProjectHigg.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == tbrowId).FirstOrDefaultAsync();
                            higgMod = _mapper.Map<ProjectHiggModel>(Prohigg);
                            ClientName = higgMod.ClientName;
                        }

                        //var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                        //var client = await _dbContext.Client.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                        //var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                        var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();


                        Acmodel.ClientName = ClientName;
                        Acmodel.OrganizationName = organization.Name;
                        Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.ApprovalStatusId = activityLog.Peo.ApprovalStatusId;
                        Acmodel.TableName = activityLog.Peo.TableName;
                      
                        Acmodel.Id = activityLog.Peo.TableRowId;
                        AuditList.Add(Acmodel);

                        //Acmodel.
                    }
                    result.ActivityLogModel = AuditList;

                }
                else
                {

                    List = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && (projects.Contains(x.TableName))).OrderByDescending(x => x.Id).ToListAsync();
                    var query = List


                           .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId, x.OrganizationId, x.TableName })
                           .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();


                    foreach (var activityLog in query)
                    {
                        ActivityLogModel Acmodel = new ActivityLogModel();

                        long tbrowId = activityLog.Peo.TableRowId;


                        //var activityClient = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "Client").FirstOrDefaultAsync();
                        var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                        //var client = await _dbContext.Client.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                        ProjectSa8000 Projectsa = null;
                        ProjectSA8000Model saMod = new ProjectSA8000Model();
                        ProjectSLCPModel SlcpMod = new ProjectSLCPModel();
                        ProjectHiggModel higgMod = new ProjectHiggModel();

                        string ClientName = "";

                        if (activityLog.Peo.TableName == "ProjectSA8000")
                        {
                            var Prosa = await _dbContext.ProjectSa8000.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == tbrowId).FirstOrDefaultAsync();
                            saMod = _mapper.Map<ProjectSA8000Model>(Prosa);
                            ClientName = saMod.ClientName;
                        }
                        if (activityLog.Peo.TableName == "ProjectSLCP")
                        {
                            var Proslcp = await _dbContext.ProjectSlcp.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == tbrowId).FirstOrDefaultAsync();
                            SlcpMod = _mapper.Map<ProjectSLCPModel>(Proslcp);
                            ClientName = SlcpMod.ClientName;
                        }
                        if (activityLog.Peo.TableName == "ProjectHigg")
                        {
                            var Prohigg = await _dbContext.ProjectHigg.Include(x => x.ClientProject).Include(x => x.ClientProject.Client).Where(x => x.Id == tbrowId).FirstOrDefaultAsync();
                            higgMod = _mapper.Map<ProjectHiggModel>(Prohigg);
                            ClientName = higgMod.ClientName;
                        }

                        var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                        Acmodel.ClientName = ClientName;
                        Acmodel.OrganizationName = organization.Name;
                        Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.Id = activityLog.Peo.TableRowId;
                        Acmodel.ApprovalStatusId = activityLog.Peo.ApprovalStatusId;
                        Acmodel.TableName = activityLog.Peo.TableName;
                        AuditList.Add(Acmodel);

                        //Acmodel.
                    }
                    result.ActivityLogModel = AuditList;

                }
              

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        //public DbSet Set(string name)
        //{
        //    // you may need to fill in the namespace of your context
        //    return base.Set(Type.GetType(name));
        //}
        public string GetProgectCode(long clientSiteId, long standardId ,long? ClientId)
        {

            //int? cmpid = 0;
            string Codelast = string.Empty;
            int newNO = 0;
            string ItemCode = string.Empty;
            string re = "-";
           // string code = "CAT-";

            var AgencyCode = "";
            var ClientCode = "";
            var clientSiteCode = "";
            var StandardCode = "";
            DateTime Date = System.DateTime.Now;
            string date = Date.ToString("yy");
            var standard = new Certification();
            var ClientProjectMod = new ClientProjectModel();
            var ClientSitesModel = new ClientSitesModel();
            var clientMod = new ClientModel();
            if (clientSiteId > 0)
            {
                var ClientProject = _dbContext.ClientProjects.Include(x => x.ClientSite).Include(x => x.Standard).Include(x => x.ClientSite.Client).Include(x => x.ClientSite.Client.Organization).Where(x => x.StandardId == standardId && x.ClientSiteId == clientSiteId && x.IsDeleted==false && x.ClientId==ClientId).OrderByDescending(a => a.Id).FirstOrDefault();
                ClientProjectMod = _mapper.Map<ClientProjectModel>(ClientProject);
            }
            else
            {
               
                

                var ClientProject = _dbContext.ClientProjects.Where(x => x.StandardId == standardId && x.ClientId==ClientId).OrderByDescending(a => a.Id).FirstOrDefault();
                ClientProjectMod = _mapper.Map<ClientProjectModel>(ClientProject);
            }
            standard = _dbContext.Certification.Where(x => x.Id == standardId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            var client = _dbContext.Client.Include(x => x.Organization).Where(x => x.Id == ClientId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            clientMod = _mapper.Map<ClientModel>(client);
            if (ClientProjectMod != null) 
            
            {
                newproject = false;


                if (clientSiteId > 0)
                {


                    clientSiteCode = ClientProjectMod.ClientSiteCode.Substring(0, 3);

                    AgencyCode = ClientProjectMod.AgencyCode.Substring(0, 3);

                    ClientCode = ClientProjectMod.ClientCode.Substring(4, 3);

                    StandardCode = ClientProjectMod.StandardCode;



                }

                else 
                {
                    clientSiteCode = "000";
                    AgencyCode = clientMod.OrganizationCode.Substring(0, 3);
                    ClientCode = clientMod.Code.Substring(4, 3);
                    StandardCode = standard.Code;
                }
               
               
                newNO = Convert.ToInt32(ClientProjectMod.ProjectCode.Substring(19,3));
                // newNO++;

                newNO = newNO + 1;
              
                var newcode = newNO.ToString().PadLeft(3, '0');
                ItemCode = AgencyCode + re + ClientCode + re + StandardCode + re + clientSiteCode + re + date + re + newcode;
            }
            else
            {
                newproject = true;
                var ClientSite = _dbContext.ClientSites.Include(x => x.Client).Include(x => x.Client.Organization).Where(x => x.Id == clientSiteId && x.IsDeleted == false).OrderByDescending(a => a.Id).FirstOrDefault();
                ClientSitesModel = _mapper.Map<ClientSitesModel>(ClientSite);
                //clientSiteCode = ClientSitesModel.Code.Substring(0, 3);
                //AgencyCode = ClientSitesModel.AgencyCode.Substring(0, 3);
                //ClientCode= ClientSitesModel.ClientCode.Substring(4, 3);

                AgencyCode = clientMod.OrganizationCode.Substring(0, 3);
                ClientCode = clientMod.Code.Substring(4, 3);
                StandardCode = standard.Code;
                if (clientSiteId > 0)
                {


                    clientSiteCode = ClientSitesModel.Code.Substring(0, 3);

                  



                }

                else
                {
                    clientSiteCode = "000";
                   
                }
                StandardCode = _dbContext.Certification.Where(x => x.Id == standardId && x.IsDeleted == false).OrderByDescending(a => a.Id).Select(x=>x.Code).FirstOrDefault();
              
                ItemCode = AgencyCode + re + ClientCode + re + StandardCode + re + clientSiteCode + re + date + re  + "001";
            }
         

            return ItemCode;


        }

        public async Task<GetPagedClientProjectModel> GetProjectSA8000BYId(long id)
        {
           
            var result = new GetPagedClientProjectModel();
            var ProjectSAModel = new ProjectSA8000Model();
            var ClientProjectMod = new ClientProjectModel();
            var ClientSitesModel = new ClientSitesModel();
            var Dbresult = await Task.Run(() => _dbContext.ProjectSa8000.Where(x => x.ClientProjectId == id).FirstOrDefault());
            ProjectSAModel = _mapper.Map<ProjectSA8000Model>(Dbresult);
            var Dbresult2 = await Task.Run(() => _dbContext.ClientProjects.Include(x=>x.ClientSite).Include(x=>x.Client).Include(X => X.Client.Eacode).Include(x => x.Client.NaceCode).Include(x => x.Client.Risk).Include(x=>x.ApprovalStatus).Include(x=>x.ProjectType).Where(x => x.Id == id).FirstOrDefault());
            ClientProjectMod = _mapper.Map<ClientProjectModel>(Dbresult2);
            var Dbresult3 = await Task.Run(() => _dbContext.ClientSites.Where(x => x.Id == Dbresult2.ClientSiteId).FirstOrDefault());
            ClientSitesModel = _mapper.Map<ClientSitesModel>(Dbresult3);
            //var Dbresult3 = await Task.Run(() => _dbContext.ClientSites.Where(x => x.Id == Dbresult2.ClientSiteId).FirstOrDefault());
            //ClientSitesModel = _mapper.Map<ClientSitesModel>(Dbresult3);


            result.ProjectSA8000Model = ProjectSAModel;
            result.ClientProjectModel = ClientProjectMod;
            result.ClientSitesModel = ClientSitesModel;

            return result;
        }

        private List<ProjectSA8000Model> GetPage(List<ProjectSA8000Model> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedProjectSA8000Model> GetPagedProjectSA8000(PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedProjectSA8000Model();
                var UserStdList = new List<ProjectSA8000Model>();

                //if (model.AuthAllowed == true)
                //{
                //var list = await _dbContext.Certification.Where(x => x.IsDeleted == false && x.IsActive == true &&
                //              (x.Module.Name.ToLower().Contains(model.Keyword.ToLower()) ||
                //             x.ModuleId.ToString().ToLower().Contains(model.Keyword.ToLower()))).OrderByDescending(x => x.Id).ToListAsync();

                var list = await _dbContext.ProjectSa8000.Where(x => x.IsDeleted == false).ToListAsync();
                UserStdList = _mapper.Map<List<ProjectSA8000Model>>(list);
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

                result.ProjectSA8000Model = GetPage(UserStdList, model.Page, model.PageSize);
                result.TotalCount = UserStdList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> ProjectSA8000DeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {

                ClientProjects Project= _dbContext.ClientProjects.Where(u => u.Id == id).FirstOrDefault();
                ProjectSa8000 dbresult = _dbContext.ProjectSa8000.Where(u => u.ClientProjectId == id).FirstOrDefault();


                if (dbresult != null)
                {
                    // SecUser user = _secuserRepository.GetUserByUserName(input.UserName);

                    try
                    {


                        dbresult.IsDeleted = true;
                        Project.IsDeleted = true;


                        Project.ProjectSa8000.Add(dbresult);
                        await base.UpdateAsync(Project);
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
                    return "User Academic not Exists!";
                }


            }
            // return "User Already Exists!";
        }
        public async Task<ProjectSA8000Model> DownloadFile(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ProjectSa8000 DbLibrary = await Task.Run(() => _dbContext.ProjectSa8000.Where(x => x.ClientProjectId == id).FirstOrDefault());

                ProjectSA8000Model Li = new ProjectSA8000Model();
                if (DbLibrary != null)
                {
                    Li.ApplicationFormPath = DbLibrary.ApplicationFormPath;
                    Li.ApplicationContentType = DbLibrary.ApplicationContentType;

                    //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();
                }
                return Li;
            }

        }
        public async Task<string> Approval(ProjectRemarksHistoryModel input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {

                ProjectSa8000 ProjectSA = _dbContext.ProjectSa8000.Where(u => u.ClientProjectId == input.Id).FirstOrDefault();
                ProjectRemarksHistory RemarksHistory = _dbContext.ProjectRemarksHistory.Where(u => u.Id == input.Id).FirstOrDefault();
                ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == input.Id).FirstOrDefault();
                ProjectRemarksHistory UserRemarksDb = new ProjectRemarksHistory();
                if (ClientProject != null && input.ApprovalStatusId > 0 && input.Remarks != null)
                {
                    try
                    {
                    UserRemarksDb.ApprovalStatusId = input.ApprovalStatusId;
                    UserRemarksDb.Remarks = input.Remarks;
                    UserRemarksDb.RemarksById = input.RemarksById;
                    UserRemarksDb.RemarksDate = DateTime.Now;
                    UserRemarksDb.ProjectId = input.ProjectId;
                    UserRemarksDb.IsDeleted = false;

                        ClientProject.ApprovalStatusId = input.ApprovalStatusId;
                        ClientProject.Remarks = input.Remarks;


                        ProjectSA.Remarks = input.Remarks;
                        //ProjectSA.ApprovalStatusId = input.ApprovalStatusId;

                        ClientProject.ProjectRemarksHistory.Add(UserRemarksDb);
                        ClientProject.ProjectSa8000.Add(ProjectSA);

                        //secuserEntity.AuthorizedBy=input.
                   
;

               
                    await base.UpdateAsync(ClientProject);
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
                else
                {
                    return "Status And Remarks con not be Empty";
                }
            }

        }
        public async Task<string> SubmitForReview(long id,long loginUserId)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ProjectRemarksHistory history = new ProjectRemarksHistory();
                ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == id).FirstOrDefault();
                ProjectSa8000 Sa = _dbContext.ProjectSa8000.Where(u => u.ClientProjectId == id).FirstOrDefault();
               // ProjectRemarksHistory history = await Task.Run(() => _dbContext.ProjectRemarksHistory.Where(x => x.ProjectId == id).FirstOrDefault());
                if (ClientProject != null)
                {

                 
                        if (ClientProject.ApprovalStatusId != 6)
                    {
                        //Sa.ApprovalStatusId = 2;

                        history.ApprovalStatusId = 2;
                        ClientProject.ApprovalStatusId = 2;
                    }
                    else
                    {
                        //Sa.ApprovalStatusId = 4;

                        history.ApprovalStatusId = 4;
                        ClientProject.ApprovalStatusId = 4;
                    }
                    history.Remarks = ClientProject.Remarks;
                    history.RemarksById = ClientProject.LastModifiedById;
                    history.RemarksDate = DateTime.Now;
                    history.ProjectId =id;

                    history.IsDeleted = false;
                  
                    ClientProject.ProjectRemarksHistory.Add(history);
                    //ClientProject.ProjectSa8000.Add(Sa);
                    await base.UpdateAsync(ClientProject);
                    await _unitOfWork.SaveChangesAsync();

                    transaction.Commit();
                    return "Successfully Record Send For Review !";
                }
                else
                {
                    return "Reques Not Send For Approval";
                }
            }

        }

        public async Task<ClientProjectModel> downloadContract(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ClientProjects DbLibrary = await Task.Run(() => _dbContext.ClientProjects.Where(x => x.Id == id).FirstOrDefault());

                ClientProjectModel Li = new ClientProjectModel();
                if (DbLibrary != null)
                {
                    Li.ContractFileContent = DbLibrary.ContractFileContent;
                    Li.ContractFilePath = DbLibrary.ContractFilePath;

                    //string password = _secPolicyRepo.GetPasswordComplexityRegexPolicy().ToString();
                }
                return Li;
            }

        }




        public async Task<string> ContractSubmit(ClientProjectModel input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ProjectRemarksHistory history = new ProjectRemarksHistory();
                ClientProjects ClientProject = _dbContext.ClientProjects.Where(u => u.Id == input.Id).FirstOrDefault();

                // ProjectRemarksHistory history = await Task.Run(() => _dbContext.ProjectRemarksHistory.Where(x => x.ProjectId == id).FirstOrDefault());
                if (ClientProject != null)
                {
                    if (ClientProject.ApprovalStatusId != 8)
                    {
                        ClientProject.ApprovalStatusId = 9;

                        history.ApprovalStatusId = 9;

                    }
                    else
                    {


                        history.ApprovalStatusId = 10;
                        ClientProject.ApprovalStatusId = 10;
                    }

                    if (input.ContractForm != null)
                    {
                        string filename = Path.GetFileName(input.ContractForm.FileName);
                        string ContentType = input.ContractForm.ContentType;

                        string reportPath = _configuration["Reporting:ProjectsSA8000Path"];

                        // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                        string newFileName = reportPath + ClientProject.Id+"_"+ Guid.NewGuid() + "_" + filename;
                        // string newFileName = @"G:\OzoneDocuments\LibraryDocument\" + +newid + "_" + filename;
                        using (var stream = new FileStream(newFileName, FileMode.Create))
                        {
                            await input.ContractForm.CopyToAsync(stream);

                        }

                        ClientProject.ContractFilePath = newFileName;
                        ClientProject.ContractFileContent = ContentType;


                    }
                    ClientProject.Remarks = input.Remarks;
                    history.Remarks = input.Remarks;
                    history.RemarksById = input.LastModifiedById;
                    history.RemarksDate = DateTime.Now;
                    history.ProjectId = input.Id;

                    history.IsDeleted = false;

                    ClientProject.ProjectRemarksHistory.Add(history);

                    await base.UpdateAsync(ClientProject);
                    await _unitOfWork.SaveChangesAsync();

                    transaction.Commit();
                    return "Successfully Contract Send For Review !";
                }
                else
                {
                    return "Not Submited!";
                }
            }

        }


    }
}
