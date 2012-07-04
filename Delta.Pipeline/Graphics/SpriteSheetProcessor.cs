using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Delta.Graphics
{

    [ContentProcessor(DisplayName = "SpriteSheetProcessor")]
    public class SpriteSheetProcessor : ContentProcessor<SpriteSheetContent, SpriteSheetContent>
    {
        public override SpriteSheetContent Process(SpriteSheetContent input, ContentProcessorContext context)
        {
            //load external images
            for (int i = 0; i < input.Images.Count; i++)
            {
                ImageContent externalImage = input.Images[i];
                externalImage._bitmapContent = context.BuildAndLoadAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(externalImage.Path), "TextureProcessor").Faces[0][0];
            }

            int maxTextureWidth = 0;
            int maxTextureHeight = 0;


            //maxTextureWidth = 1024;
            //maxTextureHeight = 1024;


            if (context.TargetPlatform == TargetPlatform.Windows)
            {
                if (context.TargetProfile == GraphicsProfile.Reach)
                {
                    maxTextureWidth = 2048;
                    maxTextureHeight = 2048;
                }
                if (context.TargetProfile == GraphicsProfile.HiDef)
                {
                    maxTextureWidth = 4096;
                    maxTextureHeight = 4096;
                }
            }


            //guess texture size by adding up all the frames widths and heights seperaetly and square rooting them.

            //build the block list
            int smallestFrameWidth = 0;
            int smallestFrameHeight = 0;
            for (int i = 0; i < input.Images.Count; i++)
            {
                ImageContent externalImage = input.Images[i];
                //calculate the smallest frame size used in this sprite sheet
                smallestFrameWidth = Math.Min(externalImage.FrameWidth, smallestFrameWidth);
                smallestFrameHeight = Math.Min(externalImage.FrameHeight, smallestFrameHeight);
                //find how many frames per row there are for this external image
                int framesPerRow = externalImage._bitmapContent.Width / externalImage.FrameWidth;
                for (int x = 0; x < (framesPerRow); x++) //loop through all the frames
                {
                    for (int y = 0; y < (externalImage._bitmapContent.Height / externalImage.FrameHeight); y++)
                    {
                        //create our sprite block which holds frame information
                        SpriteBlockContent block = new SpriteBlockContent();
                        block.Image = externalImage;
                        block.SourceRegion = new Rectangle(x * externalImage.FrameWidth, y * externalImage.FrameHeight, externalImage.FrameWidth, externalImage.FrameHeight);
                        //check if this an empty frame
                        PixelBitmapContent<Color> frameBitmap = new PixelBitmapContent<Color>(block.SourceRegion.Width, block.SourceRegion.Height);
                        BitmapContent.Copy(externalImage._bitmapContent, block.SourceRegion, frameBitmap, new Rectangle(0, 0, block.SourceRegion.Width, block.SourceRegion.Height));
                        bool isEmptyFrame = true;
                        //loop through all the pixels in the current frame
                        for (int pY = 0; pY < frameBitmap.Height; pY++)
                        {
                            //find a non-transparent pixel in the row
                            if (Array.Find<Color>(frameBitmap.GetRow(pY), a => (a.PackedValue > 0)).PackedValue > 0)
                            {
                                isEmptyFrame = false;
                                //once a non-transparent pixel is found don't loop any further!
                                break;
                            }
                        }
                        if (!isEmptyFrame) //if it's not empty add it!
                        {
                            //store the frame number of this frame, used by animations to find sprite sheet source rectangle
                            externalImage._frameReferences.Add(x + (y * framesPerRow), block);
                            input._blocks.Add(block);
                        }
                    }
                }
            }

            //sort the blocks by largest perimeter 
            input._blocks.Sort((a, b) => -((a.SourceRegion.Width * 2) + (a.SourceRegion.Height * 2)).CompareTo((b.SourceRegion.Width * 2) + (b.SourceRegion.Height * 2)));

            //pack the sprite blocks
            int outputHeight = 0;
            List<Rectangle> freeRegions = new List<Rectangle>(); //keep a list of unused regions
            freeRegions.Add(new Rectangle(0, 0, maxTextureWidth, maxTextureHeight)); //add the size of our workspace as the first free region
            for (int i = 0; i < input._blocks.Count; i++)
            {
                SpriteBlockContent block = input._blocks[i];
                ImageContent externalImage = block.Image;
                Rectangle destinationRegion = new Rectangle(0, 0, externalImage.FrameWidth, externalImage.FrameHeight);
                for (int r = 0; r < freeRegions.Count; r++) //loop through all the free regions
                {
                    Rectangle freeRegion = freeRegions[r];
                    //check if the sprite block will fit into this free region
                    if (freeRegion.Width >= destinationRegion.Width && freeRegion.Height >= destinationRegion.Height)
                    {
                        //if it fits, it sits
                        destinationRegion.X = freeRegion.X;
                        destinationRegion.Y = freeRegion.Y;
                        freeRegions.Remove(freeRegion); //remove the used region
                        //create the leftover free region to the right of the used region
                        Rectangle rightLeftoverRegion = new Rectangle(freeRegion.X + destinationRegion.Width, freeRegion.Y, freeRegion.Width - destinationRegion.Width, freeRegion.Height);
                        //create the leftover free region below the used region
                        Rectangle bottomLeftoverRegion = new Rectangle(freeRegion.X, freeRegion.Y + destinationRegion.Height, freeRegion.Width, freeRegion.Height - destinationRegion.Height);
                        //the two leftover regions must intersect each other!
                        Rectangle intersectingRegion = Rectangle.Intersect(rightLeftoverRegion, bottomLeftoverRegion);
                        //determine which leftover region has to remove the intersecting region
                        if (rightLeftoverRegion.Width * rightLeftoverRegion.Height > bottomLeftoverRegion.Width * bottomLeftoverRegion.Height)
                            bottomLeftoverRegion.Width -= intersectingRegion.Width;
                        else
                            rightLeftoverRegion.Height -= intersectingRegion.Height;
                        //add leftover regions if they are bigger than the smallest frames
                        if (rightLeftoverRegion.Width > smallestFrameWidth && rightLeftoverRegion.Height > smallestFrameHeight)
                            freeRegions.Add(rightLeftoverRegion);
                        if (bottomLeftoverRegion.Width > smallestFrameWidth && bottomLeftoverRegion.Height > smallestFrameHeight)
                            freeRegions.Add(bottomLeftoverRegion);
                        //break out of our loop, we are done here
                        break;
                    }
                }
                //sort the free regions by y-cordinattes, ensuring blocks get packed from top to bottom
                freeRegions.Sort((a, b) => (a.Y.CompareTo(b.Y)));
                //don't forgot to save the destination region to the sprite block!
                block.DestinationRegion = destinationRegion;
                //record the height in pixels we are using for far
                outputHeight = Math.Max(outputHeight, destinationRegion.Y + destinationRegion.Height);
                //if (outputHeight > maxTextureHeight)
                //    throw new Exception("The sprite sheet has overflown.");
            }

            //build the spritesheet texture
            BitmapContent outputBitmap = new PixelBitmapContent<Color>(maxTextureWidth, outputHeight);
            for (int i = 0; i < input._blocks.Count; i++)
            {
                SpriteBlockContent block = input._blocks[i];
                BitmapContent.Copy(block.Image._bitmapContent, block.SourceRegion, outputBitmap, block.DestinationRegion);
            }

            input._texture = new Texture2DContent();
            input._texture.Mipmaps.Add(outputBitmap);
            return input;

        }

    }
}