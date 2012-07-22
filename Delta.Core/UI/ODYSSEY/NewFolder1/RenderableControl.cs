//#region Using directives

//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public abstract class RenderableControl : BaseControl, IRenderableControl
//    {
//        protected const int DefaultBorderSize = 2;

//        protected Shape shape;
//        protected BorderStyle borderStyle;
//        protected Shapes.ShadingMode shadingMode;

//        protected bool applyStatusChanges = true;

//        protected int borderSize = DefaultBorderSize;

//        protected Color innerAreaColor;
//        protected Color borderColor;
//        protected ColorArray colorArray;

//        protected ShapeDescriptor[] shapeDescriptors;

//        #region Constructors

//        /// <summary>
//        /// Initializes a new instance of the <see cref="RenderableControl"/> class.
//        /// </summary>
//        /// <param name="id">The id.</param>
//        /// <param name="position">The position.</param>
//        /// <param name="size">The size.</param>
//        /// <param name="shape">The shape.</param>
//        /// <param name="style">The style.</param>
//        /// <param name="shadeMode">The shading method to use.</param>
//        /// <param name="colorArray">The color array.</param>
//        public RenderableControl(string id, Vector2 position, Size size, Shape shape,
//                                 BorderStyle style, Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, position, size)
//        {
//            this.shape = shape;
//            borderStyle = style;
//            this.colorArray = colorArray;
//            shadingMode = shadeMode;


//            innerAreaColor = colorArray.Enabled;
//            borderColor = colorArray.BorderEnabled;
//        }

//        #endregion

//        #region Properties

//        /// <summary>
//        /// Gets or sets the shape.
//        /// </summary>
//        /// <value>The shape.</value>
//        public Shape Shape
//        {
//            get { return shape; }
//            set { shape = value; }
//        }

//        /// <summary>
//        /// Gets or sets the size of the border.
//        /// </summary>
//        /// <value>The size of the border.</value>
//        public int BorderSize
//        {
//            get { return borderSize; }
//            set { borderSize = value; }
//        }

//        /// <summary>
//        /// Gets or sets the border style.
//        /// </summary>
//        /// <value>The border style.</value>
//        public BorderStyle BorderStyle
//        {
//            get { return borderStyle; }
//            set { borderStyle = value; }
//        }

//        /// <summary>
//        /// Gets or sets the color of the inner area.
//        /// </summary>
//        /// <value>The color of the inner area.</value>
//        public Color InnerAreaColor
//        {
//            get { return innerAreaColor; }
//            set { innerAreaColor = value; }
//        }

//        /// <summary>
//        /// Gets or sets the color of the border.
//        /// </summary>
//        /// <value>The color of the border.</value>
//        public Color BorderColor
//        {
//            get { return borderColor; }
//            set { borderColor = value; }
//        }

//        /// <summary>
//        /// Gets or sets a value indicating whether to apply status changes.
//        /// </summary>
//        /// <value><c>true</c> if status changes are applied for this control; otherwise, <c>false</c>.</value>
//        public bool ApplyStatusChanges
//        {
//            get { return applyStatusChanges; }
//            set { applyStatusChanges = value; }
//        }

//        /// <summary>
//        /// Gets or sets the shade mode.
//        /// </summary>
//        /// <value>The shade mode.</value>
//        public Shapes.ShadingMode ShadeMode
//        {
//            get { return shadingMode; }
//            set { shadingMode = value; }
//        }

//        /// <summary>
//        /// Gets the shape descriptors array.
//        /// </summary>
//        /// <value>The shape descriptors.</value>
//        public virtual ShapeDescriptor[] ShapeDescriptors
//        {
//            get { return shapeDescriptors; }
//        }

//        #endregion

//        #region Base Events

//        protected override void OnMouseEnter(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseEnter(ctl, args);
//            if (applyStatusChanges)
//            {
//                if (UI.ClickButton == args.Button)
//                {
//                    if (UI.CurrentHud.ClickedControl == this)
//                    {
//                        isClicked = true;
//                        Update();
//                    }
//                    else
//                        return;
//                }
//                else
//                {
//                    isHighlighted = true;
//                    Update();
//                }
//            }
//        }

//        protected override void OnMouseLeave(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseLeave(ctl, args);
//            if (applyStatusChanges)
//            {
//                if (UI.ClickButton == args.Button)
//                {
//                    if (UI.CurrentHud.ClickedControl == this)
//                    {
//                        isHighlighted = isClicked = false;
//                        Update();
//                    }
//                    else
//                        return;
//                }
//                else
//                {
//                    isHighlighted = false;
//                    Update();
//                }
//            }
//        }

