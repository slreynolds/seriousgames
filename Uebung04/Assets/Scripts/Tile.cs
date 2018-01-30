using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    public class Tile
    {
        public static int LEFT = 0, TOP = 1, RIGHT = 2, BOTTOM = 3;

        public enum Element { Block, Corner, Corridor, Cross, Deadend, TCross };


        public Element Type;
        public float Rotation;
        public bool[] ltrb_open;

        public Tile()
        {
            Type = Element.Corner;
            Rotation = 0f;
            ltrb_open = new bool[] { true, true, true, true };
        }

        public Tile(bool[] neighbours)
        {
            ltrb_open = neighbours;

            #region left = true
            if (neighbours[LEFT])
            {
                #region top = true
                if (neighbours[TOP])
                {
                    #region right = true
                    if (neighbours[RIGHT])
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.Cross, 0f);
                        else
                            SetTypeAndRotation(Element.TCross, 270f);
                    }
                    #endregion
                    #region right = false
                    else
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.TCross, 180f);
                        else
                            SetTypeAndRotation(Element.Corner, 90f);
                    }
                    #endregion
                }
                #endregion
                #region top = false
                else
                {
                    #region right = true
                    if (neighbours[RIGHT])
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.TCross, 90f);
                        else
                            SetTypeAndRotation(Element.Corridor, 90f); ////////////////////////////////////////
                    }
                    #endregion
                    #region right = false
                    else
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.Corner, 0);
                        else
                            SetTypeAndRotation(Element.Deadend, 90f);
                    }
                    #endregion
                }
            }
            #endregion
            #endregion
            #region left = false
            else
            {
                #region top = true
                if (neighbours[TOP])
                {
                    #region right = true
                    if (neighbours[RIGHT])
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.TCross, 0f);
                        else
                            SetTypeAndRotation(Element.Corner, 180f);
                    }
                    #endregion
                    #region right = false
                    else
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.Corridor, 0f); ///////////////////////////////////////
                        else
                            SetTypeAndRotation(Element.Deadend, 180f);
                    }
                    #endregion
                }
                #endregion
                #region top = false
                else
                {
                    #region right = true
                    if (neighbours[RIGHT])
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.Corner, 270f);
                        else
                            SetTypeAndRotation(Element.Deadend, 270f);
                    }
                    #endregion
                    #region right = false
                    else
                    {
                        if (neighbours[BOTTOM])
                            SetTypeAndRotation(Element.Deadend, 0f);
                        else
                            SetTypeAndRotation(Element.Block, 0f);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

        }

        private void SetTypeAndRotation(Element type, float rotation)
        {
            Type = type;
            Rotation = rotation;
        }

        public bool IsOpenInDirection(int dir)
        {
            return ltrb_open[dir];
        }

        public override string ToString()
        {
            return Type.ToString().Substring(0, 4).ToUpper()
                + "|" + Rotation.ToString("000")
                + "|" + string.Join("", ltrb_open.Select(x => x.ToString().Substring(0, 1)).ToArray());
        }
    }
}
