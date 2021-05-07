using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IPlasmaRepository _plasmaRepository;
        private readonly IOxygenProviderRepository _oxygenProviderRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IDrugsRepository _drugsRepository;

        public GoogleSheetService(ILogger<GoogleSheetService> logger, IGoogleSheetRepository googleSheetRepository,
            IConfiguration configuration, IHospitalRepository hospitalRepository, IPlasmaRepository plasmaRepository,
            IOxygenProviderRepository oxygenProviderRepository, IFoodRepository foodRepository, IAmbulanceRepository ambulanceRepository,
            IConsultationRepository consultationRepository, IDrugsRepository drugsRepository)
        {
            _logger = logger;
            _googleSheetRepository = googleSheetRepository;
            _configuration = configuration;
            _hospitalRepository = hospitalRepository;
            _plasmaRepository = plasmaRepository;
            _oxygenProviderRepository = oxygenProviderRepository;
            _foodRepository = foodRepository;
            _ambulanceRepository = ambulanceRepository;
            _consultationRepository = consultationRepository;
            _drugsRepository = drugsRepository;
        }

        public async Task<bool> GetGoogleSheetData(GoogleSheetRequestModel googleSheetRequestModel)
        {
            var sheet = await _googleSheetRepository.GetGoogleSheetByName(googleSheetRequestModel.SheetName);

            var cities = await _hospitalRepository.GetCities();

            IList<IList<object>> values = GetSheetData(sheet.Name.ToString(), sheet.ToRange, "");

            if (values == null || values.Count == 0)
                return true;

            switch (sheet.Name)
            {
                case GoogleSheetNames.AmbulanceProvider:
                    await ProcessAmbulanceRecordsAsync(values, cities);
                    break;

                case GoogleSheetNames.COVIDConsultancyDoc:
                    await ProcessConsultationRecordsAsync(values, cities);
                    break;

                case GoogleSheetNames.FoodPackets:
                    await ProcessFoodRecordsAsync(values, cities);
                    break;

                case GoogleSheetNames.HospitalsBeds:
                    await ProcessHospitalRecordsAsync(values, cities);
                    break;

                case GoogleSheetNames.MedicalEquipment:
                    break;

                case GoogleSheetNames.OxygenProvider:
                    await ProcessOxyegnProviderRecordsAsync(values, cities);
                    break;

                case GoogleSheetNames.PlasmaDonors:
                    await ProcessPlasmaDonorRecordsAsync(values, cities);
                    break;

                case GoogleSheetNames.Remdesivir:
                    await ProcessDrugsRecordsAsync(values, cities);
                    break;
            }

            return true;
        }

        private async Task ProcessHospitalRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _hospitalRepository.GetHospitalByName(row[0].ToString());

                    var time = row[8].ToString().Length == 7 ? "0" + row[8].ToString() : row[8];
                    DateTime.TryParseExact($"{row[7]} {time}", "dd-MM-yyyy HH:mm:ss",
                                                  new CultureInfo("en-IN"),
                                                  DateTimeStyles.None, out DateTime date);

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
                            CityId = cities.First(x => x.Name.Trim() == row[9].ToString()).Id,
                            Address = row[10].ToString(),
                            ModifiedAt = date,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                            Latitude = "0",
                            Longitude = "0"
                        };

                        await _hospitalRepository.InsertHospital(hospital);
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
                        existingRecord.Address = row[10].ToString();
                        existingRecord.ModifiedAt = date;
                        existingRecord.UpdatedBy = 0;
                        existingRecord.CreatedBy = 0;
                        existingRecord.Latitude = "0";
                        existingRecord.Longitude = "0";

                        await _hospitalRepository.UpdateHospital(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
            }
        }

        private async Task ProcessPlasmaDonorRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _plasmaRepository.GetPlasmaDonorByName(row[0].ToString());

                    if (existingRecord == null)
                    {
                        //insert
                        var plasma = new Plasma
                        {
                            Name = row[0].ToString(),
                            PhoneNumber = row[1].ToString(),
                            Address = row[2].ToString(),
                            BloodGroup = row[3].ToString(),
                            IsRtpcrReportAvailable = row[4].ToString().Trim() == "NA" ? false : true,
                            CovidPositiveDate = row[5].ToString() == "NA" ? default : Convert.ToDateTime(row[5].ToString()),
                            CovidNegativeDate = row[6].ToString() == "NA" ? default : Convert.ToDateTime(row[6].ToString()),
                            IsAntibodyReportAvailable = row[7].ToString().Trim() == "NA" ? false : true,
                            DonorType = row[8].ToString(),
                            CityId = cities.First(x => x.Name.Trim() == row[9].ToString()).Id,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                            IsVerified = true,
                        };

                        await _plasmaRepository.InsertPlasma(plasma);
                    }
                    else
                    {
                        existingRecord.Name = row[0].ToString();
                        existingRecord.PhoneNumber = row[1].ToString();
                        existingRecord.Address = row[2].ToString();
                        existingRecord.BloodGroup = row[3].ToString();
                        existingRecord.IsRtpcrReportAvailable = row[4].ToString().Trim() == "NA" ? false : true;
                        existingRecord.CovidPositiveDate = row[5].ToString() == "NA" ? default : Convert.ToDateTime(row[5].ToString());
                        existingRecord.CovidNegativeDate = row[6].ToString() == "NA" ? default : Convert.ToDateTime(row[6].ToString());
                        existingRecord.IsAntibodyReportAvailable = row[7].ToString().Trim() == "NA" ? false : true;
                        existingRecord.DonorType = row[8].ToString();
                        existingRecord.CityId = cities.First(x => x.Name.Trim() == row[9].ToString()).Id;
                        existingRecord.UpdatedOn = DateTime.Now;
                        existingRecord.UpdatedBy = 0;

                        await _plasmaRepository.UpdatePlasma(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
            }
        }

        private async Task ProcessOxyegnProviderRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _oxygenProviderRepository.GetOxygenProviderByName(row[0].ToString());

                    if (existingRecord == null)
                    {
                        //insert
                        var oxygenProvider = new OxygenProvider
                        {
                            Name = row[0].ToString(),
                            PhoneNumber = row[1].ToString(),
                            Area = row[2].ToString(),
                            Charges = row[4].ToString() == "NA" ? string.Empty : row[4].ToString(),
                            Type = row[5].ToString(),
                            GovRegistraionNumber = row[6].ToString() == "NA" ? string.Empty : row[6].ToString(),
                            CityId = cities.First(x => x.Name.Trim() == row[3].ToString()).Id,
                            IsVerified = true,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                        };

                        await _oxygenProviderRepository.InsertOxygenProvider(oxygenProvider);
                    }
                    else
                    {
                        existingRecord.Name = row[0].ToString();
                        existingRecord.PhoneNumber = row[1].ToString();
                        existingRecord.Area = row[2].ToString();
                        existingRecord.Charges = row[4].ToString() == "NA" ? string.Empty : row[4].ToString();
                        existingRecord.Type = row[5].ToString();
                        existingRecord.GovRegistraionNumber = row[6].ToString() == "NA" ? string.Empty : row[6].ToString();
                        existingRecord.CityId = cities.First(x => x.Name.Trim() == row[3].ToString()).Id;
                        existingRecord.IsVerified = true;
                        existingRecord.UpdatedOn = DateTime.Now;
                        existingRecord.UpdatedBy = 0;

                        await _oxygenProviderRepository.UpdateOxygenProvider(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
            }
        }

        private async Task ProcessConsultationRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _consultationRepository.GetConsultationDetails(row[0].ToString());

                    if (existingRecord == null)
                    {
                        //insert
                        var consultation = new Consultation
                        {
                            DoctorName = row[0].ToString(),
                            PhoneNumber = row[1].ToString(),
                            Area = row[2].ToString(),
                            Charges = row[3].ToString() == "NIL" ? string.Empty : row[3].ToString(),
                            Type = row[4].ToString(),
                            GovRegistraionNumber = row[5].ToString() == "NA" ? string.Empty : row[5].ToString(),
                            CityId = cities.First(x => x.Name.Trim() == row[6].ToString()).Id,
                            IsVerified = true,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                        };

                        await _consultationRepository.InsertConsultation(consultation);
                    }
                    else
                    {
                        existingRecord.DoctorName = row[0].ToString();
                        existingRecord.PhoneNumber = row[1].ToString();
                        existingRecord.Area = row[2].ToString();
                        existingRecord.Charges = row[3].ToString() == "NIL" ? string.Empty : row[3].ToString();
                        existingRecord.Type = row[4].ToString();
                        existingRecord.GovRegistraionNumber = row[5].ToString() == "NA" ? string.Empty : row[5].ToString();
                        existingRecord.CityId = cities.First(x => x.Name.Trim() == row[6].ToString()).Id;
                        existingRecord.IsVerified = true;
                        existingRecord.CreatedOn = DateTime.Now;
                        existingRecord.UpdatedOn = DateTime.Now;
                        existingRecord.UpdatedBy = 0;
                        existingRecord.CreatedBy = 0;

                        await _consultationRepository.UpdateConsultation(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
            }
        }

        private async Task ProcessDrugsRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _drugsRepository.GetDrugDetails(row[0].ToString());

                    if (existingRecord == null)
                    {
                        //insert
                        var drug = new Drug
                        {
                            SupplierName = row[0].ToString(),
                            PhoneNumber = row[1].ToString(),
                            Address = row[2].ToString(),
                            Coordinator = row[4].ToString() == "NA" ? string.Empty : row[4].ToString(),
                            GovPhoneNumber = row[5].ToString() == "NA" ? string.Empty : row[5].ToString(),
                            CityId = cities.First(x => x.Name.Trim() == row[3].ToString()).Id,
                            IsVerified = true,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                        };

                        await _drugsRepository.InsertDrugs(drug);
                    }
                    else
                    {
                        existingRecord.SupplierName = row[0].ToString();
                        existingRecord.PhoneNumber = row[1].ToString();
                        existingRecord.Address = row[2].ToString();
                        existingRecord.Coordinator = row[4].ToString() == "NA" ? string.Empty : row[4].ToString();
                        existingRecord.GovPhoneNumber = row[5].ToString() == "NA" ? string.Empty : row[5].ToString();
                        existingRecord.CityId = cities.First(x => x.Name.Trim() == row[3].ToString()).Id;
                        existingRecord.IsVerified = true;
                        existingRecord.CreatedOn = DateTime.Now;
                        existingRecord.UpdatedOn = DateTime.Now;
                        existingRecord.UpdatedBy = 0;
                        existingRecord.CreatedBy = 0;

                        await _drugsRepository.UpdateDrugs(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
            }
        }

        private async Task ProcessFoodRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _foodRepository.GetFoodByName(row[0].ToString());

                    if (existingRecord == null)
                    {
                        //insert
                        var food = new Food
                        {
                            Name = row[0].ToString(),
                            PhoneNumber = row[1].ToString(),
                            Address = row[2].ToString(),
                            Area = row[3].ToString(),
                            Delivery = row[4].ToString(),
                            FoodServed = row[5].ToString(),
                            Charges = row[6].ToString() == "NA" ? string.Empty : row[6].ToString(),
                            Type = row[7].ToString(),
                            RegistrationNumber = row[8].ToString() == "NA" ? string.Empty : row[8].ToString(),
                            CityId = cities.First(x => x.Name.Trim() == row[9].ToString()).Id,
                            IsVerified = true,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                        };

                        await _foodRepository.InsertFood(food);
                    }
                    else
                    {
                        existingRecord.Name = row[0].ToString();
                        existingRecord.PhoneNumber = row[1].ToString();
                        existingRecord.Address = row[2].ToString();
                        existingRecord.Area = row[3].ToString();
                        existingRecord.Delivery = row[4].ToString();
                        existingRecord.FoodServed = row[5].ToString();
                        existingRecord.Charges = row[6].ToString() == "NA" ? string.Empty : row[6].ToString();
                        existingRecord.Type = row[7].ToString();
                        existingRecord.RegistrationNumber = row[8].ToString() == "NA" ? string.Empty : row[8].ToString();
                        existingRecord.CityId = cities.First(x => x.Name.Trim() == row[9].ToString()).Id;
                        existingRecord.IsVerified = true;
                        existingRecord.UpdatedOn = DateTime.Now;
                        existingRecord.UpdatedBy = 0;

                        await _foodRepository.UpdateFood(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
            }
        }

        private async Task ProcessAmbulanceRecordsAsync(IList<IList<object>> values, List<City> cities)
        {
            foreach (var row in values.Skip(1))
            {
                try
                {
                    var existingRecord = await _ambulanceRepository.GetAmbulanceDetails(row[0].ToString(), row[1].ToString());

                    if (existingRecord == null)
                    {
                        //insert
                        var ambulance = new Ambulance
                        {
                            Name = row[0].ToString(),
                            PhoneNumber = row[1].ToString(),
                            Address = row[2].ToString(),
                            AmbulanceType = row[3].ToString(),
                            NumberOfAmbulances = row[4].ToString(),
                            Area = row[5].ToString(),
                            ProviderType = row[6].ToString(),
                            CityId = cities.First(x => x.Name.Trim() == row[7].ToString()).Id,
                            IsVerified = true,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = 0,
                            CreatedBy = 0,
                        };

                        await _ambulanceRepository.InsertAmbulance(ambulance);
                    }
                    else
                    {
                        existingRecord.Name = row[0].ToString();
                        existingRecord.PhoneNumber = row[1].ToString();
                        existingRecord.Address = row[2].ToString();
                        existingRecord.AmbulanceType = row[3].ToString();
                        existingRecord.NumberOfAmbulances = row[4].ToString();
                        existingRecord.Area = row[5].ToString();
                        existingRecord.ProviderType = row[6].ToString();
                        existingRecord.CityId = cities.First(x => x.Name.Trim() == row[7].ToString()).Id;
                        existingRecord.IsVerified = true;
                        existingRecord.CreatedOn = DateTime.Now;
                        existingRecord.UpdatedOn = DateTime.Now;
                        existingRecord.UpdatedBy = 0;
                        existingRecord.CreatedBy = 0;

                        await _ambulanceRepository.UpdateAmbulance(existingRecord);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured while inserting record {JsonConvert.SerializeObject(row)} ");
                    _logger.LogError(" ", ex);
                }
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