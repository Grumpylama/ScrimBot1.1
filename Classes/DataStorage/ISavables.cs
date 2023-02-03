

namespace big
{
    public class SaveableUser : ISavable
    {

        public static string FilePath = "SaveableUser.cs.SaveableUser";
        public ulong ID { get; set; }

        public ulong DMChannelID { get; set; }
        
        public SaveableUser(ulong ID, ulong DMChannelID)
        {
            this.ID = ID;
            this.DMChannelID = DMChannelID;
            StandardLogging.LogInfo(FilePath, "User saved: " + ID + " " + DMChannelID);
        }
        

        public SaveableUser()
        {
            
        }
    }


    #pragma warning disable CS8618
    

    public class SaveableTeam : ISavable
    {

        public static string FilePath = "SaveableUser.cs.SavableTeam";
        public int ID { get; set; }
        public ulong CaptainID { get; set; }
        
        public float MMR { get; set; }
        public int gameID { get; set; }
        public string TeamName { get; set;}

        public DateTime CreationTime { get; set; }

        public SaveableTeam()
        {

            
        }

        public Team ToTeam()
        {
            Team team = new Team(GameHandler.GetGameFromID(this.gameID), this.TeamName, UserHandler.GetUserFromID(this.CaptainID), false);
            team.MMR = this.MMR;
            team.teamID = this.ID;
            team.CreationTime = this.CreationTime;
            StandardLogging.LogInfo(FilePath, "Team loaded: " + team.ToString());
            return team;
        }


    }

    

    public class SavableTeamUser : ISavable 
    {

        public static string FilePath = "SaveableUser.cs.SavableTeamUser";
        
        public ulong UserID { get; set; }
        public int TeamID { get; set; }
       
        public string Position { get; set; }

        public int TrustLevel { get; set; }

        public DateTime JoinDate { get; set; }

        public SavableTeamUser(ulong UserID, int TeamID, string Position, int TrustLevel = 0)
        {
            this.UserID = UserID;
            this.TeamID = TeamID;
            
            this.Position = Position;
            this.TrustLevel = TrustLevel;
            StandardLogging.LogInfo(FilePath, "TeamUser saved: " + UserID + " " + TeamID + " " + Position);

        }

        public SavableTeamUser()
        {
            
        }

        public TeamUser ToTeamUser()
        {
            TeamUser teamUser = new TeamUser(UserHandler.GetUserFromID(this.UserID), this.TeamID, this.Position, GetTrustLevel(this.TrustLevel), this.JoinDate);
            teamUser.teamID = this.TeamID;
            StandardLogging.LogInfo(FilePath, "TeamUser loaded: " + teamUser.ToString());
            return teamUser;
        }

        private TrustLevel GetTrustLevel(int trustLevel)
        {
            switch (trustLevel)
            {
                case 0:
                    return big.TrustLevel.Member;
                case 1:
                    return big.TrustLevel.CanMatchMake;
                case 2:
                    return big.TrustLevel.CanAddMembers;
                case 3:
                    return big.TrustLevel.CanRemoveMembers;
                case 4:
                    return big.TrustLevel.CanEditTeam;
                case 5:
                    return big.TrustLevel.CanEditTrustLevels;
                case 6:
                    return big.TrustLevel.TeamCaptain;
                default:
                    return big.TrustLevel.Member;

            
            }
        }

        
    }

    public class SavableScrim : ISavable
    {

        private static readonly string FilePath = "SaveableScrim.cs";

        public ulong ID { get; set; }
        public int Team1ID { get; set; }
        public int Team2ID { get; set; }
        public int GameID { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public DateTime Date { get; set; }
        public bool IsFinished { get; set; }

        public Scrim ToScrim()
        {
            Scrim scrim = new Scrim(TeamHandler.GetTeamFromID(this.Team1ID), TeamHandler.GetTeamFromID(this.Team2ID), GameHandler.GetGameFromID(this.GameID), this.Date);
            scrim.Team1Score = this.Team1Score;
            scrim.Team2Score = this.Team2Score;
            scrim.Finished = this.IsFinished;
            scrim.ID = this.ID;
            StandardLogging.LogInfo(FilePath, "Scrim loaded: " + scrim.ToString());
            return scrim;
        }

        public SavableScrim(Scrim scrim)
        {
            this.ID = scrim.ID;
            this.Team1ID = scrim.Team1.teamID;
            this.Team2ID = scrim.Team2.teamID;
            this.GameID = scrim.Game.GameID;
            this.Team1Score = scrim.Team1Score;
            this.Team2Score = scrim.Team2Score;
            this.Date = scrim.Date;
            this.IsFinished = scrim.Finished;
            StandardLogging.LogInfo(FilePath, "Scrim saved: " + scrim.ToString());
        }

        public SavableScrim()
        {
            
        }


    }

    


    
}