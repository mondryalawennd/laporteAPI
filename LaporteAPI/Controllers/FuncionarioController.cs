using AutoMapper;
using LaporteAPI.Domain.DTO;
using LaporteAPI.Domain.Entities;
using LaporteAPI.Domain.Enum;
using LaporteAPI.Domain.Model;
using LaporteAPI.Persistente.Data;
using LaporteAPI.Persistente.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LaporteAPI.Controllers
{
    [Route("api/[controller]")]
    public class FuncionarioController: ControllerBase 
    {
        public readonly IFuncionarioService _funcionarioService;
        public readonly IFuncionarioTelefoneService _funcionarioTelefoneService;
        private readonly ILogger<FuncionarioController> _logger;

        public FuncionarioController(ILogger<FuncionarioController> logger,IFuncionarioService funcionarioService, IFuncionarioTelefoneService funcionarioTelefoneService )
        {
            this._logger = logger;
            this._funcionarioService = funcionarioService;
            this._funcionarioTelefoneService = funcionarioTelefoneService;
        }

        [HttpGet("GetFuncionarioPorId/{id}")]
        public async Task<IActionResult> GetFuncionarioPorId( int Id)
        {
            var funcionario = await _funcionarioService.GetEntityById(Id);

            if (funcionario == null)
                return NotFound();

            return Ok(funcionario);
        }


        [HttpGet("GetAllFuncionarios")]
        public async Task<IActionResult> GetAllFuncionarios()
        {
            var funcionario = await _funcionarioService.GetAll();
        
            if (funcionario == null)
                return NotFound();

            return Ok(funcionario);
        }



        [HttpPost("CadastrarFuncionario")]
        public async Task<IActionResult> CadastrarFuncionario([FromBody] FuncionarioDTO funcionarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Requisição para obter todos os funcionários iniciada.");

            var usuarioSimuladoDTO = new UsuarioSimuladoDTO();

            var funcionario = new Funcionario()
            { 
               Nome = funcionarioDTO.Nome,
               Sobrenome = funcionarioDTO.Sobrenome,
               CPF = funcionarioDTO.CPF,
               DataNascimento = funcionarioDTO.DataNascimento,
               Email = funcionarioDTO.Email,
               CargoId = (int)funcionarioDTO.NivelHierarquia,
                Senha = funcionarioDTO.Senha,
               NomeGerente = funcionarioDTO.NomeGerente,             
               Telefones = funcionarioDTO.Telefones.Select(t => new FuncionarioTelefone
                {
                    Numero = t.Numero
                }).ToList(),
              
            };

            //USUARIO SIMULADO: simulando um usário logado no sistema. 
            var funcionarioCriador = new Funcionario()
            {
                Nome = usuarioSimuladoDTO.Nome,
                Sobrenome = usuarioSimuladoDTO.Sobrenome,
                CPF = usuarioSimuladoDTO.CPF,
                DataNascimento = usuarioSimuladoDTO.DataNascimento,
                Email = usuarioSimuladoDTO.Email,
                CargoId = usuarioSimuladoDTO.Cargo,
                Senha = usuarioSimuladoDTO.Senha
            };

            try
            {
                var funcionarioCriado = await _funcionarioService.CadastrarFuncionario(funcionario, funcionarioCriador);
                _logger.LogInformation("Funcionário criado com sucesso. ID: {Id}", funcionarioCriado.FuncionarioId);
                return CreatedAtAction("CadastrarFuncionario", new { id = funcionarioCriado.FuncionarioId }, funcionarioCriado);

            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpDelete("DeleteFuncionario/{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
             await _funcionarioService.Delete(id);

            return NoContent();  
        }

        [HttpPut("atualizarFuncionario/{id}")]
        public async Task<IActionResult> AtualizarFuncionario(int id, [FromBody] FuncionarioDTO funcionarioDTO)
        {
            // Verifica se o ID passado no corpo da requisição corresponde ao ID da URL
            if (id != funcionarioDTO.Id)
            {
                return BadRequest("O ID do funcionário não corresponde.");
            }

            var funcionario = await _funcionarioService.GetEntityById(id);
            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

          
            funcionario.Nome = funcionarioDTO.Nome;
            funcionario.Sobrenome = funcionarioDTO.Sobrenome;
            funcionario.CPF = funcionarioDTO.CPF;
            funcionario.DataNascimento = funcionarioDTO.DataNascimento;
            funcionario.Email = funcionarioDTO.Email;
            funcionario.NomeGerente = funcionarioDTO.NomeGerente;
            //funcionario.CargoId = funcionarioDTO.CargoId;
            funcionario.Senha = funcionarioDTO.Senha;

            try
            {
                // Chama o serviço para atualizar o funcionário no banco de dados
                await _funcionarioService.Update(funcionario);

                return Ok(funcionario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar o funcionário: {ex.Message}");
            }
        }


    }
}
