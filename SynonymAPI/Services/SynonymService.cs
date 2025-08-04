using SynonymAPI.Models;
using SynonymAPI.Models.DTO;
using SynonymAPI.Repositories;

namespace SynonymAPI.Services
{
    public class SynonymService
    {
        private readonly IWordRepository _wordRepository;

        public SynonymService(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public async Task<Word?> GetOrCreateWordAsync(string wordText)
        {
            if (string.IsNullOrWhiteSpace(wordText))
            {
                throw new ArgumentException("Word text cannot be null or empty.", nameof(wordText));
            }
            return await _wordRepository.GetOrCreateWordAsync(wordText);
        }

        public async Task<List<WordDTO>> GetSynonymsAsync(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentException("Word cannot be null or empty.", nameof(word));
            }
            var synonyms = await _wordRepository.GetSynonymsAsync(word);
            return synonyms;
        }


        public async Task AddSynonymsAsync(string word, List<string> synonyms)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentException("Word cannot be null or empty.", nameof(word));
            }

            if (synonyms == null || !synonyms.Any() || synonyms.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Synonym list cannot be null, empty, or contain blank entries.", nameof(synonyms));
            }

            await _wordRepository.AddSynonymsAsync(word, synonyms);
        }

    }
}
