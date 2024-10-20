using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
   [SerializeField] private Button musicButton;
   [SerializeField] private Button soundEffectsButton;
   [SerializeField] private Button backButton;
   [SerializeField] private TextMeshProUGUI musicText;
   [SerializeField] private TextMeshProUGUI soundText;
   [SerializeField] private Button moveUpButton;
   [SerializeField] private Button moveDownButton;
   [SerializeField] private Button moveRightButton;
   [SerializeField] private Button moveLeftButton;
   [SerializeField] private Button interactButton;
   [SerializeField] private Button cutInteractButton;
   [SerializeField] private Button escButton;

   [SerializeField] private TextMeshProUGUI moveUpButtonText;
   [SerializeField] private TextMeshProUGUI moveDownButtonText;
   [SerializeField] private TextMeshProUGUI moveRightButtonText;
   [SerializeField] private TextMeshProUGUI moveLeftButtonText;
   [SerializeField] private TextMeshProUGUI interactButtonText;
   [SerializeField] private TextMeshProUGUI cutInteractButtonText;
   [SerializeField] private TextMeshProUGUI escButtonText;

   [SerializeField] private Transform reBindVisual;
   
   public static OptionsUI Instance { get; private set; }
   
   private void Awake()
   {
      Instance = this;
      musicButton.onClick.AddListener(() =>
      {
         MusicManager.Instance.ChangeVolume();
         UpdateVisuals();
      });
      
      soundEffectsButton.onClick.AddListener(() => {
         SoundEffectsManager.Instance.ChangeVolume();
         UpdateVisuals();});
      
      backButton.onClick.AddListener(() =>
      {
         Hide();
         GameManager.Instance.PauseGame();
      });
      
      moveUpButton.onClick.AddListener(() => {Rebind(TakeInput.Bindings.MoveUp); });
      moveDownButton.onClick.AddListener(() => { Rebind(TakeInput.Bindings.MoveDown); });
      moveRightButton.onClick.AddListener(() => { Rebind(TakeInput.Bindings.MoveRight); });
      moveLeftButton.onClick.AddListener(() => { Rebind(TakeInput.Bindings.MoveLeft);});
      interactButton.onClick.AddListener(() => { Rebind(TakeInput.Bindings.Interact); });
      cutInteractButton.onClick.AddListener(() => { Rebind(TakeInput.Bindings.CutInteract);});
      escButton.onClick.AddListener(() => { Rebind(TakeInput.Bindings.Pause); });
      
   }

   private void Instance_onPauseGame()
   {
      Hide();
   }

   private void Start()
   {
      TakeInput.Instance.onPauseGame += Instance_onPauseGame;
      UpdateVisuals();
      HideReBind();
      Hide();
   }


   private void UpdateVisuals()
   {
      soundText.text = "SoundEffects : " + Mathf.Round(SoundEffectsManager.Instance.GetVolume() * 10f);
      musicText.text = "MusicEffects : " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
      moveUpButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.MoveUp);
      moveDownButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.MoveDown);
      moveRightButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.MoveRight);
      moveLeftButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.MoveLeft);
      interactButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.Interact);
      cutInteractButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.CutInteract);
      escButtonText.text = TakeInput.Instance.GetKeyBinding(TakeInput.Bindings.Pause);

   }

   public void Show()
   {
      gameObject.SetActive(true);
   }

   public void Hide()
   {
      gameObject.SetActive(false);
   }

   private void ShowReBind()
   {
      reBindVisual.gameObject.SetActive(true);
   }

   private void HideReBind()
   {
      reBindVisual.gameObject.SetActive(false);
   }

   private void Rebind(TakeInput.Bindings Key)
   {
      ShowReBind();
      TakeInput.Instance.Rebinding(Key, () =>
      {
         HideReBind();
         UpdateVisuals();
      });
   }
   
   
}
