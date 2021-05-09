using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IPlasmaRepository
    {
        Task<List<PlasmaResponseModel>> GetPlasmas(int cityId, bool hasApprovePermission);

        Task<Plasma> InsertPlasma(Plasma plasma);
        Task<Plasma> UpdatePlasma(Plasma plasma);
        Task<Plasma> GetPlasmaDonorByName(string name);
        Task<Plasma> GetPlasmaByUserId(int userId);
        Task DeleteSyncedPlasmaDonorDetails();
        Task<Plasma> UpdateVerificationStatus(UpdateVerificationStatusModel plasmaStatusModel);
    }
}
