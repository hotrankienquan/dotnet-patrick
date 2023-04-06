global using AutoMapper;
using System;


namespace dotnet_patrick.Services.CharacterService
{
  
  public class CharacterService : ICharacterService
  {
 

    private readonly IMapper _mapper;
    private readonly DataContext _context;
    
    public CharacterService(IMapper mapper, DataContext context)
    {
      _mapper = mapper;
      _context = context;
    }
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = _mapper.Map<Character>(newCharacter);
       _context.Characters.Add(character);

      await _context.SaveChangesAsync();
      serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();

      return serviceResponse;


    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
       var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
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
      var serviceRes = new ServiceResponse<List<GetCharacterDto>>();

      var dbCharacters = await _context.Characters.ToListAsync();
      serviceRes.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

      return serviceRes;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      // get single 
      var serviceRes = new ServiceResponse<GetCharacterDto>();
      var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
      serviceRes.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
      return serviceRes;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var serviceRes = new ServiceResponse<GetCharacterDto>();

    try 
    {
        var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
        if(character is null)
        {
          throw new Exception($"character not found");

        }
        character.Name = updatedCharacter.Name;
        character.HitPoints = updatedCharacter.HitPoints;
        character.Strength = updatedCharacter.Strength;
        character.Defense = updatedCharacter.Defense;
        character.Intelligence = updatedCharacter.Intelligence;
        character.Class = updatedCharacter.Class;

        await _context.SaveChangesAsync();
        serviceRes.Data = _mapper.Map<GetCharacterDto>(character);


      }catch(Exception ex)
        {
        serviceRes.Success = false;
        serviceRes.Message = ex.Message;
      }
      return serviceRes;
    }
  }
}