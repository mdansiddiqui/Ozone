﻿using Ozone.Application.DTOs;
using Ozone.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces
{
    public interface IDashboardService
    {

        Task<ActivityLogModel> ProjectChangeRequestDownloadfile(long id);

        Task<ActivityLogModel> oldValuesDownloadfile(long id);

        Task<ActivityLogModel> NewValuesDownloadfile(long id);

    }
}