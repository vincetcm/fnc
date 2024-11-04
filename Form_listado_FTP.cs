// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_listado_FTP
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.DATASERVER.FTP_Cliente;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_listado_FTP : Form
  {
    private string ftpServerIP;
    private string ftpUserID;
    private string ftpPassword;
    private string ftpPort;
    private string working_directory = "";
    private string uri_working_directory = "";
    private IContainer components = (IContainer) null;
    private ListView listView_FTP;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fivherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private Button button_borrar;
    private Button button_renombrar;
    private Label label_directorio2;
    private Label label_directorio1;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private Button button_enviar_FTP;
    private Button button_descargar_FTP;
    private Button button_crear_directorio_FTP;
    private Button button_cambiar_directorio;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem descargarUnFicheroDesdeElDATASERVERToolStripMenuItem;
    private ToolStripMenuItem renombrarFicheroToolStripMenuItem;
    private ToolStripMenuItem borrarFicheroODirectorioToolStripMenuItem;
    private ToolStripMenuItem crearNuevoDirectorioToolStripMenuItem;
    private ToolStripMenuItem cambiarDirectorioTrabajoToolStripMenuItem;
    private ToolStripMenuItem enviarUnFicheroAlDATASERVERToolStripMenuItem;
    private Button button_help_DATASERVER;

    public Form_listado_FTP() => this.InitializeComponent();

    private void Form_listado_FTP_Load(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0)
      {
        if (informacion.leer_contador_uso(true) == -1)
          this.Close();
        if (informacion.leer_fecha_uso() == -1)
          this.Close();
      }
      try
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_FTP.String1);
        }
        else
        {
          string str = (string) registryKey1.GetValue("Machine");
          RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false);
          if ((string) registryKey2.GetValue("Type") != "ETHERNET")
          {
            int num2 = (int) MessageBox.Show(Resource_Form_listado_FTP.String2);
          }
          else if ((string) registryKey2.GetValue("DATASERVER") != "YES")
          {
            int num3 = (int) MessageBox.Show(Resource_Form_listado_FTP.String3);
          }
          else
          {
            this.ftpServerIP = (string) registryKey2.GetValue("IP_ETHERNET");
            this.ftpUserID = (string) registryKey2.GetValue("User_FTP_Client");
            this.ftpPassword = (string) registryKey2.GetValue("Password_FTP_Client");
            this.ftpPort = (string) registryKey2.GetValue("Port_FTP_Client");
            this.Text = this.Text + ": " + (string) registryKey2.GetValue("Name");
            this.Listar_listView(this.GetFilesDetailList());
            this.label_directorio2.Text = this.working_directory + "\\";
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void Listar_listView(string[] listado_ficheros)
    {
      try
      {
        this.listView_FTP.Items.Clear();
        FTPLineParser ftpLineParser = new FTPLineParser();
        this.listView_FTP.Items.Add(new ListViewItem()
        {
          Text = "<..>",
          SubItems = {
            "Dir",
            "",
            "",
            ""
          }
        });
        if (listado_ficheros == null)
        {
          this.listView_FTP.EnsureVisible(0);
          this.listView_FTP.Focus();
          this.listView_FTP.Items[0].Selected = true;
          this.listView_FTP.Items[0].Focused = true;
        }
        else
        {
          foreach (string listadoFichero in listado_ficheros)
          {
            FTPLineResult ftpLineResult = ftpLineParser.Parse(listadoFichero);
            ListViewItem listViewItem = new ListViewItem();
            if (ftpLineResult.IsDirectory)
            {
              listViewItem.Text = "<" + ftpLineResult.Name + ">";
              listViewItem.SubItems.Add("Dir");
              listViewItem.SubItems.Add("");
              listViewItem.SubItems.Add("");
              listViewItem.SubItems.Add("");
            }
            else
            {
              listViewItem.Text = ftpLineResult.Name;
              listViewItem.SubItems.Add("File");
              listViewItem.SubItems.Add(ftpLineResult.Size);
              listViewItem.SubItems.Add(ftpLineResult.Day + " - " + ftpLineResult.Month);
              listViewItem.SubItems.Add(ftpLineResult.Year_time);
            }
            this.listView_FTP.Items.Add(listViewItem);
          }
          this.listView_FTP.EnsureVisible(0);
          this.listView_FTP.Focus();
          this.listView_FTP.Items[0].Selected = true;
          this.listView_FTP.Items[0].Focused = true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String4 + ex.Message);
      }
    }

    private string[] GetFilesDetailList()
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/"));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        ftpWebRequest.Method = "LIST";
        ftpWebRequest.KeepAlive = false;
        WebResponse response = ftpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        while (true)
        {
          string str = streamReader.ReadLine();
          if (str != null)
          {
            stringBuilder.Append(str);
            stringBuilder.Append("\n");
          }
          else
            break;
        }
        streamReader.Close();
        response.Close();
        if (stringBuilder.Length == 0)
          return (string[]) null;
        stringBuilder.Remove(stringBuilder.ToString().LastIndexOf("\n"), 1);
        return stringBuilder.ToString().Split('\n');
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String5 + ex.Message);
        return (string[]) null;
      }
    }

    private string GetworkingDirectory()
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + "/"));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        ftpWebRequest.Method = "PWD";
        WebResponse response = ftpWebRequest.GetResponse();
        string str = new StreamReader(response.GetResponseStream()).ReadLine();
        response.Close();
        return str;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String6 + ex.Message);
        return (string) null;
      }
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void button_borrar_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView_FTP.FocusedItem == null)
        {
          int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (this.listView_FTP.SelectedItems[0].SubItems[1].Text == "File")
        {
          if (MessageBox.Show(Resource_Form_listado_FTP.String8 + this.listView_FTP.FocusedItem.Text + "  ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            return;
          this.DeleteFTP(this.listView_FTP.FocusedItem.Text);
          this.Form_listado_FTP_Load((object) null, (EventArgs) null);
        }
        else
        {
          if (!(this.listView_FTP.SelectedItems[0].SubItems[1].Text == "Dir"))
            throw new Exception("No way to know if it is a File or Directory");
          if (MessageBox.Show(Resource_Form_listado_FTP.String9 + this.listView_FTP.FocusedItem.Text + "  ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            return;
          Match match = new Regex("^<(?<dir_name>[^>]{1,})>$").Match(this.listView_FTP.FocusedItem.Text);
          if (!match.Success)
            throw new Exception("Nombre de directorio no es correcto");
          this.Remove_dir_FTP(match.Groups["dir_name"].Value);
          this.Form_listado_FTP_Load((object) null, (EventArgs) null);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String10 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    public void DeleteFTP(string fileName)
    {
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/" + fileName);
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        ftpWebRequest.KeepAlive = false;
        ftpWebRequest.Method = "DELE";
        string str = string.Empty;
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        long contentLength = response.ContentLength;
        Stream responseStream = response.GetResponseStream();
        StreamReader streamReader = new StreamReader(responseStream);
        str = streamReader.ReadToEnd();
        streamReader.Close();
        responseStream.Close();
        response.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String11 + ex.Message);
      }
    }

    private void button_renombrar_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView_FTP.FocusedItem == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_FTP.String12, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (this.listView_FTP.SelectedItems[0].SubItems[1].Text == "Dir")
        {
          int num2 = (int) MessageBox.Show(Resource_Form_listado_FTP.String13, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.Enabled = false;
          Form_rename_file formRenameFile = new Form_rename_file();
          formRenameFile.Programa_original = this.listView_FTP.FocusedItem.Text;
          int num3 = (int) formRenameFile.ShowDialog();
          if (formRenameFile.Boton_aceptar)
          {
            this.Rename_file_FTP(formRenameFile.Programa_original, formRenameFile.Nuevo_programa);
            this.Form_listado_FTP_Load((object) null, (EventArgs) null);
          }
          this.Enabled = true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String14 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Close();
      }
    }

    private void Rename_file_FTP(string currentFilename, string newFilename)
    {
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/" + currentFilename));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Method = "RENAME";
        ftpWebRequest.RenameTo = newFilename;
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        response.GetResponseStream().Close();
        response.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String15 + ex.Message);
      }
    }

    private void Remove_dir_FTP(string directory_name)
    {
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/" + directory_name));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Method = "RMD";
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        response.GetResponseStream().Close();
        response.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String16 + ex.Message);
      }
    }

    private void button_enviar_FTP_Click(object sender, EventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Title = Resource_Form_listado_FTP.String17;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
        Informacion informacion = new Informacion();
        if (fileInfo.Length > 10000L && informacion.leer_clave(true) <= 0)
        {
          int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String18, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.Upload(openFileDialog.FileName);
          this.Form_listado_FTP_Load((object) null, (EventArgs) null);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String19 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void Upload(string filename)
    {
      FileInfo fileInfo = new FileInfo(filename);
      FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/" + fileInfo.Name));
      ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
      ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
      ftpWebRequest.KeepAlive = false;
      ftpWebRequest.Method = "STOR";
      ftpWebRequest.UseBinary = true;
      ftpWebRequest.ContentLength = fileInfo.Length;
      int count1 = 2048;
      byte[] buffer = new byte[count1];
      FileStream fileStream = fileInfo.OpenRead();
      try
      {
        Stream requestStream = ftpWebRequest.GetRequestStream();
        for (int count2 = fileStream.Read(buffer, 0, count1); count2 != 0; count2 = fileStream.Read(buffer, 0, count1))
          requestStream.Write(buffer, 0, count2);
        requestStream.Close();
        fileStream.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String20 + ex.Message);
      }
    }

    private void button_descargar_FTP_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView_FTP.FocusedItem == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_FTP.String21, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (this.listView_FTP.SelectedItems[0].SubItems[1].Text == "Dir")
        {
          int num2 = (int) MessageBox.Show(Resource_Form_listado_FTP.String22, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Title = Resource_Form_listado_FTP.String23;
          saveFileDialog.FileName = this.listView_FTP.FocusedItem.Text;
          if (saveFileDialog.ShowDialog() != DialogResult.OK)
            return;
          int num3 = saveFileDialog.FileName.IndexOf(this.listView_FTP.FocusedItem.Text);
          if (num3 <= 0)
          {
            int num4 = (int) MessageBox.Show(Resource_Form_listado_FTP.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            string filePath = saveFileDialog.FileName.Substring(0, num3);
            string fileName = saveFileDialog.FileName.Substring(num3, saveFileDialog.FileName.Length - num3);
            if (fileName != this.listView_FTP.FocusedItem.Text)
            {
              int num5 = (int) MessageBox.Show(Resource_Form_listado_FTP.String25, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              this.Download(filePath, fileName);
              this.Form_listado_FTP_Load((object) null, (EventArgs) null);
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String26 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void Download(string filePath, string fileName)
    {
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/" + fileName));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Method = "RETR";
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        Stream responseStream = response.GetResponseStream();
        long contentLength = response.ContentLength;
        FileStream fileStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
        int count1 = 2048;
        byte[] buffer = new byte[count1];
        for (int count2 = responseStream.Read(buffer, 0, count1); count2 > 0; count2 = responseStream.Read(buffer, 0, count1))
          fileStream.Write(buffer, 0, count2);
        responseStream.Close();
        fileStream.Close();
        response.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String27 + ex.Message);
      }
    }

    private void button_crear_directorio_FTP_Click(object sender, EventArgs e)
    {
      try
      {
        this.Enabled = false;
        Form_crear_directorio formCrearDirectorio = new Form_crear_directorio();
        int num = (int) formCrearDirectorio.ShowDialog();
        if (formCrearDirectorio.Boton_aceptar)
        {
          this.MakeDir(formCrearDirectorio.Nuevo_directorio);
          this.Form_listado_FTP_Load((object) null, (EventArgs) null);
        }
        this.Enabled = true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String28 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Enabled = true;
      }
    }

    private void MakeDir(string dirName)
    {
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.ftpServerIP + ":" + this.ftpPort + this.uri_working_directory + "/" + dirName));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.Method = "MKD";
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.ftpUserID, this.ftpPassword);
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        response.GetResponseStream().Close();
        response.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_FTP.String29 + ex.Message);
      }
    }

    private void button_cambiar_directorio_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView_FTP.FocusedItem == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_FTP.String30, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (this.listView_FTP.SelectedItems[0].SubItems[1].Text != "Dir")
        {
          int num2 = (int) MessageBox.Show(Resource_Form_listado_FTP.String31, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          Match match = new Regex("^<(?<dir_name>[^>]{1,})>$").Match(this.listView_FTP.FocusedItem.Text);
          if (!match.Success)
            throw new Exception(Resource_Form_listado_FTP.String32);
          string str = match.Groups["dir_name"].Value;
          if (str == "..")
          {
            int length = this.working_directory.LastIndexOf('\\');
            this.working_directory = length > 0 ? this.working_directory.Substring(0, length) : "";
          }
          else
            this.working_directory = this.working_directory + "\\" + str;
          this.uri_working_directory = "";
          if (this.working_directory != "")
            this.uri_working_directory = "/" + this.working_directory;
          this.Form_listado_FTP_Load((object) null, (EventArgs) null);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("No es posible: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.working_directory = "";
        this.uri_working_directory = "";
        this.label_directorio2.Text = "\\";
        this.Listar_listView((string[]) null);
      }
    }

    private void listView_FTP_DoubleClick(object sender, EventArgs e)
    {
      if (this.listView_FTP.SelectedItems[0].SubItems[1].Text == "Dir")
      {
        this.button_cambiar_directorio_Click((object) null, (EventArgs) null);
      }
      else
      {
        if (!(this.listView_FTP.SelectedItems[0].SubItems[1].Text == "File") || MessageBox.Show(Resource_Form_listado_FTP.String33 + this.listView_FTP.FocusedItem.Text + "\" ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.button_descargar_FTP_Click((object) null, (EventArgs) null);
      }
    }

    private void descargarUnFicheroDesdeElDATASERVERToolStripMenuItem_Click(
      object sender,
      EventArgs e)
    {
      this.button_descargar_FTP_Click((object) null, (EventArgs) null);
    }

    private void renombrarFicheroToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_renombrar_Click((object) null, (EventArgs) null);
    }

    private void borrarFicheroODirectorioToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_borrar_Click((object) null, (EventArgs) null);
    }

    private void crearNuevoDirectorioToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_crear_directorio_FTP_Click((object) null, (EventArgs) null);
    }

    private void cambiarDirectorioTrabajoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_cambiar_directorio_Click((object) null, (EventArgs) null);
    }

    private void enviarUnFicheroAlDATASERVERToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_enviar_FTP_Click((object) null, (EventArgs) null);
    }

    private void button_help_DATASERVER_Click(object sender, EventArgs e)
    {
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_setting_DATASERVER"
      }.ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_listado_FTP));
      this.listView_FTP = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.enviarUnFicheroAlDATASERVERToolStripMenuItem = new ToolStripMenuItem();
      this.descargarUnFicheroDesdeElDATASERVERToolStripMenuItem = new ToolStripMenuItem();
      this.renombrarFicheroToolStripMenuItem = new ToolStripMenuItem();
      this.borrarFicheroODirectorioToolStripMenuItem = new ToolStripMenuItem();
      this.crearNuevoDirectorioToolStripMenuItem = new ToolStripMenuItem();
      this.cambiarDirectorioTrabajoToolStripMenuItem = new ToolStripMenuItem();
      this.menuStrip1 = new MenuStrip();
      this.fivherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.button_borrar = new Button();
      this.button_renombrar = new Button();
      this.label_directorio2 = new Label();
      this.label_directorio1 = new Label();
      this.button_enviar_FTP = new Button();
      this.button_descargar_FTP = new Button();
      this.button_crear_directorio_FTP = new Button();
      this.button_cambiar_directorio = new Button();
      this.button_help_DATASERVER = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.listView_FTP, "listView_FTP");
      this.listView_FTP.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.listView_FTP.ContextMenuStrip = this.contextMenuStrip1;
      this.listView_FTP.MultiSelect = false;
      this.listView_FTP.Name = "listView_FTP";
      this.listView_FTP.UseCompatibleStateImageBehavior = false;
      this.listView_FTP.View = View.Details;
      this.listView_FTP.DoubleClick += new EventHandler(this.listView_FTP_DoubleClick);
      componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
      componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
      componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
      componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
      componentResourceManager.ApplyResources((object) this.columnHeader5, "columnHeader5");
      componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.enviarUnFicheroAlDATASERVERToolStripMenuItem,
        (ToolStripItem) this.descargarUnFicheroDesdeElDATASERVERToolStripMenuItem,
        (ToolStripItem) this.renombrarFicheroToolStripMenuItem,
        (ToolStripItem) this.borrarFicheroODirectorioToolStripMenuItem,
        (ToolStripItem) this.crearNuevoDirectorioToolStripMenuItem,
        (ToolStripItem) this.cambiarDirectorioTrabajoToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      componentResourceManager.ApplyResources((object) this.enviarUnFicheroAlDATASERVERToolStripMenuItem, "enviarUnFicheroAlDATASERVERToolStripMenuItem");
      this.enviarUnFicheroAlDATASERVERToolStripMenuItem.Name = "enviarUnFicheroAlDATASERVERToolStripMenuItem";
      this.enviarUnFicheroAlDATASERVERToolStripMenuItem.Click += new EventHandler(this.enviarUnFicheroAlDATASERVERToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.descargarUnFicheroDesdeElDATASERVERToolStripMenuItem, "descargarUnFicheroDesdeElDATASERVERToolStripMenuItem");
      this.descargarUnFicheroDesdeElDATASERVERToolStripMenuItem.Name = "descargarUnFicheroDesdeElDATASERVERToolStripMenuItem";
      this.descargarUnFicheroDesdeElDATASERVERToolStripMenuItem.Click += new EventHandler(this.descargarUnFicheroDesdeElDATASERVERToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.renombrarFicheroToolStripMenuItem, "renombrarFicheroToolStripMenuItem");
      this.renombrarFicheroToolStripMenuItem.Name = "renombrarFicheroToolStripMenuItem";
      this.renombrarFicheroToolStripMenuItem.Click += new EventHandler(this.renombrarFicheroToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.borrarFicheroODirectorioToolStripMenuItem, "borrarFicheroODirectorioToolStripMenuItem");
      this.borrarFicheroODirectorioToolStripMenuItem.Name = "borrarFicheroODirectorioToolStripMenuItem";
      this.borrarFicheroODirectorioToolStripMenuItem.Click += new EventHandler(this.borrarFicheroODirectorioToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.crearNuevoDirectorioToolStripMenuItem, "crearNuevoDirectorioToolStripMenuItem");
      this.crearNuevoDirectorioToolStripMenuItem.Name = "crearNuevoDirectorioToolStripMenuItem";
      this.crearNuevoDirectorioToolStripMenuItem.Click += new EventHandler(this.crearNuevoDirectorioToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.cambiarDirectorioTrabajoToolStripMenuItem, "cambiarDirectorioTrabajoToolStripMenuItem");
      this.cambiarDirectorioTrabajoToolStripMenuItem.Name = "cambiarDirectorioTrabajoToolStripMenuItem";
      this.cambiarDirectorioTrabajoToolStripMenuItem.Click += new EventHandler(this.cambiarDirectorioTrabajoToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.fivherosToolStripMenuItem
      });
      this.menuStrip1.Name = "menuStrip1";
      componentResourceManager.ApplyResources((object) this.fivherosToolStripMenuItem, "fivherosToolStripMenuItem");
      this.fivherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.salirToolStripMenuItem
      });
      this.fivherosToolStripMenuItem.Name = "fivherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.button_borrar, "button_borrar");
      this.button_borrar.AccessibleRole = AccessibleRole.None;
      this.button_borrar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_borrar.Name = "button_borrar";
      this.button_borrar.UseVisualStyleBackColor = false;
      this.button_borrar.Click += new EventHandler(this.button_borrar_Click);
      componentResourceManager.ApplyResources((object) this.button_renombrar, "button_renombrar");
      this.button_renombrar.AccessibleRole = AccessibleRole.None;
      this.button_renombrar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_renombrar.Name = "button_renombrar";
      this.button_renombrar.UseVisualStyleBackColor = false;
      this.button_renombrar.Click += new EventHandler(this.button_renombrar_Click);
      componentResourceManager.ApplyResources((object) this.label_directorio2, "label_directorio2");
      this.label_directorio2.BorderStyle = BorderStyle.Fixed3D;
      this.label_directorio2.Name = "label_directorio2";
      componentResourceManager.ApplyResources((object) this.label_directorio1, "label_directorio1");
      this.label_directorio1.BorderStyle = BorderStyle.Fixed3D;
      this.label_directorio1.Name = "label_directorio1";
      componentResourceManager.ApplyResources((object) this.button_enviar_FTP, "button_enviar_FTP");
      this.button_enviar_FTP.AccessibleRole = AccessibleRole.None;
      this.button_enviar_FTP.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_enviar_FTP.Name = "button_enviar_FTP";
      this.button_enviar_FTP.UseVisualStyleBackColor = false;
      this.button_enviar_FTP.Click += new EventHandler(this.button_enviar_FTP_Click);
      componentResourceManager.ApplyResources((object) this.button_descargar_FTP, "button_descargar_FTP");
      this.button_descargar_FTP.AccessibleRole = AccessibleRole.None;
      this.button_descargar_FTP.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_descargar_FTP.Name = "button_descargar_FTP";
      this.button_descargar_FTP.UseVisualStyleBackColor = false;
      this.button_descargar_FTP.Click += new EventHandler(this.button_descargar_FTP_Click);
      componentResourceManager.ApplyResources((object) this.button_crear_directorio_FTP, "button_crear_directorio_FTP");
      this.button_crear_directorio_FTP.AccessibleRole = AccessibleRole.None;
      this.button_crear_directorio_FTP.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_crear_directorio_FTP.Name = "button_crear_directorio_FTP";
      this.button_crear_directorio_FTP.UseVisualStyleBackColor = false;
      this.button_crear_directorio_FTP.Click += new EventHandler(this.button_crear_directorio_FTP_Click);
      componentResourceManager.ApplyResources((object) this.button_cambiar_directorio, "button_cambiar_directorio");
      this.button_cambiar_directorio.AccessibleRole = AccessibleRole.None;
      this.button_cambiar_directorio.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_cambiar_directorio.Name = "button_cambiar_directorio";
      this.button_cambiar_directorio.UseVisualStyleBackColor = false;
      this.button_cambiar_directorio.Click += new EventHandler(this.button_cambiar_directorio_Click);
      componentResourceManager.ApplyResources((object) this.button_help_DATASERVER, "button_help_DATASERVER");
      this.button_help_DATASERVER.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.button_help_DATASERVER.ForeColor = System.Drawing.Color.Red;
      this.button_help_DATASERVER.Name = "button_help_DATASERVER";
      this.button_help_DATASERVER.UseVisualStyleBackColor = false;
      this.button_help_DATASERVER.Click += new EventHandler(this.button_help_DATASERVER_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.button_help_DATASERVER);
      this.Controls.Add((Control) this.button_cambiar_directorio);
      this.Controls.Add((Control) this.button_crear_directorio_FTP);
      this.Controls.Add((Control) this.button_descargar_FTP);
      this.Controls.Add((Control) this.button_enviar_FTP);
      this.Controls.Add((Control) this.label_directorio1);
      this.Controls.Add((Control) this.label_directorio2);
      this.Controls.Add((Control) this.button_renombrar);
      this.Controls.Add((Control) this.button_borrar);
      this.Controls.Add((Control) this.listView_FTP);
      this.Controls.Add((Control) this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_listado_FTP);
      this.Load += new EventHandler(this.Form_listado_FTP_Load);
      this.contextMenuStrip1.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
