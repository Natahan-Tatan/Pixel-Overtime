//  
//   Pixel Overtime API
//   Copyright (C) 2025  Natahan Tatan <license@natahan.net>
//   
//   This software is provided free of charge for personal, non-commercial, and non-professional use only.
//   
//   Permissions
//   You are permitted to:
//   Use this software for personal and educational purposes.
//   Modify the source code for personal use only.
//   Share the unmodified version with others for personal use only, as long as this license file is included.
//   
//   Restrictions
//   You are not permitted to:
//   Use this software in any commercial, professional, or for-profit context.
//   Sell, sublicense, or distribute this software as part of any paid service or product.
//   Use this software within an organization or business.
//   Modify and redistribute the software, unless with the express written permission of the author.
//   
//   Disclaimer
//   This software is provided "as is", without warranty of any kind, express or implied,
//   including but not limited to the warranties of merchantability, fitness for a particular purpose,
//   and noninfringement. In no event shall the authors or copyright holders
//   be liable for any claim, damages or other liability, whether in an action of contract,
//   tort or otherwise, arising from, out of or in connection with the software or the use
//   or other dealings in the software.
//   
//   ---
//   
//   If you wish to use this software for commercial or professional purposes, please contact the author to discuss licensing options.

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using pixel_overtime_api.Database.Models;
using pixel_overtime_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddW3CLogging(logging =>
{
    logging.FileSizeLimit = 10 * 1024 * 1024;
    logging.RetainedFileCountLimit = 5;
    logging.FileName = "pixel_overtime_api-access-";
    logging.LogDirectory = "logs/";
    logging.FlushInterval = TimeSpan.FromSeconds(1);
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<pixel_overtime_api.Database.Models.User>(opt => {

        //FIXME: make issuer taken from settings
        opt.Tokens.AuthenticatorIssuer = "PixelOvertime_debug_1456";
    })
    .AddEntityFrameworkStores<pixel_overtime_api.Database.ApiDbContext>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc($"v{pixel_overtime_api.Version.AsString()}", new OpenApiInfo
    {
        Version = $"v{pixel_overtime_api.Version.AsString()}",
        Title = "Pixel Overtime - API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        //TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Github",
            Url = new Uri("https://github.com/Natahan-Tatan/Pixel-Overtime")
        },
        License = new OpenApiLicense
        {
            Name = "GPL v3",
            Url = new Uri("https://www.gnu.org/licenses/")
        },
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: 'Bearer 12345abcdef'",
         Name = "Authorization",
         In = ParameterLocation.Header,
         Type = SecuritySchemeType.ApiKey,
         Scheme = "Bearer",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"pixel-overtime-models.xml"));
});

builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddDbContext<pixel_overtime_api.Database.ApiDbContext>(opt => {
    opt.UseSqlite("Data Source=../datas/overpixel.db;");
});

var app = builder.Build();

app.UseW3CLogging();

//app.MapIdentityApi<pixel_overtime_api.Database.Models.User>();
app.MapCustomIdentityApi<pixel_overtime_api.Database.Models.User>();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v{pixel_overtime_api.Version.AsString()}/swagger.json", $"v{pixel_overtime_api.Version.AsString()}");
        options.RoutePrefix = "doc";
    });
}

app.Run();