using InventoryServer.Models;
using InventoryServer.Repository;
using Microsoft.AspNetCore.SignalR;

namespace InventoryServer.Communication
{
    public class InventoryHub : Hub
    {
        private readonly IInventoryRepository _repository;

        public InventoryHub(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task BroadcastInventoryUpdate(Book book, string action)
        {
            await Clients.All.SendAsync("InventoryUpdated", book, action);
        }



        public async Task GetInventory()
        {
            var inventory = _repository.GetInventory();
            await Clients.Caller.SendAsync("ReceiveInventory", inventory);
        }

    }

}
