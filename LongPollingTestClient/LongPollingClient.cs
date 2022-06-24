using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedLibrary;

namespace HttpClientSample
{
    class LongPollingClient
    {
        static HttpClient client = new HttpClient();
        static int RetryCount = 0;
       
        //Function To Display Purchase order details on Console
        static void ShowPurchase(Purchase purchase)
        {
            Console.WriteLine($"ID: {purchase.ID}\tMerchantID: " +
                $"{purchase.MerchantID}\tAmount: {purchase.Amount}\tstatus: {purchase.Status}");
        }
       
        static void ShowCustomPurchase(Purchase purchase)
        {
            Console.WriteLine($"status: {purchase.Status}");
        }


        //Function to POST Purchase Request to Server
        static async Task<Uri> CreatePurchaseAsync(Purchase purchase)
        {
            
           HttpResponseMessage response = await client.PostAsJsonAsync(
                "purchase", purchase);
            
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Response Code: 202 ");
            Console.WriteLine(response.ReasonPhrase);

            // return URI of the created resource.          
            return response.Headers.Location;
        }   

        //Function to call GET method to read Request status
        static async Task<Purchase> GetPurchaseAsync(string path)
        {
            Purchase purchase = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                response.ReasonPhrase = "Please wait...Processing ...";
                Console.WriteLine("Response code : 302 ");
                Console.WriteLine(response.ReasonPhrase);
            }
            else if (response.IsSuccessStatusCode)
            {
                purchase = await response.Content.ReadAsAsync<Purchase>();
            }       
                                          
            return purchase;
        }

        //Entry function Main() to start HttpClient for LongPolling
        static void Main()
        {
            Console.WriteLine("========================HTTPClient application- LongPollingClient===================");
            Console.WriteLine("");
            int i=0;
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("Connection Established with Server");
            Console.WriteLine("");

            while (true)
            {
                if (RetryCount <= 2)
                {
                    RunAsync().GetAwaiter().GetResult();
                }
                else
                {
                    Console.WriteLine("Server is disconnected");
                    break;
                }
            }

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        static async Task RunAsync()
        {         
            try
            {
                Console.WriteLine("Create New Purchase Request (Y/N) ");                
                ConsoleKeyInfo result = Console.ReadKey();
                Console.WriteLine("");

                if ((result.KeyChar == 'Y') || (result.KeyChar == 'y'))
                {
                    Console.Write("Enter PurchaseID :");
                    int ID = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter MerchantID :");
                    int MerchantID = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter Amount :");
                    float Amount = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("");
                   
                    // Create a new Purchase
                    Purchase purchase = new Purchase
                    {
                        ID = ID,
                        MerchantID = MerchantID,
                        Amount = Amount,
                        Status = "Request Received"
                    };
                    var url = await CreatePurchaseAsync(purchase);
                    Console.WriteLine("");
                    Console.WriteLine("Updating Purchase Request Status");
                    Console.WriteLine("");                    

                    while (true)
                    {
                        purchase = await GetPurchaseAsync("Purchase/GetRecord/" + purchase.ID);
                        if (purchase.Status != "Success")
                        {
                            await GetPurchaseAsync(url.PathAndQuery);
                            ShowCustomPurchase(purchase);
                            Console.WriteLine("");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine("Response code : 200(OK)");
                            Console.WriteLine("Processing Completed");
                            ShowCustomPurchase(purchase);
                            Console.WriteLine("");
                            Console.WriteLine("Transaction Completed");
                            break;
                        }
                    }
                    Console.WriteLine("");

                }

                else if ((result.KeyChar == 'N') || (result.KeyChar == 'n'))
                {
                    Console.WriteLine("Please Try again");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Please Enter Valid Input (Y/N)");
                    Console.WriteLine("");
                }
            }
            catch (Exception e)
            {
                RetryCount++;
                Console.WriteLine("Trying to connect to server");
                //Console.WriteLine(e.Message);                
            }
        }
    }
}