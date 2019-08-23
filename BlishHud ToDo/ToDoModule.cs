using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Entities;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToDoModule {

    [Export(typeof(Module))]
    public class ToDoModule : Module {

        internal static ToDoModule ModuleInstance;

        // Service Managers
        internal SettingsManager SettingsManager => this.ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => this.ModuleParameters.Gw2ApiManager;

        private WindowTab _todoTab;
        private Panel _tabPanel;

        private Texture2D     _mugTexture;

        // Controls (be sure to dispose of these in Unload()
        private CornerIcon       _exampleIcon;

        private const string EC_ALLEVENTS = "All Tasks";

        private List<DetailsButton> _displayedTasks;
        /// <summary>
        /// Ideally you should keep the constructor as is.
        /// Use <see cref="Initialize"/> to handle initializing the module.
        /// </summary>
        [ImportingConstructor]
        public ToDoModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; }

        /// <summary>
        /// Define the settings you would like to use in your module.  Settings are persistent
        /// between updates to both Blish HUD and your module.
        /// </summary>
        protected override void DefineSettings(SettingCollection settings) {

        }

        /// <summary>
        /// Allows your module to perform any initialization it needs before starting to run.
        /// Please note that Initialize is NOT asynchronous and will block Blish HUD's update
        /// and render loop, so be sure to not do anything here that takes too long.
        /// </summary>
        protected override void Initialize()
        {
            _displayedTasks = new List<DetailsButton>();
        }

        /// <summary>
        /// Load content and more here. This call is asynchronous, so it is a good time to
        /// run any long running steps for your module. Be careful when instancing
        /// <see cref="Blish_HUD.Entities.Entity"/> and <see cref="Blish_HUD.Controls.Control"/>.
        /// Setting their parent is not thread-safe and can cause the application to crash.
        /// You will want to queue them to add later while on the main thread or in a delegate queued
        /// with <see cref="Blish_HUD.DirectorService.QueueMainThreadUpdate(Action{GameTime})"/>.
        /// </summary>
        protected override async Task LoadAsync() {

            tdTask.Load();
            // Load content from the ref directory automatically with the ContentsManager
            _mugTexture = ContentsManager.GetTexture(@"textures\603447.png");

            _tabPanel = BuildHomePanel(GameService.Overlay.BlishHudWindow);
            


            // var Dailies = await Gw2ApiManager.Gw2ApiClient.DailyCrafting.AllAsync();
        }

        /// <summary>
        /// Allows you to perform an action once your module has finished loading (once
        /// <see cref="LoadAsync"/> has completed).  You must call "base.OnModuleLoaded(e)" at the
        /// end for the <see cref="ExternalModule.ModuleLoaded"/> event to fire and for
        /// <see cref="ExternalModule.Loaded" /> to update correctly.
        /// </summary>
        protected override void OnModuleLoaded(EventArgs e) {
            _todoTab = GameService.Overlay.BlishHudWindow.AddTab("To-Do", _mugTexture, _tabPanel);

            // Add a mug icon in the top left next to the other icons
            _exampleIcon = new CornerIcon() {
                Icon             = _mugTexture,
                BasicTooltipText = $"{this.Name} [{this.Namespace}]",
                Parent           = GameService.Graphics.SpriteScreen
            };

            base.OnModuleLoaded(e);
        }
        private Panel BuildHomePanel(WindowBase wndw)
        {

            var tdPanel = new Panel()
            {
                CanScroll = false,
                Size = wndw.ContentRegion.Size
                
            };

            
            int topOffset = 40 + Panel.MenuStandard.ControlOffset.Y;

            var menuSection = new Panel
            {
                Title = "Event Categories",
                ShowBorder = true,
                Size = Panel.MenuStandard.Size - new Point(0, + topOffset + Panel.MenuStandard.ControlOffset.Y),
                Location = new Point(Panel.MenuStandard.PanelOffset.X, topOffset),
                Parent = tdPanel
            };
            var mainPanel = new FlowPanel()
            {
                FlowDirection = ControlFlowDirection.LeftToRight,
                Location = new Point(menuSection.Right + Panel.MenuStandard.ControlOffset.X, menuSection.Top),
                Size = new Point(tdPanel.Right - menuSection.Right - (Control.ControlStandard.ControlOffset.X * 2), menuSection.Height),
                CanScroll = false,
                Parent = tdPanel,
            };
            var taskPanel = new FlowPanel()
            {
                FlowDirection = ControlFlowDirection.LeftToRight,
                ControlPadding = new Vector2(8,8),
                //Location = new Point(menuSection.Right + Panel.MenuStandard.ControlOffset.X, menuSection.Top),
                Size = new Point(tdPanel.Right - menuSection.Right - (Control.ControlStandard.ControlOffset.X *2), menuSection.Height - 70),
                CanScroll = true,
                Parent = mainPanel,
                
            };

            GameService.Overlay.QueueMainThreadUpdate((gameTime) => {
                var searchBox = new TextBox()
                {
                    PlaceholderText = "Filter Tasks",
                    Width = menuSection.Width,
                    Location = new Point(menuSection.Left, TextBox.Standard.ControlOffset.Y),
                    Parent = tdPanel
                };

                searchBox.TextChanged += delegate (object sender, EventArgs args) {
                    taskPanel.FilterChildren<DetailsButton>(db => db.Text.ToLower().Contains(searchBox.Text.ToLower()));
                };
            });

            foreach (var task in tdTask.Tasks)
            {

                var es2 = new DetailsButton
                {
                    Parent = taskPanel,
                    BasicTooltipText = task.Category,
                    Text = task.Description,
                    IconSize = DetailsIconSize.Small,
                    ShowVignette = false,
                    HighlightType = DetailsHighlightType.LightHighlight,
                    ShowToggleButton = true
                    
                };

                if (task.Texture.HasTexture)
                {
                    es2.Icon = task.Texture;
                }
                _displayedTasks.Add(es2);
            }

            var actionPanel = new FlowPanel()
            {
                Size = new Point(taskPanel.Width, 50),
                CanScroll = false,
                FlowDirection = ControlFlowDirection.LeftToRight,
                ShowBorder = false,
                Parent = mainPanel,
                ControlPadding = new Vector2(8, 8)

                //BackgroundColor = Microsoft.Xna.Framework.Color.Black

            };
            var newTaskButton = new StandardButton
            {
                Parent = actionPanel,
                Text = "New Task"
            };

            //Create new panel to switch to
            var newtaskPanel = BuildNewTaskPanel(wndw);
            newTaskButton.LeftMouseButtonReleased += delegate { wndw.Navigate(newtaskPanel, true); };

            //Add categories
            var taskCategories = new Menu
            {
                Size = menuSection.ContentRegion.Size,
                MenuItemHeight = 40,
                Parent = menuSection,
                CanSelect = true
            };

            List<IGrouping<string, tdTask>> submetas = tdTask.Tasks.GroupBy(e => e.Category).ToList();

            var evAll = taskCategories.AddMenuItem(EC_ALLEVENTS);
            evAll.Select();
            evAll.Click += delegate {
                taskPanel.FilterChildren<DetailsButton>(db => true);
            };
            foreach (IGrouping<string, tdTask> e in submetas)
            {
                var ev = taskCategories.AddMenuItem(e.Key);
                ev.Click += delegate {
                    taskPanel.FilterChildren<DetailsButton>(db => string.Equals(db.BasicTooltipText, e.Key));
                };

                
            }
            return tdPanel;
        }

        private Panel BuildNewTaskPanel(WindowBase wndw)
        {
            var newTaskPanel = new Panel()
            {
                CanScroll = false,
                Size = wndw.ContentRegion.Size,
            };
            var backButton = new BackButton(wndw)
            {
                Text = "Tasks",
                NavTitle = "Home",
                Parent = newTaskPanel,
                Location = new Point(20, 20),
            };


            return newTaskPanel;
        }

        private void RepositionES()
        {
            int pos = 0;
            foreach (var es in _displayedTasks)
            {
                int x = pos % 2;
                int y = pos / 2;

                es.Location = new Point(x * 308, y * 108);

                if (es.Visible) pos++;

                // TODO: Just expose the panel to the module so that we don't have to do it this dumb way:
                ((Panel)es.Parent).VerticalScrollOffset = 0;
                es.Parent.Invalidate();
            }
        }

        /// <summary>
        /// Allows your module to run logic such as updating UI elements,
        /// checking for conditions, playing audio, calculating changes, etc.
        /// This method will block the primary Blish HUD loop, so any long
        /// running tasks should be executed on a separate thread to prevent
        /// slowing down the overlay.
        /// </summary>
        protected override void Update(GameTime gameTime) {

        }

        /// <summary>
        /// For a good module experience, your module should clean up ANY and ALL entities
        /// and controls that were created and added to either the World or SpriteScreen.
        /// Be sure to remove any tabs added to the Director window, CornerIcons, etc.
        /// </summary>
        protected override void Unload() {
            ModuleInstance = null;
            GameService.Overlay.BlishHudWindow.RemoveTab(_todoTab);
            _exampleIcon.Dispose();
            _displayedTasks.ForEach(de => de.Dispose());
            _displayedTasks.Clear();
        }

    }

}
