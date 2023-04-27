using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
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

        private readonly ApplicationDbContext _db;
        public VillaAPIController( ApplicationDbContext db)
        {
            _db = db;
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
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        // or this can be written as,
        // [ProducesResponseType(200, Type = typeof(VillaDTO))]
        // [ProducesResponseType(400)]
        // [ProducesResponseType(404)]
        public ActionResult GetVillas(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        // Create Villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            // the below commented code for ModelState is used for validations in place of [ApiController].
            // But if both are active more prority will be given to [ApiController].

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Villa Already Exists");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}, villaDTO);
        }

        // Delete Villa
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public IActionResult DeleteVilla (int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u =>u.Id == id);

            if (villa == null)
            {
                return NotFound();                
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();   
        }

        // Put (Update) Villa
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla (int id, [FromBody] VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            /*var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;*/

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        // Patch (Partial Update in Data) Villa
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla (int id, JsonPatchDocument <VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            VillaDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Name = villa.Name,
                Id = villa.Id,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Rate = villa.Rate,
                Occupancy = villa.Occupancy,
                Sqft = villa.Sqft
            };

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Update(model);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}