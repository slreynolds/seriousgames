using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour
{
    public InputField m_input;
    public Text m_text;
    public GameObject Block, Corner, Corridor, Cross, DeadEnd, TCross;
    public GameObject Pacman, Blinky, Clyde, Inky, Pinky;

    private List<GameObject> m_objects;
    private static Quaternion qempty = new Quaternion(0, 0, 0, 0);


    // Use this for initialization
    void Start()
    {
        m_text.text = "game started" + Environment.NewLine;
        m_objects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ParseLevel()
    {
        foreach (var go in m_objects)
            Destroy(go);

        m_objects = new List<GameObject>();
        m_text.text = string.Empty;

        string path = m_input.text;
        if (!path.EndsWith(".pac")) path = path + ".pac";
        m_input.text = path;

        path = Path.Combine(@"Assets\levels", path);

        if (!File.Exists(path))
        {
            m_text.text += "file not available!" + Environment.NewLine;
            return;
        }

        Tile[,] tiles = new Tile[0, 0];
        //int[] characterPositions = new int[10];
        List<Vector3> charpos;

        m_text.text += "parsing started:" + Environment.NewLine;
        string level, figures;
        if (Parser.IsValidLevel(path, out level, out figures, ref m_text))
        {
            m_text.text += Environment.NewLine + "start building" + Environment.NewLine;
            tiles = Parser.ParseValidLevel(level, ref m_text);

            //characterPositions = Parser.getCharacterPositions(figures);

            charpos = Parser.GetCharPositions();
            m_text.text += " got positions" + Environment.NewLine;
          
            m_text.text += level + Environment.NewLine;
        }
        else
        {
            m_input.text = "level structure not valid" + Environment.NewLine;
            return;
        }

        GameObject[,] objs = new GameObject[tiles.GetLength(0), tiles.GetLength(1)];


        // build prefabs
        for (int r = 0; r < tiles.GetLength(0); r++)
            for (int c = 0; c < tiles.GetLength(1); c++)
            {
                Vector3 pos = new Vector3(c, 0f, -r);
                Quaternion rot = Quaternion.Euler(0f, tiles[r, c].Rotation, 0f);

                GameObject go = null;
                #region switch
                switch (tiles[r, c].Type)
                {
                    case Tile.Element.Block:
                        go = Block;
                        break;
                    case Tile.Element.Corner:
                        go = Corner;
                        break;
                    case Tile.Element.Corridor:
                        go = Corridor;
                        break;
                    case Tile.Element.Cross:
                        go = Cross;
                        break;
                    case Tile.Element.Deadend:
                        go = DeadEnd;
                        break;
                    case Tile.Element.TCross:
                        go = TCross;
                        break;
                    default:
                        break;
                }
                #endregion

                var inst = Instantiate(go, pos, rot);
                m_objects.Add(inst);
                objs[r, c] = inst;
            }

        m_text.text += "level building finished w/success" + Environment.NewLine;

        for (int r = 0; r < tiles.GetLength(0); r++)
        {
            for (int c = 0; c < tiles.GetLength(1); c++)
            {

                if (tiles[r, c].ltrb_open[0])
                    objs[r, c].GetComponent<WayPoint>().leftWaypoint = objs[r, c - 1].GetComponent<WayPoint>();

                if (tiles[r, c].ltrb_open[1])
                    objs[r, c].GetComponent<WayPoint>().upWaypoint = objs[r - 1, c].GetComponent<WayPoint>();

                if (tiles[r, c].ltrb_open[2])
                    objs[r, c].GetComponent<WayPoint>().rightWaypoint = objs[r, c + 1].GetComponent<WayPoint>();

                if (tiles[r, c].ltrb_open[3])
                    objs[r, c].GetComponent<WayPoint>().downWaypoint = objs[r + 1, c].GetComponent<WayPoint>();
            }
        }
        m_text.text += "added waypoints to GameObjects" + Environment.NewLine;

		for (int i = 0; i < charpos.Count; i++) {
			
			m_text.text += "  char pos fig " + (i + 1) + ": " + charpos [i].x + " " + charpos [i].y + " " + charpos [i].z + objs[(int)charpos[0].x, -(int)charpos[0].z].GetComponent<WayPoint>() + Environment.NewLine;

		}
		m_text.text += "" + objs.GetLength(0) + " - " + objs.GetLength(1) + " / " + objs.Length;

		transform.position = new Vector3((tiles.GetLength(1) - 1) / 2f, 10, -(tiles.GetLength(0) - 1) / 2f);

        Pacman = Instantiate(Pacman, charpos[0], qempty);
        Blinky = Instantiate(Blinky, charpos[1], qempty);
        Inky = Instantiate(Inky, charpos[2], qempty);
        Pinky = Instantiate(Pinky, charpos[3], qempty);
        Clyde = Instantiate(Clyde, charpos[4], qempty);

		Pacman.transform.position = charpos[0];
		Blinky.transform.position = charpos[1];
		Inky.transform.position = charpos[2];
		Pinky.transform.position = charpos[3];
		Clyde.transform.position = charpos[4];

        m_objects.Add(Pacman);
        m_objects.Add(Blinky);
        m_objects.Add(Inky);
        m_objects.Add(Pinky);
        m_objects.Add(Clyde);

        //Pacman = Instantiate(Pacman, new Vector3(characterPositions[0], 0, -characterPositions[1]),  new Quaternion(0, 0, 0, 0));
        //Blinky = Instantiate(Blinky, new Vector3(characterPositions[2], 0, -characterPositions[3]), new Quaternion(0, 0, 0, 0));
        //Inky = Instantiate(Inky, new Vector3(characterPositions[4], 0, -characterPositions[5]), new Quaternion(0, 0, 0, 0));
        //Pinky = Instantiate(Pinky, new Vector3(characterPositions[6], 0, -characterPositions[7]), new Quaternion(0, 0, 0, 0));
        //Clyde = Instantiate(Clyde, new Vector3(characterPositions[8], 0, -characterPositions[9]), new Quaternion(0, 0, 0, 0));


        //Pacman.GetComponent<PlayerControlScript>().currentWaypoint = m_obj[characterPositions[0], characterPositions[1]].GetComponent<WayPoint>();
        //Blinky.GetComponent<EnemyBehaviourScript>().currentWaypoint = m_obj[characterPositions[2], characterPositions[3]].GetComponent<WayPoint>();
        //Inky.GetComponent<EnemyBehaviourScript>().currentWaypoint = m_obj[characterPositions[4], characterPositions[5]].GetComponent<WayPoint>();
        //Pinky.GetComponent<EnemyBehaviourScript>().currentWaypoint = m_obj[characterPositions[6], characterPositions[7]].GetComponent<WayPoint>();
        //Clyde.GetComponent<EnemyBehaviourScript>().currentWaypoint = m_obj[characterPositions[8], characterPositions[9]].GetComponent<WayPoint>();

		Pacman.GetComponent<PlayerControlScript>().currentWaypoint = objs[-(int)charpos[0].z, (int)charpos[0].x].GetComponent<WayPoint>();
		Blinky.GetComponent<EnemyBehaviourScript>().currentWaypoint = objs[-(int)charpos[1].z, (int)charpos[1].x].GetComponent<WayPoint>();
		Inky.GetComponent<EnemyBehaviourScript>().currentWaypoint = objs[-(int)charpos[2].z, (int)charpos[2].x].GetComponent<WayPoint>();
		Pinky.GetComponent<EnemyBehaviourScript>().currentWaypoint = objs[-(int)charpos[3].z, (int)charpos[3].x].GetComponent<WayPoint>();
		Clyde.GetComponent<EnemyBehaviourScript>().currentWaypoint = objs[-(int)charpos[4].z, (int)charpos[4].x].GetComponent<WayPoint>();


        m_text.text += "instanziated characters" + Environment.NewLine;

    }
}
