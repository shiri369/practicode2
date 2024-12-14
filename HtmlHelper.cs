using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HtmlS
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance=> _instance; //עצם יחידי שרק ממנו ישתמשו ורק פעם אחת הוא נוצר
        public string [] Existing_tags { get; set; }
        public string [] Open_tags { get; set; }
       // public string[]  tags_arr { get; set; }
        private HtmlHelper() //מנענו גישה
        {
            Existing_tags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("HtmlTags.json"));
            Open_tags     = JsonSerializer.Deserialize<string[]>(File.ReadAllText("HtmlVoidTags.json")); 
        }
        
    }
}
