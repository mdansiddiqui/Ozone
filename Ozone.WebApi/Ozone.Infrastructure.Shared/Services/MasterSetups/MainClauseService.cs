using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ozone.Application;
using Ozone.Application.DTOs;

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
    public class MainClauseService : GenericRepositoryAsync<MainClause>, IMainClause
    {
        private readonly OzoneContext _dbContext;
        //  private readonly DbSet<Library> _user;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        // private IDataShapeHelper<Library> _dataShaper;
        private readonly IUnitOfWork _unitOfWork;

        public MainClauseService(
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

        public async Task<string> Create(MainClauseModel input)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                MainClause DbMainClause = await Task.Run(() => _dbContext.MainClause.Where(x => x.IsDeleted == false && x.Id == input.Id).FirstOrDefault());

                try
                {
                    long newid;
                    bool New = false;
                    if (DbMainClause == null)
                    {
                        New = true;
                        DbMainClause = new MainClause();
                    }
                    DbMainClause.Id = input.Id;
                    DbMainClause.StandardId = input.StandardId;
                    DbMainClause.MainClauseName = input.MainClauseName;
                    DbMainClause.Heading = input.Heading;
                    DbMainClause.Requirement = input.Requirement;

                    if (New == true)
                    {
                        DbMainClause.IsDeleted = false;
                        await base.AddAsync(DbMainClause);
                    }
                    else
                    {
                        await base.UpdateAsync(DbMainClause);
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
        private List<MainClauseModel> GetPage(List<MainClauseModel> list, int page, int pageSize)
        {
            return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<GetPagedMainClauseModel> GetPagedMainClause(PagedResponseModel model)
        {
            try
            {
                var result = new GetPagedMainClauseModel();
                var productDenoList = new List<MainClauseModel>();

                var list = await Task.Run(() => _dbContext.MainClause.Include(x => x.Standard).Where(x => x.IsDeleted == false).ToList());
                productDenoList = _mapper.Map<List<MainClauseModel>>(list);

                result.MainClauseModel = GetPage(productDenoList, model.Page, model.PageSize);
                result.TotalCount = productDenoList.Count;
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> MainClauseDeleteById(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                MainClause dbMainClause = _dbContext.MainClause.Where(x => x.Id == id).FirstOrDefault();
                if (dbMainClause != null)
                {
                    try
                    {
                        dbMainClause.IsDeleted = true;
                        await base.UpdateAsync(dbMainClause);
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

    }
}
