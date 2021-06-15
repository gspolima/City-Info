using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddMvcOptions(options =>
                        options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));

            /* ---Change the default casing for properties in the JSON response.---
            //.AddJsonOptions(jsonOptions =>
            //    {
            //        if (jsonOptions.SerializerSettings.ContractResolver != null)
            //        {
            //            var castedResolver = jsonOptions.SerializerSettings.ContractResolver as DefaultContractResolver;
            //            castedResolver.NamingStrategy = null;
            //        }
            //    }
            //);*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler();

            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
