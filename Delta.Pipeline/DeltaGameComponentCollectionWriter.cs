
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentTypeWriter]
    public class DeltaGameComponentCollectionWriter<T> : ContentTypeWriter<DeltaGameComponentCollection<T>> where T : IGameComponent
    {
        protected override void Write(ContentWriter output, DeltaGameComponentCollection<T> value)
        {
            output.WriteRawObject<DeltaGameComponent>(value as DeltaGameComponent);
            List<IGameComponent> gameComponents = new List<IGameComponent>();
            foreach (var gameComponent in value.Components)
                gameComponents.Add(gameComponent);
            output.WriteObject<List<IGameComponent>>(gameComponents);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(DeltaGameComponentCollection<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(DeltaGameComponentCollectionReader<T>).AssemblyQualifiedName;
        }
    }
}
