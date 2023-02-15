using DbEntities.Models;

namespace BudgetApi.Dto
{
    public class CategorieDto : BaseDto<CategorieDto, Categorie>
    {
        public int Idcategorie { get; set; }
        public string Libelle { get; set; } = null!;
    }
}
