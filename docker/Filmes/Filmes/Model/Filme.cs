using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Filmes.Model.Enum;

namespace Filmes.Model
{
    public class Filme
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título do filme deve ter no máximo 100 caracteres.")]
        public string TituloFilme  { get; set; }
        [Required]
        public int  idGenero { get; set; }
        public string Sinopse { get; set; }
        [Range(1, 600, ErrorMessage = "A duração do filme deve ser entre 1 e 600 minutos.")]
        [Required(ErrorMessage = "A duração do filme é obrigatória.")]
        public int Duracao { get; set; }
        [Required(ErrorMessage = "A classificação é obrigatória.")]
        [EnumDataType(typeof(EClassificacao))]

        public EClassificacao Classificacao { get; set; }
        public DateTime DateCriacao { get; set; } = DateTime.Now;
        public DateTime? DateAtualizacao { get; set; } = null;
        [JsonIgnore]
        public Genero? Genero { get; set; }
    }

  
}
