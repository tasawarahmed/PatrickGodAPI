using PatrickGodAPI.Dtos.Character;
using System.Security.Claims;

namespace PatrickGodAPI.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        //private static List<Character> characters = new List<Character>
        //{
        //    new Character(),
        //    new Character{ Id = 1, Name = "Sam"}
        //};

        private readonly IMapper mapper;
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor contextAccessor)
        {
            this.mapper = mapper;
            this.context = context;
            this.httpContextAccessor = contextAccessor;
        }

        private int GetUserId()
        {
            return int.Parse(httpContextAccessor.HttpContext!.User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var myCharacter = mapper.Map<Character>(character);
            //myCharacter.Id = characters.Max(x => x.Id) + 1;
            myCharacter.User = await context.Users.FirstOrDefaultAsync(x => x.Id == GetUserId());
            context.Characters.Add(myCharacter);
            await context.SaveChangesAsync();
            serviceResponse.Data = await context.Characters
                .Where(c => c.User!.Id ==  GetUserId())
                .Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = await context.Characters.FirstOrDefaultAsync(
                    c => c.Id == id && c.User!.Id == GetUserId()
                    ) ?? throw new Exception($"Character with id: '{id}' not found.");

                context.Characters.Remove( character );
                await context.SaveChangesAsync();

                //character.Name = updatedCharacter.Name;
                //character.Strength = updatedCharacter.Strength;
                //character.Defence = updatedCharacter.Defence;
                //character.HitPoints = updatedCharacter.HitPoints;
                //character.Intelligence = updatedCharacter.Intelligence;
                //character.Class = updatedCharacter.Class;

                serviceResponse.Data = await context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            int userId = GetUserId();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User!.Id == userId).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(
                    c => c.Id == updatedCharacter.Id);

                if (character is null || character.User!.Id != GetUserId())
                {
                    throw new Exception($"Character with id: '{updatedCharacter.Id}' not found.");
                }

                mapper.Map(updatedCharacter, character);

                await context.SaveChangesAsync();
                serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto characterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == characterSkill.CharacterId &&
                    c.User!.Id == GetUserId());

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var skill = await context.Skills
                    .FirstOrDefaultAsync(s => s.Id == characterSkill.SkillId);

                if (skill is null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                }

                character.Skills!.Add(skill);
                await context.SaveChangesAsync();
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