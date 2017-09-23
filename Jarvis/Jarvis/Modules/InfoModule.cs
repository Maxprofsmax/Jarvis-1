﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Modules;
using Discord.Commands;
using Discord.WebSocket;

namespace Jarvis
{
    public class InfoModule : ModuleBase
    {

		[Command("help"), Summary("Displays a list of commands.")]
		public async Task Help([Remainder, Summary("The list of commands")] String help = null)
		{
			await Context.Channel.SendMessageAsync(
							$"{Format.Bold("Commands")}\n\n" +
							$"{ Format.Bold("Info Commands")}\n" +
							$"- ~help - Displays a list of commands \n" +
							$"- ~users  - Displays the amount of users connected to this server \n" +
							$"- ~say x - Repeats x message \n" +
							$"- ~square x - Squares x number \n" +
							$"- ~userinfo x- Displays user name with Discord tag number \n" +
							$"- ~invlink - Displays the link to invite Jarvis to a server \n" +
							$"- ~info  - Displays info about Jarvis \n" +
							$"- ~ping  - Replies if Jarvis is online \n" +

							$"{ Format.Bold("Admin Commands")}\n" +
							$"- ~purge x - Deletes x number of messages from the text channel \n" 

							);
			Console.WriteLine(DateTime.Now.ToString() + "	Help | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

		// ~say hello -> hello
		[Command("say"), Summary("Echos a message.")]
		public async Task Say([Remainder, Summary("The text to echo")] string echo)
		{
			// ReplyAsync is a method on ModuleBase
			await ReplyAsync(echo);
			Console.WriteLine(DateTime.Now.ToString() + "	Say | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + " | Message: " + echo + "");
		}

		// ~sample square 20 -> 400
		[Command("square"), Summary("Squares a number.")]
		public async Task Square([Summary("The number to square.")] int num)
		{
			// We can also access the channel from the Command Context.
			await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
			Console.WriteLine(DateTime.Now.ToString() + "	Square| Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + " | Number: " + num + "");
		}

		// ~sample userinfo --> foxbot#0282
		// ~sample userinfo @Khionu --> Khionu#8708
		// ~sample userinfo Khionu#8708 --> Khionu#8708
		// ~sample userinfo Khionu --> Khionu#8708
		// ~sample userinfo 96642168176807936 --> Khionu#8708
		// ~sample whois 96642168176807936 --> Khionu#8708
		[Command("userinfo"), Summary("Returns info about the current user, or the user parameter, if one passed.")]
		[Alias("user", "whois")]
		public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
		{
			var userInfo = user ?? Context.Client.CurrentUser;
			await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
			Console.WriteLine(DateTime.Now.ToString() + "	UserInfo | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

		private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
		private static string GetBitness() => $"{IntPtr.Size * 8}-bit";
		private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
		// ~say info -> displays info
		[Command("info"), Summary("Displays bot info.")]
		public async Task Info([Remainder, Summary("The info")] String info = null)
		{
			var guilds = await Context.Client.GetGuildsAsync();
			var guildcount = guilds.Count();
			var channels = await Context.Guild.GetChannelsAsync();
			var channelscount = channels.Count();
			var users = await Context.Guild.GetUsersAsync();
			var userscount = users.Count();
			await Context.Channel.SendMessageAsync("" +
							$"{ Format.Bold("Info")}\n" +
							$"- Author: @Tiiiimster#0946 \n" +
							$"- Library: {"Discord.Net Core"} ({DiscordConfig.APIVersion})\n" +
							$"- Runtime: {".NETCore v2.0"} {GetBitness()}\n" +
							$"- Uptime: {GetUptime()}\n\n" +


							$"{ Format.Bold("Stats")}\n" +
							$"- Heap Size: {GetHeapSize()} mb\n" +
							$"- Servers: {guildcount}\n" +
							$"- Channels: {channelscount} (in this guild) \n" +
							$"- Users: {userscount} (in this guild) \n" 
							);
			Console.WriteLine(DateTime.Now.ToString() + "	Info | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

		[Command("ping"), Summary("Replies to prove Jarvis is online")]
		private async Task Ping()
		{
			
			await Context.Channel.SendMessageAsync("Pong, I guess. I do get *tired* of this you know...");
			Console.WriteLine(DateTime.Now.ToString() + "	Ping | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

		[Command("users"), Summary("Gets the amount of users in the server")]
		private async Task GetUsers()
		{
			var count = await Context.Guild.GetUsersAsync();
			var users = count.Count();
			await Context.Channel.SendMessageAsync($"There are currently {users} users in this server!");
			Console.WriteLine(DateTime.Now.ToString() + "	GetUsers | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

		[Command("invlink"), Summary("Displays the link to invite Jarvis to a server")]
		private async Task InvLink()
		{
			await Context.Channel.SendMessageAsync($"Jarvis invite link: <https://discordapp.com/oauth2/authorize?client_id=236013160228716544&scope=bot&permissions=506985687>");
			Console.WriteLine(DateTime.Now.ToString() + "	InvLink | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

		[Command("rolecount"), Summary("Displays the amount of users in this role")]
		private async Task RoleCount(SocketRole role)
		{
			var rolecount = role.Members.Count();
			await Context.Channel.SendMessageAsync($"There are currently **{rolecount}** users with this role in **{Context.Guild.Name}**!");
			Console.WriteLine(DateTime.Now.ToString() + "	RoleCount | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

	}
}
