namespace BeerQuest.Core
{
    public class Contacts
    {
         Contacts()
        { }

        public Contacts(string address, string phone, string twitter)
        {
            Address = address;
            Phone = phone;
            Twitter = twitter;
        }

        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string Twitter { get; private set; }
    }
}
