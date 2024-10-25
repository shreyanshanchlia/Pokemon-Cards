using DG.Tweening;
using UnityEngine;

namespace Utility
{
	public class MoveAnimator : MonoBehaviour
	{
		[SerializeField] private Ease ease;
		[SerializeField] private LoopType loopType = LoopType.Yoyo;

		[SerializeField] public float moveDuration = 1f;
		[SerializeField] private Vector3 startPosition;
		[SerializeField] private Vector3 endPosition;

		private void Start()
		{
			transform.DOLocalMove(endPosition, moveDuration).SetEase(ease)
				.OnComplete(() => transform.DOLocalMove(startPosition, moveDuration).SetEase(ease).SetLoops(-1, loopType));
		}

		public void StopAnimation()
		{
			transform.DOKill(true);
		}
	}
}