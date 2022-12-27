namespace big
{

    public class UserMMR : IMMR
    {

        public int GameID { get; private set; }

        public float MMR { get; private set; }

        public void printinMMR()
        {
            Console.WriteLine("User MMR");
        }
    }

    public class TeamMMR : IMMR
    {
        public void printinMMR()
        {
            Console.WriteLine("Team MMR");
        }
    }
}