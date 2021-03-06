﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Jarvis.addons.Logging;

namespace Jarvis
{
    class Program
    {
		private CommandService commands;
		public DiscordSocketClient _client;
		private IServiceProvider services;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			string Jarvis = "Jarvis";
			Console.Title = Jarvis;
			_client = new DiscordSocketClient();
			commands = new CommandService();

			services = new ServiceCollection()
				.BuildServiceProvider();

            //_client.UserJoined += WelcomeUser;
            _client.Log += Log;

			string token = File.ReadAllText("token.txt");

			await InstallCommands();
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();
			await _client.SetGameAsync("~help for commands");


            // Block this task until the program is closed.
            await Task.Delay(-1);
		}

        //public async Task UserJoined()
        //{
        //    _client.UserJoined += WelcomeUser;
        //}
        //public async Task WelcomeUser(SocketGuildUser user)
        //{
        //    Console.WriteLine("Welcome1");
        //    //need to find channel to send message in
        //    var channel = user.Guild.DefaultChannel; //gets channel to send message in
        //    Console.WriteLine("Welcome2");
        //    await channel.SendMessageAsync("Welcome " + user.Mention + " to the server!"); //Welcomes the new user
        //    Console.WriteLine("Welcome3");
        //    //await Context.Channel.SendMessageAsync("Welcome" + user.Mention + "to the server!");
        //    //Console.WriteLine(DateTime.Now.ToString() + "	Welcome | Guild: " + Context.Guild.Name + " | Channel: " + Context.Channel.Name + " | User " + Context.User.Username + "");
        //    Console.WriteLine("Welcome4");
        //}

        public async Task InstallCommands()
		{
			// Hook the MessageReceived Event into our Command Handler
			_client.MessageReceived += HandleCommand;
			// Discover all of the commands in this assembly and load them
			await commands.AddModulesAsync(Assembly.GetEntryAssembly());
		}

		public async Task HandleCommand(SocketMessage messageParam)
		{
			// Don't process the command if it was a System Message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;
			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;
			// Determine if the message is a command, based on if it starts with '!' or a mention prefix
			if (!(message.HasCharPrefix('~', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
			// Create a Command Context
			var context = new CommandContext(_client, message);
			// Execute the command. (result does not indicate a return value, 
			// rather an object stating if the command executed successfully)
			var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                Logger.LoggerInstance.Error(message.ToString(), context.Guild, context.Channel, result.ErrorReason);
				//await context.Channel.SendMessageAsync(result.ErrorReason);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
