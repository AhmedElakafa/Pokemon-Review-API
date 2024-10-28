using AutoMapper;
using Pokemon_Review_API.DTO;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API.Helper
{
    public class MapingProviel:Profile
    {
        public MapingProviel()
        {
            CreateMap<Pokemon,PokemonDto>();
            CreateMap<PokemonDto,Pokemon>();
            CreateMap<Category,CategoryDto>();
            CreateMap<CategoryDto,Category>();
            CreateMap<Country,CountryDto>();
            CreateMap<CountryDto,Country>();
            CreateMap<OwnerDto,Owner>();
            CreateMap<Review,ReviwDto>();
            CreateMap<ReviwDto,Review>();
            CreateMap<Reviewer,ReviewerDto>();
            CreateMap<ReviewerDto,Reviewer>();
            CreateMap<Owner, OwnerDto>();

        }
    }
}
