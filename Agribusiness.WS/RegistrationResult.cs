using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agribusiness.WS
{
    public class RegistrationResult
    {
        public RegistrationResult(string referenceId, string transactionId, bool paid)
        {
            ReferenceId = referenceId;
            TransactionId = transactionId;
            Paid = paid;
        }

        public string ReferenceId { get; set; }
        public string TransactionId { get; set; }
        public bool Paid { get; set; }
    }
}
