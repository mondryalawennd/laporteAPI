using LaporteAPI.Domain.Entities;

namespace LaporteAPI.Persistente.Service.Interface
{
    public interface IFuncionarioService
    {
        Task<Funcionario> Add(Funcionario Funcionario);
        Task<Funcionario> GetEntityById(int id);
        Task<IEnumerable<Funcionario>> GetAll();
        Task Update(Funcionario Funcionario);
        Task Delete(int id);
        Task<(bool Sucesso, string Mensagem, int FuncionarioId)> CadastrarFuncionario(Funcionario novoFuncionario, Funcionario usuarioCriador);
    }
}
