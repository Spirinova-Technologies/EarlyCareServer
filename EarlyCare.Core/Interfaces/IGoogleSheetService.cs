﻿using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IGoogleSheetService
    {
        Task<bool> GetGoogleSheetData(GoogleSheetRequestModel googleSheetRequestModel);
    }
}