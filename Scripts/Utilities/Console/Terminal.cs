using Godot;

namespace Utilities.Console
{
    public class Terminal : Control
    {
        [Export]
        private NodePath containerPath;
        private VBoxContainer container;

        [Export]
        private NodePath inputFieldPath;
        private LineEdit inputField;

        [Export]
        private NodePath autoCompleteLabelPath;
        private Label autoCompleteLabel;

        public override void _Process(float delta) {
            if(Godot.Input.IsActionJustPressed("tilde")) {
                Visible = !Visible;

                if(Visible) {
                    inputField.Text = string.Empty;
                    ResetInputFocus();
                }
            }
            else if(Input.IsActionJustPressed("ui_focus_next")){
                SetInputFromAutoComplete();
            }
            else if(Input.IsActionJustPressed("ui_up")) {
                SetInputFromLog(true);
            }
            else if(Input.IsActionJustPressed("ui_down")) {
                SetInputFromLog(false);   
            }
        }

        public override void _Ready() {
            inputField = GetNode<LineEdit>(inputFieldPath);
            container = GetNode<VBoxContainer>(containerPath);
            autoCompleteLabel = GetNode<Label>(autoCompleteLabelPath);

            inputField.Connect("text_entered", this, nameof(OnTextEntered));
            inputField.Connect("text_changed", this, nameof(OnTextChanged));
        }

        private void SetInputFromLog(bool up) {
            string log = Shell.GetCommandLog(up);

            if(!string.IsNullOrEmpty(log)) {
                inputField.Text = log;
            }
            ResetInputFocus();
        }

        private void SetInputFromAutoComplete() {
            inputField.Text = autoCompleteLabel.Text;
            ResetInputFocus();
        }

        private void OnTextEntered(string text) {
            if(string.IsNullOrWhiteSpace(text))
                return;

            RichTextLabel label = new RichTextLabel();
            container.AddChild(label);
            
            label.FitContentHeight = true;
            label.BbcodeEnabled = true;
            label.BbcodeText = Shell.ParseCommand(text);

            inputField.Text = string.Empty;
            ResetInputFocus();
        }

        private void OnTextChanged(string text) {
            autoCompleteLabel.Text = Shell.GetAutoCompleteText(text);
        }

        private void ResetInputFocus() {
            inputField.GrabFocus();
            inputField.CaretPosition = inputField.Text.Length;
        }
    }
}
