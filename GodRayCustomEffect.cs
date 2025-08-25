using GodRayPlugin;
using System.Runtime.InteropServices;
using Vortice;
using Vortice.Direct2D1;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;

namespace YukkuriMovieMaker.Plugin.Community.Effect.Video.GodRay
{
    public class GodRayCustomEffect(IGraphicsDevicesAndContext devices) : D2D1CustomShaderEffectBase(Create<EffectImpl>(devices))
    {
        public float X
        {
            set => SetValue((int)EffectImpl.Properties.X, value);
            get => GetFloatValue((int)EffectImpl.Properties.X);
        }
        public float Y
        {
            set => SetValue((int)EffectImpl.Properties.Y, value);
            get => GetFloatValue((int)EffectImpl.Properties.Y);
        }
        public float Intensity
        {
            set => SetValue((int)EffectImpl.Properties.Intensity, value);
            get => GetFloatValue((int)EffectImpl.Properties.Intensity);
        }
        public float Decay
        {
            set => SetValue((int)EffectImpl.Properties.Decay, value);
            get => GetFloatValue((int)EffectImpl.Properties.Decay);
        }
        public float Density
        {
            set => SetValue((int)EffectImpl.Properties.Density, value);
            get => GetFloatValue((int)EffectImpl.Properties.Density);
        }
        public float Weight
        {
            set => SetValue((int)EffectImpl.Properties.Weight, value);
            get => GetFloatValue((int)EffectImpl.Properties.Weight);
        }
        public float ColorR
        {
            set => SetValue((int)EffectImpl.Properties.ColorR, value);
            get => GetFloatValue((int)EffectImpl.Properties.ColorR);
        }
        public float ColorG
        {
            set => SetValue((int)EffectImpl.Properties.ColorG, value);
            get => GetFloatValue((int)EffectImpl.Properties.ColorG);
        }
        public float ColorB
        {
            set => SetValue((int)EffectImpl.Properties.ColorB, value);
            get => GetFloatValue((int)EffectImpl.Properties.ColorB);
        }
        public float Time
        {
            set => SetValue((int)EffectImpl.Properties.Time, value);
            get => GetFloatValue((int)EffectImpl.Properties.Time);
        }
        public float FluctuationFrequency
        {
            set => SetValue((int)EffectImpl.Properties.FluctuationFrequency, value);
            get => GetFloatValue((int)EffectImpl.Properties.FluctuationFrequency);
        }
        public float FluctuationAmount
        {
            set => SetValue((int)EffectImpl.Properties.FluctuationAmount, value);
            get => GetFloatValue((int)EffectImpl.Properties.FluctuationAmount);
        }


        [CustomEffect(1)]
        class EffectImpl : D2D1CustomShaderEffectImplBase<EffectImpl>
        {
            ConstantBuffer constants;

            [CustomEffectProperty(PropertyType.Float, (int)Properties.X)]
            public float X
            {
                set { constants.X = value; UpdateConstants(); }
                get => constants.X;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.Y)]
            public float Y
            {
                set { constants.Y = value; UpdateConstants(); }
                get => constants.Y;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.Intensity)]
            public float Intensity
            {
                set { constants.Intensity = value; UpdateConstants(); }
                get => constants.Intensity;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.Decay)]
            public float Decay
            {
                set { constants.Decay = value; UpdateConstants(); }
                get => constants.Decay;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.Density)]
            public float Density
            {
                set { constants.Density = value; UpdateConstants(); }
                get => constants.Density;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.Weight)]
            public float Weight
            {
                set { constants.Weight = value; UpdateConstants(); }
                get => constants.Weight;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.ColorR)]
            public float ColorR
            {
                set { constants.ColorR = value; UpdateConstants(); }
                get => constants.ColorR;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.ColorG)]
            public float ColorG
            {
                set { constants.ColorG = value; UpdateConstants(); }
                get => constants.ColorG;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.ColorB)]
            public float ColorB
            {
                set { constants.ColorB = value; UpdateConstants(); }
                get => constants.ColorB;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.Time)]
            public float Time
            {
                set { constants.Time = value; UpdateConstants(); }
                get => constants.Time;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.FluctuationFrequency)]
            public float FluctuationFrequency
            {
                set { constants.FluctuationFrequency = value; UpdateConstants(); }
                get => constants.FluctuationFrequency;
            }
            [CustomEffectProperty(PropertyType.Float, (int)Properties.FluctuationAmount)]
            public float FluctuationAmount
            {
                set { constants.FluctuationAmount = value; UpdateConstants(); }
                get => constants.FluctuationAmount;
            }

            public EffectImpl() : base(ShaderResourceLoader.GetShaderResource("GodRayShader.cso"))
            {

            }

            protected override void UpdateConstants()
            {
                drawInformation?.SetPixelShaderConstantBuffer(constants);
            }

            public override void MapInputRectsToOutputRect(RawRect[] inputRects, RawRect[] inputOpaqueSubRects, out RawRect outputRect, out RawRect outputOpaqueSubRect)
            {
                if (inputRects.Length != 1)
                    throw new ArgumentException("InputRects must be length of 1", nameof(inputRects));

                var input = inputRects[0];
                int expansion = 2000;

                outputRect = new RawRect(
                    input.Left - expansion,
                    input.Top - expansion,
                    input.Right + expansion,
                    input.Bottom + expansion);

                outputOpaqueSubRect = default;
            }

            public override void MapOutputRectToInputRects(RawRect outputRect, RawRect[] inputRects)
            {
                if (inputRects.Length != 1)
                    throw new ArgumentException("InputRects must be length of 1", nameof(inputRects));

                int expansion = 2000;

                inputRects[0] = new RawRect(
                    outputRect.Left - expansion,
                    outputRect.Top - expansion,
                    outputRect.Right + expansion,
                    outputRect.Bottom + expansion);
            }

            [StructLayout(LayoutKind.Sequential)]
            struct ConstantBuffer
            {
                public float X;
                public float Y;
                public float Intensity;
                public float Decay;
                public float Density;
                public float Weight;
                public float ColorR;
                public float ColorG;
                public float ColorB;
                public float Time;
                public float FluctuationFrequency;
                public float FluctuationAmount;
            }
            public enum Properties : int
            {
                X = 0,
                Y = 1,
                Intensity = 2,
                Decay = 3,
                Density = 4,
                Weight = 5,
                ColorR = 6,
                ColorG = 7,
                ColorB = 8,
                Time = 9,
                FluctuationFrequency = 10,
                FluctuationAmount = 11,
            }
        }
    }
}