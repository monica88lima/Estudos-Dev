using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Filmes.Model
{
    public class Genero
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do gênero é obrigatório.")]
        public string Nome { get; set; }
        [JsonIgnore]
        public List<Filme>? Filmes { get; set; } 
        public DateTime DateCriacao { get; set; } = DateTime.Now;
        public DateTime? DateAtualizacao { get; set; } = null;
    }
}
