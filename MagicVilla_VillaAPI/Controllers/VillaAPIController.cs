using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    // [Route("api/controller")]  --> another way to route but below one is (line 8) more preffred.
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        // New logger with custom logger

        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
        }


        // Old logger with system pre defined in progran.cs logger
        /*public ILogger<VillaAPIController> _logger { get; }
        //Adding a logger dependency injection
        public VillaAPIController(ILogger <VillaAPIController> logger)
        {
            _logger = logger;
        }*/


        // Get Villa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            return Ok(_mapper.Map<List<VillaDTO>> (villaList));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        // or this can be written as,
        // [ProducesResponseType(200, Type = typeof(VillaDTO))]
        // [ProducesResponseType(400)]
        // [ProducesResponseType(404)]
        public async Task <ActionResult> GetVillas(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        // Create Villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            // the below commented code for ModelState is used for validations in place of [ApiController].
            // But if both are active more prority will be given to [ApiController].

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Villa Already Exists");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            /*if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/

            Villa model = _mapper.Map<Villa>(createDTO);

            /* Old mapping
             * Villa model = new()
            {
                Amenity = createDTO.Amenity,
                Name = createDTO.Name,
                Details = createDTO.Details,
                ImageUrl = createDTO.ImageUrl,
                Rate = createDTO.Rate,
                Occupancy = createDTO.Occupancy,
                Sqft = createDTO.Sqft
            };*/

            await _dbVilla.CreateAsync(model);

            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }

        // Delete Villa
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> DeleteVilla (int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(u =>u.Id == id);

            if (villa == null)
            {
                return NotFound();                
            }

            await _dbVilla.RemoveAsync(villa);
            return NoContent();   
        }

        // Put (Update) Villa
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdateVilla (int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if(updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            /*var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;*/

            Villa model = _mapper.Map<Villa>(updateDTO);

            /* Old mapping
             * Villa model = new()
            {
                Amenity = updateDTO.Amenity,
                Name = updateDTO.Name,
                Id = updateDTO.Id,
                Details = updateDTO.Details,
                ImageUrl = updateDTO.ImageUrl,
                Rate = updateDTO.Rate,
                Occupancy = updateDTO.Occupancy,
                Sqft = updateDTO.Sqft
            };*/

            await _dbVilla.UpdateAsync(model);
            return NoContent();
        }

        // Patch (Partial Update in Data) Villa
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla (int id, JsonPatchDocument <VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _dbVilla.GetAsync(u => u.Id == id, tracked:false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            /* Old mapping
             * VillaUpdateDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Name = villa.Name,
                Id = villa.Id,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Rate = villa.Rate,
                Occupancy = villa.Occupancy,
                Sqft = villa.Sqft
            };*/

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);


            Villa model = _mapper.Map<Villa>(villaDTO);

            /* Old mapping
             * Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft
            };*/

            _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}