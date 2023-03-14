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
using Ozone.Application.Helpers;
using System.ComponentModel;
//using Ozone.Application.DTOs;
//using AutoMapper.Configuration;

namespace Ozone.Infrastructure.Shared.Services.MasterSetups
{
   public class ClientService : GenericRepositoryAsync<Client>, IClientService
    {

        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public ClientService(
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
        
        //    public async Task<ClientModel> GeClientfullDatabyId(long siteId)
        //{
        //    var result = new ClientModel();
        //    var Clientdata = await Task.Run(() => _dbContext.Client.Include(x => x.City).Where(x => x.Id == id).FirstOrDefault());
        //    result = _mapper.Map<ClientModel>(Clientdata);



        //    var clientprojects = await Task.Run(() => _dbContext.ClientProjects.Include(x => x.ClientSite).Where(x => x.ClientSite.ClientId == id && x.IsDeleted == false).FirstOrDefault());
        //    if (clientprojects != null)
        //    {
        //        //result.
        //        result.IsProjectExist = true;
        //    }
        //    else
        //    {
        //        result.IsProjectExist = false;
        //    }

        //    return result;
        //}
        public async Task<ClientModel> GetClientBYId(long id)
        {

            Client Clientdata = new Client();
            var result = new ClientModel();
           Clientdata = await Task.Run(() => _dbContext.Client.Include(x=>x.ClientSites).Include(x=>x.City).Include(x=>x.Country).Include(x=>x.State).Include(x=> x.Eacode).Include(x => x.NaceCode).Include(x => x.Risk).Where(x => x.Id == id).FirstOrDefault());
             result = _mapper.Map<ClientModel>(Clientdata);
            if (Clientdata !=null && Clientdata.ClientSites != null)
            {
                var sitecount = Clientdata.ClientSites.Count();
                result.SiteCount = sitecount;
            }
            var clientprojects = await Task.Run(() => _dbContext.ClientProjects.Include(x=>x.ClientSite).Where(x => x.ClientId == id && x.IsDeleted==false).FirstOrDefault());
            if (clientprojects != null)
            {
                //result.
                result.IsProjectExist = true;
            }
            else 
            {
                result.IsProjectExist = false;
            }

            return result;
        }
        private List<ClientModel> GetPage(List<ClientModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<string> CreateChangeClient(ClientModel input)
        {
            var SecFormData = await Task.Run(() => _dbContext.SecForm.Where(x => x.Name == input.FormName && x.IsClosed == false).FirstOrDefault());
          
            var newcode = "";
            string Message = "";
            long newid;

        

                // OzoneContext ozonedb = new OzoneContext();
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    var DbClient = new ClientModel();
                    if (input.Id > 0)
                    {
                        try
                        {

                            var clientResult = await Task.Run(() => _dbContext.Client.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault());
                            DbClient = _mapper.Map<ClientModel>(clientResult);

                            var result = new ClientModel();
                            var properties = TypeDescriptor.GetProperties(typeof(ClientModel));

                        foreach (PropertyDescriptor property in properties)
                        {
                            var currentValue = property.GetValue(input);
                            var dbdata = property.GetValue(DbClient);
                            if (property.Name.ToString() != "CreatorUserId" && property.Name.ToString() != "OrganizationId" && property.Name.ToString() != "FormName")
                            {
                                var dbdataValue = (dbdata == null ? "" : dbdata.ToString());
                                var NewValue = (currentValue == null ? "" : currentValue.ToString());
                                // var dbdataValue = (currentValue == null ? "" : dbdata.ToString());
                                // var test = (!(dbdata is DBNull)) ? Convert.ToInt64(dataReader["Id"]) : 0;

                                if (NewValue != "" && NewValue != null && NewValue != dbdataValue)
                                {
                                    //if (currentValue != null && dbdata != null && currentValue.ToString() != dbdata.ToString())
                                    //{
                                    ActivityLog activityLog = new ActivityLog();
                                    property.SetValue(result, currentValue);

                                    activityLog.UserId = input.CreatorUserId;
                                    activityLog.FormId = SecFormData.Id;
                                    activityLog.TableName = "Client";
                                    activityLog.TableRowId = DbClient.Id;
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

                                        string reportPath = _configuration["Reporting:UserChangeDocumentsPath"];

                                        // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                                        string newFileName = reportPath + +newid + "_" + Guid.NewGuid() + "_" + filename;
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


        public async Task<string> CreateChangeClienSite(ClientSitesModel input)
        {
            var SecFormData = await Task.Run(() => _dbContext.SecForm.Where(x => x.Name == input.FormName && x.IsClosed == false).FirstOrDefault());

            var newcode = "";
            string Message = "";
            long newid;



            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var DbClient = new ClientSitesModel();
                if (input.Id > 0)
                {
                    try
                    {

                        var clientSiteResult = await Task.Run(() => _dbContext.ClientSites.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefault());
                        DbClient = _mapper.Map<ClientSitesModel>(clientSiteResult);

                        var result = new ClientSitesModel();
                        var properties = TypeDescriptor.GetProperties(typeof(ClientSitesModel));

                        foreach (PropertyDescriptor property in properties)
                        {
                            var currentValue = property.GetValue(input);
                            var dbdata = property.GetValue(DbClient);
                            if (property.Name.ToString() != "CreatedById")
                            {
                                if (currentValue != null && dbdata != null && currentValue.ToString() != dbdata.ToString())
                                {
                                    ActivityLog activityLog = new ActivityLog();
                                    property.SetValue(result, currentValue);

                                    activityLog.UserId = input.CreatedById;
                                    activityLog.FormId = SecFormData.Id;
                                    activityLog.TableName = "ClientSites";
                                    activityLog.TableRowId = DbClient.Id;
                                    activityLog.OldValues = property.GetValue(DbClient).ToString();
                                    activityLog.NewValues = property.GetValue(input).ToString();
                                    activityLog.AffectedColumns = property.Name;
                                    activityLog.ApprovalStatusId = 1;
                                    activityLog.CreatedDate = DateTime.Now;
                                    activityLog.OrganizationId = input.OrganizationId;
                                    activityLog.CreatedBy = input.CreatedById;
                                    //activityLog.FilePath = input.FilePath;
                                    //activityLog.ContentType = input.ContentType;


                                    _dbContext.ActivityLog.Add(activityLog);
                                    await _unitOfWork.SaveChangesAsync();



                                    newid = activityLog.Id;



                                    if (input.File != null)
                                    {
                                        string filename = Path.GetFileName(input.File.FileName);
                                        string ContentType = input.File.ContentType;

                                        string reportPath = _configuration["Reporting:ClientSitesDocumentPath"];

                                        // string newFileName = @"D:\Update work\LIbrary_Documents\" + +newid + "_" + filename;

                                        string newFileName = reportPath + +newid + "_" + Guid.NewGuid() + "_" + filename;
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

        public async Task<string> CreateClient(ClientModel input)
        {
            var Agencycode = await Task.Run(() => _dbContext.Organization.Where(x => x.Id == input.OrganizationId).Select(x => x.Code).FirstOrDefault());

            var newcode ="";
            string Message ="";
            if (input.Id > 0) 
            {
                newcode = input.Code;
            }else
            {
                var agcode = Agencycode.Substring(0, 3);

                newcode = agcode + "-" + input.Code;
            }
           var Clientcode = await Task.Run(() => _dbContext.Client.Where(x => x.Code == newcode && x.Id!=input.Id && x.IsDeleted==false).FirstOrDefault());
            
                if (Clientcode == null)
            {
                // OzoneContext ozonedb = new OzoneContext();
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    Client DbClient = null;
                    if (input.Id > 0)
                    {
                        DbClient = await Task.Run(() => _dbContext.Client.Where(x => x.Id == input.Id).FirstOrDefault());
                    }

                    try
                    {
                        //long newid;
                        bool New = false;
                        if (DbClient == null)
                        {
                            New = true;
                            DbClient = new Client();
                        }

                        DbClient.Name = input.Name;
                        DbClient.Address1 = input.Address1;
                        DbClient.Address2 = input.Address2;
                        DbClient.CountryId = input.CountryId;
                        DbClient.CityId = input.CityId;
                        DbClient.StateId = input.StateId;
                        DbClient.PostalCode = input.PostalCode;
                        DbClient.PrefixId = input.PrefixId;
                        DbClient.ContactPerson = input.ContactPerson;
                        if (input.Date != null)
                        {
                            DbClient.Date = Convert.ToDateTime(input.Date);
                        }
                        DbClient.Position = input.Position;
                        DbClient.PhoneNumber = input.PhoneNumber;
                        DbClient.MobileNumber = input.MobileNumber;
                        DbClient.Email = input.Email;
                        DbClient.Website = input.Website;
                        DbClient.Multisite = input.Multisite;
                        DbClient.IsActive = input.IsActive;
                        DbClient.Code = input.Code;
                        DbClient.EacodeId = input.EacodeId;
                        DbClient.NaceCodeId = input.NaceCodeId;
                        DbClient.RiskId = input.RiskId;


                        DbClient.PersonContactNumber = input.PersonContactNumber;
                        DbClient.OrganizationId = input.OrganizationId;
                       // var Agencycode = await Task.Run(() => _dbContext.Organization.Where(x => x.Id == input.OrganizationId).Select(x=>x.Code).FirstOrDefault());
                       






                        if (New == true)
                        {
                            var agcode = Agencycode.Substring(0, 3);

                            DbClient.Code = agcode + "-" + input.Code;
                            DbClient.IsDeleted = false;
                            DbClient.CreationTime = DateTime.Now;
                            DbClient.CreatorUserId = input.CreatorUserId;
                            DbClient.EacodeId = input.EacodeId;
                            DbClient.NaceCodeId = input.NaceCodeId;
                            DbClient.RiskId = input.RiskId;
                            // DbClient.IsDeleted = false;
                            await base.AddAsync(DbClient);
                            Message= "1";
                        }
                        else
                        {
                            //  if (input.IsDeleted == false) { DbClient.IsDeleted = input.IsDeleted; }
                            DbClient.Code= input.Code;
                            DbClient.LastModificationTime = DateTime.Now;
                            DbClient.LastModifierUserId = input.LastModifierUserId;

                            await base.UpdateAsync(DbClient);
                            Message= "2";
                        }
                        await _unitOfWork.SaveChangesAsync();
                        //  for user




                        transaction.Commit();

                        return Message;




                    }
                    catch (Exception ex)
                    {
                        var Exception = ex;
                        transaction.Rollback();
                        return "Not Inserted!";
                    }




                }
            }
            else
            {
                Message = "0";
                return Message; 
            }

        }

        

       public async Task<ClientModel> DownloadClientChange(long id)
        {
           
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ActivityLog activity = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.Id == id).FirstOrDefault());

                ClientModel cli = new ClientModel();
                if (activity != null)
                {
                    cli.FilePath = activity.FilePath;
                    cli.ContentType = activity.ContentType;

                }
                return cli;
            }

        }


        public async Task<ClientSitesModel> ClientSitesdownloadFile(long id)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ActivityLog activity = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.Id == id).FirstOrDefault());

                ClientSitesModel cli = new ClientSitesModel();
                if (activity != null)
                {
                    cli.FilePath = activity.FilePath;
                    cli.ContentType = activity.ContentType;

                }
                return cli;
            }

        }


        public async Task<GetPagedClientModel> GetPagedClient(long orgid,PagedResponseModel model)
        {
            try
            {

                var result = new GetPagedClientModel();
                var ClientList = new List<ClientModel>();

                if (model.AuthAllowed == true)
                {
                    var list = await _dbContext.Client.Where(x => x.IsDeleted == false && x.IsActive == true && x.OrganizationId== orgid &&
                                  (x.Name.ToLower().Contains(model.Keyword.ToLower())
                                )).OrderByDescending(x => x.Id).ToListAsync();
                    ClientList = _mapper.Map<List<ClientModel>>(list);
                  
                }


             
                //  var list = await _productDenominationRepository.GetPagedProductDenominationReponseAsync(model);

                result.ClientModel = GetPage(ClientList, model.Page, model.PageSize);
                result.TotalCount = ClientList.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> ClientDeleteById(long id)
        {
            // OzoneContext ozonedb = new OzoneContext();
            using (var transaction = _unitOfWork.BeginTransaction())
            {


                Client dbresult = _dbContext.Client.Where(u => u.Id == id).FirstOrDefault();
               
             
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
        public async Task<List<UserStandardModel>> GetLeadAuditorByStandardId(long id, long? OrganizationId)
        {


            List<UserStandards> dbdata = new List<UserStandards>();
            var result = new List<UserStandardModel>();
            dbdata = await Task.Run(() => _dbContext.UserStandards.Include(x => x.User).Where(x => x.StandardId == id && x.User.OrganizationId == OrganizationId && x.User.ApprovelStatusId == 2 && x.AuditorTypeId == 2 && x.IsDeleted == false  && x.User.IsDeleted == false).ToListAsync());
            //var LeadAuditordata = await Task.Run(() => _dbContext.UserStandards.Include(x => x.User).Where(x => x.StandardId == id && x.User.OrganizationId == OrganizationId && x.User.ApprovelStatusId == 2 && x.AuditorTypeId == 2 && x.IsDeleted == false).ToListAsync());

            //dbdata.AddRange(LeadAuditordata);
            result = _mapper.Map<List<UserStandardModel>>(dbdata);



            UserStandardModel MM = new UserStandardModel();
            MM.UserId = 0;
            MM.UserName = "--- Not Selected ---";
            result.Add(MM);
            return result.OrderBy(x => x.Id).ToList();
            // return result;
        }
        public async Task<List<UserStandardModel>> GetAuditorByStandardId(long id, long? OrganizationId)
        {


            List<UserStandards> dbdata = new List<UserStandards>();
            var result = new List<UserStandardModel>();
             dbdata = await Task.Run(() => _dbContext.UserStandards.Include(x=>x.User).Where(x=>x.StandardId == id && x.User.OrganizationId==OrganizationId && x.User.ApprovelStatusId==2 && x.AuditorTypeId == 1 && x.IsDeleted==false  && x.User.IsDeleted == false).ToListAsync());
           // var LeadAuditordata = await Task.Run(() => _dbContext.UserStandards.Include(x => x.User).Where(x => x.StandardId == id && x.User.OrganizationId == OrganizationId && x.User.ApprovelStatusId == 2 && x.AuditorTypeId == 2 && x.IsDeleted == false).ToListAsync());

            //dbdata.AddRange(LeadAuditordata);
            result = _mapper.Map<List<UserStandardModel>>(dbdata);



            UserStandardModel MM = new UserStandardModel();
            MM.UserId = 0;
            MM.UserName = "--- Not Selected ---";
            result.Add(MM);
            return result.OrderBy(x => x.Id).ToList();
           // return result;
        }
        public async Task<List<UserStandardModel>> GetReviewerByStandardId(long id)
        {
            var result = new List<UserStandardModel>();
            var data = await Task.Run(() => _dbContext.UserStandards.Include(x => x.User).Where(x => x.StandardId == id && x.User.ApprovelStatusId == 2 && x.AuditorTypeId==4 && x.IsDeleted==false && x.User.IsDeleted==false && x.User.OrganizationId==1).ToListAsync());
            result = _mapper.Map<List<UserStandardModel>>(data);


            //UserStandardModel MM = new UserStandardModel();
            //MM.UserId = 0;
            //MM.UserName = "--- Not Selected ---";
            //result.Add(MM);
            //return result.OrderBy(x => x.Id).ToList();

            return result;
        }
        public async Task<UserStandardModel> GetReviewerByStandard(long? standardId,long? userId)
        {
          
            var result = new UserStandardModel();
            
                var data = await Task.Run(() => _dbContext.UserStandards.Include(x => x.User).Where(x => x.StandardId == standardId && x.UserId == userId && x.User.ApprovelStatusId == 2 && x.AuditorTypeId == 4 && x.IsDeleted==false && x.User.IsDeleted == false).FirstOrDefault());
                if (data != null)
                {
                    result = _mapper.Map<UserStandardModel>(data);
                }




                return result;
            
         
        }

        public Task ApprovedClientChange(long id)
        {
            throw new NotImplementedException();
        }
    }
}
