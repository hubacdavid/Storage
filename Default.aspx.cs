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
    //CRUD Operations working. Play still needs some attention with duplicities and file ending solution (in case .srt is before .avi, don't run .srt file).
    protected bool _movies = false;
    protected bool _books = false;
    protected bool _music = false;
    protected bool _games = false;
    protected Dictionary<string, string> _itemDict;
    protected string content = "";
    private string videosLocation = "";
    private string dbLoc = AppDomain.CurrentDomain.BaseDirectory + @"\db\";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            LoadLocation();
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
                        else
                        {
                            if (action == "play")
                            {
                                var movieToPlay = Request.QueryString["rid"];
                                PlayItem(movieToPlay);
                            }
                            else if(action == "update")
                            {
                                var itemToUpdate = Request.QueryString["rid"];
                                var newDescription = Request.QueryString["newvalue"];
                                UpdateItemDesc(itemToUpdate, newDescription);
                            }
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

    private void LoadLocation()
    {
        using (StreamReader sr = new StreamReader(new FileStream(@"D:\mojweb\videos.txt", FileMode.Open, FileAccess.ReadWrite)))
        {
            videosLocation = sr.ReadToEnd();
        }
    }

    private void UpdateItemDesc(string itemName, string newDesc)
    {
        string path = dbLoc + content + ".xml";
        XDocument doc;
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
        {
            doc = XDocument.Load(fs);
            var element = doc.Descendants("item").Where(x => x.Elements()
                                                           .First()
                                                           .Value == itemName);
            foreach(var item in element.Elements())
            {
                if(item.Name == "description")
                {
                    item.Value = newDesc;
                }
            }
        }
        doc.Save(path);
    }

    private void PlayItem(string itemName)
    {
        string[] allFiles = System.IO.Directory.GetFiles(videosLocation);
        string[] allDirectories = System.IO.Directory.GetDirectories(videosLocation);
        bool found = false;
        foreach (string file in allFiles)
        {
            if (file.Contains(itemName))
            {
                found = true;
                System.Diagnostics.Process.Start(file);
                string url = HttpContext.Current.Request.Url.AbsoluteUri + "?content=" + content;
            }
        }
        if(!found)
        {
            foreach(string dir in allDirectories)
            {
                if (dir.Contains(itemName))
                {
                    string[] insideFiles = System.IO.Directory.GetFiles(dir);
                    if(insideFiles.Length != 0)
                    {
                        foreach (var item in insideFiles)
                        {
                            string[] splitter = item.Split('\\');
                            if (splitter[splitter.Length - 1].Contains(itemName))
                            {
                                found = true;
                                System.Diagnostics.Process.Start(item);
                                string url = HttpContext.Current.Request.Url.AbsoluteUri + "?content=" + content;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void AddItem(string itemName, string itemDescript, string category)
    {
        if(itemName + "" != string.Empty && itemDescript + "" != string.Empty)
        {
            string path = dbLoc + category + ".xml";
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
        string path = dbLoc + category + ".xml";
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
        using (FileStream fs = new FileStream(dbLoc + contentType + ".xml", FileMode.Open, FileAccess.ReadWrite))
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
}