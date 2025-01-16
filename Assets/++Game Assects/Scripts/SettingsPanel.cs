using System;
using System.Collections.Generic;
using System.Xml;
using Mirror.BouncyCastle.Asn1.Mozilla;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static SettingData;
using static SettingData.Controls;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private List<LeftNavigationSlot> allLeftSlots;
    [SerializeField] private List<GameObject> allPanels;

    [Header("---------Graphics Control Panels---------")]
    [SerializeField] private List<ButtonUIAttributes> allGraphicsButton;
    [SerializeField] private List<ButtonUIAttributes> allFrameRateButton;
    [SerializeField] private SliderUIAttributes Brightness;

    [Header("---------Sounds Control Panels---------")]
    [SerializeField] private SliderUIAttributes MasterVolume;
    [SerializeField] private SliderUIAttributes BackgroundMusic;
    [SerializeField] private SliderUIAttributes VoiceChat;
    [SerializeField] private SliderUIAttributes GameSFX;

    [Header("---------Controls Panels---------")]
    [SerializeField] private ToggleButtonUIAttributes FireSetting_AlwaysOn_Attributes;
    private bool FireSetting_AlwaysOn = true;
    [SerializeField] private ToggleButtonUIAttributes FireSetting_Scope_Attributes;
    private bool FireSetting_Scope = true;
    [SerializeField] private List<ButtonUIAttributes> allBoltRifleButton;
    [SerializeField] private List<ButtonUIAttributes> allShotgunFireButton;
    [SerializeField] private List<ButtonUIAttributes> allSensivitySettingsButton;

    [Space(5)]
    [SerializeField] private SliderUIAttributes TppnoScope;
    [SerializeField] private SliderUIAttributes RedDote;
    [SerializeField] private SliderUIAttributes TwoXScope;
    [SerializeField] private SliderUIAttributes ThreeXScope;
    [SerializeField] private SliderUIAttributes FourXScope;
    [SerializeField] private SliderUIAttributes SixXScope;
    [SerializeField] private SliderUIAttributes CameraSensivity;
    [SerializeField] private SliderUIAttributes CameraParachuting;


    [Header("---------Gameplay Control Panels---------")]
    [SerializeField] private ToggleButtonUIAttributes AutoPicksHints_Attributes;
    private bool autoPickHints = true;
    [SerializeField] private ToggleButtonUIAttributes AutoPicks_Attributes;
    private bool autoPick = true;
    [SerializeField] private ToggleButtonUIAttributes Hints_Attributes;
    private bool hints = true;


    [Header("---------Others---------")]

    public Sprite UnSelectButton_Sprite;
    public Sprite SelectButton_Sprite;

    private void Start()
    {
        SetGraphicsAttributes();

        SetSoundControlAttributes();

        SetControlAttributes();
    }
    public void OpenPanel(int index)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(i == index);
            allLeftSlots[i].Disable();
        }
        allLeftSlots[index].Enable();
    }

    private void SetSlider(SliderUIAttributes sliderUI,  float sliderValue)
    {
        // Set the initial value of the slider
        sliderUI.slider.value = sliderValue;
        int val = Mathf.RoundToInt(sliderUI.slider.value * 100);
        sliderUI._valueText.text = $"<color=#ABFEAF>{val}</color>/<color=white>100%</color>";

        // Register the event listener, using the current slider value
        sliderUI.slider.onValueChanged.AddListener((value) => UpdateSlider(sliderUI,sliderValue));

    }

    private void UpdateSlider(SliderUIAttributes sliderUI,  float sliderValue)
    {
        sliderValue = sliderUI.slider.value;
        int val = Mathf.RoundToInt(sliderUI.slider.value * 100);
        sliderUI._valueText.text = $"<color=#ABFEAF>{val}</color>/<color=white>100%</color>";

       
    }

    #region Graphics Func

    private void SetGraphicsAttributes()
    {
        // Retrieve the current Control settings
        var GraphicSetting = DataHandler.Instance.IngameData.Setting.graphic;

        SetSlider(Brightness, GraphicSetting.BrightnessSlider_val);
    }


    public void UpdateGraphicState(int graphic)
    {
        for(int i=0; i< allGraphicsButton.Count; i++)
        {
            allGraphicsButton[i].Button.sprite = UnSelectButton_Sprite;
            allGraphicsButton[i].Text.color = Color.grey;
            allGraphicsButton[i].Text.fontStyle &= ~FontStyles.Bold; // Remove the Bold flag

        }

        allGraphicsButton[graphic].Button.sprite = SelectButton_Sprite;
        allGraphicsButton[graphic].Text.fontStyle |= FontStyles.Bold; // Add the Bold flag
        allGraphicsButton[graphic].Text.color = Color.black;

        // Map the index to the corresponding enum value
        if (System.Enum.IsDefined(typeof(GraphicState), graphic))
        {
            DataHandler.Instance.IngameData.Setting.graphic.Graphic_val = (GraphicState)graphic;
        }
        else
        {
            Debug.LogWarning("Invalid graphic index!");
        }
    }

    public void UpdateFrameRateState(int FrameRate)
    {
        for (int i = 0; i < allFrameRateButton.Count; i++)
        {
            allFrameRateButton[i].Button.sprite = UnSelectButton_Sprite;
            allFrameRateButton[i].Text.color = Color.grey;
            allFrameRateButton[i].Text.fontStyle &= ~FontStyles.Bold; // Remove the Bold flag

        }

        allFrameRateButton[FrameRate].Button.sprite = SelectButton_Sprite;
        allFrameRateButton[FrameRate].Text.fontStyle |= FontStyles.Bold; // Add the Bold flag
        allFrameRateButton[FrameRate].Text.color = Color.black;

        // Map the index to the corresponding enum value
        if (System.Enum.IsDefined(typeof(FrameRateState), FrameRate))
        {
            DataHandler.Instance.IngameData.Setting.graphic.FrameRate_val = (FrameRateState)FrameRate;
        }
        else
        {
            Debug.LogWarning("Invalid graphic index!");
        }
    }
    #endregion


    #region Sound Func
    void SetSoundControlAttributes()
    {
        // Retrieve the current sound settings
        var soundSettings = DataHandler.Instance.IngameData.Setting.sound;

        // Update the Master Volume slider and text
        SetSlider(MasterVolume,  soundSettings.MasterVolumeSlider_val);

        // Update the Background Music slider and text
        SetSlider(BackgroundMusic,  soundSettings.BackgroundMusicSlider_val);

        // Update the Voice Chat slider and text
        SetSlider(VoiceChat,  soundSettings.VoiceChatSlider_val);

        // Update the Game SFX slider and text
        SetSlider(GameSFX,  soundSettings.GameSFXSlider_val);
    }

    #endregion


    #region Controls Func


    void SetControlAttributes()
    {
        // Retrieve the current Control settings
        var ControlSetting = DataHandler.Instance.IngameData.Setting.Control;

       SetSlider(TppnoScope, ControlSetting.TppnoScopeSlider_val);
       SetSlider(RedDote, ControlSetting.RedDotSlider_val);
       SetSlider(TwoXScope, ControlSetting.twoxScopeSlider_val);
       SetSlider(ThreeXScope, ControlSetting.ThreexScopeSlider_val);
       SetSlider(FourXScope, ControlSetting.FourxScopeSlider_val);
       SetSlider(SixXScope, ControlSetting.SixxScopeSlider_val);
       SetSlider(CameraSensivity, ControlSetting.CameraSensivitySlider_val);
       SetSlider(CameraParachuting, ControlSetting.CameraParachutingSlider_val);
    }

    public void UpdateFireSettingBoltRifleState(int BoltRifleState)
    {
        for (int i = 0; i < allBoltRifleButton.Count; i++)
        {
            allBoltRifleButton[i].Button.sprite = UnSelectButton_Sprite;
            allBoltRifleButton[i].Text.color = Color.grey;
            allBoltRifleButton[i].Text.fontStyle &= ~FontStyles.Bold; // Remove the Bold flag

        }

        allBoltRifleButton[BoltRifleState].Button.sprite = SelectButton_Sprite;
        allBoltRifleButton[BoltRifleState].Text.fontStyle |= FontStyles.Bold; // Add the Bold flag
        allBoltRifleButton[BoltRifleState].Text.color = Color.black;

        // Map the index to the corresponding enum value
        if (System.Enum.IsDefined(typeof(FireAction), BoltRifleState))
        {
            DataHandler.Instance.IngameData.Setting.Control.fireSetting.BoltActionRifle = (FireAction)BoltRifleState;
        }
        else
        {
            Debug.LogWarning("Invalid graphic index!");
        }
    }

    public void UpdateFireSettingShotgunState(int ShotGunStateState)
    {
        for (int i = 0; i < allShotgunFireButton.Count; i++)
        {
            allShotgunFireButton[i].Button.sprite = UnSelectButton_Sprite;
            allShotgunFireButton[i].Text.color = Color.grey;
            allShotgunFireButton[i].Text.fontStyle &= ~FontStyles.Bold; // Remove the Bold flag

        }

        allShotgunFireButton[ShotGunStateState].Button.sprite = SelectButton_Sprite;
        allShotgunFireButton[ShotGunStateState].Text.fontStyle |= FontStyles.Bold; // Add the Bold flag
        allShotgunFireButton[ShotGunStateState].Text.color = Color.black;

        // Map the index to the corresponding enum value
        if (System.Enum.IsDefined(typeof(FireAction), ShotGunStateState))
        {
            DataHandler.Instance.IngameData.Setting.Control.fireSetting.ShotgunFiringMode = (FireAction)ShotGunStateState;
        }
        else
        {
            Debug.LogWarning("Invalid graphic index!");
        }
    }

    public void UpdateFireSensivityState(int SensivityState)
    {
        for (int i = 0; i < allSensivitySettingsButton.Count; i++)
        {
            allSensivitySettingsButton[i].Button.sprite = UnSelectButton_Sprite;
            allSensivitySettingsButton[i].Text.color = Color.grey;
            allSensivitySettingsButton[i].Text.fontStyle &= ~FontStyles.Bold; // Remove the Bold flag

        }

        allSensivitySettingsButton[SensivityState].Button.sprite = SelectButton_Sprite;
        allSensivitySettingsButton[SensivityState].Text.fontStyle |= FontStyles.Bold; // Add the Bold flag
        allSensivitySettingsButton[SensivityState].Text.color = Color.black;

        // Map the index to the corresponding enum value
        if (System.Enum.IsDefined(typeof(Sensitivity), SensivityState))
        {
            DataHandler.Instance.IngameData.Setting.Control.fireSetting.SensitivitySetting = (Sensitivity)SensivityState;
        }
        else
        {
            Debug.LogWarning("Invalid graphic index!");
        }
    }

    public void ToggleFireSetting_AlwaysOn()
    {
        FireSetting_AlwaysOn = !FireSetting_AlwaysOn;
        LeanTween.moveLocalX(FireSetting_AlwaysOn_Attributes.toggleButton.gameObject, FireSetting_AlwaysOn ? FireSetting_AlwaysOn_Attributes.toggleOnTransform.localPosition.x : FireSetting_AlwaysOn_Attributes.toggleOffTransform.localPosition.x, 0.1f);
        if (FireSetting_AlwaysOn)
        {
            //rgba(137, 204, 140, 1)
            Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
            LeanTween.imageColor(FireSetting_AlwaysOn_Attributes.toggleButton.GetComponent<RectTransform>(), greenColor, 0.2f);
            LeanTween.imageColor(FireSetting_AlwaysOn_Attributes.toggleBg.GetComponent<RectTransform>(), greenColor, 0.2f);
        }
        else
        {
            Color grey = new Color(.70f, .70f, .70f);
            LeanTween.imageColor(FireSetting_AlwaysOn_Attributes.toggleButton.GetComponent<RectTransform>(), grey, 0.2f);
            LeanTween.imageColor(FireSetting_AlwaysOn_Attributes.toggleBg.GetComponent<RectTransform>(), grey, 0.2f);
        }

        DataHandler.Instance.IngameData.Setting.Control.fireSetting.AlwaysOn = FireSetting_AlwaysOn;
    }

    public void ToggleFireSetting_Scope()
    {
        FireSetting_Scope = !FireSetting_Scope;
        LeanTween.moveLocalX(FireSetting_Scope_Attributes.toggleButton.gameObject, FireSetting_Scope ? FireSetting_Scope_Attributes.toggleOnTransform.localPosition.x : FireSetting_Scope_Attributes.toggleOffTransform.localPosition.x, 0.1f);
        if (FireSetting_Scope)
        {
            //rgba(137, 204, 140, 1)
            Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
            LeanTween.imageColor(FireSetting_Scope_Attributes.toggleButton.GetComponent<RectTransform>(), greenColor, 0.2f);
            LeanTween.imageColor(FireSetting_Scope_Attributes.toggleBg.GetComponent<RectTransform>(), greenColor, 0.2f);
        }
        else
        {
            Color grey = new Color(.70f, .70f, .70f);
            LeanTween.imageColor(FireSetting_Scope_Attributes.toggleButton.GetComponent<RectTransform>(), grey, 0.2f);
            LeanTween.imageColor(FireSetting_Scope_Attributes.toggleBg.GetComponent<RectTransform>(), grey, 0.2f);
        }

        DataHandler.Instance.IngameData.Setting.Control.fireSetting.Scope = FireSetting_Scope;
    }

    #endregion


    #region GamePlay Func
    public void ToggleAutoPicksHints()
    {
        autoPickHints = !autoPickHints;
        LeanTween.moveLocalX(AutoPicksHints_Attributes.toggleButton.gameObject, autoPickHints ? AutoPicksHints_Attributes.toggleOnTransform.localPosition.x : AutoPicksHints_Attributes.toggleOffTransform.localPosition.x, 0.1f);
        if (autoPickHints)
        {
            //rgba(137, 204, 140, 1)
            Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
            LeanTween.imageColor(AutoPicksHints_Attributes.toggleButton.GetComponent<RectTransform>(), greenColor, 0.2f);
            LeanTween.imageColor(AutoPicksHints_Attributes.toggleBg.GetComponent<RectTransform>(), greenColor, 0.2f);
        }
        else
        {
            Color grey = new Color(.70f, .70f, .70f);
            LeanTween.imageColor(AutoPicksHints_Attributes.toggleButton.GetComponent<RectTransform>(), grey, 0.2f);
            LeanTween.imageColor(AutoPicksHints_Attributes.toggleBg.GetComponent<RectTransform>(), grey, 0.2f);
        }

        DataHandler.Instance.IngameData.Setting.gameplay.autoPickHints = autoPickHints;
    }

    public void ToggleAutoPicks()
    {
        autoPick = !autoPick;
        LeanTween.moveLocalX(AutoPicks_Attributes.toggleButton.gameObject, autoPick ? AutoPicks_Attributes.toggleOnTransform.localPosition.x : AutoPicks_Attributes.toggleOffTransform.localPosition.x, 0.1f);
        if (autoPick)
        {
            //rgba(137, 204, 140, 1)
            Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
            LeanTween.imageColor(AutoPicks_Attributes.toggleButton.GetComponent<RectTransform>(), greenColor, 0.2f);
            LeanTween.imageColor(AutoPicks_Attributes.toggleBg.GetComponent<RectTransform>(), greenColor, 0.2f);
        }
        else
        {
            Color grey = new Color(.70f, .70f, .70f);
            LeanTween.imageColor(AutoPicks_Attributes.toggleButton.GetComponent<RectTransform>(), grey, 0.2f);
            LeanTween.imageColor(AutoPicks_Attributes.toggleBg.GetComponent<RectTransform>(), grey, 0.2f);
        }

        DataHandler.Instance.IngameData.Setting.gameplay.autoPick = autoPick;
    }

    public void ToggleHints()
    {
        hints = !hints;
        LeanTween.moveLocalX(Hints_Attributes.toggleButton.gameObject, hints ? Hints_Attributes.toggleOnTransform.localPosition.x : Hints_Attributes.toggleOffTransform.localPosition.x, 0.1f);
        if (hints)
        {
            //rgba(137, 204, 140, 1)
            Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
            LeanTween.imageColor(Hints_Attributes.toggleButton.GetComponent<RectTransform>(), greenColor, 0.2f);
            LeanTween.imageColor(Hints_Attributes.toggleBg.GetComponent<RectTransform>(), greenColor, 0.2f);
        }
        else
        {
            Color grey = new Color(.70f, .70f, .70f);
            LeanTween.imageColor(Hints_Attributes.toggleButton.GetComponent<RectTransform>(), grey, 0.2f);
            LeanTween.imageColor(Hints_Attributes.toggleBg.GetComponent<RectTransform>(), grey, 0.2f);
        }

        DataHandler.Instance.IngameData.Setting.gameplay.Hints = hints;
    }

    #endregion
}

