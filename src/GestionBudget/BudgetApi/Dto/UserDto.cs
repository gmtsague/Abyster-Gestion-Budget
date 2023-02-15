using DbEntities.Models;
using Mapster;

namespace BudgetApi.Dto
{
    public class UserDto : BaseDto<UserDto, User>
    {
        public int UserId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; }
        public DateTime Lastconnexion { get; set; }
        public bool Isactive { get; set; } = false;

        public string UserName { get; set; }

        public List<OperationDto> Operations { get; set; } = new List<OperationDto>();

        public override void AddCustomMappings()
        {
            SetCustomMappings()
                .Map(dest => dest.Operations, src => src.Operations)
                .Map(dest => dest.Userid, src => src.UserId);

            SetCustomMappingsInverse()
                .Map(dest => dest.Operations, src => src.Operations)
                .Map(dest => dest.UserId, src => src.Userid);
        }
    }
}
