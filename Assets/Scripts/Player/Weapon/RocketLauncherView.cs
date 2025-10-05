using UnityEngine;
using DG.Tweening;

public class RocketLauncherView : MonoBehaviour
{
    [Header("Настройки отдачи")]
    [SerializeField] private float _recoilBack = 0.4f;
    [SerializeField] private float _recoilUp = 6f;
    [SerializeField] private float _recoilDuration = 0.15f;
    [SerializeField] private float _returnDuration = 0.25f;

    private Vector3 _startLocalPos;
    private Vector3 _startLocalRot;

    private void Start()
    {
        _startLocalPos = transform.localPosition;
        _startLocalRot = transform.localEulerAngles;
    }

    public void PlayRecoil(float multiplier)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMove(_startLocalPos + new Vector3(0, 0, -_recoilBack * multiplier * 0.05f), _recoilDuration)
            .SetEase(Ease.OutQuad));
        seq.Join(transform.DOLocalRotate(_startLocalRot + new Vector3(-_recoilUp * multiplier * 0.05f, 0, 0), _recoilDuration)
            .SetEase(Ease.OutQuad));

        seq.Append(transform.DOLocalMove(_startLocalPos, _returnDuration).SetEase(Ease.OutCubic));
        seq.Join(transform.DOLocalRotate(_startLocalRot, _returnDuration).SetEase(Ease.OutCubic));
    }
}