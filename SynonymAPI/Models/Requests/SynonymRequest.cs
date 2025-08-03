namespace SynonymAPI.Models.Requests
{
    public class SynonymRequest
    {
        public string Word { get; set; }
        public List<string> Synonym { get; set; }
    }
}
