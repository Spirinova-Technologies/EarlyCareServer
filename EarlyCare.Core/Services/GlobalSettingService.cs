using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class GlobalSettingService : IGlobalSettingService
    {
        private readonly ILogger<GlobalSettingService> _logger;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;

        public GlobalSettingService(ILogger<GlobalSettingService> logger, IGlobalSettingsRepository globalSettingsRepository)
        {
            _logger = logger;
            _globalSettingsRepository = globalSettingsRepository;
        }
        public async Task<List<GlobalSetting>> GetGlobalSetting()
        {
            return await _globalSettingsRepository.GetGlobalSetting();
        }
    }
}
