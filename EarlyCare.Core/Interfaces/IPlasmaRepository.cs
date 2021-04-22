﻿using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IPlasmaRepository
    {
        Task<List<PlasmaResponseModel>> GetPlasmas(int cityId);
    }
}
