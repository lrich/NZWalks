using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database
            var walksDomain = await walkRepository.GetAllAsync();

            // Convert domain to DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // Return response
            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Walks domain
            var walkDomain = await walkRepository.GetAsync(id);

            // Walks DTO
            var walksDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Return response
            return Ok(walksDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass Domain to repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert Domain back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Send DTO return
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            // Pass detials to repo
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // Handle null
            if(walkDomain == null)
            { 
                return NotFound();
            }
            else
            {
                // Convert back to DTO
                var walkDTO = new Models.DTO.Walk
                {
                    Id = walkDomain.Id,
                    Name = walkDomain.Name,
                    Length = walkDomain.Length,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId
                };

                return Ok(walkDTO);
            }

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // Call repo to delete
            var walkDomain = await walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }
    }
}
