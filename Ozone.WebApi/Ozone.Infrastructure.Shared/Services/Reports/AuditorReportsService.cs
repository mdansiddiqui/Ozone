using AutoMapper;
using Ozone.Application;
using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Setup;
using Ozone.Application.Interfaces;
using Ozone.Application.Interfaces.Setup;
using Ozone.Application.Repository;
using Ozone.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ozone.Application.DTOs.Projects;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Ozone.Application.DTOs.Reports;

namespace Ozone.Infrastructure.Shared.Services
{
     public class AuditorReportsService :GenericRepositoryAsync<ClientAuditVisit>, IAuditorReportsService
    {
        private readonly OzoneContext _dbContext;
        private readonly IMapper _mapper;
        private IUserSessionHelper _userSession;
        private readonly IUnitOfWork _unitOfWork;
   
        public AuditorReportsService(
        IUnitOfWork unitOfWork,
        OzoneContext dbContext,
        IMapper mapper,
        IUserSessionHelper userSession) : base(dbContext)
        {
            this._unitOfWork = unitOfWork;
            _dbContext = dbContext;
            this._mapper = mapper;
            this._userSession = userSession;
        }


        //private List<LeadAuditorReportsModel> GetPage(List<LeadAuditorReportsModel> list, int page, int pageSize)
        //{
        //    return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //}

        //public async Task<GetPagedLeadAuditorReportsModel> GetPagedLeadAuditorReportsModel(IDictionary<string, string> keyValuePairs)
        //{
        //    try
        //    {
        //        var result = new GetPagedLeadAuditorReportsModel();
        //        var leadAuditorModel = new List<LeadAuditorReportsModel>();
        //        var auditList = new List<ClientAuditVisitModel>();
        //        var auditReportMSModel = new List<AuditReportMSModel>();
        //        var AVGTAT_Auditor = new List<SPAuditorReportsModel>();

            
        //        string fromdate = null;
        //        string todate = null;
        //        string OrganizationId = null;
        //        DateTime frmdate = Convert.ToDateTime(fromdate);
        //        if (keyValuePairs.ContainsKey("fromdate"))
        //            fromdate = keyValuePairs["fromdate"].Split("T")[0];
        //        if (keyValuePairs.ContainsKey("todate"))
        //            todate = keyValuePairs["todate"];
        //        if (keyValuePairs.ContainsKey("OrganizationId"))
        //            OrganizationId = keyValuePairs["OrganizationId"];
        //        long? OrgId;
        //        if (OrganizationId != null && OrganizationId != "" && OrganizationId != string.Empty)
        //        {
        //            OrgId = Convert.ToInt64(OrganizationId);


        //        }
        //        var ClientAuditVisit = await Task.Run(() => _dbContext.ClientAuditVisit.Include(x => x.LeadAuditor).Where(x => x.VisitStatusId == 2).ToList());
        //        auditList = _mapper.Map<List<ClientAuditVisitModel>>(ClientAuditVisit);

        //        var query2 = auditList

        //                  .GroupBy(x => new { x.LeadAuditorId, x.LeadAuditorName })
        //                  .Select(group => new { Peo = group.Key, Count = group.Count() }).ToList();


               
        //        var auditReportMaster = await Task.Run(() => _dbContext.AuditReportMaster.Include(x => x.ClientAuditVisit).Where(x => x.ClientAuditVisit.VisitStatusId == 2).ToList());
        //        auditReportMSModel = _mapper.Map<List<AuditReportMSModel>>(auditReportMaster);

        //        var NCSAuditList = (from m in auditReportMSModel
        //                            group m by m.LeadAuditorId into g
        //                            select new
        //                            {
        //                                LeadAuditorId = g.Key,
        //                                NCS = g.Sum(i => i.Major + i.Minor + i.Critical + i.TimeBound)
        //                            }).ToList();


        //        AVGTAT_Auditor = await AuditorReports(keyValuePairs);



