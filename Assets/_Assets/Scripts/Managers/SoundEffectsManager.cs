using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectsManager : MonoBehaviour
{
    private const string PLAYER_PREFS_VOLUME_SAVE = "SoundEffects";
    [SerializeField] private SoundEffects Sounds;
    [SerializeField] private float volume;
    
    public static SoundEffectsManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUME_SAVE, 1f);
    }

    public void Start()
    {
        DeliveryManager.Instance.onPlayFailSound += InstanceOnonPlayFailSound;
        DeliveryManager.Instance.onPlaySuccessSound += InstanceOnonPlaySuccessSound;
        CuttingCounter.OnAnyCutPlaySound += CuttingCounterOnOnAnyCutPlaySound;
        PlayerMovement.onPlayerSpawn += OnPlayerSpawn_AttachSound;
        BaseCounterScript.onPlayObjectDropSound += BaseCounterScriptOnonPlayObjectDropSound;
        TrashScript.onTrashSoundPlay += TrashScriptOnonPlayObjectDropSound;
    }

    private void OnPlayerSpawn_AttachSound(object sender, EventArgs e)
    {
        if (PlayerMovement.LocalInstance != null)
        {
             PlayerMovement.onPlayerSpawn -= OnPlayerSpawn_AttachSound;
             PlayerMovement.LocalInstance.OnPlayPickUpSound += InstanceOnOnPlayPickUpSound;
             PlayerSound.onPlayFootSounds += PlayerSoundOnonPlayFootSounds;
        }
    }

    private void PlayerSoundOnonPlayFootSounds(object sender, EventArgs e)
    {
       PlaySound(Sounds.footstep,PlayerMovement.LocalInstance.transform.position);
    }

    private void TrashScriptOnonPlayObjectDropSound(object sender, EventArgs e)
    {
        TrashScript senderAsTrashScript = sender as TrashScript;
        PlaySound(Sounds.trash,senderAsTrashScript.transform.position);
    }

    private void BaseCounterScriptOnonPlayObjectDropSound(object sender, EventArgs e)
    {
        BaseCounterScript baseCounterSender = sender as BaseCounterScript;
        PlaySound(Sounds.objectDrop, baseCounterSender.transform.position);
    }

    private void InstanceOnOnPlayPickUpSound(object sender, EventArgs e)
    {
        PlayerMovement player = sender as PlayerMovement;
        PlaySound(Sounds.objectPickup,player.transform.position);
    }

    private void CuttingCounterOnOnAnyCutPlaySound(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounterSender = sender as CuttingCounter;
        PlaySound(Sounds.chop,cuttingCounterSender.transform.position);
    }

    private void InstanceOnonPlaySuccessSound(object sender, EventArgs e)
    {
        PlaySound(Sounds.deliverySuccess,DeliveryCounter.Instance.transform.position);
    }

    private void InstanceOnonPlayFailSound(object sender, EventArgs e)
    {
        PlaySound(Sounds.deliveryFail,DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audios, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audios[Random.Range(0,audios.Length)],position,volumeMultiplier);
    }

    private void PlaySound(AudioClip audio, Vector3 position, float volumeMultiplier)
    {
        AudioSource.PlayClipAtPoint(audio,position, volumeMultiplier * volume);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUME_SAVE,volume);
        PlayerPrefs.Save();
        
        
    }

    public float GetVolume()
    {
        return volume;
    }
}
