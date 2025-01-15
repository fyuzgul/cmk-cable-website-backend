using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Entities
{
    public class CloudinaryInfo
    {
        private string CloudName = "dk7nt7ar5";
        private string ApiKey = "217945473356569";
        private string ApiSecret = "Pl8abWcSM1OUwYJXTk1FWrNam-I"; // Removed the extra semicolon
        public Account account;

        public CloudinaryInfo()
        {
            account = new Account(CloudName, ApiKey, ApiSecret);
        }
    }
}
