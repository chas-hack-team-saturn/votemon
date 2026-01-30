using Microsoft.EntityFrameworkCore;

namespace BackendAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var password = Environment.GetEnvironmentVariable("MARIADB_PASSWORD");

            var serverVersion = new MySqlServerVersion(new Version(12, 1));

            Console.WriteLine(password);


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();



            //builder.Services.AddDbContext<>(options => options.UseMysSql(connectionString, serverVersion, UseMicrosoftJson))
            //    .LogTo(Console.WriteLine, LogLevel.Information).UseMicrosoftJson();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.Run();
        }
    }
}
