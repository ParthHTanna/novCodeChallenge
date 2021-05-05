using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            List<Teller> tellerSpecZero = new List<Teller>();
            List<Teller> tellerSpecOne = new List<Teller>();
            List<Teller> tellerSpecTwo = new List<Teller>();
            List<Teller> tellerSpecThree = new List<Teller>();

            foreach (Teller teller in tellers.Teller)
            {
                if (teller.specialtyType == "0") {
                    tellerSpecZero.Add(teller);
                }
                if (teller.specialtyType == "1") {
                    tellerSpecOne.Add(teller);
                }
                if (teller.specialtyType == "2") {
                    tellerSpecTwo.Add(teller);
                }
                if (teller.specialtyType == "3") {
                    tellerSpecThree.Add(teller);
                }
            }

            int i = 0, j = 0, k = 0, l = 0;

            foreach (Customer customer in customers.Customer)
            {
                if (customer.type == "1") {
                    var appointmentOne = new Appointment(customer, tellerSpecOne[j]);
                    appointmentList.Add(appointmentOne);
                    j++;
                    if (j >= tellerSpecOne.Count) {
                        j = 0;
                    }
                }
                if (customer.type == "2") {
                    var appointmentTwo = new Appointment(customer, tellerSpecTwo[k]);
                    appointmentList.Add(appointmentTwo);
                    k++;
                    if (k >= tellerSpecTwo.Count) {
                        k = 0;
                    }
                }
                if (customer.type == "3") {
                    var appointmentThree = new Appointment(customer, tellerSpecThree[l]);
                    appointmentList.Add(appointmentThree);
                    l++;
                    if (l >= tellerSpecThree.Count) {
                        l = 0;
                    }
                }
                if (customer.type == "4") {
                    var appointmentZero = new Appointment(customer, tellerSpecZero[i]);
                    appointmentList.Add(appointmentZero);
                    i++;
                    if (i >= tellerSpecZero.Count) {
                        i = 0;
                    }
                }

            }
        }
       
        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
        }

    }
}
