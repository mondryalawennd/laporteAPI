using LaporteAPI.Domain.Entities;
using LaporteAPI.Domain.Enum;
using LaporteAPI.Domain.Model;
using LaporteAPI.Persistente.Data;
using LaporteAPI.Persistente.Repository.Interface;
using LaporteAPI.Persistente.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace LaporteAPI.Persistente.Service
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly PasswordHasher<Funcionario> _passwordHasher = new();
        private readonly IGenericRepository<Funcionario> _funcionarioRepository;
        protected readonly DataContext _context;

        public FuncionarioService(IGenericRepository<Funcionario> funcionarioRepository, DataContext context)
        {
            _context = context;
            _funcionarioRepository = funcionarioRepository;
        }

        public async Task<(bool Sucesso, string Mensagem, int FuncionarioId)> CadastrarFuncionario(Funcionario novoFuncionario, Funcionario usuarioCriador)
        {
            if (!ValidarIdade(Convert.ToDateTime(novoFuncionario.DataNascimento)))
                throw new ArgumentException("Funcionário deve ser maior de idade.");

            ValidarPermissaoCriacao(novoFuncionario.CargoId, usuarioCriador.CargoId);

            novoFuncionario.Senha = HashSenha(novoFuncionario, novoFuncionario.Senha);


            await _funcionarioRepository.Add(novoFuncionario);

            return (true, "Funcionário cadastrado com sucesso.", 1);
        }


        private bool ValidarIdade(DateTime dataNascimento)
        {
            var idade = DateTime.Today.Year - dataNascimento.Year;
            if (dataNascimento > DateTime.Today.AddYears(-idade)) idade--;
            return idade >= 18;
        }

        public void ValidarPermissaoCriacao(int permissaoNovoFuncionario, int permissaoFuncionarioCriador)
        {
            if (permissaoNovoFuncionario > permissaoFuncionarioCriador)
            {
                throw new InvalidOperationException("Não é permitido criar usuário com permissões superiores às suas.");
            }
        }

        public string HashSenha(Funcionario funcionario, string senha)
        {
            return _passwordHasher.HashPassword(funcionario, senha);
        }

        public async Task<Funcionario> Add(Funcionario Funcionario)
        {
            return await _funcionarioRepository.Add(Funcionario);
        }

        public async Task<Funcionario> GetEntityById(int id)
        {
            return await _funcionarioRepository.GetEntityById(id);
        }

        public async Task<IEnumerable<Funcionario>> GetAll()
        {
            return await _funcionarioRepository.List();
        }


        public async Task Update(Funcionario Funcionario)
        {
            await _funcionarioRepository.Update(Funcionario);
        }

        public async Task Delete(int id)
        {
            await _funcionarioRepository.Delete(id);
        }

        
    }
}
