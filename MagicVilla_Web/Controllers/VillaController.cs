using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController (IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var responce = await _villaService.GetAllAsync<APIResponse>();

            if (responce != null && responce.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(responce.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVilla()
        {
           return View();
        }

        // Create new villa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _villaService.CreateAsync<APIResponse>(model);
                if (responce != null && responce.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(model);
        }

        //Update existing villa
        public async Task<IActionResult> UpdateVilla(int VillaId)
        {
            var responce = await _villaService.GetAsync<APIResponse>(VillaId);
            if (responce != null && responce.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(responce.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _villaService.UpdateAsync<APIResponse>(model);
                if (responce != null && responce.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(model);
        }

        //Delete existing villa
        public async Task<IActionResult> DeleteVilla(int VillaId)
        {
            var responce = await _villaService.GetAsync<APIResponse>(VillaId);
            if (responce != null && responce.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(responce.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var responce = await _villaService.DeleteAsync<APIResponse>(model.Id);
            if (responce != null && responce.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
            return View(model);
        }
    }
}
