using System;
using AutoMapper;
using ZipPayApi.Data;
using ZipPayApi.Model;

namespace ZipPayApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>();

            CreateMap<Account, AccountResponse>();
            CreateMap<AccountRequest, Account>();
        }
    }
}
