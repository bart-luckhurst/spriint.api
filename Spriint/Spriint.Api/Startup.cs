using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spriint.Api.Managers;
using Spriint.Api.Mappers;
using Spriint.Api.Repositories;

namespace Spriint.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string connectionString => Configuration.GetConnectionString("Spriint"); //@"Data Source=.\SQLEXPRESS;Initial Catalog=Spriint;Integrated Security=True;";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //register repositories
            services.AddTransient<IProjectRepository, ProjectRepository>(x => new ProjectRepository(connectionString));
            services.AddTransient<IEpicRepository, EpicRepository>(x => new EpicRepository(connectionString));
            services.AddTransient<IIssueRepository, IssueRepository>(x => new IssueRepository(connectionString));
            //register managers
            services.AddTransient<IProjectManager, ProjectManager>();
            services.AddTransient<IEpicManager, EpicManager>();
            services.AddTransient<IIssueManager, IssueManager>();
            services.AddTransient<IExceptionManager, ExceptionManager>();
            //register mappers
            services.AddTransient<IProjectMapper, ProjectMapper>();
            services.AddTransient<IEpicMapper, EpicMapper>();
            services.AddTransient<IIssueMapper, IssueMapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
