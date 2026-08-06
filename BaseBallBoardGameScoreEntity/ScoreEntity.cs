namespace BaseBallBoardGameScoreEntity
{
    public class ScoreEntity
    {
        #region Fields ====================================================================================================

        private int _inning = 1;
        private int[] _scores = new int[3];
        private bool _isOmote = true;

        #endregion

        #region Properties =================================================================================================
        #endregion

        #region Constructors ================================================================================================
        #endregion

        #region Public Methods ==============================================================================================

        public int GetOffensePlayerNumber() => _isOmote ? 1 : 2;

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
            _isOmote = !_isOmote;

            if (_isOmote)
            {
                _inning++;
            }
        }

        public ScoreEntity Clone()
        {
            ScoreEntity clone = new()
            {
                _inning = _inning,
                _scores = (int[])_scores.Clone(),
                _isOmote = _isOmote
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
