// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Skinning;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Osu.Skinning.Legacy
{
    public partial class LegacyReverseArrow : BeatSyncedContainer
    {
        [Resolved]
        private DrawableHitObject drawableRepeat { get; set; } = null!;

        [Resolved(canBeNull: true)]
        private DrawableHitObject? drawableHitObject { get; set; }

        private Drawable proxy = null!;

        private Bindable<Color4> accentColour = null!;

        private bool textureIsDefaultSkin;

        private Drawable arrow = null!;

        [BackgroundDependencyLoader]
        private void load(ISkinSource skinSource)
        {
            Divisor = 2;
            MinimumBeatLength = 200;

            AutoSizeAxes = Axes.Both;

            string lookupName = new OsuSkinComponentLookup(OsuSkinComponents.ReverseArrow).LookupName;

            var skin = skinSource.FindProvider(s => s.GetTexture(lookupName) != null);

            InternalChild = arrow = (skin?.GetAnimation(lookupName, true, true, maxSize: OsuHitObject.OBJECT_DIMENSIONS) ?? Empty());
            textureIsDefaultSkin = skin is ISkinTransformer transformer && transformer.Skin is DefaultLegacySkin;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            proxy = CreateProxy();

            if (drawableHitObject != null)
            {
                drawableHitObject.HitObjectApplied += onHitObjectApplied;
                onHitObjectApplied(drawableHitObject);

                accentColour = drawableHitObject.AccentColour.GetBoundCopy();
                accentColour.BindValueChanged(c =>
                {
                    arrow.Colour = textureIsDefaultSkin && c.NewValue.R + c.NewValue.G + c.NewValue.B > (600 / 255f) ? Color4.Black : Color4.White;
                }, true);
            }
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes)
        {
            if (!drawableRepeat.Judged)
                Child.ScaleTo(1.3f).ScaleTo(1f, timingPoint.BeatLength, Easing.Out);
        }

        private void onHitObjectApplied(DrawableHitObject drawableObject)
        {
            Debug.Assert(proxy.Parent == null);

            // see logic in LegacySliderHeadHitCircle.
            (drawableObject as DrawableSliderRepeat)?.DrawableSlider
                                                    .OverlayElementContainer.Add(proxy);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (drawableHitObject != null)
                drawableHitObject.HitObjectApplied -= onHitObjectApplied;
        }
    }
}
