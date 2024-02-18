using PatrickGodAPI.Dtos.Weapon;
using System.Security.Claims;

namespace PatrickGodAPI.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        public WeaponService(DataContext context, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            this.dataContext = context;
            this.httpContextAccessor = contextAccessor;
            this.mapper = mapper;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await dataContext.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                    c.User!.Id == int.Parse(httpContextAccessor.HttpContext!.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!));
                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                dataContext.Weapons.Add(weapon);
                await dataContext.SaveChangesAsync();

                response.Data = mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
