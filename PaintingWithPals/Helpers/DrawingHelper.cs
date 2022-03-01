using System.Collections.Generic;
using System.Linq;

namespace PaintingWithPals.Helpers
{
    public static class DrawingHelper
    {
        public static List<Client> Artists = new List<Client>();
        public static List<PaintModel> History = new List<PaintModel>();

        public static void ClearRoom(string roomCode)
        {
            var list = History.Where(m => m.Room == roomCode);
            History = History.Except(list).ToList();
        }
        public static void UndoLast(string connectionId)
        {
            if (!History.Any(m => m.ConnectionId == connectionId))
                return;

            int lastUndoId = History.LastOrDefault(m => m.ConnectionId == connectionId).UndoSessionId;

            var list = History.Where(m => m.UndoSessionId == lastUndoId);
            History = History.Except(list).ToList();
        }
    }

    public class Client
    {
        public string ConnectionId { get; set; }
        public string RoomCode { get; set; }
    }

    public class PaintModel
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int EndX { get; set; }
        public int UndoSessionId { get; set; }
        public int EndY { get; set; }
        public string ConnectionId { get; set; }
        public string Room { get; set; }
    }
}