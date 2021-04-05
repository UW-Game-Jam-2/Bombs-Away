using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{

	public Image img;
	public AnimationCurve curve;
	public float fadeTime;

	void Start()
	{
		StartCoroutine(FadeIn());
	}

	public void FadeTo(string scene)
	{
		StartCoroutine(FadeOut(scene));
	}

	private IEnumerator FadeIn()
	{
		float t = fadeTime;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
		}
	}

	private IEnumerator FadeOut(string scene)
	{
		float t = -fadeTime;

		while (t < 0f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
		}

		SceneManager.LoadScene(scene);
	}

}