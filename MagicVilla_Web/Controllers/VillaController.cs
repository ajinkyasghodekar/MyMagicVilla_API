﻿using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;


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
            var responce = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (responce != null && responce.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(responce.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla()
        {
           return View();
        }

        // Create new villa
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (responce != null && responce.IsSuccess)
                {
                    TempData["success"] = "Villa Created Successfully...";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error Occured Please Check...";
            return View(model);
        }

        //Update existing villa
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int VillaId)
        {
            var responce = await _villaService.GetAsync<APIResponse>(VillaId, HttpContext.Session.GetString(SD.SessionToken));
            if (responce != null && responce.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(responce.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _villaService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (responce != null && responce.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Successfully...";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error Occured Please Check...";
            return View(model);
        }

        /*//Delete existing villa without opening delete page
        public async Task<IActionResult> DeleteVilla(int VillaId)
        {
            var responce = await _villaService.GetAsync<APIResponse>(VillaId);
            if (responce != null && responce.IsSuccess)
            {
                //VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(responce.Result));
                var responceNew = await _villaService.DeleteAsync<APIResponse>(VillaId);
                return RedirectToAction(nameof(IndexVilla));
                //return View(model);
            }
            return NotFound();
        }*/


        //Delete existing villa
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int VillaId)
        {
            var responce = await _villaService.GetAsync<APIResponse>(VillaId, HttpContext.Session.GetString(SD.SessionToken));
            if (responce != null && responce.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(responce.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var responce = await _villaService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (responce != null && responce.IsSuccess)
            {
                TempData["success"] = "Villa Deleted Successfully...";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error Occured Please Check...";
            return View(model);
        }

    }
}
