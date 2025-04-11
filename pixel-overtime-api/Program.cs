//    Pixel Overtime API
//    Copyright (C) 2025  Natahan Tatan <license@natahan.net>
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using pixel_overtime_api.Database.Models;
using pixel_overtime_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<pixel_overtime_api.Database.Models.User>(opt => {

        //TODO: make issuer taken from settings
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
});

builder.Services.AddControllers();

builder.Services.AddDbContext<pixel_overtime_api.Database.ApiDbContext>(opt => {
    opt.UseSqlite("Data Source=../datas/overpixel.db;");
});

var app = builder.Build();

//app.MapIdentityApi<pixel_overtime_api.Database.Models.User>();
app.MapCustomIdentityApi<pixel_overtime_api.Database.Models.User>();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{pixel_overtime_api.Version.AsString()}/swagger.json", $"v{pixel_overtime_api.Version.AsString()}");
        options.RoutePrefix = "doc";
    });
}

app.Run();