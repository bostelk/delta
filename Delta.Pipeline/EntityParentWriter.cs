
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta
{
    [ContentTypeWriter, EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityParentWriter<T> : ContentTypeWriter<EntityParent<T>> where T : Entity
    {
        protected override void Write(ContentWriter output, EntityParent<T> existingInstance)
        {
            output.WriteRawObject<Entity>(existingInstance as Entity);
            List<T> gameComponents = new List<T>();
            foreach (var gameComponent in existingInstance.Children)
                gameComponents.Add(gameComponent);
            output.WriteObject<List<T>>(gameComponents);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(EntityParent<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(EntityParentReader<T>).AssemblyQualifiedName;
        }
    }
}
