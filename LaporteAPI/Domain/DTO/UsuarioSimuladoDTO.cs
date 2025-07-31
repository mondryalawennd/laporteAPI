using LaporteAPI.Domain.Enum;

namespace LaporteAPI.Domain.DTO
{
    public class UsuarioSimuladoDTO
    {
        public string Nome { get; set; } = "Maria Julia";
        public string Sobrenome { get; set; } = "Diogenes";
        public string Email { get; set; } = "mjdiogenes@empresa.com";
        public string CPF { get; set; } = "05765212598";
       // public DateTime DataNascimento { get; set; } = new DateTime(1990, 1, 1);
        public string DataNascimento { get; set; } = "01/01/1994";
        public string Senha { get; set; } = "Senha123!";
        public int Cargo { get; set; } = (int)Hierarquia.Gerente;
    }
}
