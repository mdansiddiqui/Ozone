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



namespace Ozone.Infrastructure.Shared.Services
{
   public class AllDropdownService: GenericRepositoryAsync<ProjectSa8000>, IAllDropdownService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
    
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;

        public AllDropdownService(
             IUnitOfWork unitOfWork,
         OzoneContext dbContext,
        //IDataShapeHelper<Library> dataShaper,
        IMapper mapper,
       // IUserSessionHelper userSession,
         IConfiguration configuration) : base(dbContext)
        {
            this._unitOfWork = unitOfWork;
            _dbContext = dbContext;
            //  _user = dbContext.Set<Library>();
            //_dataShaper = dataShaper;
            this._mapper = mapper;
          
            //_mockData = mockData;
            this._configuration = configuration;
        }
        public async Task<List<AuditTypeModel>> GetAllAuditType()
        {
            var result = new List<AuditTypeModel>();

            var list = await Task.Run(() => _dbContext.AuditorTypes.Where(x => x.IsDeleted == false==x.IsActive==true).ToList());
            result = _mapper.Map<List<AuditTypeModel>>(list);
            return result;
        }
        public async Task<List<ClientSitesModel>> GetAllClientSites()
        {
            var result = new List<ClientSitesModel>();

            var list = await Task.Run(() => _dbContext.ClientSites.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<ClientSitesModel>>(list);
            return result;
        }
        public async Task<List<VerificationTypeModel>> GetAllVerificationType()
        {
            var result = new List<VerificationTypeModel>();

            var list = await Task.Run(() => _dbContext.VerificationType.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<VerificationTypeModel>>(list);
            return result;
        }
            public async Task<List<ProjectTypeModel>> GetAllProjectType()
        {
            var result = new List<ProjectTypeModel>();

            var list = await Task.Run(() => _dbContext.ProjectType.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<ProjectTypeModel>>(list);
            return result;
        }

        public async Task<List<ConsultantModel>> GetAllConsultantList(long id)
        {
            var result = new List<ConsultantModel>();

            var list = await Task.Run(() => _dbContext.Consultant.Where(x => x.IsDeleted == false == x.IsActive == true && x.OrganizationId == id).ToList());
            result = _mapper.Map<List<ConsultantModel>>(list);
            return result;
        }

            public async Task<List<HolidayTypeModel>> GetAllHolidayTypeList()
            {
                var result = new List<HolidayTypeModel>();

                var list = await Task.Run(() => _dbContext.HolidayType.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
                result = _mapper.Map<List<HolidayTypeModel>>(list);
                return result;
            }
            public async Task<List<AccreditationModel>> GetAllAccreditation()
        {
            var result = new List<AccreditationModel>();

            var list = await Task.Run(() => _dbContext.Accreditation.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<AccreditationModel>>(list);
            return result;
        }

              public async Task<List<SurveillanceVisitFrequencyModel>> SurveillanceVisitFrequencyList()
        {
            var result = new List<SurveillanceVisitFrequencyModel>();

            var list = await Task.Run(() => _dbContext.SurveillanceVisitFrequency.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<SurveillanceVisitFrequencyModel>>(list);
            return result;
        }

        public async Task<List<ClientSitesModel>> GetAllClientSites(long ClientId)
        {
            var result = new List<ClientSitesModel>();

            var list = await Task.Run(() => _dbContext.ClientSites.Where(x => x.IsDeleted == false == x.IsActive == true && x.ClientId==ClientId).ToList());
            result = _mapper.Map<List<ClientSitesModel>>(list);
            return result;
        }
        public async Task<List<RiskModel>> GetAllRisk()
        {
            var result = new List<RiskModel>();

            var list = await Task.Run(() => _dbContext.Risk.Where(x => x.IsDeleted == false == x.IsActive == true ).ToList());
            result = _mapper.Map<List<RiskModel>>(list);
            return result;
        }

        public async Task<List<RiskModel>> GetAllRiskByNaceCode(long naceCodeId)
        {
            var result = new List<RiskModel>();

            var list = await Task.Run(() => _dbContext.Risk.Where(x =>x.IsActive == true && x.Id== naceCodeId).ToList());
            result = _mapper.Map<List<RiskModel>>(list);
            return result;
        }



        public async Task<List<SurveillanceMethodModel>> GetALlSurveillanceMethod()
        {
            var result = new List<SurveillanceMethodModel>();

            var list = await Task.Run(() => _dbContext.SurveillanceMethod.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<SurveillanceMethodModel>>(list);
            return result;
        }
        public async Task<List<ExpencesModel>> GetALlExpences()
        {
            var result = new List<ExpencesModel>();

            var list = await Task.Run(() => _dbContext.Expenses.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<ExpencesModel>>(list);
            return result;
        }

        public async Task<GetPagedProjectRemarksModel> GetPagedProjectRemarks(PagedResponseModel model)
        {
            try
            {
                // OzoneContext _dbContext = new OzoneContext();
                var result = new GetPagedProjectRemarksModel();
                var RemarksList = new List<ProjectRemarksHistoryModel>();

                //var list = string.Empty;
                if (model.AuthAllowed == true)
                {
                    var list = await _dbContext.ProjectRemarksHistory.Include(x => x.ApprovalStatus).Include(x => x.RemarksBy).Include(x=>x.Project.ProjectType).Include(x=>x.Project.ClientSite).Where(x => x.IsDeleted == false && x.ProjectId.ToString()==model.Keyword)
                                  .OrderByDescending(x => x.Id).ToListAsync();
                    RemarksList = _mapper.Map<List<ProjectRemarksHistoryModel>>(list);

                }




                result.ProjectRemarksHistoryModel = GetPage(RemarksList, model.Page, model.PageSize);
                result.TotalCount = RemarksList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<ProjectRemarksHistoryModel> GetPage(List<ProjectRemarksHistoryModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<List<ProjectApprovalStatusModel>> GetALLProjectStatus(long id)
        {
            var result = new List<ProjectApprovalStatusModel>();

            var list = await Task.Run(() => _dbContext.ProjectsApprovalStatus.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<ProjectApprovalStatusModel>>(list);
            return result;
        }

        public async Task<List<ClientModel>> GetALLClients(long id)
        {
            var result = new List<ClientModel>();

            var list = await Task.Run(() => _dbContext.Client.Where(x => x.IsDeleted == false == x.IsActive == true && x.OrganizationId == id).ToList());
            result = _mapper.Map<List<ClientModel>>(list);
            return result;
        }

        public async Task<List<ServiceTypeModel>> GetALLServicesType()
        {
            var result = new List<ServiceTypeModel>();

            var list = await Task.Run(() => _dbContext.ServiceType.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<ServiceTypeModel>>(list);
            return result;
        }
        public async Task<List<MethodologyModel>> GetALLMethodology()
        {
            var result = new List<MethodologyModel>();

            var list = await Task.Run(() => _dbContext.Methodology.Where(x => x.IsDeleted == false == x.IsActive == true ).ToList());
            result = _mapper.Map<List<MethodologyModel>>(list);
            return result;
        }
        public async Task<List<AssessmentCompletedModel>> GetALLAssessmentCompleted()
        {
            var result = new List<AssessmentCompletedModel>();

            var list = await Task.Run(() => _dbContext.AssessmentCompleted.Where(x => x.IsDeleted == false == x.IsActive == true) .ToList());
            result = _mapper.Map<List<AssessmentCompletedModel>>(list);
            return result;
        }
        public async Task<List<CompletedModuleModel>> GetALLCompletedModule()
        {
            var result = new List<CompletedModuleModel>();

            var list = await Task.Run(() => _dbContext.CompletedModule.Where(x => x.IsDeleted == false == x.IsActive == true ).ToList());
            result = _mapper.Map<List<CompletedModuleModel>>(list);
            return result;
        }
        public async Task<List<EffluentTreatmentPlantModel>> GetALLEffluentTreatmentPlant()
        {
            var result = new List<EffluentTreatmentPlantModel>();

            var list = await Task.Run(() => _dbContext.EffluentTreatmentPlant.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<EffluentTreatmentPlantModel>>(list);
            return result;
        }
        public async Task<List<ModuleVersionModel>> GetALLModuleVersion(long standardid)
        {
            var result = new List<ModuleVersionModel>();

            var list = await Task.Run(() => _dbContext.ModuleVersion.Where(x => x.IsDeleted == false == x.IsActive == true && x.StandardId==standardid).ToList());
            result = _mapper.Map<List<ModuleVersionModel>>(list);
            return result;
        }
        public async Task<List<ModuleShareModel>> GetALLModuleShare()
        {
            var result = new List<ModuleShareModel>();

            var list = await Task.Run(() => _dbContext.ModuleShare.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<ModuleShareModel>>(list);
            return result;
        }
        public async Task<List<RequestOfSiteModel>> GetALLRequestOfSite()
        {
            var result = new List<RequestOfSiteModel>();

            var list = await Task.Run(() => _dbContext.RequestOfSite.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<RequestOfSiteModel>>(list);
            return result;
        }
        
       public async Task<List<CompletedStepsModel>> GetALLCompletedSetup()
        {
            var result = new List<CompletedStepsModel>();

            var list = await Task.Run(() => _dbContext.CompletedSteps.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<CompletedStepsModel>>(list);
            return result;
        }

        public async Task<List<VisitTypeModel>> GetALLVisitType()
        {
            var result = new List<VisitTypeModel>();

            var list = await Task.Run(() => _dbContext.VisitType.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<VisitTypeModel>>(list);
            return result;
        }
        public async Task<List<VisitStatusModel>> GetALLVisitStatus()
        {
            var result = new List<VisitStatusModel>();

            var list = await Task.Run(() => _dbContext.VisitStatus.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<VisitStatusModel>>(list);
            return result;
        }
        
        public async Task<List<ClientProjectModel>> GetAllProjectCode()
        {
            var result = new List<ClientProjectModel>();

            var list = await Task.Run(() => _dbContext.ClientProjects.Include(x=>x.ClientSite).Where(x => x.IsDeleted == false).ToList());
            result = _mapper.Map<List<ClientProjectModel>>(list);
            return result;
        }
        public async Task<List<ClientProjectModel>> GetAllProjectCodeById(long id)
        {
            var result = new List<ClientProjectModel>();

            var list = await Task.Run(() => _dbContext.ClientProjects.Include(x => x.ClientSite).Where(x => x.IsDeleted == false && x.Id==id).ToList());
            result = _mapper.Map<List<ClientProjectModel>>(list);
            return result;
        }

        public async Task<List<AuditDocumentsTypeModel>> GetAllAuditDocumentsType()
        {
            var result = new List<AuditDocumentsTypeModel>();

            var list = await Task.Run(() => _dbContext.AuditDocumentsType.Where(x => x.IsDeleted == false && x.IsActive==true).ToList());
            result = _mapper.Map<List<AuditDocumentsTypeModel>>(list);
            return result;
        }
      
        public async Task<ClientProjectModel> GetProjectCodeById(long id)
        {
            var result = new ClientProjectModel();

            var list = await Task.Run(() => _dbContext.ClientProjects.Include(x=>x.Standard).Where(x => x.IsDeleted == false  && x.Id==id).FirstOrDefault());
            result = _mapper.Map<ClientProjectModel>(list);
            return result;
        }
        public async Task<List<StateModel>> GetAllStateBycountry(long countryId)
        {
            var result = new List<StateModel>();
            var list = await Task.Run(() => _dbContext.State.Where(x => x.IsActive == true && x.CountryId == countryId).ToList());
            result = _mapper.Map<List<StateModel>>(list);
            return result;
        }


        public async Task<List<CityModel>> GetAllCityByState(long statId)
        {
            var result = new List<CityModel>();
            var list = await Task.Run(() => _dbContext.Cities.Where(x => x.IsActive == true && x.StateId == statId).ToList());
            result = _mapper.Map<List<CityModel>>(list);
            return result;
        }
        public async Task<List<StageCertificationModel>> GetAllStageCertification()
        {
            var result = new List<StageCertificationModel>();

            var list = await Task.Run(() => _dbContext.StageOfCertification.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<StageCertificationModel>>(list);
            return result;
        }
        public async Task<List<SecUserModel>> GetAllAdminList()
        {
            var result = new List<SecUserModel>();

            var list = await Task.Run(() => _dbContext.SecUser.Where(x => x.IsDeleted == false && x.IsActive == true && x.RoleId==2).ToList());
            result = _mapper.Map<List<SecUserModel>>(list);
            return result;
        }
        public async Task<List<SecUserModel>> GetAllTechnicalExpert(long id)
        {
            var result = new List<UserAuditorNaceModel>();
            var standard = new List<UserStandardModel>();
            List<UserStandards> userstandardList = new List<UserStandards>();
            var SecUserMod = new List<SecUserModel>();

            var Project = await Task.Run(() => _dbContext.ClientProjects.Include(x => x.Client).Where(x => x.Id == id).FirstOrDefault());
           
            var list = await Task.Run(() => _dbContext.UserAuditorNace.Include(x => x.User).Where(x => x.IsDeleted == false && x.NaceCodeId == Project.Client.NaceCodeId && x.ApprovalStatusId == 2).ToList());

            result = _mapper.Map<List<UserAuditorNaceModel>>(list);


            var TechExpt = result

                     .GroupBy(x => new { x.UserId, x.UserName })
                     .Select(group => new { Id = group.Key.UserId, UserName= group.Key.UserName }).ToList();

          

            foreach (var user in TechExpt)
            {
                SecUserModel users = new SecUserModel();
                users.Id =user.Id.Value;
                users.FullName = user.UserName;
                SecUserMod.Add(users);


            }

            SecUserModel MM = new SecUserModel();
            MM.Id = 0;
            MM.FullName = "--- Not Selected ---";
            SecUserMod.Add(MM);
            return SecUserMod.OrderBy(x => x.Id).ToList();
          
        }


        public async Task<List<SecUserModel>> GetAllJustifiedPerson(long id)
        {
            var result = new List<UserAuditorNaceModel>();
            var standard = new List<UserStandardModel>();
            var SecUserMod = new List<SecUserModel>();
            //var Dbresult = await Task.Run(() => _dbContext.ClientProjects.Include(x=>x.Client).Where(x => x.Id == id).FirstOrDefault());
            //var Dbresult = await Task.Run(() => _dbContext.ProjectSa8000.Where(x => x.ClientProjectId == id).FirstOrDefault());

            var Project = await Task.Run(() => _dbContext.ClientProjects.Include(x => x.Client).Where(x => x.Id == id).FirstOrDefault());
            //if (Project.StandardId == 7)
            //{
            //    var list = await Task.Run(() => _dbContext.UserAuditorNace.Include(x => x.User).Where(x => x.IsDeleted == false && x.StandardId == Project.StandardId && x.NaceCodeId == Project.Client.NaceCodeId && x.EacodeId == Project.Client.EacodeId && x.ApprovalStatusId == 2).ToList());
            //    result = _mapper.Map<List<UserAuditorNaceModel>>(list);

            //    foreach (var user in result)
            //    {
            //        SecUserModel users = new SecUserModel();
            //        users.Id = user.UserId.Value;
            //        users.FullName = user.UserName;
            //        SecUserMod.Add(users);


            //    }

            //}
            //else  
            //{

            //    var list = await Task.Run(() => _dbContext.UserStandards.Include(x => x.User).Where(x => x.IsDeleted == false && x.StandardId == Project.StandardId && x.AuditorTypeId == 8 && x.ApprovalStatusId == 2).ToList());
            //    standard = _mapper.Map<List<UserStandardModel>>(list);
            //    foreach (var std in standard)
            //    {
            //        SecUserModel users = new SecUserModel();
            //        users.Id = std.UserId.Value;
            //        users.FullName = std.UserName;
            //        SecUserMod.Add(users);


            //    }
            //}
            var list = await Task.Run(() => _dbContext.UserAuditorNace.Include(x => x.User).Where(x => x.IsDeleted == false && x.StandardId == Project.StandardId && x.NaceCodeId == Project.Client.NaceCodeId && x.EacodeId == Project.Client.EacodeId && x.ApprovalStatusId == 2).ToList());
            result = _mapper.Map<List<UserAuditorNaceModel>>(list);

            foreach (var user in result)
            {
                SecUserModel users = new SecUserModel();
                users.Id = user.UserId.Value;
                users.FullName = user.UserName;
                SecUserMod.Add(users);


            }

            SecUserModel MM = new SecUserModel();
            MM.Id = 0;
            MM.FullName = "--- Not Selected ---";
            SecUserMod.Add(MM);
            return SecUserMod.OrderBy(x => x.Id).ToList();
            //return SecUserMod;
        }
        public async Task<List<VisitLevelModel>> GetALLVisitLevel()
        {
            var result = new List<VisitLevelModel>();

            var list = await Task.Run(() => _dbContext.VisitLevel.Where(x => x.IsDeleted == false == x.IsActive == true).ToList());
            result = _mapper.Map<List<VisitLevelModel>>(list);

            VisitLevelModel MM = new VisitLevelModel();
            MM.Id = 0;
            MM.Name = "--- Not Selected ---";
            result.Add(MM);
            return result.OrderBy(x => x.Id).ToList();
            //return result;
        }

        public async Task<List<SecUserModel>> GetAllUsers(long organizationId)
        {
            var result = new List<SecUserModel>();

            var list = await Task.Run(() => _dbContext.SecUser.Where(x => x.OrganizationId == organizationId && x.IsDeleted == false && x.IsActive == true ).ToList());
            result = _mapper.Map<List<SecUserModel>>(list);
            return result;
        }
        public async Task<List<EnrollmentTypeModel>> GetEnrollmentType()
        {
            var result = new List<EnrollmentTypeModel>();

            var list = await Task.Run(() => _dbContext.TypeOfEnrollment.ToList());
            result = _mapper.Map<List<EnrollmentTypeModel>>(list);
            return result;
        }

        public async Task<string> CreateVisitLevel(VisitLevelModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                VisitLevel DbVisitLevel = await Task.Run(() => _dbContext.VisitLevel.Where(x => x.IsDeleted == false && x.Id == input.Id).FirstOrDefault());

                try
                {
                    var message = "";
                    long newid;
                    bool New = false;
                    if (DbVisitLevel == null)
                    {
                        New = true;
                        DbVisitLevel = new VisitLevel();
                    }
                    //DbModules.Id = input.Id;
                    DbVisitLevel.Id = input.Id;
                    DbVisitLevel.Name = input.Name;
                    DbVisitLevel.IsActive = input.IsActive;
                    DbVisitLevel.Description = input.Description;
                    //DbAudditor.Address = input.Address;
                    DbVisitLevel.Code = input.Code;
                    DbVisitLevel.CreatedById = input.CreatedById;


                    if (New == true)
                    {
                        DbVisitLevel.IsDeleted = false;
                        //await base.AddAsync(DbVisitLevel);
                        _dbContext.VisitLevel.Add(DbVisitLevel);
                        message = "Successfully Inserted!";
                    }
                    else
                    {
                        _dbContext.VisitLevel.Update(DbVisitLevel);
                        //await base.UpdateAsync(DbVisitLevel);
                        message = "Successfully Updated!";
                    }
                    var result = await _unitOfWork.SaveChangesAsync();

                    newid = DbVisitLevel.Id;

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

        public async Task<string> DeleteVisitLevelById(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {

                VisitLevel dbVisit = _dbContext.VisitLevel.Where(u => u.Id == id).FirstOrDefault();


                if (dbVisit != null)
                {
                    try
                    {
                        dbVisit.IsDeleted = true;
                        _dbContext.VisitLevel.Update(dbVisit);
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
        }


         public async Task<List<OrganizationModel>> GetAllAgencywithHeadOffice()
        {
            var result = new List<OrganizationModel>();
            var list = await Task.Run(() => _dbContext.Organization.Where(x => x.IsActive == true && x.IsDeleted==false).ToList());
            result = _mapper.Map<List<OrganizationModel>>(list);
            return result;
        }
    }
}
