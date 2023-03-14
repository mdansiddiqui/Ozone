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

    public class GetPagedProjectStatusModel
    {
        public List<ProjectDashboardStatusModel> ProjectDashboardStatusModel { get; set; }
        public List<ProjectRequestDashboardStatusModel> ProjectRequestDashboardStatusModel { get; set; }

        public List<ClientSitetRequestDashboardStatusModel> ClientSitetRequestDashboardStatusModel { get; set; }

        public List<ContractDashboardStatusModel> ContractDashboardStatusModel { get; set; }

        public List<AuditsDashboardStatusModel> AuditsDashboardStatusModel { get; set; }

        public List<ClientDashboardStatusModel> ClientDashboardStatusModel { get; set; }

    }
}
