using MscrmTools.SolutionComponentsDeduplicator.AppCode;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MscrmTools.SolutionComponentsDeduplicator.Forms
{
    public partial class LogForm : DockContent
    {
        public LogForm(Logger log)
        {
            InitializeComponent();

            log.OnLog += Log_OnLog;
        }

        private void Log_OnLog(object sender, LogEventArgs e)
        {
            Invoke(new Action(() =>
            {
                lvLogs.Items.Add(new ListViewItem
                {
                    Text = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {e.Message}",
                    ForeColor = e.Message.IndexOf("error", StringComparison.InvariantCultureIgnoreCase) >= 0 ? Color.Red : e.Message.IndexOf("success", StringComparison.InvariantCultureIgnoreCase) >= 0 ? Color.Green : SystemColors.WindowText
                });
            }));
        }

        private void LogForm_Resize(object sender, EventArgs e)
        {
            if (lvLogs.Width > 20)
            {
                lvLogs.Columns[0].Width = lvLogs.Width - 20;
            }
        }

        private void lvLogs_DoubleClick(object sender, EventArgs e)
        {
            if (lvLogs.SelectedItems.Count > 0)
            {
                MessageBox.Show(this, lvLogs.SelectedItems[0].Text, "Log Detail", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}