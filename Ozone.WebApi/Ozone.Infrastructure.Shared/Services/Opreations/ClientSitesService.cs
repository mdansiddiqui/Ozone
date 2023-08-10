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

namespace Ozone.Infrastructure.Shared.Services
{
  public  class ClientSitesService : GenericRepositoryAsync<ClientSites>, IClientSitesService

    {

        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public ClientSitesService(
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

        public async Task<string> CreateClientSites(ClientSitesModel input)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ClientSites DbClient = null;
                if (input.Id > 0)
                {
                    DbClient = await Task.Run(() => _dbContext.ClientSites.Where(x => x.Id == input.Id).FirstOrDefault());
                }

                try
                {
                    //long newid;
                    bool New = false;
                    if (DbClient == null)
                    {

                        New = true;
                        DbClient = new ClientSites();
                        var newcSiteCod ="";
                        var ClientSiteCode = await Task.Run(() => _dbContext.ClientSites.Where(x => x.ClientId == input.ClientId && x.IsDeleted==false).OrderByDescending(x=>x.Id).Select(x=>x.Code).FirstOrDefault());
                        if (ClientSiteCode != null && ClientSiteCode != "")
                        {
                            int newcode;
                            newcode = Convert.ToInt32(ClientSiteCode.Substring(0, 3));
                            newcode = newcode + 1;
                            newcSiteCod = newcode.ToString().PadLeft(3, '0');
                        }
                        else 
                        {
                            newcSiteCod= "001";
                        }
                        DbClient.Code = newcSiteCod;

                    }
                    
                    DbClient.SiteName = input.SiteName;
                    DbClient.ClientId = input.ClientId;
                    DbClient.Address = input.Address;
                    DbClient.CountryId = input.CountryId;
                    DbClient.CityId = input.CityId;
                    DbClient.StateId = input.StateId;
                    DbClient.LegalStatus = input.LegalStatus;
                    DbClient.OutsourceProductionProcessess = input.OutsourceProductionProcessess;
                    DbClient.TotalEmployees = input.TotalEmployees; 
                    DbClient.ShiftTimings = input.ShiftTimings;
                    DbClient.IsActive = true;

                    //DbClient.Code = input.Code;










                    if (New == true)
                    {
                        DbClient.IsDeleted = false;
                        DbClient.CreatedDate = DateTime.Now;
                        DbClient.CreatedById = input.CreatedById;
                        // DbClient.IsDeleted = false;
                        await base.AddAsync(DbClient);
                    }
                    else
                    {
                        //  if (input.IsDeleted == false) { DbClient.IsDeleted = input.IsDeleted; }

                        DbClient.LastModifiedDate = DateTime.Now;
                        DbClient.LastModifiedById = input.LastModifiedById;

                        await base.UpdateAsync(DbClient);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    //  for user




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


        public async Task<GetPagedClientSitesModel> GetPagedClientSites(PagedResponseModel model)
  {
            try
            {

                var result = new GetPagedClientSitesModel();
                var ClientList = new List<ClientSitesModel>();

                if (model.AuthAllowed == true)
                {
                    var list = await _dbContext.ClientSites.Include(x=>x.City).Include(x=>x.Country).Include(x=>x.State).Include(x=>x.ClientProjects).Where(x => x.IsDeleted == false && x.IsActive == true && x.ClientId.ToString()==model.Keyword).OrderByDescending(x => x.Id).ToListAsync();
                    ClientList = _mapper.Map<List<ClientSitesModel>>(list);

                }



                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.ClientSitesModel = GetPage(ClientList, model.Page, model.PageSize);
                result.TotalCount = ClientList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            private List<ClientSitesModel> GetPage(List<ClientSitesModel> list, int page, int pageSize)
            {
                return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        public async Task<ClientSitesModel> GetClientSitesBYId(long id)
        {
            var result = new ClientSitesModel();
            var Clientdata = await Task.Run(() => _dbContext.ClientSites.Where(x => x.Id == id).FirstOrDefault());
            result = _mapper.Map<ClientSitesModel>(Clientdata);




            return result;
        }

        public async Task<string> ClientSitesDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                ClientSites dbresult = _dbContext.ClientSites.Where(u => u.Id == id).FirstOrDefault();


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


        

       public async Task<GetPagedClientProjectsModel> GetPagedClientProjects(long id,PagedResponseModel model)
        {
            var result = new GetPagedClientProjectsModel();
            var ClientList = new List<ClientProjectModel>();
            try
            {

               

                if (id > 0)
                {
                    

                    var list = await _dbContext.ClientProjects.Include(x => x.ClientSite).Include(x => x.ProjectType).Include(x => x.VerificationType).Include(x => x.Client).Include(x => x.Client.Organization).Include(x => x.Standard).Include(x => x.CreatedBy).Include(x=>x.ApprovalStatus).Where(x => x.IsDeleted == false && x.IsActive == true && x.ClientId == id ).OrderByDescending(x => x.Id).ToListAsync();
                    
                   var Client= _mapper.Map<List<ClientProjectModel>>(list);
                    ClientList=Client.Where(x=>x.AgencyName.ToLower().Contains(model.Keyword.ToLower()) ||
                               x.ClientName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.StandardName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.ApprovalStatusName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.ProjectTypeName.ToLower().Contains(model.Keyword.ToLower())).OrderByDescending(x=> x.Id).ToList();

                }
                else 
                {
                    var list = await _dbContext.ClientProjects.Include(x => x.ClientSite).Include(x => x.ProjectType).Include(x => x.VerificationType).Include(x => x.Client).Include(x => x.Client.Organization).Include(x => x.Standard).Include(x => x.CreatedBy).Include(x => x.ApprovalStatus).Where(x => x.IsDeleted == false && x.IsActive == true && x.ApprovalStatusId!=6).OrderByDescending(x => x.Id).ToListAsync();
                    var Client = _mapper.Map<List<ClientProjectModel>>(list);

                    ClientList = Client.Where(x => x.AgencyName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.ClientName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.StandardName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.ApprovalStatusName.ToLower().Contains(model.Keyword.ToLower()) ||
                                 x.ProjectTypeName.ToLower().Contains(model.Keyword.ToLower())).OrderByDescending(x => x.Id).ToList();




                }



                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.ClientProjectModel = GetPage(ClientList, model.Page, model.PageSize);
                result.TotalCount = ClientList.Count();
                return result;
            }
            catch (Exception ex)
            {
                var exp = ex;
               // throw ex;
                return result;
            }
        }
        private List<ClientProjectModel> GetPage(List<ClientProjectModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        

        public async Task<ProjectFormPathModel> GetProjectUrlBYId(long id)
          
         {
            var result = new ProjectFormPathModel();
            var Clientdata = await Task.Run(() => _dbContext.ProjectFormsPath.Where(x => x.StandardId == id && x.IsActive==true && x.IsDeletes==false ).FirstOrDefault());
            result = _mapper.Map<ProjectFormPathModel>(Clientdata);    
                                                                        
            return result;   
        }

        public async Task<GetPagedClientProjectsModel> GetPagedAllProjects(long id, PagedResponseModel model)
        {
                 
            var result = new GetPagedClientProjectsModel();
            var ClientProjectsdb = new List<ClientProjects>();

            if (model.organizationId > 1)
            {
                ClientProjectsdb = await _dbContext.ClientProjects.Include(x => x.ClientSite).Include(x => x.Client.Organization).Include(x => x.Client).Include(x => x.ApprovalStatus).Where(x => x.IsDeleted == false && x.IsActive == true && x.Client.OrganizationId == model.organizationId && x.ApprovalStatusId == id).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ClientProjectsdb = await _dbContext.ClientProjects.Include(x => x.ClientSite).Include(x => x.Client.Organization).Include(x => x.Client).Include(x => x.ApprovalStatus).Where(x => x.IsDeleted == false && x.IsActive == true  && x.ApprovalStatusId == id).OrderByDescending(x => x.Id).ToListAsync();
            }
           

            List<UserStandards> Userstandard = new List<UserStandards>();

            if (model.roleId == 19 || model.roleId == 21)
            {
                Userstandard = await Task.Run(() => _dbContext.UserStandards.Where(x => x.IsDeleted == false && x.UserId == model.userId && x.AuditorTypeId == 4).ToList());

        
            List<ClientProjects> AllProjects = new List<ClientProjects>();
            foreach (var standard in Userstandard)
            {
              AllProjects.AddRange(ClientProjectsdb.Where(x => x.StandardId == standard.StandardId).ToList());
            
            }

            ClientProjectsdb = AllProjects;
            }
            var Client = _mapper.Map<List<ClientProjectModel>>(ClientProjectsdb);
            //result.ClientProjectModel= GetPage(Client, model.Page,7);

            result.ClientProjectModel = Client;
            result.TotalCount = Client.Count();





            return result;


        }


        public async Task<GetPagedClientProjectsModel> GetPagedAllClientChange(long id, PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedClientProjectsModel();
                List<ActivityLog> List = new List<ActivityLog>();
                List<ActivityLogModel> AuditList = new List<ActivityLogModel>();

                

                if (model.organizationId > 1)
                {
                    List = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "Client" &&  x.OrganizationId==model.organizationId).OrderByDescending(x => x.Id).ToListAsync();
                        var query = List


                               .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId, x.OrganizationId })
                               .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();


                        foreach (var activityLog in query)
                        {
                            ActivityLogModel Acmodel = new ActivityLogModel();
                     
                            var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                            var client = await _dbContext.Client.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                            var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                            Acmodel.ClientName = client.Name;
                            Acmodel.OrganizationName = organization.Name;
                            Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.ApprovalStatusId = activityLog.Peo.ApprovalStatusId;
                        Acmodel.Id = activityLog.Peo.TableRowId;
                        AuditList.Add(Acmodel);

                            //Acmodel.
                        }
                        result.ActivityLogModel = AuditList;
                  
                }
                else
                {
                   
                        List = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "Client").OrderByDescending(x => x.Id).ToListAsync();
                        var query = List


                               .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId,x.OrganizationId})
                               .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();

                  
                        foreach (var activityLog in query)
                        {
                            ActivityLogModel Acmodel = new ActivityLogModel();
                       

                        //var activityClient = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "Client").FirstOrDefaultAsync();
                        var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                        var client = await _dbContext.Client.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                        var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                            Acmodel.ClientName = client.Name;
                            Acmodel.OrganizationName = organization.Name;
                            Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.Id = activityLog.Peo.TableRowId;
                        Acmodel.ApprovalStatusId = activityLog.Peo.ApprovalStatusId;
                        
                        AuditList.Add(Acmodel);

                            //Acmodel.
                        }
                        result.ActivityLogModel = AuditList;
                   
                  
                   


                }
                //AuditList = _mapper.Map<List<ActivityLogModel>>(query);
                //List<UserStandards> Userstandard = new List<UserStandards>();

                //if (model.roleId == 19 || model.roleId == 21)
                //{
                //    Userstandard = await Task.Run(() => _dbContext.UserStandards.Where(x => x.IsDeleted == false && x.UserId == model.userId && x.AuditorTypeId == 4).ToList());


                //    List<ClientAuditVisitModel> AllProjects = new List<ClientAuditVisitModel>();
                //    foreach (var standard in Userstandard)
                //    {
                //        AllProjects.AddRange(AuditList.Where(x => x.StandardId == standard.StandardId && x.ReviewerId == model.userId).ToList());

                //    }

                //    AuditList = AllProjects;
                //}

                ////var Audit = _mapper.Map<List<ClientAuditVisitModel>>(List);

                ////result.ClientAuditVisitModel = GetPage(Audit, model.Page, model.PageSize);
                //result.ClientAuditVisitModel = AuditList.OrderByDescending(x => x.Id).ToList();
                //result.TotalCount = AuditList.Count();

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<GetPagedClientProjectsModel> GetAllClientSitesChangeRequest(long id, PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedClientProjectsModel();
                List<ActivityLog> List = new List<ActivityLog>();
                List<ActivityLogModel> AuditList = new List<ActivityLogModel>();



                if (model.organizationId > 1)
                {
                    List = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "ClientSites" && x.OrganizationId == model.organizationId).OrderByDescending(x => x.Id).ToListAsync();
                    var query = List


                           .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId, x.OrganizationId })
                           .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();


                    foreach (var activityLog in query)
                    {
                        ActivityLogModel Acmodel = new ActivityLogModel();

                        var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                        var clientsites = await _dbContext.ClientSites.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                        var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                        Acmodel.ClientName = clientsites.SiteName;
                        Acmodel.OrganizationName = organization.Name;
                        Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.Id = activityLog.Peo.TableRowId;
                        Acmodel.ApprovalStatusId = activityLog.Peo.ApprovalStatusId;

                        AuditList.Add(Acmodel);

                        //Acmodel.
                    }
                    result.ActivityLogModel = AuditList;

                }
                else
                {

                    List = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "ClientSites").OrderByDescending(x => x.Id).ToListAsync();
                    var query = List


