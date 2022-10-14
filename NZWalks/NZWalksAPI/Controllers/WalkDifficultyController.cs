using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {

            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();

            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Request DTO to Domain Model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };

            // Pass details to repo
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);


            // Convert back to DTO

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficulty.Code
            };

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficulty.Id }, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            // Get region from db
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            // If null NotFound
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert response to DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to domain
            // Request DTO to Domain Model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
              
            };

            // Update Regions using repo
            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            // If null - NotFound
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code
            };

            // Return Ok
            return Ok(walkDifficultyDTO);
        }
    }
}
