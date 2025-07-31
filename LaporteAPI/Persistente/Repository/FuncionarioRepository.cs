using LaporteAPI.Domain.Entities;
using LaporteAPI.Persistente.Data;
using LaporteAPI.Persistente.Repository.Interface;

namespace LaporteAPI.Persistente.Repository
{
    public class FuncionarioRepository : GenericRepository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(DataContext context) : base(context)
        {
        }
    }
}
