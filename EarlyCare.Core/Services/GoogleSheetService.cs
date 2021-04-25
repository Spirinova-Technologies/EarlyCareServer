using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class GoogleSheetService : IGoogleSheetService
    {
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private static readonly string ApplicationName = "Earlycare";

        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleSheetService> _logger;
        private readonly IGoogleSheetRepository _googleSheetRepository;
        private readonly IHospitalRepository _hospitalRepository;

        public GoogleSheetService(ILogger<GoogleSheetService> logger, IGoogleSheetRepository googleSheetRepository,
            IConfiguration configuration, IHospitalRepository hospitalRepository)
        {
            _logger = logger;
            _googleSheetRepository = googleSheetRepository;
            _configuration = configuration;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<IList<IList<object>>> GetGoogleSheetData()
        {
            var sheets = await _googleSheetRepository.GetGoogleSheets();

            var cities = await _hospitalRepository.GetCities();

            foreach (var sheet in sheets)
            {
                IList<IList<object>> values = GetSheetData(sheet.Name.ToString(), sheet.ToRange, "");
                switch (sheet.Name)
                {
                    case GoogleSheetNames.AmbulanceProvider:
                        break;

                    case GoogleSheetNames.COVIDConsultancyDoc:
                        break;

                    case GoogleSheetNames.FoodPackets:
                        break;

                    case GoogleSheetNames.HospitalsBeds:
                        await ProcessHospitalRecordsAsync(values, cities);
                        break;

                    case GoogleSheetNames.MedicalEquipment:
                        break;

                    case GoogleSheetNames.MedicalOxygenProvider:
                        break;

                    case GoogleSheetNames.PlasmaDonors:
                        break;
                }
            }

            return null;
        }

        private async Task ProcessHospitalRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            if (values != null && values.Count > 0)
            {
                foreach (var row in values.Skip(1))
                {
                    try
                    {
                        var existingRecord = await _hospitalRepository.GetHospitalByName(row[0].ToString());

                        if (existingRecord == null)
                        {
                            //insert
                            var hospital = new Hospital
                            {
                                Name = row[0].ToString(),
                                HospitalType = row[1].ToString(),
                                PhoneNumber1 = row[2].ToString(),
                                PhoneNumber2 = row[3].ToString(),
                                IsAvailableIsolationBed = row[4].ToString().Trim() == "Available" ? true : false,
                                IsAvailableOxygen = row[5].ToString().Trim() == "Available" ? true : false,
                                IsAvailableICU = row[6].ToString().Trim() == "Available" ? true : false,
                                CityId = cities.First(x => x.Name.Trim() == row[7].ToString()).Id,
                                Address = row[8].ToString(),
                                UpdatedBy = 0,
                                CreatedBy = 0,
                                Latitude = "0",
                                Longitude = "0"
                            };

                            _hospitalRepository.InsertHospital(hospital);
                        }
                        else
                        {
                            existingRecord.Name = row[0].ToString();
                            existingRecord.HospitalType = row[1].ToString();
                            existingRecord.PhoneNumber1 = row[2].ToString();
                            existingRecord.PhoneNumber2 = row[3].ToString();
                            existingRecord.IsAvailableIsolationBed = row[4].ToString().Trim() == "Available" ? true : false;
                            existingRecord.IsAvailableOxygen = row[5].ToString().Trim() == "Available" ? true : false;
                            existingRecord.IsAvailableICU = row[6].ToString().Trim() == "Available" ? true : false;
                            existingRecord.CityId = cities.First(x => x.Name.Trim() == row[7].ToString()).Id;
                            existingRecord.Address = row[8].ToString();
                            existingRecord.UpdatedBy = 0;
                            existingRecord.CreatedBy = 0;
                            existingRecord.Latitude = "0";
                            existingRecord.Longitude = "0";

                            _hospitalRepository.UpdateHospital(existingRecord);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                        _logger.LogError(" ", ex);
                    }
                }
            }
            else
            {
                _logger.LogInformation("No data found for hospital");
            }
        }

        private static SheetsService GetServcie()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        private IList<IList<object>> GetSheetData(string sheetName, string rangeTo, string spreadsheetId)
        {
            SheetsService service = GetServcie();

            var range = $"{sheetName}!A:{rangeTo}";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(_configuration["Google:SpreadsheetId"], range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            return values;
        }
    }
}