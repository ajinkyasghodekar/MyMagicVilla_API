using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    // [Route("api/controller")]  --> another way to route but below one is (line 8) more preffred.
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaAPINumberController : ControllerBase
    {
        // New logger with custom logger

        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        public VillaAPINumberController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            this._response = new();
            _dbVilla = dbVilla;
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
        public async Task<ActionResult<APIResponse>> GetVillasNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync();

                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        // or this can be written as,
        // [ProducesResponseType(200, Type = typeof(VillaDTO))]
        // [ProducesResponseType(400)]
        // [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map <VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        // Create Villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            // the below commented code for ModelState is used for validations in place of [ApiController].
            // But if both are active more prority will be given to [ApiController].

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            try
            {

                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("", "Villa Number Already Exists");
                    return BadRequest(ModelState);
                }

                if (await _dbVilla.GetAsync(u => u.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid !");
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

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

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

                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map <VillaNumberDTO> (villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // Delete Villa
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // Put (Update) Villa
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {

                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }

                if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid !");
                    return BadRequest(ModelState);
                }

                /*var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
                villa.Name = villaDTO.Name;
                villa.Sqft = villaDTO.Sqft;
                villa.Occupancy = villaDTO.Occupancy;*/

                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

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

                await _dbVillaNumber.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        /*// Patch (Partial Update in Data) Villa
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            *//* Old mapping
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
            };*//*

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);


            Villa model = _mapper.Map<Villa>(villaDTO);

            *//* Old mapping
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
            };*//*

            _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }*/
    }
}