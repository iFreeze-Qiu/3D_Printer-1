using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using PrintingLogic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;

namespace GUI
{
    /// <summary>
    /// This form handles the Z-Stage Tracking Mode.
    /// </summary>
    public partial class ZStageTrackingModeForm : Form
    {
        #region Field Region

        private Logic logic;
        private bool trackingOn;

        #endregion

        #region Constructor Region

        /// <summary>
        /// Constructor for ZStageTrackingModeForm.
        /// </summary>
        /// <param name="logic"></param>
        public ZStageTrackingModeForm(Logic logic)
        {
            InitializeComponent();

            this.logic = logic;

            this.FormClosing += ZStageTrackingModeForm_FormClosing;

            this.logic.VoltageDetected += logic_VoltageDetected;
            this.logic.ForceDetectedDuringTracking += logic_ForceDetectedDuringTracking;
            this.logic.MaxHeightReachedDuringTracking += logic_MaxHeightReachedDuringTracking;
            this.logic.UpdateFeedback += logic_UpdateFeedback;
        }

        #endregion

        #region Event Handler Region

        /// <summary>
        /// This event will update the feedback box with updates from the
        /// tracking mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_UpdateFeedback(object sender, UpdateFeedbackEventArgs e)
        {
            try
            {
                if (!feedbackRichTextBox.IsHandleCreated)
                {
                    feedbackRichTextBox.CreateControl();
                }

                feedbackRichTextBox.Invoke(new Action(() =>
                    feedbackRichTextBox.Text += e.Result + "\n\n"));
            }
            catch (Exception)
            {
                feedbackRichTextBox.Text += e.Result + "\n\n";
            }
        }

        /// <summary>
        /// Form closing event handler. Simply unsubscribe the
        /// VoltageDetected event so that the main form can
        /// re-subscribe to it and monitor for printing
        /// voltage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZStageTrackingModeForm_FormClosing(object sender, 
            FormClosingEventArgs e)
        {
            this.logic.VoltageDetected -= logic_VoltageDetected;
            this.logic.UpdateFeedback -= logic_UpdateFeedback;
        }

        /// <summary>
        /// Event that fires when the force is detected during tracking mode.
        /// Update the feedback box of the position/force value and return the
        /// Z-Stage to the starting position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_ForceDetectedDuringTracking(object sender, ForceDetectedEventArgs e)
        {
            try
            {
                if (!feedbackRichTextBox.IsHandleCreated)
                {
                    feedbackRichTextBox.CreateControl();
                }

                    feedbackRichTextBox.Invoke(new Action(() =>
                        feedbackRichTextBox.Text += "Force of " + e.ForceDetectedValue.ToString()
                    + " detected at position " + e.ForceDetectedPosition.ToString() + "\n\n"));
            }
            catch (Exception)
            {
                feedbackRichTextBox.Text += "Force of " + e.ForceDetectedValue.ToString()
                    + " detected at position " + e.ForceDetectedPosition.ToString() + "\n\n";
            }

            logic.ZStageReset();

            logic.IsVoltageDetected = false;
        }

        /// <summary>
        /// Event that fires if the Z-Stage reaches its maximum height during tracking.
        /// This means that the Z-Stage itself is too low to reach the printing tips
        /// and must be adjusted accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logic_MaxHeightReachedDuringTracking(object sender, EventArgs e)
        {
            feedbackRichTextBox.Invoke(new Action(() =>
                feedbackRichTextBox.Text += "The Z-Stage reached its maximum height, " +
                "meaning that it is too far from the ink. Adjust 3D printer settings.\n\n"));

            trackingOn = false;

            stopTrackingButton.Invoke(new Action(() =>
                stopTrackingButton.Enabled = false));

            beginTrackingButton.Invoke(new Action(() =>
                beginTrackingButton.Enabled = true));
            
            exportDataButton.Invoke(new Action(() =>
                exportDataButton.Enabled = true));

            logic.ZStageReset();

            logic.IsVoltageDetected = false;
        }

        /// <summary>
        /// This event fires when the voltage change is detected. This will
        /// begin the tracking program if tracking mode is enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void logic_VoltageDetected(object sender, EventArgs e)
        {
            logic.IsVoltageDetected = true;

            if (trackingOn == true && logic.IsTracking == false)
            {
                await logic.BeginTracking();
            }

        }

