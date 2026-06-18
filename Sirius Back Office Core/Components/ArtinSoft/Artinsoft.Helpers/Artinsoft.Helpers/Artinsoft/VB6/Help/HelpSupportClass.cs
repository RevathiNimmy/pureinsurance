using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Artinsoft.VB6.Help
{
    /// <summary>
    /// This class provides support for Help feature.
    /// </summary>
    public class HelpSupportClass
    {
        /// <summary>
        /// Help Ignore Restrictions Enum
        /// </summary>
        public enum HelpIgnoreResctrictionsEnum
        {
            /// <summary>
            /// MDI Container Restriction
            /// </summary>
            MDIContainerRestriction = 1
        }

        /// <summary>
        /// Name of the event to patch.
        /// </summary>
        private static readonly string HelpRequestedEvent = "HelpRequested";
        private static OpenFileDialog openFileDialog = new OpenFileDialog();

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static HelpSupportClass()
        {
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = Artinsoft.VB6.Resources.Artinsoft_VB6.Artinsoft_VB6_Help_HelpSupportClass_ValidateHelpFile_OpenDialog_Filter;
        }

        /// <summary>
        /// List of the HelpRequested event handlers patched by control.
        /// </summary>
        private static Dictionary<Control, List<Delegate>> PatchedHelpRequested = new Dictionary<Control, List<Delegate>>();
        private static Dictionary<Control, List<HelpIgnoreResctrictionsEnum>> restrictionsToIgnore = new Dictionary<Control, List<HelpIgnoreResctrictionsEnum>>();

        /// <summary>
        /// Add a flag to ignore restrictions for a control.
        /// </summary>
        /// <param name="ctrl">The control to aply the rule to ignore the restriction.</param>
        /// <param name="restriction">The restriction that should be ignored.</param>
        public static void SetIgnoreResctriction(Control ctrl, HelpIgnoreResctrictionsEnum restriction)
        {
            if (!restrictionsToIgnore.ContainsKey(ctrl))
                restrictionsToIgnore.Add(ctrl, new List<HelpIgnoreResctrictionsEnum>());

            if (!restrictionsToIgnore[ctrl].Contains(restriction))
                restrictionsToIgnore[ctrl].Add(restriction);
        }

        private static HelpProvider helpProvider = new HelpProvider();
        /// <summary>
        /// Get/Set HelpNamespace value
        /// </summary>
        public static string HelpFile
        {
            get
            {
                return (string.IsNullOrEmpty(helpProvider.HelpNamespace) ? string.Empty : helpProvider.HelpNamespace);
            }
            set
            {
                helpProvider.HelpNamespace = value;
            }
        }
        
        /// <summary>
        /// Sets the Help Context Id to the control
        /// </summary>
        /// <param name="ctrl">Control to set the help id</param>
        /// <param name="HelpId">Help Id index</param>
        public static void SetHelpContextId(Control ctrl, long HelpId)
        {
            SetHelpContextId(ctrl, HelpId, HelpNavigator.TopicId);
        }

        /// <summary>
        /// Sets the Help Context Id to the control
        /// </summary>
        /// <param name="ctrl">Control to set the help id</param>
        /// <param name="HelpId">Help Id index</param>
        /// <param name="hNavigator">One of the HelpNavigator values to set</param>
        public static void SetHelpContextId(Control ctrl, long HelpId, HelpNavigator hNavigator)
        {
            helpProvider.SetHelpKeyword(ctrl, HelpId.ToString());
            SetHelpNavigator(ctrl, hNavigator);
        }
        /// <summary>
        /// Returns the Help Id key from the control
        /// </summary>
        /// <param name="ctrl">Control to search the help id</param>
        /// <returns></returns>
        public static long GetHelpContextId(Control ctrl)
        {
            return long.Parse(helpProvider.GetHelpKeyword(ctrl));
        }

        /// <summary>
        /// Sets the Help Navigator value to the control
        /// </summary>
        /// <param name="ctrl">Control to set the help navigator</param>
        /// <param name="hNavigator">One of the HelpNavigator values</param>
        public static void SetHelpNavigator(Control ctrl, HelpNavigator hNavigator)
        {
            RestoreHelpEventHandler(ctrl);
            helpProvider.SetHelpNavigator(ctrl, hNavigator);
            PatchHelpEventHandler(ctrl);
        }

        /// <summary>
        /// It will clean the internal dictionaries from old references of controls alreay disposed.
        /// </summary>
        private static void CleanDeadReferences()
        {
            try
            {
                List<Control> toClean = new List<Control>();
                foreach (Control ctrl in PatchedHelpRequested.Keys)
                {
                    if (ctrl.IsDisposed)
                        toClean.Add(ctrl);
                }
                foreach (Control ctrl in toClean)
                {
                    PatchedHelpRequested.Remove(ctrl);
                }
            }
            catch { }
        }


        /// <summary>
        /// Replace the HelpRequested event handler placed by the helpProvider with 
        /// a custom event handler so we can catch when the user is requesting help.
        /// </summary>
        /// <param name="ctrl">The source control.</param>
        private static void PatchHelpEventHandler(Control ctrl)
        {
            CleanDeadReferences();

            if (PatchedHelpRequested.ContainsKey(ctrl))
                throw new InvalidOperationException(HelpRequestedEvent + " event for this control has been previously patched: '" + ctrl.Name + "'");

            Delegate[] EventDelegates = Artinsoft.VB6.Gui.ContainerHelper.GetEventSubscribers(ctrl, HelpRequestedEvent);

            if (EventDelegates != null)
            {
                EventInfo eInfo = typeof(Control).GetEvent(HelpRequestedEvent);
                if (eInfo == null)
                    throw new InvalidOperationException("Event info for event '" + HelpRequestedEvent + "' could not be found");

                PatchedHelpRequested.Add(ctrl, new List<Delegate>());

                foreach (Delegate del in EventDelegates)
                {
                    PatchedHelpRequested[ctrl].Add(del);
                    eInfo.RemoveEventHandler(ctrl, del);
                }

                ctrl.HelpRequested += new HelpEventHandler(Control_HelpRequested);
            }
        }

        /// <summary>
        /// Restore the HelpRequested event handler that was originally added by 
        /// the helpProvider if one was previously patched.
        /// </summary>
        /// <param name="ctrl">The source control.</param>
        private static void RestoreHelpEventHandler(Control ctrl)
        {
            if (PatchedHelpRequested.ContainsKey(ctrl))
            {
                ctrl.HelpRequested -= new HelpEventHandler(Control_HelpRequested);

                EventInfo eInfo = typeof(Control).GetEvent(HelpRequestedEvent);
                if (eInfo == null)
                    throw new InvalidOperationException("Event info for event '" + HelpRequestedEvent + "' could not be found");

                foreach (Delegate del in PatchedHelpRequested[ctrl])
                {
                    eInfo.AddEventHandler(ctrl, del);
                }

                PatchedHelpRequested.Remove(ctrl);
            }
        }

        /// <summary>
        /// Custom event handler used to patch the HelpRequested event of 
        /// the controls controlled by  the help provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hlpevent"></param>
        private static void Control_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Control ctrl = (Control)sender;

            //The MDIForms do not show help in VB6
            if ((ctrl is Form) && (((Form)ctrl).IsMdiContainer) 
                && ((!restrictionsToIgnore.ContainsKey(ctrl)) || (restrictionsToIgnore.ContainsKey(ctrl) && !restrictionsToIgnore[ctrl].Contains(HelpIgnoreResctrictionsEnum.MDIContainerRestriction))))
                return;

            if (PatchedHelpRequested.ContainsKey(ctrl) && ValidateHelpFile())
            {
                foreach (Delegate del in PatchedHelpRequested[ctrl])
                {
                    del.DynamicInvoke(new object[] { sender, hlpevent });
                }
            }
        }

        /// <summary>
        /// Validates that HelpFile exists.
        /// </summary>
        /// <returns></returns>
        private static bool ValidateHelpFile()
        {
            string helpPath = string.Empty;
            if (!string.IsNullOrEmpty(HelpFile))
            {
                helpPath = System.IO.Path.GetFullPath(HelpFile);

                if (!System.IO.Path.HasExtension(helpPath))
                    helpPath = helpPath + ".chm";

                if (!System.IO.File.Exists(helpPath))
                {
                    if ((MessageBox.Show(string.Format(Artinsoft.VB6.Resources.Artinsoft_VB6.Artinsoft_VB6_Help_HelpSupportClass_ValidateHelpFile_Question, helpPath),
                        Artinsoft.VB6.Resources.Artinsoft_VB6.Artinsoft_VB6_Help_HelpSupportClass_ValidateHelpFile_Question_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        && (openFileDialog.ShowDialog() == DialogResult.OK))
                    {
                        HelpFile = openFileDialog.FileName;

                        return true;
                    }

                    MessageBox.Show(string.Format(Artinsoft.VB6.Resources.Artinsoft_VB6.Artinsoft_VB6_Help_HelpSupportClass_ValidateHelpFile_ValidationFailure, helpPath),
                        Artinsoft.VB6.Resources.Artinsoft_VB6.Artinsoft_VB6_Help_HelpSupportClass_ValidateHelpFile_Question_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return false;
                }
                else
                    return true;
            }
            else
                return false;
        }
    }
}
