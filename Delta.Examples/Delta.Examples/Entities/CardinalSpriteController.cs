using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Delta.Examples.Entities
{
    class CardinalSpriteController //: ISpriteController
    {
        static readonly float INV_SQRT_2 = (float)(1.0 / Math.Sqrt(2.0));
        static readonly Vector2[] _intercardinalVectors = new Vector2[] {
            new Vector2(0),             // idle
            new Vector2(0, -1),         // north
            new Vector2(INV_SQRT_2, -INV_SQRT_2),   // north east
            new Vector2(1, 0),          // east
            new Vector2(INV_SQRT_2, INV_SQRT_2),   // south east
            new Vector2(0, 1),          // south
            new Vector2(-INV_SQRT_2, INV_SQRT_2),   // south west
            new Vector2(-1, 0),         // west
            new Vector2(-INV_SQRT_2, -INV_SQRT_2),   // north west
        };

        SpriteEntity _sprite;
        string _animation;
        bool _inIdle;

        public IntercardinalDirection FacingDirection { get; private set; }
    
        public CardinalSpriteController(SpriteEntity sprite)
        {
            _sprite = sprite;
            WalkNorth();
            Idle();
        }

        public void WalkNorth()
        {
            _animation = "walkup";
            if (FacingDirection != IntercardinalDirection.North || _inIdle)
            {
                _sprite.Play(_animation, AnimationOptions.Looped);
                _inIdle = false;
            }
            _sprite.SpriteEffects = SpriteEffects.None;
            FacingDirection = IntercardinalDirection.North;
        }

        public void WalkSouth()
        {
            _animation = "walkdown";
            if (FacingDirection != IntercardinalDirection.South || _inIdle)
            {
                _sprite.Play(_animation, AnimationOptions.Looped);
                _inIdle = false;
            }
            _sprite.SpriteEffects = SpriteEffects.None;
            FacingDirection = IntercardinalDirection.South;
        }

        public void WalkEast()
        {
            _animation = "walkright";
            if (FacingDirection != IntercardinalDirection.East || _inIdle)
            {
                _sprite.Play(_animation, AnimationOptions.Looped);
                _inIdle = false;
            }
            _sprite.SpriteEffects = SpriteEffects.None;
            FacingDirection = IntercardinalDirection.East;
        }

        public void WalkWest()
        {
            _animation = "walkright";
            if (FacingDirection != IntercardinalDirection.West || _inIdle)
            {
                _sprite.Play(_animation, AnimationOptions.Looped);
                _inIdle = false;
            }
            _sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
            FacingDirection = IntercardinalDirection.West;
        }

        public void Idle()
        {
            _inIdle = true;
            _sprite.Play(_animation, AnimationOptions.None);
            _sprite.Pause();
        }

        public void Walk(Vector2 direction)
        {
            switch (vectorToIntercardinal(direction))
            {
                case IntercardinalDirection.North:
                case IntercardinalDirection.NorthEast:
                case IntercardinalDirection.NorthWest:
                    WalkNorth();
                    break;
                case IntercardinalDirection.South:
                case IntercardinalDirection.SouthEast:
                case IntercardinalDirection.SouthWest:
                    WalkSouth();
                    break;
                case IntercardinalDirection.West:
                    WalkWest();
                    break;
                case IntercardinalDirection.East:
                    WalkEast();
                    break;
                case IntercardinalDirection.Idle:
                    Idle();
                    break;
            }
        }

        private IntercardinalDirection vectorToIntercardinal(Vector2 direction)
        {
            for (int i = 0; i < _intercardinalVectors.Length; i++)
                if (Vector2Extensions.AlmostEqual(ref _intercardinalVectors[i], ref direction))
                    return (IntercardinalDirection) i;
            return IntercardinalDirection.Idle;
        }

        public override string ToString()
        {
            return _animation;
        }
    }
}
