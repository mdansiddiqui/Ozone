﻿using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //int SaveChanges();
        Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<int> SaveChangesCheckerApprovalAsync();

    }
}
