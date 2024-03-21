using Application;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(option =>
// {
//     option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
//     option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Please enter a valid token",
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         BearerFormat = "JWT",
//         Scheme = "Bearer"
//     });
//     option.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type=ReferenceType.SecurityScheme,
//                     Id="Bearer"
//                 }
//             },
//             new string[]{}
//         }
//     });
// });
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Logging.AddConsole();
builder.Logging.AddDebug();
var name = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: name,
                      policy  =>
                      {
                          policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                          ;
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     // app.UseSwaggerUI(c => {
//     // c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//     // });
// }

app.UseHttpsRedirection();
app.UseCors(name);
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.MapHealthChecks("/healthz");
app.UseStaticFiles(new StaticFileOptions{
    OnPrepareResponse = ctx => {
                ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", 
                  "Origin, X-Requested-With, Content-Type, Accept");
    },
    FileProvider = new PhysicalFileProvider(
        "/app/Uploads"),
        RequestPath = "/resources"
});
//Console.WriteLine(builder.Environment.ContentRootPath);
// app.UseStaticFiles(new StaticFileOptions{
//     OnPrepareResponse = ctx => {
//                 ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
//                 ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", 
//                   "Origin, X-Requested-With, Content-Type, Accept");
//     },
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(builder.Environment.ContentRootPath,"Uploads")),
//         RequestPath = "/resources"
// });
using(var scope = app.Services.CreateScope())
    {
        var historyTakingDb = scope.ServiceProvider.GetRequiredService<HistoryTakingDb>();
        if(historyTakingDb.Database.GetPendingMigrations().Any()){
            historyTakingDb.Database.Migrate();
        }
    }
app.MapGet("/", () => "Hello, World!");
app.Run();