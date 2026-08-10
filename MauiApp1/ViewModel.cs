using Microsoft.Maui.Controls.Shapes;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Linq;
using System.Text;

using static MauiApp1.MainPage;

namespace MauiApp1
{
    public partial class MainPage
    {
        #region Fields ====================================================================================================

        private Model _model = new();

        #endregion

        #region Properties =================================================================================================

        public ReadOnlyReactivePropertySlim<SolidColorBrush?> OmotePlayerBackground { get; }
        public ReadOnlyReactivePropertySlim<SolidColorBrush?> UraPlayerBackground { get; }
        public ReadOnlyReactivePropertySlim<int?> Omote1Score { get; }
        public ReadOnlyReactivePropertySlim<int?> Ura1Score { get; }
        public ReadOnlyReactivePropertySlim<int?> Omote2Score { get; }
        public ReadOnlyReactivePropertySlim<int?> Ura2Score { get; }
        public ReadOnlyReactivePropertySlim<int?> Omote3Score { get; }
        public ReadOnlyReactivePropertySlim<int?> Ura3Score { get; }
        public ReadOnlyReactivePropertySlim<int> OmoteTotalScore { get; }
        public ReadOnlyReactivePropertySlim<int> UraTotalScore { get; }

        public ReadOnlyReactivePropertySlim<SolidColorBrush?> Base1Brush { get; }
        public ReadOnlyReactivePropertySlim<SolidColorBrush?> Base2Brush { get; }
        public ReadOnlyReactivePropertySlim<SolidColorBrush?> Base3Brush { get; }

        public ReadOnlyReactivePropertySlim<bool> Ball1Visibility { get; }
        public ReadOnlyReactivePropertySlim<bool> Ball2Visibility { get; }
        public ReadOnlyReactivePropertySlim<bool> Ball3Visibility { get; }
        public ReadOnlyReactivePropertySlim<bool> Strike1Visibility { get; }
        public ReadOnlyReactivePropertySlim<bool> Strike2Visibility { get; }
        public ReadOnlyReactivePropertySlim<bool> Out1Visibility { get; }
        public ReadOnlyReactivePropertySlim<bool> Out2Visibility { get; }

        public ReactiveCommand BallCommand { get; } = new();
        public ReactiveCommand StrikeCommand { get; } = new();

        public ReactiveCommand FoulCommand { get; } = new();
        public ReactiveCommand OutCommand { get; } = new();
        public ReactiveCommand SingleHitCommand { get; } = new();
        public ReactiveCommand TwoBaseHitCommand { get; } = new();
        public ReactiveCommand ThreeBaseHitCommand { get; } = new();
        public ReactiveCommand HomeRunCommand { get; } = new();

        public ReactiveCommand InitializeCommand { get; } = new();
        public ReactiveCommand UndoCommand { get; } = new();

        #endregion

        #region Constructors ================================================================================================

