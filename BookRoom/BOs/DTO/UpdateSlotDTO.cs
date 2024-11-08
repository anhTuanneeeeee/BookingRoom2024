using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTO
{
    public class UpdateSlotDTO
    {
        public int RoomId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

private string GetConnectionString()
{
    IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();
    return configuration["ConnectionStrings:DefaultConnection"];
}*/