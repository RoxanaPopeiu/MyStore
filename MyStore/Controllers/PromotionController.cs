using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromotionController(IPromotionService promotionService) :ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PromotionDto promotionDto)
        {
            var createPromotion=await promotionService.Create(promotionDto);
            return Ok(createPromotion);
        }
        [HttpGet("GetAllPromotions")]
        public async Task<List<PromotionDto>> GetPromotionList()
        {
            return await promotionService.ReadAllPromotions();
        }
        [HttpGet("GetOnePromotion/{ID:int}")]
        public async Task<PromotionDto> GetOnePromotion(int ID)
        {
            return await promotionService.ReadOnePromotionById(ID);
        }
        [HttpPut("Update/{ID:int}")]
        public async Task<PromotionDto> Update(int ID, [FromBody] PromotionDto promotionDto)
        {
            return await promotionService.Update(ID, promotionDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public async Task<bool> Delete(int ID)
        {
            return await promotionService.Delete(ID);
        }


    }
}
