using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IMedicalEquipmentRepository
    {
        Task<List<MedicalEquipmentResponse>> GetMedicalEquipments(int cityId);
        Task DeleteSyncedMedicalEquipmentDetails();
    }
}