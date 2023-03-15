using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;


namespace GameManager
{
    public class GameManagerCamera
    {
        Camera m_camera;

        public GameManagerCamera(Camera camera)
        {
            m_camera = camera;
        }

        public void AddPosition(Vector3 pos,float time,Ease ease)
        {
            m_camera.transform.DOMove(m_camera.transform.position + pos, time).SetEase(ease);
        }

        public void FailGame(UnityAction unityAction)
        {
            m_camera.DOOrthoSize(1, 3).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                unityAction.Invoke();
            });
        }
    }
}