        //        foreach (var Audits in query2)
        //        {
        //            LeadAuditorReportsModel leadAuditorReportsModel = new LeadAuditorReportsModel();
        //            leadAuditorReportsModel.NoOfAudits = Audits.Count;
        //            leadAuditorReportsModel.LeadAuditorId = Audits.Peo.LeadAuditorId;
        //            leadAuditorReportsModel.Id = Audits.Peo.LeadAuditorId;
        //            leadAuditorReportsModel.LeadAuditorName = Audits.Peo.LeadAuditorName;
        //            var auditorNcs = NCSAuditList.Where(x => x.LeadAuditorId == Audits.Peo.LeadAuditorId).FirstOrDefault();
        //            if (auditorNcs != null)
        //            {
        //                leadAuditorReportsModel.NoOfNCs = auditorNcs.NCS;
        //                decimal AvgNCS =Convert.ToDecimal(leadAuditorReportsModel.NoOfNCs / leadAuditorReportsModel.NoOfAudits);
        //                leadAuditorReportsModel.AvgNoOfNCs =Math.Round(AvgNCS, 2);
        //            }
        //            else
        //            {
        //                leadAuditorReportsModel.NoOfNCs = 0;
        //                leadAuditorReportsModel.AvgNoOfNCs = 0;
        //            }
        //            SPAuditorReportsModel AvgTat = AVGTAT_Auditor.Where(x => x.LeadAuditorId == Audits.Peo.LeadAuditorId).FirstOrDefault();
        //            if (AvgTat != null)
        //            {

        //                leadAuditorReportsModel.AvgTAT = AvgTat.totalDays;
        //            }
        //            else
        //            {
        //                leadAuditorReportsModel.AvgTAT = 0;
        //                leadAuditorReportsModel.AvgTAT = 0;
        //            }


        //            leadAuditorModel.Add(leadAuditorReportsModel);

        //        }
          
        //        result.LeadAuditorReportsModel = leadAuditorModel;
        //        //result.LeadAuditorReportsModel = GetPage(leadAuditorModel, model.Page, model.PageSize);
        //        result.TotalCount = leadAuditorModel.Count();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        throw ex;

        //    }

        //}

