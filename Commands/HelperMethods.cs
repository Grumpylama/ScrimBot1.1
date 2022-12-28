namespace big
{
    public partial class Commands : BaseCommandModule
    {
        private bool CheckIfValid(CommandContext ctx)
        {
            //Check if user is a bot
            if (ctx.User.IsBot)
            {
                Console.WriteLine("User is a bot. Canceling command");
                return false;
            }

            //Check if user is registered
            foreach (var user in d.Users)
            {
                if (user.Id == ctx.User.Id)
                {
                    Console.WriteLine("User is registered. Continuing command");
                    return true;
                }
            }

            Console.WriteLine("User is not registered. Canceling command");
            return false;
        }

        private List<Team> GetUsersTeams(DiscordUser user)
        {
            List<Team> UsersTeams = new List<Team>();
            foreach (Team team in d.Teams)
            {
                if (team.TeamCaptain.Id == user.Id)
                {
                    UsersTeams.Add(team);
                }
            }

            if(UsersTeams.Count == 0)
            {
                Console.WriteLine("User is not a captain of any teams");
                return null;
            }

            return UsersTeams;
        }

        private List<DiscordUser> GetUsersNotCaptain(Team t)
        {
            List<DiscordUser> users = new List<DiscordUser>();
            foreach (TeamUser member in t.TeamMembers)
            {
                if (member.User != t.TeamCaptain)
                {
                    users.Add(member.User);
                }
            }
            return users;
            
        }

        private async Task<Team> ChooseTeam(CommandContext ctx, List<Team> Teams)
        {
            string s = "Which team?";
            int i = 1;
            foreach (Team team in Teams)
            {
                s += "\n" + i + ": " + team.TeamName + " Playing " + team.game.GameName;
                i++;
            }

            while (true)
            {
                await ctx.RespondAsync(s);
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (message.Result.Content.ToLower().Contains("cancel"))
                {
                    await ctx.RespondAsync("Canceled");
                    return null;
                }
                if (Int32.TryParse(message.Result.Content, out i))
                {
                    if (i > 0 && i <= Teams.Count)
                    {
                        return Teams[i - 1];
                    }
                    else
                    {
                        await ctx.RespondAsync("Please enter a valid number");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Please enter a valid number");
                }

            }
        }

        private bool isInTeam(DiscordUser user, Team team)
        {
            foreach (TeamUser tu in team.TeamMembers)
            {
                if (tu.User.Id == user.Id)
                {
                    return true;
                }
            }

            return false;
        }
        
        private List<Team> GetMemberTeams(DiscordUser user)
        {
            List<Team> teams = new List<Team>();
            foreach (Team team in d.Teams)
            {
                foreach (TeamUser tu in team.TeamMembers)
                {
                    if (tu.User.Id == user.Id)
                    {
                        teams.Add(team);
                    }
                }
            }

            return teams;
        }
        private async Task<DiscordUser> ChooseUser(CommandContext ctx, List<DiscordUser> users)
        {
            string s = "Choose a user";
            int i = 1;
            foreach (DiscordUser user in users)
            {
                s += "\n" + i + ": " + user.Username + "#" + user.Discriminator;
                i++;
            }
            await ctx.RespondAsync(s);

            while(true)
            {
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if(message.Result.Content.ToLower().Contains("cancel"))
                {
                    return null;
                }

                if (Int32.TryParse(message.Result.Content, out i))
                {
                    if (i > 0 && i <= users.Count)
                    {
                        return users[i - 1];
                    }
                    else
                    {
                        await ctx.RespondAsync("Please enter a valid number");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Please enter a valid number");
                }
            }
        }

        private bool ValidResponse(string response)
        {
            if(response.StartsWith("!"))
            {
                return false;
            }
            return true;
        }
    }
}