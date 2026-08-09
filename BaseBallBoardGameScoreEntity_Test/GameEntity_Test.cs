using BaseBallBoardGameScoreEntity;

using Xunit;

namespace BaseBallBoardGameScoreEntity_Test
{
    public class GameEntity_Test
    {
        [Fact]
        public void 初期化すると全状態がリセットされる()
        {
            var game = new GameEntity();

            game.NotifyHit(1);
            game.NotifyStrike();
            game.NotifyBall();
            game.NotifyOut();

            game.Initialize();

            Assert.Equal(0, game.GetStrikeCount());
            Assert.Equal(0, game.GetBallCount());
            Assert.Equal(0, game.GetOutCount());
            Assert.Equal(0, game.GetTotalScore(true));
            Assert.False(game.IsRunnerExists(1));
            Assert.False(game.IsRunnerExists(2));
            Assert.False(game.IsRunnerExists(3));
        }

        [Fact]
        public void 表裏が切り替わり裏から表に戻るとイニングが進む()
        {
            var game = new GameEntity();

            // 初期状態は「1回表」
            Assert.Equal(1, game.GetInningNumber());
            Assert.Equal(1, game.GetOffensePlayerNumber()); // 表＝1

            // アウト3 → 裏へ
            game.NotifyOut();
            game.NotifyOut();
            game.NotifyOut();

            Assert.Equal(1, game.GetInningNumber());
            Assert.Equal(2, game.GetOffensePlayerNumber()); // 裏＝2

            // 裏でアウト3 → 次イニングへ
            game.NotifyOut();
            game.NotifyOut();
            game.NotifyOut();

            Assert.Equal(2, game.GetInningNumber());
            Assert.Equal(1, game.GetOffensePlayerNumber()); // 表に戻る
        }

        [Fact]
        public void ストライク三回でアウトになり表裏が切り替わる()
        {
            var game = new GameEntity();
            Assert.Equal(1, game.GetInningNumber());
            Assert.Equal(0, game.GetOutCount());

            game.NotifyStrike();
            game.NotifyStrike();
            game.NotifyStrike(); // 1アウト
            Assert.Equal(1, game.GetOutCount());

            game.NotifyStrike();
            game.NotifyStrike();
            game.NotifyStrike(); // 2アウト
            Assert.Equal(2, game.GetOutCount());

            game.NotifyOut(); // 3アウトで表裏が切り替わる
            Assert.Equal(1, game.GetInningNumber());
            Assert.Equal(0, game.GetOutCount());
            Assert.Equal(2, game.GetOffensePlayerNumber()); // 裏
        }

        [Fact]
        public void ファウルはストライク2まで増える()
        {
            var game = new GameEntity();

            game.NotifyFoul();
            Assert.Equal(1, game.GetStrikeCount());

            game.NotifyFoul();
            Assert.Equal(2, game.GetStrikeCount());

            game.NotifyFoul();
            Assert.Equal(2, game.GetStrikeCount()); // 2以上は増えない
        }

        [Fact]
        public void ボール四球で走者が進みカウントがリセットされる()
        {
            var game = new GameEntity();

            game.NotifyBall();
            game.NotifyBall();
            game.NotifyBall();
            game.NotifyBall(); // 四球

            Assert.True(game.IsRunnerExists(1));
            Assert.Equal(0, game.GetStrikeCount());
            Assert.Equal(0, game.GetBallCount());
        }

        [Fact]
        public void 四球押し出しで得点が加算される()
        {
            var game = new GameEntity();

            // 1塁 → 2塁 → 3塁 → 押し出し
            for (int i = 0; i < 4; i++) game.NotifyBall(); // 1塁
            for (int i = 0; i < 4; i++) game.NotifyBall(); // 2塁
            for (int i = 0; i < 4; i++) game.NotifyBall(); // 3塁
            for (int i = 0; i < 4; i++) game.NotifyBall(); // 押し出し

            Assert.Equal(1, game.GetTotalScore(true));
        }

        [Fact]
        public void ヒットで走者が進み得点が加算される()
        {
            var game = new GameEntity();

            game.NotifyHit(1); // 打者 → 1塁
            Assert.True(game.IsRunnerExists(1));

            game.NotifyHit(3); // 1塁走者 → ホームイン
            Assert.Equal(1, game.GetTotalScore(true));
            Assert.True(game.IsRunnerExists(3)); // 打者が3塁
        }

        [Fact]
        public void ホームランで全走者が帰還し得点が加算される()
        {
            var game = new GameEntity();

            game.NotifyHit(1); // 1塁
            game.NotifyHit(1); // 2塁

            game.NotifyHomeRun();

            Assert.False(game.IsRunnerExists(1));
            Assert.False(game.IsRunnerExists(2));
            Assert.False(game.IsRunnerExists(3));

            Assert.Equal(3, game.GetTotalScore(true)); // 打者 + 2走者
        }

        [Fact]
        public void アウト三回で表裏が切り替わり裏から表に戻るとイニングが進む()
        {
            var game = new GameEntity();

            // 表 → 裏
            game.NotifyOut();
            game.NotifyOut();
            game.NotifyOut();
            Assert.Equal(2, game.GetOffensePlayerNumber());

            // 裏 → 次イニング表
            game.NotifyOut();
            game.NotifyOut();
            game.NotifyOut();
            Assert.Equal(2, game.GetInningNumber());
            Assert.Equal(1, game.GetOffensePlayerNumber());
        }

        [Fact]
        public void クローンは全ての状態を正しく複製する()
        {
            var game = new GameEntity();

            game.NotifyHit(1);
            game.NotifyStrike();
            game.NotifyBall();
            game.NotifyOut(); // 表→裏へ切り替わる

            var clone = game.Clone();

            Assert.Equal(game.GetStrikeCount(), clone.GetStrikeCount());
            Assert.Equal(game.GetBallCount(), clone.GetBallCount());
            Assert.Equal(game.GetOutCount(), clone.GetOutCount());
            Assert.Equal(game.GetTotalScore(true), clone.GetTotalScore(true));
            Assert.Equal(game.GetInningNumber(), clone.GetInningNumber());
            Assert.Equal(game.GetOffensePlayerNumber(), clone.GetOffensePlayerNumber());
            Assert.Equal(game.IsRunnerExists(1), clone.IsRunnerExists(1));
        }

        [Fact]
        public void チェンジ時に走者がリセットされる()
        {
            var game = new GameEntity();
            game.NotifyHit(1); // 1塁に走者
            Assert.True(game.IsRunnerExists(1));
            // アウト3で裏へ
            game.NotifyOut();
            game.NotifyOut();
            game.NotifyOut();
            Assert.False(game.IsRunnerExists(1)); // 走者はリセットされる
        }
    }
}
