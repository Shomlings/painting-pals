using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PaintingWithPals.Helpers;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PaintingWithPals.Hubs
{
    public class DrawHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            DrawingHelper.Artists.Add(new Client()
            {
                ConnectionId = Context.ConnectionId,
                RoomCode = null
            });
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var artist = DrawingHelper.Artists.FirstOrDefault(m => m.ConnectionId == Context.ConnectionId);
            if (!DrawingHelper.Artists.Any(m => m.RoomCode == artist.RoomCode && m.ConnectionId != artist.ConnectionId))
                DrawingHelper.ClearRoom(artist.RoomCode);
            DrawingHelper.Artists.Remove(artist);
            return base.OnDisconnectedAsync(exception);
        }

        public Task SetRoom(string room)
        {
            DrawingHelper.Artists.FirstOrDefault(m => m.ConnectionId == Context.ConnectionId).RoomCode = room;
            return Clients.Caller.SendAsync("ReceiveHistory", DrawingHelper.History.Where(m => m.Room == room));
        }

        public Task SendPaint(PaintModel model)
        {
            model.ConnectionId = Context.ConnectionId;
            DrawingHelper.History.Add(model);
            return Clients.Clients(DrawingHelper.Artists.Where(m => m.RoomCode == model.Room).Select(m => m.ConnectionId).ToList())
                .SendAsync("ReceivePaint", model);
        }

        public Task UndoLine(string roomCode)
        {
            DrawingHelper.UndoLast(Context.ConnectionId);
            return Clients.Clients(DrawingHelper.Artists.Where(m => m.RoomCode == roomCode).Select(m => m.ConnectionId).ToList())
                .SendAsync("ReceiveHistory", DrawingHelper.History.Where(m => m.Room == DrawingHelper.Artists.FirstOrDefault(m => m.ConnectionId == Context.ConnectionId).RoomCode));
        }
    }
}