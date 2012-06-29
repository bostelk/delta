
    using System;
    using System.ComponentModel;
using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

    namespace Delta
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContentTypeWriter]
        public class EntityParentWriter<T> : ContentTypeWriter<EntityParent<T>> where T : IEntity
        {
            protected override void Write(ContentWriter output, EntityParent<T> value)
            {
                output.WriteRawObject<Entity>(value as Entity);
                output.WriteObject<int>(value.Children.Count);
                for (int x = 0; x < value.Children.Count; x++)
                    output.WriteObject<T>(value.Children[x]);
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