        public async Task<List<SPAuditorReportsModel>> AuditorReports(IDictionary<string, string> keyValuePairs)
        //public async Task<List<SPAuditorReportsModel>> AuditorReports(string fromdate, string todate, string OrganizationId)
        {
            //GetListallpatient();

            try
            {

                string StandardId ="";
                string LeadAuditorId = "";
                List<SPAuditorReportsModel> AVGTAT_Auditor = new List<SPAuditorReportsModel>();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;
                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    
                    command.CommandText = "New_LeadAuditorReport";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                    new SqlParameter("@LeadAuditorId",(LeadAuditorId == "" ? null : LeadAuditorId)),
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {


                                AVGTAT_Auditor.Add(new SPAuditorReportsModel()
                                {
                                    Id = (!(dataReader["Id"] is DBNull)) ? Convert.ToInt64(dataReader["Id"]):0,
                                    LeadAuditorName = (!(dataReader["LeadAuditorName"] is DBNull)) ? dataReader["LeadAuditorName"].ToString() : null,
                                    NoOfAudits = (!(dataReader["NoOfAudits"] is DBNull)) ? Convert.ToInt64(dataReader["NoOfAudits"]) : 0,
                                    NCS = (!(dataReader["NCS"] is DBNull)) ? Convert.ToInt64(dataReader["NCS"]) : 0,
                                    AVGNCS = (!(dataReader["AVGNCS"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["NCS"]) / Convert.ToDecimal(dataReader["NoOfAudits"]),2) : 0,
                                    AvgTAT = (!(dataReader["AvgTAT"] is DBNull)) ? Convert.ToInt64(dataReader["AvgTAT"]) : 0,
                                    AvgTAT_1 = (!(dataReader["AvgTAT_1"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["AvgTAT_1"]) / Convert.ToDecimal(dataReader["NoOfAudits"]),2) : 0,
                                    AvgTAT_2 = (!(dataReader["AvgTAT_2"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["AvgTAT_2"]) / Convert.ToDecimal(dataReader["NoOfAudits"]),2) : 0,
                                    AvgTAT_3 = (!(dataReader["AvgTAT_3"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["AvgTAT_3"]) / Convert.ToDecimal(dataReader["NoOfAudits"]),2) : 0,
                                    AvgTAT_4 = (!(dataReader["AvgTAT_4"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["AvgTAT_4"]) / Convert.ToDecimal(dataReader["NoOfAudits"]),2) : 0,
                                    AvgTAT_5 = (!(dataReader["AvgTAT_5"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["AvgTAT_5"]) / Convert.ToDecimal(dataReader["NoOfAudits"]),2) : 0,
                                }); ;
                            }
                        }
                    }
                }
                return AVGTAT_Auditor;

            }

            catch (Exception ex)
            {
               // throw ex;
                return null;
            }


           
        }

        public async Task<GetPagedClientProjectReportsModel> GetClientProjectReport(IDictionary<string, string> keyValuePairs)
        {
            try
            {

                string StandardId = "";
                string LeadAuditorId = "";
                GetPagedClientProjectReportsModel AVGTAT_Auditor = new GetPagedClientProjectReportsModel();
                string fromdate = null;
                string todate = null;
                string statusId = null;
                string OrganizationId = null;

                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];

                if (keyValuePairs.ContainsKey("StatusId"))
                {
                    if (keyValuePairs["StatusId"] == "0" || keyValuePairs["StatusId"]=="1")
                    {
                        statusId = keyValuePairs["StatusId"];
                    }
                }

                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<ClientProjectReportModel> CPRLIst = new List<ClientProjectReportModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_ClientProjectList";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    new SqlParameter("@StatusId",statusId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                    new SqlParameter("@LeadAuditorId",(LeadAuditorId == "" ? null : LeadAuditorId)),
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();
                    
                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                ClientProjectReportModel CPR = new ClientProjectReportModel();

                                //AVGTAT_Auditor.ClientProjectReportModel.Add(new ClientProjectReportModel()
                                //{
                                CPR.S_No = i++;
                                CPR.StatusCode = (!(dataReader["StatusName"] is DBNull)) ? dataReader["StatusName"].ToString() : null;
                                CPR.Registration_no = (!(dataReader["Registration_no"] is DBNull)) ? dataReader["Registration_no"].ToString() : null;
                                CPR.ClientName = (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null;
                                CPR.Address = (!(dataReader["Address"] is DBNull)) ? dataReader["Address"].ToString() : null;
                                CPR.Scope = (!(dataReader["Scope"] is DBNull)) ? dataReader["Scope"].ToString() : null;
                                CPR.NaceCode = (!(dataReader["NaceCode"] is DBNull)) ? dataReader["NaceCode"].ToString() : null;
                                CPR.CountryName = (!(dataReader["CountryName"] is DBNull)) ? dataReader["CountryName"].ToString() : null;
                                CPRLIst.Add(CPR);
                                //}); ;
                            }
                        }
                    }
                }

                AVGTAT_Auditor.ClientProjectReportModel = CPRLIst;
                AVGTAT_Auditor.TotalCount = AVGTAT_Auditor.ClientProjectReportModel.Count();

                return AVGTAT_Auditor;


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }



