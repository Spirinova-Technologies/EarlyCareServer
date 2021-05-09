using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class EmailModel
    {
        public List<string> ToEmailAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromEmail { get; set; }
        public List<string> BccEmail { get; set; }
    }
}
