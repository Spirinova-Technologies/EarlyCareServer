using Amazon.SimpleNotificationService;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Repositories;
using EarlyCare.Core.Services;
using EarlyCare.WebApi.AutoMapperConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EarlyCare.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

            services.AddAWSService<IAmazonSimpleNotificationService>();

            #region Services

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<IGlobalSettingService, GlobalSettingService>();
            services.AddTransient<IHospitalService, HospitalService>();

            services.AddTransient<IGlobalSettingsRepository, GlobalSettingsRepository>();
            services.AddTransient<IOtpRepository, OtpRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IHospitalRepository, HospitalRepository>();
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<IPlasmaRepository, PlasmaRepository>();
            services.AddTransient<IAmbulanceRepository, AmbulanceRepository>();
            services.AddTransient<IRtpcrTestRepository, RtpcrTestRepository>();
            services.AddTransient<IFoodRepository, FoodRepository>();
            services.AddTransient<IMedicalEquipmentRepository, MedicalEquipmentRepository>();
            services.AddTransient<IOxygenProviderRepository, OxygenProviderRepository>();
            services.AddTransient<IConsultationRepository, ConsultationRepository>();

            #endregion Services

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder =>
                    builder
                   .AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                 );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}