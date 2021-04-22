using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class PlasmaRepository : ConnectionRepository, IPlasmaRepository
    {
        public PlasmaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<PlasmaResponseModel>> GetPlasmas(int cityId)
        {
            var query = @"select p.Id, p.Name,p.Address,p.BloodGroup,p.PhoneNumber,p.IsRtpcrReportAvailable,p.IsAntibodyReportAvailable,
                          p.CovidPositiveDate, p.CovidNegativeDate, p.UpdatedOn, u.FullName as UpdatedBy, d.Name as DonorType,  c.Name as City from PlasmaDonor p
                          join User u on u.id = p.UpdatedBy
                          join Cities c on c.id = p.CityId
                          join DonerTypes d on d.id = p.DonorType
                          where p.CityId = @cityId and p.IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<PlasmaResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
