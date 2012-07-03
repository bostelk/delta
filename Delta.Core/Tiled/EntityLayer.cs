using System;

#if WINDOWS
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
                IEntity entity = null;
                if (objectNode.Attributes["type"] != null)
                {
                    string typeName = objectNode.Attributes["type"].Value;
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Type type = assembly.GetType(typeName, false, true);
                        if (type != null)
                            entity = Activator.CreateInstance(type) as IEntity;
                    }
                }
                else

                if (entity == null)
                    continue;

                entity.ID = objectNode.Attributes["name"] == null ? null : objectNode.Attributes["name"].Value;

                TransformableEntity transformableEntity = entity as TransformableEntity;
                if (transformableEntity != null)
                {
                    transformableEntity.Position = new Vector2(
                        objectNode.Attributes["x"] == null ? 0 : float.Parse(objectNode.Attributes["x"].Value, CultureInfo.InvariantCulture),
                        objectNode.Attributes["y"] == null ? 0 : float.Parse(objectNode.Attributes["y"].Value, CultureInfo.InvariantCulture)
                        );
                }

                entity.ImportXmlProperties(objectNode["properties"]);

                if (transformableEntity != null)
                    transformableEntity.Rotation = MathHelper.ToRadians(transformableEntity.Rotation);
                 
                //mapEntity.Size = new Vector2(
                //    objectNode.Attributes["width"] == null ? 0 : float.Parse(objectNode.Attributes["width"].Value, CultureInfo.InvariantCulture),
                //    objectNode.Attributes["height"] == null ? 0 : float.Parse(objectNode.Attributes["height"].Value, CultureInfo.InvariantCulture)
                //    );

                //List<Vector2> polyVertices = new List<Vector2>();
                //XmlNode polyNode = objectNode["polygon"];
                //if (polyNode == null)
                //    polyNode = objectNode["polyline"];
                //if (polyNode != null)
                //{
                //    foreach (string point in polyNode.Attributes["points"].Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        string[] split = point.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //        if (split.Length == 2)
                //            polyVertices.Add(position + new Vector2(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture)));
                //        else
                //            throw new Exception(string.Format("The poly point'{0}' is not the format 'x,y'. Map: {1}", point, fileName));
                //    }
                //}

                //IEntity entity = mapEntity as IEntity;
                //if (entity != null)
                //{
                //    entity.ID = objectName;
                //    entity.Position = position;

                    

                    //IDrawable2D drawable2D = mapObject as IDrawable2D;
                    //if (drawable2D != null)
                    //{
                    //    drawable2D.Position = position;
                    //    drawable2D.Size = size;
                    //}

                    //WrappedFixture wrappedFixture = mapObject as WrappedFixture;
                    //if (wrappedFixture != null)
                    //{
                    //    wrappedFixture.Position = position;

                    //    Delta.Physics.Geometry.Rectangle rectangle = mapObject as Delta.Physics.Geometry.Rectangle;
                    //    if (rectangle != null)
                    //        rectangle.Size = size;

                    //    Delta.Physics.Geometry.Polyline polyline = mapObject as Delta.Physics.Geometry.Polyline;
                    //    if (polyline != null)
                    //    {
                    //        if (polyVertices[0] == polyVertices[polyVertices.Count - 1])
                    //            polyVertices.RemoveAt(polyVertices.Count - 1);
                    //        polyline.Vertices = polyVertices;
                    //    }

                    //    Delta.Physics.Geometry.Polygon polygon = mapObject as Delta.Physics.Geometry.Polygon;
                    //    if (polygon != null)
                    //        polygon.Vertices = polyVertices;
                    //}

                    Add(entity);
                //}
            }
        }
#endif
    }
}