//        protected override void OnMouseDown(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseDown(ctl, args);
//            if (applyStatusChanges)
//            {
//                Update();
//            }

//            if (UI.CurrentHud.FocusedControl != this && UI.CurrentHud.FocusedControl != UI.CurrentHud)
//                LoseFocus(UI.CurrentHud.FocusedControl);
//        }

//        protected override void OnMouseUp(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseUp(ctl, args);
//            if (applyStatusChanges)
//            {
//                Update();
//            }
//        }

//        protected override void OnGotFocus(BaseControl ctl)
//        {
//            base.OnGotFocus(ctl);
//            if (applyStatusChanges)
//            {
//                Update();
//            }
//        }

//        protected override void OnLostFocus(BaseControl ctl)
//        {
//            base.OnLostFocus(ctl);
//            if (applyStatusChanges)
//            {
//                Update();
//            }
//        }

//        protected override void OnSelectedChanged(BaseControl ctl)
//        {
//            base.OnSelectedChanged(ctl);
//            if (applyStatusChanges)
//            {
//                Update();
//            }
//        }

//        protected override void OnHighlightedChanged(BaseControl ctl)
//        {
//            base.OnVisibleChanged(ctl);
//            if (applyStatusChanges)
//            {
//                Update();
//            }
//        }

//        protected override void OnVisibleChanged(BaseControl ctl)
//        {
//            base.OnVisibleChanged(ctl);
//            if (!UI.CurrentHud.DesignState)
//                UI.CurrentHud.BuildVertexBuffer();
//        }


//        protected override void OnSizeChanged(BaseControl ctl)
//        {
//            base.OnSizeChanged(ctl);
//            if (!UI.CurrentHud.DesignState)
//                Update();
//        }

//        protected override void OnPositionChanged(BaseControl ctl)
//        {
//            base.OnPositionChanged(ctl);
//            if (!UI.CurrentHud.DesignState)
//                Update();
//        }

//        #endregion

//        #region Exposed events

//        public event ControlEventHandler ShapeUpdate;

//        protected virtual void OnShapeUpdate(BaseControl ctl)
//        {
//            if (ShapeUpdate != null)
//                ShapeUpdate(ctl);
//        }

//        #endregion

//        /// <summary>
//        /// Updates the status of the control.
//        /// </summary>
//        protected void UpdateStatus()
//        {
//            if (isEnabled)
//            {
//                if (isClicked)
//                {
//                    innerAreaColor = colorArray[ColorIndex.Clicked];
//                    borderColor = colorArray[ColorIndex.BorderHighlighted];
//                }
//                else if (isSelected)
//                {
//                    innerAreaColor = colorArray[ColorIndex.Selected];
//                    borderColor = colorArray[ColorIndex.BorderHighlighted];
//                }
//                else if (isFocused)
//                {
//                    innerAreaColor = colorArray[ColorIndex.Focused];
//                    borderColor = colorArray[ColorIndex.BorderHighlighted];
//                }
//                else if (isHighlighted)
//                {
//                    innerAreaColor = colorArray[ColorIndex.Highlighted];
//                    borderColor = colorArray[ColorIndex.BorderHighlighted];
//                }
//                else
//                {
//                    innerAreaColor = colorArray[ColorIndex.Enabled];
//                    borderColor = colorArray[ColorIndex.BorderEnabled];
//                }
//            }
//            else
//            {
//                innerAreaColor = colorArray[ColorIndex.Disabled];
//            }
//        }


//        public virtual void CreateShape()
//        {
//            ComputeAbsolutePosition();
//        }

//        public abstract void UpdateShape();


//        /// <summary>
//        /// Updates this instance of the control by recomputing its vertices and rewriting
//        /// its <see cref="ShapeDescriptor"/> object in the <see cref="HUD"/> vertexbuffer.
//        /// </summary>
//        public virtual void Update()
//        {
//            UpdateStatus();
//            if (!UI.CurrentHud.DesignState && !isDisposing)
//            {
//                UpdateShape();

//                foreach (ShapeDescriptor sDesc in ShapeDescriptors)
//                {
//                    UI.CurrentHud.UpdateControl(sDesc, this);
//                }
//            }
//        }
//    }
//}