[System.Serializable]
public class ButtonUIAttributes
{
    public Image Button;
    public TMP_Text Text;
}


[System.Serializable]
public class SliderUIAttributes
{
    public Slider slider;
    public TMP_Text _valueText;
}

[System.Serializable]
public class ToggleButtonUIAttributes
{
    public Image toggleButton;
    public Image toggleBg;
    public Transform toggleOffTransform;
    public Transform toggleOnTransform;
}

[System.Serializable]
public class SettingData
{
    public Graphic graphic;

    public Sound sound;

    public Controls Control;

    public CustomControl CustomGamePlayControl;

    public Gameplay gameplay;

    #region Graphic 
    [System.Serializable]
    public struct Graphic
    {
        public GraphicState Graphic_val;
        public FrameRateState FrameRate_val;
        public GraphicStyleState GraphicStyle_val;

        public int BrightnessSlider_val;
    }
    [System.Serializable]
    public enum GraphicState
    {
        Smooth,Balance,HD,HDR
    }
    public enum FrameRateState
    {
        PawerSaving, Medium, High, UltraHigh
    }
    public enum GraphicStyleState
    {
        Classic, Colorful, Movie, AutoAdjust
    }
    #endregion

    #region Sound 
    [System.Serializable]
    public class Sound
    {
        public float MasterVolumeSlider_val;
        public float BackgroundMusicSlider_val;
        public float VoiceChatSlider_val;
        public float GameSFXSlider_val;
    }


    #endregion

    #region Controls 
    [System.Serializable]
    public struct Controls
    {
        public FireSetting fireSetting;

        [Space(5)]

        public int TppnoScopeSlider_val;
        public int RedDotSlider_val;

        public int twoxScopeSlider_val;
        public int ThreexScopeSlider_val;

        public int FourxScopeSlider_val;
        public int SixxScopeSlider_val;

        public int CameraSensivitySlider_val;
        public int CameraParachutingSlider_val;

        [System.Serializable]
        public struct FireSetting
        {
            public bool AlwaysOn;
            public bool Scope;

            public FireAction BoltActionRifle;
            public FireAction ShotgunFiringMode;

            public Sensitivity SensitivitySetting;
        }

        public enum FireAction
        {
            Tap,Release
        }

        public enum Sensitivity
        {
            Low,Medium,High
        }
    }


    #endregion

    #region CustomControl 
    [System.Serializable]
    public struct CustomControl
    {
        public int CustomControlSelected;
    }


    #endregion

    #region Gameplay 
    [System.Serializable]
    public struct Gameplay
    {
        public bool  autoPickHints;
        public bool  autoPick;
        public bool  Hints;
    }


    #endregion
}


