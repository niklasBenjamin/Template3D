using Godot;

namespace InputSystem
{
    public static class InputMapper 
    {
        public static void MapFromConfig(ConfigFile config, string section1, string section2 = "") {
            foreach (string actionEventName in config.GetSectionKeys(section1)) {

                //Remove existing inputactions for action "actionEventName"
                var actionList = InputMap.GetActionList(actionEventName);

                if(actionList.Count != 0) 
                    InputMap.EraseAction(actionEventName);

                InputMap.AddAction(actionEventName);

                if(config.HasSectionKey(section1, actionEventName)) {
                    var newInputEventKey = new InputEventKey();
                    
                    newInputEventKey.Scancode = (uint)(int) config?.GetValue(section1, actionEventName, null);
                    InputMap.ActionAddEvent(actionEventName, newInputEventKey);
                }
                
                if(config.HasSectionKey(section2, actionEventName)) {
                    var newInputEventKey = new InputEventKey();
                    
                    newInputEventKey.Scancode = (uint)(int) config?.GetValue(section2, actionEventName, null);
                    InputMap.ActionAddEvent(actionEventName, newInputEventKey);
                }
            }
        }
    }
}
