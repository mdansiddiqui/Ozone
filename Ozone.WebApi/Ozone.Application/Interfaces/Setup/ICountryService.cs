﻿using Ozone.Application.DTOs;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
   public interface ICountryService
    {
        Task<string> Create(CountryModel input);
        Task<GetPagedCountryModel> GetPagedCountryResponse(PagedResponseModel model);
        Task<CountryModel> GetCountryBYId(long id);
        Task<string> CountryDeleteById(long id);

    }
}
