using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedLibrary;
public interface IRegistrant
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration config);
}
