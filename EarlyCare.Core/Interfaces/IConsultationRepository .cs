using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IConsultationRepository
    {
        Task<List<ConsultationResponseModel>> GetConsultations(int cityId);
    }
}