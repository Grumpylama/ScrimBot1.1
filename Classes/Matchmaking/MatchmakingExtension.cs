namespace big
{
    public static class MatchmakingExtensions
    {
        public async static Task<Tuple<ScrimResponse, ScrimResponse>> PromtCaptainsForScrimAync(this Tuple<MatchMakingTeam, MatchMakingTeam> teams, double timeout)
        {
            MatchMakingTeam team1 = teams.Item1;
            MatchMakingTeam team2 = teams.Item2;

            List<Task<ScrimResponse>> tasks = new List<Task<ScrimResponse>>();
            tasks.Add(team1.PromtCaptainForScrimAsync(team2, timeout));
            tasks.Add(team2.PromtCaptainForScrimAsync(team1, timeout));

            var responses = await Task.WhenAll(tasks);
            return new Tuple<ScrimResponse, ScrimResponse>(responses[0], responses[1]);
           
        }
    }
}