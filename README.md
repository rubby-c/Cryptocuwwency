# Cryptocuwwency
A discord bot that checks btc, eth, other coins and stock prices. Uses the coingecko public api.

## How to build?
- Make a bot application here https://discord.com/developers/applications
- Copy paste the token in [Program.cs](https://github.com/rubby-c/Cryptocuwwency/blob/main/cryptocuwwency/Program.cs).

```c#
var discord = new DiscordClient(new DiscordConfiguration()
{
    Token = "[Enter your bot token here]",
    TokenType = TokenType.Bot
});
```

- Compile.

## How to use?
- The commands on discord are:
- ```!btc```
- ```!eth```
- ```!top5crypto```
- ```!coin [name]```
- ```!stock [name]``` //Untested!
