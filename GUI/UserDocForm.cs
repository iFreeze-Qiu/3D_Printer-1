using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    /// <summary>
    /// This form will display the user documentation assocated
    /// with the program, by loading a HTML document 
    /// into a WebBrowser control displayed on the form.
    /// </summary>
    public partial class UserDocForm : Form
    {
        #region Constructor Region

        /// <summary>
        /// Constructor for the UserDocForm
        /// class.
        /// </summary>
        public UserDocForm()
        {
            InitializeComponent();

            this.Load += UserDocForm_Load;
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// On form load, navigate the webbrowser control to the
        /// user doc html document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserDocForm_Load(object sender, EventArgs e)
        {
            string fileName = Application.StartupPath + @"\html\UserDoc.html";

            webBrowser1.Navigate(fileName);
        }

        #endregion
    }
}
