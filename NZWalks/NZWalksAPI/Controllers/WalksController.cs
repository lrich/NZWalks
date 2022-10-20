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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, 
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
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

            // Validate request Model
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
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

            // Validate request Model
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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


        #region Private methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            //if (addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //        $"{nameof(addWalkRequest)} cannot be empty.");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} is required.");
            //}

            //if (addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            //}

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                    $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                       $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest),
            //        $"{nameof(updateWalkRequest)} cannot be empty.");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{nameof(updateWalkRequest.Name)} is required.");
            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{nameof(updateWalkRequest.Length)} should be greater than zero.");
            //}

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                    $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                       $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
