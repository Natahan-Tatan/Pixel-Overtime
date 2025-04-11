using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pixel_overtime_api.Database.Models;

namespace pixel_overtime_api.Database;

public class ApiDbContext: IdentityDbContext<User>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options){ }
}
