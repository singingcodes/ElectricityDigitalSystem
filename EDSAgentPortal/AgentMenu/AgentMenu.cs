using EDSAgentPortal.AgentMenu.AgentLogInMenu;
using EDSAgentPortal.Validation;
using ElectricityDigitalSystem.AgentServices;
using ElectricityDigitalSystem.AgentServices.IServices;
using ElectricityDigitalSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EDSAgentPortal.AgentMenu
{
    public class AgentMenu
    {

        readonly IAgentServices agentServices = new AgentServices();

        readonly AgentMenuNav agentMenuNav = new AgentMenuNav();

        readonly ValidationClass validation = new ValidationClass();

        readonly LogInMenuNav loginMenuNav = new LogInMenuNav();

        public void RegisterAgent()
        {
            Dictionary<string, string> navItemDIc = new Dictionary<string, string>();
            List<string> navigationItems = new List<string>
            {
                "FirstName", "LastName", "Email", "Password", "PhoneNumber"
            };
            Console.Clear();
            Console.WriteLine("Please Provide your Details : ");

            for (int i = 0; i < navigationItems.Count; i++)
            {
                Console.Write($"Enter your {navigationItems[i]} : ");
                var value = Console.ReadLine();

                while (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine($"{navigationItems[i]} cannot be left blank");
                    Console.Write($"{navigationItems[i]} : ");
                    value = Console.ReadLine();
                }

                navItemDIc.Add(navigationItems[i], value);
            }

            string FirstName, LastName, Email, Password, PhoneNumber;
            FirstName = navItemDIc["FirstName"];
            LastName = navItemDIc["LastName"];
            Email = navItemDIc["Email"];
            Password = navItemDIc["Password"];
            PhoneNumber = navItemDIc["PhoneNumber"];

            ulong number = validation.CheckPhoneNumber(PhoneNumber);
            
            AgentsModel model = new AgentsModel
            {
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = Email,
                Password = Password,
                PhoneNumber = number.ToString("00000000000"),
            };

            string registrationResponds = agentServices.RegisterAgent(model);
            if (registrationResponds == "Success")
            {
                Console.WriteLine();
                Console.WriteLine("Registration Successful");
                Console.WriteLine("Redirecting you to Home Page....");
                Thread.Sleep(3000);
            }
            else
                Console.WriteLine("An Error occured While Trying to Create your Account Please try Again");

            agentMenuNav.PageMenuNav();
           
        }

        public void LoginAgent()
        {
            string Email, Password;

            Console.Clear();
            Console.WriteLine("Please Login with your Email and Password");
            Console.Write($"Please Enter your Email : ");
            Email = Console.ReadLine();
            Console.Write($"Please Enter your Password : ");
            Password = Security.ReadPassword();

            var agent = agentServices.GetAgentByEmail(Email);
            
            if (agent == null)
            {
                Console.WriteLine("No Email Found");
                Thread.Sleep(3000);

                agentMenuNav.PageMenuNav();
            }
            else
            {
                if (agent.Password != Password)
                {
                    Console.WriteLine("Invalid Login Credentials \nPlease Create an Account!");
                    Thread.Sleep(3000);

                    agentMenuNav.PageMenuNav();
                }
                else
                {
                    //call the loginPageNav
                    loginMenuNav.LogInPageMenuNav(agent.Id, agent.FirstName, agent.LastName);

                }
            }
        }
    }
}

