namespace big
{

    public class UserMMR : Interfaces.IMMR
    {

        public int GameID { get; private set; }

        public float MMR { get; private set; }

        public void printinMMR()
        {
            Console.WriteLine("User MMR");
        }
    }

    public class TeamMMR : Interfaces.IMMR
    {
        public void printinMMR()
        {
            Console.WriteLine("Team MMR");
        }
    }
}