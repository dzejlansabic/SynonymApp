namespace SynonymAPI.Models.DTO
{
    public class WordDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? MainSynonymId { get; set; }
    }
}
