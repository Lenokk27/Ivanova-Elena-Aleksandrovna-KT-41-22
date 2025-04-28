using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DepartmentInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DisciplineInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.LoadInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.TeachersInterfaces;

namespace IvanovaElenaAleksandrovnaKt_41_22.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<ILoadService, LoadService>(); 

            return services;
        }
    }
}