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


namespace Ozone.Infrastructure.Shared.Services.MasterSetups
{
    public class DashboardService : GenericRepositoryAsync<DashboardService>, IDashboardService
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public DashboardService(
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


        public async Task<ActivityLogModel> ProjectChangeRequestDownloadfile(long id)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ActivityLog activity = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.Id == id).FirstOrDefault());

                ActivityLogModel cli = new ActivityLogModel();
                if (activity != null)
                {
                    cli.FilePath = activity.FilePath;
                    cli.ContentType = activity.ContentType;

                }
                return cli;
            }

        }



        public async Task<ActivityLogModel> oldValuesDownloadfile(long id)
        {
           

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                ActivityLog activitylog = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.Id == id).FirstOrDefault());
                ActivityLogModel cli = new ActivityLogModel();

                if (activitylog.FormId == 22)
                {
                         ProjectSA8000Model Dbs8000 = new ProjectSA8000Model();
                         var  s8000Result = await Task.Run(() => _dbContext.ProjectSa8000.Include(x=>x.ClientProject).Where(x => x.Id == activitylog.TableRowId).FirstOrDefault());
                         Dbs8000 = _mapper.Map<ProjectSA8000Model>(s8000Result);
                         if (Dbs8000 != null)
                         {
                              cli.FilePath = Dbs8000.ContractFilePath;
                              cli.ContentType = Dbs8000.ContractFileContentType;

                         }
                }

                if (activitylog.FormId == 10037)
                {
                    ProjectSLCPModel Dbslcp = new ProjectSLCPModel();
                    var slcpResult = await Task.Run(() => _dbContext.ProjectSlcp.Include(x => x.ClientProject).Where(x => x.Id == activitylog.TableRowId).FirstOrDefault());
                    Dbslcp = _mapper.Map<ProjectSLCPModel>(slcpResult);
                    if (Dbslcp != null)
                    {
                        cli.FilePath = Dbslcp.ContractFilePath;
                        cli.ContentType = Dbslcp.ContractFileContentType;

                    }
                }


                if (activitylog.FormId == 37)
                {
                    ProjectHiggModel Dbhigg = new ProjectHiggModel();
                    var higgResult = await Task.Run(() => _dbContext.ProjectHigg.Include(x => x.ClientProject).Where(x => x.Id == activitylog.TableRowId).FirstOrDefault());
                    Dbhigg = _mapper.Map<ProjectHiggModel>(higgResult);
                    if (Dbhigg != null)
                    {
                        cli.FilePath = Dbhigg.ContractFilePath;
                        cli.ContentType = Dbhigg.ContractFileContentType;

                    }
                }

                return cli;

            }

        }

        public async Task<ActivityLogModel> NewValuesDownloadfile(long id)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                
                ActivityLog activitylog = await Task.Run(() => _dbContext.ActivityLog.Where(x => x.Id == id).FirstOrDefault());

         
                string fullfilepath =activitylog.NewValues;
                var a=fullfilepath.Split("_ContentType_");
              var filepath=  a[0].Split("FilePath_");


                ActivityLogModel cli = new ActivityLogModel();
                if (activitylog != null)
                {
                    cli.FilePath =  filepath[1];
                    cli.ContentType = a[1];

                }
                return cli;
            }

        }

    }
}
