using EarlyCare.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetCategories();
        Task<List<Service>> GetServices();
    }
}