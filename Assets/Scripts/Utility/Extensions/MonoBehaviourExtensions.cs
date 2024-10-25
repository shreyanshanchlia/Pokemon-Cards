using System.Collections;
using UnityEngine;

namespace Utility.Extensions
{
	public static class MonoBehaviourExtensions
	{
		public static void RunUpdateUntilBool(this MonoBehaviour monoBehaviour, System.Func<bool> runWhile)
		{
			monoBehaviour.StartCoroutine(BoolCheckCoroutine(runWhile));
		}

		private static IEnumerator BoolCheckCoroutine(System.Func<bool> runWhile)
		{
			while (runWhile())
			{
				yield return null;
			}
		}
	}
}