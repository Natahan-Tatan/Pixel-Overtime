
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pixel_overtime_api.Database.Models;
using pixel_overtime_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<pixel_overtime_api.Database.Models.User>(opt => {
        opt.Tokens.AuthenticatorIssuer = "PixelOvertime_debug_1456";
    })
    .AddEntityFrameworkStores<pixel_overtime_api.Database.ApiDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
    app.UseSwaggerUI();
}

app.Run();