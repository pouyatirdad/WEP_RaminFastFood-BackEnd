using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface ILogRepository
    {
        Task<Log> LogEvent(string TableName, int id, string Action);
    }
    public class LogRepository : ILogRepository
    {
        private readonly StorePanelDbContext _context;

        public LogRepository(StorePanelDbContext context)
        {
            _context = context;
        }
        public async Task<Log> LogEvent(string TableName, int id, string Action)
        {
            var userName = GetCurrentUsersName();
            var log = new Log();
            log.Action = Action;
            log.TableName = TableName;
            log.EntityId = id;
            log.UserName = userName;
            log.ActionDate = DateTime.Now;
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }
        private string GetCurrentUsersName()
        {
            var userName = "";
            if (MyAppContext.Current?.User?.Identity?.Name != null)
            {
                userName = MyAppContext.Current.User.Identity.Name;
            }
            return userName;
        }
    }
}
