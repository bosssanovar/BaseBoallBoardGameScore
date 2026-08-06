namespace BaseBallBoardGameScoreEntity
{
    internal class RunnerEntity
    {
        #region Fields ====================================================================================================

        // 1,2,3 塁に走者がいるかどうか
        private bool[] _bases = new bool[3];

        #endregion

        #region Properties =================================================================================================
        #endregion

        #region Constructors ================================================================================================
        #endregion

        #region internal Methods ==============================================================================================
        internal bool IsExists(int baseNumber)
        {
            return _bases[baseNumber - 1];
        }

        internal void Clear()
        {
            _bases[0] = _bases[1] = _bases[2] = false;
        }

        internal void NotifyFourBall(out int homeInCount)
        {
            homeInCount = 0;

            // 1塁埋まっている → 2塁 → 3塁 → ホームイン
            if (_bases[0])
            {
                if (_bases[1])
                {
                    if (_bases[2])
                    {
                        homeInCount++;
                    }
                    _bases[2] = true;
                }
                _bases[1] = true;
            }

            _bases[0] = true;
        }

        internal void NotifyHit(int baseNumber, out int homeInCount)
        {
            homeInCount = 0;

            // 走者を後ろから順に動かす
            for (int i = 2; i >= 0; i--)
            {
                if (_bases[i])
                {
                    int newBase = i + baseNumber;
                    if (newBase >= 3)
                    {
                        homeInCount++;
                    }
                    else
                    {
                        _bases[newBase] = true;
                    }
                    _bases[i] = false;
                }
            }

            // 打者が進塁
            if (baseNumber >= 4)
            {
                homeInCount++;
            }
            else
            {
                _bases[baseNumber - 1] = true;
            }
        }

        internal void NotifyHomeRun(out int homeInCount)
        {
            homeInCount = 1; // 打者

            for (int i = 0; i < 3; i++)
            {
                if (_bases[i])
                {
                    homeInCount++;
                    _bases[i] = false;
                }
            }
        }

        internal RunnerEntity Clone()
        {
            RunnerEntity clone = new()
            {
                _bases = (bool[])_bases.Clone()
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
