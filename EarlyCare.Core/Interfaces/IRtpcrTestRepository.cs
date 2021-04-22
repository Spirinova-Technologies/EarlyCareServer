using EarlyCare.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IRtpcrTestRepository
    {
        Task<List<RtpcrTest>> GetRtpcrTests(int cityId);
    }
}