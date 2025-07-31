using LaporteAPI.Domain.Entities;
using LaporteAPI.Persistente.Data;
using LaporteAPI.Persistente.Repository.Interface;

namespace LaporteAPI.Persistente.Repository
{
    public class FuncionarioTelefoneRepository : GenericRepository<FuncionarioTelefone>, IFuncionarioTelefoneRepository
    {
        public FuncionarioTelefoneRepository(DataContext context) : base(context)
        {
        }
    }
}

