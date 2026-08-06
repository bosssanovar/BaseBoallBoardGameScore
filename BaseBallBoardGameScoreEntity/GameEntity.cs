namespace BaseBallBoardGameScoreEntity
{
    public class GameEntity
    {
        #region Fields ====================================================================================================

        private ScoreEntity _score = new();

        private CountEntity _count = new();

        private RunnerEntity _runner = new();

        #endregion

        #region Properties =================================================================================================
        #endregion

        #region Constructors ================================================================================================
        #endregion

        #region Public Methods ==============================================================================================

        public void Initialize()
        {
            _score = new();
            _count = new();
            _runner = new();
        }

        public GameEntity Clone()
        {
            GameEntity cloned = new()
            {
                _score = _score.Clone(),
                _count = _count.Clone(),
                _runner = _runner.Clone()
            };
            return cloned;
        }

        public int GetOffensePlayerNumber()
        {
            return ScoreEntity.GetOffensePlayerNumber();
        }

        public int GetInningNumber()
        {
            return _score.GetInningNumber();
        }

        public int GetInningScore(int inningNumber)
        {
            return _score.GetInningScore(inningNumber);
        }

        public int GetTotalScore()
        {
            return _score.GetTotalScore();
        }

        public int GetStrikeCount()
        {
            return _count.GetStrikeCount();
        }

        public int GetBallCount()
        {
            return _count.GetBallCount();
        }

        public int GetOutCount()
        {
            return _count.GetOutCount();
        }

        public bool IsRunnerExists(int baseNumber)
        {
            return _runner.IsExists(baseNumber);
        }

        public void NotifyStrike()
        {
            _count.NotifyStrike(out bool isInningEnd);
            if(isInningEnd)
            {
                _score.Next();
            }
        }

        public void NotifyBall()
        {
            _count.NotifyBall(out bool isFourBall);
            if(isFourBall)
            {
                _runner.NotifyFourBall(out int HomeInCount);
                _score.AddScore(HomeInCount);
            }
        }

        public void NotifyHit(int baseNumber)
        {
            _runner.NotifyHit(baseNumber, out int HomeInCount);
            _score.AddScore(HomeInCount);
            _count.ResetStrikeAndBall();
        }

        public void NotifyOut()
        {
            _count.NotifyOut(out bool isInningEnd);
            if (isInningEnd)
            {
                _score.Next();
            }
        }

        public void NotifyHomeRun()
        {
            _runner.NotifyHomeRun(out int HomeInCount);
            _score.AddScore(HomeInCount);
            _count.ResetStrikeAndBall();
        }

        public void NotifyFoul()
        {
            _count.NotifyFoul();
        }

        #endregion

        #region Private Methods =============================================================================================
        #endregion

        #region Helpers =====================================================================================================
        #endregion
    }
}
