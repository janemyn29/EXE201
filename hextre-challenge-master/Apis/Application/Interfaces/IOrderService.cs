using Application.ViewModels.OrderViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        public Task<Guid> CreateOrder(OrderCreateModel model);
        public Task<string> Payment(Order order, string option);
        public Task<string> PaymentMonly(ServicePayment service, string option);

        /*    "partnerCode": "MOMOBKUN20180529",
            "accessKey": "klm05TvNBzhg7h7j",
            "secretKey": "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa",*/
    }
}
