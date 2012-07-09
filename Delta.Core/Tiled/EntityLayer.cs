using System;

#if WINDOWS
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Delta.Collision.Geometry;
using Delta.Collision;
using Delta.Graphics;
#endif

namespace Delta.Tiled
{

    public class EntityLayer : EntityParent<IEntity>, ILayer
    {
        public float Parallax { get; set; }

        public EntityLayer()
            : base()
        {
        }

#if WINDOWS
        public EntityLayer(string fileName, XmlNode node)
            : base()
        {
            this.ImportLayer(node);

            foreach (XmlNode objectNode in node.SelectNodes("object"))
            {
                IEntity entity = StyleSheet.Load(objectNode.Attributes["type"].Value);

                if (entity == null)
                    continue;

                // parse all the data from the tiled object.
                String name = objectNode.Attributes["name"] == null ? String.Empty : objectNode.Attributes["name"].Value;
                Vector2 position = new Vector2(
                    objectNode.Attributes["x"] == null ? 0 : float.Parse(objectNode.Attributes["x"].Value, CultureInfo.InvariantCulture),
                    objectNode.Attributes["y"] == null ? 0 : float.Parse(objectNode.Attributes["y"].Value, CultureInfo.InvariantCulture)
                );
                Vector2 size = new Vector2(
                    objectNode.Attributes["width"] == null ? 0 : float.Parse(objectNode.Attributes["width"].Value, CultureInfo.InvariantCulture),
                    objectNode.Attributes["height"] == null ? 0 : float.Parse(objectNode.Attributes["height"].Value, CultureInfo.InvariantCulture)
                );
                
                // the distinction between polygon and polyline is determined by the entity type.
                bool IsPolygon;
                List<Vector2> polyVertices = new List<Vector2>();
                XmlNode polyNode = objectNode["polygon"];
                if (IsPolygon = polyNode == null)
                    polyNode = objectNode["polyline"];
                if (polyNode != null)
                {
                    foreach (string point in polyNode.Attributes["points"].Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] split = point.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (split.Length == 2)
                            polyVertices.Add(position + new Vector2(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture)));
                        else
                            throw new Exception(string.Format("The poly point'{0}' is not in the format 'x,y'. Map: {1}", point, fileName));
                    }
                }

                // all entities get a name. Made unique when added to an EntityParent
                entity.ID = name;

                // attempt to set entity specific properties.
                TransformableEntity transformableEntity = entity as TransformableEntity;
                if (transformableEntity != null)
                {
                    transformableEntity.Position = position;
                }

                CollideableEntity collideableEntity = entity as CollideableEntity;
                if (collideableEntity != null)
                {
                    if (size != Vector2.Zero)
                    {
                        collideableEntity.Polygon = new OBB(size.X, size.Y);
                        // tiled's position is the top-left position of a tile. position the entity at the tile center.
                        collideableEntity.Position += new Vector2(size.X / 2, size.Y / 2);
                    } else {
                        /*
                        // remove the closing vertex
                        if (polyVertices[0] == polyVertices[polyVertices.Count - 1])
                            polyVertices.RemoveAt(polyVertices.Count - 1);
                        */
                        // unless the polygon is convex decompose it into polylines.
                        Vector2 distance = Vector2.Zero;
                        Vector2 totalDistance = Vector2.Zero;
                        for (int i = 0; i < polyVertices.Count; i++)
                        {
                            CollideableEntity line = new CollideableEntity();
                            line.Polygon = new Polygon(polyVertices[i], polyVertices[(i+1)%(polyVertices.Count-1)]);
                            distance =  (polyVertices[(i + 1) % (polyVertices.Count - 1)] - polyVertices[i]);
                            totalDistance += distance;
                            line.Position = position + totalDistance - distance / 2;
                            Add(line);
                        }
                    }
                }

                // populate entity properties with tiled properties or try invoking a method.
                entity.ImportXmlProperties(objectNode["properties"]);
                   
                bool added = false;
                SpriteEntity sprite = entity as SpriteEntity;
                if (sprite != null)
                {
                    if (sprite.IsOverlay)
                    {
                        if (sprite.IsVisible)
                            sprite.IsVisible = IsVisible;
                        added = Map.Instance.OverlayLayer.Add(sprite);
                    }
                }

                if (!added)
                    Add(entity);
            }
        }
#endif
    }
}