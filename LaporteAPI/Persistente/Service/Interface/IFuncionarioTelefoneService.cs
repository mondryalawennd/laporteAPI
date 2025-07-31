using LaporteAPI.Domain.Entities;

namespace LaporteAPI.Persistente.Service.Interface
{
    public interface IFuncionarioTelefoneService
    {
        Task AddTelefoneFuncionario(List<FuncionarioTelefone> listaTelefone);
    }
}
