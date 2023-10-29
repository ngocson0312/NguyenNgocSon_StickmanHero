using System;
using UnityEngine;
using UnityEngine.UI;
public class ImageChange : MonoBehaviour
{
	public Sprite[] listSprite;

	public Action<int> changeCallBack;

	private SpriteRenderer mSpriteRenderer;

	private Image tmp;

	private void Awake()
	{
		if (tmp == null) tmp = GetComponent<Image>();
		if(mSpriteRenderer == null) mSpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void ChangeImage(int index)
	{
		if (mSpriteRenderer != null)
		{
			if (index >= listSprite.Length) index = listSprite.Length - 1;
			mSpriteRenderer.sprite = listSprite[index];
			changeCallBack?.Invoke(index);
		}
	}

	public void SetDefautColor()
	{
		mSpriteRenderer.color = Color.white;
		if (transform.childCount >= 1)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}

	public void ChangeAll(int index, bool UI = false)
	{
		if (!UI)
		{
			ChangeImage(index);
			if (transform == null)
			{
				return;
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				ImageChange component = transform.GetChild(i).gameObject.GetComponent<ImageChange>();
				if (component != null)
				{
					component.ChangeImage(index);
				}
			}
			return;
		}
		ChangeImageUI(index);
		for (int j = 0; j < transform.childCount; j++)
		{
			ImageChange component2 = transform.GetChild(j).gameObject.GetComponent<ImageChange>();
			if (component2 != null)
			{
				component2.ChangeImageUI(index);
			}
		}
	}

	public void SetColorAll(Color color)
	{
		mSpriteRenderer.color = color;
		if (transform.childCount >= 1)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = color;
			}
		}
	}

	public void Fade()
	{
		FadeIn();
	}

	private void FadeIn()
	{
		ChangeImage(1);
		Invoke(nameof(FadeOut), 0.3f);
	}

	private void FadeOut()
	{
		ChangeImage(0);
		Invoke(nameof(FadeIn), 0.3f);
	}

	public void StopFade()
	{
		CancelInvoke();
	}

	public void ChangeImageUI(int ind, bool navetiveSize = false)
	{
		if (tmp == null)
		{
			tmp = GetComponent<Image>();
			if(tmp == null)
				return;
		}
		if (listSprite[ind] != null)
		{
			tmp.enabled = true;
			tmp.sprite = listSprite[ind];
			changeCallBack?.Invoke(ind);
			if (navetiveSize)
			{
				tmp.SetNativeSize();
			}
		}
		else
		{
			tmp.enabled = false;
		}
	}
}
