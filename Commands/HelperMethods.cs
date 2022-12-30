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

        private async Task<Team> ChooseTeamAsync(CommandContext ctx, List<Team> Teams)
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
        private async Task<DiscordUser> ChooseUserAsync(CommandContext ctx, List<DiscordUser> users)
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

        public DateTime GetNearestHour(DateTime dt)
        {
            if(dt.Minute >= 30)           
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour + 1, 0, 0);       
            else        
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }

        

        private async Task<Nullable<EDate>> PromtDateAsync(CommandContext ctx)
        {
            
            while(true)
            {
                await ctx.RespondAsync("What day will you be playing? \n 1:Tonight \n 2:Tomorrow \n 3:Other");
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                if (message.Result.Content.ToLower().Contains("cancel"))
                {
                    return null;
                }
                if(message.Result.Content != "1" && message.Result.Content != "2" && message.Result.Content != "3")
                {
                    await ctx.RespondAsync("Please enter a valid number");
                    continue;
                }
                break;
            }
            while(true)
            {
                await ctx.RespondAsync("What time will you be playing? \n Please Enter in the format HH:MM \n Please matchmake at either xx:00 or xx:30 \n Enter \"ASAP\" if you are looking for a game ASAP");
                var message = await ctx.Client.GetInteractivity().WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id);
                switch (message.Result.Content.ToLower())
                {
                    case "cancel":
                        return null;
                    case "asap":
                        return new EDate(GetNearestHour(DateTime.Now), true);
                    default:
                        if (DateTime.TryParse(message.Result.Content, out DateTime date))
                        {                   
                            return new EDate(GetNearestHour(date), false);
                        }
                        else
                        {
                            await ctx.RespondAsync("Please enter a valid time");
                            continue;
                        }
                        
                }
            }
            
        }

        
    }
}