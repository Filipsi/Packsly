using Packsly.Configuration;
using Packsly.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Packsly {

    public class Packsly {

        internal static readonly OperationManager Manager = new OperationManager();

        static void Main(string[] args) {
            Config.Load();

            // args = new string[] { "-seek", "packsly.filipsi.net" };
            // args = new string[] { "-install", "seek", "testpack" };
            // args = new string[] { "-install", "packsly.filipsi.net", "testpack" };
            // args = new string[] { "-help" };
            args = new string[] { "-update", @"C:\Users\Filipsi\Documents\MultiMC\instances\horizon" };


            if(args.Length == 0) {
                Console.WriteLine("Welcome! Use -help to see how to use Packsly.");
            } else {
                //try {
                    Manager.Execute(args);
                //} catch(Exception ex) {
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    Console.WriteLine(string.Format("Error: {0}", ex.Message));
                //    Console.ResetColor();
                //}

                Thread.Sleep(2000);
            }

        }

    }

}
