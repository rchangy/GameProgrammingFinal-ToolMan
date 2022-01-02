using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ToolMan.UI
{
    public class Notification : MonoBehaviour
    {
        public TextMeshProUGUI _text;
        public GameObject checkMark;
        public CanvasGroup canvasGroup;
        public float fadingTime = 0.5f;
        private bool countingDown;
        private float countDownTime;

        private State state;

        // Start is called before the first frame update
        void Start()
        {
            state = State.Idle;
            countingDown = false;
            countDownTime = fadingTime;
            checkMark.SetActive(false);
            canvasGroup.alpha = 0;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.FadingIn:
                    FadeIn();
                    break;
                case State.Displaying:
                    break;
                case State.FadingOut:
                    FadeOut();
                    break;

            }

            Debug.Log("state = " + state);

            if (countingDown)
                countDownTime -= Time.deltaTime;
        }

        private void FadeIn()
        {
            Debug.Log(name + " obj st fading in " + canvasGroup.alpha);
            canvasGroup.alpha = 1 - countDownTime / fadingTime;

            if (countDownTime <= 0)
            {
                canvasGroup.alpha = 1;
                state = State.Displaying;
                countingDown = false;
            }
        }

        private void FadeOut()
        {
            canvasGroup.alpha = countDownTime / fadingTime;

            if (countDownTime <= 0)
            {
                canvasGroup.alpha = 0;
                state = State.Idle;
                countingDown = false;
            }

        }

        public void OnStart(string text)
        { // Start to fade in
            _text.text = text;
            state = State.FadingIn;
            countDownTime = fadingTime;
            countingDown = true;
            canvasGroup.alpha = 0;
            checkMark.SetActive(false);
        }

        public void OnComplete()
        { // Start to fade out
            state = State.FadingOut;
            countDownTime = fadingTime;
            countingDown = true;
            canvasGroup.alpha = 1;
            checkMark.SetActive(true);
        }

        private enum State
        {
            Idle,
            FadingIn,
            Displaying,
            FadingOut
        }
    }
}
