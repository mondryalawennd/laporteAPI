using LaporteAPI.Domain.DTO;
using LaporteAPI.Domain.Entities;
using LaporteAPI.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace LaporteAPI.Domain.Model
{
    public class FuncionarioDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sobrenome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string DataNascimento { get; set; }


        [Required]
        public Hierarquia NivelHierarquia { get; set; }
        public string NomeGerente { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }

        public List<FuncionarioTelefoneDTO> Telefones { get; set; } = new();
    }
}
