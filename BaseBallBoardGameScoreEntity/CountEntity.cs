namespace BaseBallBoardGameScoreEntity
{
    internal class CountEntity
    {
        #region Fields ====================================================================================================

        private int _strike = 0;
        private int _ball = 0;
        private int _out = 0;

        #endregion

        #region Properties =================================================================================================
        #endregion

        #region Constructors ================================================================================================
        #endregion

        #region internal Methods ==============================================================================================
        internal int GetStrikeCount() => _strike;
        internal int GetBallCount() => _ball;
        internal int GetOutCount() => _out;

        internal void ResetStrikeAndBall()
        {
            _strike = 0;
            _ball = 0;
        }

        internal void NotifyStrike(out bool isInningEnd)
        {
            _strike++;

            if (_strike >= 3)
            {
                NotifyOut(out isInningEnd);
                ResetStrikeAndBall();
                return;
            }

            isInningEnd = false;
        }

        internal void NotifyBall(out bool isFourBall)
        {
            _ball++;

            if (_ball >= 4)
            {
                isFourBall = true;
                ResetStrikeAndBall();
                return;
            }

            isFourBall = false;
        }

        internal void NotifyOut(out bool isInningEnd)
        {
            _out++;
            ResetStrikeAndBall();

            if (_out >= 3)
            {
                _out = 0;
                isInningEnd = true;
                return;
            }

            isInningEnd = false;
        }

        internal void NotifyFoul()
        {
            if (_strike < 2)
            {
                _strike++;
            }
        }

        internal CountEntity Clone()
        {
            return new CountEntity
            {
                _strike = this._strike,
                _ball = this._ball,
                _out = this._out
            };
        }

        #endregion

        #region Private Methods =============================================================================================
        #endregion

        #region Helpers =====================================================================================================
        #endregion
    }
}
