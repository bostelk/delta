
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentTypeWriter]
    public class EntityCollectionWriter<T> : ContentTypeWriter<EntityCollection<T>> where T : IEntity
    {
        protected override void Write(ContentWriter output, EntityCollection<T> value)
        {
            output.WriteRawObject<EntityBase>(value as EntityBase);
            List<IEntity> gameComponents = new List<IEntity>();
            foreach (var gameComponent in value.Children)
                gameComponents.Add(gameComponent);
            output.WriteObject<List<IEntity>>(gameComponents);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(EntityCollection<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(EntityCollectionReader<T>).AssemblyQualifiedName;
        }
    }
}
