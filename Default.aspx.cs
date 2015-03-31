using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Linq;

public partial class _Default : Page
{
    protected bool _movies = false;
    protected bool _books = false;
    protected bool _music = false;
    protected bool _games = false;
    protected Dictionary<string, string> _itemDict;
    protected string current = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //TODO:
            //figure out why clicking ADD always shows EventHandler on PAge error. Postback shit.
                string content = "";
                if (Request.QueryString["content"] != null)
                {
                    content = Request.QueryString["content"].Trim();
                    if (Request.QueryString["action"] != null)
                    {
                        var action = Request.QueryString["action"].Trim();
                        if (action == "delete")
                        {
                            var queryString = Request.QueryString["rid"];
                            DeleteItem(queryString, content);
                        }
                    }
                    var action2 = Request.Form["action"];

                    if (action2 != null && action2.Trim() == "add")
                    {
                        var itemName = Request.Form["addedName"];
                        var description = Request.Form["addedDescript"];
                        AddItem(itemName, description, content);
                    }
                }
                if (content != null)
                {
                    SetContent(content);
                    _itemDict = LoadContent(content);
                }
        }
        catch (Exception)
        {

        }
    }

    private void AddItem(string itemName, string itemDescript, string category)
    {
        if(itemName + "" != string.Empty && itemDescript + "" != string.Empty)
        {
            string path = @"D:\mojweb\Storage\Storage\db\" + category + ".xml";
            XDocument doc;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                doc = XDocument.Load(fs);
                XElement xmlTree = new XElement("item",
                    new XElement("name", itemName), new XElement("description", itemDescript)
                );
                doc.Descendants("items").First().Add(xmlTree);
            }
            doc.Save(path);
        }        
    }

    private void DeleteItem(string itemName, string category)
    {
        string path = @"D:\mojweb\Storage\Storage\db\" + category + ".xml";
        XDocument doc;
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
        {
            doc = XDocument.Load(fs);
            doc.Descendants("item").Where(x => x.Elements().First().Value == itemName).Remove();
        }
        doc.Save(path);
    }

    private void SetContent(string contentType)
    {
        _movies = false;
        _music = false;
        _books = false;
        _games = false;
        switch(contentType)
        {
            case "movies":
                _movies = true;
                break;
            case "music":
                _music = true;
                break;
            case "games":
                _games = true;
                break;
            case "books":
                _books = true;
                break;
            default: break;
        }
    }

    private Dictionary<string, string> LoadContent(string contentType)
    {
        using (FileStream fs = new FileStream(@"D:\mojweb\Storage\Storage\db\" + contentType + ".xml", FileMode.Open, FileAccess.ReadWrite))
        {
            var itemDescritions = new Dictionary<string, string>();
            XDocument doc = XDocument.Load(fs);
            IEnumerable<XElement> movies = doc.Descendants("item");
            foreach (var item in movies)
            {
                itemDescritions.Add(item.Elements().Where(x => x.Name == "name").FirstOrDefault().Value,
                                    item.Elements().Where(y => y.Name == "description").FirstOrDefault().Value);          
            }
            return itemDescritions;
        }
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {

    }
}