using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.NotificationCenter.Hubs
{
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Elenco di tutte le connessioni al socket.
        /// </summary>
        static readonly List<SocketConnection> _notificationHubConnections = new List<SocketConnection>();

        /// <summary>
        /// Quando un utente esegue l'accesso sul client viene creata una nuova SocketConnection con UserId e ConnectionId,
        /// viene registrata nelle _socketConnections e se è andato tutto a buon fine comunica al client la sua connectionID.
        /// </summary>
        /// <param name="userId">ID dell'utente loggato che si sta connettendo al socket</param>
        /// <returns></returns>
        public async Task Connect(int userId)
        {
            var connectionId = Context.ConnectionId;
            if (_notificationHubConnections.All(p => p.ConnectionId != connectionId && p.UserId != userId))
                _notificationHubConnections.Add(new SocketConnection { UserId = userId, ConnectionId = connectionId });

            await Clients.Client(connectionId).SendAsync("MyConnectionId", connectionId);
        }

        /// <summary>
        /// Override messo a disposizione per effettuare operazioni quando un utente si connette al socket.
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Override del metodo chiamato quando un utente si disconnette dal socket.
        /// Elimina la connessione legata all'utente disconnesso dalla lista _socketConnections.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var item = _notificationHubConnections.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            if (item != null)
                _notificationHubConnections.Remove(item);

            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// Metodo che invia la notifica push ai client connessi (recuperati filtrando le connessioni attive con la lista degli ID ricevuti) in tempo reale.
        /// </summary>
        /// <param name="receiverIDs">Lista degli ID degli User destinatari della notifica</param>
        /// <param name="notificationTitle">Titolo della notifica che viene poi visualizzato nel toast sul client</param>
        public void NotificationDataUpdate(List<int> receiverIDs, string notificationTitle)
        {
            var connectedUserInfo = _notificationHubConnections.Where(p => receiverIDs.Contains(p.UserId)).ToList();
            foreach (var connectedUser in connectedUserInfo)
            {
                Clients.Client(connectedUser.ConnectionId).SendAsync("NotificationDataUpdate", notificationTitle).Wait();
            }
        }
    }
}
