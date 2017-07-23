using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class GameControllerScript : MonoBehaviour {

	public GameObject block;
	public GameObject payload;
	public GameObject goal;
	public GameObject player;

	public int stage_no = 1;

	public GameObject clear_msg;

	public Dropdown stage_select;

	public enum maptype { SPACE, BLOCK, PAYLOAD, GOAL, PLAYER };

	private class Stage {
		public maptype[][] m_Map;
		public int m_Cols;
		public int m_Rows;

		public Stage() {
			m_Cols = 0;
			m_Rows = 0;
		}

		public string stage_filename(int stage_no)
		{
			string filename = "screens/screen" + stage_no.ToString ("D2");

			return (filename);
		}

		public bool load_stage(string file)
		{
			print ("open " + file);
			//FileInfo fi = new FileInfo (file);
			TextAsset text = Resources.Load(file) as TextAsset;
			try {
				using (var sr = new StringReader(text.text)) {
					string line;
					m_Rows = 0;
					m_Cols = 0;
					var map_rows = new List<maptype[]>();
					while ((line = sr.ReadLine()) != null) {
						maptype[] map_row = new maptype[line.Length];
						if (line.Length > m_Cols) {
							m_Cols = line.Length;
						}
						print(line);
						for (int col = 0; col < line.Length; col ++) {
							switch (line[col]) {
							case '@': // Player
								map_row[col] = maptype.PLAYER;
								break;
							case '#': // Wall
								map_row[col] = maptype.BLOCK;
								break;
							case '$': // Payload
								map_row[col] = maptype.PAYLOAD;
								break;
							case '.': // Goal
								map_row[col] = maptype.GOAL;
								break;
							default:
								map_row[col] = maptype.SPACE;
								break;
							}
						}
						map_rows.Add(map_row);
						m_Rows ++;
					}
					print("this map size is (" + m_Cols.ToString() + "x" + m_Rows.ToString() + ")");
					m_Map = map_rows.ToArray();
				}
			} catch (Exception ex) {
				Resources.UnloadAsset (text);
				return (false);
			}
			Resources.UnloadAsset (text);
			return (true);
		}
	}

	private GameObject[] set_stage(Stage stage)
	{
		m_AllStageObjs = new List<GameObject> ();
		GameObject obj;

		var payload_list = new List<GameObject> ();
		int x_ofs = 12 - (25 - stage.m_Cols) / 2;
		int y_ofs = 12 - (25 - stage.m_Rows) / 2;
		for (int y = 0; y < stage.m_Rows; y++) {
			for (int x = 0; x < stage.m_Map[y].Length; x++) {
				switch (stage.m_Map[y][x]) {
				case maptype.BLOCK:
					obj = Instantiate (block, new Vector3 ((x - x_ofs), 0, (y_ofs - y)), transform.rotation);
					m_AllStageObjs.Add (obj);
					break;
				case maptype.PAYLOAD:
					obj = Instantiate (payload, new Vector3 ((x - x_ofs), 0, (y_ofs - y)), transform.rotation);
					payload_list.Add (obj);
					m_AllStageObjs.Add (obj);
					break;
				case maptype.GOAL:
					obj = Instantiate (goal, new Vector3 ((x - x_ofs), 0.5f, (y_ofs - y)), transform.rotation);
					m_AllStageObjs.Add (obj);
					break;
				case maptype.PLAYER:
					player.transform.SetPositionAndRotation (new Vector3 ((x - x_ofs), 0, (y_ofs - y)), transform.rotation);
					break;
				}
			}
		}
		return (payload_list.ToArray ());
	}

	private void clear_stage()
	{
		foreach (var obj in m_AllStageObjs) {
			Destroy (obj);
		}
		m_AllStageObjs = null;
	}

	private Stage m_CurStage;
	private GameObject[] m_Payloads;
	private List<GameObject> m_AllStageObjs;
	private float m_StageClearTime;

	Stage InitStage()
	{
		m_StageClearTime = 0;
		clear_msg.SetActive (false);
		Stage stage = new Stage ();
		if (stage.load_stage (stage.stage_filename (stage_no))) {
			m_Payloads = set_stage (stage);
		} else {
			return (null);
		}
		return (stage);
	}

	// Use this for initialization
	void Start () {
		m_CurStage = InitStage ();
		if (m_Payloads != null) {
			print (m_Payloads.Length.ToString () + " payloads");
		}

		var list = new List<Dropdown.OptionData> ();
		for (int cnt = 0; cnt < 90; cnt++) {
			list.Add (new Dropdown.OptionData ("Stage " + (cnt + 1).ToString ()));
		}
		stage_select.AddOptions (list);
		stage_select.onValueChanged.AddListener (OnStageSelected);
	}

	void OnStageSelected(int _stage_no)
	{
		stage_no = _stage_no + 1;
		clear_stage ();
		m_CurStage = InitStage ();
	}

	// Update is called once per frame
	void Update () {
		int idx;
		for (idx = 0; idx < m_Payloads.Length; idx++) {
			if (!m_Payloads [idx].CompareTag ("OnGoal")) {
				break;
			}
		}
		if (idx == m_Payloads.Length) {
			if (m_StageClearTime == 0) {
				m_StageClearTime = 1;
			} else if (m_StageClearTime > 0) {
				m_StageClearTime -= Time.deltaTime;
				if (m_StageClearTime < 0) {
					for (idx = 0; idx < m_Payloads.Length; idx++) {
						m_Payloads [idx].GetComponent<Rigidbody> ().isKinematic = true;
					}
					clear_msg.SetActive (true);
					print ("CLEARED!!");
				}
			}
		}
	}
}
