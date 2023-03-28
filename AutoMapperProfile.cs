using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _Net_REST_API
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