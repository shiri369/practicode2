using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlS
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public string[] Classes { get; set; }
        public Selector parent { get; set; }
        public Selector children { get; set; }
        public Selector()
        {
            Classes = new string[10];
        }
        public void add_class(string clas)
        {
            for (var i = 0; i < this.Classes.Length; i++)
            {
                if (Classes[i] == null)
                {
                    Classes[i] = clas;
                    break;
                }
            }
        }
        public static Selector st_to_obj(string query)
        {
            bool flag_tags=false;
           string [] query_arr = query.Split(' ');
            Selector root_selector=new Selector();
            Selector tmp_selector = new Selector();
            root_selector.TagName = "root";
            root_selector.children = tmp_selector;


            foreach (string que in query_arr)
            {
                if (que.IndexOf('#')!= -1)
                {
                    if (que.IndexOf('.') != -1)
                        tmp_selector.Id = que.Substring(que.IndexOf('#')+1, que.IndexOf('.') - que.IndexOf('#')-1 );
                    else
                        tmp_selector.Id = que.Substring(que.IndexOf('#') + 1);

                }

                if (que.IndexOf('.') != -1)
                {
                    
                        tmp_selector.add_class(que.Substring(que.IndexOf('.') + 1));
                   
                }
                string[] existing_tags = HtmlHelper.Instance.Existing_tags;
                foreach (string item in existing_tags)
                {
                    if (que.IndexOf('#') != -1)
                    {
                        if (item == que.Substring(0, que.IndexOf('#')))
                        {
                            //flag_tags = true;
                            tmp_selector.TagName = que.Substring(0, que.IndexOf('#'));
                        }
                    }
                     else if (que.IndexOf('.') != -1)
                    {
                        if (item == que.Substring(0, que.IndexOf('#')))
                        {
                            //flag_tags = true;
                            tmp_selector.TagName = que.Substring(0, que.IndexOf('.'));
                        }
                    }      
                    else tmp_selector.TagName = que;
                }
                  
                   // flag_tags = false;
                var new_obj = new Selector();
                tmp_selector.children = new_obj;
                tmp_selector = new_obj;
            }
          
            return root_selector;

        }
       

     }
  }

