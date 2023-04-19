using Ozone.Application.DTOs;
using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Infrastructure
{
  public  interface IAllDropdownService
    {
        // Task<List<ClientSitesModel>> GetAllClientSites();
        Task<List<VerificationTypeModel>> GetAllVerificationType();
        Task<List<ProjectTypeModel>> GetAllProjectType();
        Task<List<ConsultantModel>> GetAllConsultantList(long id);
        Task<List<HolidayTypeModel>> GetAllHolidayTypeList();
        Task<List<AccreditationModel>> GetAllAccreditation();
        Task<List<SurveillanceVisitFrequencyModel>> SurveillanceVisitFrequencyList();
        Task<List<ClientSitesModel>> GetAllClientSites(long ClientId);
        Task<List<RiskModel>> GetAllRisk();
        Task<List<RiskModel>> GetAllRiskByNaceCode(long naceCodeId);

        Task<List<SurveillanceMethodModel>> GetALlSurveillanceMethod();
        Task<List<ExpencesModel>> GetALlExpences();
        Task<GetPagedProjectRemarksModel> GetPagedProjectRemarks(PagedResponseModel model);

        Task<List<ProjectApprovalStatusModel>> GetALLProjectStatus(long id);
        Task<List<ClientModel>> GetALLClients(long id);

        Task<List<ServiceTypeModel>> GetALLServicesType();
        Task<List<MethodologyModel>> GetALLMethodology();
        Task<List<CompletedModuleModel>> GetALLCompletedModule();
        Task<List<AssessmentCompletedModel>> GetALLAssessmentCompleted();
        Task<List<EffluentTreatmentPlantModel>> GetALLEffluentTreatmentPlant();
        Task<List<ModuleVersionModel>> GetALLModuleVersion(long standardid);
        Task<List<ModuleShareModel>> GetALLModuleShare();
        Task<List<RequestOfSiteModel>> GetALLRequestOfSite();


        Task<List<CompletedStepsModel>> GetALLCompletedSetup();
        Task<List<VisitTypeModel>> GetALLVisitType();
        Task<List<VisitStatusModel>> GetALLVisitStatus();
        Task<List<ClientProjectModel>> GetAllProjectCode();
        Task<List<AuditDocumentsTypeModel>> GetAllAuditDocumentsType();
        Task<ClientProjectModel> GetProjectCodeById(long id);
        Task<List<CityModel>> GetAllCityByState(long statId);
        Task<List<StateModel>> GetAllStateBycountry(long countryId);
        Task<List<StageCertificationModel>> GetAllStageCertification();
        Task<List<SecUserModel>> GetAllAdminList();
        Task<List<SecUserModel>> GetAllTechnicalExpert(long id);
        Task<List<SecUserModel>> GetAllJustifiedPerson(long id);
        Task<List<VisitLevelModel>> GetALLVisitLevel();
        Task<List<SecUserModel>> GetAllUsers(long organizationId);
        Task<List<EnrollmentTypeModel>> GetEnrollmentType();
        Task<string> CreateVisitLevel(VisitLevelModel input);
        Task<string> DeleteVisitLevelById(long id);
        Task<List<OrganizationModel>> GetAllAgencywithHeadOffice();
    }
}
