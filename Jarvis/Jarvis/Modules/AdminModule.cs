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


namespace Jarvis.Modules
{
    public class AdminModule : ModuleBase
    {
		[Command("purge", RunMode = RunMode.Async)]
		[Summary("Deletes the specified amount of messages.")]
		[RequireUserPermission(GuildPermission.Administrator)]
		[RequireBotPermission(ChannelPermission.ManageMessages)]
		public async Task PurgeChat(uint amount)
		{

			var messages = await this.Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();

			await this.Context.Channel.DeleteMessagesAsync(messages);
			const int delay = 5000;
			var m = await this.ReplyAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
			await Task.Delay(delay);
			await m.DeleteAsync();

			Console.WriteLine(DateTime.Now.ToString() + "	PurgeChat | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + " | Amount: " + amount + "");
		}

		//[Command("purgeu", RunMode = RunMode.Async)]
		//[Summary("Deletes the specified amount of messages.")]
		//[RequireUserPermission(GuildPermission.Administrator)]
		//[RequireBotPermission(ChannelPermission.ManageMessages)]
		//public async Task PurgeUChat(IGuildUser user = null, uint amount)
		//{

		// keep getting messages until we have amount of user messages aquired
		//	var messages = await this.Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
		//	//if(messages.
		// going to have to use linq to sort through messages here



		//	await this.Context.Channel.DeleteMessagesAsync(messages);
		//	const int delay = 5000;
		//	var m = await this.ReplyAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
		//	await Task.Delay(delay);
		//	await m.DeleteAsync();

		//	Console.WriteLine(DateTime.Now.ToString() + "	PurgeUChat | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + " | Amount: " + amount + "");
		//}

		[RequireUserPermission(GuildPermission.ManageRoles)]
		[RequireBotPermission(GuildPermission.ManageRoles)]
		[Command("rolecount"), Summary("Displays the amount of users in this role")]
		private async Task RoleCount(SocketRole role)
		{
			var rolecount = role.Members.Count();
			await Context.Channel.SendMessageAsync($"There are currently **{rolecount}** users with this role in **{Context.Guild.Name}**!");
			Console.WriteLine(DateTime.Now.ToString() + "	RoleCount | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + "");
		}

        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Command("addrole"), Summary("Creates a new joinable role")]
        private async Task AddRole([Remainder, Summary("Role name")] String input = null)
        {
            if (input.StartsWith("+"))
            {
                await Context.Guild.CreateRoleAsync(input);
                await Context.Channel.SendMessageAsync("Created role: **" + input + "**");
            }
            else
            {
                await Context.Guild.CreateRoleAsync("+" + input);
                await Context.Channel.SendMessageAsync("Created role: **" + input + "**");
            }
        }

        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Command("deleterole"), Summary("Deletes a role")]
        private async Task DeleteRole([Remainder, Summary("Role name")] String input = null)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == input);
            await role.DeleteAsync();
            await Context.Channel.SendMessageAsync("Deleted role: **" + input + "**");
        }


        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Command("join"), Summary("Joins a joinable role")]
        private async Task Join([Remainder, Summary("Role name")] String input = null)
        {
            if (input.StartsWith("+"))
            {
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == input);

                if (role != null)
                {
                    SocketGuildUser user = Context.User as SocketGuildUser;

                    if (user.Roles.Contains(role))
                    {
                        await Context.Channel.SendMessageAsync(user.Mention + " already has the role **" + role.ToString() + "**!");
                    }
                    else
                    {
                        await user.AddRoleAsync(role);
                        await Context.Channel.SendMessageAsync("Successfully gave " + user.Mention + " **" + role.ToString() + "**");
                    }
                }
            }
            else
                await Context.Channel.SendMessageAsync("Unable to find role: **" + input + "**");

        }

        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Command("listroles"), Summary("Lists joinable roles")]
        private async Task ListRole()
        {
            await Context.Channel.SendMessageAsync("Joinable roles: ");
        }



    }
}
