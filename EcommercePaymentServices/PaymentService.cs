using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace EcommercePaymentServices
{
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// Créé les headers d'une requête avec un Guid existant
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public KeyValuePair<string, StringValues> BuildGuidHeaders(string guid)
        {
            return new KeyValuePair<string, StringValues>("guid", guid.ToString());
        }

        /// <summary>
        /// Créé les headers d'une requête avec un nouveau Guid
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, StringValues> BuildGuidHeaders()
        {
            return this.BuildGuidHeaders(CreateGuid());
        }

        /// <summary>
        /// Créé le Guid d'une transaction
        /// </summary>
        /// <returns></returns>
        public string CreateGuid()
        {
            return Guid.NewGuid().ToString();
        }


    }
}
