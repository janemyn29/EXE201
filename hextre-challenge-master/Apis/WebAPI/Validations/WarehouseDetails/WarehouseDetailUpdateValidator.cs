using Application.ViewModels.WarehouseDetailViewModels;
using FluentValidation;
using Infrastructures;

namespace WebAPI.Validations.WarehouseDetails
{
    public class WarehouseDetailUpdateValidator:AbstractValidator<WarehouseDetailUpdateModel>
    {
        public WarehouseDetailUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id không được để trống.")
                .Must(IsExistDetails).WithMessage("Chi tiết kho này không tồn tại.");
            RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("WarehouseId không được để trống.")
                .Must(IsExistWarehouse).WithMessage("Kho chứa này không tồn tại.");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá tiền không được để trống.");
            RuleFor(x => x.Width).NotEmpty().WithMessage("Chiều rộng không được để trống.");
            RuleFor(x => x.Height).NotEmpty().WithMessage("Chiều cao không được để trống.");
            RuleFor(x => x.Depth).NotEmpty().WithMessage("Chiều sâu không được để trống.");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Số lượng kho không được để trống.");
            RuleFor(x => x.UnitType).NotEmpty().WithMessage("Loại kho không được để trống.");
        }

        private bool IsExistWarehouse(Guid id)
        {
            var _context = new AppDbContext();
            var cate = _context.Warehouse.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (cate == null)
            {
                return false;
            }
            return true;
        }
        private bool IsExistDetails(Guid id)
        {
            var _context = new AppDbContext();
            var cate = _context.WarehouseDetail.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (cate == null)
            {
                return false;
            }
            return true;
        }
    }
}
