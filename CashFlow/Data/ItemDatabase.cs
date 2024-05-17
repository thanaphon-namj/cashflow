using System;
using SQLite;

namespace CashFlow.Data
{
    public class ItemDatabase
    {
        SQLiteAsyncConnection Database;

        async Task Init()
        {
            if (Database is not null) return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<Models.Item>();
        }

        public async Task<List<Models.Item>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Models.Item>().OrderByDescending(i => i.ID).ToListAsync();
        }

        public async Task<List<Models.Item>> GetItemsAsync(DateTime startDate, DateTime toDate)
        {
            await Init();
            return await Database.Table<Models.Item>().Where(i => i.date >= startDate && i.date < toDate).ToListAsync();
        }

        public async Task<Models.Item> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Models.Item>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Models.Item item)
        {
            await Init();
            if (item.ID != 0) return await Database.UpdateAsync(item);
            else return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(Models.Item item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> DeleteItemsAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<Models.Item>();
        }
    }
}

