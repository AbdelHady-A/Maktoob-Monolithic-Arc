using System;
using System.IO;
using Maktoob.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Maktoob.Persistance.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MaktoobDbContext>
    {
        public MaktoobDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("DBConnectionString");
            Console.WriteLine("args : ------------------ : " + connectionString?.Length);
            var builder = new DbContextOptionsBuilder<MaktoobDbContext>();
            builder.UseSqlServer(connectionString ?? "Server=(localdb)\\mssqllocaldb;Database=Maktoob;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new MaktoobDbContext(builder.Options);
        }
    }
}