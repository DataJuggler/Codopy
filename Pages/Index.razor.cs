

#region using statements

using DataJuggler.Blazor.Components;
using DataJuggler.Blazor.Components.Interfaces;
using DataJuggler.Blazor.Components.Util;
using DataJuggler.UltimateHelper;
using DataJuggler.UltimateHelper.Enumerations;
using Microsoft.JSInterop;
using Timer = System.Timers;

#endregion

namespace Codopy.Pages
{

    #region class Index
    /// <summary>
    /// This class is the main page for this app
    /// </summary>
    public partial class Index : IBlazorComponentParent
    {
        
        #region Private Variables
        private List<IBlazorComponent> children;
        private ComboBox comboBox;
        private ValidationComponent sourceControl;
        private ValidationComponent resultsControl;
        private LanguageEnum language;
        private Timer.Timer timer;
        private string checkVisibility;
        private string checkMarkClassName;
        private string statusMessage;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Create a new instance of an 'Index' object.
        /// </summary>
        public Index()
        {
            // default to invisible
            CheckVisibility = "hidden";
        }
        #endregion
        
        #region Events
            
            #region TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
            /// <summary>
            /// event is fired when Timer Elapsed
            /// </summary>
            private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                // destroy
                Timer.Dispose();
                
                // hide
                CheckVisibility = "hidden";
                
                // update the UI
                Refresh();
            }
            #endregion
            
        #endregion
        
        #region Methods
            
            #region Copy()
            /// <summary>
            /// Copy
            /// </summary>
            public async void Copy()
            {
                // if the value for HasResultsControl is true
                if (HasResultsControl)
                {
                    // format the code
                    string formatted = ResultsControl.Text;
                    
                    // Copy
                    await BlazorJSBridge.CopyToClipboard(JSRuntime, formatted);
                    
                    // set to visible
                    CheckVisibility = "visible";
                    
                    // Update the UI
                    Refresh();
                    
                    // Start the timer
                    Timer = new Timer.Timer(3000);
                    Timer.Elapsed += TimerElapsed;
                    Timer.Start();
                }
            }
            #endregion
            
            #region FindChildByName(string name)
            /// <summary>
            /// method returns the Child By Name
            /// </summary>
            public IBlazorComponent FindChildByName(string name)
            {
                // return the child name
                return ComponentHelper.FindChildByName(Children, name);
            }
            #endregion
            
            #region Format()
            /// <summary>
            /// Format
            /// </summary>
            public void Format()
            {
                // if the value for HasSourceControl is true
                if ((HasSourceControl) && (HasResultsControl))
                {
                    // get the formatted Text
                    string formattedText = CodeHelper.FormatCode(SourceControl.Text, Language);
                    
                    // Set the formattedText
                    ResultsControl.SetTextValue(formattedText);
                }
            }
            #endregion
            
            #region ReceiveData(Message message)
            /// <summary>
            /// method returns the Data
            /// </summary>
            public void ReceiveData(Message message)
            {
                if (NullHelper.Exists(message, ComboBox))
                {
                    // if this is the combobox
                    if (message.Sender.Name == "ComboBoxControl")
                    {  
                        // if Python
                        if (message.Text == "Python")
                        {
                            // Set to Python
                            Language = LanguageEnum.Python;

                            // Show the user a message
                            StatusMessage = "Python is not supported yet.";                            
                        }
                        else
                        {
                            // default to C#
                            Language = LanguageEnum.CSharp;
                            StatusMessage = "";
                        }

                        // Update the UI
                        Refresh();

                        // Reset to C#
                        ComboBox.SetSelectedItem(message.Text);

                        // Update the UI
                        ComboBox.Refresh();
                    }
                }
            }
            #endregion
            
            #region Refresh()
            /// <summary>
            /// method Refresh
            /// </summary>
            public void Refresh()
            {
                // Update the UI
                InvokeAsync(() =>
                {
                    StateHasChanged();
                    });
                }
                #endregion
                
                #region Register(IBlazorComponent component)
                /// <summary>
                /// method returns the
                /// </summary>
                public void Register(IBlazorComponent component)
                {
                    // If the component object exists
                    if (NullHelper.Exists(component))
                    {
                        // if this is a ComboBox
                        if (component is ComboBox)
                        {
                            // register the ComboBox
                            ComboBox = component as ComboBox;
                            
                            // Load the items
                            ComboBox.LoadItems(typeof(LanguageEnum));
                            
                            // Select C# By Default
                            ComboBox.SetSelectedItem("CSharp");
                        }
                        else if (component is ValidationComponent)
                        {
                            // if the SourceControl
                            if (TextHelper.IsEqual(component.Name, "SourceControl"))
                            {
                                // register the SourceControl
                                SourceControl = component as ValidationComponent;
                            }
                            else if (TextHelper.IsEqual(component.Name, "ResultsControl"))
                            {
                                // register the ResultsControl
                                ResultsControl = component as ValidationComponent;
                            }
                        }
                    }
                }
                #endregion
                
