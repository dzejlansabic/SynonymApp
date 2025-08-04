using Microsoft.EntityFrameworkCore;
using SynonymAPI.DBUtility;
using SynonymAPI.Models;
using SynonymAPI.Models.DTO;

namespace SynonymAPI.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly SynonymDbContext _context;
        public WordRepository(SynonymDbContext context)
        {
            _context = context;
        }
        public async Task<Word?> GetOrCreateWordAsync(string wordText)
        {
            var word = await _context.Words.FirstOrDefaultAsync(w => w.Text == wordText);
            if (word == null)
            {
                word = new Word { Text = wordText};
                _context.Words.Add(word);
                await _context.SaveChangesAsync();
                word = await _context.Words.FirstOrDefaultAsync(w => w.Text == wordText);
            }
            return word;
        }

        public async Task AddSynonymsAsync(string word, List<string> synonyms)
        {
            if (string.IsNullOrWhiteSpace(word))
                throw new ArgumentException("Word cannot be null or empty.", nameof(word));

            if (synonyms == null || !synonyms.Any() || synonyms.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException("Synonyms list cannot be null, empty, or contain blank entries.", nameof(synonyms));

            var wordEntity = await GetOrCreateWordAsync(word);
            if (wordEntity == null)
                throw new InvalidOperationException("Failed to create or retrieve the main word.");

            int wordRootId = wordEntity.MainSynonymId ?? wordEntity.Id;

            foreach (var synonymText in synonyms.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (string.Equals(synonymText, word, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("A word cannot be its own synonym.");

                var synonymEntity = await GetOrCreateWordAsync(synonymText);
                if (synonymEntity == null)
                    throw new InvalidOperationException($"Failed to create or retrieve synonym: {synonymText}");

                int synonymRootId = synonymEntity.MainSynonymId ?? synonymEntity.Id;

                if (wordRootId == synonymRootId)
                    continue;

                int newRootId = wordRootId;
                int oldRootId = synonymRootId;

                var wordsToUpdate = await _context.Words
                    .Where(w => w.MainSynonymId == oldRootId || w.Id == oldRootId)
                    .ToListAsync();

                foreach (var w in wordsToUpdate)
                    w.MainSynonymId = newRootId;

                if (synonymEntity.MainSynonymId != newRootId)
                    synonymEntity.MainSynonymId = newRootId;
            }

            await _context.SaveChangesAsync();
        }


        public async Task ChangeMainSynonyms(int oldMainId, int newMainId)
        {
            var wordsToUpdate = await _context.Words.Where(w => w.MainSynonymId == oldMainId).ToListAsync();
            foreach (var word in wordsToUpdate)
            {
                word.MainSynonymId = newMainId;
            }
            if (wordsToUpdate.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<WordDTO>> GetSynonymsAsync(string wordText)
        {
            var word = await _context.Words.FirstOrDefaultAsync(w => w.Text == wordText);
            if (word == null)
                return new List<WordDTO>();

            int rootId = word.MainSynonymId ?? word.Id;

            var synonyms = await _context.Words
                .Where(w => (w.MainSynonymId == rootId || w.Id == rootId) && w.Id != word.Id)
                .Select(w => new WordDTO
                {
                    Id = w.Id,
                    Text = w.Text,
                    MainSynonymId = w.MainSynonymId
                })
                .ToListAsync();

            return synonyms;
        }
    }
}
