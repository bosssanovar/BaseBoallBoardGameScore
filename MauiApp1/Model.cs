using BaseBallBoardGameScoreEntity;

using Reactive.Bindings;

using System;
using System.Collections.Generic;
using System.Text;

namespace MauiApp1
{
    internal class Model
    {
        public ReactivePropertySlim<GameEntity> GameEntity { get; } = new(new());

        internal void Initialize()
        {
            GameEntity.Value = new();
        }

        internal void NotifyBall()
        {
            var cloned = GameEntity.Value.Clone();
            cloned.NotifyBall();
            GameEntity.Value = cloned;
        }

        internal void NotifyFoul()
        {
            var cloned = GameEntity.Value.Clone();
            cloned.NotifyFoul();
            GameEntity.Value = cloned;
        }

        internal void NotifyHit(int v)
        {
            var cloned = GameEntity.Value.Clone();
            cloned.NotifyHit(v);
            GameEntity.Value = cloned;
        }

        internal void NotifyHomeRun()
        {
            var cloned = GameEntity.Value.Clone();
            cloned.NotifyHomeRun();
            GameEntity.Value = cloned;
        }

        internal void NotifyOut()
        {
            var cloned = GameEntity.Value.Clone();
            cloned.NotifyOut();
            GameEntity.Value = cloned;
        }

        internal void NotifyStrike()
        {
            var cloned = GameEntity.Value.Clone();
            cloned.NotifyStrike();
            GameEntity.Value = cloned;
        }
    }
}
