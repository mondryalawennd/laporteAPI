using LaporteAPI.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LaporteAPI.Domain.Entities
{
    public class Funcionario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sobrenome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string DataNascimento { get; set; }

        [Required]
        public int CargoId { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }

        public string NomeGerente { get; set; }
        public ICollection<FuncionarioTelefone> Telefones { get; set; }
    }
}