        public MainPage()
        {
            MainPageBase();

            OmotePlayerBackground =
                _model.GameEntity
                .Select(
                    x =>
                    x.GetOffensePlayerNumber() == 1
                    ? new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#555500"))
                    : new SolidColorBrush(Colors.Black))
                .ToReadOnlyReactivePropertySlim();
            UraPlayerBackground =
                _model.GameEntity
                .Select(
                    x =>
                    x.GetOffensePlayerNumber() == 2
                    ? new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#555500"))
                    : new SolidColorBrush(Colors.Black))
                .ToReadOnlyReactivePropertySlim();
            Omote1Score =
                _model.GameEntity
                .Select(x => x.GetInningScore(1, true))
                .ToReadOnlyReactivePropertySlim();
            Ura1Score =
                _model.GameEntity
                .Select(x => x.GetInningScore(1, false))
                .ToReadOnlyReactivePropertySlim();
            Omote2Score =
                _model.GameEntity
                .Select(x => x.GetInningScore(2, true))
                .ToReadOnlyReactivePropertySlim();
            Ura2Score =
                _model.GameEntity
                .Select(x => x.GetInningScore(2, false))
                .ToReadOnlyReactivePropertySlim();
            Omote3Score =
                _model.GameEntity
                .Select(x => x.GetInningScore(3, true))
                .ToReadOnlyReactivePropertySlim();
            Ura3Score =
                _model.GameEntity
                .Select(x => x.GetInningScore(3, false))
                .ToReadOnlyReactivePropertySlim();
            OmoteTotalScore =
                _model.GameEntity
                .Select(x => x.GetTotalScore(true))
                .ToReadOnlyReactivePropertySlim();
            UraTotalScore =
                _model.GameEntity
                .Select(x => x.GetTotalScore(false))
                .ToReadOnlyReactivePropertySlim();

            Base1Brush =
                _model.GameEntity
                .Select(x => GetBaseBrush(x.IsRunnerExists(1)))
                .ToReadOnlyReactivePropertySlim();
            Base2Brush =
                _model.GameEntity
                .Select(x => GetBaseBrush(x.IsRunnerExists(2)))
                .ToReadOnlyReactivePropertySlim();
            Base3Brush =
                _model.GameEntity
                .Select(x => GetBaseBrush(x.IsRunnerExists(3)))
                .ToReadOnlyReactivePropertySlim();

            Ball1Visibility =
                _model.GameEntity
                .Select(x => x.GetBallCount() > 0 ? true : false)
                .ToReadOnlyReactivePropertySlim();
            Ball2Visibility =
                _model.GameEntity
                .Select(x => x.GetBallCount() > 1 ? true : false)
                .ToReadOnlyReactivePropertySlim();
            Ball3Visibility =
                _model.GameEntity
                .Select(x => x.GetBallCount() > 2 ? true : false)
                .ToReadOnlyReactivePropertySlim();
            Strike1Visibility =
                _model.GameEntity
                .Select(x => x.GetStrikeCount() > 0 ? true : false)
                .ToReadOnlyReactivePropertySlim();
            Strike2Visibility =
                _model.GameEntity
                .Select(x => x.GetStrikeCount() > 1 ? true : false)
                .ToReadOnlyReactivePropertySlim();
            Out1Visibility =
                _model.GameEntity
                .Select(x => x.GetOutCount() > 0 ? true : false)
                .ToReadOnlyReactivePropertySlim();
            Out2Visibility =
                _model.GameEntity
                .Select(x => x.GetOutCount() > 1 ? true : false)
                .ToReadOnlyReactivePropertySlim();

            BallCommand.Subscribe(() =>
            {
                _model.NotifyBall();
            });
            StrikeCommand.Subscribe(() =>
            {
                _model.NotifyStrike();
            });

            FoulCommand.Subscribe(() =>
            {
                _model.NotifyFoul();
            });
            OutCommand.Subscribe(() =>
            {
                _model.NotifyOut();
            });
            SingleHitCommand.Subscribe(async () =>
            {
                await StartAnimation(HitType.Single);
                _model.NotifyHit(1);
            });
            TwoBaseHitCommand.Subscribe(async () =>
            {
                await StartAnimation(HitType.Double);
                _model.NotifyHit(2);
            });
            ThreeBaseHitCommand.Subscribe(async () =>
            {
                await StartAnimation(HitType.Triple);
                _model.NotifyHit(3);
            });
            HomeRunCommand.Subscribe(async () =>
            {
                await StartAnimation(HitType.HomeRun);
                _model.NotifyHomeRun();
            });

            InitializeCommand.Subscribe(() =>
            {
                _model.Initialize();
            });
            UndoCommand.Subscribe(() =>
            {
                _model.Undo();
            });
        }

        #endregion

        #region Public Methods ==============================================================================================
       
        #endregion

        #region Private Methods =============================================================================================
        private IEnumerable<Func<Task>> BuildAdvanceSequence(int startBase, int basesToAdvance)
        {
            var seq = new List<Func<Task>>();

            int current = startBase;

            for (int i = 0; i < basesToAdvance; i++)
            {
                seq.Add(GetStepAnimation(current));
                current++;
                if (current > 4) break; // ホームを超えない
            }

            return seq;
        }
        private Func<Task> GetStepAnimation(int baseNumber)
        {
            return baseNumber switch
            {
                0 => () => RunFromHomeTo1Base(),
                1 => () => RunFrom1BaseTo2Base(),
                2 => () => RunFrom2BaseTo3Base(),
                3 => () => RunFrom3BaseToHome(),
                _ => () => Task.CompletedTask
            };
        }

        private static SolidColorBrush GetBaseBrush(bool isRunnerExists)
        {
            return
                isRunnerExists
                ? new SolidColorBrush(Colors.White)
                : new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#363636"));
        }

        private Task MoveBallAsync()
        {
            var tcs = new TaskCompletionSource();

            // 初期化
            Ball.TranslationX = 0;
            Ball.TranslationY = 0;

            double startX = Ball.TranslationX;
            double startY = Ball.TranslationY;
            double endX = 100;
            double endY = -300;

            var animation = new Animation();

            // 移動
            animation.Add(0, 1, new Animation(v =>
            {
                Ball.TranslationX = startX + (endX - startX) * v;
                Ball.TranslationY = startY + (endY - startY) * v;
            }));

            // 透明化
            animation.Add(0, 1, new Animation(v =>
            {
                Ball.Opacity = 1 - v;
            }));

            animation.Commit(
                owner: this,
                name: "MoveBall",
                rate: 16,
                length: 1200,
                easing: Easing.CubicOut,
                finished: (v, c) => tcs.SetResult()
            );

            return tcs.Task;
        }

        private async Task RunFromHomeTo1Base()
        {
            await Task.Delay(300);
            await LeaveFromHomeAsync(HomeRunner, 0, 0, 50, -30, "LeaveFromHome");
            await AriveTo1BaseAsync(Base1Runner, -50, 30, 0, 0, "AriveTo1Base");
        }

