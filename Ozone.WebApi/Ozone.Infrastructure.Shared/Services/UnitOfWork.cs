﻿//using COI.Application.Interfaces;
using Ozone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Ozone.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ozone.Infrastructure.Persistence.Models;

namespace Ozone.Infrastructure.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private OzoneContext _context;
        public UnitOfWork(
            OzoneContext context
            )
        {
            _context = context;
        }
        //public int SaveChanges()
        //{
        //    return _context.SaveChanges();
        //}

        //public async Task<int> SaveChangesCheckerApprovalAsync()
        //{
        //    return await _context.SaveChangesCheckerApprovalAsync();
        //}
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public Task<int> SaveChangesCheckerApprovalAsync()
        {
            throw new NotImplementedException();
        }
    }
}
