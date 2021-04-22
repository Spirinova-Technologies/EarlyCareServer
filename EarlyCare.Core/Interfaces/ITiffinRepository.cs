using EarlyCare.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface ITiffinRepository
    {
        Task<List<Tiffin>> GetTiffins(int cityId);
    }
}
