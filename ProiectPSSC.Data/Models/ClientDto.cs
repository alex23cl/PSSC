using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Data.Models
{
    public class ClientDto
    {
        public int ClientId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; }= string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
