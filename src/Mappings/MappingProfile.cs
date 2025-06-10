namespace InsuraNova.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            // UserProfile mapping
            CreateMap<UserProfile, UserDto>().ReverseMap();
        }
    }
}
