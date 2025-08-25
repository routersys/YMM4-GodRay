using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Effects;

namespace YukkuriMovieMaker.Plugin.Community.Effect.Video.GodRay
{
    [VideoEffect("ゴッドレイ", [VideoEffectCategories.Drawing], ["godray", "light ray", "sun ray", "光線", "ゴッドレイ"])]
    public class GodRayVideoEffect : VideoEffectBase
    {
        public override string Label => "ゴッドレイ";

        [Display(GroupName = "ゴッドレイ", Name = "", Order = 0)]
        [UpdateCheckPanelEditor]
        public bool UpdateCheckPlaceholder { get; set; }

        [Display(GroupName = "ゴッドレイ", Name = "X", Description = "光源のX座標", Order = 100)]
        [AnimationSlider("F1", "px", -500d, 500d)]
        public Animation X { get; } = new Animation(0, YMM4Constants.VerySmallValue, YMM4Constants.VeryLargeValue);

        [Display(GroupName = "ゴッドレイ", Name = "Y", Description = "光源のY座標", Order = 101)]
        [AnimationSlider("F1", "px", -500d, 500d)]
        public Animation Y { get; } = new Animation(0, YMM4Constants.VerySmallValue, YMM4Constants.VeryLargeValue);

        [Display(GroupName = "ゴッドレイ", Name = "強度", Description = "光線の強度", Order = 102)]
        [AnimationSlider("F2", "", 0d, 2d)]
        public Animation Intensity { get; } = new Animation(0.5, 0, YMM4Constants.VeryLargeValue);

        [Display(GroupName = "ゴッドレイ", Name = "減衰", Description = "光線の減衰率", Order = 103)]
        [AnimationSlider("F3", "", 0.99d, 1d)]
        public Animation Decay { get; } = new Animation(0.995, 0, 1);

        [Display(GroupName = "ゴッドレイ", Name = "密度", Description = "光線の密度", Order = 104)]
        [AnimationSlider("F2", "", 0.1d, 2d)]
        public Animation Density { get; } = new Animation(0.8, 0, YMM4Constants.VeryLargeValue);

        [Display(GroupName = "ゴッドレイ", Name = "重み", Description = "光線の重み", Order = 105)]
        [AnimationSlider("F2", "", 0d, 1d)]
        public Animation Weight { get; } = new Animation(0.1, 0, 1);

        [Display(GroupName = "ゴッドレイ", Name = "色", Description = "光線の色", Order = 106)]
        [ColorPicker]
        public Color LightColor { get => lightColor; set => Set(ref lightColor, value); }
        Color lightColor = Color.FromRgb(255, 255, 200);

        [Display(GroupName = "ゴッドレイ", Name = "揺らぎの周期", Description = "光線の揺らぎの周期", Order = 107)]
        [AnimationSlider("F2", "", 0d, 100d)]
        public Animation FluctuationFrequency { get; } = new Animation(10, 0, 100);

        [Display(GroupName = "ゴッドレイ", Name = "揺らぎの量", Description = "光線の揺らぎの大きさ", Order = 108)]
        [AnimationSlider("F2", "px", 0d, 100d)]
        public Animation FluctuationAmount { get; } = new Animation(5, 0, 100);

        public override IEnumerable<string> CreateExoVideoFilters(int keyFrameIndex, ExoOutputDescription exoOutputDescription)
        {
            var fps = exoOutputDescription.VideoInfo.FPS;

            yield return $"_name=アニメーション効果\r\n" +
                $"_disable={(IsEnabled ? 0 : 1)}\r\n" +
                $"track0={X.ToExoString(keyFrameIndex, "F2", fps)}\r\n" +
                $"track1={Y.ToExoString(keyFrameIndex, "F2", fps)}\r\n" +
                $"track2={Intensity.ToExoString(keyFrameIndex, "F2", fps)}\r\n" +
                $"track3={Decay.ToExoString(keyFrameIndex, "F3", fps)}\r\n" +
                $"name=ゴッドレイ@YMM4\r\n" +
                $"param=\r\n";
        }

        public override IVideoEffectProcessor CreateVideoEffect(IGraphicsDevicesAndContext devices)
        {
            return new GodRayVideoEffectProcessor(devices, this);
        }

        protected override IEnumerable<IAnimatable> GetAnimatables() =>
        [
            X,
            Y,
            Intensity,
            Decay,
            Density,
            Weight,
            FluctuationFrequency,
            FluctuationAmount
        ];
    }
}