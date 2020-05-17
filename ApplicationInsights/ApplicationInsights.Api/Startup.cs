using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationInsights.Api
{

    [Route("test")]
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger)
        {
            logger.LogInformation("TestController initialized");
        }

        [HttpGet]
        [Route("")]
        public ActionResult<string> Test()
        {
            return "test ok";
        }

        [HttpGet]
        [Route("secret")]
        [Authorize]
        public ActionResult<string> TestSecret()
        {
            return "it's our secret";
        }

        [Route("greet/{name}")]
        [HttpGet]
        public ActionResult<string> Greet(string name)
        {
            return $"Hello, {name}";
        }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddAuthorization();
            services.AddControllers();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // middleware/request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // based on the route an endpoint object is created, which can be inspected by the middleware that comes after this step
            // Authentication wasn't set up correclty, but this will only affect routes that require authorization
            // InvalidOperationException: No authenticationScheme was specified, and there was no DefaultChallengeScheme found
            app.UseRouting();

            // The following middleware inspects the endpoint 
            // and authorization + authentication is only applied when the endpoint requires this
            app.UseAuthentication();
            app.UseAuthorization();

            // The endpoints are mapped to certain logic
            // there are many options for mapping endpoints, fe. MapControllers, MapHub(SignalR), MapRazorPages, MapHealthChecks, MapGet, MapPost...
            // multiple options can be combined
            app.UseEndpoints(endpoints => {

                // Configure the Health Check endpoint and require an authorized user.
                endpoints.MapHealthChecks("/health").RequireAuthorization();
                endpoints.MapControllers();
            });
        }
    }
}
