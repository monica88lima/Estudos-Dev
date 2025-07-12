using System.ComponentModel.DataAnnotations;

namespace Filmes.Model.Enum
{
    public enum EClassificacao
    {
        [Display(Name = "Livre")]
        Livre = 0,

        [Display(Name = "10 anos")]
        DezAnos = 10,

        [Display(Name = "12 anos")]
        DozeAnos = 12,

        [Display(Name = "14 anos")]
        QuatorzeAnos = 14,

        [Display(Name = "16 anos")]
        DezesseisAnos = 16,

        [Display(Name = "18 anos")]
        DezoitoAnos = 18


    }
}
