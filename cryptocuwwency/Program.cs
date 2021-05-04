using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;


namespace cryptocuwwency
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
			var discord = new DiscordClient(new DiscordConfiguration()
			{
				Token = "[Enter your bot token here]",
				TokenType = TokenType.Bot
			});
            var commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<Commands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
