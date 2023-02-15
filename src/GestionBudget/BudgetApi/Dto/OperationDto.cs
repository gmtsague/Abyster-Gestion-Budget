using DbEntities.Models;

namespace BudgetApi.Dto
{
    public class OperationDto : BaseDto<OperationDto, Operation>
    {
        public int Idoperation { get; set; }
        public double Montant { get; set; }
        public DateTime Dateop { get; set; }
        public bool IsRevenu { get; set; }

        public int UserId { get; set; }
        public int Idcategorie { get; set; }

        public string UserName { get; set; } = null!;
        public string CategorieName { get; set; }= null!;

        public override void AddCustomMappings()
        {
            SetCustomMappings()
                .Map(dest => dest.Dateoperation, src => src.Dateop)
                .Map(dest => dest.Userid, src => src.UserId);

            SetCustomMappingsInverse()
                .Map(dest => dest.CategorieName, src => src.Category.Libelle)
                .Map(dest => dest.UserName, src => $"{src.User.Nom} ({src.User.Prenom})")
                .Map(dest => dest.UserId, src => src.Userid);
        }
    }
}
