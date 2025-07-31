using LaporteAPI.Domain.Entities;
using LaporteAPI.Persistente.Repository;
using LaporteAPI.Persistente.Repository.Interface;
using LaporteAPI.Persistente.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LaporteAPI.Persistente.Service
{
    public class FuncionarioTelefonesService: IFuncionarioTelefoneService
    {
        private readonly IGenericRepository<FuncionarioTelefone> _telefoneRepository;
        private readonly IGenericRepository<Funcionario> _funcionarioRepository;

        public FuncionarioTelefonesService(
            IGenericRepository<FuncionarioTelefone> telefoneRepo,
            IGenericRepository<Funcionario> funcionarioRepo)
        {
            _telefoneRepository = telefoneRepo;
            _funcionarioRepository = funcionarioRepo;
        }
        public async Task AddTelefoneFuncionario(List<FuncionarioTelefone> listaTelefone)
        {
            var funcionarioId = listaTelefone.FirstOrDefault().FuncionarioID;
            var funcionarioEntity = await _funcionarioRepository.GetEntityById(funcionarioId);

            if (funcionarioEntity == null)
                throw new Exception("Funcionário não encontrado");

            var telefones = listaTelefone.Select(p => new FuncionarioTelefone
            {
                FuncionarioID = funcionarioId,
                Numero = p.Numero
            });

            await _telefoneRepository.AddRangeAsync(telefones);
        }

       
    }
}
