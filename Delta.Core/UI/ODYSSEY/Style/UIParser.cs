//namespace AvengersUTD.Odyssey.UserInterface.Style
//{
//    /// <summary>
//    /// Old code: will be used in the future to allow skinning of the User Interface.
//    /// </summary>
//    public class UIParser
//    {
//        //    private const string sGUI = "GUI";
//        //    private const string sLocation = "Location";

//        //    private GUIParser()
//        //    {
//        //    }

//        //    public static HUD[] Read(string xmlFile)
//        //    {

//        //        XmlDocument xDoc = new XmlDocument();
//        //        xDoc.Load(xmlFile);

//        //        return Parse(xDoc.GetElementsByTagName(sGUI));
//        //    }

//        //    private static HUD[] Parse(XmlNodeList nodes)
//        //    {
//        //        ArrayList hudList = new ArrayList();
//        //        HUD[] hudArray;

//        //        foreach (XmlNode nGui in nodes)
//        //        {
//        //            foreach (XmlNode nHud in nGui.ChildNodes)
//        //            {
//        //                HUD hud = new HUD(nHud.Attributes[0].InnerText);

//        //                // FirstChild is Controls
//        //                foreach (XmlNode nCtl in nHud.FirstChild.ChildNodes)
//        //                {
//        //                    Control ctl = ParseControl(nCtl);
//        //                    hud.Add(ctl);

//        //                    // If 'controls' child node is not null then it must be a panel
//        //                    if (nCtl.ChildNodes[2] == null)
//        //                        continue;
//        //                    else 
//        //                    {
//        //                        Panel panel = (Panel) ctl;
//        //                        foreach (XmlNode nChild in nCtl.ChildNodes[2].ChildNodes)
//        //                            panel.Add(ParseControl(nChild));
//        //                    }
//        //                }

//        //                hudList.Add(hud);
//        //            }
//        //        }

//        //        hudArray = new HUD[hudList.Count];
//        //        hudList.CopyTo(hudArray);

//        //        return hudArray;
//        //    }

//        //    private static Rectangle ParseArea(XmlNode nArea)
//        //    {
//        //        Point pRelative = new Point(int.Parse(nArea.Attributes[0].InnerText),
//        //            int.Parse(nArea.Attributes[1].InnerText)
//        //            );
//        //        Size size = new Size(int.Parse(nArea.Attributes[2].InnerText), 
//        //            int.Parse(nArea.Attributes[3].InnerText));

//        //        return new Rectangle(pRelative, size);
//        //    }

//        //    private static Control ParseControl(XmlNode nCtl)
//        //    {
//        //        Texture texture;
//        //        Rectangle enabledArea, highlightedArea, relativeLocation;
//        //        XmlNode nTex;
//        //        XmlNode nLoc = nCtl.ChildNodes[0]; 

//        //        relativeLocation = ParseArea(nLoc);

//        //        // Control ID
//        //        string id = nCtl.Attributes[0].InnerText;

//        //        switch (nCtl.Name)
//        //        {
//        //            case Panel.Type:
//        //                nTex = nCtl.ChildNodes[1];
//        //                texture = TextureLoader.FromFile(Game.Device, nTex.Attributes[0].InnerText);
//        //                enabledArea = ParseArea(nTex.ChildNodes[0]);

//        //                Panel panel = new Panel(id, texture, relativeLocation, enabledArea);
//        //                return panel;

//        //            case PictureBox.Type:
//        //                nTex = nCtl.ChildNodes[1];
//        //                texture = TextureLoader.FromFile(Game.Device, nTex.Attributes[0].InnerText);
//        //                enabledArea = ParseArea(nTex.ChildNodes[0]);

//        //                return new PictureBox(id, texture, relativeLocation, enabledArea);

//        //            case Label.Type:
//        //                return new Label(id,
//        //                    nCtl.Attributes[1].InnerText,
//        //                    UI.ParseAlignment(nCtl.Attributes[2].InnerText),
//        //                    relativeLocation);

//        //            case Button.Type:
//        //                nTex = nCtl.ChildNodes[1];
//        //                texture = TextureLoader.FromFile(Game.Device, nTex.Attributes[0].InnerText);
//        //                enabledArea = ParseArea(nTex.ChildNodes[0]);
//        //                highlightedArea = ParseArea(nTex.ChildNodes[1]);

//        //                return new Button(id, nCtl.Attributes[1].InnerText, texture,
//        //                    relativeLocation, enabledArea, highlightedArea);

//        //            default:
//        //                return null;
//        //        }
//    }
//}