using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{
    private static string nl = Environment.NewLine;
    private static string[] fig = { "Pacman", "Blinky", "Inky", "Pinky", "Clyde" };
    private static List<Vector3> m_figpoints = new List<Vector3>();


    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }


    // Statemachine
    //
    //  +--------------+     +--------------+
    //  | Level Syntax |  t  | Figure & Pos |  t
    //  | Check        | --> | Check        | --> valid level
    //  +--------------+     +--------------+
    //    |                     |
    //    | false               | false
    //    v                     v
    //  +-----------------------------------+
    //  | not valid                         |
    //  +-----------------------------------+



    public static bool IsValidLevel(string path, out string level, out string figures, ref Text debug)
    {
        level = string.Empty;
        figures = string.Empty;

        string[] file = File.ReadAllLines(path);
        int dim1 = file[0].Length, dim2 = file.Length - (1 + 4);
        debug.text += "size: " + dim1 + " x " + dim2 + nl + nl;

        if (file.Length < 1 + 4)
        {
            debug.text += " ERR: lines: " + file.Length + nl;
            debug.text += "   -> fewer lines than excepted";
            return false;
        }

        debug.text += ("-- level parsing ").PadRight(dim1 + 7 + 3 + 7, '-') + nl;
        StringBuilder sb = new StringBuilder();
        using (StreamReader sr = new StreamReader(path))
        {
            int lix = 0;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                debug.text += " l." + (++lix).ToString("00") + ": " + line.PadRight(dim1 + 3);

                if (line.StartsWith("Pac"))
                {
                    debug.text += nl + " INF: first char is letter" + nl + "   -> try to parse figures next" + nl;
                    break;
                }

                sb.AppendLine(line);
                foreach (char c in line)
                    if (!(c == '+' || c == '-' || c == '|'))
                    {
                        debug.text += nl + " ERR: char not valid: " + c + nl + "   -> syntax error!" + nl;
                        return false;
                    }

                debug.text += "> valid" + nl;
            }
            level = sb.ToString().Trim();
            debug.text += ("-- level parsed ").PadRight(dim1 + 7 + 3 + 7, '-') + "" + nl;

            debug.text += ("-- figure parsing ").PadRight(dim1 + 7 + 3 + 7, '-') + nl;
            sb = new StringBuilder();
            int ix = 0, posx, posy;
            m_figpoints = new List<Vector3>();
            do
            {
                sb.AppendLine(line);
                debug.text += " l." + (lix++).ToString("00") + ": " + line.PadRight(dim1 + 3);

                string[] arr = line.Split(' ');
                if (arr[0] != fig[ix++])
                {
                    debug.text += nl + " ERR: wrong figur: " + arr[0] + nl + "  -> syntax error!" + nl;
                    return false;
                }

                if (!int.TryParse(arr[1], out posx) || !int.TryParse(arr[2], out posy))
                {
                    debug.text += nl + " ERR: non-parseble position for " + arr[0] + nl + "   -> syntax error!" + nl;
                    return false;
                }

                if (posx > dim1 - 1 || posy > dim2 - 1)
                {
                    debug.text += nl + " ERR: position out of bounds for " + arr[0] + nl + "   -> semantic error!" + nl;
                    return false;
                }
                else
                    m_figpoints.Add(new Vector3(posx, 0, -posy));

                debug.text += "> valid" + nl;
            } while ((line = sr.ReadLine()) != null);
            figures = sb.ToString().Trim();
            debug.text += ("-- figures parsed ").PadRight(dim1 + 7 + 3 + 7, '-') + nl;
        }

        return true;
    }

    public static Tile[,] ParseValidLevel(string level, ref Text debug)
    {
        Tile[,] tiles;
        using (StringReader sr = new StringReader(level))
        {
            string line = sr.ReadLine();
            tiles = new Tile[level.Split('\n').Length, line.Length];

            int row = 0; int col = 0;
            do
            {
                col = 0;
                foreach (char c in line)
                {
                    Tile t = new Tile();
                    t.Type = c == '+' ? Tile.Element.Cross : Tile.Element.Corridor;
                    //t.Rotation = c == '|' ? 90f : 0f;
                    t.ltrb_open = c == '-' ? new bool[] { true, false, true, false }
                                : c == '|' ? new bool[] { false, true, false, true }
                                : new bool[] { true, true, true, true };

                    tiles[row, col] = t;
                    ++col;
                }
                ++row;
            } while ((line = sr.ReadLine()) != null);
        }

        for (int row = 0; row < tiles.GetLength(0); row++)
            for (int col = 0; col < tiles.GetLength(1); col++)
            {
                Tile cur = tiles[row, col];

                bool[] neighbours = new bool[4];
                neighbours[0] = col == 0 ? false : (tiles[row, col - 1].IsOpenInDirection(Tile.RIGHT) && cur.IsOpenInDirection(Tile.LEFT));
                neighbours[1] = row == 0 ? false : (tiles[row - 1, col].IsOpenInDirection(Tile.BOTTOM) && cur.IsOpenInDirection(Tile.TOP));
                neighbours[2] = col == tiles.GetLength(1) - 1 ? false : (tiles[row, col + 1].IsOpenInDirection(Tile.LEFT) && cur.IsOpenInDirection(Tile.RIGHT));
                neighbours[3] = row == tiles.GetLength(0) - 1 ? false : (tiles[row + 1, col].IsOpenInDirection(Tile.TOP) && cur.IsOpenInDirection(Tile.BOTTOM));

                tiles[row, col] = new Tile(neighbours);
            }

        //debug.text += nl + nl;

        //for (int r = 0; r < tiles.GetLength(0); r++)
        //{
        //    for (int c = 0; c < tiles.GetLength(1); c++)
        //        debug.text += "[" + tiles[r, c].ToString() + "]";


        //    debug.text += nl;
        //}

        return tiles;
    }

    /**
	 * Extracts the chracter positons from the given string and returns them
	 **/
    public static int[] getCharacterPositions(string level)
    {
        int[] res = new int[10];
        int inx = 0;
        string[] arr = level.Split('\n');
        for (int i = 0; i < arr.Length; i++)
        {
            string[] line = arr[i].Split(' ');
            res[inx++] = int.Parse(line[2]);
            res[inx++] = int.Parse(line[1]);
        }
        return res;
    }

    public static List<Vector3> GetCharPositions()
    {
        return m_figpoints;
    }
}
