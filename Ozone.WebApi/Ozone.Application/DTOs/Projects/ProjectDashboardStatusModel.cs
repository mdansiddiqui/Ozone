using Ozone.Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozone.Application.DTOs
{
    public class ProjectDashboardStatusModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }
    public class ContractDashboardStatusModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }
    public class AuditsDashboardStatusModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }

    public class ClientDashboardStatusModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }

    public class ProjectRequestDashboardStatusModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }

    public class ClientSitetRequestDashboardStatusModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
    }
    public class windiwPeriodModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Count { get; set; }
        

    }
    public class GetPagedProjectStatusModel
    {
        public List<ProjectDashboardStatusModel> ProjectDashboardStatusModel { get; set; }
        public List<ProjectRequestDashboardStatusModel> ProjectRequestDashboardStatusModel { get; set; }

        public List<ClientSitetRequestDashboardStatusModel> ClientSitetRequestDashboardStatusModel { get; set; }

        public List<ContractDashboardStatusModel> ContractDashboardStatusModel { get; set; }

        public List<AuditsDashboardStatusModel> AuditsDashboardStatusModel { get; set; }

        public List<ClientDashboardStatusModel> ClientDashboardStatusModel { get; set; }
        public List<windiwPeriodModel> WindiwPeriodModel { get; set; }
        public List<CertifiedClientModel> CertifiedClientModel { get; set; }


        public List<CertifiedClientModel> IntimationdateSurv_1 { get; set; }
        public List<CertifiedClientModel> Windowperiod_start_Surv_1 { get; set; }
        public List<CertifiedClientModel> Windowperiod_Start_FUP_1 { get; set; }
        public List<CertifiedClientModel> IntimationdateSurv_2 { get; set; }
        public List<CertifiedClientModel> Windowperiod_start_Surv_2 { get; set; }
        public List<CertifiedClientModel> Windowperiod_Sart_FUP_2 { get; set; }
        public List<CertifiedClientModel> Recertification_Windowperiod_start_Surv_2 { get; set; }
        public List<CertifiedClientModel> Followup_Recert_Start { get; set; }
        

    }
}
