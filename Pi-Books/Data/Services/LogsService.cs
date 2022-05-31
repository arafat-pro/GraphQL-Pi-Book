using System.Collections.Generic;
using System.Linq;
using Pi_Books.Data.Models;

namespace Pi_Books.Data.Services
{
    public class LogsService
    {
        private AppDbContext dbContext;
        public LogsService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Log> GetAllLogsFromDatabase() => dbContext.Logs.ToList();
    }
}