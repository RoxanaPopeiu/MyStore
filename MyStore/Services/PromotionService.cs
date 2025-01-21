using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;

namespace MyStore.Services
{
    public class PromotionService
    {
        private readonly ApplicationDbContext _context;
        public PromotionService(ApplicationDbContext context)
        {
            _context = context;
        }
        #region CRUD
        public PromotionDto Create(PromotionDto promotionDto)
        {
            ValidatePromotion(promotionDto);
            if (CheckPromotionExistence(promotionDto.StartDate, promotionDto.EndDate, promotionDto.Value))
                throw new PromotionNotFoundException("The promotion already exists!");
            var promotion=promotionDto.ToPromotion();
            var result=_context.Promotions.Add(promotion);
            _context.SaveChanges();
            return result.Entity.ToPromotionDto();
        }
        public List<PromotionDto> ReadAllPromotions()
        {
            var result=PromotionMapping.ToPromotionDtoList( _context.Promotions );
            return result;
        }
        public PromotionDto ReadOnePromotionById(int id)
        {
            var promotion=_context.Promotions.SingleOrDefault(X => X.Id == id);
            return promotion?.ToPromotionDto();
        }
        public PromotionDto Update(int id, PromotionDto promotionDto)
        {
            ValidatePromotion(promotionDto);
            var extPromotion = _context.Promotions.Find(id);
            if (extPromotion == null)
                throw new PromotionNotFoundException("The promotion doesn't exist!");
            if (CheckPromotionExistence(promotionDto.StartDate, promotionDto.EndDate, promotionDto.Value))
                throw new PromotionNotFoundException("The promotion value already exists!");
            extPromotion.StartDate= promotionDto.StartDate;
             extPromotion.EndDate= promotionDto.EndDate;
             extPromotion.Value= promotionDto.Value;
             _context.SaveChanges();
             return extPromotion.ToPromotionDto();
           
        }
        public bool Delete(int id)
        {
            var extPromotion = _context.Promotions.Find(id);
            if (extPromotion == null)
                return false;
            _context.Promotions.Remove(extPromotion);
            _context.SaveChanges();
            return true;
        }
        #endregion
        #region Exeption Handling
        public class PromotionNotFoundException : Exception
        {
            public PromotionNotFoundException(string message) : base(message) { }
        }
        #endregion
        #region Private Methods
        private void ValidatePromotion(PromotionDto promotionDto)
        {
            if (promotionDto.StartDate >= promotionDto.EndDate)
                throw new Exception("Start date must be before end date.");
            if (promotionDto.EndDate < DateTime.UtcNow)
                throw new Exception("End date cannot be in the past.");
            if (promotionDto.Value < 0 || promotionDto.Value > 100)
                throw new Exception("Invalid discount value.");
            // Ensure dates are UTC
            promotionDto.StartDate = DateTime.SpecifyKind(promotionDto.StartDate, DateTimeKind.Utc);
            promotionDto.EndDate = DateTime.SpecifyKind(promotionDto.EndDate, DateTimeKind.Utc);
        }
        private bool CheckPromotionExistence(DateTime startDate, DateTime endDate, double value)
        {
            return _context.Promotions.Any(X=> X.StartDate == startDate && X.EndDate==endDate && X.Value==value);
        }

        #endregion
    }
}
