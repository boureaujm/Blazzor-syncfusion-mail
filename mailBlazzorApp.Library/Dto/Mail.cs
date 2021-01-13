using System;

namespace mailBlazzorApp.Library.Dto 
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Mail    {
        public string Subject { get; set; } 
        public string Recipient { get; set; } 
        public string Sender { get; set; } 
        public  DateTime? Date { get; set; }
        public string Html { get; set; }
        public long Uid { get; set; }
    }
}