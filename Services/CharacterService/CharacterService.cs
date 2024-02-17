using PatrickGodAPI.Dtos.Character;

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

        public CharacterService(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var myCharacter = mapper.Map<Character>(character);
            //myCharacter.Id = characters.Max(x => x.Id) + 1;
            context.Characters.Add(myCharacter);
            await context.SaveChangesAsync();
            serviceResponse.Data = await context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = await context.Characters.FirstOrDefaultAsync(
                    c => c.Id == id
                    ) ?? throw new Exception($"Character with id: '{id}' not found.");

                context.Characters.Remove( character );
                await context.SaveChangesAsync();

                //character.Name = updatedCharacter.Name;
                //character.Strength = updatedCharacter.Strength;
                //character.Defence = updatedCharacter.Defence;
                //character.HitPoints = updatedCharacter.HitPoints;
                //character.Intelligence = updatedCharacter.Intelligence;
                //character.Class = updatedCharacter.Class;

                serviceResponse.Data = await context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
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
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await context.Characters.FirstOrDefaultAsync(
                    c => c.Id == updatedCharacter.Id
                    ) ?? throw new Exception($"Character with id: '{updatedCharacter.Id}' not found.");

                mapper.Map(updatedCharacter, character);
                //character.Name = updatedCharacter.Name;
                //character.Strength = updatedCharacter.Strength;
                //character.Defence = updatedCharacter.Defence;
                //character.HitPoints = updatedCharacter.HitPoints;
                //character.Intelligence = updatedCharacter.Intelligence;
                //character.Class = updatedCharacter.Class;

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
    }
}
