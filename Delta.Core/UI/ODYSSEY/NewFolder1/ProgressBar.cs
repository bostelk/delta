//#region Using directives

//using System.ComponentModel;
//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class ProgressBar : RenderableControl, ISpriteControl
//    {
//        protected Label label;
//        protected float progress = 0.0f;
//        protected Size progressAreaSize;
//        protected Vector2 progressAreaPosition;
//        protected Vector2 progressAreaAbsolutePosition;

//        protected ShapeDescriptor progressFrameDescriptor;
//        protected ShapeDescriptor progressAreaDescriptor;

//        BackgroundWorker backgroundWorker;

//        protected static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.FromArgb(200, Color.MediumBlue),
//            Color.DeepSkyBlue,
//            Color.Gray,
//            Color.SteelBlue,
//            Color.RoyalBlue,
//            Color.Gray,
//            Color.Gray
//            );

//        #region Properties

//        public BackgroundWorker BackgroundWorker
//        {
//            get { return backgroundWorker; }
//        }

//        #endregion

//        #region Constructors

//        public ProgressBar(string id, Vector2 position, Size size) :
//            base(id, position, size, Shape.Rectangle, BorderStyle.Raised, Shapes.ShadeNone, defaultColors)
//        {
//            label = new Label(id + "_Label",
//                              progress.ToString("0%"),
//                              LabelSize.Normal,
//                              Alignment.Center,
//                              Alignment.Center,
//                              new Vector2(0, size.Height/2),
//                              Label.DefaultColor
//                );

//            progressAreaSize = new Size(0, size.Height - borderSize*2);
//            progressAreaPosition = new Vector2(borderSize, borderSize);

//            backgroundWorker = new BackgroundWorker();
//            backgroundWorker.WorkerReportsProgress = true;
//            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);

//            shapeDescriptors = new ShapeDescriptor[2];
//        }

//        #endregion

//        void ProgressChanged(object sender, ProgressChangedEventArgs e)
//        {
//            progress = ((float) e.ProgressPercentage/100);
//            label.Text = progress.ToString("0%");
//            Update();
//        }

//        public void ReportProgress(float newProgressValue)
//        {
//            backgroundWorker.ReportProgress((int) (newProgressValue*100));
//        }

//        #region Overriden inherited events

//        #endregion

//        #region IRenderable overriden methods

//        public override void ComputeVertices()
//        {
//            base.UpdateStatus();
//            progressAreaSize.Width = (int) (progress*(size.Width - borderSize*2));
//            progressAreaDescriptor.UpdateShape(
//                Shapes.DrawRectangle(progressAreaAbsolutePosition, progressAreaSize, colorArray.Highlighted));
//            shapeDescriptors[1] = progressAreaDescriptor;

//            progressAreaDescriptor.Depth = new Depth(windowOrder, 1, 0);
//        }


//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            progressAreaAbsolutePosition = Vector2.Add(progressAreaPosition, absolutePosition);
//            progressFrameDescriptor =
//                Shapes.DrawRectangularOutline(absolutePosition, size, borderSize, borderColor, borderStyle, Border.All);
//            progressAreaDescriptor =
//                Shapes.DrawRectangle(progressAreaAbsolutePosition, progressAreaSize, colorArray.Highlighted);

//            shapeDescriptors[0] = progressFrameDescriptor;
//            shapeDescriptors[1] = progressAreaDescriptor;
//            label.Parent = this;
//        }

//        #endregion

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }

//        #region ISpriteControl Members

//        public void Render()
//        {
//            label.Render();
//        }

//        #endregion
//    }
//}