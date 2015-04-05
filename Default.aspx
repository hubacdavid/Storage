<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
    EnableEventValidation="false"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Hubač.Storage</h1>
        <p class="lead">Storage of all awesome things.</p>
        <div>
            <input type="hidden" runat="server" ID="moviess" Value="false" Visible="false"/>
            <input type="hidden" runat="server" ID="gamess" Value="false" Visible="false"/>
            <input type="hidden" runat="server" ID="bookss" Value="false" Visible="false"/>
            <input type="hidden" runat="server" ID="musicc" Value="false" Visible="false"/>
        </div>
    <a href="Default.aspx?content=movies" class="btn btn-default">Movies</a>
    <a href="Default.aspx?content=music" class="btn btn-default">Music</a>
    <a href="Default.aspx?content=books" class="btn btn-default">Books</a>
    <a href="Default.aspx?content=games" class="btn btn-default">Games</a>
    <div>        
    </div>
    <%if(!_music && !_movies && !_games && !_books)
    {%>
    <div class="row">
        <div class="col-md-4">
            <iframe width=811 height=600 src="https://www.youtube.com/embed/5g8ykQLYnX0" frameborder="0" allowfullscreen></iframe>
        </div>
    </div>
    <%} else if(_movies || _music || _games || _books)
    {
        var theOne = _movies ? "MOVIES" : (_music ? "MUSIC" : (_games ? "GAMES" : "BOOKS"));
          %>
        <h2><%=theOne %></h2>
    <%
          if(_itemDict != null)
          {
              %>
            <form method="post">
                <input id="action" type="hidden" name="action" value="" />
                <input id="category" type="hidden" name="category" value="<%=theOne.ToLower() %>"/>
                <table style="width:100%">
                  <tr style="width: 80%">
                    <th style ="background-color: black; color: white; width: 10%;">Name</th>
                    <th class="mainTable1">Description</th>
                    <th class="actions">Action</th>
                  </tr>
              <%
              int counter = 0;
              foreach(var item in _itemDict.Keys)
              {%>
              <tr>
                  <td>
                      <div>
                          <p>
                              <%=item %> 
                          </p>
                      </div>
                  </td>
                  <td>
                          <p>
                              <%=_itemDict[item] %> (<a id="<%=item %>" href="#" onclick="setDescript('<%=HttpUtility.HtmlEncode(_itemDict[item]) %>', '<%=item %>')">Edit</a>)
                          </p>
                          <%if(theOne.Equals("MOVIES", StringComparison.OrdinalIgnoreCase))
                          { %>
                          <a href="Default.aspx?content=<%=theOne.ToLower() %>&rid=<%=item %>&action=play">Play</a>
                          <%} 
                            else if(theOne.Equals("MUSIC", StringComparison.OrdinalIgnoreCase))
                            { %>
                          <a href="http://youtube.com/results?search_query="<%=item %>>Youtube search</a>
                            <% }%>
                  </td> 
                  <td>
                      <input type="hidden" name="<%=theOne + ":" + counter %>" id="<%=theOne.ToLower() + ":" + counter %>" value="<%=item %>"/>
                      <input type="hidden" name="itemSelected" value="<%=item %>" id="itemIndex"/>
                      <a href="Default.aspx?content=<%=theOne.ToLower() %>&rid=<%=item %>&action=delete" class="btn btn-default">DELETE</a>
                  </td>
              </tr>
              <%
                  counter++;
              }
               %>
              <tr style="width: 80%">
                  <td>
                      <input id="addedName" name="addedName" type="text" />
                  </td>
                  <td>
                      <input id="addedDescript" name="addedDescript" type="text" style="width: 100%"/>
                  </td>
                  <td>
                      <input type="submit" class="btn btn-default" value="ADD" onclick="document.getElementById('action').value = 'add';"/>
                  </td>
              </tr>              
              </table>
            </form>
    <%
          }
    } %>
    <script>
        function setDescript(previousName, id)
        {
            var newDesc = prompt("Enter new descript!", previousName);
            if (newDesc != null && newDesc != '' && previousName != newDesc)
            {
                var element = document.getElementById(id);
                element.innerText = newDesc;
                var url = updateQueryStringParameter(document.URL, 'action', 'update');
                url = updateQueryStringParameter(url, 'rid', id);
                url = updateQueryStringParameter(url, 'newvalue', newDesc);
                window.location.href = url;
            }            
        }

        function updateQueryStringParameter(uri, key, value) {
            var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
            var separator = uri.indexOf('?') !== -1 ? "&" : "?";
            if (uri.match(re)) {
                return uri.replace(re, '$1' + key + "=" + value + '$2');
            }
            else {
                return uri + separator + key + "=" + value;
            }
        }
    </script>
    <style>
        .mainTable1 {
            width:50%; 
            margin: 40px auto;
            padding-left: 10px;
            background-image: -webkit-linear-gradient(left, black, white); /* For Safari 5.1 to 6.0 */
            background-image: -o-linear-gradient(left, black, white); /* For Opera 11.1 to 12.0 */
            background-image: -moz-linear-gradient(left, black, white); /* For Firefox 3.6 to 15 */
            background-image: linear-gradient(to right, black, white); /* Standard syntax (must be last) */
            color: white;
        }
    </style>
</asp:Content>
