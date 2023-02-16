using DbEntities.Models;
using System.ComponentModel.DataAnnotations;

namespace BudgetApi.Dto
{
    public class CategorieDto : BaseDto<CategorieDto, Categorie>
    {
        [Required] 
        public int Idcategorie { get; set; }
        [Required]
        public string Libelle { get; set; } = null!;
    }
}
