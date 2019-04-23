using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePaymentServices
{
    public interface IPaymentService
    {
        string CreateGuid();
        KeyValuePair<string, StringValues> BuildGuidHeaders(string guid);
        KeyValuePair<string, StringValues> BuildGuidHeaders();
    }
}
