using System;
using System.Windows.Forms;
using System.IO;
using FileEncrypt;
using System.Diagnostics;

namespace BackFile
    {
    public partial class BackupForm : Form
        {
        DataEncryptor dataCryptor = new DataEncryptor();
        DataEncryptor dataCryptor2 = new DataEncryptor();
        FileInfo info;
        FileInfo info2;
        public BackupForm()
            {
            InitializeComponent();
            }

        private void btnOutfutFolder_Click(object sender , EventArgs e)
            {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.RestoreDirectory = true;
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                dataCryptor.outPutFilePath(Path.GetDirectoryName(folderBrowser.FileName));
                string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                txtFolderName.Text = folderPath;
                // ...
                }
            }

        private void btnBackup_Click(object sender , EventArgs e)
            {
                try
                {
                    if(string.IsNullOrEmpty(txtFolderName.Text)){
                    throw new Exception("Select folder to backup file");
                    }
                    timer1.Enabled = true;
                    timer1.Start();

                    FileInfo infor = new FileInfo(Application.StartupPath + "/CASHDESKDB.mdf");
                    info = new FileInfo(infor.FullName);
                    dataCryptor.inPutFilePath(infor.FullName);
                    dataCryptor.FileType(info.Extension);
                    dataCryptor.FileType(info.Name.Substring(0, info.Name.Length - info.Extension.Length), info.Extension);
                    dataCryptor.isDeletePlainFile(false);
                    dataCryptor.EncryptFile();

                    FileInfo infor2 = new FileInfo(Application.StartupPath + "/CASHDESKDB_log.ldf");
                    info2 = new FileInfo(infor2.FullName);
                    dataCryptor2.inPutFilePath(infor2.FullName);
                    dataCryptor2.FileType(info2.Extension);
                    dataCryptor2.FileType(info2.Name.Substring(0, info2.Name.Length - info2.Extension.Length), info2.Extension);
                    dataCryptor2.isDeletePlainFile(false);
                    dataCryptor2.EncryptFile();
                    MessageBox.Show("Backup Sucessfull", "Cashdesk", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Stop();
                    pictureWait.Visible = false;
                    lblWait.Visible = false;

                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBox.Show("Access to File is denied for backup", "Cashdesk", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception ex)
                {
                    pictureWait.Visible = false;
                    lblWait.Visible = false;
                    timer1.Stop();
                    MessageBox.Show( ex.Message, "Cashdesk", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        private void btnClose_Click(object sender , EventArgs e)
            {
            try
                {
                ProcessStartInfo info = new ProcessStartInfo("WindowsFormsApplication1.exe");
                info.UseShellExecute = true;
                info.Verb = "runas";
                Process.Start(info);
                Application.ExitThread();
                }
            catch (Exception)
                {
                MessageBox.Show("Specified File was not found" , "Cash Desk" , MessageBoxButtons.OK , MessageBoxIcon.Information);
                }
            }
        

        private void BackupForm_FormClosed(object sender , FormClosedEventArgs e)
            {
            try
                {
                ProcessStartInfo info = new ProcessStartInfo("WindowsFormsApplication1.exe");
                info.UseShellExecute = true;
                info.Verb = "runas";
                Process.Start(info);
                Application.ExitThread();
                }
            catch (Exception)
                {
                MessageBox.Show("Specified File was not found" , "Cash Desk" , MessageBoxButtons.OK , MessageBoxIcon.Information);
                }
            }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureWait.Visible = true;
            lblWait.Visible = true;
        }
        
        }
    }
