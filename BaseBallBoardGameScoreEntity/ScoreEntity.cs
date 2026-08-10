namespace BaseBallBoardGameScoreEntity
{
    public class ScoreEntity
    {
        #region Fields ====================================================================================================

        private int _inning = 1;
        private UraOmoteScore[] _scores = new UraOmoteScore[3];
        private bool _isOmote = true;

        #endregion

        #region Properties =================================================================================================
        #endregion

        #region Constructors ================================================================================================
        
        public ScoreEntity()
        {
            _scores[0] = new UraOmoteScore()
            {
                OmoteScore = 0
            };
        }

        #endregion

        #region Public Methods ==============================================================================================

        public int GetOffensePlayerNumber() => _isOmote ? 1 : 2;

        public int GetInningNumber() => _inning;

        public int? GetInningScore(int inningNumber, bool isOmote)
        {
            if(_scores[inningNumber - 1] is null)
            {
                return null;
            }

            return isOmote ? _scores[inningNumber - 1].OmoteScore : _scores[inningNumber - 1].UraScore;
        }

        public int GetTotalScore(bool isOmote)
        {
            int total = 0;
            foreach (var s in _scores)
            {
                if (s is not null)
                {
                    total += isOmote ? s.OmoteScore ?? 0 : s.UraScore ?? 0;
                }
            }

            return total;
        }

        public void AddScore(int count)
        {
            if (_isOmote)
            {
                _scores[_inning - 1].OmoteScore += count;
            }
            else
            {
                _scores[_inning - 1].UraScore += count;
            }
        }

        public void Next()
        {
            _isOmote = !_isOmote;

            if (_isOmote)
            {
                _inning++;
            }

            _scores[_inning - 1] ??= new UraOmoteScore();

            if (_isOmote)
            {
                _scores[_inning - 1].OmoteScore ??= 0;
            }
            else
            {
                _scores[_inning - 1].UraScore ??= 0;
            }
        }

        public ScoreEntity Clone()
        {
            ScoreEntity clone = new()
            {
                _inning = _inning,
                _isOmote = _isOmote
            };

            UraOmoteScore[] clonedScore = new UraOmoteScore[_scores.Length];
            for (int i = 0; i < _scores.Length; i++)
            {
                if(_scores[i] is not null)
                {
                    clonedScore[i] = _scores[i].Clone();
                }
            }
            clone._scores = clonedScore;

            return clone;
        }

        #endregion

        #region Private Methods =============================================================================================

        #endregion

        #region Helpers =====================================================================================================

        class UraOmoteScore
        {
            public int? OmoteScore { get; set; }
            public int? UraScore { get; set; }

            public UraOmoteScore Clone()
            {
                return new UraOmoteScore()
                {
                    OmoteScore = this.OmoteScore,
                    UraScore = this.UraScore
                };
            }
        }

        #endregion
    }
}
