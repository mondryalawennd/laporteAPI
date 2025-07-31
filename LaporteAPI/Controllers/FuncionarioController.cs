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

    }
}
