using DbService.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbService
{
    public class DatabaseService
    {
        public bool AddPosition(StorePosition position)
        {
            using var db = new AppDbContext();
            db.Positions.Add(position);
            db.SaveChanges();
            return true;
        }

        public List<StorePosition> GetPositions()
        {
            using var db = new AppDbContext();
            return db.Positions.ToList();
        }

        public StorePosition GetPositionById(int id)
        {
            using var db = new AppDbContext();
            return db.Positions.FirstOrDefault(p => p.Id == id);
        }

        public bool DeletePosition(int id)
        {
            using var db = new AppDbContext();
            var item = db.Positions.Find(id);
            if (item == null) return false;

            db.Positions.Remove(item);
            db.SaveChanges();
            return true;
        }

        public bool CreateUser(StoreUser user)
        {
            using var db = new AppDbContext();

            if (db.Users.Any(u => u.UserName == user.UserName)) return false;

            db.Users.Add(user);
            db.SaveChanges();
            return true;
        }

        public StoreUser GetUserByUserName(String name)
        {
            using var db = new AppDbContext();
            return db.Users.FirstOrDefault(u => u.UserName == name);
        }

        public StoreUser GetUserById(int id)
        {
            using var db = new AppDbContext();
            return db.Users.FirstOrDefault(p => p.Id == id);
        }
    }
}