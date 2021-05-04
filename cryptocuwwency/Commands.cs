using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Globalization;

public class Commands : BaseCommandModule
{
    public class Coin
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public double current_price { get; set; }
        public object market_cap { get; set; }
        public int market_cap_rank { get; set; }
    }
    public class MarketData
    {
        public CurrentPrice current_price { get; set; }
        public CurrentPrice high_24h { get; set; }
        public CurrentPrice low_24h { get; set; }
        public double price_change_24h { get; set; }
        public double price_change_percentage_24h { get; set; }
        public double price_change_percentage_7d { get; set; }
    }
    public class CurrentPrice
    {
        public int aed { get; set; }
        public int ars { get; set; }
        public int aud { get; set; }
        public double bch { get; set; }
        public int bdt { get; set; }
        public double bhd { get; set; }
        public int bmd { get; set; }
        public double bnb { get; set; }
        public int brl { get; set; }
        public double btc { get; set; }
        public int cad { get; set; }
        public int chf { get; set; }
        public int clp { get; set; }
        public int cny { get; set; }
        public int czk { get; set; }
        public int dkk { get; set; }
        public int dot { get; set; }
        public int eos { get; set; }
        public double eth { get; set; }
        public int eur { get; set; }
        public int gbp { get; set; }
        public int hkd { get; set; }
        public int huf { get; set; }
        public int idr { get; set; }
        public int ils { get; set; }
        public int inr { get; set; }
        public int jpy { get; set; }
        public int krw { get; set; }
        public double kwd { get; set; }
        public int lkr { get; set; }
        public double ltc { get; set; }
        public int mmk { get; set; }
        public int mxn { get; set; }
        public int myr { get; set; }
        public int ngn { get; set; }
        public int nok { get; set; }
        public int nzd { get; set; }
        public int php { get; set; }
        public int pkr { get; set; }
        public int pln { get; set; }
        public int rub { get; set; }
        public int sar { get; set; }
        public int sek { get; set; }
        public int sgd { get; set; }
        public int thb { get; set; }
        public int @try { get; set; }
        public int twd { get; set; }
        public int uah { get; set; }
        public int usd { get; set; }
        public double vef { get; set; }
        public int vnd { get; set; }
        public double xag { get; set; }
        public double xau { get; set; }
        public int xdr { get; set; }
        public int xlm { get; set; }
        public int xrp { get; set; }
        public double yfi { get; set; }
        public int zar { get; set; }
        public int bits { get; set; }
        public int link { get; set; }
        public int sats { get; set; }
    }
    public class SpecificCoin
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public Image image { get;set; }
        public int block_time_in_minutes { get; set; }
        public string hashing_algorithm { get; set; }
        public double public_interest_score { get; set; }
        public MarketData market_data { get; set; }
        public DateTime last_updated { get; set; }
    }
    public class Image
    {
        public string thumb { get; set; }
        public string small { get; set; }
        public string large { get; set; }
    }

    [Command("top5crypto")]
    public async Task CheckPrice(CommandContext ctx)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=5"),
        };

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            var obj = JsonConvert.DeserializeObject<List<Coin>>(body);
            await ctx.TriggerTypingAsync();

            var embed = new DiscordEmbedBuilder
            {
                Title = "Coin Prices",
                Description = "The 5 most popular coins and their prices in USD.",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = "Market Bot - Cryptocurrency",
                    IconUrl = "https://i.imgur.com/IOMRrG0.png"
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter { 
                    IconUrl = "https://static.coingecko.com/s/thumbnail-007177f3eca19695592f0b8b0eabbdae282b54154e1be912285c9034ea6cbaf2.png",
                    Text = "Data gathered from CoinGecko"
                },
                Color = DiscordColor.CornflowerBlue
            };

            foreach (Coin c in obj)
            {
                embed.AddField(c.name, c.current_price.ToString("N2") + "$");
            }

            await ctx.RespondAsync(embed: embed);
        }
    }

    [Command("btc")]
    public async Task CheckBTC(CommandContext ctx, string currency = "usd")
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.coingecko.com/api/v3/coins/bitcoin"),
        };

        try
        {
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                try
                {
                    var obj = (JObject)JsonConvert.DeserializeObject(body);
                    await ctx.TriggerTypingAsync();

                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"{obj["name"]} Data",
                        Description = $"Data/Analytics for {obj["name"]}",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            Name = "Market Bot - Cryptocurrency",
                            IconUrl = obj["image"]["small"].ToString()
                        },
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = "https://static.coingecko.com/s/thumbnail-007177f3eca19695592f0b8b0eabbdae282b54154e1be912285c9034ea6cbaf2.png",
                            Text = "Data gathered from CoinGecko"
                        },
                        Color = DiscordColor.CornflowerBlue
                    };

                    try
                    {
                        string price_in_currency = obj["market_data"]["current_price"][currency].Value<double>().ToString("N");
                        embed.AddField("Price", $"{price_in_currency} {currency.ToUpper()}");
                        embed.AddField("Interest Score", obj["public_interest_score"].ToString(), true);
                        embed.AddField("Block Time (min)", obj["block_time_in_minutes"].ToString(), true);
                        embed.AddField("Hashin Algorithm", obj["hashing_algorithm"].ToString(), true);
                        embed.AddField("Highest Price (24h)", $"{obj["market_data"]["high_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Lowest Price (24h)", $"{obj["market_data"]["low_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Price Change (24h)", obj["market_data"]["price_change_percentage_24h"].Value<double>().ToString("0.00") + "%");
                        embed.AddField("Price Change (7d)", obj["market_data"]["price_change_percentage_7d"].Value<double>().ToString("0.00") + "%", true);
                        await ctx.RespondAsync(embed: embed);
                    }
                    catch
                    {
                        await ctx.RespondAsync("Third world / Nonexistent (real) currency detected.");
                    }
                }
                catch
                {
                    await ctx.RespondAsync("Third world / Nonexistent (crypto) currency detected.");
                }
            }
        }
        catch
        {
            await ctx.RespondAsync("Type the full name of the coin or a real coin retard.");
        }
    }

    [Command("eth")]
    public async Task CheckETH(CommandContext ctx, string currency = "usd")
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.coingecko.com/api/v3/coins/ethereum"),
        };

        try
        {
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                try
                {
                    var obj = (JObject)JsonConvert.DeserializeObject(body);
                    await ctx.TriggerTypingAsync();

                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"{obj["name"]} Data",
                        Description = $"Data/Analytics for {obj["name"]}",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            Name = "Market Bot - Cryptocurrency",
                            IconUrl = obj["image"]["small"].ToString()
                        },
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = "https://static.coingecko.com/s/thumbnail-007177f3eca19695592f0b8b0eabbdae282b54154e1be912285c9034ea6cbaf2.png",
                            Text = "Data gathered from CoinGecko"
                        },
                        Color = DiscordColor.CornflowerBlue
                    };

                    try
                    {
                        string price_in_currency = obj["market_data"]["current_price"][currency].Value<double>().ToString("N");
                        embed.AddField("Price", $"{price_in_currency} {currency.ToUpper()}");
                        embed.AddField("Interest Score", obj["public_interest_score"].ToString(), true);
                        embed.AddField("Block Time (min)", obj["block_time_in_minutes"].ToString(), true);
                        embed.AddField("Hashin Algorithm", obj["hashing_algorithm"].ToString(), true);
                        embed.AddField("Highest Price (24h)", $"{obj["market_data"]["high_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Lowest Price (24h)", $"{obj["market_data"]["low_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Price Change (24h)", obj["market_data"]["price_change_percentage_24h"].Value<double>().ToString("0.00") + "%");
                        embed.AddField("Price Change (7d)", obj["market_data"]["price_change_percentage_7d"].Value<double>().ToString("0.00") + "%", true);
                        await ctx.RespondAsync(embed: embed);
                    }
                    catch
                    {
                        await ctx.RespondAsync("Third world / Nonexistent (real) currency detected.");
                    }
                }
                catch
                {
                    await ctx.RespondAsync("Third world / Nonexistent (crypto) currency detected.");
                }
            }
        }
        catch
        {
            await ctx.RespondAsync("Broken response.");
        }
    }

    [Command("coin")]
    public async Task CheckCoin(CommandContext ctx, string name, string currency = "eur")
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[LOG] | {name.ToUpper()} price requested from {ctx.Message.Author.Username} in currency: {currency}.");
        Console.ForegroundColor = ConsoleColor.White;
        switch (name)
        {
            case "btc":
                name = "bitcoin";
                break;
            case "eth":
                name = "ethereum";
                break;
            case "doge":
                name = "dogecoin";
                break;
            case "ltc":
                name = "litecoin";
                break;
            default:
                break;
        }

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.coingecko.com/api/v3/coins/" + name),
        };

        try
        {
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                try
                {
                    var obj = (JObject)JsonConvert.DeserializeObject(body);
                    await ctx.TriggerTypingAsync();

                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"{obj["name"]} Data",
                        Description = $"Data/Analytics for {obj["name"]}",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            Name = "Market Bot - Cryptocurrency",
                            IconUrl = obj["image"]["small"].ToString()
                        },
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = "https://static.coingecko.com/s/thumbnail-007177f3eca19695592f0b8b0eabbdae282b54154e1be912285c9034ea6cbaf2.png",
                            Text = "Data gathered from CoinGecko"
                        },
                        Color = DiscordColor.CornflowerBlue
                    };

                    try
                    {
                        string price_in_currency = obj["market_data"]["current_price"][currency].Value<double>().ToString("N");
                        embed.AddField("Price", $"{price_in_currency} {currency.ToUpper()}");
                        embed.AddField("Interest Score", obj["public_interest_score"].ToString(), true);
                        embed.AddField("Block Time (min)", obj["block_time_in_minutes"].ToString(), true);
                        embed.AddField("Hashin Algorithm", obj["hashing_algorithm"].ToString(), true);
                        embed.AddField("Highest Price (24h)", $"{obj["market_data"]["high_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Lowest Price (24h)", $"{obj["market_data"]["low_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Price Change (24h)", obj["market_data"]["price_change_percentage_24h"].Value<double>().ToString("0.00") + "%");
                        embed.AddField("Price Change (7d)", obj["market_data"]["price_change_percentage_7d"].Value<double>().ToString("0.00") + "%", true);
                        await ctx.RespondAsync(embed: embed);
                    }
                    catch
                    {
                        await ctx.RespondAsync("Third world / Nonexistent (real) currency detected.");
                    }
                }
                catch
                {
                    await ctx.RespondAsync("Third world / Nonexistent (crypto) currency detected.");
                }
            }
        }
        catch
        {
            await ctx.RespondAsync("Type the full name of the coin or a real coin retard.");
        }
    }


    [Command("stock")]
    public async Task CheckStock(CommandContext ctx, string name, string currency = "eur")
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[LOG] | {name.ToUpper()} price requested from {ctx.Message.Author.Username} in currency: {currency}.");
        Console.ForegroundColor = ConsoleColor.White;

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.coingecko.com/api/v3/coins/" + name),
        };

        try
        {
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                try
                {
                    var obj = (JObject)JsonConvert.DeserializeObject(body);
                    await ctx.TriggerTypingAsync();

                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"{obj["name"]} Data",
                        Description = $"Data/Analytics for {obj["name"]}",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            Name = "Market Bot - Stocks",
                            IconUrl = obj["image"]["small"].ToString()
                        },
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = "https://www.alphavantage.co/static/img/favicon.ico",
                            Text = "Data gathered from AlphaVantage"
                        },
                        Color = DiscordColor.CornflowerBlue
                    };

                    try
                    {
                        string price_in_currency = obj["market_data"]["current_price"][currency].Value<double>().ToString("N");
                        embed.AddField("Price", $"{price_in_currency} {currency.ToUpper()}");
                        embed.AddField("Interest Score", obj["public_interest_score"].ToString(), true);
                        embed.AddField("Block Time (min)", obj["block_time_in_minutes"].ToString(), true);
                        embed.AddField("Hashin Algorithm", obj["hashing_algorithm"].ToString(), true);
                        embed.AddField("Highest Price (24h)", $"{obj["market_data"]["high_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Lowest Price (24h)", $"{obj["market_data"]["low_24h"][currency].Value<double>().ToString("N")} {currency.ToUpper()}", true);
                        embed.AddField("Price Change (24h)", obj["market_data"]["price_change_percentage_24h"].Value<double>().ToString("0.00") + "%");
                        embed.AddField("Price Change (7d)", obj["market_data"]["price_change_percentage_7d"].Value<double>().ToString("0.00") + "%", true);
                        await ctx.RespondAsync(embed: embed);
                    }
                    catch
                    {
                        await ctx.RespondAsync("Third world / Nonexistent (real) currency detected.");
                    }
                }
                catch
                {
                    await ctx.RespondAsync("Third world / Nonexistent (crypto) currency detected.");
                }
            }
        }
        catch
        {
            await ctx.RespondAsync("Type the full name of the coin or a real coin retard.");
        }
    }
}
