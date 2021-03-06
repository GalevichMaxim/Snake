﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// изображение экрана при столкновении игрока с фатальными препятствиями
public class DamageScreen : MonoBehaviour {

	public Text text;
	public Image image;
	public float flashSpeed = 2.5f;

	private PlayerController playerController;
	private Color flashColor;
	private int lastHealth;

	void Awake()
	{
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		flashColor = new Color (1f, 0f, 0f, 1f);
		lastHealth = playerController.health;
	}

	void Update () 
	{
		// если произошло фатальное столкновение
		if (playerController.damage && lastHealth != playerController.health )
		{
			// становится видимым изображение символа жизней и их количества
			image.color = flashColor;
			text.text = playerController.health.ToString();
			text.color = flashColor;
			lastHealth = playerController.health;
		} 
		else 
		{
			image.color = Color.Lerp (image.color, Color.clear, flashSpeed * Time.deltaTime);
			text.color = Color.Lerp (text.color, Color.clear, flashSpeed * Time.deltaTime);
		}
	}
}