        #endregion

        #region Tracking Button Method Region

        /// <summary>
        /// This button will engage the tracking mode. On its own, it will
        /// do nothing until the voltage change is detected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beginTrackingButton_Click(object sender, EventArgs e)
        {
            stopTrackingButton.Enabled = true;
            beginTrackingButton.Enabled = false;
            exportDataButton.Enabled = false;
            logic.TrackingDataList = new List<TrackingData>();

            trackingOn = true;
        }

        /// <summary>
        /// This button will disable the tracking mode. Any voltage 
        /// detection will be ignored and nothing will happen until
        /// the begin tracking mode button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopTrackingButton_Click(object sender, EventArgs e)
        {
            stopTrackingButton.Enabled = false;
            beginTrackingButton.Enabled = true;
            exportDataButton.Enabled = true;
        }

        #endregion

        #region Export Method Region

        /// <summary>
        /// This button will export the tracking data to a spreadsheet
        /// that will be saved to the local file system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportDataButton_Click(object sender, EventArgs e)
        {
            string fileName = Interaction.InputBox("Save file as: ", "Save File",
                @"\TrackingData.xlsx");

            FolderBrowserDialog fbDialog = new FolderBrowserDialog();

            fbDialog.Description = "Select Data Folder";
            fbDialog.SelectedPath = Application.StartupPath;

            DialogResult result = fbDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                CreateSpreadsheet(fbDialog.SelectedPath + fileName);               
            }
        }

        #endregion

        #region Spreadsheet Method Region

        /// <summary>
        /// This function will handle the spreadsheet
        /// creation. 
        /// </summary>
        /// <param name="fileName"></param>
        private void CreateSpreadsheet(string fileName)
        {
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                   Create(fileName, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Get the SharedStringTablePart. If it does not exist, create a new one.
            SharedStringTablePart shareStringPart;
            if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "mySheet",
            };


            sheets.Append(sheet);

            workbookpart.Workbook.Save();

            // Insert the text into the SharedStringTablePart.
            int index = InsertSharedStringItem("Position", shareStringPart);

            // Create the two cells for the spreadsheet - force and position.
            Cell cell = InsertCellInWorkSheet("A", 1, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            index = InsertSharedStringItem("Force", shareStringPart);

            cell = InsertCellInWorkSheet("B", 1, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            uint row = 2;

            // Loop through each position data in the tracking data list and
            // insert it into the A cell.
            foreach (TrackingData data in logic.TrackingDataList)
            {
                index = InsertSharedStringItem(data.Position.ToString(), shareStringPart);

                cell = InsertCellInWorkSheet("A", row, worksheetPart);
                cell.CellValue = new CellValue(index.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

                row++;
            }

            row = 2;

            // Loop through each force data in the tracking data list and 
            // insert it into the B cell
            foreach (TrackingData data in logic.TrackingDataList)
            {
                index = InsertSharedStringItem(data.ForceValue.ToString(), shareStringPart);

                cell = InsertCellInWorkSheet("B", row, worksheetPart);
                cell.CellValue = new CellValue(index.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

                row++;
            }

            worksheetPart.Worksheet.Save();

            // Close the document.
            spreadsheetDocument.Close();

            MessageBox.Show("File saved.", "Export Data");
        }        

        /// <summary>
        /// Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        /// and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="shareStringPart"></param>
        /// <returns></returns>
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        /// <summary>
        /// Inserts a cell into the spreadsheet worksheet.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="worksheetPart"></param>
        /// <returns></returns>
        private Cell InsertCellInWorkSheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
            {
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                string cellReference = columnName + rowIndex;

                // If the worksheet does not contain a row with the specified row index, insert one.
                Row row;
                if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
                {
                    row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
                }
                else
                {
                    row = new Row() { RowIndex = rowIndex };
                    sheetData.Append(row);
                }

                // If there is not a cell with the specified column name, insert one.  
                if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
                {
                    return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
                }
                else
                {
                    // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                    Cell refCell = null;
                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }

                    Cell newCell = new Cell() { CellReference = cellReference };
                    row.InsertBefore(newCell, refCell);

                    worksheet.Save();
                    return newCell;
                }
            }

        #endregion
    }
}
