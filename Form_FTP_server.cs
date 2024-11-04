// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_FTP_server
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Properties;
using FANUC_Open_Com.Servidor_CNC.FTP_Servidor;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_FTP_server : Form
  {
    private TcpListener FTPCommandListner;
    private static int intPort;
    private bool bClose = false;
    private IPAddress local_IP = (IPAddress) null;
    private int FTP_command_port;
    private Thread service_thread;
    private bool esperando_cliente = false;
    private Thread thread_servicio_atencion_nuevo_cliente;
    private string default_rootDirOnSystem = "C:\\";
    private bool servidor_activo = false;
    private IContainer components = (IContainer) null;
    private Button button_connect;
    private Button button_disconnect;
    private TextBox textBox_server_ip;
    private TextBox textBox_server_port;
    private TextBox textBox_output_message;
    private Label label2;
    private Label label3;
    private TextBox textBox_server_name;
    private Label label4;
    private TextBox textBox_input_message;
    private TextBox textBox_detalle_status;
    private Label label6;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private GroupBox groupBox_configurar_servidor;
    private Label label_directorio_seleccionado;
    private Button button_seleccionar_fichero;
    private Button button_seleccionar_directorio;
    private TextBox textBox_directorio;
    private Label label7;
    private Label label8;
    private ToolStripMenuItem configuracionToolStripMenuItem;
    private Label label5;
    private GroupBox groupBox_status;
    private Label label9;
    private TextBox textBox_user;
    private RadioButton radioButton_lista;
    private RadioButton radioButton_todos;
    private Button button_lista;
    private Label label15;
    private ToolStripMenuItem exploradorToolStripMenuItem1;
    private Button button_help_FTP_SERVER;

    public Form_FTP_server() => this.InitializeComponent();

    private void Form_FTP_server_load(object sender, EventArgs e)
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
        string hostName = Dns.GetHostName();
        IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
        this.textBox_server_ip.Text = "";
        foreach (IPAddress ipAddress in hostAddresses)
        {
          if (Regex.Match(ipAddress.ToString(), "\\b(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})\\b").Success)
          {
            TextBox textBoxServerIp = this.textBox_server_ip;
            textBoxServerIp.Text = textBoxServerIp.Text + ipAddress.ToString() + "\n";
          }
        }
        this.textBox_server_name.Text = hostName;
        if (this.textBox_server_port.Text == "")
          this.textBox_server_port.Text = "21";
        this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String1;
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Server_FTP", false);
        if (registryKey == null)
          return;
        this.textBox_directorio.Text = (string) registryKey.GetValue("Directory");
        this.textBox_server_port.Text = (string) registryKey.GetValue("FTP_port");
        if ((string) registryKey.GetValue("User_list") == "ON")
          this.radioButton_lista.Checked = true;
        if ((string) registryKey.GetValue("Activate_FTP_Server") == "YES")
          this.button_connect_Click((object) null, (EventArgs) null);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String2 + ex.Message);
      }
    }

    private void button_connect_Click(object sender, EventArgs e)
    {
      if (this.radioButton_todos.Checked)
      {
        if (this.textBox_directorio.Text == "")
        {
          int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.textBox_directorio.Focus();
          return;
        }
        if (!Directory.Exists(this.textBox_directorio.Text))
        {
          int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.textBox_directorio.Focus();
          return;
        }
        this.default_rootDirOnSystem = this.textBox_directorio.Text;
      }
      this.bClose = false;
      try
      {
        if (this.textBox_server_port.Text == "")
          return;
        this.FTP_command_port = (int) Convert.ToInt16(this.textBox_server_port.Text);
        this.FTPCommandListner = new TcpListener(IPAddress.Any, this.FTP_command_port);
        this.FTPCommandListner.Start();
        this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String6 + this.FTP_command_port.ToString();
        this.thread_servicio_atencion_nuevo_cliente = new Thread(new ThreadStart(this.servicio_atencion_nuevo_cliente));
        this.esperando_cliente = false;
        this.thread_servicio_atencion_nuevo_cliente.Start();
        this.label8.Text = Resource_Form_servidor_FTP.String7;
        this.button_connect.Enabled = false;
        this.button_disconnect.Enabled = true;
        this.groupBox_configurar_servidor.Enabled = false;
        this.groupBox_status.Enabled = true;
        this.servidor_activo = true;
        try
        {
          IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
          this.textBox_server_ip.Text = "";
          foreach (IPAddress ipAddress in hostAddresses)
          {
            if (Regex.Match(ipAddress.ToString(), "\\b(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})\\b").Success)
            {
              TextBox textBoxServerIp = this.textBox_server_ip;
              textBoxServerIp.Text = textBoxServerIp.Text + ipAddress.ToString() + "\n";
            }
          }
        }
        catch (Exception ex)
        {
          this.textBox_server_ip.Text = "";
        }
      }
      catch (SocketException ex)
      {
        if (ex.ErrorCode != 10048)
          return;
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String8 + ex.Message);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String9 + ex.Message);
      }
    }

    private void button_disconnect_Click(object sender, EventArgs e)
    {
      try
      {
        this.bClose = true;
        Thread.Sleep(500);
        if (this.FTPCommandListner != null)
          this.FTPCommandListner.Stop();
        if (this.thread_servicio_atencion_nuevo_cliente != null && this.thread_servicio_atencion_nuevo_cliente.IsAlive)
          this.thread_servicio_atencion_nuevo_cliente.Abort();
        this.label8.Text = Resource_Form_servidor_FTP.String10;
        this.textBox_detalle_status.Text = "";
        this.textBox_user.Text = "";
        this.button_connect.Enabled = true;
        this.button_disconnect.Enabled = false;
        this.groupBox_configurar_servidor.Enabled = true;
        this.groupBox_status.Enabled = false;
        this.servidor_activo = false;
      }
      catch (ThreadAbortException ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String11 + ex.Message);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String11 + ex.Message);
      }
    }

    private void servicio_atencion_nuevo_cliente()
    {
      string str = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", false).GetValue("Language");
      if (str == "SPANISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
      if (str == "ENGLISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
      if (str == "FRENCH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
      if (str == "ITALIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("it");
      if (str == "RUSSIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
      if (str == "GERMAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
      if (str == "PORTUGUESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt");
      if (str == "CHINESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-Hans");
      try
      {
        do
        {
          Thread.Sleep(500);
          if (!this.esperando_cliente)
          {
            this.service_thread = new Thread(new ThreadStart(this.FTPClientThread));
            this.service_thread.Start();
            this.esperando_cliente = true;
          }
        }
        while (!this.bClose);
      }
      catch (Exception ex)
      {
        Control.CheckForIllegalCrossThreadCalls = false;
        this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String12;
      }
    }

    private void FTPClientThread()
    {
      string str1 = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", false).GetValue("Language");
      if (str1 == "SPANISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
      if (str1 == "ENGLISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
      if (str1 == "FRENCH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
      if (str1 == "ITALIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("it");
      if (str1 == "RUSSIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
      if (str1 == "GERMAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
      if (str1 == "PORTUGUESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt");
      if (str1 == "CHINESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-Hans");
      int Port = 0;
      string str2 = "";
      string PresentDirOfFTP = "/";
      TcpClient client = (TcpClient) null;
      Socket clientSocket1 = (Socket) null;
      bool PassiveMode = false;
      string strIP = "";
      try
      {
        Socket clientSocket2 = this.FTPCommandListner.AcceptSocket();
        this.esperando_cliente = false;
        NetworkStream inBuffer = new NetworkStream(clientSocket2, FileAccess.Read);
        NetworkStream outBuffer = new NetworkStream(clientSocket2, FileAccess.Write);
        string str3 = "";
        string msg1 = "220 FTP Server OK\r\n";
        this.SendMsg(msg1, ref outBuffer);
        Control.CheckForIllegalCrossThreadCalls = false;
        this.textBox_output_message.Text += msg1;
        this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String13;
        string str4 = this.default_rootDirOnSystem;
        string str5 = "";
        string str6 = "";
        bool flag1 = false;
        while (!flag1)
        {
          Thread.Sleep(100);
          Control.CheckForIllegalCrossThreadCalls = false;
          if (!this.bClose)
          {
            string clientMsg = this.ReadMsgFromBuffer(ref clientSocket2, ref inBuffer);
            if (!(clientMsg == ""))
            {
              this.textBox_input_message.Text += clientMsg;
              if (this.textBox_input_message.Text.Length > 1000)
                this.textBox_input_message.Text = "";
              this.textBox_input_message.SelectionStart = this.textBox_input_message.Text.Length;
              this.textBox_input_message.ScrollToCaret();
              switch ((clientMsg.Length == 0 ? "" : clientMsg.Substring(0, 4).Trim()).ToUpper())
              {
                case "":
                case "HELP":
                  if (this.textBox_output_message.Text.Length > 1000)
                    this.textBox_output_message.Text = "";
                  this.textBox_output_message.SelectionStart = this.textBox_output_message.Text.Length;
                  this.textBox_output_message.ScrollToCaret();
                  break;
                case "ABOR":
                case "QUIT":
                  clientSocket1?.Close();
                  client?.Close();
                  string msg2 = "221 GOOD BYE\r\n";
                  this.SendMsg(msg2, ref outBuffer);
                  this.textBox_output_message.Text += msg2;
                  flag1 = true;
                  goto case "";
                case "CDUP":
                  string fileName1 = "..";
                  string msg3 = this.ChangeDirectory(str4, ref PresentDirOfFTP, fileName1);
                  this.SendMsg(msg3, ref outBuffer);
                  this.textBox_output_message.Text += msg3;
                  goto case "";
                case "CWD":
                case "XCWD":
                  string fileName2 = clientMsg.Substring(3).Trim();
                  string msg4 = this.ChangeDirectory(str4, ref PresentDirOfFTP, fileName2);
                  this.SendMsg(msg4, ref outBuffer);
                  this.textBox_output_message.Text += msg4;
                  goto case "";
                case "DELE":
                  string fileName3 = clientMsg.Substring(4).Trim();
                  string msg5 = this.DeleteFileForServer(str4, PresentDirOfFTP, fileName3);
                  this.SendMsg(msg5, ref outBuffer);
                  this.textBox_output_message.Text += msg5;
                  goto case "";
                case "LIST":
                  string msg6 = "150 ASCII data\r\n";
                  this.SendMsg(msg6, ref outBuffer);
                  this.textBox_output_message.Text += msg6;
                  NetworkStream nw1 = this.Mode(PassiveMode, ref client, ref clientSocket1, strIP, Port);
                  this.ListDirectory(str4, PresentDirOfFTP, ref nw1);
                  if (!PassiveMode)
                    nw1.Close();
                  else if (clientSocket1 != null)
                    nw1.Close();
                  string msg7 = "226 Transfer complete.\r\n";
                  this.SendMsg(msg7, ref outBuffer);
                  this.textBox_output_message.Text += msg7;
                  goto case "";
                case "MKD":
                case "XMKD":
                  string fileName4 = clientMsg.Substring(4).Trim();
                  string directory = this.CreateDirectory(str4, PresentDirOfFTP, fileName4);
                  this.SendMsg(directory, ref outBuffer);
                  this.textBox_output_message.Text += directory;
                  goto case "";
                case "NLST":
                  string msg8 = "150 ASCII data\r\n";
                  this.SendMsg(msg8, ref outBuffer);
                  this.textBox_output_message.Text += msg8;
                  string argumento = (string) null;
                  if (clientMsg.Length > 7)
                    argumento = clientMsg.Substring(5, clientMsg.Length - 5 - 2);
                  NetworkStream nw2 = this.Mode(PassiveMode, ref client, ref clientSocket1, strIP, Port);
                  this.ListDirectory_solo_nombres(str4, PresentDirOfFTP, ref nw2, argumento);
                  if (!PassiveMode)
                    nw2.Close();
                  else if (clientSocket1 != null)
                    nw2.Close();
                  string msg9 = "226 Transfer complete.\r\n";
                  this.SendMsg(msg9, ref outBuffer);
                  this.textBox_output_message.Text += msg9;
                  goto case "";
                case "NOOP":
                  string msg10 = "200 NOOP command executed.\r\n";
                  this.SendMsg(msg10, ref outBuffer);
                  this.textBox_output_message.Text += msg10;
                  goto case "";
                case "PASS":
                  bool flag2 = false;
                  string str7 = clientMsg.Substring(4).Trim();
                  if (this.radioButton_todos.Checked)
                  {
                    flag2 = true;
                  }
                  else
                  {
                    string str8 = str7.Trim();
                    str5 = str5.Trim();
                    if (str5.ToLower() == str8.ToLower())
                    {
                      flag2 = true;
                      if (str6 != "")
                        str4 = str6;
                    }
                  }
                  string msg11 = !flag2 ? "530 Not logged in. Password is not correct\r\n" : "230 User " + str2 + " logged in\r\n";
                  this.SendMsg(msg11, ref outBuffer);
                  this.textBox_output_message.Text += msg11;
                  goto case "";
                case "PASV":
                  TcpListener clientDataListner = (TcpListener) null;
                  int intPort = this.PassiveModePort(ref clientDataListner);
                  string str9 = clientSocket2.RemoteEndPoint.ToString();
                  string[] strArray = new string[5]
                  {
                    (str9.IndexOf(":") > 0 ? str9.Substring(0, str9.IndexOf(":")) : str9).Replace(".", ","),
                    ",",
                    null,
                    null,
                    null
                  };
                  int num = intPort / 256;
                  strArray[2] = num.ToString();
                  strArray[3] = ",";
                  num = intPort % 256;
                  strArray[4] = num.ToString();
                  this.SendMsg("227 Entering Passive Mode (" + string.Concat(strArray) + ")\r\n", ref outBuffer);
                  clientSocket1 = this.PassiveClientSocket(ref clientDataListner, intPort);
                  if (clientSocket1 == null)
                  {
                    string msg12 = "425 Error in Passive Mode connection\r\n";
                    this.SendMsg(msg12, ref outBuffer);
                    this.textBox_output_message.Text += msg12;
                  }
                  PassiveMode = true;
                  goto case "";
                case "PORT":
                  this.PortData(clientMsg, out Port, out strIP);
                  PassiveMode = false;
                  string msg13 = "200 PORT command successful\r\n";
                  this.SendMsg(msg13, ref outBuffer);
                  this.textBox_output_message.Text += msg13;
                  goto case "";
                case "PWD":
                case "XPWD":
                  string msg14 = "257 \"" + PresentDirOfFTP + "\" is current directory \r\n";
                  this.SendMsg(msg14, ref outBuffer);
                  this.textBox_output_message.Text += msg14;
                  goto case "";
                case "REST":
                  string msg15 = "350 Requested file action pending further info \r\n";
                  this.SendMsg(msg15, ref outBuffer);
                  this.textBox_output_message.Text += msg15;
                  goto case "";
                case "RETR":
                  string msg16 = "150 Binary data connection\r\n";
                  this.SendMsg(msg16, ref outBuffer);
                  this.textBox_output_message.Text += msg16;
                  string str10 = clientMsg.Substring(4).Trim().Replace("\\", "/");
                  string str11 = str10.Substring(0, 1) == "/" ? str10.Substring(1) : str10;
                  string str12 = str4 + PresentDirOfFTP;
                  string str13 = (str12.Substring(str12.Length - 1, 1) == "/" ? str12 : str12 + "/") + str11;
                  NetworkStream nw3 = this.Mode(PassiveMode, ref client, ref clientSocket1, strIP, Port);
                  try
                  {
                    FileInfo fileInfo = new FileInfo(str13);
                    Informacion informacion = new Informacion();
                    if (fileInfo.Length > 10000L && informacion.leer_clave(true) <= 0)
                    {
                      string msg17 = "550 limited access.\r\n";
                      this.SendMsg(msg17, ref outBuffer);
                      this.textBox_output_message.Text = msg17;
                      this.textBox_output_message.Text += "LIMITED test Version.\r\nOnly small programs(data) are allowed to send.\r\n\r\nPlease set the LICENCE number in \"Information\" menu to get the FULL operation version";
                      goto case "";
                    }
                  }
                  catch (Exception ex)
                  {
                    string msg18 = "550 file not found, or no access.\r\n";
                    this.SendMsg(msg18, ref outBuffer);
                    this.textBox_output_message.Text += msg18;
                  }
                  if (this.SendFile(str13, ref nw3))
                  {
                    string msg19 = "226 transfer complete\r\n";
                    this.SendMsg(msg19, ref outBuffer);
                    this.textBox_output_message.Text += msg19;
                  }
                  else
                  {
                    string msg20 = "550 file not found, or no access.\r\n";
                    this.SendMsg(msg20, ref outBuffer);
                    this.textBox_output_message.Text += msg20;
                  }
                  PassiveMode = false;
                  goto case "";
                case "RMD":
                case "XRMD":
                  string fileName5 = clientMsg.Substring(4).Trim();
                  string msg21 = this.RemoveDirectory(str4, PresentDirOfFTP, fileName5);
                  this.SendMsg(msg21, ref outBuffer);
                  this.textBox_output_message.Text += msg21;
                  goto case "";
                case "RNFR":
                  str3 = this.FilePath(str4, PresentDirOfFTP, clientMsg.Substring(4).Trim());
                  if (System.IO.File.Exists(str3))
                  {
                    string msg22 = "350 Requested file action pending further info \r\n";
                    this.SendMsg(msg22, ref outBuffer);
                    this.textBox_output_message.Text += msg22;
                    goto case "";
                  }
                  else
                  {
                    string msg23 = "550 Requested file not found\r\n";
                    this.SendMsg(msg23, ref outBuffer);
                    this.textBox_output_message.Text += msg23;
                    str3 = "";
                    goto case "";
                  }
                case "RNTO":
                  if (str3 != "")
                  {
                    string destFileName = this.FilePath(str4, PresentDirOfFTP, clientMsg.Substring(4).Trim());
                    System.IO.File.Copy(str3, destFileName);
                    System.IO.File.Delete(str3);
                  }
                  string msg24 = "250 Requested file action completed. \r\n";
                  this.SendMsg(msg24, ref outBuffer);
                  this.textBox_output_message.Text += msg24;
                  goto case "";
                case "STOR":
                  string msg25 = "150 Binary data connection\r\n";
                  this.SendMsg(msg25, ref outBuffer);
                  this.textBox_output_message.Text += msg25;
                  string str14 = clientMsg.Substring(4).Trim();
                  NetworkStream nw4 = this.Mode(PassiveMode, ref client, ref clientSocket1, strIP, Port);
                  string str15 = str14.Replace("\\", "/");
                  string str16 = str15.Substring(0, 1) == "/" ? str15.Substring(1) : str15;
                  string str17 = str4 + PresentDirOfFTP;
                  if (this.CreateFile((str17.Substring(str17.Length - 1, 1) == "/" ? str17 : str17 + "/") + str16, ref nw4))
                  {
                    string msg26 = "226 transfer complete \r\n";
                    this.SendMsg(msg26, ref outBuffer);
                    this.textBox_output_message.Text += msg26;
                  }
                  else
                  {
                    string msg27 = "550 file not found, or no access.\r\n";
                    this.SendMsg(msg27, ref outBuffer);
                    this.textBox_output_message.Text += msg27;
                  }
                  PassiveMode = false;
                  goto case "";
                case "TYPE":
                  string msg28 = "200 type set\r\n";
                  this.SendMsg(msg28, ref outBuffer);
                  this.textBox_output_message.Text += msg28;
                  goto case "";
                case "USER":
                  str2 = clientMsg.Substring(4).Trim();
                  bool flag3 = false;
                  if (this.radioButton_todos.Checked)
                  {
                    flag3 = true;
                  }
                  else
                  {
                    try
                    {
                      for (int index = 1; index <= 20; ++index)
                      {
                        string str18 = index.ToString();
                        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str18, false);
                        if (registryKey != null && !((string) registryKey.GetValue("Type") != "ETHERNET"))
                        {
                          string str19 = (string) registryKey.GetValue("User_FTP_Server");
                          str5 = (string) registryKey.GetValue("Password_FTP_Server");
                          str6 = (string) registryKey.GetValue("Directory_FTP_Server");
                          str2 = str2.Trim();
                          if (str19.Trim().ToLower() == str2.ToLower())
                          {
                            flag3 = true;
                            break;
                          }
                        }
                      }
                    }
                    catch (Exception ex)
                    {
                    }
                  }
                  string msg29 = !flag3 ? "530 Not logged in. User name is not on the list\r\n" : "331 Password\r\n";
                  this.SendMsg(msg29, ref outBuffer);
                  this.textBox_output_message.Text += msg29;
                  this.textBox_user.Text = str2;
                  goto case "";
                case "XCUP":
                  string fileName6 = "..";
                  string msg30 = this.ChangeDirectory(str4, ref PresentDirOfFTP, fileName6);
                  this.SendMsg(msg30, ref outBuffer);
                  this.textBox_output_message.Text += msg30;
                  goto case "";
                default:
                  string msg31 = "500 Command not understood\r\n";
                  this.SendMsg(msg31, ref outBuffer);
                  this.textBox_output_message.Text += msg31;
                  goto case "";
              }
            }
          }
          else
          {
            flag1 = true;
            string msg32 = "221 Connection forcefull closed. \r\n";
            this.SendMsg(msg32, ref outBuffer);
            this.textBox_output_message.Text += msg32;
            outBuffer.Close();
            inBuffer.Close();
            clientSocket2.Close();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String14;
          }
        }
        Control.CheckForIllegalCrossThreadCalls = false;
        this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String15;
      }
      catch (Exception ex)
      {
        Control.CheckForIllegalCrossThreadCalls = false;
        this.textBox_detalle_status.Text = Resource_Form_servidor_FTP.String16;
      }
    }

    private string FilePath(string rootDirOnServer, string PresentDirOfFTP, string fileName)
    {
      lock (Thread.CurrentThread)
      {
        fileName = fileName.Trim();
        rootDirOnServer = rootDirOnServer.Trim();
        PresentDirOfFTP = PresentDirOfFTP.Trim();
        rootDirOnServer = rootDirOnServer.Replace("\\", "/");
        PresentDirOfFTP = PresentDirOfFTP.Replace("\\", "/");
        fileName = fileName.Replace("\\", "/");
        string str1 = "";
        string str2;
        if (fileName.LastIndexOf("/") > 0)
        {
          fileName = fileName.Substring(0, 1) != "/" ? "/" + fileName : fileName;
          str2 = rootDirOnServer + fileName;
          str1 = fileName;
        }
        else
        {
          string str3 = rootDirOnServer + PresentDirOfFTP;
          if (str3.Substring(str3.Length - 1, 1) != "/")
            str3 += "/";
          if (fileName.Substring(0, 1) == "/")
            fileName = fileName.Substring(1).Trim();
          string path1 = str3 + fileName;
          try
          {
            if (System.IO.File.Exists(path1))
            {
              str2 = path1;
            }
            else
            {
              string str4 = fileName.Substring(0, 1) != "/" ? "/" + fileName : fileName;
              string path2 = rootDirOnServer + str4;
              str2 = !Directory.Exists(path2) ? str3 + fileName : path2;
            }
          }
          catch (Exception ex)
          {
            str2 = str3 + fileName;
          }
        }
        return str2;
      }
    }

    private string RenameTheFile(string fileName)
    {
      lock (Thread.CurrentThread)
      {
        try
        {
          if (!System.IO.File.Exists(fileName))
            ;
        }
        catch (Exception ex)
        {
        }
        return (string) null;
      }
    }

    private string RemoveDirectory(string rootDirOnServer, string PresentDirOfFTP, string fileName)
    {
      lock (Thread.CurrentThread)
      {
        rootDirOnServer = rootDirOnServer.Replace("\\", "/");
        PresentDirOfFTP = PresentDirOfFTP.Replace("\\", "/");
        fileName = fileName.Replace("\\", "/");
        string str = rootDirOnServer + PresentDirOfFTP;
        if (str.Substring(str.Length - 1, 1) != "/")
          str += "/";
        if (fileName.Substring(0, 1) == "/")
          fileName = fileName.Substring(1).Trim();
        string path = str + fileName;
        try
        {
          if (!Directory.Exists(path))
            return "550 Directory not found, or no access.\r\n";
          Directory.Delete(path, true);
          return "250 Directory deleted.\r\n";
        }
        catch (Exception ex)
        {
          return "550 Command can't be executed.\r\n";
        }
      }
    }

    private string CreateDirectory(string rootDirOnServer, string PresentDirOfFTP, string fileName)
    {
      lock (Thread.CurrentThread)
      {
        string path = this.FilePath(rootDirOnServer, PresentDirOfFTP, fileName);
        try
        {
          if (Directory.Exists(path))
            return "550 Directory already exists!\r\n";
          Directory.CreateDirectory(path);
          return "257 Directory Created.\r\n";
        }
        catch (Exception ex)
        {
          return "550 Command can't executed.\r\n";
        }
      }
    }

    private string ChangeDirectory(
      string rootDirOnServer,
      ref string PresentDirOfFTP,
      string fileName)
    {
      lock (Thread.CurrentThread)
      {
        bool flag = false;
        string path = this.FilePath(rootDirOnServer, PresentDirOfFTP, fileName);
        if (fileName.Length >= 2 && (fileName.Substring(0, 2) == ".." || fileName.IndexOf("..") > 0))
        {
          if (PresentDirOfFTP.Length > 1)
          {
            string str = "";
            int length1 = fileName.Split('/').Length;
            int startIndex = PresentDirOfFTP.Length;
            for (int index = 0; index < length1; ++index)
            {
              int length2 = PresentDirOfFTP.LastIndexOf("/", startIndex);
              str = PresentDirOfFTP.Substring(0, length2);
              startIndex = length2 - 1;
              if (startIndex <= 0)
                break;
            }
            fileName = str;
          }
          else if (PresentDirOfFTP.LastIndexOf("/") == 0)
            fileName = "";
          path = rootDirOnServer + fileName;
          flag = true;
        }
        try
        {
          if (!Directory.Exists(path))
            return "550 Directory not found, or no access.\r\n";
          if (PresentDirOfFTP.Substring(PresentDirOfFTP.Length - 1, 1) == "/")
            PresentDirOfFTP = PresentDirOfFTP.Substring(0, PresentDirOfFTP.Length - 1);
          if (flag)
          {
            PresentDirOfFTP = path.Remove(0, rootDirOnServer.Length);
            if (PresentDirOfFTP == "")
              PresentDirOfFTP = "/";
          }
          else
            PresentDirOfFTP = path.Remove(0, rootDirOnServer.Length);
          return "250 CWD command succesful\r\n";
        }
        catch (IOException ex)
        {
          return "550 Directory not found, or no access.\r\n";
        }
      }
    }

    private string DeleteFileForServer(
      string rootDirOnServer,
      string PresentDirOfFTP,
      string fileName)
    {
      lock (Thread.CurrentThread)
      {
        string path = this.FilePath(rootDirOnServer, PresentDirOfFTP, fileName);
        try
        {
          System.IO.File.Open(path, FileMode.Open).Close();
          System.IO.File.Delete(path);
          return "250 delete command successful\r\n";
        }
        catch (FileNotFoundException ex)
        {
          return "550 file not found, or no access.\r\n";
        }
        catch (IOException ex)
        {
          return "550 file not found, or no access.\r\n";
        }
        catch (Exception ex)
        {
          return "550 file not found, or no access.\r\n";
        }
      }
    }

    private bool ListDirectory(
      string rootDirOnSystem,
      string PresentDirOnFTP,
      ref NetworkStream nw)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(rootDirOnSystem + PresentDirOnFTP);
      FileInfo[] files = directoryInfo1.GetFiles();
      DirectoryInfo[] directories = directoryInfo1.GetDirectories();
      try
      {
        foreach (FileInfo fileInfo in files)
        {
          string str1 = "<FILE> ";
          try
          {
            if (fileInfo.Name.Substring(fileInfo.Name.Length - 4) != ".SYS")
            {
              string str2 = fileInfo.Name.Replace("\\", "/");
              byte[] bytes = Encoding.ASCII.GetBytes(str1 + fileInfo.Length.ToString() + " " + fileInfo.LastWriteTime.ToString() + " " + str2.Trim() + "\r\n");
              try
              {
                if (nw.CanWrite)
                  nw.Write(bytes, 0, bytes.Length);
              }
              catch (Exception ex)
              {
              }
            }
          }
          catch (Exception ex)
          {
          }
        }
        foreach (DirectoryInfo directoryInfo2 in directories)
        {
          string str3 = "<DIR> ";
          if (directoryInfo2.Exists)
          {
            string str4 = directoryInfo2.Name.Replace("\\", "/");
            byte[] bytes = Encoding.ASCII.GetBytes(str3 + "  0    " + directoryInfo2.CreationTime.ToString() + "  " + str4.Trim() + "\r\n");
            try
            {
              if (nw.CanWrite)
                nw.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      catch (IOException ex)
      {
        return false;
      }
      return true;
    }

    private bool ListDirectory_solo_nombres(
      string rootDirOnSystem,
      string PresentDirOnFTP,
      ref NetworkStream nw,
      string argumento)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(rootDirOnSystem + PresentDirOnFTP);
      FileInfo[] files = directoryInfo1.GetFiles();
      DirectoryInfo[] directories = directoryInfo1.GetDirectories();
      try
      {
        foreach (FileInfo fileInfo in files)
        {
          try
          {
            if (fileInfo.Name.Substring(fileInfo.Name.Length - 4) != ".SYS")
            {
              if (argumento == null || !(argumento != fileInfo.Name.ToString()))
              {
                byte[] bytes = Encoding.ASCII.GetBytes(fileInfo.Name.ToString() + "\r\n");
                try
                {
                  if (nw.CanWrite)
                    nw.Write(bytes, 0, bytes.Length);
                }
                catch (Exception ex)
                {
                }
              }
            }
          }
          catch (Exception ex)
          {
          }
        }
        foreach (DirectoryInfo directoryInfo2 in directories)
        {
          if (directoryInfo2.Exists)
          {
            byte[] bytes = Encoding.ASCII.GetBytes(directoryInfo2.Name.ToString() + "\r\n");
            try
            {
              if (nw.CanWrite)
                nw.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      catch (IOException ex)
      {
        return false;
      }
      return true;
    }

    private NetworkStream Mode(
      bool PassiveMode,
      ref TcpClient client,
      ref Socket clientSocket,
      string strIP,
      int Port)
    {
      Thread currentThread = Thread.CurrentThread;
      NetworkStream networkStream = (NetworkStream) null;
      lock (currentThread)
      {
        if (PassiveMode)
        {
          if (clientSocket != null)
            networkStream = new NetworkStream(clientSocket, FileAccess.ReadWrite);
        }
        else
        {
          client = new TcpClient(strIP, Port);
          networkStream = client.GetStream();
        }
      }
      return networkStream;
    }

    private string ReadMsgFromBuffer(ref Socket clientSocket, ref NetworkStream inBuffer)
    {
      string str1 = "";
      StringBuilder stringBuilder = new StringBuilder();
      byte[] numArray = new byte[1024];
      lock (Thread.CurrentThread)
      {
        if (clientSocket.Available > 0)
        {
          while (clientSocket.Available > 0)
          {
            int count = inBuffer.Read(numArray, 0, numArray.Length);
            string str2 = Encoding.ASCII.GetString(numArray, 0, count);
            stringBuilder.Append(str2);
          }
        }
        str1 = stringBuilder.ToString();
      }
      return str1;
    }

    private void SendMsg(string msg, ref NetworkStream outBuffer)
    {
      lock (Thread.CurrentThread)
      {
        byte[] bytes = Encoding.ASCII.GetBytes(msg);
        outBuffer.Write(bytes, 0, bytes.Length);
      }
    }

    private bool SendFile(string strpath, ref NetworkStream nw)
    {
      Thread currentThread = Thread.CurrentThread;
      try
      {
        lock (currentThread)
        {
          StreamReader streamReader = new StreamReader(strpath);
          char[] chArray = new char[1024];
          int count;
          while ((count = streamReader.Read(chArray, 0, 1024)) != 0)
          {
            byte[] bytes = Encoding.ASCII.GetBytes(chArray);
            nw.Write(bytes, 0, count);
          }
        }
      }
      catch (Exception ex)
      {
        nw.Close();
        return false;
      }
      nw.Close();
      return true;
    }

    private bool CreateFile(string strpath, ref NetworkStream nw)
    {
      lock (Thread.CurrentThread)
      {
        try
        {
          StreamWriter streamWriter = new StreamWriter(strpath);
          byte[] numArray = new byte[128];
          int count = 1;
          while (count != 0)
          {
            count = nw.Read(numArray, 0, numArray.Length);
            char[] chars = Encoding.ASCII.GetChars(numArray);
            streamWriter.Write(chars, 0, count);
          }
          streamWriter.Close();
        }
        catch (Exception ex)
        {
          nw.Close();
          return false;
        }
        nw.Close();
        return true;
      }
    }

    private int PassiveModePort(ref TcpListener clientDataListner)
    {
      lock (Thread.CurrentThread)
      {
        int port = 0;
        bool flag = true;
        while (flag)
        {
          port = this.Port();
          try
          {
            if (clientDataListner != null)
              clientDataListner.Stop();
            clientDataListner = new TcpListener(IPAddress.Any, port);
            clientDataListner.Start();
            flag = false;
          }
          catch (Exception ex)
          {
          }
        }
        return port;
      }
    }

    private Socket PassiveClientSocket(ref TcpListener clientDataListner, int intPort)
    {
      lock (Thread.CurrentThread)
      {
        try
        {
          if (clientDataListner.LocalEndpoint != null)
            return clientDataListner.AcceptSocket();
          bool flag = false;
          Socket socket = (Socket) null;
          try
          {
            socket = clientDataListner.AcceptSocket();
            flag = true;
          }
          catch (Exception ex)
          {
          }
          return socket;
        }
        catch (Exception ex)
        {
        }
        return (Socket) null;
      }
    }

    private int Port()
    {
      if (Form_FTP_server.intPort == 0)
        Form_FTP_server.intPort = 1100;
      else
        ++Form_FTP_server.intPort;
      return Form_FTP_server.intPort;
    }

    private void PortData(string clientMsg, out int Port, out string strIP)
    {
      lock (Thread.CurrentThread)
      {
        Port = 0;
        clientMsg = clientMsg.Remove(0, 5);
        clientMsg = clientMsg.Substring(0, clientMsg.Length - 2);
        int length = clientMsg.Length;
        int num1 = 0;
        StringBuilder stringBuilder = new StringBuilder();
        string str = "";
        for (int startIndex = 0; startIndex < length; ++startIndex)
        {
          if (clientMsg.Substring(startIndex, 1) == ",")
          {
            ++num1;
            if (num1 == 4)
            {
              str = clientMsg.Substring(startIndex + 1);
              break;
            }
            stringBuilder.Append(clientMsg.Substring(startIndex, 1));
          }
          else
            stringBuilder.Append(clientMsg.Substring(startIndex, 1));
        }
        strIP = stringBuilder.ToString().Replace(',', '.');
        string[] strArray = str.Split(',');
        if (strArray.Length == 0)
          return;
        int num2 = (int) Convert.ToDecimal(strArray[0]);
        int num3 = (int) Convert.ToDecimal(strArray[1]);
        Port = num2 * 256 + num3;
      }
    }

    private void button_seleccionar_directorio_Click(object sender, EventArgs e)
    {
      try
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.SelectedPath = this.textBox_directorio.Text;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
          return;
        this.textBox_directorio.Text = folderBrowserDialog.SelectedPath;
        if (this.textBox_directorio.Text.Substring(this.textBox_directorio.Text.Length - 1) != "\\")
          this.textBox_directorio.Text += "\\";
        this.textBox_directorio.SelectionStart = this.textBox_directorio.Text.Length;
        this.textBox_directorio.Focus();
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Server_FTP", true);
        if (registryKey == null)
        {
          registryKey = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Server_FTP");
          if (registryKey == null)
          {
            int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String5);
            this.Close();
          }
        }
        registryKey.SetValue("Directory", (object) this.textBox_directorio.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String17 + ex.Message);
      }
    }

    private void button_seleccionar_fichero_Click(object sender, EventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        string fileName = openFileDialog.FileName;
        int startIndex = 0;
        while (true)
        {
          int num = fileName.IndexOf("\\", startIndex + 1);
          if (num != -1)
            startIndex = num;
          else
            break;
        }
        string str = fileName.Remove(startIndex);
        if (MessageBox.Show(Resource_Form_servidor_FTP.String18 + str + "   ", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.textBox_directorio.Text = str + "\\";
          this.textBox_directorio.SelectionStart = this.textBox_directorio.Text.Length;
          this.textBox_directorio.Focus();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String19 + ex.Message);
      }
    }

    private void configuracionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num1 = (int) new Form_config().ShowDialog();
      this.Enabled = true;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_servidor_FTP.String20);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "ETHERNET")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_servidor_FTP.String21);
        }
      }
    }

    private void Form_FTP_server_FormClosed(object sender, FormClosedEventArgs e)
    {
      try
      {
        this.button_disconnect_Click((object) null, (EventArgs) null);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("error :" + ex.Message);
      }
    }

    private void Form_FTP_server_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.servidor_activo)
        return;
      if (MessageBox.Show(Resource_Form_servidor_FTP.String22, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      {
        try
        {
          this.button_disconnect_Click((object) null, (EventArgs) null);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error: " + ex?.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      else
        e.Cancel = true;
    }

    private void button_lista_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      new Form_listado_usuarios().Show();
      this.Enabled = true;
    }

    private void radioButton_todos_CheckedChanged(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Server_FTP", true);
      if (registryKey == null)
      {
        registryKey = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Server_FTP");
        if (registryKey == null)
        {
          int num = (int) MessageBox.Show(Resource_Form_servidor_FTP.String5);
          this.Close();
        }
      }
      if (this.radioButton_todos.Checked)
      {
        this.button_lista.Enabled = false;
        registryKey.SetValue("User_list", (object) "OFF");
      }
      else
      {
        this.button_lista.Enabled = true;
        registryKey.SetValue("User_list", (object) "ON");
      }
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void exploradorToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_explorer().ShowDialog();
      this.Enabled = true;
    }

    private void button_help_FTP_SERVER_Click(object sender, EventArgs e)
    {
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_FTP_SERVER"
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_FTP_server));
      this.button_connect = new Button();
      this.button_disconnect = new Button();
      this.textBox_server_ip = new TextBox();
      this.textBox_server_port = new TextBox();
      this.textBox_output_message = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.textBox_server_name = new TextBox();
      this.label4 = new Label();
      this.textBox_input_message = new TextBox();
      this.textBox_detalle_status = new TextBox();
      this.label6 = new Label();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.configuracionToolStripMenuItem = new ToolStripMenuItem();
      this.exploradorToolStripMenuItem1 = new ToolStripMenuItem();
      this.groupBox_configurar_servidor = new GroupBox();
      this.label_directorio_seleccionado = new Label();
      this.radioButton_lista = new RadioButton();
      this.textBox_directorio = new TextBox();
      this.button_seleccionar_fichero = new Button();
      this.button_lista = new Button();
      this.button_seleccionar_directorio = new Button();
      this.radioButton_todos = new RadioButton();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label5 = new Label();
      this.groupBox_status = new GroupBox();
      this.label15 = new Label();
      this.label9 = new Label();
      this.textBox_user = new TextBox();
      this.button_help_FTP_SERVER = new Button();
      this.menuStrip1.SuspendLayout();
      this.groupBox_configurar_servidor.SuspendLayout();
      this.groupBox_status.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.button_connect, "button_connect");
      this.button_connect.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_connect.Name = "button_connect";
      this.button_connect.UseVisualStyleBackColor = false;
      this.button_connect.Click += new EventHandler(this.button_connect_Click);
      componentResourceManager.ApplyResources((object) this.button_disconnect, "button_disconnect");
      this.button_disconnect.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_disconnect.Name = "button_disconnect";
      this.button_disconnect.UseVisualStyleBackColor = false;
      this.button_disconnect.Click += new EventHandler(this.button_disconnect_Click);
      componentResourceManager.ApplyResources((object) this.textBox_server_ip, "textBox_server_ip");
      this.textBox_server_ip.Name = "textBox_server_ip";
      this.textBox_server_ip.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBox_server_port, "textBox_server_port");
      this.textBox_server_port.Name = "textBox_server_port";
      componentResourceManager.ApplyResources((object) this.textBox_output_message, "textBox_output_message");
      this.textBox_output_message.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.textBox_output_message.Name = "textBox_output_message";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.textBox_server_name, "textBox_server_name");
      this.textBox_server_name.Name = "textBox_server_name";
      this.textBox_server_name.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.textBox_input_message, "textBox_input_message");
      this.textBox_input_message.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.textBox_input_message.Name = "textBox_input_message";
      componentResourceManager.ApplyResources((object) this.textBox_detalle_status, "textBox_detalle_status");
      this.textBox_detalle_status.ForeColor = System.Drawing.Color.Red;
      this.textBox_detalle_status.Name = "textBox_detalle_status";
      this.textBox_detalle_status.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configuracionToolStripMenuItem,
        (ToolStripItem) this.exploradorToolStripMenuItem1
      });
      this.menuStrip1.Name = "menuStrip1";
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.salirToolStripMenuItem
      });
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.configuracionToolStripMenuItem, "configuracionToolStripMenuItem");
      this.configuracionToolStripMenuItem.Image = (Image) Resources.CONNECT2;
      this.configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
      this.configuracionToolStripMenuItem.Click += new EventHandler(this.configuracionToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.exploradorToolStripMenuItem1, "exploradorToolStripMenuItem1");
      this.exploradorToolStripMenuItem1.Image = (Image) Resources.DIR10A;
      this.exploradorToolStripMenuItem1.Name = "exploradorToolStripMenuItem1";
      this.exploradorToolStripMenuItem1.Click += new EventHandler(this.exploradorToolStripMenuItem1_Click);
      componentResourceManager.ApplyResources((object) this.groupBox_configurar_servidor, "groupBox_configurar_servidor");
      this.groupBox_configurar_servidor.Controls.Add((Control) this.label_directorio_seleccionado);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.radioButton_lista);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.textBox_directorio);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.button_seleccionar_fichero);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.button_lista);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.button_seleccionar_directorio);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.textBox_server_port);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.radioButton_todos);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.label2);
      this.groupBox_configurar_servidor.Name = "groupBox_configurar_servidor";
      this.groupBox_configurar_servidor.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label_directorio_seleccionado, "label_directorio_seleccionado");
      this.label_directorio_seleccionado.Name = "label_directorio_seleccionado";
      componentResourceManager.ApplyResources((object) this.radioButton_lista, "radioButton_lista");
      this.radioButton_lista.Name = "radioButton_lista";
      this.radioButton_lista.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.textBox_directorio, "textBox_directorio");
      this.textBox_directorio.Name = "textBox_directorio";
      componentResourceManager.ApplyResources((object) this.button_seleccionar_fichero, "button_seleccionar_fichero");
      this.button_seleccionar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_seleccionar_fichero.Name = "button_seleccionar_fichero";
      this.button_seleccionar_fichero.UseVisualStyleBackColor = false;
      this.button_seleccionar_fichero.Click += new EventHandler(this.button_seleccionar_fichero_Click);
      componentResourceManager.ApplyResources((object) this.button_lista, "button_lista");
      this.button_lista.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_lista.Name = "button_lista";
      this.button_lista.UseVisualStyleBackColor = false;
      this.button_lista.Click += new EventHandler(this.button_lista_Click);
      componentResourceManager.ApplyResources((object) this.button_seleccionar_directorio, "button_seleccionar_directorio");
      this.button_seleccionar_directorio.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_seleccionar_directorio.Name = "button_seleccionar_directorio";
      this.button_seleccionar_directorio.UseVisualStyleBackColor = false;
      this.button_seleccionar_directorio.Click += new EventHandler(this.button_seleccionar_directorio_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_todos, "radioButton_todos");
      this.radioButton_todos.Checked = true;
      this.radioButton_todos.Name = "radioButton_todos";
      this.radioButton_todos.TabStop = true;
      this.radioButton_todos.UseVisualStyleBackColor = true;
      this.radioButton_todos.CheckedChanged += new EventHandler(this.radioButton_todos_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.BorderStyle = BorderStyle.FixedSingle;
      this.label8.ForeColor = System.Drawing.Color.Red;
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.groupBox_status, "groupBox_status");
      this.groupBox_status.Controls.Add((Control) this.label15);
      this.groupBox_status.Controls.Add((Control) this.label9);
      this.groupBox_status.Controls.Add((Control) this.textBox_user);
      this.groupBox_status.Controls.Add((Control) this.textBox_detalle_status);
      this.groupBox_status.Controls.Add((Control) this.textBox_input_message);
      this.groupBox_status.Controls.Add((Control) this.label5);
      this.groupBox_status.Controls.Add((Control) this.textBox_output_message);
      this.groupBox_status.Controls.Add((Control) this.label6);
      this.groupBox_status.Controls.Add((Control) this.label4);
      this.groupBox_status.Controls.Add((Control) this.textBox_server_name);
      this.groupBox_status.Controls.Add((Control) this.textBox_server_ip);
      this.groupBox_status.Controls.Add((Control) this.label3);
      this.groupBox_status.Name = "groupBox_status";
      this.groupBox_status.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label15, "label15");
      this.label15.Name = "label15";
      componentResourceManager.ApplyResources((object) this.label9, "label9");
      this.label9.Name = "label9";
      componentResourceManager.ApplyResources((object) this.textBox_user, "textBox_user");
      this.textBox_user.ForeColor = System.Drawing.Color.Red;
      this.textBox_user.Name = "textBox_user";
      this.textBox_user.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.button_help_FTP_SERVER, "button_help_FTP_SERVER");
      this.button_help_FTP_SERVER.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.button_help_FTP_SERVER.ForeColor = System.Drawing.Color.Red;
      this.button_help_FTP_SERVER.Name = "button_help_FTP_SERVER";
      this.button_help_FTP_SERVER.UseVisualStyleBackColor = false;
      this.button_help_FTP_SERVER.Click += new EventHandler(this.button_help_FTP_SERVER_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.button_help_FTP_SERVER);
      this.Controls.Add((Control) this.groupBox_status);
      this.Controls.Add((Control) this.button_disconnect);
      this.Controls.Add((Control) this.button_connect);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.groupBox_configurar_servidor);
      this.Controls.Add((Control) this.label8);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_FTP_server);
      this.FormClosing += new FormClosingEventHandler(this.Form_FTP_server_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.Form_FTP_server_FormClosed);
      this.Load += new EventHandler(this.Form_FTP_server_load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBox_configurar_servidor.ResumeLayout(false);
      this.groupBox_configurar_servidor.PerformLayout();
      this.groupBox_status.ResumeLayout(false);
      this.groupBox_status.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
