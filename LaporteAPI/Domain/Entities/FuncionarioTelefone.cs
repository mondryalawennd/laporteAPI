using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LaporteAPI.Domain.Entities
{
    public class FuncionarioTelefone
    {
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; }

        public int FuncionarioID { get; set; }
       
        public Funcionario Funcionario { get; set; }
    }
}
