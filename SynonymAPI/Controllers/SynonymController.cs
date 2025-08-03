using Microsoft.AspNetCore.Mvc;
using SynonymAPI.Models.Requests;
using SynonymAPI.Services;

namespace SynonymAPI.Controllers
{
    [ApiController]
    public class SynonymController : ControllerBase
    {
        private readonly SynonymService _synonymService;
        public SynonymController(SynonymService synonymService)
        {
            _synonymService = synonymService;
        }
        [HttpGet("synonyms/{word}")]
        public async Task<IActionResult> GetSynonyms(string word)
        {
            try
            {
                var synonyms = await _synonymService.GetSynonymsAsync(word);
                return Ok(synonyms);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("synonyms")]
        public async Task<IActionResult> AddSynonym([FromBody] SynonymRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Word) || request.Synonym == null || !request.Synonym.Any())
            {
                return BadRequest("Both 'word' and 'synonym' must be provided, and synonym cannot be empty.");
            }

            try
            {
                await _synonymService.AddSynonymsAsync(request.Word, request.Synonym);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        //[HttpPost("synonyms")]
        //public async Task<IActionResult> AddSynonym([FromBody] SynonymRequest request)
        //{
        //    try
        //    {
        //        await _synonymService.AddSynonymAsync(request.Word, request.Synonym);
        //        return NoContent();
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(ex.Message);
        //    }
        //}

        [HttpGet("words/{word}")]
        public async Task<IActionResult> GetOrCreateWord(string word)
        {
            try
            {
                var wordEntity = await _synonymService.GetOrCreateWordAsync(word);
                if (wordEntity == null)
                {
                    return NotFound();
                }
                return Ok(wordEntity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