        public async Task<GetPagedAuditorListReportModel> GetAuditorListReport(IDictionary<string, string> keyValuePairs)
        {
            try
            {
                string StandardId = "";
                string LeadAuditorId = "";
                GetPagedAuditorListReportModel AVGTAT_Auditor = new GetPagedAuditorListReportModel();
                string fromdate = "2000-01-01";
                fromdate.Split();
                DateTime todate = DateTime.Now;
                string OrganizationId = null;

                // DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate") && keyValuePairs["fromdate"] != "")
                {
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                }

                if (keyValuePairs.ContainsKey("todate") && keyValuePairs["todate"] != "")
                {
                    todate = Convert.ToDateTime(keyValuePairs["todate"]);
                }
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0" && keyValuePairs["OrganizationId"]!="")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0" && keyValuePairs["StandardId"] != "")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<sp_AuditorListReportModel> CPRLIst = new List<sp_AuditorListReportModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_All_AuditorList";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;

                    //SqlCommand sql = new SqlCommand();
                    //List<SqlParameter> lstParams = new List<SqlParameter>();
                    //SqlParameter sqlParam1 = new SqlParameter();
                    //if (fromdate != "")
                    //{
                    //    new SqlParameter("@FromDate", Convert.ToDateTime(fromdate));
                    //    lstParams.Add(sqlParam1);
                    //}
                    //else 
                    //{
                    //    new SqlParameter("@FromDate", null);
                    //}
                    //lstParams.Add(sqlParam1);
                    // sql.Parameters.AddRange(lstParams.ToArray());
                   // DateTime frmdate = Convert.ToDateTime(fromdate);

                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate", Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                sp_AuditorListReportModel CPR = new sp_AuditorListReportModel();

                                //AVGTAT_Auditor.ClientProjectReportModel.Add(new ClientProjectReportModel()
                                //{
                                CPR.S_No = i++;
                                CPR.AuditorLevel = (!(dataReader["AuditorLevel"] is DBNull)) ? dataReader["AuditorLevel"].ToString() : null;
                                CPR.FullName = (!(dataReader["FullName"] is DBNull)) ? dataReader["FullName"].ToString() : null;
                                CPR.Status = (!(dataReader["Status"] is DBNull)) ? dataReader["Status"].ToString() : null;
                                CPR.TypeOfEnrollment = (!(dataReader["TypeOfEnrollment"] is DBNull)) ? dataReader["TypeOfEnrollment"].ToString() : null;
                                CPR.Gender = (!(dataReader["Gender"] is DBNull)) ? dataReader["Gender"].ToString() : null;
                                CPR.NaceApproved = (!(dataReader["NaceApproved"] is DBNull)) ? dataReader["NaceApproved"].ToString() : null;
                                CPR.Country = (!(dataReader["Country"] is DBNull)) ? dataReader["Country"].ToString() : null;
                                CPRLIst.Add(CPR);
                                //}); ;
                            }
                        }
                    }
                }



                AVGTAT_Auditor.sp_AuditorListReportModel = CPRLIst;
                AVGTAT_Auditor.TotalCount = AVGTAT_Auditor.sp_AuditorListReportModel.Count();

                return AVGTAT_Auditor;


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }



        public async Task<GetPagedAuditorScheduleReportModel> GetAuditorScheduleReport(IDictionary<string, string> keyValuePairs)
        {
            try
            {

                string StandardId = "";
                string LeadAuditorId = "";
                GetPagedAuditorScheduleReportModel AVGTAT_Auditor = new GetPagedAuditorScheduleReportModel();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;

                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<sp_AuditorScheduleReportModel> CPRLIst = new List<sp_AuditorScheduleReportModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_AuditorValidationSchedule";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",(fromdate == "" ? null : fromdate)),
                    new SqlParameter("@ToDate",(todate == "" ? null : todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                    new SqlParameter("@LeadAuditorId",(LeadAuditorId == "" ? null : LeadAuditorId)),
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                sp_AuditorScheduleReportModel CPR = new sp_AuditorScheduleReportModel();

                                //AVGTAT_Auditor.ClientProjectReportModel.Add(new ClientProjectReportModel()
                                //{
                                CPR.S_No = i++;
                                CPR.AuditorName = (!(dataReader["AuditorName"] is DBNull)) ? dataReader["AuditorName"].ToString() : null;
                                CPR.AuditorGrade = (!(dataReader["AuditorGrade"] is DBNull)) ? dataReader["AuditorGrade"].ToString() : null;
                                CPR.JoiningDate = (!(dataReader["JoiningDate"] is DBNull)) ? dataReader["JoiningDate"].ToString() : null;
                                CPR.LastValidationDate = (!(dataReader["LastValidationdate"] is DBNull)) ? dataReader["LastValidationdate"].ToString() : null;
                                CPR.NextDueDate = (!(dataReader["NextDueDate"] is DBNull)) ? dataReader["NextDueDate"].ToString() : null;
                            
                                CPRLIst.Add(CPR);
                                //}); ;
                            }
                        }
                    }
                }



                AVGTAT_Auditor.sp_AuditorScheduleReportModel = CPRLIst;
                AVGTAT_Auditor.TotalCount = AVGTAT_Auditor.sp_AuditorScheduleReportModel.Count();

                return AVGTAT_Auditor;


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }


        public async Task<GetPagedScheduleOfAuditReportModel> GetScheduleOfAuditReport(IDictionary<string, string> keyValuePairs)
        {
            try
            {

                string StandardId = "";
                string LeadAuditorId = "";
                GetPagedScheduleOfAuditReportModel SHOfAudit = new GetPagedScheduleOfAuditReportModel();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;

                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<sp_ScheduleOfAuditReportModel> SHALIst = new List<sp_ScheduleOfAuditReportModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_ScheduleOfAudit";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                sp_ScheduleOfAuditReportModel SHA = new sp_ScheduleOfAuditReportModel();

                                //AVGTAT_Auditor.ClientProjectReportModel.Add(new ClientProjectReportModel()
                                //{
                                //CPR.S_No = i++;
                                //CPR.StatusCode = (!(dataReader["StatusCode"] is DBNull)) ? dataReader["StatusCode"].ToString() : null;
                                //CPR.Registration_no = (!(dataReader["Registration_no"] is DBNull)) ? dataReader["Registration_no"].ToString() : null;
                                //CPR.ClientName = (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null;
                                //CPR.Address = (!(dataReader["Address"] is DBNull)) ? dataReader["Address"].ToString() : null;
                                //CPR.Scope = (!(dataReader["Scope"] is DBNull)) ? dataReader["Scope"].ToString() : null;
                                //CPR.NaceCode = (!(dataReader["NaceCode"] is DBNull)) ? dataReader["NaceCode"].ToString() : null;
                                //CPR.CountryName = (!(dataReader["CountryName"] is DBNull)) ? dataReader["CountryName"].ToString() : null;
                                //CPRLIst.Add(CPR);

                                 
                                  SHA.S_No= i++;
                                  SHA.Registration_no = (!(dataReader["Registration_no"] is DBNull)) ? dataReader["Registration_no"].ToString() : null;

                                SHA.ClientName= (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null;

                                SHA.ProjectCode = (!(dataReader["ProjectCode"] is DBNull)) ? dataReader["ProjectCode"].ToString() : null;
                                //SHA.ProjectTypeId =
  
                                 SHA.ProjectType= (!(dataReader["ProjectType"] is DBNull)) ? dataReader["ProjectType"].ToString() : null;
                                SHA.NaceCode = (!(dataReader["NaceCode"] is DBNull)) ? dataReader["NaceCode"].ToString() : null;

                                SHA.Stage_1 = (!(dataReader["Stage_1"] is DBNull)) ? dataReader["Stage_1"].ToString() : null;

                                SHA.Stage_2 = (!(dataReader["Stage_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Stage_2"]) : (DateTime?)null;

                                //SHA.SurveillanceVisitFrequencyId = (!(dataReader["Stage_2"] is DBNull)) ? dataReader["Stage_2"].ToString() : null;

                                SHA.Surveillance_Frequency = (!(dataReader["Surveillance_Frequency"] is DBNull)) ? dataReader["Surveillance_Frequency"].ToString() : null;
                                SHA.Surv_1 = (!(dataReader["Surv_1"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_1"]).AddDays(-1) : (DateTime?)null;

                                SHA.Followup_1 = (!(dataReader["Followup_1"] is DBNull)) ? Convert.ToDateTime(dataReader["Followup_1"]) : (DateTime?)null;

                                if (Convert.ToInt64(dataReader["SurveillanceVisitFrequencyId"]) == 1)
                                {
                                    SHA.Surv_2 = (!(dataReader["Surv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_2"]).AddDays(-2) : (DateTime?)null;
                                }
                                else if (Convert.ToInt64(dataReader["SurveillanceVisitFrequencyId"]) == 2)
                                {
                                    SHA.Surv_2 = (!(dataReader["Surv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_2"]).AddDays(-2) : (DateTime?)null;
                                }
                                SHA.Followup_2 = (!(dataReader["Followup_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Followup_2"]) : (DateTime?)null;

                                SHA.Surv_3 = (!(dataReader["Surv_3"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_3"]).AddDays(-3) : (DateTime?)null;
                                SHA.Surv_4 = (!(dataReader["Surv_4"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_4"]).AddDays(-4) : (DateTime?)null;

                                SHA.Surv_5 = (!(dataReader["Surv_5"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_5"]).AddDays(-5) : (DateTime?)null;
                                SHA.Recertification = (!(dataReader["Recertification"] is DBNull)) ? Convert.ToDateTime(dataReader["Recertification"]).AddDays(-1) : (DateTime?)null;
                                SHA.CertificationIssueDate = (!(dataReader["CertificationIssueDate"] is DBNull)) ? Convert.ToDateTime(dataReader["CertificationIssueDate"]) : (DateTime?)null;

                                SHALIst.Add(SHA);
                                //}); ;
                            }
                        }
                    }
                }



                SHOfAudit.sp_ScheduleOfAuditReportModel = SHALIst;
                SHOfAudit.TotalCount = SHOfAudit.sp_ScheduleOfAuditReportModel.Count();

                return SHOfAudit;


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }


        public async Task<GetPagedImpartialityReviewReportModel> GetImpartialityReviewReport(IDictionary<string, string> keyValuePairs)
        {
            try
            {

                string StandardId = "";
             
                GetPagedImpartialityReviewReportModel AVGTAT_Auditor = new GetPagedImpartialityReviewReportModel();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;

                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<sp_ImpartialityReviewReportModel> CPRLIst = new List<sp_ImpartialityReviewReportModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_ImpartialityReview";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                sp_ImpartialityReviewReportModel CPR = new sp_ImpartialityReviewReportModel();

                                //AVGTAT_Auditor.ClientProjectReportModel.Add(new ClientProjectReportModel()
                                //{
                                CPR.S_No = i++;
                                CPR.ProjectNo = (!(dataReader["ProjectNo"] is DBNull)) ? dataReader["ProjectNo"].ToString() : null;
                                CPR.ClientName = (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null;
                                CPR.Consultant = (!(dataReader["Consultant"] is DBNull)) ? dataReader["Consultant"].ToString() : null;
                                CPR.ConsultancyName = (!(dataReader["ConsultancyName"] is DBNull)) ? dataReader["ConsultancyName"].ToString() : null;
                                CPR.ContarctReview = (!(dataReader["ContarctReview"] is DBNull)) ? dataReader["ContarctReview"].ToString() : null;
                                CPR.AuditTeam = (!(dataReader["AuditTeam"] is DBNull)) ? dataReader["AuditTeam"].ToString() : null;
                                CPR.CertificateDecision = (!(dataReader["CertificateDecision"] is DBNull)) ? dataReader["CertificateDecision"].ToString() : null;
                                CPR.PaymentRecived = (!(dataReader["PaymentRecived"] is DBNull)) ? dataReader["PaymentRecived"].ToString() : null;

                                CPRLIst.Add(CPR);
                                //}); ;
                            }
                        }
                    }
                }



                AVGTAT_Auditor.sp_ImpartialityReviewReportModel = CPRLIst;
                AVGTAT_Auditor.TotalCount = AVGTAT_Auditor.sp_ImpartialityReviewReportModel.Count();

                return AVGTAT_Auditor;


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }

        public async Task<List<AuditReportHistoryModel>> AuditReportHistory(int id)
        {
            var result = new List<AuditReportHistoryModel>();
            var DbAuditReportHistory = await Task.Run(() => _dbContext.AuditReportHistory.Include(x=>x.ApprovalStatus).Include(x=>x.RemarksBy).Where(x => x.ClientAuditVisitId == id).ToList());
            result = _mapper.Map<List<AuditReportHistoryModel>>(DbAuditReportHistory);
            return result;
        }


        public async Task<List<SPAuditorDetailModel>> AuditorReportsDetail(IDictionary<string, string> keyValuePairs)
        //public async Task<List<SPAuditorReportsModel>> AuditorReports(string fromdate, string todate, string OrganizationId)
        {
            //GetListallpatient();

            try
            {

                string StandardId = "";
                string LeadAuditorId = "";
                List<SPAuditorDetailModel> AVGTAT_Auditor = new List<SPAuditorDetailModel>();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;
                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_LeadAuditorReportDetails";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),
                    new SqlParameter("@LeadAuditorId",(LeadAuditorId == "" ? null : LeadAuditorId)),
                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {

                                 //LeadAuditorId bigint ,ProjectCode nvarchar(max),VisitLevel nvarchar(max) ,ProjectId bigint , NCS bigint , tat_1 bigint ,tat_2 bigint ,tat_3 bigint,tat_4 bigint, tat_5 bigint)

                                AVGTAT_Auditor.Add(new SPAuditorDetailModel()
                                {
                                    Id = (!(dataReader["ClientAvditVisitId"] is DBNull)) ? Convert.ToInt64(dataReader["ClientAvditVisitId"]) : 0,
                                    LeadAuditorId = (!(dataReader["LeadAuditorId"] is DBNull)) ? Convert.ToInt64(dataReader["LeadAuditorId"]) : 0,
                                    LeadAuditorName = (!(dataReader["LeadAuditorName"] is DBNull)) ? dataReader["LeadAuditorName"].ToString() : null,
                                    ProjectCode = (!(dataReader["ProjectCode"] is DBNull)) ? dataReader["ProjectCode"].ToString() : null,
                                    VisitLevel = (!(dataReader["VisitLevel"] is DBNull)) ? dataReader["VisitLevel"].ToString() : null,
                                    ClientName = (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null,

                                    NCS = (!(dataReader["NCS"] is DBNull)) ? Convert.ToInt64(dataReader["NCS"]) : 0,
                                   
                                    AvgTAT_1 = (!(dataReader["tat_1"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["tat_1"])) : 0,
                                    AvgTAT_2 = (!(dataReader["tat_2"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["tat_2"])) : 0,
                                    AvgTAT_3 = (!(dataReader["tat_3"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["tat_3"])) : 0,
                                    AvgTAT_4 = (!(dataReader["tat_4"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["tat_4"])) : 0,
                                    AvgTAT_5 = (!(dataReader["tat_5"] is DBNull)) ? Math.Round(Convert.ToDecimal(dataReader["tat_5"])) : 0,
                                }); ;
                            }
                        }
                    }
                }
                return AVGTAT_Auditor;

            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }



        }




        public async Task<List<CertifiedClientModel>> GetScheduleOfAuditWithWindowPeriod(IDictionary<string, string> keyValuePairs)
        {
            try
            {

                string StandardId = "";
                string LeadAuditorId = "";
                CertifiedClientModel SHOfAudit = new CertifiedClientModel();
                string fromdate = null;
                string todate = null;
                string OrganizationId = null;

                DateTime frmdate = Convert.ToDateTime(fromdate);
                if (keyValuePairs.ContainsKey("fromdate"))
                    fromdate = keyValuePairs["fromdate"].Split("T")[0];
                if (keyValuePairs.ContainsKey("todate"))
                    todate = keyValuePairs["todate"];
                if (keyValuePairs.ContainsKey("OrganizationId"))
                {
                    if (keyValuePairs["OrganizationId"] != "0")
                    {
                        OrganizationId = keyValuePairs["OrganizationId"];
                    }
                }
                if (keyValuePairs.ContainsKey("StandardId"))
                {
                    if (keyValuePairs["StandardId"] != "0")
                    {
                        StandardId = keyValuePairs["StandardId"];
                    }
                }
                //long? OrgId;
                //if (OrganizationId != null && OrganizationId !="" && OrganizationId != string.Empty) 
                //{
                //    OrgId = Convert.ToInt64(OrganizationId);


                //}
                List<CertifiedClientModel> SHALIst = new List<CertifiedClientModel>();

                await EnsureConnectionOpenAsync();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "sp_ScheduleOfAudit_with_Windowperiod";
                    //command.CommandText = "sp_AVG_Tat_AuditorReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@FromDate",Convert.ToDateTime(fromdate)),
                    new SqlParameter("@ToDate",Convert.ToDateTime(todate)),
                    //new SqlParameter("@StandardId",StandardId),
                    //new SqlParameter("@LeadAuditorId",LeadAuditorId),
                    
                    
                    new SqlParameter("@StandardId",(StandardId == "" ? null : StandardId)),

                    new SqlParameter("@OrganizationId",(OrganizationId == "" ? null : OrganizationId)),
                    //new SqlParameter("@OrganizationId",OrganizationId),
                    });
                    //  command.Transaction = GetActiveTransaction();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            var i = 1;
                            while (dataReader.Read())
                            {
                                CertifiedClientModel SHA = new CertifiedClientModel();

                              


                                SHA.S_No = i++;
                                SHA.ProjectCode = (!(dataReader["ProjectCode"] is DBNull)) ? dataReader["ProjectCode"].ToString() : null;
                                SHA.ClientName = (!(dataReader["ClientName"] is DBNull)) ? dataReader["ClientName"].ToString() : null;
                                SHA.Country = (!(dataReader["Country"] is DBNull)) ? dataReader["Country"].ToString() : null;
                                SHA.CertificationStatus = (!(dataReader["CertificationStatus"] is DBNull)) ? dataReader["CertificationStatus"].ToString() : null;
                                SHA.currentState = (!(dataReader["currentState"] is DBNull)) ? dataReader["currentState"].ToString() : null;
                                SHA.Registration_no = (!(dataReader["Registration_no"] is DBNull)) ? dataReader["Registration_no"].ToString() : null;
                                SHA.CertificationIssueDate = (!(dataReader["CertificationIssueDate"] is DBNull)) ? Convert.ToDateTime(dataReader["CertificationIssueDate"]) : (DateTime?)null;
                                SHA.CertificationExpiryDate = (!(dataReader["CertificationExpiryDate"] is DBNull)) ? Convert.ToDateTime(dataReader["CertificationExpiryDate"]) : (DateTime?)null;
                                SHA.Surveillance_Frequency = (!(dataReader["Surveillance_Frequency"] is DBNull)) ? dataReader["Surveillance_Frequency"].ToString() : null;
                                SHA.IntimationdateSurv_1 = (!(dataReader["IntimationdateSurv_1"] is DBNull)) ? Convert.ToDateTime(dataReader["IntimationdateSurv_1"]) : (DateTime?)null;
                                SHA.Windowperiod_start_Surv_1 = (!(dataReader["Windowperiod_start_Surv_1"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_start_Surv_1"]) : (DateTime?)null;
                                SHA.Windowperiod_end_Surv_1 = (!(dataReader["Windowperiod_end_Surv_1"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_end_Surv_1"]) : (DateTime?)null;
                                SHA.Surv_1_due = (!(dataReader["Surv_1_due"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_1_due"]) : (DateTime?)null;
                                SHA.Windowperiod_Sart_FUP_1 = (!(dataReader["Windowperiod_Sart_FUP_1"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_Sart_FUP_1"]) : (DateTime?)null;
                                SHA.Windowperiod_end_FUP_1 = (!(dataReader["Windowperiod_end_FUP_1"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_end_FUP_1"]) : (DateTime?)null;

                                SHA.IntimationdateSurv_2 = (!(dataReader["IntimationdateSurv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["IntimationdateSurv_2"]) : (DateTime?)null;

                                SHA.Windowperiod_start_Surv_2 = (!(dataReader["Windowperiod_start_Surv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_start_Surv_2"]) : (DateTime?)null;
                                SHA.Windowperiod_end_Surv_2 = (!(dataReader["Windowperiod_end_Surv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_end_Surv_2"]) : (DateTime?)null;
                                SHA.Surv_2_due = (!(dataReader["Surv_2_due"] is DBNull)) ? Convert.ToDateTime(dataReader["Surv_2_due"]) : (DateTime?)null;
                                SHA.Windowperiod_Sart_FUP_2 = (!(dataReader["Windowperiod_Sart_FUP_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_Sart_FUP_2"]) : (DateTime?)null;
                                SHA.Windowperiod_end_FUP_2 = (!(dataReader["Windowperiod_end_FUP_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Windowperiod_end_FUP_2"]) : (DateTime?)null;

                                SHA.RecertificationIntimationdate = (!(dataReader["RecertificationIntimationdate"] is DBNull)) ? Convert.ToDateTime(dataReader["RecertificationIntimationdate"]) : (DateTime?)null;

                                SHA.Recertification_Windowperiod_start_Surv_2 = (!(dataReader["Recertification_Windowperiod_start_Surv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Recertification_Windowperiod_start_Surv_2"]) : (DateTime?)null;
                                SHA.Recertification_Windowperiod_end_Surv_2 = (!(dataReader["Recertification_Windowperiod_end_Surv_2"] is DBNull)) ? Convert.ToDateTime(dataReader["Recertification_Windowperiod_end_Surv_2"]) : (DateTime?)null;
                                SHA.Followup_Recert_Start = (!(dataReader["Followup_Recert_Start"] is DBNull)) ? Convert.ToDateTime(dataReader["Followup_Recert_Start"]) : (DateTime?)null;
                                SHA.Followup_Recert_end = (!(dataReader["Followup_Recert_end"] is DBNull)) ? Convert.ToDateTime(dataReader["Followup_Recert_end"]) : (DateTime?)null;



                                SHALIst.Add(SHA);
                                //}); ;
                            }
                        }
                    }
                }



               
               

                return SHALIst;


            }

            catch (Exception ex)
            {
                // throw ex;
                return null;
            }

        }


        private async Task EnsureConnectionOpenAsync()
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }
        //private DbTransaction GetActiveTransaction()
        //{
        //    return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
        //    {
        //        {"ContextType", typeof(CoherentDbContext) },
        //        {"MultiTenancySide",MultiTenancySides.Host }

        //    });
        //}
    }
}
