using LongPollingServer.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using System.Data;

namespace Server.Controllers
{
    [ApiController]
    [Route ("[Controller]")] 
    public class PurchaseController : ControllerBase        
    {
       
        [HttpPost("")]
        public AcceptedAtActionResult PostAddPurchase(Purchase Purchase)
        {
            Console.WriteLine("Purchase Request has been received ");
            Purchase.AddPurchase(Purchase);            
            return AcceptedAtAction("Get",new {ID= Purchase.ID},Purchase);

        }

        [HttpGet]
        [Route("GetAllRecords")]
        public List<Purchase> GetAllRecords()
        {
            int ID = 0;
            int MerchantID = 0;
            double Amount = 0;
            string Status = String.Empty;
            var P = new Purchase();
            List<Purchase> AllPurchaseRecords = new List<Purchase>() ;
            DataTable dt = P.GetAllRecordsPurchase();

            foreach (DataRow row in dt.Rows)
            {
                var Purchase = new Purchase();
                ID = row.Field<int>("ID");
                MerchantID = row.Field<int>("MerchantID");
                Amount = row.Field<double>("Amount");
                Status = row.Field<string>("Status");
                Purchase.ID = ID;
                Purchase.MerchantID = MerchantID;
                Purchase.Amount = Convert.ToSingle(Amount);
                Purchase.Status = Status;
                AllPurchaseRecords.Add(Purchase);               

            }           

            return AllPurchaseRecords;
           
        }

        [HttpGet("{id}")]
        public RedirectResult Get([FromRoute] int id)
        {
            int ID = 0;
            int MerchantID =0;
            double Amount =0;
            string Status = String.Empty;
            var Purchase = new Purchase() { ID = id };
            DataTable dt = Purchase.GetPurchase(id);           

            foreach (DataRow row in dt.Rows)
            {
                 ID = row.Field<int>("ID");  
                 MerchantID = row.Field<int>("MerchantID");   
                 Amount = row.Field<double>("Amount"); 
                 Status = row.Field<string>("Status");
               
            }            
            Purchase.ID = ID;
            Purchase.MerchantID = MerchantID;
            Purchase.Amount = Convert.ToSingle(Amount);
            Purchase.Status = Status;
            
            return Redirect("/Purchase/"+Purchase.ID);

        }

        [Route("GetRecord/{id}")]
        public Purchase GetRecord(int id)
        {
            int ID = 0;
            int MerchantID = 0;
            double Amount = 0;
            string Status = String.Empty;
            var Purchase = new Purchase() { ID = id };
            DataTable dt = Purchase.GetPurchase(id);
            foreach (DataRow row in dt.Rows)
            {
                ID = row.Field<int>("ID");
                MerchantID = row.Field<int>("MerchantID");
                Amount = row.Field<double>("Amount");
                Status = row.Field<string>("Status");

            }            
            Purchase.ID = ID;
            Purchase.MerchantID = MerchantID;
            Purchase.Amount = Convert.ToSingle(Amount);
            Purchase.Status = Status;
            return Purchase;           
        }

    }
}
