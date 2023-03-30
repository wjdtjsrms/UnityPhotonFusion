namespace JSGCode.Internship.DataModel
{
    public class ChatMessageModel
    {
        public string sender;
        public string meesage;

        public ChatMessageModel()
        {

        }

        public ChatMessageModel(string sender, string meesage)
        {
            this.sender = sender;
            this.meesage = meesage;
        }
    }
}