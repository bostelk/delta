using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI.Controls
{
    internal class CaptionButton : Button
    {
        public CaptionButton()
            : base()
        {
        }

        protected override void OnReleased()
        {
            base.OnReleased();
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            //switch (_buttonType)
            //{
            //    case CaptionButtonType.Close:
            //        spriteBatch.DrawPixel(_innerRenderPosition + _innerRenderSize / 2, Color.White);
            //        break;
            //}
        }

        //public override void UpdateShape()
        //{
        //    switch (_buttonType)
        //    {
        //        default:
        //        case CaptionButtonType.Close:
        //            buttonDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, Shape.Rectangle));
        //            crossDescriptor.UpdateShape(ShapeDescriptor.Join(
        //                                            Shapes.DrawLine(4, Color.AntiqueWhite,
        //                                                            crossTopLeftAbsolutePosition,
        //                                                            crossTopLeftAbsolutePosition +
        //                                                            new Vector2(DefaultCrossWidth, DefaultCrossWidth)),
        //                                            Shapes.DrawLine(4, Color.AntiqueWhite,
        //                                                            crossTopLeftAbsolutePosition +
        //                                                            new Vector2(0, DefaultCrossWidth),
        //                                                            crossTopLeftAbsolutePosition +
        //                                                            new Vector2(DefaultCrossWidth, 0)
        //                                                ))
        //                );
        //            break;
        //    }
        //}

        //public override void CreateShape()
        //{
        //    base.CreateShape();

        //    switch (_buttonType)
        //    {
        //        default:
        //        case CaptionButtonType.Close:
        //            buttonDescriptor = ShapeDescriptor.ComputeShape(this, Shape.Rectangle);
        //            crossDescriptor = ShapeDescriptor.Join(
        //                Shapes.DrawLine(4, Color.AntiqueWhite,
        //                                crossTopLeftAbsolutePosition,
        //                                crossTopLeftAbsolutePosition + new Vector2(DefaultCrossWidth, DefaultCrossWidth)),
        //                Shapes.DrawLine(4, Color.AntiqueWhite,
        //                                crossTopLeftAbsolutePosition + new Vector2(0, DefaultCrossWidth),
        //                                crossTopLeftAbsolutePosition + new Vector2(DefaultCrossWidth, 0)
        //                    )
        //                );
        //            buttonDescriptor.Depth = depth;
        //            crossDescriptor.Depth = Depth.AsChildOf(depth);

        //            shapeDescriptors[0] = buttonDescriptor;
        //            shapeDescriptors[1] = crossDescriptor;
        //            break;
        //    }
        //}

    }
}
