using Microsoft.AspNetCore.Mvc;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Services;

namespace MyStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromotionController:ControllerBase
    {
        public PromotionService promotionService { get; set; }
        public PromotionController(PromotionService promotionService)
        {
            this.promotionService= promotionService;
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] PromotionDto promotionDto)
        {
            var createPromotion=promotionService.Create(promotionDto);
            return Ok(createPromotion);
        }
        [HttpGet("GetAllPromotions")]
        public List<PromotionDto> GetPromotionList()
        {
            return promotionService.ReadAllPromotions();
        }
        [HttpGet("GetOnePromotion/{ID:int}")]
        public PromotionDto GetOnePromotion(int ID)
        {
            return promotionService.ReadOnePromotionById(ID);
        }
        [HttpPut("Update/{ID:int}")]
        public PromotionDto Update(int ID, [FromBody] PromotionDto promotionDto)
        {
            return promotionService.Update(ID, promotionDto);
        }
        [HttpDelete("Delete/{ID:int}")]
        public bool Delete(int ID)
        {
            return promotionService.Delete(ID);
        }


    }
}
