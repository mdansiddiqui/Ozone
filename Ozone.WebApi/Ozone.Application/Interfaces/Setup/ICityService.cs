using Ozone.Application.DTOs;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozone.Application.Interfaces.Setup
{
   public interface ICityService
    {
        Task<string> Create(CityModel input);
        Task<GetPagedCityModel> GetPagedCityResponse(PagedResponseModel model);
        Task<CityModel> GetCityBYId(long id);
        Task<string> CityDeleteById(long id);

    }
}
