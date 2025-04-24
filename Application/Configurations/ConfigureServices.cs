﻿using Microsoft.Extensions.DependencyInjection;
using Application.Services.Email;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Application.Services.UserService;
using Application.Services.File;

using Application.Services.Notificationsd;
using FluentValidation;
using Application.Utilities.Contractors;
using Application.Services.DeleteService;
using Application.Utilities.Models;

namespace Application.Configurations
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //services.AddMapster();
            //services.AddScoped<INotificationSender,EmailSender>();
            services.AddTransient<EmailSender>();
            services.AddTransient<FCMNotificationSender>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISoftDeleteService, SoftDeleteService>();
            services.AddScoped<LocalFiletService>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, LocalFiletService>();

            services.AddScoped<AttachmentService>();
            //services.AddScoped<IImageCompress, ImageCompress>();

            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TaskManager-Firebase-Key.json"))
            //});

            services.AddDistributedMemoryCache(options => { });
            
            return services;
        }
    }
}
