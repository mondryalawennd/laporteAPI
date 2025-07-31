using LaporteAPI.Persistente.Service;
using LaporteAPI.Persistente.Repository.Interface;
using LaporteAPI.Domain.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LaporteAPI.Tests
{
    public class FuncionarioServiceTests
    {
        private readonly Mock<IGenericRepository<Funcionario>> _mockFuncionarioRepository;
        private readonly FuncionarioService _funcionarioService;

        public FuncionarioServiceTests()
        {
            _mockFuncionarioRepository = new Mock<IGenericRepository<Funcionario>>();

            _funcionarioService = new FuncionarioService(_mockFuncionarioRepository.Object, null);
        }

        [Fact]
        public async Task CadastrarFuncionario_DeveRetornarSucesso_QuandoDadosValidos()
        {
            // Arrange
            var novoFuncionario = new Funcionario
            {
                Nome = "João",
                Sobrenome = "Silva",
                CPF = "12345678900",
                DataNascimento = "1990-01-01",
                Email = "joao.silva@example.com",
                CargoId = 1,
                Senha = "senha123",
                NomeGerente = "Carlos"
            };

            var usuarioCriador = new Funcionario
            {
                Id = 1,
                CargoId = 2 // Cargo superior ao do novo funcionário
            };

            // Configuração do mock para o repositório (não faz nada aqui, só simula o add)
            _mockFuncionarioRepository.Setup(repo => repo.Add(It.IsAny<Funcionario>()))
                                      .ReturnsAsync(novoFuncionario); // Simula a criação do funcionário

            // Act
            var result = await _funcionarioService.CadastrarFuncionario(novoFuncionario, usuarioCriador);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal("Funcionário cadastrado com sucesso.", result.Mensagem);
            Assert.Equal(1, result.FuncionarioId); // Verifica o id retornado (simulado)
        }

        [Fact]
        public async Task CadastrarFuncionario_DeveLancarExcecao_QuandoIdadeInvalida()
        {
            // Arrange
            var novoFuncionario = new Funcionario
            {
                Nome = "João",
                Sobrenome = "Silva",
                CPF = "12345678900",
                DataNascimento = "2010-01-01", // Data de nascimento inválida (menor que 18)
                Email = "joao.silva@example.com",
                CargoId = 1,
                Senha = "senha123",
                NomeGerente = "Carlos"
            };

            var usuarioCriador = new Funcionario
            {
                Id = 1,
                CargoId = 2
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _funcionarioService.CadastrarFuncionario(novoFuncionario, usuarioCriador));
            Assert.Equal("Funcionário deve ser maior de idade.", exception.Message);
        }

        [Fact]
        public async Task CadastrarFuncionario_DeveLancarExcecao_QuandoPermissaoCriacaoInvalida()
        {
            // Arrange
            var novoFuncionario = new Funcionario
            {
                Nome = "João",
                Sobrenome = "Silva",
                CPF = "12345678900",
                DataNascimento = "1990-01-01",
                Email = "joao.silva@example.com",
                CargoId = 3, // Cargo superior ao cargo do criador
                Senha = "senha123",
                NomeGerente = "Carlos"
            };

            var usuarioCriador = new Funcionario
            {
                Id = 1,
                CargoId = 2 // Cargo inferior ao do novo funcionário
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _funcionarioService.CadastrarFuncionario(novoFuncionario, usuarioCriador));
            Assert.Equal("Não é permitido criar usuário com permissões superiores às suas.", exception.Message);
        }
    }
}
