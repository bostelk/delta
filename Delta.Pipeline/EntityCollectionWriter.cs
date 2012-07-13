
    using System;
    using System.ComponentModel;
using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

    namespace Delta
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContentTypeWriter]
        public class EntityCollectionWriter : ContentTypeWriter<EntityCollection>
        {
            protected override void Write(ContentWriter output, EntityCollection value)
            {
                output.WriteObject<List<IUpdateable>>(value._updateables);
                output.WriteObject<List<IDrawable>>(value._drawables);
            }

            public override string GetRuntimeType(TargetPlatform targetPlatform)
            {
                return typeof(EntityCollection).AssemblyQualifiedName;
            }

            public override string GetRuntimeReader(TargetPlatform targetPlatform)
            {
                return typeof(EntityCollectionReader).AssemblyQualifiedName;
            }
        }
    }