        private async Task RunFrom1BaseTo2Base()
        {
            await Task.Delay(300);
            await LeaveFromHomeAsync(Base1Runner, 0, 0, -50, -30, "LeaveFrom1Base");
            await AriveTo1BaseAsync(Base2Runner, 50, 30, 0, 0, "AriveTo2Base");
        }

        private async Task RunFrom2BaseTo3Base()
        {
            await Task.Delay(300);
            await LeaveFromHomeAsync(Base2Runner, 0, 0, -50, 30, "LeaveFrom2Base");
            await AriveTo1BaseAsync(Base3Runner, 50, -30, 0, 0, "AriveTo3Base");
        }

        private async Task RunFrom3BaseToHome()
        {
            await Task.Delay(300);
            await LeaveFromHomeAsync(Base3Runner, 0, 0, 50, 30, "LeaveFrom3Base");
            await AriveTo1BaseAsync(HomeRunner, -50, -30, 0, 0, "AriveToHome");
        }

        private async Task LeaveFromHomeAsync(
            Microsoft.Maui.Controls.Shapes.Rectangle target,
            double startX,
            double startY,
            double endX,
            double endY,
            string animationName)
        {
            var tcs = new TaskCompletionSource();

            // 初期化
            target.TranslationX = startX;
            target.TranslationY = startY;

            var animation = new Animation();

            // 移動アニメーション
            animation.Add(0, 1, new Animation(v =>
            {
                target.TranslationX = startX + (endX - startX) * v;
                target.TranslationY = startY + (endY - startY) * v;
            }));

            // 透明化アニメーション
            animation.Add(0, 1, new Animation(v =>
            {
                target.Opacity = 1 - v;
            }));

            animation.Commit(
                owner: this,
                name: animationName,
                rate: 16,
                length: 500,
                easing: Easing.CubicOut,
                finished: (v, c) => tcs.SetResult()
            );

            await tcs.Task;
        }

        private async Task AriveTo1BaseAsync(
            Microsoft.Maui.Controls.Shapes.Rectangle target,
            double startX,
            double startY,
            double endX,
            double endY,
            string animationName)
        {
            var tcs = new TaskCompletionSource();

            // 初期化
            target.TranslationX = startX;
            target.TranslationY = startY;
        
            var animation = new Animation();

            // 移動アニメーション
            animation.Add(0, 1, new Animation(v =>
            {
                target.TranslationX = startX + (endX - startX) * v;
                target.TranslationY = startY + (endY - startY) * v;
            }));

            // 透明化アニメーション
            animation.Add(0, 1, new Animation(v =>
            {
                target.Opacity = v;
            }));

            animation.Commit(
                owner: this,
                name: animationName,
                rate: 16,
                length: 500,
                easing: Easing.CubicOut,
                finished: (v, c) => tcs.SetResult()
            );

            await tcs.Task;

            // 透明化アニメーション
            tcs = new TaskCompletionSource();
            animation = new Animation();
            animation.Add(0, 1, new Animation(v =>
            {
                target.Opacity = 1 - v;
            }));

            animation.Commit(
                owner: this,
                name: animationName,
                rate: 16,
                length: 200,
                easing: Easing.CubicOut,
                finished: (v, c) => tcs.SetResult()
            );

            await tcs.Task;
        }

        private async Task StartAnimation(HitType hitType)
        {
            var tasks = new List<Task>();

            foreach (var anim in GetAnimations(hitType))
            {
                tasks.Add(anim());
            }

            await Task.WhenAll(tasks);
        }
        private IEnumerable<Func<Task>> GetAnimations(HitType hit)
        {
            var list = new List<Func<Task>>();

            // ボールアニメは常に実行
            list.Add(() => MoveBallAsync());

            int basesToAdvance = hit switch
            {
                HitType.Single => 1,
                HitType.Double => 2,
                HitType.Triple => 3,
                HitType.HomeRun => 4,
            };

            List<int> runners = _model.GameEntity.Value.GetAllRunners();

            foreach (var r in runners)
            {
                // ★ ランナーごとに直列アニメを返す
                list.Add(() => RunAdvanceAsync(r, basesToAdvance));
            }

            return list;
        }


        private async Task RunAdvanceAsync(int startBase, int basesToAdvance)
        {
            int current = startBase;

            for (int i = 0; i < basesToAdvance; i++)
            {
                switch (current)
                {
                    case 0: await RunFromHomeTo1Base(); break;
                    case 1: await RunFrom1BaseTo2Base(); break;
                    case 2: await RunFrom2BaseTo3Base(); break;
                    case 3: await RunFrom3BaseToHome(); break;
                }

                current++;
                if (current > 3) break; // ホームを超えたら終了
            }
        }



        #endregion

        #region Helpers =====================================================================================================

        public enum HitType
        {
            Single,   // 単打
            Double,   // 二塁打
            Triple,   // 三塁打
            HomeRun   // 本塁打
        }
        
        #endregion
    }
}
