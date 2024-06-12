using ADMReestructuracion.Auth.DataAccess.Models;
using ADMReestructuracion.Auth.Domain.Models;
using ADMReestructuracion.Auth.Domain.RequestModel;
using ADMReestructuracion.Common.Extensions;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ADMReestructuracion.Auth.BusinessLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ForAllMembersIfNotEmpty();
            CreateMap<Usuario, UsuarioRequest>().ForAllMembersIfNotEmpty();
        }
    }

    public static class MappingProfileExtensions
    {
        public static void ConfigureMappingProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
