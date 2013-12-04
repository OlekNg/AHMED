using BuildingEditor.ViewModel.Tools;
using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BuildingEditor.ViewModel
{
    /// <summary>
    /// View model for building editor's main window.
    /// </summary>
    [ImplementPropertyChanged]
    public class Editor : ISegmentEventHandler
    {
        private FrameworkElement _workspace;
        private FrameworkElement _window;
        private Tool _lastTool;

        #region Editor construction
        public Editor(FrameworkElement workspace, FrameworkElement window)
        {
            _workspace = workspace;
            _window = window;

            CreateCommands();
            CreateToolbox();

            SegmentEventHandler.Register(this);
        }

        protected void CreateCommands()
        {
            ExpandCommand = new SimpleCommand(x => Expand((Direction)x));
            NewCommand = new SimpleCommand(x => NewBuilding());
            AddFloorCommand = new SimpleCommand(x => AddFloor());
        }

        private void CreateToolbox()
        {
            Tools = new ObservableCollection<Tool>();
            Tools.Add(new DragTool(_workspace, _window));
            Tools.Add(new FloorTool(this));
            Tools.Add(new SideElementTool(this, SideElementType.WALL, "Wall"));
            Tools.Add(new SideElementTool(this, SideElementType.DOOR, "Door") { Capacity = 5 });
            Tools.Add(new PeopleTool());
            Tools.Add(new DeleteTool(this));
            Tools.Add(new StairsTool(this));

            CurrentTool = Tools[0];
        }
        #endregion

        #region Commands
        public ICommand ExpandCommand { get; set; }
        public ICommand NewCommand { get; set; }
        public ICommand AddFloorCommand { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Currently edited building.
        /// </summary>
        public Building CurrentBuilding { get; set; }

        /// <summary>
        /// Path to currently open building file.
        /// </summary>
        public string CurrentFile { get; set; }

        /// <summary>
        /// Currently chosen tool.
        /// </summary>
        public Tool CurrentTool { get; set; }

        /// <summary>
        /// Toolbox.
        /// </summary>
        public ObservableCollection<Tool> Tools { get; set; }
        #endregion

        #region Actions
        public void AddFloor()
        {
            CurrentBuilding.AddFloor();
        }

        public void Expand(Direction direction)
        {
            CurrentBuilding.CurrentFloor.Expand(direction);
        }

        /// <summary>
        /// Loads building definition from given xml file.
        /// </summary>
        /// <param name="file">Path to building definition file.</param>
        public void LoadBuilding(string file)
        {
            CurrentFile = file;
            Common.DataModel.Building building = new Common.DataModel.Building();
            building.Load(file);
            CurrentBuilding = new Building(building);
        }

        /// <summary>
        /// Creates new building.
        /// </summary>
        public void NewBuilding()
        {
            CurrentFile = "";
            CurrentBuilding = new Building(5, 5);
        }

        public void RemoveFloor(int index)
        {
            CurrentBuilding.RemoveFloor(index);
        }

        /// <summary>
        /// Saves building to currently open file.
        /// </summary>
        public void SaveBuilding()
        {
            SaveBuilding(CurrentFile);
        }

        /// <summary>
        /// Saves currently edited building to given file location.
        /// </summary>
        /// <param name="file">Destination file.</param>
        public void SaveBuilding(string file)
        {
            CurrentFile = file;
            Common.DataModel.Building building = CurrentBuilding.ToDataModel();
            building.Save(file);
        }
        #endregion


        #region Segment events handlers.
        public void Segment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentTool.MouseDown(sender, e);
        }

        public void Segment_MouseMove(object sender, MouseEventArgs e)
        {
            CurrentTool.MouseMove(sender, e);
        }

        public void Segment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentTool.MouseUp(sender, e);
        }

        public void Segment_MouseEnter(object sender, MouseEventArgs e)
        {
            CurrentTool.MouseEnter(sender, e);
        }

        public void Segment_MouseLeave(object sender, MouseEventArgs e)
        {
            CurrentTool.MouseLeave(sender, e);
        }


        #endregion

        #region Workspace events handlers.
        /// <summary>
        /// Performs zoom in/out of the building.
        /// </summary>
        public void Workspace_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            CurrentTool.MouseWheel(sender, e);
        }

        public void Workspace_MouseLeave(object sender, MouseEventArgs e)
        {
            CurrentTool.ClearPreview();
        }

        public void Toolbox_Selected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CurrentBuilding.ViewMode = CurrentTool.Name;
            if (_lastTool != null) _lastTool.CancelAction();
            _lastTool = CurrentTool;
        }
        #endregion
    }
}
