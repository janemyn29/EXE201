using Application.ViewModels.GoodViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RequestViewModels
{

    public class RequestWithGoodViewModel
    {
        public CreateRequestWithRequestDetailViewModel CreateRequestWithRequestDetailViewModel { get; set; }
        public IList<GoodCreateWithImage> GoodCreateWithImage { get; set; }
    }
}
