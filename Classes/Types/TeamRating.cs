namespace big
{
    public class TeamRating
    {
        public const float kfactor = 32; //Subject for change
        public TeamRating()
        {

        }
        //rt1 is Team 1's rating
        //rt2 is Team 2's rating
        //nE is the total number of games that has decided the rating for each team
        public void Elo(float rt1, float rt2, float kfactor, bool t1win)
        {
            eloHelper(rt1, rt2, kfactor, t1win);
        }

        //K is elo constant
        private void eloHelper(float rT1, float rT2, float K, bool T1win)
        {
            float pT1 = probability(rT1, rT2);

            float pT2 = probability(rT2, rT1);
            //Team1 wins
            if(T1win == true)
            {
                rT1 = rT1 + K * (1 - pT1);
                rT2 = rT2 + K * (0 - pT2);
            }
            //Team2 wins
            else
            {
                rT1 = rT1 + K * (0 - rT1);
                rT2 = rT2 + K * (1 - rT2);
            }
        }
        //Will be put into use when UserMMR is implemented
       /* private float KFactor(float nE, float m)
        {
            return 800.0f / (nE + m);
        }*/ 
        //Probability of either winning
        private float probability(float rating1, float rating2)
        {
            return 1.0f * 1.0f / (1 + 1.0f * 
            (float) (Math.Pow(10, 1.0f *
            (rating1 - rating2) / 400)));
        }
    }
}