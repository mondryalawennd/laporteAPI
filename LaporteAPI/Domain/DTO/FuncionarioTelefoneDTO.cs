using LaporteAPI.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LaporteAPI.Domain.DTO
{
    public class FuncionarioTelefoneDTO
    {
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; }

        public int FuncionarioID { get; set; }
      
    }
}
