namespace SynonymAPI.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int? MainSynonymId { get; set; }

        public Word? MainSynonym { get; set; }
        public ICollection<Word>? Synonyms { get; set; }

    }
}