            #endregion
            
            #region Properties
                
                #region CheckMarkClassName
                /// <summary>
                /// This property gets or sets the value for 'CheckMarkClassName'.
                /// </summary>
                public string CheckMarkClassName
                {
                    get { return checkMarkClassName; }
                    set { checkMarkClassName = value; }
                }
                #endregion
                
                #region CheckVisibility
                /// <summary>
                /// This property gets or sets the value for 'CheckVisibility'.
                /// </summary>
                public string CheckVisibility
                {
                    get { return checkVisibility; }
                    set { checkVisibility = value; }
                }
                #endregion
                
                #region Children
                /// <summary>
                /// This property gets or sets the value for 'Children'.
                /// </summary>
                public List<IBlazorComponent> Children
                {
                    get { return children; }
                    set { children = value; }
                }
                #endregion
                
                #region ComboBox
                /// <summary>
                /// This property gets or sets the value for 'ComboBox'.
                /// </summary>
                public ComboBox ComboBox
                {
                    get { return comboBox; }
                    set { comboBox = value; }
                }
                #endregion
                
                #region HasChildren
                /// <summary>
                /// This property returns true if this object has a 'Children'.
                /// </summary>
                public bool HasChildren
                {
                    get
                    {
                        // initial value
                        bool hasChildren = (this.Children != null);
                        
                        // return value
                        return hasChildren;
                    }
                }
                #endregion
                
                #region HasComboBox
                /// <summary>
                /// This property returns true if this object has a 'ComboBox'.
                /// </summary>
                public bool HasComboBox
                {
                    get
                    {
                        // initial value
                        bool hasComboBox = (this.ComboBox != null);
                        
                        // return value
                        return hasComboBox;
                    }
                }
                #endregion
                
                #region HasResultsControl
                /// <summary>
                /// This property returns true if this object has a 'ResultsControl'.
                /// </summary>
                public bool HasResultsControl
                {
                    get
                    {
                        // initial value
                        bool hasResultsControl = (this.ResultsControl != null);
                        
                        // return value
                        return hasResultsControl;
                    }
                }
                #endregion
                
                #region HasSourceControl
                /// <summary>
                /// This property returns true if this object has a 'SourceControl'.
                /// </summary>
                public bool HasSourceControl
                {
                    get
                    {
                        // initial value
                        bool hasSourceControl = (this.SourceControl != null);
                        
                        // return value
                        return hasSourceControl;
                    }
                }
                #endregion
                
                #region HasTimer
                /// <summary>
                /// This property returns true if this object has a 'Timer'.
                /// </summary>
                public bool HasTimer
                {
                    get
                    {
                        // initial value
                        bool hasTimer = (this.Timer != null);
                        
                        // return value
                        return hasTimer;
                    }
                }
                #endregion
                
                #region Language
                /// <summary>
                /// This property gets or sets the value for 'Language'.
                /// </summary>
                public LanguageEnum Language
                {
                    get { return language; }
                    set { language = value; }
                }
                #endregion
                
                #region ResultsControl
                /// <summary>
                /// This property gets or sets the value for 'ResultsControl'.
                /// </summary>
                public ValidationComponent ResultsControl
                {
                    get { return resultsControl; }
                    set { resultsControl = value; }
                }
                #endregion
                
                #region SourceControl
                /// <summary>
                /// This property gets or sets the value for 'SourceControl'.
                /// </summary>
                public ValidationComponent SourceControl
                {
                    get { return sourceControl; }
                    set { sourceControl = value; }
                }
                #endregion
                
                #region StatusMessage
                /// <summary>
                /// This property gets or sets the value for 'StatusMessage'.
                /// </summary>
                public string StatusMessage
                {
                    get { return statusMessage; }
                    set { statusMessage = value; }
                }
                #endregion
            
                #region Timer
                /// <summary>
                /// This property gets or sets the value for 'Timer'.
                /// </summary>
                public Timer.Timer Timer
                {
                    get { return timer; }
                    set { timer = value; }
                }
                #endregion
                
            #endregion
            
        }
        #endregion
    
    }
