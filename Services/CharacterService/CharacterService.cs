global using AutoMapper;
using System;


namespace dotnet_patrick.Services.CharacterService
{
  
  public class CharacterService : ICharacterService
  {
    private List<Character> characters = new List<Character>()
    {
        new Character(),
        new Character {Id = 1, Name = "Sam"}
    };

    private readonly IMapper _mapper;
    
    public CharacterService(IMapper mapper)
    {
        this._mapper = mapper;
    }
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

      var character = _mapper.Map<Character>(newCharacter);

        // because Add character dto do not have field ID,
        // so i add character.Id and Map to Character model

      character.Id = this.characters.Max(c => c.Id) + 1;
      // data structure List have method Add()
      characters.Add(character);
      // Map from c(is Character) to GetCharacterDto to return json
      serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

      return serviceResponse;


    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
       var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = characters.FirstOrDefault(c => c.Id == id);
                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                characters.Remove(character);

                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
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

      serviceRes.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
      return serviceRes;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      // get single 
      var serviceRes = new ServiceResponse<GetCharacterDto>();
      var character = characters.FirstOrDefault(c => c.Id == id);
      serviceRes.Data = _mapper.Map<GetCharacterDto>(character);
      return serviceRes;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var serviceRes = new ServiceResponse<GetCharacterDto>();

    try 
    {
        var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
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