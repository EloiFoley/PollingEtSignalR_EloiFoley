using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace labo.signalr.api.Hubs
{
    public class UselessTasksHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public UselessTasksHub(ApplicationDbContext context)
        {
            _context = context;
        }


        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            await Clients.Caller.SendAsync("TaskList", _context.UselessTasks.ToListAsync());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            // TODO: Ajouter votre logique
        }
    }
}
