using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace labo.signalr.api.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    public class UselessTasksHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public UselessTasksHub(ApplicationDbContext context)
        {
            _context = context;
        }


        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            var tasks = await _context.UselessTasks.ToListAsync();
            await Clients.Caller.SendAsync("TaskList", tasks);
        }

        public async Task AddTask(string task)
        {
            _context.UselessTasks.Add(new UselessTask() { Text = task });
            _context.SaveChanges();
            await Clients.All.SendAsync("TaskList", _context.UselessTasks.ToList());
        }

        public async Task CompleteTask(int taskId)
        {
            var Task = _context.UselessTasks.Single(t => t.Id == taskId);
            Task.Completed = true;
            _context.SaveChanges();
            await Clients.All.SendAsync("TaskList", _context.UselessTasks.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            await base.OnDisconnectedAsync(exception);
        }

    }
}
