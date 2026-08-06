using BaseBallBoardGameScoreEntity;

using Xunit;

namespace BaseBallBoardGameScoreEntity_Test
{
    public class GameEntity_Test
    {
        [Fact]
        public void 初期化すると全状態がリセットされる()
        {
            GameEntity game = new();
            game.NotifyStrike();
            game.NotifyBall();
            game.NotifyHit(1);

            game.Initialize();

            Assert.Equal(0, game.GetStrikeCount());
            Assert.Equal(0, game.GetBallCount());
            Assert.Equal(0, game.GetOutCount());
            Assert.Equal(0, game.GetTotalScore());
            Assert.False(game.IsRunnerExists(1));
            Assert.False(game.IsRunnerExists(2));
            Assert.False(game.IsRunnerExists(3));
        }

        [Fact]
        public void ストライクを通知するとストライク数が増える()
        {
            GameEntity game = new();

            game.NotifyStrike();
            Assert.Equal(1, game.GetStrikeCount());

            game.NotifyStrike();
            Assert.Equal(2, game.GetStrikeCount());
        }

        [Fact]
        public void ストライク三回でアウトになりカウントがリセットされる()
        {
            GameEntity game = new();

            game.NotifyStrike(); // 1
            game.NotifyStrike(); // 2
            game.NotifyStrike(); // 3 → OUT

            Assert.Equal(0, game.GetStrikeCount());
            Assert.Equal(0, game.GetBallCount());
            Assert.Equal(1, game.GetOutCount());
        }

        [Fact]
        public void ボールを通知するとボール数が増える()
        {
            GameEntity game = new();

            game.NotifyBall();
            Assert.Equal(1, game.GetBallCount());

            game.NotifyBall();
            Assert.Equal(2, game.GetBallCount());
        }

        [Fact]
        public void 四球になると走者が一塁に入りカウントがリセットされる()
        {
            GameEntity game = new();

            game.NotifyBall(); // 1
            game.NotifyBall(); // 2
            game.NotifyBall(); // 3
            game.NotifyBall(); // 4 → 四球

            Assert.True(game.IsRunnerExists(1));
            Assert.Equal(0, game.GetBallCount());
            Assert.Equal(0, game.GetStrikeCount());
        }

        [Fact]
        public void ヒットを通知すると走者が進塁しカウントがリセットされる()
        {
            GameEntity game = new();

            game.NotifyHit(1); // 単打

            Assert.True(game.IsRunnerExists(1));
            Assert.Equal(0, game.GetStrikeCount());
            Assert.Equal(0, game.GetBallCount());
        }

        [Fact]
        public void ホームランを通知すると走者が全て帰還し得点が加算される()
        {
            GameEntity game = new();

            // ランナーを作る
            game.NotifyHit(1); // 1塁
            game.NotifyHit(1); // 1塁 → 2塁

            game.NotifyHomeRun();

            Assert.False(game.IsRunnerExists(1));
            Assert.False(game.IsRunnerExists(2));
            Assert.False(game.IsRunnerExists(3));

            Assert.Equal(3, game.GetTotalScore()); // 打者 + 2走者
        }

        [Fact]
        public void アウトを通知するとアウト数が増える()
        {
            GameEntity game = new();

            game.NotifyOut();
            Assert.Equal(1, game.GetOutCount());

            game.NotifyOut();
            Assert.Equal(2, game.GetOutCount());
        }

        [Fact]
        public void アウト三回でイニングが進みアウトカウントがリセットされる()
        {
            GameEntity game = new();

            game.NotifyOut(); // 1
            game.NotifyOut(); // 2
            game.NotifyOut(); // 3 → イニング終了

            Assert.Equal(0, game.GetOutCount());
            Assert.Equal(2, game.GetInningNumber()); // 1 → 2
        }

        [Fact]
        public void クローンは全ての状態を正しく複製する()
        {
            GameEntity game = new();

            game.NotifyHit(1);
            game.NotifyStrike();
            game.NotifyBall();
            game.NotifyOut();

            GameEntity cloned = game.Clone();

            Assert.Equal(game.GetStrikeCount(), cloned.GetStrikeCount());
            Assert.Equal(game.GetBallCount(), cloned.GetBallCount());
            Assert.Equal(game.GetOutCount(), cloned.GetOutCount());
            Assert.Equal(game.GetTotalScore(), cloned.GetTotalScore());
            Assert.Equal(game.IsRunnerExists(1), cloned.IsRunnerExists(1));
        }
    }
}
