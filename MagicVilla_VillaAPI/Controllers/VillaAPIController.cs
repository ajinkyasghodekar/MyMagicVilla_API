using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    // [Route("api/controller")]  --> another wahy to route but below one is (line 8) more preffred.
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<VillaDTO> GetVillas()
        {
            return VillaStore.VillaList;
        }

        [HttpGet("Id:int")]
        public VillaDTO GetVillas(int id)
        {
            return VillaStore.VillaList.FirstOrDefault(u=>u.Id == id);
        }
    }

}

