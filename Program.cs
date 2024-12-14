
using HtmlS;
using System;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


var html = await Load("https://hebrewbooks.org/beis");
var cleanHtml = new Regex("\\s+").Replace(html, " "); //החלפה של המחרוזת רווח או \ במחרוזת ריקה
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => !string.IsNullOrWhiteSpace(s)); //חיתוך לפי שורות לפי<<
var htmlElemnt = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\" 100%\" > text </div>";
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElemnt);

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

//בנית העץ       
var root_html = new HtmlElements();
var tmp_html = new HtmlElements();
var tmp = new HtmlElements();

root_html.Name = "html";
tmp_html.Parent = root_html;
root_html.Children.Add(tmp_html);

bool flag_tags = false;
bool flag_open= false;
var i = 0;


foreach (var line in htmlLines)
{
    i++;
    var tag = line.Contains(" ") ? line.Substring(0, line.IndexOf(' ')) : line;
    if (tag.Length == 0)
    {
        continue;
    }
    if (tag == "/html")
    {
        break;
    }
    foreach (var item in HtmlHelper.Instance.Open_tags) //בדיקה אם התגית נסגרת עם תגית ללא סלאש
    {
        if (item == tag)
        {
            flag_open = true;
            break;
        }
    }
   
    if (tag[0] == '/'|| flag_open) //בדיקה אם נסגר בסלאש
    {
       if (tmp_html.Name!="html")
        tmp_html = tmp_html.Parent; 
        flag_open= false;
    }
    else 
    {
        foreach (var item in HtmlHelper.Instance.Existing_tags)//בדיקה אם התגית היא אלמנט
        {
            if (item == tag)
            {
                flag_tags = true;
                break;
            }
        }
        if (flag_tags )
        {
           Console.WriteLine("אלמנט:  " + tag);
           tmp = new HtmlElements();
           tmp.Name = tag;
           tmp.Parent = tmp_html;

            var str = line.Substring(line.IndexOf(' ') + 1);
            var attributs = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(str); //לכידת אטריביוטים
            foreach (Match att in attributs)   //סריקה
            {
                string attributeName = att.Groups[1].Value;
                string value_att = att.Groups[2].Value;
                if (attributeName == "class" && value_att != null)
                {
                    string[] value_arr = att.Groups[2].Value.Split(' ');
                    foreach (string value in value_arr) //הכנסת הקלאסים
                    {
                        tmp.add_class(value);
                        Console.WriteLine("class"+value);
                    }
                }
                if (attributeName == "id" && value_att != null)
                {
                    tmp.Id = value_att;
                    Console.WriteLine("id"+ value_att);
                }
            }
            tmp_html.Children.Add(tmp);
            Console.WriteLine(tmp.Name);
            Console.WriteLine(tmp.Parent.Name +"  אבא");
            tmp_html = tmp;
        }
       
        if (tag[0] != '/' && !flag_tags)
        {
            // Console.WriteLine(tmp.InnerHtml + "----");
            tmp.InnerHtml = tmp.InnerHtml + line.Trim();  // הוספת תוכן HTML
        }   
    }
    if (i == 150)
    {
        break;
    }
    flag_tags = false;
}


//בדיקת תקינות מחלקת selector
Console.WriteLine("selector בדיקת תקינות פונקציית ");

Selector result = Selector.st_to_obj("div#header.container div#header.container div#header.container div#header.container");
PrintSelectorTree(result);

///Descendants בדיקת תקינות פונקציה 
Console.WriteLine("Descendants בדיקת תקינות פונקציית ");

var tag_list = root_html.Descendants();
i = 0;
foreach (var tag_l in tag_list)
{
    if (i == 150)
    {
        break;
    }
    i++;
    Console.WriteLine($"Id: {tag_l.Id}");
    Console.WriteLine($"Name: {tag_l.Name}");
    Console.WriteLine($"InnerHtml: {tag_l.InnerHtml}");
    Console.WriteLine("Attributes: " + string.Join(", ", tag_l.Attributes));
    Console.WriteLine("Classes: " + string.Join(", ", tag_l.Classes));  
}

Console.WriteLine("Ancestors בדיקת תקינות פונקציית ");

//Ancestors בדיקת תקינות פונקציית 
var tag_list_parent = tmp_html.Ancestors();
i = 0;
foreach (var tag_l in tag_list_parent)
{
    if (i == 150)
    {
        break;
    }
    i++;
    //Console.WriteLine($"Name: {tag_l.Name}");
}

// רקורסיה
Console.WriteLine(" בדיקת תקינות פונקציית רקורסיה ");

result = Selector.st_to_obj("div#outer");
result = result.children;
//PrintSelectorTree(result);
List <HtmlElements> list = new List<HtmlElements>();

List<HtmlElements> list_result = root_html.find_by_selctors(result, root_html, list);

foreach (var l in list_result)
{
    Console.WriteLine($"Name--: {l.Name}");
}

//הדפסת סלקטורים
static void PrintSelectorTree(Selector selector, int level = 0)
{
    // הדפסת הרמה הנוכחית
    string indent = new string(' ', level * 2);
    Console.WriteLine($"{indent}TagName: {selector.TagName}");
    if (!string.IsNullOrEmpty(selector.Id))
        Console.WriteLine($"{indent}Id: {selector.Id}");
    if (selector.Classes != null && selector.Classes.Any())
        Console.WriteLine($"{indent}Classes: {string.Join(", ", selector.Classes)}");

    // עבור לילד אם קיים
    if (selector.children != null)
    {
        Console.WriteLine($"{indent}Children:");
        Console.WriteLine(selector.children);

        PrintSelectorTree(selector.children, level + 1);
    }
}