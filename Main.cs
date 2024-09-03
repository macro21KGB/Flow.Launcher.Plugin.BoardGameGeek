using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.BoardGameGeek
{
    public class BoardGameGeek : IPlugin
    {
        private PluginInitContext _context;

        private BGGQuery _query;

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Search))
            {
                var startup = new Result
                {
                    Title = "Search for a board game",
                    SubTitle = "Type the name of a board game to search for it on BoardGameGeek",
                    IcoPath = "Images/icon.png"
                };

                return new List<Result> { startup };
            }

            var games = _query.GetGames(query.Search).Result;

            var results = games.Select(game => new Result
            {
                Title = game.Name,
                SubTitle = game.Id,
                IcoPath = "Images/icon.png",
                Action = c =>
                {
                    _context.API.OpenUrl($"https://boardgamegeek.com/boardgame/{game.Id}");
                    return true;
                }
            }).ToList();

            return results;
        }

        void IPlugin.Init(PluginInitContext context)
        {
            _context = context;
            _query = new BGGQuery();
        }
    }
}