using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            
            var regions = await regionRepository.GetAllAsync();

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);  

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null) 
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Validate request Model
            //if(!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState) ;
            //}

            // Request DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            // Pass details to repo
            region = await regionRepository.AddAsync(region);


            // Convert back to DTO

            var regionDTO = new Models.DTO.Region
            {
                Id =  region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id) 
        {
            // Get region from db
            var region = await regionRepository.DeleteAsync(id);

            // If null NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Convert response to DTO

            //var regionDTO = new Models.DTO.Region
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    Area = region.Area,
            //    Lat = region.Lat,
            //    Long = region.Long,
            //    Population = region.Population
            //};

            // return Ok response

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task <IActionResult> UpdateRegionAsync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            // Validate request Model
            //if (!ValidateUpdateRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Convert DTO to domain
            // Request DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            // Update Regions using repo
            region = await regionRepository.UpdateAsync(id, region);

            // If null - NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            // Return Ok
            return Ok(regionDTO);
        }

        #region Private Methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                  $"Add Region Data is required");
            }

            if(string.IsNullOrWhiteSpace(addRegionRequest.Code))
            { 
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null or empty or white space");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space");
            }

            if(addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero");
            }

            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} cannot be less than or equal to zero");
            }

            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} cannot be less than or equal to zero");
            }

            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} cannot be less than to zero");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                  $"Add Region Data is required");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null or empty or white space");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null or empty or white space");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero");
            }

            //if (updateRegionRequest.Lat <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.Lat),
            //        $"{nameof(updateRegionRequest.Lat)} cannot be less than or equal to zero");
            //}

            //if (updateRegionRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.Long),
            //        $"{nameof(updateRegionRequest.Long)} cannot be less than or equal to zero");
            //}

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} cannot be less than to zero");
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