                           .GroupBy(x => new { x.TableRowId, x.ApprovalStatusId, x.OrganizationId })
                           .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();


                    foreach (var activityLog in query)
                    {
                        ActivityLogModel Acmodel = new ActivityLogModel();


                        //var activityClient = await _dbContext.ActivityLog.Where(x => x.ApprovalStatusId == id && x.TableName == "Client").FirstOrDefaultAsync();
                        var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                        var clientsites = await _dbContext.ClientSites.Where(x => x.Id == activityLog.Peo.TableRowId).FirstOrDefaultAsync();
                        var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                        Acmodel.ClientName = clientsites.SiteName;
                        Acmodel.OrganizationName = organization.Name;
                        Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.Id = activityLog.Peo.TableRowId;
                        Acmodel.ApprovalStatusId = activityLog.Peo.ApprovalStatusId;

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


        public async Task<List<ActivityLogRemarksHistoryModel>> GetClientChangeRemarksDataById(long id)
        {
            try
            {
                //var result = new ActivityLogRemarksHistoryModel();
                List<ActivityLogRemarksHistory> List = new List<ActivityLogRemarksHistory>();
                List<ActivityLogRemarksHistoryModel> AuditList = new List<ActivityLogRemarksHistoryModel>();

                List = await _dbContext.ActivityLogRemarksHistory.Include(x=> x.ApprovalStatus).Include(x=>x.RemarksBy).Where(x => x.ActivityLogId == id ).OrderByDescending(x => x.Id).ToListAsync();


                AuditList = _mapper.Map<List<ActivityLogRemarksHistoryModel>>(List);


                return AuditList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ActivityLogRemarksHistoryModel>> GetClientSitesChangeRemarksDataById(long id)
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



        public async Task<List<ActivityLogModel>> GetClientChangeDataById(long id, PagedResponseModel model)
        {
            try
            {

             
                List<ActivityLog> List = new List<ActivityLog>();
                List<ActivityLogModel> AuditList = new List<ActivityLogModel>();

                List = await _dbContext.ActivityLog.Where(x => x.TableRowId == id && x.ApprovalStatusId == model.statusId && x.TableName == "Client" ).OrderByDescending(x => x.Id).ToListAsync();
                
                    foreach (var activityLog in List)
                    {
                        ActivityLogModel Acmodel = new ActivityLogModel();
                             //var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                             var client = await _dbContext.Client.Where(x => x.Id == activityLog.TableRowId).FirstOrDefaultAsync();
                       //var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                         Acmodel.ClientName = client.Name;
                        //Acmodel.OrganizationName = organization.Name;
                        //Acmodel.ApprovalStatusName = status.Name;
                        Acmodel.AffectedColumns = activityLog.AffectedColumns;
                        Acmodel.OldValues = activityLog.OldValues;
                        Acmodel.NewValues = activityLog.NewValues;
                    Acmodel.CreatedDate = activityLog.CreatedDate;

                    Acmodel.Id = activityLog.Id;
                    Acmodel.FilePath = activityLog.FilePath;

                    if (Acmodel.AffectedColumns == "CountryId")
                    {
                        var countryold = await _dbContext.Countries.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                        var countrynew = await _dbContext.Countries.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                        Acmodel.OldValues = countryold.Name;
                        Acmodel.NewValues = countrynew.Name;

                    }
                    if (Acmodel.AffectedColumns == "StateId")
                    {
                        var stateold = await _dbContext.State.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                        var statenew = await _dbContext.State.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                        Acmodel.OldValues = stateold.Name;
                        Acmodel.NewValues = statenew.Name;
                    }
                    if (Acmodel.AffectedColumns == "CityId")
                    {
                        var cityold = await _dbContext.Cities.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                        var citynew = await _dbContext.Cities.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                        Acmodel.OldValues = cityold.Name;
                        Acmodel.NewValues = citynew.Name;
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

        public async Task<List<ActivityLogModel>> GetClientSitesChangeDataById(long id, PagedResponseModel model)
        {
            try
            {


                List<ActivityLog> List = new List<ActivityLog>();
                List<ActivityLogModel> AuditList = new List<ActivityLogModel>();

                List = await _dbContext.ActivityLog.Where(x => x.TableRowId == id && x.ApprovalStatusId == model.statusId && x.TableName == "ClientSites").OrderByDescending(x => x.Id).ToListAsync();

                foreach (var activityLog in List)
                {
                    ActivityLogModel Acmodel = new ActivityLogModel();
                    //var status = await _dbContext.ApprovalStatus.Where(x => x.Id == activityLog.Peo.ApprovalStatusId).FirstOrDefaultAsync();
                    var clientSites = await _dbContext.ClientSites.Where(x => x.Id == activityLog.TableRowId).FirstOrDefaultAsync();
                    //var organization = await _dbContext.Organization.Where(x => x.Id == activityLog.Peo.OrganizationId).FirstOrDefaultAsync();
                    Acmodel.ClientName = clientSites.SiteName;
                    //Acmodel.OrganizationName = organization.Name;
                    //Acmodel.ApprovalStatusName = status.Name;
                    Acmodel.AffectedColumns = activityLog.AffectedColumns;
                    Acmodel.OldValues = activityLog.OldValues;
                    Acmodel.NewValues = activityLog.NewValues;
                    Acmodel.CreatedDate = activityLog.CreatedDate;

                    Acmodel.Id = activityLog.Id;
                    Acmodel.FilePath = activityLog.FilePath;

                    if (Acmodel.AffectedColumns == "CountryId")
                    {
                        var countryold = await _dbContext.Countries.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                        var countrynew = await _dbContext.Countries.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                        Acmodel.OldValues = countryold.Name;
                        Acmodel.NewValues = countrynew.Name;

                    }
                    if (Acmodel.AffectedColumns == "StateId")
                    {
                        var stateold = await _dbContext.State.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                        var statenew = await _dbContext.State.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                        Acmodel.OldValues = stateold.Name;
                        Acmodel.NewValues = statenew.Name;
                    }
                    if (Acmodel.AffectedColumns == "CityId")
                    {
                        var cityold = await _dbContext.Cities.Where(x => x.Id == Convert.ToInt64(activityLog.OldValues)).FirstOrDefaultAsync();
                        var citynew = await _dbContext.Cities.Where(x => x.Id == Convert.ToInt64(activityLog.NewValues)).FirstOrDefaultAsync();
                        Acmodel.OldValues = cityold.Name;
                        Acmodel.NewValues = citynew.Name;
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


        public async Task<string> ApprovedClientChange(ActivityLogModel input)
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
                                Client client = await Task.Run(() => _dbContext.Client.Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());
                                string columnName = activityLog.AffectedColumns;



                                if (columnName == "Email")
                                {

                                    client.Email = activityLog.NewValues;
                                }

                                if (columnName == "Name")
                                {

                                    client.Name = activityLog.NewValues;
                                }

                                if (columnName == "Address1")
                                {

                                    client.Address1 = activityLog.NewValues;
                                }
                                if (columnName == "StateId")
                                {

                                    client.StateId = Convert.ToInt64(activityLog.NewValues);
                                }
                                if (columnName == "CityId")
                                {

                                    client.CityId = Convert.ToInt64(activityLog.NewValues);
                                }

                                if (columnName == "CountryId")
                                {

                                    client.CountryId = Convert.ToInt64(activityLog.NewValues);
                                }

                                if (columnName == "PostalCode")
                                {

                                    client.PostalCode = activityLog.NewValues;
                                }

                                _dbContext.Client.Update(client);
                            }
                            //Client cl = new Client();


                            //var properties = TypeDescriptor.GetProperties(client);


                            //foreach (PropertyDescriptor property in properties)
                            //{
                            //    if(property.Name.ToString() == columnName)

                            //    {


                            //switch (columnName.ToString())
                            //{
                            //    case var Email when columnName.Contains("Email"):
                            //        client.Email = activityLog.NewValues;
                            //        break;
                            //    default:
                            //        break;
                            //}

                            //        _dbContext.Client.Update(client);
                            //    }

                            //}

                            // properties.Find(x => x.Name == columnName);
                            //  client.columnName=
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

        public async Task<string> ApprovedClientSitesChange(ActivityLogModel input)
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
                                ClientSites clientSites = await Task.Run(() => _dbContext.ClientSites.Where(x => x.Id == activityLog.TableRowId).FirstOrDefault());
                                string columnName = activityLog.AffectedColumns;



                                if (columnName == "SiteName")
                                {

                                    clientSites.SiteName = activityLog.NewValues;
                                }

                                if (columnName == "LegalStatus")
                                {

                                    clientSites.LegalStatus = activityLog.NewValues;
                                }

                                if (columnName == "OutsourceProductionProcessess")
                                {

                                    clientSites.OutsourceProductionProcessess = activityLog.NewValues;
                                }
                                if (columnName == "TotalEmployees")
                                {

                                    clientSites.TotalEmployees = int.Parse(activityLog.NewValues);
                                }
                                if (columnName == "ShiftTimings")
                                {

                                    clientSites.ShiftTimings = activityLog.NewValues;
                                }

                                if (columnName == "Address")
                                {

                                    clientSites.Address = activityLog.NewValues;
                                }

                                if (columnName == "CountryId")
                                {

                                    clientSites.CountryId = long.Parse(activityLog.NewValues);
                                }

                                if (columnName == "StateId")
                                {

                                    clientSites.StateId = long.Parse(activityLog.NewValues);
                                }

                                if (columnName == "CityId")
                                {

                                    clientSites.CityId = long.Parse(activityLog.NewValues);
                                }

                                _dbContext.ClientSites.Update(clientSites);
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


        public async Task<GetPagedClientProjectsModel> GetPagedAllAudits(long id, PagedResponseModel model)
        {
            try
            {

                long userId = model.userId;
            var result = new GetPagedClientProjectsModel();
            List<ClientAuditVisit> List = new List<ClientAuditVisit>();
            List<ClientAuditVisitModel> AuditList = new List<ClientAuditVisitModel>();

            if (model.organizationId > 1)
            {
                if (model.roleId == 12)
                {
                    List = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.Project).Include(x => x.Project.Client).Include(x => x.Project.ClientSite).Include(x => x.Project.Client.Organization).Include(x => x.VisitStatus).Include(x => x.Project.Standard).Include(x => x.Reviewer).Include(x => x.LeadAuditor).Include(x => x.VisitLevel).Include(x=>x.Auditor1).Where(x => x.IsDeleted == false && x.VisitStatusId == id && x.OrganizationId == model.organizationId).OrderByDescending(x => x.Id).ToList());
                    List = List.Where(x => x.TechnicalExpertId == userId || x.JustifiedPersonId == userId || x.LeadAuditorId == userId || x.Auditor1Id == userId || x.Auditor2Id == userId || x.Auditor3Id == userId || x.Auditor4Id == userId || x.Auditor5Id == userId).ToList();
                    //List = await _dbContext.ClientAuditVisit.Include(x => x.Project.ClientSite).Include(x => x.Project.Client.Organization).Include(x => x.Project.Client).Include(x => x.VisitStatus).Include(x => x.Reviewer).Include(x => x.LeadAuditor).Include(x => x.VisitLevel).Where(x => x.IsDeleted == false && x.VisitStatusId == id && x.Project.Client.OrganizationId == model.organizationId && x.ReviewerId == model.userId).OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    List = await _dbContext.ClientAuditVisit.Include(x => x.Project.ClientSite).Include(x => x.Project.Client.Organization).Include(x => x.Project.Client).Include(x => x.VisitStatus).Include(x => x.Reviewer).Include(x => x.LeadAuditor).Include(x => x.VisitLevel).Where(x => x.IsDeleted == false && x.VisitStatusId == id && x.OrganizationId == model.organizationId).OrderByDescending(x => x.Id).ToListAsync();
                }
            }
            else
            {

                List = await _dbContext.ClientAuditVisit.Include(x => x.Project.ClientSite).Include(x => x.Project.Client.Organization).Include(x => x.Project.Client).Include(x => x.VisitStatus).Include(x => x.Reviewer).Include(x => x.LeadAuditor).Include(x => x.VisitLevel).Where(x => x.IsDeleted == false && x.VisitStatusId == id).OrderByDescending(x => x.Id).ToListAsync();


            }
            AuditList = _mapper.Map<List<ClientAuditVisitModel>>(List);
           // List<Userstandard> Userstandard = new List<UserStandards>();

            if (model.roleId == 19 || model.roleId == 21)
            {
                //Userstandard = await Task.Run(() => _dbContext.UserStandards.Where(x => x.IsDeleted == false && x.UserId == model.userId && x.AuditorTypeId == 4).ToList());
                   var Userstandard = (from m in _dbContext.UserStandards
                                         .Where(x => x.IsDeleted == false && x.UserId == userId && x.AuditorTypeId == 4)
                                        group m by m.StandardId into g
                                        select new 
                                        {
                                            StandardId = g.Key,
                                           
                                        }).ToList();

                    List<ClientAuditVisitModel> AllProjects = new List<ClientAuditVisitModel>();
                foreach (var standard in Userstandard)
                {
                    AllProjects.AddRange(AuditList.Where(x => x.StandardId == standard.StandardId && x.ReviewerId == userId).ToList());

                }

                AuditList = AllProjects;
            }

            //var Audit = _mapper.Map<List<ClientAuditVisitModel>>(List);

            //result.ClientAuditVisitModel = GetPage(Audit, model.Page, model.PageSize);
            result.ClientAuditVisitModel = AuditList.OrderByDescending(x => x.Id).ToList();
            result.TotalCount = AuditList.Count();

            return result;
        }
             catch (Exception)
            {

                throw;
            }
        }
       
        
        private List<ClientAuditVisitModel> GetPage(List<ClientAuditVisitModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

       
    }
}
