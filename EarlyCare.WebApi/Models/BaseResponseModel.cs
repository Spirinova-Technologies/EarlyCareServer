using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class BaseResponseModel
    {
        public string Message { get; set; }
        public int Status { get; set; }

        public object Result { get; set; }
    }
}
