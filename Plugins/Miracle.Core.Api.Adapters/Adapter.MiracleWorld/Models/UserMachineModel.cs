using Library.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.MiracleWorld.Models
{
    public class UserMachineModel
    {
        [MiracleRequired]
        public string OS { get; set; }
        [MiracleRequired]
        public string OSVersion { get; set; }
        [MiracleRequired]
        public string CPU { get; set; }
        [MiracleRequired]
        public string UUID { get; set; }
    }
}
