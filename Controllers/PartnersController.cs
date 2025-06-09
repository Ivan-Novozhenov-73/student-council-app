using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.Services;
using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentCouncilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        // Получить партнеров (поиск по строке или по eventId)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerDto>>> GetPartners(
            [FromQuery] string? search = null,
            [FromQuery] Guid? eventId = null)
        {
            IEnumerable<Partner> partners;

            if (!string.IsNullOrEmpty(search))
            {
                partners = await _partnerService.SearchPartnersAsync(search);
            }
            else if (eventId.HasValue)
            {
                partners = await _partnerService.GetPartnersByEventAsync(eventId.Value);
            }
            else
            {
                partners = await _partnerService.GetAllPartnersAsync();
            }

            return Ok(partners.Select(PartnerMapper.ToDto));
        }

        // Получить партнера по id
        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerDto>> GetPartner(Guid id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null)
                return NotFound();
            return Ok(PartnerMapper.ToDto(partner));
        }

        // Создать партнера (Head, Chairman)
        [HttpPost]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<PartnerDto>> CreatePartner([FromBody] CreatePartnerDto partnerDto)
        {
            try
            {
                var createdPartner = await _partnerService.CreatePartnerAsync(PartnerMapper.ToEntity(partnerDto));
                return CreatedAtAction(nameof(GetPartner), new { id = createdPartner.PartnerId }, PartnerMapper.ToDto(createdPartner));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Обновить партнера (Head, Chairman)
        [HttpPut("{id}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<PartnerDto>> UpdatePartner(Guid id, [FromBody] UpdatePartnerDto partnerDto)
        {
            if (id != partnerDto.PartnerId)
                return BadRequest();

            var partnerEntity = await _partnerService.GetPartnerByIdAsync(id);
            if (partnerEntity == null)
                return NotFound();

            PartnerMapper.UpdateEntity(partnerEntity, partnerDto);
            var updatedPartner = await _partnerService.UpdatePartnerAsync(partnerEntity);
            return Ok(PartnerMapper.ToDto(updatedPartner));
        }

        // Удалить партнера (Chairman)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Chairman")]
        public async Task<ActionResult> DeletePartner(Guid id)
        {
            var result = await _partnerService.DeletePartnerAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Архивировать партнера (Head, Chairman)
        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> ArchivePartner(Guid id)
        {
            var result = await _partnerService.ArchivePartnerAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}