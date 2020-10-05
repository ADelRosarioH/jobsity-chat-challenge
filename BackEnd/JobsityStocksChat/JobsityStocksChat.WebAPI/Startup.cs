using System.Text;
using System.Threading.Tasks;
using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.Infrastructure.Persistence;
using JobsityStocksChat.Infrastructure.Services;
using JobsityStocksChat.WebAPI.Hubs;
using JobsityStocksChat.WebAPI.MQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace JobsityStocksChat.WebAPI
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("JobsityStocksChatConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            var key = Encoding.ASCII.GetBytes("JWT_SECRET_KEY_A7D3318FC11874C4F8B4682A899BB");
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // Sending the access token in the query string is required due to
                // a limitation in Browser APIs. We restrict it to only calls to the
                // SignalR hub in this code.
               
                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors();

            services.AddSignalR();

            services.AddScoped<ITokenClaimService, IdentityTokenClaimService>();
            services.AddScoped<IAsyncRepository<UserMessage>, UserMessageAsyncRepository>();
            services.AddScoped<IUserMessageAsyncRepository, UserMessageAsyncRepository>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IChatCommandHandler, ChatCommandHandler>();

            services.AddSingleton<IStockPriceQueueProducer, StockPriceQueueProducer>();
            services.AddHostedService<StockPriceQueueConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseCors(options => options.WithOrigins("http://localhost:4200")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/hubs/chat");
                endpoints.MapControllers();
            });
        }
    }
}
