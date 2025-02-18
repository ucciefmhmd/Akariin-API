using API.Hubs;
using Infrastructure;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Configurations
{
    public static class ConfigureApp
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // Initialise and seed database
                using (var scope = app.Services.CreateScope())
                {
                    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                    initialiser.InitialiseAsync().Wait();
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/RealEstate/swagger.json", "Real Estate API v1");
                c.SwaggerEndpoint("/swagger/Common/swagger.json", "Common API v1");
                c.SwaggerEndpoint("/swagger/Member/swagger.json", "Member API v1");
                c.SwaggerEndpoint("/swagger/Contract/swagger.json", "Contract API v1");
                c.SwaggerEndpoint("/swagger/Bill/swagger.json", "Bill API v1");
                c.SwaggerEndpoint("/swagger/RealEstateUnit/swagger.json", "Real Estate Unit API v1");
                //c.SwaggerEndpoint("/swagger/Tenant/swagger.json", "Tenant API v1");

                c.RoutePrefix = "swagger";
                c.DocExpansion(DocExpansion.None);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.EnableFilter();
            });

            //app.UseCors("AllowSpecificOrigin");
            //app.UseCors("AllowOrigin");
            app.UseCors("AllowServerOrigin");
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

#pragma warning disable ASP0014 // Suggest using top level route registrations
            app.UseEndpoints(endpoints => endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}"));
#pragma warning restore ASP0014 // Suggest using top level route registrations
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<ChatHub>("/chathub");
            //});

            app.MapControllers();
            app.MapFallbackToFile("index.html");
    
            
            return app;
        }
    }
}