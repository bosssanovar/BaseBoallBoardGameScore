using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace MauiApp1
{
    internal class ViewModel
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

        public ReadOnlyReactivePropertySlim<Visibility> Ball1Visibility { get; }
        public ReadOnlyReactivePropertySlim<Visibility> Ball2Visibility { get; }
        public ReadOnlyReactivePropertySlim<Visibility> Ball3Visibility { get; }
        public ReadOnlyReactivePropertySlim<Visibility> Strike1Visibility { get; }
        public ReadOnlyReactivePropertySlim<Visibility> Strike2Visibility { get; }
        public ReadOnlyReactivePropertySlim<Visibility> Out1Visibility { get; }
        public ReadOnlyReactivePropertySlim<Visibility> Out2Visibility { get; }

        public ReactiveCommand BallCommand { get; } = new();
        public ReactiveCommand StrikeCommand { get; } = new();

        public ReactiveCommand FoulCommand { get; } = new();
        public ReactiveCommand OutCommand { get; } = new();
        public ReactiveCommand SingleHitCommand { get; } = new();
        public ReactiveCommand TwoBaseHitCommand { get; } = new();
        public ReactiveCommand ThreeBaseHitCommand { get; } = new();
        public ReactiveCommand HomeRunCommand { get; } = new();

        #endregion

        #region Constructors ================================================================================================

        public ViewModel()
        {
            OmotePlayerBackground =
                _model.GameEntity
                .Select(
                    x =>
                    x.GetOffensePlayerNumber() == 1
                    ? new SolidColorBrush(Colors.Yellow)
                    : new SolidColorBrush(Color.FromArgb("#252525")))
                .ToReadOnlyReactivePropertySlim();
            UraPlayerBackground =
                _model.GameEntity
                .Select(
                    x =>
                    x.GetOffensePlayerNumber() == 2
                    ? new SolidColorBrush(Colors.Yellow)
                    : new SolidColorBrush(Color.FromArgb("#252525")))
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
                .Select(x => x.GetBallCount() > 0 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();
            Ball2Visibility =
                _model.GameEntity
                .Select(x => x.GetBallCount() > 1 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();
            Ball3Visibility =
                _model.GameEntity
                .Select(x => x.GetBallCount() > 2 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();
            Strike1Visibility =
                _model.GameEntity
                .Select(x => x.GetStrikeCount() > 0 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();
            Strike2Visibility =
                _model.GameEntity
                .Select(x => x.GetStrikeCount() > 1 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();
            Out1Visibility =
                _model.GameEntity
                .Select(x => x.GetOutCount() > 0 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();
            Out2Visibility =
                _model.GameEntity
                .Select(x => x.GetOutCount() > 1 ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactivePropertySlim();

            BallCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyBall();
            });
            StrikeCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyStrike();
            });

            FoulCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyFoul();
            });
            OutCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyOut();
            });
            SingleHitCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyHit(1);
            });
            TwoBaseHitCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyHit(2);
            });
            ThreeBaseHitCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyHit(3);
            });
            HomeRunCommand.Subscribe(() =>
            {
                _model.GameEntity.Value.NotifyHomeRun();
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
                : new SolidColorBrush(Color.FromArgb("#1f1f1f"));
        }

        #endregion

        #region Helpers =====================================================================================================
        #endregion
    }
}
