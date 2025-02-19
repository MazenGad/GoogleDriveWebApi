﻿
namespace GoogleDriveWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SchemaGeneratorOptions.SupportNonNullableReferenceTypes = true;
				options.OperationFilter<SwaggerFileUploadOperationFilter>();

			});


			builder.Services.AddSingleton<GoogleDriveService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
				app.UseSwagger();
                app.UseSwaggerUI();
			}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
