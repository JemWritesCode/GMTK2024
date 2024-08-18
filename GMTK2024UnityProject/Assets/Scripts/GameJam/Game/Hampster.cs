using System.Collections;

using UnityEngine;

namespace GameJam
{
    public class Hamster : MonoBehaviour
    {
        public CableStartPoint Lunch;
        public GameObject Effects;

        private void Awake()
        {
            StartCoroutine(EatLunch());
        }

        private IEnumerator EatLunch()
        {
            yield return null;
            if (Lunch != null)
            {
                Effects.SetActive(true);
                foreach (var effect in Effects.GetComponentsInChildren<ParticleSystem>())
                {
                    effect.Play();
                }

                yield return new WaitForSeconds(5);
                Lunch.BreakConnection();
            }

            Lunch.HasHamster = false;
            GameObject.Destroy(this.gameObject);
        }
    }
}