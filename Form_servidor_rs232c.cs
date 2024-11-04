// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_servidor_rs232c
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Enviar;
using FANUC_Open_Com.Servidor_CNC;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_servidor_rs232c : Form
  {
    private bool parar_envio = false;
    private bool cancelar_envio = false;
    private bool error_puerto_serie = false;
    private bool modo_servidor = false;
    private bool error_comando = false;
    private string mensaje_error = (string) null;
    private string nombre_fichero = (string) null;
    private string nombre_directorio_trabajo = (string) null;
    private bool orden_enviar = false;
    private bool orden_listar_dir = false;
    private bool orden_recibir = false;
    private bool orden_borrar = false;
    private bool orden_renombrar = false;
    private bool orden_copiar = false;
    private bool orden_directorio_trabajo = false;
    private bool timer_normal_end = false;
    private bool timer_abnormal_end = false;
    private string old_path = (string) null;
    private IContainer components = (IContainer) null;
    private SerialPort serialPort1;
    private OpenFileDialog openFileDialog1;
    private TextBox textBox_directorio;
    private ProgressBar progressBar1;
    private Label label1;
    private Button button_parar_envio;
    private Button button_continuar_envio;
    private Button button_cancelar_envio;
    private RichTextBox richTextBox_enviar;
    private GroupBox groupBox_control_envio;
    private GroupBox groupBox_configurar_servidor;
    private Button button_seleccionar_directorio;
    private FolderBrowserDialog folderBrowserDialog1;
    private Button button_seleccionar_fichero;
    private RichTextBox richTextBox_ordenes;
    private Button button_modo_servidor;
    private ComboBox comboBox_tiempo;
    private Button button_cancelar_servidor;
    private Label label_orden_recibida;
    private Label label2;
    private Panel panel_configuracion;
    private System.Timers.Timer timer_responder_orden;
    private Label label4;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private ToolStripMenuItem configuracionToolStripMenuItem;
    private ToolStripMenuItem informacionToolStripMenuItem;
    private System.Timers.Timer timer_end_recieve;
    private System.Timers.Timer timer_limit_receive;
    private TextBox textBox_nombre_maquina;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label3;
    private GroupBox groupBox_status;
    private Button button_help_CNC_SERVER;

    public Form_servidor_rs232c() => this.InitializeComponent();

    private void Form_servidor_Load(object sender, EventArgs e)
    {
      try
      {
        this.old_path = Directory.GetCurrentDirectory();
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
          return;
        string str = (string) registryKey1.GetValue("Machine");
        RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str + "\\Server_RS232C", false);
        if (registryKey2 != null)
        {
          this.textBox_directorio.Text = (string) registryKey2.GetValue("Directory");
          this.comboBox_tiempo.Text = (string) registryKey2.GetValue("Timer");
        }
        else
        {
          this.textBox_directorio.Text = "";
          this.comboBox_tiempo.Text = "30";
        }
        RegistryKey registryKey3 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false);
        if (registryKey3 != null)
          this.textBox_nombre_maquina.Text = (string) registryKey3.GetValue("Name");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void Form_servidor_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.serialPort1.IsOpen)
        this.serialPort1.Close();
      Directory.SetCurrentDirectory(this.old_path);
    }

    private void Form_servidor_rs232c_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.modo_servidor)
        return;
      if (MessageBox.Show(Resource_Form_servidor_rs232c.String30, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      {
        this.cancelar_envio = true;
        try
        {
          this.serialPort1.DiscardOutBuffer();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error: " + ex?.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      else
        e.Cancel = true;
    }

    private void configuracionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.modo_servidor)
        return;
      this.Enabled = false;
      int num1 = (int) new Form_config().ShowDialog();
      this.Enabled = true;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String1);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "RS232C")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String2);
        }
        this.Form_servidor_Load((object) null, (EventArgs) null);
      }
    }

    private int configurar()
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      string str1 = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false);
      if (registryKey2 == null)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      try
      {
        if (this.serialPort1.IsOpen)
          this.serialPort1.Close();
        this.serialPort1.PortName = (string) registryKey2.GetValue("Port");
        this.serialPort1.BaudRate = Convert.ToInt32(registryKey2.GetValue("Baud"));
        this.serialPort1.DataBits = (int) Convert.ToInt16(registryKey2.GetValue("Data"));
        this.serialPort1.StopBits = !((string) registryKey2.GetValue("Stop") == "1") ? StopBits.Two : StopBits.One;
        this.serialPort1.Parity = Parity.None;
        string str2 = ((string) registryKey2.GetValue("Parity")).Substring(0, 1);
        if (str2 == "e" || str2 == "E")
          this.serialPort1.Parity = Parity.Even;
        if (str2 == "o" || str2 == "O")
          this.serialPort1.Parity = Parity.Odd;
        this.serialPort1.Handshake = Handshake.None;
        if ((string) registryKey2.GetValue("Protocol") == "Software")
          this.serialPort1.Handshake = Handshake.XOnXOff;
        if ((string) registryKey2.GetValue("Protocol") == "Hardware")
          this.serialPort1.Handshake = Handshake.RequestToSend;
        if ((string) registryKey2.GetValue("Protocol") == "Hardware-Software")
          this.serialPort1.Handshake = Handshake.RequestToSendXOnXOff;
        this.serialPort1.WriteBufferSize = 2048;
        this.serialPort1.ReadBufferSize = 2048;
        this.serialPort1.ReadTimeout = 5000;
        this.serialPort1.WriteTimeout = 600000;
        this.serialPort1.ReceivedBytesThreshold = 10;
        this.serialPort1.Open();
        this.serialPort1.DiscardOutBuffer();
        this.serialPort1.DiscardInBuffer();
        this.serialPort1.DtrEnable = true;
        return 0;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String8 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
    }

    private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      Control.CheckForIllegalCrossThreadCalls = false;
      RichTextBox richTextBoxOrdenes = this.richTextBox_ordenes;
      richTextBoxOrdenes.Text = richTextBoxOrdenes.Text + "\n" + Resource_Form_servidor_rs232c.String6 + e.EventType.ToString();
      RichTextBox richTextBoxEnviar = this.richTextBox_enviar;
      richTextBoxEnviar.Text = richTextBoxEnviar.Text + "\n" + Resource_Form_servidor_rs232c.String6 + e.EventType.ToString();
      this.error_puerto_serie = true;
      this.serialPort1.DiscardInBuffer();
      this.serialPort1.DiscardOutBuffer();
      Control.CheckForIllegalCrossThreadCalls = true;
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resource_Form_servidor_rs232c.String5, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      try
      {
        if (this.serialPort1.IsOpen)
        {
          this.serialPort1.DiscardOutBuffer();
          this.serialPort1.Close();
        }
        this.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String6 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void button_parar_envio_Click(object sender, EventArgs e) => this.parar_envio = true;

    private void button_continuar_envio_Click(object sender, EventArgs e)
    {
      this.parar_envio = false;
    }

    private void button_cancelar_envio_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resource_Form_servidor_rs232c.String7, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.cancelar_envio = true;
      this.serialPort1.DiscardOutBuffer();
    }

    private void button_modo_servidor_Click(object sender, EventArgs e)
    {
      if (this.textBox_directorio.Text == "")
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.textBox_directorio.Focus();
      }
      else if (!Directory.Exists(this.textBox_directorio.Text))
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String9, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.textBox_directorio.Focus();
      }
      else if (this.comboBox_tiempo.Text == "")
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String10, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.comboBox_tiempo.Focus();
      }
      else
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
        {
          int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String11);
          this.Close();
        }
        string str = (string) registryKey1.GetValue("Machine");
        RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str + "\\Server_RS232C", true);
        if (registryKey2 == null)
        {
          registryKey2 = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str + "\\Server_RS232C");
          if (registryKey2 == null)
          {
            int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String11);
            this.Close();
          }
        }
        registryKey2.SetValue("Directory", (object) this.textBox_directorio.Text);
        registryKey2.SetValue("Timer", (object) this.comboBox_tiempo.Text);
        try
        {
          if (this.configurar() == -1)
            return;
          Directory.SetCurrentDirectory(this.textBox_directorio.Text.ToString());
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String12 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          if (!this.serialPort1.IsOpen)
            return;
          this.serialPort1.Close();
          return;
        }
        this.timer_responder_orden.Interval = (double) (Convert.ToInt32(this.comboBox_tiempo.Text) * 1000);
        this.parar_envio = false;
        this.cancelar_envio = false;
        this.error_puerto_serie = false;
        this.modo_servidor = true;
        this.error_comando = false;
        this.mensaje_error = (string) null;
        this.nombre_fichero = (string) null;
        this.orden_enviar = false;
        this.orden_listar_dir = false;
        this.orden_recibir = false;
        this.orden_borrar = false;
        this.orden_renombrar = false;
        this.orden_copiar = false;
        this.orden_directorio_trabajo = false;
        this.label3.Text = Resource_Form_servidor_rs232c.String13;
        this.button_modo_servidor.Enabled = false;
        this.button_cancelar_servidor.Enabled = true;
        this.groupBox_configurar_servidor.Enabled = false;
        this.groupBox_control_envio.Enabled = true;
        this.groupBox_status.Enabled = true;
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
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String14 + ex.Message);
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
        if (MessageBox.Show(Resource_Form_servidor_rs232c.String15 + str + "   ", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.textBox_directorio.Text = str + "\\";
          this.textBox_directorio.SelectionStart = this.textBox_directorio.Text.Length;
          this.textBox_directorio.Focus();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String16 + ex.Message);
      }
    }

    private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      if (this.error_puerto_serie)
      {
        this.orden_enviar = false;
        this.orden_listar_dir = false;
        this.orden_recibir = false;
        this.orden_borrar = false;
        this.orden_renombrar = false;
        this.orden_copiar = false;
        this.orden_directorio_trabajo = false;
        this.error_puerto_serie = false;
      }
      else
      {
        if (this.orden_recibir)
          return;
        this.cancelar_envio = true;
        this.serialPort1.DiscardOutBuffer();
        string str1 = "(R=";
        string str2 = ")";
        string str3 = "(S=";
        string str4 = "(L=";
        string str5 = "(F=";
        string str6 = "(D=";
        string str7 = "(N=";
        string str8 = "(C=";
        string str9 = ",";
        string str10 = "(W=";
        string str11 = "O8888";
        string str12 = ":8888";
        string str13 = (string) null;
        Control.CheckForIllegalCrossThreadCalls = false;
        this.richTextBox_ordenes.Text = "";
        Control.CheckForIllegalCrossThreadCalls = true;
        int num1 = 0;
        this.timer_responder_orden.Stop();
        Thread.Sleep(1000);
        try
        {
          int num2 = 0;
          while (this.serialPort1.BytesToRead > 0)
          {
            Application.DoEvents();
            string str14 = str13 + this.serialPort1.ReadExisting();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.richTextBox_ordenes.Text = str14;
            this.richTextBox_enviar.Text = "";
            Control.CheckForIllegalCrossThreadCalls = true;
            str13 = str14.Trim();
            if (str13.IndexOf(str11) >= 0 || str13.IndexOf(str12) >= 0)
            {
              str1 = "R";
              str2 = "\n";
              str3 = "S";
              str4 = "L";
              str5 = "F";
              str6 = "D";
              str7 = "N";
              str8 = "C";
              str9 = ",";
            }
            num1 = str13.IndexOf(str1);
            if (num1 >= 0)
            {
              this.orden_enviar = true;
              num2 = str1.Length;
              break;
            }
            num1 = str13.IndexOf(str4);
            if (num1 >= 0)
            {
              this.orden_listar_dir = true;
              num2 = str4.Length;
              break;
            }
            num1 = str13.IndexOf(str5);
            if (num1 >= 0)
            {
              this.orden_listar_dir = true;
              num2 = str5.Length;
              break;
            }
            num1 = str13.IndexOf(str3);
            if (num1 >= 0)
            {
              this.orden_recibir = true;
              num2 = str3.Length;
              break;
            }
            num1 = str13.IndexOf(str6);
            if (num1 >= 0)
            {
              this.orden_borrar = true;
              num2 = str6.Length;
              break;
            }
            num1 = str13.IndexOf(str7);
            if (num1 >= 0)
            {
              this.orden_renombrar = true;
              num2 = str7.Length;
              break;
            }
            num1 = str13.IndexOf(str8);
            if (num1 >= 0)
            {
              this.orden_copiar = true;
              num2 = str8.Length;
              break;
            }
            num1 = str13.IndexOf(str10);
            if (num1 >= 0)
            {
              this.orden_directorio_trabajo = true;
              num2 = str10.Length;
              break;
            }
          }
          if (this.serialPort1.BytesToRead == 0 && !this.orden_enviar && !this.orden_listar_dir && !this.orden_recibir && !this.orden_borrar && !this.orden_renombrar && !this.orden_copiar && !this.orden_directorio_trabajo)
          {
            this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
            this.enviar_error();
          }
          else
          {
            string str15 = str13;
            if (this.orden_enviar || this.orden_recibir || this.orden_borrar)
            {
              int startIndex = num1 + num2;
              int num3 = str15.IndexOf(str2, startIndex);
              if (num3 <= 0 || num3 - startIndex <= 0)
              {
                this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
                this.enviar_error();
                return;
              }
              this.nombre_fichero = str15.Substring(startIndex, num3 - startIndex);
              this.nombre_fichero = this.textBox_directorio.Text + this.nombre_fichero;
              if (!File.Exists(this.nombre_fichero) && !this.orden_recibir)
              {
                this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
                this.enviar_error();
                return;
              }
              if (this.orden_borrar)
              {
                File.Delete(this.nombre_fichero);
                Control.CheckForIllegalCrossThreadCalls = false;
                this.richTextBox_enviar.Text = "OK";
              }
            }
            if (this.orden_renombrar || this.orden_copiar)
            {
              int startIndex1 = num1 + num2;
              int num4 = str15.IndexOf(str9, startIndex1);
              if (num4 <= 0 || num4 - startIndex1 <= 0)
              {
                this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
                this.enviar_error();
                return;
              }
              int startIndex2 = num4 + 1;
              int num5 = str15.IndexOf(str2, startIndex2);
              if (num5 <= 0 || num5 - startIndex2 <= 0)
              {
                this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
                this.enviar_error();
                return;
              }
              string str16 = this.textBox_directorio.Text + str15.Substring(startIndex1, num4 - startIndex1);
              string destFileName = this.textBox_directorio.Text + str15.Substring(startIndex2, num5 - startIndex2);
              if (!File.Exists(str16))
              {
                this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
                this.enviar_error();
                return;
              }
              if (this.orden_renombrar)
                File.Move(str16, destFileName);
              if (this.orden_copiar)
                File.Copy(str16, destFileName);
              Control.CheckForIllegalCrossThreadCalls = false;
              this.richTextBox_enviar.Text = "OK";
            }
            if ((this.orden_enviar || this.orden_listar_dir) && !this.timer_responder_orden.Enabled)
            {
              Control.CheckForIllegalCrossThreadCalls = false;
              this.timer_responder_orden.Interval = (double) (Convert.ToInt32(this.comboBox_tiempo.Text) * 1000);
              this.timer_responder_orden.Start();
            }
            if (this.orden_recibir && !this.timer_responder_orden.Enabled)
            {
              Control.CheckForIllegalCrossThreadCalls = false;
              this.timer_responder_orden.Interval = 100.0;
              this.timer_responder_orden.Start();
            }
            if (!this.orden_directorio_trabajo)
              return;
            int startIndex3 = num1 + num2;
            int num6 = str15.IndexOf(str2, startIndex3);
            if (num6 <= 0 || num6 - startIndex3 <= 0)
            {
              this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
              this.enviar_error();
            }
            else
            {
              this.nombre_directorio_trabajo = str15.Substring(startIndex3, num6 - startIndex3);
              this.nombre_directorio_trabajo = this.nombre_directorio_trabajo.Replace("/", "\\");
              if (!Directory.Exists(this.nombre_directorio_trabajo))
              {
                this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
                this.enviar_error();
              }
              else
              {
                this.nombre_directorio_trabajo = Path.GetFullPath(this.nombre_directorio_trabajo);
                if (this.nombre_directorio_trabajo.Substring(this.nombre_directorio_trabajo.Length - 1) != "\\")
                  this.nombre_directorio_trabajo += "\\";
                RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
                if (registryKey1 == null)
                {
                  int num7 = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String11);
                  this.Close();
                }
                string str17 = (string) registryKey1.GetValue("Machine");
                RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str17 + "\\Server_RS232C", true);
                if (registryKey2 == null)
                {
                  registryKey2 = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str17 + "\\Server_RS232C");
                  if (registryKey2 == null)
                  {
                    int num8 = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String11);
                    this.Close();
                  }
                }
                registryKey2.SetValue("Directory", (object) this.nombre_directorio_trabajo);
                this.textBox_directorio.Text = this.nombre_directorio_trabajo;
                Directory.SetCurrentDirectory(this.nombre_directorio_trabajo);
              }
            }
          }
        }
        catch (Exception ex)
        {
          this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String29 + "M00\n%";
          this.enviar_error();
          Control.CheckForIllegalCrossThreadCalls = false;
          this.richTextBox_enviar.Text = Resource_Form_servidor_rs232c.String29 + " : " + ex.Message;
        }
        finally
        {
          this.serialPort1.DiscardInBuffer();
        }
      }
    }

    private void enviar_error()
    {
      this.error_comando = true;
      this.orden_enviar = false;
      this.orden_listar_dir = false;
      this.orden_recibir = false;
      this.orden_borrar = false;
      this.orden_renombrar = false;
      this.orden_copiar = false;
      Control.CheckForIllegalCrossThreadCalls = false;
      this.timer_responder_orden.Interval = (double) (Convert.ToInt32(this.comboBox_tiempo.Text) * 1000);
      this.richTextBox_enviar.Text = "Error";
      Control.CheckForIllegalCrossThreadCalls = true;
      this.timer_responder_orden.Start();
    }

    private void timer_responder_orden_Elapsed(object sender, ElapsedEventArgs e)
    {
      new Thread(new ThreadStart(this.responder_orden)).Start();
      this.timer_responder_orden.Interval = (double) (Convert.ToInt32(this.comboBox_tiempo.Text) * 1000);
      this.timer_responder_orden.Stop();
    }

    private void responder_orden()
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
      this.timer_responder_orden.Stop();
      Control.CheckForIllegalCrossThreadCalls = false;
      this.richTextBox_enviar.Text = "";
      if (this.error_comando)
      {
        try
        {
          this.serialPort1.DiscardOutBuffer();
          this.serialPort1.WriteLine(this.mensaje_error);
          Control.CheckForIllegalCrossThreadCalls = false;
          this.richTextBox_enviar.Text = this.mensaje_error;
          this.error_comando = false;
        }
        catch (Exception ex)
        {
          Control.CheckForIllegalCrossThreadCalls = false;
          this.richTextBox_enviar.Text = Resource_Form_servidor_rs232c.String17 + ex.Message;
          this.error_comando = false;
        }
      }
      else if (!this.orden_enviar && !this.orden_listar_dir && !this.orden_recibir)
      {
        int num = (int) MessageBox.Show(Resource_Form_servidor_rs232c.String18);
      }
      else if (this.orden_enviar)
        this.ejecutar_orden_enviar();
      else if (this.orden_listar_dir)
      {
        this.ejecutar_orden_listar();
      }
      else
      {
        if (!this.orden_recibir)
          return;
        this.ejecutar_orden_recibir();
      }
    }

    private void button_cancelar_servidor_Click(object sender, EventArgs e)
    {
      this.modo_servidor = false;
      this.label3.Text = Resource_Form_servidor_rs232c.String27;
      this.button_modo_servidor.Enabled = true;
      this.button_cancelar_servidor.Enabled = false;
      this.groupBox_configurar_servidor.Enabled = true;
      this.groupBox_control_envio.Enabled = false;
      this.groupBox_status.Enabled = false;
    }

    private void informacionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_informacion_servidor_rs232c().ShowDialog();
      this.Enabled = true;
    }

    private void ejecutar_orden_enviar()
    {
      this.orden_enviar = false;
      FileStream fileStream;
      long length1;
      try
      {
        fileStream = new FileStream(this.nombre_fichero, FileMode.Open, FileAccess.Read, FileShare.None);
        if (fileStream.Length > 10000L && new Informacion().leer_clave(true) <= 0)
        {
          this.mensaje_error = "%\nO0000" + Resource_Form_servidor_rs232c.String21 + "M00\n%";
          this.serialPort1.WriteLine(this.mensaje_error);
          this.richTextBox_enviar.Text = this.mensaje_error;
          fileStream.Close();
          return;
        }
        length1 = fileStream.Length;
      }
      catch (Exception ex)
      {
        Control.CheckForIllegalCrossThreadCalls = false;
        this.richTextBox_enviar.Text = Resource_Form_servidor_rs232c.String22 + ex.Message;
        Control.CheckForIllegalCrossThreadCalls = true;
        return;
      }
      int length2 = 1000;
      byte[] numArray1 = new byte[length2];
      try
      {
        this.serialPort1.DiscardOutBuffer();
        int num1 = length2;
        int num2 = 10;
        int length3 = num2;
        byte[] numArray2 = new byte[length3];
        int num3 = num1 / length3;
        int num4 = num1 % length3;
        this.parar_envio = false;
        this.cancelar_envio = false;
        this.progressBar1.Maximum = (int) length1;
        this.progressBar1.Value = 0;
        this.label1.Text = Resource_Form_servidor_rs232c.String23;
        this.label1.Update();
        this.richTextBox_enviar.ResetText();
        this.button_seleccionar_fichero.Enabled = false;
        this.button_parar_envio.Enabled = true;
        this.button_continuar_envio.Enabled = false;
        this.button_cancelar_envio.Enabled = true;
        this.button_parar_envio.Focus();
        do
        {
          Array.Clear((Array) numArray1, 0, length2);
          int count = num2;
          int num5 = fileStream.Read(numArray1, 0, length2);
          if (num5 != 0)
          {
            if (num5 < length2)
            {
              int num6 = num5;
              num3 = num6 / count;
              num4 = num6 % count;
            }
            for (int index = 0; index <= num3; ++index)
            {
              int srcOffset = index * count;
              if (index == num3)
              {
                count = num4;
                if (num4 == 0)
                  break;
              }
              Buffer.BlockCopy((Array) numArray1, srcOffset, (Array) numArray2, 0, count);
              this.serialPort1.Write(numArray2, 0, count);
              Control.CheckForIllegalCrossThreadCalls = false;
              this.label1.Text = Resource_Form_servidor_rs232c.String24 + this.progressBar1.Value.ToString();
              this.progressBar1.Value += count;
              Application.DoEvents();
              if (this.parar_envio)
              {
                this.button_continuar_envio.Enabled = true;
                this.button_parar_envio.Enabled = false;
                this.button_continuar_envio.Focus();
                do
                {
                  Application.DoEvents();
                  if (this.cancelar_envio)
                    goto label_18;
                }
                while (this.parar_envio);
                this.button_continuar_envio.Enabled = false;
                this.button_parar_envio.Enabled = true;
                this.button_parar_envio.Focus();
label_18:;
              }
              if (this.cancelar_envio)
                break;
            }
            this.richTextBox_enviar.Text += Encoding.ASCII.GetString(numArray1);
            this.richTextBox_enviar.SelectionStart += this.richTextBox_enviar.Text.Length;
            this.richTextBox_enviar.ScrollToCaret();
          }
          else
            goto label_25;
        }
        while (!this.cancelar_envio);
        goto label_24;
label_25:
        return;
label_24:;
      }
      catch (Exception ex)
      {
        if (this.cancelar_envio)
          return;
        this.richTextBox_enviar.Text = Resource_Form_servidor_rs232c.String25 + ex.Message;
      }
      finally
      {
        if (!this.cancelar_envio)
        {
          this.label1.Text = Resource_Form_servidor_rs232c.String26 + this.progressBar1.Value.ToString();
          this.label1.Update();
        }
        fileStream.Close();
        Control.CheckForIllegalCrossThreadCalls = true;
        this.orden_enviar = false;
        this.orden_listar_dir = false;
        this.orden_recibir = false;
      }
    }

    private void ejecutar_orden_listar()
    {
      try
      {
        this.orden_listar_dir = false;
        FileInfo[] files = new DirectoryInfo(this.textBox_directorio.Text).GetFiles();
        this.richTextBox_enviar.Text = "";
        string text1 = "%\nO0000M00";
        this.serialPort1.DiscardOutBuffer();
        this.serialPort1.WriteLine(text1);
        this.richTextBox_enviar.Text = text1 + "\n";
        foreach (object obj in files)
        {
          string text2 = ("(" + obj.ToString()).ToUpper() + ")";
          this.serialPort1.WriteLine(text2);
          RichTextBox richTextBoxEnviar = this.richTextBox_enviar;
          richTextBoxEnviar.Text = richTextBoxEnviar.Text + text2 + "\n";
        }
        string text3 = "M00\n%";
        this.serialPort1.WriteLine(text3);
        RichTextBox richTextBoxEnviar1 = this.richTextBox_enviar;
        richTextBoxEnviar1.Text = richTextBoxEnviar1.Text + text3 + "\n";
        this.progressBar1.Value = this.progressBar1.Maximum;
        this.label1.Text = Resource_Form_servidor_rs232c.String19;
        this.label1.Update();
      }
      catch (Exception ex)
      {
        Control.CheckForIllegalCrossThreadCalls = false;
        this.richTextBox_enviar.Text = "Error: " + ex.Message;
        Control.CheckForIllegalCrossThreadCalls = true;
      }
      finally
      {
        this.orden_enviar = false;
        this.orden_listar_dir = false;
        this.orden_recibir = false;
      }
    }

    private void ejecutar_orden_recibir()
    {
      FileStream fileStream;
      try
      {
        fileStream = new FileStream(this.nombre_fichero, FileMode.Create, FileAccess.Write, FileShare.None);
      }
      catch (Exception ex)
      {
        Control.CheckForIllegalCrossThreadCalls = false;
        this.richTextBox_enviar.Text = "Error: " + ex.Message;
        Control.CheckForIllegalCrossThreadCalls = true;
        return;
      }
      int count1 = 1000;
      byte[] numArray1 = new byte[count1];
      long num = 0;
      this.progressBar1.Maximum = 10000;
      byte[] numArray2 = new byte[10000 + count1];
      try
      {
        this.serialPort1.DiscardInBuffer();
        this.timer_limit_receive.Interval = (double) (Convert.ToInt32(this.comboBox_tiempo.Text) * 1000);
        this.timer_limit_receive.Start();
        int count2 = 0;
        this.timer_normal_end = false;
        this.timer_abnormal_end = false;
        this.error_puerto_serie = false;
        bool flag = false;
        do
        {
          Array.Clear((Array) numArray1, 0, numArray1.Length);
          if (flag && !this.timer_end_recieve.Enabled)
          {
            this.timer_end_recieve.Start();
            this.timer_limit_receive.Stop();
          }
          if (this.serialPort1.BytesToRead > 0)
          {
            flag = true;
            this.timer_end_recieve.Stop();
            int count3 = this.serialPort1.Read(numArray1, 0, count1);
            Application.DoEvents();
            if (!this.error_puerto_serie)
            {
              num += (long) count3;
              this.label1.Text = Resource_Form_servidor_rs232c.String31 + num.ToString();
              count2 += count3;
              Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, count2 - count3, count3);
              if (count2 >= 10000)
              {
                fileStream.Write(numArray2, 0, count2);
                count2 = 0;
              }
              this.progressBar1.Value = count2;
            }
            else
              goto label_18;
          }
          Application.DoEvents();
          if (this.timer_normal_end)
          {
            fileStream.Write(numArray2, 0, count2);
            this.timer_normal_end = false;
            this.richTextBox_enviar.Text += "\n End OK";
            return;
          }
        }
        while (!this.timer_abnormal_end);
        goto label_15;
label_18:
        return;
label_15:
        this.timer_abnormal_end = false;
        this.richTextBox_enviar.Text += "\n Abnormal End\n Limit Timer";
      }
      catch (Exception ex)
      {
        Control.CheckForIllegalCrossThreadCalls = false;
        this.richTextBox_enviar.Text = "Error: " + ex.Message;
        Control.CheckForIllegalCrossThreadCalls = true;
      }
      finally
      {
        fileStream.Close();
        this.orden_enviar = false;
        this.orden_listar_dir = false;
        this.orden_recibir = false;
        this.timer_end_recieve.Stop();
        this.timer_limit_receive.Stop();
        this.timer_normal_end = false;
        this.timer_abnormal_end = false;
        this.error_puerto_serie = false;
        this.progressBar1.Value = this.progressBar1.Maximum;
      }
    }

    private void timer_end_recieve_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.timer_normal_end = true;
      this.timer_end_recieve.Stop();
    }

    private void timer_limit_receive_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.timer_abnormal_end = true;
      this.timer_limit_receive.Stop();
    }

    private void button_help_CNC_SERVER_Click(object sender, EventArgs e)
    {
      Directory.SetCurrentDirectory(this.old_path);
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_CNC_SERVER_RS232"
      }.ShowDialog();
      Directory.SetCurrentDirectory(this.textBox_directorio.Text.ToString());
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_servidor_rs232c));
      this.serialPort1 = new SerialPort(this.components);
      this.openFileDialog1 = new OpenFileDialog();
      this.textBox_directorio = new TextBox();
      this.progressBar1 = new ProgressBar();
      this.label1 = new Label();
      this.button_parar_envio = new Button();
      this.button_continuar_envio = new Button();
      this.button_cancelar_envio = new Button();
      this.richTextBox_enviar = new RichTextBox();
      this.groupBox_control_envio = new GroupBox();
      this.label_orden_recibida = new Label();
      this.richTextBox_ordenes = new RichTextBox();
      this.groupBox_configurar_servidor = new GroupBox();
      this.label7 = new Label();
      this.panel_configuracion = new Panel();
      this.label6 = new Label();
      this.label2 = new Label();
      this.comboBox_tiempo = new ComboBox();
      this.button_seleccionar_fichero = new Button();
      this.button_seleccionar_directorio = new Button();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.textBox_nombre_maquina = new TextBox();
      this.button_cancelar_servidor = new Button();
      this.button_modo_servidor = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.timer_responder_orden = new System.Timers.Timer();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.configuracionToolStripMenuItem = new ToolStripMenuItem();
      this.informacionToolStripMenuItem = new ToolStripMenuItem();
      this.timer_end_recieve = new System.Timers.Timer();
      this.timer_limit_receive = new System.Timers.Timer();
      this.groupBox_status = new GroupBox();
      this.button_help_CNC_SERVER = new Button();
      this.groupBox_control_envio.SuspendLayout();
      this.groupBox_configurar_servidor.SuspendLayout();
      this.panel_configuracion.SuspendLayout();
      this.timer_responder_orden.BeginInit();
      this.menuStrip1.SuspendLayout();
      this.timer_end_recieve.BeginInit();
      this.timer_limit_receive.BeginInit();
      this.groupBox_status.SuspendLayout();
      this.SuspendLayout();
      this.serialPort1.ReadTimeout = 5000;
      this.serialPort1.ReceivedBytesThreshold = 10;
      this.serialPort1.WriteTimeout = 600000;
      this.serialPort1.ErrorReceived += new SerialErrorReceivedEventHandler(this.serialPort1_ErrorReceived);
      this.serialPort1.DataReceived += new SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
      componentResourceManager.ApplyResources((object) this.openFileDialog1, "openFileDialog1");
      componentResourceManager.ApplyResources((object) this.textBox_directorio, "textBox_directorio");
      this.textBox_directorio.Name = "textBox_directorio";
      componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
      this.progressBar1.Name = "progressBar1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.button_parar_envio, "button_parar_envio");
      this.button_parar_envio.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_parar_envio.Name = "button_parar_envio";
      this.button_parar_envio.UseVisualStyleBackColor = false;
      this.button_parar_envio.Click += new EventHandler(this.button_parar_envio_Click);
      componentResourceManager.ApplyResources((object) this.button_continuar_envio, "button_continuar_envio");
      this.button_continuar_envio.AccessibleRole = AccessibleRole.None;
      this.button_continuar_envio.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_continuar_envio.Name = "button_continuar_envio";
      this.button_continuar_envio.UseVisualStyleBackColor = false;
      this.button_continuar_envio.Click += new EventHandler(this.button_continuar_envio_Click);
      componentResourceManager.ApplyResources((object) this.button_cancelar_envio, "button_cancelar_envio");
      this.button_cancelar_envio.AccessibleRole = AccessibleRole.None;
      this.button_cancelar_envio.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar_envio.Name = "button_cancelar_envio";
      this.button_cancelar_envio.UseVisualStyleBackColor = false;
      this.button_cancelar_envio.Click += new EventHandler(this.button_cancelar_envio_Click);
      componentResourceManager.ApplyResources((object) this.richTextBox_enviar, "richTextBox_enviar");
      this.richTextBox_enviar.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox_enviar.Name = "richTextBox_enviar";
      componentResourceManager.ApplyResources((object) this.groupBox_control_envio, "groupBox_control_envio");
      this.groupBox_control_envio.Controls.Add((Control) this.button_parar_envio);
      this.groupBox_control_envio.Controls.Add((Control) this.button_continuar_envio);
      this.groupBox_control_envio.Controls.Add((Control) this.button_cancelar_envio);
      this.groupBox_control_envio.Controls.Add((Control) this.richTextBox_enviar);
      this.groupBox_control_envio.Controls.Add((Control) this.progressBar1);
      this.groupBox_control_envio.Controls.Add((Control) this.label1);
      this.groupBox_control_envio.Name = "groupBox_control_envio";
      this.groupBox_control_envio.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label_orden_recibida, "label_orden_recibida");
      this.label_orden_recibida.Name = "label_orden_recibida";
      componentResourceManager.ApplyResources((object) this.richTextBox_ordenes, "richTextBox_ordenes");
      this.richTextBox_ordenes.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox_ordenes.Name = "richTextBox_ordenes";
      componentResourceManager.ApplyResources((object) this.groupBox_configurar_servidor, "groupBox_configurar_servidor");
      this.groupBox_configurar_servidor.Controls.Add((Control) this.label7);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.panel_configuracion);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.button_seleccionar_fichero);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.button_seleccionar_directorio);
      this.groupBox_configurar_servidor.Controls.Add((Control) this.textBox_directorio);
      this.groupBox_configurar_servidor.Name = "groupBox_configurar_servidor";
      this.groupBox_configurar_servidor.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.panel_configuracion, "panel_configuracion");
      this.panel_configuracion.BorderStyle = BorderStyle.FixedSingle;
      this.panel_configuracion.Controls.Add((Control) this.label6);
      this.panel_configuracion.Controls.Add((Control) this.label2);
      this.panel_configuracion.Controls.Add((Control) this.comboBox_tiempo);
      this.panel_configuracion.Name = "panel_configuracion";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.comboBox_tiempo, "comboBox_tiempo");
      this.comboBox_tiempo.FormattingEnabled = true;
      this.comboBox_tiempo.Items.AddRange(new object[6]
      {
        (object) componentResourceManager.GetString("comboBox_tiempo.Items"),
        (object) componentResourceManager.GetString("comboBox_tiempo.Items1"),
        (object) componentResourceManager.GetString("comboBox_tiempo.Items2"),
        (object) componentResourceManager.GetString("comboBox_tiempo.Items3"),
        (object) componentResourceManager.GetString("comboBox_tiempo.Items4"),
        (object) componentResourceManager.GetString("comboBox_tiempo.Items5")
      });
      this.comboBox_tiempo.Name = "comboBox_tiempo";
      componentResourceManager.ApplyResources((object) this.button_seleccionar_fichero, "button_seleccionar_fichero");
      this.button_seleccionar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_seleccionar_fichero.Name = "button_seleccionar_fichero";
      this.button_seleccionar_fichero.UseVisualStyleBackColor = false;
      this.button_seleccionar_fichero.Click += new EventHandler(this.button_seleccionar_fichero_Click);
      componentResourceManager.ApplyResources((object) this.button_seleccionar_directorio, "button_seleccionar_directorio");
      this.button_seleccionar_directorio.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_seleccionar_directorio.Name = "button_seleccionar_directorio";
      this.button_seleccionar_directorio.UseVisualStyleBackColor = false;
      this.button_seleccionar_directorio.Click += new EventHandler(this.button_seleccionar_directorio_Click);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.BorderStyle = BorderStyle.FixedSingle;
      this.label3.ForeColor = System.Drawing.Color.Red;
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.textBox_nombre_maquina, "textBox_nombre_maquina");
      this.textBox_nombre_maquina.Name = "textBox_nombre_maquina";
      this.textBox_nombre_maquina.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.button_cancelar_servidor, "button_cancelar_servidor");
      this.button_cancelar_servidor.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar_servidor.Name = "button_cancelar_servidor";
      this.button_cancelar_servidor.UseVisualStyleBackColor = false;
      this.button_cancelar_servidor.Click += new EventHandler(this.button_cancelar_servidor_Click);
      componentResourceManager.ApplyResources((object) this.button_modo_servidor, "button_modo_servidor");
      this.button_modo_servidor.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_modo_servidor.Name = "button_modo_servidor";
      this.button_modo_servidor.UseVisualStyleBackColor = false;
      this.button_modo_servidor.Click += new EventHandler(this.button_modo_servidor_Click);
      componentResourceManager.ApplyResources((object) this.folderBrowserDialog1, "folderBrowserDialog1");
      this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
      this.timer_responder_orden.Interval = 10000.0;
      this.timer_responder_orden.SynchronizingObject = (ISynchronizeInvoke) this;
      this.timer_responder_orden.Elapsed += new ElapsedEventHandler(this.timer_responder_orden_Elapsed);
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configuracionToolStripMenuItem,
        (ToolStripItem) this.informacionToolStripMenuItem
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
      this.configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
      this.configuracionToolStripMenuItem.Click += new EventHandler(this.configuracionToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.informacionToolStripMenuItem, "informacionToolStripMenuItem");
      this.informacionToolStripMenuItem.Name = "informacionToolStripMenuItem";
      this.informacionToolStripMenuItem.Click += new EventHandler(this.informacionToolStripMenuItem_Click);
      this.timer_end_recieve.Interval = 5000.0;
      this.timer_end_recieve.SynchronizingObject = (ISynchronizeInvoke) this;
      this.timer_end_recieve.Elapsed += new ElapsedEventHandler(this.timer_end_recieve_Elapsed);
      this.timer_limit_receive.Interval = 60000.0;
      this.timer_limit_receive.SynchronizingObject = (ISynchronizeInvoke) this;
      this.timer_limit_receive.Elapsed += new ElapsedEventHandler(this.timer_limit_receive_Elapsed);
      componentResourceManager.ApplyResources((object) this.groupBox_status, "groupBox_status");
      this.groupBox_status.Controls.Add((Control) this.textBox_nombre_maquina);
      this.groupBox_status.Controls.Add((Control) this.label4);
      this.groupBox_status.Controls.Add((Control) this.label5);
      this.groupBox_status.Controls.Add((Control) this.label3);
      this.groupBox_status.Controls.Add((Control) this.richTextBox_ordenes);
      this.groupBox_status.Controls.Add((Control) this.label_orden_recibida);
      this.groupBox_status.Name = "groupBox_status";
      this.groupBox_status.TabStop = false;
      componentResourceManager.ApplyResources((object) this.button_help_CNC_SERVER, "button_help_CNC_SERVER");
      this.button_help_CNC_SERVER.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.button_help_CNC_SERVER.ForeColor = System.Drawing.Color.Red;
      this.button_help_CNC_SERVER.Name = "button_help_CNC_SERVER";
      this.button_help_CNC_SERVER.UseVisualStyleBackColor = false;
      this.button_help_CNC_SERVER.Click += new EventHandler(this.button_help_CNC_SERVER_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.button_help_CNC_SERVER);
      this.Controls.Add((Control) this.groupBox_status);
      this.Controls.Add((Control) this.groupBox_configurar_servidor);
      this.Controls.Add((Control) this.button_modo_servidor);
      this.Controls.Add((Control) this.button_cancelar_servidor);
      this.Controls.Add((Control) this.groupBox_control_envio);
      this.Controls.Add((Control) this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_servidor_rs232c);
      this.Tag = (object) " ";
      this.FormClosing += new FormClosingEventHandler(this.Form_servidor_rs232c_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.Form_servidor_FormClosed);
      this.Load += new EventHandler(this.Form_servidor_Load);
      this.groupBox_control_envio.ResumeLayout(false);
      this.groupBox_control_envio.PerformLayout();
      this.groupBox_configurar_servidor.ResumeLayout(false);
      this.groupBox_configurar_servidor.PerformLayout();
      this.panel_configuracion.ResumeLayout(false);
      this.panel_configuracion.PerformLayout();
      this.timer_responder_orden.EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.timer_end_recieve.EndInit();
      this.timer_limit_receive.EndInit();
      this.groupBox_status.ResumeLayout(false);
      this.groupBox_status.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
