using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietQrMobile.Models;

namespace VietQrMobile.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "VietQrMobile.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Account>().Wait();
        }

        public Task<int> AddOrUpdateAccount(Account account)
        {
            return _database.InsertOrReplaceAsync(account);
        }

        public Task<Account> GetAccountById(int id)
        {
            return _database.Table<Account>().FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
