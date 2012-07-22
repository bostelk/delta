//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Helpers;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public abstract class ContainerControl : DefaultShapeControl, IContainer
//    {
//        protected ControlCollection controls;
//        protected ControlCollection internalControls;

//        #region Properties

//        internal override Depth Depth
//        {
//            get { return base.Depth; }
//            set
//            {
//                base.Depth = value;
//                foreach (BaseControl ctl in internalControls)
//                    ctl.Depth = new Depth(depth.WindowLayer, ctl.Depth.ComponentLayer, ctl.Depth.ZOrder);
//            }
//        }

//        public override bool CanRaiseEvents
//        {
//            get
//            {
//                return base.CanRaiseEvents;
//            }
//            set
//            {
//                base.CanRaiseEvents = value;
//                foreach (BaseControl ctl in PreOrderVisible)
//                    ctl.CanRaiseEvents = false;
//            }
//        }


//        /// <summary>
//        /// Returns the internal collection of subcomponent controls
//        /// and child controls.
//        /// </summary>
//        internal ControlCollection InternalControls
//        {
//            get { return internalControls; }
//        }

//        /// <summary>
//        /// Returns the publicly available collection of child controls.
//        /// </summary>
//        public virtual ControlCollection Controls
//        {
//            get { return controls; }
//        }

//        public IEnumerable<BaseControl> PreOrder
//        {
//            get { return PreOrderVisit(this); }
//        }

//        public IEnumerable<BaseControl> PreOrderVisible
//        {
//            get { return PreOrderVisibleVisit(this); }
//        }

//        public IEnumerable<BaseControl> PostOrderVisible
//        {
//            get { return PostOrderVisibleVisit(this); }
//        }

//        #endregion

//        public ContainerControl(string id, Vector2 location, Size size) :
//            this(id, location, size, Shape.None, BorderStyle.None, Shapes.ShadeNone, ColorArray.Transparent)
//        {
//        }

//        public ContainerControl(string id, Vector2 location, Size size,
//                                Shape shape, BorderStyle style, Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, location, size, shape, style, shadeMode, colorArray)
//        {
//            controls = new ControlCollection(this);
//            internalControls = new ControlCollection(this);

//            isFocusable = false;
//            applyStatusChanges = false;
//        }

//        #region Visit Algorithms

//        IEnumerable<BaseControl> PreOrderVisit(ContainerControl container)
//        {
//            foreach (BaseControl ctl in container.internalControls)
//            {
//                yield return ctl;
//                if (ctl is IContainer)
//                {
//                    foreach (BaseControl ctlChild in PreOrderVisit(ctl as ContainerControl))
//                        yield return ctlChild;
//                }

//                else continue;
//            }
//        }

//        IEnumerable<BaseControl> PreOrderVisibleVisit(ContainerControl container)
//        {
//            foreach (BaseControl ctl in container.internalControls)
//            {
//                if (ctl.IsVisible)
//                {
//                    yield return ctl;
//                    if (ctl is IContainer)
//                    {
//                        foreach (BaseControl ctlChild in PreOrderVisibleVisit(ctl as ContainerControl))
//                        {
//                            if (!ctlChild.IsVisible)
//                                continue;
//                            yield return ctlChild;
//                        }
//                    }
//                }
//                else continue;
//            }
//        }

//        IEnumerable<BaseControl> PostOrderVisibleVisit(ContainerControl container)
//        {
//            foreach (BaseControl ctl in container.internalControls)
//            {
//                if (ctl.IsVisible)
//                {
//                    if (ctl is IContainer)
//                    {
//                        foreach (BaseControl ctlChild in PostOrderVisibleVisit(ctl as ContainerControl))
//                            yield return ctlChild;
//                    }
//                    yield return ctl;
//                }
//                else continue;
//            }
//        }

//        #endregion


//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            if (internalControls.Count != 0)
//                foreach (BaseControl ctl in PostOrderVisible)
//                {
//                    ctl.ComputeAbsolutePosition();
//                    if (!UI.CurrentHud.DesignState && ctl.IsVisible && ctl is RenderableControl)
//                        (ctl as RenderableControl).Update();
//                }
//        }

//        #region IContainer Members

//        public virtual void Add(List<BaseControl> controlList)
//        {
//            foreach (BaseControl ctl in controlList)
//                Add(ctl);
//        }

//        public virtual void Add(BaseControl ctl)
//        {
//            bool isWindow = ctl is Window;
//            if (isWindow && !(this is HUD))
//                throw new ArgumentException("You can only add windows to the HUD.");

//            ctl.Parent = this;
//            ctl.Index = controls.Count;


//            ctl.Depth = new Depth(
//                (isWindow) ? UI.CurrentHud.WindowManager.RegisterWindow(ctl as Window) : depth.WindowLayer,
//                ctl.Depth.ComponentLayer,
//                depth.ZOrder + 1);

//            if (ctl is IContainer)
//            {
//                foreach (BaseControl childControl in (ctl as IContainer).PreOrderVisible)
//                {
//                    childControl.Depth = new Depth(
//                        ctl.Depth.WindowLayer,
//                        ctl.Depth.ComponentLayer,
//                        ctl.Depth.ZOrder + childControl.Depth.ZOrder);
//                }
//            }
//            try
//            {
//                internalControls.Add(ctl);

//                if (!ctl.IsSubComponent)
//                    controls.Add(ctl);
//            }
//            catch (ArgumentException ex)
//            {
//                DebugManager.LogError("ContainerControl.Add", ex.Message, ctl.ID);
//            }
//        }

//        public bool Remove(BaseControl ctl)
//        {
//            bool value;

//            value = internalControls.Remove(ctl) && controls.Remove(ctl);

//            if (this is HUD && ctl is Window)
//                (this as HUD).WindowManager.Remove(ctl as Window);

//            ctl.CanRaiseEvents = false;
//            ctl.IsDisposing = true;

//            if (ctl is IContainer)
//                foreach (BaseControl control in (ctl as IContainer).PreOrder)
//                {
//                    control.CanRaiseEvents = false;
//                    control.IsDisposing = true;
//                }


//            return value;
//        }

//        public BaseControl Find(string id)
//        {
//            foreach (BaseControl ctl in PreOrderVisible)
//            {
//                if (ctl.ID == id)
//                    return ctl;
//                else
//                    continue;
//            }
//            throw new ArgumentException();
//        }

//        #endregion
//    }
//}