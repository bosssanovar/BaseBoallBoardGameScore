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
    }
}
