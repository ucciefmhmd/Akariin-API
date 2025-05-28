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
                using var scope = app.Services.CreateScope();
                var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
                initialiser.InitialiseAsync().Wait();
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
                c.SwaggerEndpoint("/swagger/MaintenanceRequest/swagger.json", "MaintenanceRequest API v1");
                c.SwaggerEndpoint("/swagger/Summary/swagger.json", "Summary API v1");

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

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}"));

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