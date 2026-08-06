namespace BaseBallBoardGameScoreEntity
{
    public class ScoreEntity
    {
        #region Fields ====================================================================================================

        private int _inning = 1;
        private int[] _scores = new int[30]; // 最大30イニング分確保（十分）

        #endregion

        #region Properties =================================================================================================
        #endregion

        #region Constructors ================================================================================================
        #endregion

        #region Public Methods ==============================================================================================

        public static int GetOffensePlayerNumber() => 1; // 今回不要なので固定値でOK

        public int GetInningNumber() => _inning;

        public int GetInningScore(int inningNumber)
        {
            return _scores[inningNumber - 1];
        }

        public int GetTotalScore()
        {
            int total = 0;
            foreach (var s in _scores) total += s;
            return total;
        }

        public void AddScore(int count)
        {
            _scores[_inning - 1] += count;
        }

        public void Next()
        {
            _inning++;
        }

        public ScoreEntity Clone()
        {
            ScoreEntity clone = new()
            {
                _inning = _inning,
                _scores = (int[])_scores.Clone()
            };
            return clone;
        }

        #endregion

        #region Private Methods =============================================================================================
        #endregion

        #region Helpers =====================================================================================================
        #endregion
    }
}
