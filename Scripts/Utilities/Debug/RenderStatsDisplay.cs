using Godot;
using Utilities.Console;

namespace Utilities.Debug.Stats
{
    public class RenderStatsDisplay : Control
    {
        [Export] private NodePath currentFpsLabelPath;
        private Label currentFpsLabel;

        [Export] private NodePath averageFpsLabelPath;
        private Label averageFpsLabel;

        [Export] private NodePath minFpsLabelPath;
        private Label minFpsLabel;

        [Export] private NodePath staticMemoryLabelPath;
        private Label staticMemoryLabel;

        [Export] private NodePath dynamicMemoryLabelPath;
        private Label dynamicMemoryLabel;

        [Export] private NodePath objectCountLabelPath;
        private Label objectCountLabel;

        [Export] private NodePath objectsInFrameLabelPath;
        private Label objectsInFrameLabel;

        [Export] private NodePath verticesInFrameLabelPath;
        private Label verticesInFrameLabel;

        [Export] private NodePath drawCalls3DLabelPath;
        private Label drawCalls3DLabel;

        [Export] private NodePath drawCalls2DLabelPath;
        private Label drawCalls2DLabel;

        private int frameRange = 60;
        private int frameRangeSum;
        private int frameRangeIndex;
        private int frameRangeMin = int.MaxValue;

        private Timer timer;
        

        public override void _EnterTree() {
            Shell.RegisterCommand("Stats.SetActive", this, nameof(SetActive), "Ladida");
        }

        public override void _ExitTree() {
            Shell.UnregisterCommand("Stats.SetActive");
        }

        public override void _Ready() {
            currentFpsLabel      = GetNode<Label>(currentFpsLabelPath);
            averageFpsLabel      = GetNode<Label>(averageFpsLabelPath);
            minFpsLabel          = GetNode<Label>(minFpsLabelPath);
            staticMemoryLabel    = GetNode<Label>(staticMemoryLabelPath);
            dynamicMemoryLabel   = GetNode<Label>(dynamicMemoryLabelPath);
            objectCountLabel     = GetNode<Label>(objectCountLabelPath);
            objectsInFrameLabel  = GetNode<Label>(objectsInFrameLabelPath);
            verticesInFrameLabel = GetNode<Label>(verticesInFrameLabelPath);
            drawCalls3DLabel     = GetNode<Label>(drawCalls3DLabelPath);
            drawCalls2DLabel     = GetNode<Label>(drawCalls2DLabelPath);

            timer = new Timer();
            AddChild(timer);

            timer.Connect("timeout", this, nameof(UpdateStats));
            timer.WaitTime = 0.5f;
            timer.OneShot = false;
            timer.Start();
        }

        private void UpdateStats() {
            CalculateFPS();

            staticMemoryLabel.Text = $"Static memory : {(int) (Performance.GetMonitor(Performance.Monitor.MemoryStatic) / 1024) / 1024} MiB";
            dynamicMemoryLabel.Text = $"Dynamic memory : {Performance.GetMonitor(Performance.Monitor.MemoryDynamic)} B";
            objectCountLabel.Text = $"Object count : {Performance.GetMonitor(Performance.Monitor.ObjectCount)}";
            objectsInFrameLabel.Text = $"Objects in frame : {Performance.GetMonitor(Performance.Monitor.RenderObjectsInFrame)}";
            verticesInFrameLabel.Text = $"Vertices in frame : {Performance.GetMonitor(Performance.Monitor.RenderVerticesInFrame)}";
            drawCalls3DLabel.Text = $"Draw calls 3D : {Performance.GetMonitor(Performance.Monitor.RenderDrawCallsInFrame)}";
            drawCalls2DLabel.Text = $"Draw calls 2D : {Performance.GetMonitor(Performance.Monitor.Render2dDrawCallsInFrame)}";
    
        }

        private void CalculateFPS() {
            int fps = Mathf.RoundToInt(Performance.GetMonitor(Performance.Monitor.TimeFps));
            currentFpsLabel.Text = $"FPS : {fps}";
            
            frameRangeSum += fps;

            if(fps < frameRangeMin) {
                frameRangeMin = fps;
                minFpsLabel.Text = $"Min FPS : {frameRangeMin}";
            }

            if(frameRangeIndex++ >= frameRange) {
                averageFpsLabel.Text = $"Average FPS : {Mathf.FloorToInt(frameRangeSum / frameRange)}";
                frameRangeIndex = frameRangeSum = 0;
                frameRangeMin = int.MaxValue;
            }
        }

        public void SetActive(bool state) {
            this.Visible = state;

            if(state)
                timer.Start();
            else
                timer.Stop();
        }
    }
}