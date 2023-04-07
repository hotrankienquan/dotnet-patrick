global using AutoMapper;
using System;
using System.Security.Claims;

namespace dotnet_patrick.Services.CharacterService
{
  
  public class CharacterService : ICharacterService
  {
 

    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
      _mapper = mapper;
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }
    private int GetUserId() {
      return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
  }
    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = _mapper.Map<Character>(newCharacter);
       _context.Characters.Add(character);

      // when add character, u need to assign props User for character model 
      character.User = await _context.Users.FirstOrDefaultAsync(user => user.Id == GetUserId());
      // -- 

      await _context.SaveChangesAsync();
      serviceResponse.Data = await _context.Characters
      .Where(c=>c.User!.Id == GetUserId())
      .Select(c => _mapper.Map<GetCharacterDto>(c))
      .ToListAsync();

      return serviceResponse;


    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
       var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
              // character myself must be delete
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Characters
                .Where(c=>c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
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

      // trong model Character co 1 props la User User, User lai la 1 object
      var dbCharacters = await _context.Characters
          .Where(c=>c.User!.Id == GetUserId())
          .ToListAsync();
      serviceRes.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

      return serviceRes;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      // get single 
      var serviceRes = new ServiceResponse<GetCharacterDto>();
      var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

      serviceRes.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
      return serviceRes;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var serviceRes = new ServiceResponse<GetCharacterDto>();

    try 
    {
        var character = await _context.Characters.Include(c=>c.User).
        FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
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