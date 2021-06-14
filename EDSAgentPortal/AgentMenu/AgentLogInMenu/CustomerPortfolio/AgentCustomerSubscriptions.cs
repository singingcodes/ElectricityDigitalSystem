using ElectricityDigitalSystem.AgentServices;
using ElectricityDigitalSystem.AgentServices.IServices;
using ElectricityDigitalSystem.Common;
using ElectricityDigitalSystem.Common.ISubscriptionsServices;
using ElectricityDigitalSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EDSAgentPortal.AgentMenu.AgentLogInMenu.CustomerPortfolio
{
    public class AgentCustomerSubscriptions
    {
        readonly ISubscriptionServices subscriptionServices = new SubscriptionServices();

        readonly ITariffServices tariffServices = new TariffServices();

        readonly IAgentCustomerServices agentCustomerServices = new AgentCustomerServices();

        readonly CustomerPortMenuNav customerPortMenuNav = new CustomerPortMenuNav();

        decimal t1, t2, t3, t4 = default;
        string a1, a2, a3, a4 = default;

        public void CancelCustomerSubscription()
        {
            Console.Clear();
            Console.WriteLine("Would you like to cancel your active subscription?\n1 : Yes\n2 : No");
            var entry = Console.ReadLine();
            switch (entry)
            {
                case "1":
                    CancelSub();
                    break;
                case "2":
                    Console.WriteLine("Processing...");
                    Thread.Sleep(3000);
                    break;
                default:
                    CancelCustomerSubscription();
                    break;
            }
        }

        public void CancelSub()
        {
            Console.Clear();
            var userEmailCheck = EmailCheck();

            if (userEmailCheck == "Failed")
            {
                Console.WriteLine("No Valid Email Found");
                Thread.Sleep(3000);
            }
            else
            {
                var customerToBeSubscribeID = agentCustomerServices.GetCustomerByEmail(userEmailCheck).Id;
                var activeSub = subscriptionServices.CheckActiveSubscription(customerToBeSubscribeID);

                if (activeSub.Count == 0)
                {
                    Console.WriteLine("\nYou do not have an Active subscription");
                    Thread.Sleep(3000);
                }
                else
                {
                    foreach (var item in activeSub)
                    {
                        item.SubscriptionStatus = "Inactive";
                        subscriptionServices.UpdateSubscription(item);
                        Console.WriteLine("\nSubscription cancelled successfully");
                    }
                    Thread.Sleep(3000);
                }
            }
        }

        public void MakeSubscription(string registeringAgent)
        {
            Console.Clear();
            var userEmailCheck = EmailCheck();

            if (userEmailCheck == "Failed")
            {
                Console.WriteLine("No Valid Email Found");
                Thread.Sleep(3000);
            }
            else
            {
                var customerToBeSubscribeID = agentCustomerServices.GetCustomerByEmail(userEmailCheck).Id;
                var activeSub = subscriptionServices.CheckActiveSubscription(customerToBeSubscribeID);

                if (activeSub.Count != 0)
                {
                    //Show all Tariff
                    Console.WriteLine("You currently have an active subscription \nBuying a new Subscription will deactivate your previous subscription");
                    Console.WriteLine("1 : Continue \n2 : Back to Home ");
                    string entry = Console.ReadLine();

                    switch (entry)
                    {
                        case "1":
                            MakeSubscriptionPayment(registeringAgent, customerToBeSubscribeID);
                            break;
                        case "2":
                            customerPortMenuNav.CustomerPortPageMenuNav(registeringAgent);
                            break;
                        default:
                            MakeSubscription(registeringAgent);
                            break;
                    }
                }
                else
                {
                    MakeSubscriptionPayment(registeringAgent, customerToBeSubscribeID);
                }
            } 
        }

        private void MakeSubscriptionPayment(string registeringAgent, string customerToBeSubscribeID)
        {
            Console.Clear();
            var tariffs = tariffServices.GetAllTariff();
            Console.WriteLine("We have four Tariffs");
            Console.WriteLine("\nTariff Name      Tariff Price");
            for (var i = 0; i < tariffs.Count; i++)
            {
                Console.Write($"{i + 1} : {tariffs[i].Name}     #{tariffs[i].PricePerUnit} \n");
                if (i == 0)
                {
                    t1 = tariffs[i].PricePerUnit;
                    a1 = tariffs[i].Id;

                }
                else if (i == 1)
                {
                    t2 = tariffs[i].PricePerUnit;
                    a2 = tariffs[i].Id;
                }
                else if (i == 2)
                {
                    t3 = tariffs[i].PricePerUnit;
                    a3 = tariffs[i].Id;

                }
                else if (i == 3)
                {
                    t4 = tariffs[i].PricePerUnit; ;
                    a4 = tariffs[i].Id;
                }
            }
            Console.WriteLine();
            string userInput = Console.ReadLine();

            Console.Clear();
            int amountToBuy;
            switch (userInput)
            {
                case "1":
                    Console.WriteLine("How many units would you like to purchase?");
                    string firstInput = Console.ReadLine();
                    
                    while (!int.TryParse(firstInput, out amountToBuy))
                    {
                        Console.WriteLine("Invalid Input\nHow many units would you like to purchase?");  
                       firstInput = Console.ReadLine();
                    }
                    CalculateTotalUnit(amountToBuy, t1, a1, registeringAgent, customerToBeSubscribeID);
                    break;
                case "2":
                    Console.WriteLine("How many units would you like to purchase?");
                    string secondInput = Console.ReadLine();

                    while (!int.TryParse(secondInput, out amountToBuy))
                    {
                        Console.WriteLine("Invalid Input\nHow many units would you like to purchase?");
                        secondInput = Console.ReadLine();
                    }
                    CalculateTotalUnit(amountToBuy, t2, a2, registeringAgent, customerToBeSubscribeID);
                    break;
                case "3":
                    Console.WriteLine("How many units would you like to purchase?");
                    string thirdInput = Console.ReadLine();

                    while (!int.TryParse(thirdInput, out amountToBuy))
                    {
                        Console.WriteLine("Invalid Input\nHow many units would you like to purchase?");
                        thirdInput = Console.ReadLine();
                    }
                    CalculateTotalUnit(amountToBuy, t3, a3, registeringAgent, customerToBeSubscribeID);
                    break;
                case "4":
                    Console.WriteLine("How many units would you like to purchase?");
                    string fourthInput = Console.ReadLine();

                    while (!int.TryParse(fourthInput, out amountToBuy))
                    {
                        Console.WriteLine("Invalid Input\nHow many units would you like to purchase?");
                        fourthInput = Console.ReadLine();
                    }
                    CalculateTotalUnit(amountToBuy, t4, a4, registeringAgent, customerToBeSubscribeID);
                    break;
                default:
                    MakeSubscriptionPayment(registeringAgent, customerToBeSubscribeID);
                    break;
            }

        }

        private void CalculateTotalUnit(int unitToBePurchased, decimal pricePerUnit, string tariffId, string registeringAgent, string customerToBeSubscribeID)
        {
            decimal totalAmountPurchased = Convert.ToDecimal(unitToBePurchased) * pricePerUnit;
            Console.WriteLine($"You are about to pay #{totalAmountPurchased} \nDo you want to proceed? \n1 : Yes\n2 : No  ");
            string entry = Console.ReadLine();
            
            switch (entry)
            {
                case "1":
                    Console.WriteLine($"You have Successfully purchased {unitToBePurchased} Units");

                    var sub = subscriptionServices.CheckActiveSubscription(customerToBeSubscribeID);

                    foreach (var item in sub)
                    {
                        item.SubscriptionStatus = "Inactive";
                        subscriptionServices.UpdateSubscription(item);
                    }

                    Subscriptions subscriptions = new Subscriptions
                    {
                        Id = Guid.NewGuid().ToString(),
                        SubscriptionStatus = "Active",
                        CustomerId = customerToBeSubscribeID,
                        TariffId = tariffId,
                        AgentId = registeringAgent,
                        SubcriptionDateTime = DateTime.Now,
                        Amount = totalAmountPurchased,
                    };
                    
                    subscriptionServices.MakeSubscription(subscriptions);
                    Console.WriteLine("Successfull!!!");
                    Thread.Sleep(3000);
                    break;
                case "2":
                    Console.WriteLine("Cancelled!!!\nRedirecting....");
                    Thread.Sleep(3000);
                    break;
            }

        }


        public void ViewSubscriptionsHistory()
        {
            Console.Clear();
            var userEmailCheck = EmailCheck();

            if (userEmailCheck == "Failed")
            {
                Console.WriteLine("No Valid Email Found");
                Thread.Sleep(3000);
            }
            else
            {
                var customerToBeSubscribeID = agentCustomerServices.GetCustomerByEmail(userEmailCheck).Id;
                var subscriptions = subscriptionServices.GetCustomerSubscription(customerToBeSubscribeID);

                string tariffName;

                if (subscriptions.Count == 0)
                {
                    Console.WriteLine("You have not made any Subscription");
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("Subscription History\n");
                    Console.WriteLine($"{"Tariff Name", -20}\tAmount\t\t\tSubscription Date\t\tSubscription Status\n\n");
                    foreach (var subscription in subscriptions)
                    {
                        tariffName = subscription.TariffId;
                        var customerTariff = tariffServices.GetTarriffById(tariffName);
                        Console.Write($"{customerTariff.Name, -20}\t#{subscription.Amount}\t\t{subscription.SubcriptionDateTime}\t\t{subscription.SubscriptionStatus}\n");
                    }
                    Console.ReadKey();
                }
            }
        }

        private string EmailCheck()
        {
            Console.Clear();
            Console.WriteLine("Please Can I have the Email you used in Registering :");
            Console.Write("The Email Address : ");
            string email = Console.ReadLine();

            while (string.IsNullOrEmpty(email))
            {
                Console.Write("Please Enter an Email Address : ");
                email = Console.ReadLine();
            }

            var customer = agentCustomerServices.GetCustomerByEmail(email);

            if (customer == null)
            {
                return "Failed";
            }
            else
            {
                return customer.EmailAddress;
            }
        }
    }
}
