using Microsoft.Maui.Controls.Shapes;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Linq;
using System.Text;

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
                await Task.WhenAll(
                    MoveBallAsync(),
                    RunFromHomeTo1Base(),
                    RunFrom1BaseTo2Base()
                );
                _model.NotifyHit(1);
            });
            TwoBaseHitCommand.Subscribe(async () =>
            {
                _model.NotifyHit(2);
            });
            ThreeBaseHitCommand.Subscribe(async () =>
            {
                _model.NotifyHit(3);
            });
            HomeRunCommand.Subscribe(async () =>
            {
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


        #endregion

        #region Helpers =====================================================================================================
        #endregion
    }
}
