using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Input;

namespace Delta
{
    public class World : EntityManager<IEntity>
    {
        public IEntityCollection BelowGround { get; set; }
        public IEntityCollection Ground { get; set; }
        public IEntityCollection AboveGround { get; set; }

        public World()
            : base()
        {
        }

        public List<T> GetEntitiesUnderMouse<T>() where T: Entity
        {
            List<T> result = new List<T>();
            //foreach (Entity entity in Entity.GlobalEntities)
            //{
            //    T entityT = entity as T;
            //    if (entityT == null) continue;
            //    if ((entityT.Parent == null || !entityT.Parent.IsVisible) && !entityT.IsVisible) continue;
            //    Rectangle hitbox = new Rectangle((int)entityT.Position.X, (int)entityT.Position.Y, (int)(entityT.Size.X * entityT.Scale.X), (int)(entityT.Size.Y * entityT.Scale.Y));
            //    if (hitbox.Contains(G.World.Camera.ToWorldPosition(G.Input.Mouse.Position).ToPoint()))
            //        result.Add(entityT);
            //}
            return result;
        }
        
        //public string ToDebugString()
        //{
        //    StringBuilder text = new StringBuilder();
        //    text.Append(String.Format("{0,-60}{1,2}\n", "Name", "Layer"));
        //    foreach (IEntity gameComponent in _components)
        //        text.Append(gameComponent.ToString());
        //    return text.ToString();
        //}

        //public void DebugDraw()
        //{
        //    G.SpriteBatch.Begin();
        //    G.SpriteBatch.DrawString(G.Font, this.ToDebugString(), new Vector2(0, 100), Color.White);
        //    G.SpriteBatch.End();
        //}

    }
}
