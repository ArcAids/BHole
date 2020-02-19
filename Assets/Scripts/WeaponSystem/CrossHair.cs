using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace WeaponSystem
{
    public class CrossHair : MonoBehaviour
    {
        Image reticle;
        [SerializeField]
        float expandSpeed = 10;

        private void Awake()
        {
            reticle = GetComponent<Image>();
        }

        public void Expand(float value)
        {
            StopAllCoroutines();
            StartCoroutine(ExpandReticle(1 + value));

        }

        IEnumerator ExpandReticle(float value)
        {
            Vector3 targetScale = Vector3.one * (value);
            while (reticle.transform.localScale.x != value)
            {
                reticle.transform.localScale = Vector3.Lerp(reticle.transform.localScale, targetScale, Time.deltaTime * expandSpeed);
                yield return null;
            }
        }

        public void Hide()
        {
            reticle.enabled = false;
        }
        public void Show()
        {
            reticle.enabled = true;
        }
    }
}