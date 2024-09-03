using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

public class GameResult
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }

    public GameResult(string id, string name, string description, string thumbNailUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        ThumbnailUrl = thumbNailUrl;
    }
}

public class BGGQuery
{
    private HttpClient _client;

    public BGGQuery()
    {
        _client = new HttpClient();
    }

    static private List<GameResult> ConvertXMLStringToNodes(string xml)
    {
        XmlDocument xmlDocument = new();
        xmlDocument.LoadXml(xml);
        XmlNode node = xmlDocument.DocumentElement.SelectSingleNode("/items");

        // map the xml to GameResult
        var nodes = node.SelectNodes("item");

        List<GameResult> gameResults = new();
        foreach (var item in nodes)
        {
            var nodeItem = (XmlNode)item;
            var gameResult = new GameResult(nodeItem.Attributes["id"].Value, nodeItem.SelectSingleNode("name").Attributes["value"].Value, "", "");

            gameResults.Add(gameResult);
        }

        return gameResults;
    }

    public async Task<List<GameResult>> GetGames(string gameName)
    {
        var url = $"https://www.boardgamegeek.com/xmlapi2/search?type=boardgame&query={gameName}";
        var response = await _client.GetAsync(url);
        var responseString = await response.Content.ReadAsStringAsync();
        var gameInfo = ConvertXMLStringToNodes(responseString);

        return gameInfo;
    }
}