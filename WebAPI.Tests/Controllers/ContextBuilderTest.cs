using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TruckSelling.WebAPI.Models;

namespace TruckSelling.WebAPI.Tests
{
    public class TestsContextBuilder
    {
        public AppDbContext AppDbContext { get; set; }

        public void Build()
        {
            // load DB context
            string connectionString = "server=localhost;port=5432;database=truck_selling;userid=postgres;pwd=Yhuj2510#;";
            var contextBuilder = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString);
            AppDbContext = new AppDbContext(contextBuilder.Options);
        }
    }
}
