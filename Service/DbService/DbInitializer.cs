using System;
using System.Collections.Generic;
using System.Text;

namespace DbService
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
        }
    }
}