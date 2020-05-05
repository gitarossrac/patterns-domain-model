﻿using System;
using DomainModel.AppService;
using DomainModel.Domain.Checkout;

namespace DomainModel.Checkout.Terminal
{
    internal static class Program
    {
        private const string ExitCode = "c";
        private const string ShowCode = "s";

        private static void Main()
        {
            // See: https://www.codeproject.com/Questions/455766/Euro-symbol-does-not-show-up-in-Console-WriteLine
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Press any key to start checkout process!");
            Console.ReadKey(true);

            var service = new CheckoutService();
            service.Start();

            string code;

            do
            {
                Console.Write($"Bar code - or '{ExitCode}' to close checkout / '{ShowCode}' to show bill so far: ");
                code = Console.ReadLine();
                if (code == ExitCode) continue;

                if (code == ShowCode)
                {
                    Console.WriteLine("Partial bill so far:");
                    Console.WriteLine(service.GetCurrentBill());
                    continue;
                }

                try
                {
                    service.Scan(code);
                    Console.WriteLine(service.GetLastScanned());
                }
                catch (InvalidBarCodeException e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (code != ExitCode);

            service.Close();

            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(service.GetCurrentBill());
        }
    }
}
