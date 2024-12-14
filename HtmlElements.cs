using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlS
{
     public class HtmlElements
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string [] Attributes { get; set; }
        public string [] Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElements Parent { get; set; }
        public List <HtmlElements> Children { get; set; } = new List<HtmlElements>();

        public HtmlElements()
        {
            Children = new List<HtmlElements>();
            Classes=new string[10];
            Attributes = new string[10];
        }
        public void add_class(string clas)
        {
            for (var i = 0; i< this.Classes.Length; i++)
            {
                if (Classes[i] ==null)
                {
                    Classes[i] = clas;
                    break;
                }
            }

        }
       
         public IEnumerable <HtmlElements> Descendants()
        {
            Queue<HtmlElements> objects = new Queue<HtmlElements> ();
            objects.Enqueue(this);
            HtmlElements tmp_hlmnt;

            while (objects.Count > 0)
            {
                tmp_hlmnt = objects.Dequeue ();
                yield return tmp_hlmnt;
                foreach (var child in tmp_hlmnt.Children)
                {
                        objects.Enqueue(child);
                }

            }

        }
        
         public IEnumerable<HtmlElements> Ancestors()
        {
            HtmlElements tmp_hlmnt=this;

            while (tmp_hlmnt.Parent!= null )
            {
                yield return tmp_hlmnt;
                tmp_hlmnt=tmp_hlmnt.Parent;
            }
        }
         
        public List<HtmlElements> find_by_selctors(Selector select,HtmlElements html_elemnt, List<HtmlElements> list_html)
        {
         
            var tmp = html_elemnt.Descendants();
            return find(select,  list_html, tmp.ToArray(),0);

        }
        public List<HtmlElements> find(Selector select,  List<HtmlElements> list_html, HtmlElements[] lidt_des,int i)
        {
            if (i >= lidt_des.Length)
                return list_html;

            if (lidt_des[i].Name == select.TagName && lidt_des[i].Id == select.Id)
                {
                    list_html.Add(lidt_des[i]);
                }
                find(select, list_html, lidt_des, i+1);
            if (i==20 )
            {
                return list_html;
            }
            return list_html;
        }

    }
}
