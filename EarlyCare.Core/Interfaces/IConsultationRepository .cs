using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IConsultationRepository
    {
        Task<List<ConsultationResponseModel>> GetConsultations(int cityId);
        Task<Consultation> InsertConsultation(Consultation consultation);
        Task<Consultation> UpdateConsultation(Consultation consultation);
        Task<Consultation> GetConsultationDetails(string name);
    }
}