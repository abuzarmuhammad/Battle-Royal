using System;
using System.Collections.Generic;
using System.Xml;
using Mirror.BouncyCastle.Asn1.Mozilla;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SettingData;
using static SettingData.Controls;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private List<LeftNavigationSlot> allLeftSlots;
    [SerializeField] private List<GameObject> allPanels;

    [Header("---------Graphics Control Panels---------")]
    [SerializeField] private List<ButtonUIAttributes> allGraphicsButton;
    int GraphicState_index;
    [SerializeField] private List<ButtonUIAttributes> allFrameRateButton;
    int FrameRateState_index;
    [SerializeField] private List<ButtonUIAttributes> allGraphicsStyleButton;
    int GraphicStyleState_index;
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
    int FBoltRifleState_index;
    [SerializeField] private List<ButtonUIAttributes> allShotgunFireButton;
    int ShotgunFireState_index;
    [SerializeField] private List<ButtonUIAttributes> allSensivitySettingsButton;
    int SensivitySettingsState_index;

    [SerializeField] private List<ButtonUIAttributes> allControlType_Buttons;

    [Space(5)]
    [SerializeField] private SliderUIAttributes TppnoScope;
    [SerializeField] private SliderUIAttributes RedDote;
    [SerializeField] private SliderUIAttributes TwoXScope;
    [SerializeField] private SliderUIAttributes ThreeXScope;
    [SerializeField] private SliderUIAttributes FourXScope;
    [SerializeField] private SliderUIAttributes SixXScope;
    [SerializeField] private SliderUIAttributes CameraSensivity;
    [SerializeField] private SliderUIAttributes CameraParachuting;

    [Header("---------Custom Control Panels---------")]
    [SerializeField] private GameObject _CustomControlPopup;
    [SerializeField] private TMP_Text _CustomControl_txt;

    int CustomControl;
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

    private void OnEnable()
    {
        if (!DataHandler.Instance.isFileExits())
        {
            ResetSettings();
        }
        else
        {
            SetSettingAttributes();
        }
        
    }

    void SetSettingAttributes()
    {
        SetGraphicsAttributes();

        SetSoundControlAttributes();

        SetControlAttributes();

        SetGamePlayAttributes();
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

    public void SaveSettings()
    {
        // Save Graphic Settings
        DataHandler.Instance.IngameData.Setting.graphic.Graphic_val = (GraphicState)GraphicState_index;
        DataHandler.Instance.IngameData.Setting.graphic.FrameRate_val = (FrameRateState)FrameRateState_index;
        DataHandler.Instance.IngameData.Setting.graphic.GraphicStyle_val = (GraphicStyleState)GraphicStyleState_index;
        DataHandler.Instance.IngameData.Setting.graphic.BrightnessSlider_val = Brightness.slider.value;

        // Save Sound Settings
        DataHandler.Instance.IngameData.Setting.sound.MasterVolumeSlider_val = MasterVolume.slider.value;
        DataHandler.Instance.IngameData.Setting.sound.BackgroundMusicSlider_val = BackgroundMusic.slider.value;
        DataHandler.Instance.IngameData.Setting.sound.VoiceChatSlider_val = VoiceChat.slider.value;
        DataHandler.Instance.IngameData.Setting.sound.GameSFXSlider_val = GameSFX.slider.value;

        // Save Control Settings
        DataHandler.Instance.IngameData.Setting.Control.TppnoScopeSlider_val = TppnoScope.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.RedDotSlider_val = RedDote.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.twoxScopeSlider_val = TwoXScope.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.ThreexScopeSlider_val =ThreeXScope.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.FourxScopeSlider_val = FourXScope.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.SixxScopeSlider_val = SixXScope.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.CameraSensivitySlider_val = CameraSensivity.slider.value;
        DataHandler.Instance.IngameData.Setting.Control.CameraParachutingSlider_val = CameraParachuting.slider.value;

        // Save FireSettings
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.AlwaysOn = FireSetting_AlwaysOn;
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.Scope = FireSetting_Scope;
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.BoltActionRifle = (FireAction)FBoltRifleState_index;
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.ShotgunFiringMode = (FireAction)ShotgunFireState_index;
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.SensitivitySetting = (Sensitivity)SensivitySettingsState_index;

        // Save Custom Control Settings
        DataHandler.Instance.IngameData.Setting.CustomGamePlayControl.CustomControlSelected = CustomControl;

        // Save Gameplay Settings
        DataHandler.Instance.IngameData.Setting.gameplay.autoPickHints = autoPickHints;
        DataHandler.Instance.IngameData.Setting.gameplay.autoPick = autoPick; 
        DataHandler.Instance.IngameData.Setting.gameplay.Hints = hints; 

        // Save settings to persistent data
        DataHandler.Instance.SaveData();
    }
    public void ResetSettings()
    {
        // Reset Graphic Settings
        DataHandler.Instance.IngameData.Setting.graphic.Graphic_val = SettingData.GraphicState.Balance; // Default graphic state
        DataHandler.Instance.IngameData.Setting.graphic.FrameRate_val = SettingData.FrameRateState.Medium; // Default frame rate state
        DataHandler.Instance.IngameData.Setting.graphic.GraphicStyle_val = SettingData.GraphicStyleState.Colorful; // Default style
        DataHandler.Instance.IngameData.Setting.graphic.BrightnessSlider_val = 0.5f; // Default brightness

        // Reset Sound Settings
        DataHandler.Instance.IngameData.Setting.sound.MasterVolumeSlider_val = 0.5f; // Default master volume
        DataHandler.Instance.IngameData.Setting.sound.BackgroundMusicSlider_val = 0.5f; // Default background music volume
        DataHandler.Instance.IngameData.Setting.sound.VoiceChatSlider_val = 0.5f; // Default voice chat volume
        DataHandler.Instance.IngameData.Setting.sound.GameSFXSlider_val = 0.5f; // Default game sound effects volume

        // Reset Control Settings
        DataHandler.Instance.IngameData.Setting.Control.TppnoScopeSlider_val = 0.5f; // Default TPP no scope sensitivity
        DataHandler.Instance.IngameData.Setting.Control.RedDotSlider_val = 0.5f; // Default red dot scope sensitivity
        DataHandler.Instance.IngameData.Setting.Control.twoxScopeSlider_val = 0.5f; // Default 2x scope sensitivity
        DataHandler.Instance.IngameData.Setting.Control.ThreexScopeSlider_val = 0.5f; // Default 3x scope sensitivity
        DataHandler.Instance.IngameData.Setting.Control.FourxScopeSlider_val = 0.5f; // Default 4x scope sensitivity
        DataHandler.Instance.IngameData.Setting.Control.SixxScopeSlider_val = 0.5f; // Default 6x scope sensitivity
        DataHandler.Instance.IngameData.Setting.Control.CameraSensivitySlider_val = 0.5f; // Default camera sensitivity
        DataHandler.Instance.IngameData.Setting.Control.CameraParachutingSlider_val = 0.5f; // Default camera parachuting sensitivity

        // Reset FireSettings
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.AlwaysOn = true; // Default fire setting AlwaysOn
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.Scope = false; // Default fire setting for scope
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.BoltActionRifle = FireAction.Tap; // Default fire action for bolt-action rifle
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.ShotgunFiringMode = FireAction.Release; // Default shotgun firing mode
        DataHandler.Instance.IngameData.Setting.Control.fireSetting.SensitivitySetting = Sensitivity.Medium; // Default sensitivity

        // Reset Custom Control Settings
        DataHandler.Instance.IngameData.Setting.CustomGamePlayControl.CustomControlSelected = 0; // Default custom control selection

        // Reset Gameplay Settings
        DataHandler.Instance.IngameData.Setting.gameplay.autoPickHints = true; // Default auto pick hints
        DataHandler.Instance.IngameData.Setting.gameplay.autoPick = true; // Default auto pick
        DataHandler.Instance.IngameData.Setting.gameplay.Hints = true; // Default hints

        // Save the default settings to persistent data
        DataHandler.Instance.SaveData();

        SetSettingAttributes();
    }


    #region Slider Relate Function
    private void SetSlider(SliderUIAttributes sliderUI,  float sliderValue)
    {
        // Set the initial value of the slider
        LeanTween.value(gameObject, sliderUI.slider.value, sliderValue, 0.15f)
            .setOnUpdate((float val) =>
            {
                sliderUI.slider.value = val;
            });


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

       // SaveSliderData(sliderUI);
    }

    void SaveSliderData(SliderUIAttributes sliderUI)
    {
        if (sliderUI == Brightness)
        {
            DataHandler.Instance.IngameData.Setting.graphic.BrightnessSlider_val = sliderUI.slider.value;
        }
        else if(sliderUI == MasterVolume)
        {
            DataHandler.Instance.IngameData.Setting.sound.MasterVolumeSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == BackgroundMusic)
        {
            DataHandler.Instance.IngameData.Setting.sound.BackgroundMusicSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == VoiceChat)
        {
            DataHandler.Instance.IngameData.Setting.sound.VoiceChatSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == GameSFX)
        {
            DataHandler.Instance.IngameData.Setting.sound.GameSFXSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == TppnoScope)
        {
            DataHandler.Instance.IngameData.Setting.Control.TppnoScopeSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == RedDote)
        {
            DataHandler.Instance.IngameData.Setting.Control.RedDotSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == TwoXScope)
        {
            DataHandler.Instance.IngameData.Setting.Control.twoxScopeSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == ThreeXScope)
        {
            DataHandler.Instance.IngameData.Setting.Control.ThreexScopeSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == FourXScope)
        {
            DataHandler.Instance.IngameData.Setting.Control.FourxScopeSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == SixXScope)
        {
            DataHandler.Instance.IngameData.Setting.Control.SixxScopeSlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == CameraSensivity)
        {
            DataHandler.Instance.IngameData.Setting.Control.CameraSensivitySlider_val = sliderUI.slider.value;
        }
        else if (sliderUI == CameraParachuting)
        {
            DataHandler.Instance.IngameData.Setting.Control.CameraParachutingSlider_val = sliderUI.slider.value;
        }
    }

    #endregion

    #region Graphics Function

    private void SetGraphicsAttributes()
    {
        // Retrieve the current Control settings
        var GraphicSetting = DataHandler.Instance.IngameData.Setting.graphic;

        UpdateGraphicState((int)GraphicSetting.Graphic_val);
        UpdateFrameRateState((int) GraphicSetting.FrameRate_val);
        UpdateGraphicsStyleState((int)GraphicSetting.GraphicStyle_val);

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

        GraphicState_index = graphic;
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

       FrameRateState_index = FrameRate;
    }

    public void UpdateGraphicsStyleState(int GraphicStyle)
    {
        for (int i = 0; i < allGraphicsStyleButton.Count; i++)
        {
            allGraphicsStyleButton[i].Button.transform.GetChild(0).gameObject.SetActive(false);

        }

        allGraphicsStyleButton[GraphicStyle].Button.transform.GetChild(0).gameObject.SetActive(true); // Enable The Highlight

       GraphicStyleState_index = GraphicStyle;
    }
    #endregion


    #region Sound Fucntion
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

    #region CustomControl

    public void OpenCustomControlPopup(int customcontrol_index)
    {
        LeanTween.alphaCanvas(_CustomControlPopup.GetComponent<CanvasGroup>(), 1, 0.2f).setOnComplete(() =>
        {
            _CustomControlPopup.SetActive(true);
        });

        CustomControl = customcontrol_index;
    }


    public void CloseCustomControlPopup()
    {
        LeanTween.alphaCanvas(_CustomControlPopup.GetComponent<CanvasGroup>(), 0, 0.2f).setOnComplete(() =>
        {
            _CustomControlPopup.SetActive(false);
        });
    }
    #endregion


    #region Controls Function


    void SetControlAttributes()
    {
        // Retrieve the current Control settings
        var ControlSetting = DataHandler.Instance.IngameData.Setting.Control;

        FireSetting_AlwaysOn = !ControlSetting.fireSetting.AlwaysOn;
        ToggleFireSetting_AlwaysOn();

        FireSetting_Scope = !ControlSetting.fireSetting.Scope;
        ToggleFireSetting_Scope();

        UpdateFireSettingBoltRifleState((int)ControlSetting.fireSetting.BoltActionRifle);
        UpdateFireSettingShotgunState((int)ControlSetting.fireSetting.ShotgunFiringMode);
        UpdateFireSensivityState((int)ControlSetting.fireSetting.SensitivitySetting);

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

        FBoltRifleState_index = BoltRifleState;
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

        ShotgunFireState_index = ShotGunStateState;
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

       SensivitySettingsState_index = SensivityState;
    }

    public void UpdateControlTypeState(int ControlType)
    {
        for (int i = 0; i < allControlType_Buttons.Count; i++)
        {
            allControlType_Buttons[i].Button.sprite = UnSelectButton_Sprite;
            allControlType_Buttons[i].Text.color = Color.grey;
            allControlType_Buttons[i].Text.fontStyle &= ~FontStyles.Bold; // Remove the Bold flag

        }

        allControlType_Buttons[ControlType].Button.sprite = SelectButton_Sprite;
        allControlType_Buttons[ControlType].Text.fontStyle |= FontStyles.Bold; // Add the Bold flag
        allControlType_Buttons[ControlType].Text.color = Color.black;

        //SensivitySettingsState_index = ControlType;
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

    }

    #endregion


    #region GamePlay Function


    void SetGamePlayAttributes()
    {
        var Gameplaydata = DataHandler.Instance.IngameData.Setting.gameplay;

        autoPickHints = !Gameplaydata.autoPickHints;
        ToggleAutoPicksHints();

        this.autoPick = !Gameplaydata.autoPick;
        ToggleAutoPicks();

        this.hints = !Gameplaydata.Hints;
        ToggleHints();
    }
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

    }

    #endregion
}

#region Attributes

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

#endregion

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

        public float BrightnessSlider_val;
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

        public float TppnoScopeSlider_val;
        public float RedDotSlider_val;

        public float twoxScopeSlider_val;
        public float ThreexScopeSlider_val;

        public float FourxScopeSlider_val;
        public float SixxScopeSlider_val;

        public float CameraSensivitySlider_val;
        public float CameraParachutingSlider_val;

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


