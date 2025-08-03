using SynonymAPI.Models;
using SynonymAPI.Models.DTO;

namespace SynonymAPI.Repositories
{
    public interface IWordRepository
    {
        Task<Word?> GetOrCreateWordAsync(string wordText);
        Task AddSynonymsAsync(string word, List<string> synonyms);
        Task ChangeMainSynonyms(int oldMainId, int newMainId);
        Task<List<WordDTO>> GetSynonymsAsync(string word);
    }
}
