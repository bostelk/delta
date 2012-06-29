using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Delta.Graphics.Effects
{
    public class SimpleEffect : Effect
    {
        EffectParameter _timeParameter;
        EffectParameter _flickerRateParameter;
        EffectParameter _colorParameter;
        EffectParameter _replaceColorsParameter;
        EffectParameter _withColorsParameter;
        EffectParameter _verticalScrollRateParameter;
        EffectParameter _horizontalScrollRateParameter;

        [Flags]
        public enum Technique
        {
            FillColor,
            Flicker,
            ReplaceColors,
            HorizontalScroll,
            VerticalScroll
        }

        public float Time
        {
            get
            {
                return _timeParameter.GetValueSingle();
            }
            set
            {
                _timeParameter.SetValue(value);
            }
        }

        public float FlickerRate
        {
            get
            {
                return _flickerRateParameter.GetValueSingle();
            }
            set
            {
                _flickerRateParameter.SetValue(value);
            }
        }

        public Color Color
        {
            get
            {
                return new Color(_colorParameter.GetValueVector4());
            }
            set
            {
                _colorParameter.SetValue(value.ToVector4());
            }
        }

        public Color[] ReplaceColors
        {
            get
            {
                Vector4[] val = _replaceColorsParameter.GetValueVector4Array(4);
                Color[] result = new Color[val.Length];
                for(int i = 0; i < val.Length; i++) {
                    result[i] = new Color(val[i]);
                }
                return result;
            }
            set
            {
                Vector4[] val = new Vector4[value.Length];
                for(int i = 0; i < value.Length; i++) {
                    val[i] = value[i].ToVector4();
                }
                _replaceColorsParameter.SetValue(val);
            }
        }

        public Color[] WithColors
        {
            get
            {
                Vector4[] val = _replaceColorsParameter.GetValueVector4Array(4);
                Color[] result = new Color[val.Length];
                for(int i = 0; i < val.Length; i++) {
                    result[i] = new Color(val[i]);
                }
                return result;
            }
            set
            {
                Vector4[] val = new Vector4[value.Length];
                for(int i = 0; i < value.Length; i++) {
                    val[i] = value[i].ToVector4();
                }
                _replaceColorsParameter.SetValue(val);
            }
        }

        public float VertialScrollRate
        {
            get
            {
                return _verticalScrollRateParameter.GetValueSingle();
            }
            set
            {
                _verticalScrollRateParameter.SetValue(value);
            }
        }

        public float HoriontalScrollRate
        {
            get
            {
                return _horizontalScrollRateParameter.GetValueSingle();
            }
            set
            {
                _horizontalScrollRateParameter.SetValue(value);
            }
        }
       public SimpleEffect(Effect e) : base(e)
        {
            _timeParameter = Parameters["Time"];
            _flickerRateParameter = Parameters["FlickerRate"];
            _colorParameter = Parameters["Color"];
            _replaceColorsParameter = Parameters["ColorsSrc"];
            _withColorsParameter = Parameters["ColorsDst"];
            //_horizontalScrollRateParameter = Parameters["HorizontalScrollRate"];
            //_verticalScrollRateParameter = Parameters["VerticalScrollRate"];
        }

        public void SetTechnique(Technique technique)
        {
            CurrentTechnique = Techniques[(int)technique];
        }
    }
}
