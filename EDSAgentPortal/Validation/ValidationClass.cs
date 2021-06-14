using System;
using System.Collections.Generic;
using System.Text;

namespace EDSAgentPortal.Validation
{
    public class ValidationClass
    {  
        public ulong CheckPhoneNumber(string password)
        {
            ulong number;
            while (!ulong.TryParse(password, out number))
            {
                Console.WriteLine("Please enter an 11 digit number");
                Console.Write("Phone Number : ");
                password = Console.ReadLine();
            }
            return number;
        }
    }
}
