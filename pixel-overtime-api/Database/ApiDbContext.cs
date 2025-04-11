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


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pixel_overtime_api.Database.Models;

namespace pixel_overtime_api.Database;

public class ApiDbContext: IdentityDbContext<User>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options){ }
}
