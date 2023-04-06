using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_patrick.Dtos.Character;

namespace dotnet_patrick
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<Character, GetCharacterDto>();
      CreateMap<AddCharacterDto, Character>();
    }
}
}