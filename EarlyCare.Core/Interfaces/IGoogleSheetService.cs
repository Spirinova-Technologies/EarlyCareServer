using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IGoogleSheetService
    {
        Task<IList<IList<object>>> GetGoogleSheetData();
    }
}