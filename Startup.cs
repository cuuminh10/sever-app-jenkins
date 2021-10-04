using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.Exceptions;
using gmc_api.Base.Files;
using gmc_api.Base.Helpers;
using gmc_api.Base.InterFace;
using gmc_api.DTO.PP;
using gmc_api.Helpers;
using gmc_api.Repositories;
using gmc_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace gmc_api
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
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_devCors",
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                              .AllowAnyMethod()
                                              .AllowAnyHeader();
                                  });
            });
            /* services.AddControllers().AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.IgnoreNullValues = true;
                 //options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
             }); ;*/

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            //  mappingConfig.AssertConfigurationIsValid();
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<GMCContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // configure DI for application services
            // services.AddScoped<IUserService, UserService>();
            //  services.AddScoped<IProductionOrderService, ProductionOrderService>();
            //  services.AddScoped<IFavoriestService, FavoriestService>();
            //  services.AddScoped<ISystemCommonService, SystemCommonService>();
            services.AddScoped<IStorageService, StorageLocalService>();

            services
                .AddTransient<IADUsersRepository, UserRepository>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IProductionOrderReponsitory, ProductionOrderReponsitory>()
                .AddTransient<IProductionOrderService, ProductionOrderService>()
                .AddTransient<IFavoriestReponsitory, FavoriestReponsitory>()
                .AddTransient<IFavoriestService, FavoriestService>()
                .AddTransient<ISystemCommonReponsitory, SystemCommonReponsitory>()
                .AddTransient<ISystemCommonService, SystemCommonService>()
                .AddTransient<IHREmployeeOffWorkReponsitory, HREmployeeOffWorkReponsitory>()
                .AddTransient<IHREmployeeOffWorkService, HREmployeeOffWorkService>()
                .AddTransient<IHREmployeeOvertimesReponsitory, HREmployeeOvertimesReponsitory>()
                .AddTransient<IHREmployeeOvertimeService, HREmployeeOvertimeService>()
                .AddTransient<IHRTravelCalendarReponsitory, HRTravelCalendarReponsitory>()
                .AddTransient<IHRTravelCalendarService, HRTravelCalendarService>()
                .AddTransient<IHRTravelCalendarItemReponsitory, HRTravelCalendarItemReponsitory>()
                .AddTransient<IHRTravelCalendarItemService, HRTravelCalendarItemService>()
                .AddTransient<IADInboxItemReponsitory, ADInboxItemReponsitory>()
                .AddTransient<IADInboxItemService, ADInboxItemService>()
                .AddTransient<IADOutboxItemReponsitory, ADOutboxItemReponsitory>()
                .AddTransient<IADOutboxItemService, ADOutboxItemService>()
                .AddTransient<IADAttachmentReponsitory, ADAttachmentReponsitory>()
                .AddTransient<IADAttachmentService, ADAttachmentService>()
                .AddTransient<IADCommentReponsitory, ADCommentReponsitory>()
                .AddTransient<IADCommentService, ADCommentService>()
                .AddTransient<IPPProductionOrdrEstFGReponsitory, PPProductionOrdrEstFGReponsitory>()
                .AddTransient<IPPProductionOrdrEstFGService, PPProductionOrdrEstFGService>()
                .AddTransient<IPPProductionOrdrEstRMReponsitory, PPProductionOrdrEstRMReponsitory>()
                .AddTransient<IPPProductionOrdrEstRMService, PPProductionOrdrEstRMService>()
                .AddTransient<IADDocHistoryReponsitory, ADDocHistoryReponsitory>()
                .AddTransient<IADDocHistoryService, ADDocHistoryService>()
                .AddTransient<ISimpleReponsitory, SimpleReponsitory>()
                .AddTransient<ISimpleService, SimpleService>();
                
            //    .AddTransient<IReponsitoriesFireBase, FireBaseUsersReponsitories>();

            services.AddScoped<ValidationActionFilter>();
            services.AddScoped<ValidateEntityExistsAttribute<User>>();
            services.AddMvc()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.OperationFilter<FileUploadOperation>();
                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            });

            // Fire Base // onfiguration.GetSection("AppSettings"))
            // Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Configuration.;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseCors("_devCors");

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                Console.WriteLine("exception.GetType()" + exception.GetType());
                //System.NullReferenceException

                await context.Response.WriteAsJsonAsync(new { error = exception.Message });
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Could not find anything");
            });
        }
    }
}
