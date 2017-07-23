using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {

	private Animator anim;
	private int turn_state = 0;
	private bool is_running = false;

	private enum dir { UP, LEFT, DOWN, RIGHT };
	private dir m_Dir;
	private int m_Turn;
	private int m_Move;

    // Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		m_Dir = dir.UP;
		m_Turn = 0;
		m_Move = 0;
	}

	float GetAngleFromDir(dir _dir)
	{
		switch (_dir) {
		case dir.UP:
			return (0);
		case dir.RIGHT:
			return (90);
		case dir.DOWN:
			return (180);
		case dir.LEFT:
			return (270);
		}
		return (0);
	}

	bool MoveLeft()
	{
		switch (m_Dir) {
		case dir.UP:
			m_Turn = 90;
			break;
		case dir.LEFT:
			m_Turn = 0;
			break;
		case dir.DOWN:
			m_Turn = -90;
			break;
		case dir.RIGHT:
			m_Turn = 180;
			break;
		}
		m_Dir = dir.LEFT;
		m_Move = 1;

		return (true);
	}

	bool MoveRight()
	{
		switch (m_Dir) {
		case dir.UP:
			m_Turn = -90;
			break;
		case dir.LEFT:
			m_Turn = 180;
			break;
		case dir.DOWN:
			m_Turn = 90;
			break;
		case dir.RIGHT:
			m_Turn = 0;
			break;
		}
		m_Dir = dir.RIGHT;
		m_Move = 1;

		return (true);
	}

	bool MoveUp()
	{
		switch (m_Dir) {
		case dir.UP:
			m_Turn = 0;
			break;
		case dir.LEFT:
			m_Turn = -90;
			break;
		case dir.DOWN:
			m_Turn = 180;
			break;
		case dir.RIGHT:
			m_Turn = 90;
			break;
		}
		m_Dir = dir.UP;
		m_Move = 1;

		return (true);
	}

	bool MoveDown()
	{
		switch (m_Dir) {
		case dir.UP:
			m_Turn = 180;
			break;
		case dir.LEFT:
			m_Turn = 90;
			break;
		case dir.DOWN:
			m_Turn = 0;
			break;
		case dir.RIGHT:
			m_Turn = -90;
			break;
		}
		m_Dir = dir.DOWN;
		m_Move = 1;

		return (true);
	}

	// Update is called once per frame
	void Update () {
		if (m_Move == 0 && m_Turn == 0) {
			if (Input.GetKey ("up")) {
				MoveUp ();
			} else if (Input.GetKey ("down")) {
				MoveDown ();
			} else if (Input.GetKey ("right")) {
				MoveRight ();
			} else if (Input.GetKey ("left")) {
				MoveLeft ();
			}
		} else if (m_Turn != 0) {
			if (m_Turn < 0) {
				m_Turn += 15;
				if (m_Turn == 0) {
					transform.Rotate (0, GetAngleFromDir(m_Dir) - transform.eulerAngles.y, 0);
				} else {
					transform.Rotate (0, 15, 0);
				}
			} else {
				m_Turn -= 15;
				if (m_Turn == 0) {
					transform.Rotate (0, GetAngleFromDir(m_Dir) - transform.eulerAngles.y, 0);
				} else {
					transform.Rotate (0, -15, 0);
				}
			}
		} else if (m_Move != 0) {
			if (!is_running) {
				is_running = true;
				anim.SetBool ("is_running", true);
				turn_state = 10;
			}
			transform.position += transform.forward * 0.1f;
			turn_state--;

			if (turn_state == 0) {
				m_Move--;
				is_running = false;
				anim.SetBool ("is_running", false);
			}
		}
	}
}
