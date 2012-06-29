using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Content
{
    public class DeltaContentManager : ContentManager
    {

        public DeltaContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory) 
        {
        }

        public DeltaContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override T Load<T>(string assetName)
        {
            T obj = base.Load<T>(assetName);
            if (G._contentReferences.ContainsKey(assetName))
                G._contentReferences[assetName] = obj;
            else
                G._contentReferences.Add(assetName, obj);
            return obj;
        }

        public override void Unload()
        {
            base.Unload();
            List<string> assetsReferencesToRemove = new List<string>();
            foreach (var asset in G._contentReferences)
            {
                if (asset.Value == null)
                    assetsReferencesToRemove.Add(asset.Key);
            }
            for (int x = 0; x < assetsReferencesToRemove.Count; x++)
                G._contentReferences.Remove(assetsReferencesToRemove[x]);
        }


    }
}
