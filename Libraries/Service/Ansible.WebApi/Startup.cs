using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ansible.Data.Entity;
using Ansible.Data.Model;
using Ansible.Data.Repository;
using Ansible.Services;
using Ansible.Services.Interfaces;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Urf.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace Ansible.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //api
            services.AddCors();
            services.AddRouting();
            services.AddMvc()
                    .AddJsonOptions(options =>
                        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All);
            services.AddOData();

            //database
            services.AddDbContext<AnsibleDbContext>(opt => opt.UseInMemoryDatabase("AnsibleVote"));
            services.AddTransient<AnsibleDbInitializer>();

            //unit of work
            services.AddScoped<DbContext, AnsibleDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //repositories
            services.AddScoped<IVoteRepository, VoteRepository>();

            //services
            services.AddScoped<IVoteService, VoteService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AnsibleDbInitializer seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //cross origin
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                builder.Build();
            });

            //odata
            var oDataConventionModelBuilder = new ODataConventionModelBuilder(app.ApplicationServices);

            //add models to odata
            var voteEntitySetConfiguration = oDataConventionModelBuilder.EntitySet<Vote>(nameof(Vote));
            voteEntitySetConfiguration.EntityType.HasKey(x => x.VoteId);

            //default routes
            app.UseMvc(routeBuilder =>
            {
                //api route
                //routeBuilder.MapRoute("default", "api/{controller=Vote}/{action=Index}/{id?}");

                //odata route
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(1000).Count();
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", oDataConventionModelBuilder.GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });

            //seed database
            seeder.Seed().Wait();
        }

        public IConfiguration Configuration { get; }
    }
}
