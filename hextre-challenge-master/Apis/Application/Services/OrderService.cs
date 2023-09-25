using Application.Interfaces;
using Application.ViewModels.OrderViewModels;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService: IOrderService
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(IUnitOfWork unit,UserManager<ApplicationUser> userManager)
        {
            _unit = unit;
            _userManager = userManager;
        }
        public async Task CreateOrder(OrderCreateModel model )
        {
            var Customer = await _userManager.Users.SingleOrDefaultAsync(x => x.Id.ToLower().Equals(model.CustomerId.ToLower()));
            if (Customer == null)
            {
                throw new Exception("Không tìm thấy khách hàng bạn yêu cầu");
            }
            var warehouseDetail = await _unit.WarehouseDetailRepository.GetByIdAsync(model.WarehouseDetailId);
            if (warehouseDetail == null)
            {
                throw new Exception("Không tìm thấy chi tiết kho bạn yêu cầu");
            }
            var order = new Order();
            order.CustomerId = model.CustomerId;
            order.WarehouseDetailId = model.WarehouseDetailId;
            order.ContactInDay = false;
            order.TotalCall = 0;
            order.OrderStatus = OrderStatus.Pending;
            order.WarehousePrice = warehouseDetail.WarehousePrice;
            order.ServicePrice = warehouseDetail.ServicePrice;
            order.TotalPrice = warehouseDetail.WarehousePrice + warehouseDetail.ServicePrice;
            order.Deposit = warehouseDetail.WarehousePrice;
            order.Width = warehouseDetail.Width;
            order.Height = warehouseDetail.Height;
            order.Depth = warehouseDetail.Depth;
            order.UnitType = warehouseDetail.UnitType;
            order.PaymentStatus = PaymentStatus.Waiting;

            await _unit.OrderRepository.AddAsync(order);
            await _unit.SaveChangeAsync();
        }
    }
}
