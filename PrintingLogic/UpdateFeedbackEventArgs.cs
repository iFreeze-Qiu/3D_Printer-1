using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingLogic
{
    /// <summary>
    /// The string to send to the feedback textbox in 
    /// the GUI layer.
    /// </summary>
    public class UpdateFeedbackEventArgs : EventArgs
    {
        #region Field Region

        private string result;

        #endregion

        #region Property Region

        /// <summary>
        /// The string to update the GUI feedback box with.
        /// </summary>
        public string Result
        {
            get { return result; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for the UpdateFeedbackEventArgs class.
        /// </summary>
        /// <param name="result"></param>
        public UpdateFeedbackEventArgs(string result)
        {
            this.result = result;
        }

        #endregion
    }
}
