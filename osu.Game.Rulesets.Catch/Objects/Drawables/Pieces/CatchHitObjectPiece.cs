// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Catch.Objects.Drawables.Pieces
{
    public class CatchHitObjectPiece : CompositeDrawable
    {
        public readonly Bindable<Color4> AccentColour = new Bindable<Color4>();
        public readonly Bindable<bool> HyperDash = new Bindable<bool>();

        [Resolved(canBeNull: true)]
        private DrawableHitObject drawableHitObject { get; set; }

        [CanBeNull]
        protected DrawablePalpableCatchHitObject DrawableHitObject => (DrawablePalpableCatchHitObject)drawableHitObject;

        [CanBeNull]
        protected BorderPiece BorderPiece;

        [CanBeNull]
        protected HyperBorderPiece HyperBorderPiece;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (DrawableHitObject != null)
            {
                AccentColour.BindTo(DrawableHitObject.AccentColour);
                HyperDash.BindTo(DrawableHitObject.HyperDash);
            }

            HyperDash.BindValueChanged(hyper =>
            {
                if (HyperBorderPiece != null)
                    HyperBorderPiece.Alpha = hyper.NewValue ? 1 : 0;
            }, true);
        }

        protected override void Update()
        {
            if (BorderPiece != null && DrawableHitObject?.HitObject != null)
                BorderPiece.Alpha = (float)Math.Clamp((DrawableHitObject.HitObject.StartTime - Time.Current) / 500, 0, 1);
        }
    }
}
