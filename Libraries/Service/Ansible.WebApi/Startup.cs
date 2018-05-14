using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ansible.Data.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            //data layer
            services.AddDbContext<AnsibleDbContext>(opt => opt.UseInMemoryDatabase("AnsibleVote"));
            services.AddTransient<AnsibleDbInitializer>();

            //ui layer
            services.AddRouting();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AnsibleDbInitializer seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //default routes
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Vote}/{action=Get}/{id?}");
            });

            //seed database
            seeder.Seed().Wait();
        }

        public IConfiguration Configuration { get; }
    }
}
