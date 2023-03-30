namespace JSGCode.Internship.DataModel
{
    public class ChatServerModel
    {
        public string serverName;
        public string userID;

        public ChatServerModel()
        {

        }

        public ChatServerModel(string serverName, string userID)
        {
            this.serverName = serverName;
            this.userID = userID;
        }
    }
}