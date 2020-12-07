// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Catch.Objects.Drawables.Pieces
{
    public class DropletPiece : CatchHitObjectPiece
    {
        public DropletPiece()
        {
            Size = new Vector2(CatchHitObject.OBJECT_RADIUS / 2);

            InternalChildren = new Drawable[]
            {
                new Pulp
                {
                    RelativeSizeAxes = Axes.Both,
                    AccentColour = { BindTarget = AccentColour }
                },
                HyperBorderPiece = new HyperDropletBorderPiece()
            };
        }
    }
}
