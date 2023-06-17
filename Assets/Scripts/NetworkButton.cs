using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Button))]

    public class NetworkButton : MonoBehaviour
    {
        public UnityEvent<Mode> request = new();
        [SerializeField] private Mode mode;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => request.Invoke(mode));
        }

        public enum Mode
        {
            Host,
            Client
        }
    }
}
