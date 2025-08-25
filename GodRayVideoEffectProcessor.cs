using Vortice.Direct2D1;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Player.Video.Effects;

namespace YukkuriMovieMaker.Plugin.Community.Effect.Video.GodRay
{
    public class GodRayVideoEffectProcessor(IGraphicsDevicesAndContext devices, GodRayVideoEffect item) : VideoEffectProcessorBase(devices)
    {
        GodRayCustomEffect? godRayEffect;

        public override DrawDescription Update(EffectDescription effectDescription)
        {
            if (IsPassThroughEffect || godRayEffect is null) return effectDescription.DrawDescription;

            var frame = effectDescription.ItemPosition.Frame;
            var length = effectDescription.ItemDuration.Frame;
            var fps = effectDescription.FPS;

            var x = item.X.GetValue(frame, length, fps);
            var y = item.Y.GetValue(frame, length, fps);
            var intensity = item.Intensity.GetValue(frame, length, fps);
            var decay = item.Decay.GetValue(frame, length, fps);
            var density = item.Density.GetValue(frame, length, fps);
            var weight = item.Weight.GetValue(frame, length, fps);
            var lightColor = item.LightColor;
            var fluctuationFrequency = item.FluctuationFrequency.GetValue(frame, length, fps);
            var fluctuationAmount = item.FluctuationAmount.GetValue(frame, length, fps);

            godRayEffect.X = (float)x;
            godRayEffect.Y = (float)y;
            godRayEffect.Intensity = (float)intensity;
            godRayEffect.Decay = (float)decay;
            godRayEffect.Density = (float)density;
            godRayEffect.Weight = (float)weight;
            godRayEffect.ColorR = lightColor.R / 255f;
            godRayEffect.ColorG = lightColor.G / 255f;
            godRayEffect.ColorB = lightColor.B / 255f;
            godRayEffect.Time = frame;
            godRayEffect.FluctuationFrequency = (float)fluctuationFrequency;
            godRayEffect.FluctuationAmount = (float)fluctuationAmount;

            return effectDescription.DrawDescription;
        }

        protected override void ClearEffectChain()
        {
            SetInput(null);
        }

        protected override ID2D1Image? CreateEffect(IGraphicsDevicesAndContext devices)
        {
            godRayEffect = new(devices);
            if (!godRayEffect.IsEnabled)
            {
                godRayEffect.Dispose();
                godRayEffect = null;
                return null;
            }
            disposer.Collect(godRayEffect);

            var output = godRayEffect.Output;
            disposer.Collect(output);
            return output;
        }

        protected override void setInput(ID2D1Image? input)
        {
            godRayEffect?.SetInput(0, input, true);
        }
    }
}