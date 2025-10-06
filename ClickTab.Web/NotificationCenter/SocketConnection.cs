namespace ClickTab.Web.NotificationCenter
{
    /// <summary>
    /// Modello per memorizzare l'ID della connessione e l'ID dell'utente che l'ha aperta
    /// </summary>
    public class SocketConnection
    {
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
}
