using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ozone.Application;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using Ozone.Application.Interfaces;
using Ozone.Application.Interfaces.Setup;
using Ozone.Application.Repository;
using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Infrastructure.Shared.Services.MasterSetups
{
    public class SubClauseService : GenericRepositoryAsync<SubClause>, ISubClause
    {

        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;

        public SubClauseService(
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

        public async Task<string> Create(SubClauseModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                SubClause DbSubClause = await Task.Run(() => _dbContext.SubClause.Where(x => x.IsDeleted == false && x.Id == input.Id).FirstOrDefault());

                try
                {
                    long newid;
                    bool New = false;
                    if (DbSubClause == null)
                    {
                        New = true;
                        DbSubClause = new SubClause();
                    }
                    DbSubClause.Id = input.Id;
                    DbSubClause.StandardId = input.StandardId;
                    DbSubClause.MainClauseId = input.MainClauseId;
                    DbSubClause.SubClauseName = input.SubClauseName;
                    DbSubClause.Requirement = input.Requirement;

                    if (New == true)
                    {
                        DbSubClause.IsDeleted = false;
                        await base.AddAsync(DbSubClause);
                    }
                    else
                    {
                        await base.UpdateAsync(DbSubClause);
                    }

                    var result = await _unitOfWork.SaveChangesAsync();

                    transaction.Commit();
                    return "Successfully Saved";
                }
                catch
                {
                    transaction.Rollback();
                    return "Not Inserted";
                }


            }

            return null;
        }


        private List<SubClauseModel> GetPage(List<SubClauseModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedSubClauseModel> GetPagedSubClause(PagedResponseModel model)
        {
            try
            {
                var result = new GetPagedSubClauseModel();
                var productDenoList = new List<SubClauseModel>();

                var list = await Task.Run(() => _dbContext.SubClause.Include(x => x.Standard).Include(x => x.MainClause).Where(x => x.IsDeleted == false).ToList());
                productDenoList = _mapper.Map<List<SubClauseModel>>(list);

                result.SubClauseModel = GetPage(productDenoList, model.Page, model.PageSize);
                result.TotalCount = productDenoList.Count;
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> SubClauseDeleteById(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                SubClause dbSubClause = _dbContext.SubClause.Where(x => x.Id == id).FirstOrDefault();
                if (dbSubClause != null)
                {
                    try
                    {
                        dbSubClause.IsDeleted = true;
                        await base.UpdateAsync(dbSubClause);
                        await _unitOfWork.SaveChangesAsync();

                        transaction.Commit();
                        return "Successfully Deleted!";
                    }
                    catch
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
        }

        public async Task<List<MainClauseModel>> GetAllClauseByStandard(long StandardId)
        {
            var result = new List<MainClauseModel>();
            var list = await Task.Run(() => _dbContext.MainClause.Where(x => x.IsDeleted == false && x.StandardId == StandardId).ToList());
            result = _mapper.Map<List<MainClauseModel>>(list);
            return result;
        }
        public async Task<List<MainClauseModel>> GetAllMainClaus()
        {
            var result = new List<MainClauseModel>();
            var list = await Task.Run(() => _dbContext.MainClause.Where(x => x.IsDeleted == false).ToList());
            result = _mapper.Map<List<MainClauseModel>>(list);
            return result;
        }



    }
}
