using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUi : MonoBehaviour
{
   [SerializeField] private Button createButton;
   [SerializeField] private Button closeButton;
   [SerializeField] private TMP_InputField inputField;

   private void Awake()
   {
      createButton.onClick.AddListener(() => {KitchenGameLobby.Instance.CreateLobby(inputField.text,true);});
      closeButton.onClick.AddListener(() => {Hide();});
   }


   private void Start()
   {
      Hide();
   }

   public void Show()
   {
      gameObject.SetActive(true);
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }
}
