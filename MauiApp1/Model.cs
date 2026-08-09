using BaseBallBoardGameScoreEntity;

using Reactive.Bindings;

using System;
using System.Collections.Generic;
using System.Text;

namespace MauiApp1
{
    internal class Model
    {
        private Stack<GameEntity> _entityUndoStack = new();

        public ReactivePropertySlim<GameEntity> GameEntity { get; } = new(new());

        internal void Initialize()
        {
            StackEntity();

            GameEntity.Value = new();
        }

        internal void Undo()
        {
            GameEntity.Value = _entityUndoStack.Pop().Clone();
        }

        internal void NotifyBall()
        {
            StackEntity();

            var cloned = GameEntity.Value.Clone();
            cloned.NotifyBall();
            GameEntity.Value = cloned;
        }

        internal void NotifyFoul()
        {
            StackEntity();

            var cloned = GameEntity.Value.Clone();
            cloned.NotifyFoul();
            GameEntity.Value = cloned;
        }

        internal void NotifyHit(int v)
        {
            StackEntity();

            var cloned = GameEntity.Value.Clone();
            cloned.NotifyHit(v);
            GameEntity.Value = cloned;
        }

        internal void NotifyHomeRun()
        {
            StackEntity();

            var cloned = GameEntity.Value.Clone();
            cloned.NotifyHomeRun();
            GameEntity.Value = cloned;
        }

        internal void NotifyOut()
        {
            StackEntity();

            var cloned = GameEntity.Value.Clone();
            cloned.NotifyOut();
            GameEntity.Value = cloned;
        }

        internal void NotifyStrike()
        {
            StackEntity();

            var cloned = GameEntity.Value.Clone();
            cloned.NotifyStrike();
            GameEntity.Value = cloned;
        }

        private void StackEntity()
        {
            _entityUndoStack.Push(GameEntity.Value.Clone());
        }
    }
}
