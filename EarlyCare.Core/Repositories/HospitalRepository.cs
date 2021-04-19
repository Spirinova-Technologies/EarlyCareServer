using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class HospitalRepository : ConnectionRepository, IHospitalRepository
    {
        public HospitalRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<BedCountDetails> GetBedCounts(int cityId)
        {
            var query = @"SELECT SUM(VacantIsolation) AS VacantIsolationBeds , SUM(VacantWithOxygen) AS VacantWithOxygenBeds
                            , SUM(VacantWithICU) AS VacantWithICUBeds, SUM(VacantWithICUVentilator) AS VacantWithICUVentilatorBeds,
                            SUM(TotalIsolation) AS TotalIsolationBeds, SUM(TotalWithOxygen) AS TotalWithOxygenBeds,
                            SUM(TotalWithICU) AS TotalWithICUBeds, SUM(TotalWithICUVentilator) AS TotalWithICUVentilatorBeds FROM Hospitals
                         where CityId = @cityId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<BedCountDetails>(query, new
                {
                    cityId
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<City>> GetCities()
        {
            var query = @"SELECT * from Cities where IsActive = 1 ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<City>(query);

                return result.ToList();
            }
        }

        public async Task<List<Hospital>> SearchHospitals(HospitalFilterModel hospitalFilters)
        {
            string whereCluase = string.Empty;

            switch (hospitalFilters.BedType)
            {
                case 1:
                    whereCluase = " AND h.IsAvailableIsolationBed = 1";
                    break;
                case 2:
                    whereCluase = "AND h.IsAvailableICU = 1"; 
                    break;
                case 3:
                    whereCluase = "AND h.IsAvailableOxygen = 1";
                    break;
                case 4:
                    whereCluase = "AND h.IsAvailableICUVentilator = 1";
                    break;
                default:
                    break;
            }


            var query = $@"select h.* , c.Name as City  from Hospitals h
                          join Cities c on c.id = h.CityId  where CityId = @cityId {whereCluase}";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Hospital>(query, new {
                    cityId= hospitalFilters.CityId
                });

                return result.ToList();
            }
        }


    }
}
