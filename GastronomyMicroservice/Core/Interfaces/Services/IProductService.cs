using System.Collections.Generic;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IProductService
    {
        public object Get(int espId);
    }
}
