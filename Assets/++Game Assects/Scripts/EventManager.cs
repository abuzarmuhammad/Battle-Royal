public static class EventManager
{
    public delegate void PanelOpenDelegate(PanelType _type);
    public static PanelOpenDelegate OnPanelOpenClicked;
    
    public delegate void PanelCloseDelegate();
    public static PanelCloseDelegate OnPanelCloseClicked;
    
    
    public delegate void EnterTriggerDelegate(bool _entered);
    public static EnterTriggerDelegate OnEnterTrigger_LootBox;


}
