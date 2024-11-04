// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_config
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Configuracion;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_config : Form
  {
    private RegistryKey key_last_machine;
    private RegistryKey key_machine_config;
    private string tipo_comunicacion = "RS232C";
    private bool cancelar_test = false;
    private IContainer components = (IContainer) null;
    private GroupBox groupBox_ETHERNET;
    private Label label1;
    private ComboBox comboBox_IP;
    private Label label2;
    private ComboBox comboBox_puerto_ETHERNET;
    private Label label3;
    private ComboBox comboBox_tiempo_ETHERNET;
    private Button button_cancelar;
    private Label label8;
    private ComboBox comboBox_maquina;
    private Label label9;
    private GroupBox groupBox_maquina;
    private TextBox textBox_nombre_maquina;
    private Label label4;
    private Button button_probar_ETHERNET;
    private GroupBox groupBox_tipo_com;
    private RadioButton radioButton_ETHERNET;
    private RadioButton radioButton_RS232C;
    private GroupBox groupBox_RS232C;
    private Label label7;
    private ComboBox comboBox_tiempo_fin;
    private Label label6;
    private ComboBox comboBox_protocolo;
    private Label label5;
    private ComboBox comboBox_paridad;
    private Label label10;
    private ComboBox comboBox_bits_stop;
    private Label label11;
    private ComboBox comboBox_bits_datos;
    private Label label12;
    private ComboBox comboBox_baudios;
    private ComboBox comboBox_puerto_RS232C;
    private Label label13;
    private Button button_default_RS232C;
    private Button button_default_ETHERNET;
    private GroupBox groupBox_data_server;
    private Label label_port;
    private Label label_password;
    private Label label_user;
    private Button button_probar_FTP;
    private TextBox textBox_FTP_result;
    private Label label14;
    private CheckBox checkBox_FTP;
    private ComboBox comboBox_FTP_user;
    private ComboBox comboBox_FTP_port;
    private ComboBox comboBox_FTP_password;
    private Button button_salvar;
    private TextBox textBox_host_ip;
    private Label label15;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private Label label17;
    private Label label18;
    private Button button_probar_enviar;
    private SerialPort serialPort1;
    private Button button_probar_recibir;
    private TextBox textBox_prueba_rs232c;
    private Button button_cancelar_test;
    private Label label16;
    private GroupBox groupBox_test_rs232c;
    private Button button_help_serial;
    private Button button_help_ethernet;
    private Button button_help_DATASERVER;
    private TextBox textBox_ping;
    private Label label19;
    private TextBox textBox_resultado_ETHERNET;
    private Button PC_IP_infobutton;

    public Form_config() => this.InitializeComponent();

    private void button_cancelar_Click(object sender, EventArgs e)
    {
      this.cancelar_test = true;
      this.Close();
    }

    private void Form_configurar_Load(object sender, EventArgs e)
    {
      this.comboBox_maquina.Items.Clear();
      for (int index = 1; index <= 20; ++index)
        this.comboBox_maquina.Items.Add((object) index.ToString());
      this.key_last_machine = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (this.key_last_machine == null)
      {
        this.comboBox_maquina.Text = "1";
        this.radioButton_RS232C.Checked = true;
        this.radioButton_ETHERNET.Checked = false;
        this.groupBox_RS232C.Visible = true;
        this.groupBox_ETHERNET.Visible = false;
        this.button_default_RS232C_Click((object) null, (EventArgs) null);
        this.button_default_ETHERNET_Click((object) null, (EventArgs) null);
      }
      else
      {
        string str = (string) this.key_last_machine.GetValue("Machine");
        this.comboBox_maquina.Text = str;
        this.key_machine_config = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false);
        this.tipo_comunicacion = (string) this.key_machine_config.GetValue("Type");
        this.textBox_nombre_maquina.Text = (string) this.key_machine_config.GetValue("Name");
        this.comboBox_puerto_RS232C.Text = (string) this.key_machine_config.GetValue("Port");
        this.comboBox_baudios.Text = (string) this.key_machine_config.GetValue("Baud");
        this.comboBox_bits_datos.Text = (string) this.key_machine_config.GetValue("Data");
        this.comboBox_bits_stop.Text = (string) this.key_machine_config.GetValue("Stop");
        this.comboBox_paridad.Text = (string) this.key_machine_config.GetValue("Parity");
        this.comboBox_protocolo.Text = (string) this.key_machine_config.GetValue("Protocol");
        this.comboBox_tiempo_fin.Text = (string) this.key_machine_config.GetValue("Time");
        this.comboBox_IP.Text = (string) this.key_machine_config.GetValue("IP_ETHERNET");
        this.comboBox_puerto_ETHERNET.Text = (string) this.key_machine_config.GetValue("Port_ETHERNET");
        this.comboBox_tiempo_ETHERNET.Text = (string) this.key_machine_config.GetValue("Time_ETHERNET");
        this.checkBox_FTP.Checked = (string) this.key_machine_config.GetValue("DATASERVER") == "YES";
        this.comboBox_FTP_user.Text = (string) this.key_machine_config.GetValue("User_FTP_Client");
        this.comboBox_FTP_password.Text = (string) this.key_machine_config.GetValue("Password_FTP_Client");
        this.comboBox_FTP_port.Text = (string) this.key_machine_config.GetValue("Port_FTP_Client");
        if (this.tipo_comunicacion == "ETHERNET")
        {
          this.radioButton_RS232C.Checked = false;
          this.radioButton_ETHERNET.Checked = true;
          this.groupBox_RS232C.Visible = false;
          this.groupBox_ETHERNET.Visible = true;
        }
        if (this.tipo_comunicacion == "RS232C")
        {
          this.radioButton_RS232C.Checked = true;
          this.radioButton_ETHERNET.Checked = false;
          this.groupBox_RS232C.Visible = true;
          this.groupBox_ETHERNET.Visible = false;
        }
      }
    }

    private void button_salvar_Click(object sender, EventArgs e)
    {
      this.cancelar_test = true;
      this.key_last_machine = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", true);
      if (this.key_last_machine == null)
      {
        this.key_last_machine = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine");
        if (this.key_last_machine == null)
        {
          int num = (int) MessageBox.Show(Resource_Form_config.String1);
          this.Close();
        }
      }
      this.key_last_machine.SetValue("Machine", (object) this.comboBox_maquina.Text);
      this.key_machine_config = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + this.comboBox_maquina.Text, true);
      if (this.key_machine_config == null)
      {
        this.key_machine_config = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + this.comboBox_maquina.Text);
        if (this.key_machine_config == null)
        {
          int num = (int) MessageBox.Show(Resource_Form_config.String1);
          this.Close();
        }
      }
      if (this.radioButton_ETHERNET.Checked)
        this.tipo_comunicacion = "ETHERNET";
      if (this.radioButton_RS232C.Checked)
        this.tipo_comunicacion = "RS232C";
      this.key_machine_config.SetValue("Name", (object) this.textBox_nombre_maquina.Text);
      this.key_machine_config.SetValue("Type", (object) this.tipo_comunicacion);
      this.key_machine_config.SetValue("Port", (object) this.comboBox_puerto_RS232C.Text.Trim());
      this.key_machine_config.SetValue("Baud", (object) this.comboBox_baudios.Text.Trim());
      this.key_machine_config.SetValue("Data", (object) this.comboBox_bits_datos.Text.Trim());
      this.key_machine_config.SetValue("Stop", (object) this.comboBox_bits_stop.Text.Trim());
      this.key_machine_config.SetValue("Parity", (object) this.comboBox_paridad.Text.Trim());
      this.key_machine_config.SetValue("Protocol", (object) this.comboBox_protocolo.Text.Trim());
      this.key_machine_config.SetValue("Time", (object) this.comboBox_tiempo_fin.Text.Trim());
      this.key_machine_config.SetValue("IP_ETHERNET", (object) this.comboBox_IP.Text.Trim());
      this.key_machine_config.SetValue("Port_ETHERNET", (object) this.comboBox_puerto_ETHERNET.Text.Trim());
      this.key_machine_config.SetValue("Time_ETHERNET", (object) this.comboBox_tiempo_ETHERNET.Text.Trim());
      if (this.checkBox_FTP.Checked)
        this.key_machine_config.SetValue("DATASERVER", (object) "YES");
      else
        this.key_machine_config.SetValue("DATASERVER", (object) "NO");
      this.key_machine_config.SetValue("User_FTP_Client", (object) this.comboBox_FTP_user.Text.Trim());
      this.key_machine_config.SetValue("Password_FTP_Client", (object) this.comboBox_FTP_password.Text.Trim());
      this.key_machine_config.SetValue("Port_FTP_Client", (object) this.comboBox_FTP_port.Text.Trim());
      this.Close();
    }

    private void comboBox_maquina_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.key_machine_config = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + this.comboBox_maquina.Text, true);
      if (this.key_machine_config == null)
      {
        int num = (int) MessageBox.Show(Resource_Form_config.String2);
        this.textBox_nombre_maquina.Text = Resource_Form_config.String13 + " " + this.comboBox_maquina.Text;
        this.button_default_ETHERNET_Click((object) null, (EventArgs) null);
        this.button_default_RS232C_Click((object) null, (EventArgs) null);
      }
      else
      {
        this.tipo_comunicacion = (string) this.key_machine_config.GetValue("Type");
        if (this.tipo_comunicacion == "RS232C")
          this.radioButton_RS232C.Checked = true;
        if (this.tipo_comunicacion == "ETHERNET")
          this.radioButton_ETHERNET.Checked = true;
        this.textBox_nombre_maquina.Text = (string) this.key_machine_config.GetValue("Name");
        this.comboBox_puerto_RS232C.Text = (string) this.key_machine_config.GetValue("Port");
        this.comboBox_baudios.Text = (string) this.key_machine_config.GetValue("Baud");
        this.comboBox_bits_datos.Text = (string) this.key_machine_config.GetValue("Data");
        this.comboBox_bits_stop.Text = (string) this.key_machine_config.GetValue("Stop");
        this.comboBox_paridad.Text = (string) this.key_machine_config.GetValue("Parity");
        this.comboBox_protocolo.Text = (string) this.key_machine_config.GetValue("Protocol");
        this.comboBox_tiempo_fin.Text = (string) this.key_machine_config.GetValue("Time");
        this.comboBox_IP.Text = (string) this.key_machine_config.GetValue("IP_ETHERNET");
        this.comboBox_puerto_ETHERNET.Text = (string) this.key_machine_config.GetValue("Port_ETHERNET");
        this.comboBox_tiempo_ETHERNET.Text = (string) this.key_machine_config.GetValue("Time_ETHERNET");
        this.checkBox_FTP.Checked = (string) this.key_machine_config.GetValue("DATASERVER") == "YES";
        this.comboBox_FTP_user.Text = (string) this.key_machine_config.GetValue("User_FTP_Client");
        this.comboBox_FTP_password.Text = (string) this.key_machine_config.GetValue("Password_FTP_Client");
        this.comboBox_FTP_port.Text = (string) this.key_machine_config.GetValue("Port_FTP_Client");
      }
    }

    private void button_probar_ETHERNET_Click(object sender, EventArgs e)
    {
      try
      {
        this.textBox_resultado_ETHERNET.Clear();
        this.textBox_resultado_ETHERNET.Refresh();
        this.textBox_host_ip.Clear();
        this.textBox_host_ip.Refresh();
        this.textBox_ping.Clear();
        this.textBox_ping.Refresh();
        if (this.comboBox_IP.Text.Trim() == "")
        {
          int num = (int) MessageBox.Show(Resource_Form_config.String3);
          return;
        }
        if (this.comboBox_puerto_ETHERNET.Text.Trim() == "")
        {
          int num = (int) MessageBox.Show(Resource_Form_config.String4);
          return;
        }
        ushort int16 = (ushort) Convert.ToInt16(this.comboBox_puerto_ETHERNET.Text.Trim());
        int int32 = Convert.ToInt32(this.comboBox_tiempo_ETHERNET.Text.Trim());
        ushort FlibHndl;
        short num1 = Focas1.cnc_allclibhndl3((object) this.comboBox_IP.Text.Trim(), int16, int32, out FlibHndl);
        if (num1 == (short) 0)
        {
          Focas1.ODBSYS a = new Focas1.ODBSYS();
          int num2 = (int) Focas1.cnc_sysinfo(FlibHndl, a);
          string str1 = new string(a.cnc_type);
          string str2 = new string(a.mt_type);
          byte[] bytes = BitConverter.GetBytes(a.addinfo);
          string str3 = "";
          if (((int) bytes[0] & 2) == 2)
            str3 = "i";
          string str4 = "";
          if (bytes[1] == (byte) 0)
            str4 = "";
          if (bytes[1] == (byte) 1)
            str4 = "A";
          if (bytes[1] == (byte) 2)
            str4 = "B";
          if (bytes[1] == (byte) 3)
            str4 = "C";
          if (bytes[1] == (byte) 4)
            str4 = "D";
          if (bytes[1] == (byte) 5)
            str4 = "E";
          if (bytes[1] == (byte) 6)
            str4 = "F";
          string str5 = str1 + " " + str3 + " - " + str2 + str4;
          this.textBox_ping.ForeColor = System.Drawing.Color.Green;
          this.textBox_ping.Text = "OK (Success)";
          this.textBox_resultado_ETHERNET.ForeColor = System.Drawing.Color.Green;
          this.textBox_resultado_ETHERNET.Text = str5;
          int num3 = (int) Focas1.cnc_freelibhndl(FlibHndl);
        }
        else
        {
          PingReply pingReply = new Ping().Send(this.comboBox_IP.Text.Trim(), 3000);
          if (pingReply != null)
          {
            string newLine = Environment.NewLine;
            if (pingReply.Status == IPStatus.Success)
            {
              this.textBox_ping.ForeColor = System.Drawing.Color.Green;
              this.textBox_ping.Text = "OK (" + pingReply.Status.ToString() + ")";
              this.textBox_resultado_ETHERNET.ForeColor = System.Drawing.Color.Red;
              this.textBox_resultado_ETHERNET.Text = "ERROR(" + num1.ToString() + ")";
              TextBox resultadoEthernet1 = this.textBox_resultado_ETHERNET;
              resultadoEthernet1.Text = resultadoEthernet1.Text + newLine + "Check :";
              TextBox resultadoEthernet2 = this.textBox_resultado_ETHERNET;
              resultadoEthernet2.Text = resultadoEthernet2.Text + newLine + "TCP port (CNC = PC) ?";
            }
            else
            {
              this.textBox_ping.ForeColor = System.Drawing.Color.Red;
              this.textBox_ping.Text = pingReply.Status.ToString() + ")";
              this.textBox_resultado_ETHERNET.ForeColor = System.Drawing.Color.Red;
              this.textBox_resultado_ETHERNET.Text = "No connection (" + num1.ToString() + ")";
              TextBox resultadoEthernet3 = this.textBox_resultado_ETHERNET;
              resultadoEthernet3.Text = resultadoEthernet3.Text + newLine + "Cable connection ?";
              TextBox resultadoEthernet4 = this.textBox_resultado_ETHERNET;
              resultadoEthernet4.Text = resultadoEthernet4.Text + newLine + "IP address (CNC , PC) ?";
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_config.String5 + ex.Message);
      }
      try
      {
        IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
        this.textBox_host_ip.Text = "";
        foreach (IPAddress ipAddress in hostAddresses)
        {
          if (Regex.Match(ipAddress.ToString(), "\\b(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})\\b").Success)
          {
            TextBox textBoxHostIp = this.textBox_host_ip;
            textBoxHostIp.Text = textBoxHostIp.Text + ipAddress.ToString() + "\n";
          }
        }
      }
      catch (Exception ex)
      {
        this.textBox_host_ip.Text = "";
        int num = (int) MessageBox.Show(Resource_Form_config.String6 + ex.Message);
      }
    }

    private void radioButton_RS232C_CheckedChanged(object sender, EventArgs e)
    {
      this.groupBox_ETHERNET.Visible = false;
      this.groupBox_RS232C.Visible = true;
      this.groupBox_test_rs232c.Visible = true;
    }

    private void radioButton_ETHERNET_CheckedChanged(object sender, EventArgs e)
    {
      this.groupBox_ETHERNET.Visible = true;
      this.groupBox_RS232C.Visible = false;
      this.groupBox_test_rs232c.Visible = false;
    }

    private void button_default_RS232C_Click(object sender, EventArgs e)
    {
      try
      {
        this.comboBox_puerto_RS232C.Items.Clear();
        foreach (string portName in SerialPort.GetPortNames())
        {
          this.comboBox_puerto_RS232C.Items.Add((object) portName);
          this.comboBox_puerto_RS232C.Text = portName;
        }
        this.comboBox_puerto_RS232C.Refresh();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_config.String7 + ex.Message);
        this.comboBox_puerto_RS232C.Text = "";
      }
      this.comboBox_baudios.SelectedIndex = 4;
      this.comboBox_bits_datos.SelectedIndex = 0;
      this.comboBox_bits_stop.SelectedIndex = 1;
      this.comboBox_paridad.SelectedIndex = 0;
      this.comboBox_protocolo.SelectedIndex = 0;
      this.comboBox_tiempo_fin.SelectedIndex = 1;
    }

    private void button_default_ETHERNET_Click(object sender, EventArgs e)
    {
      this.comboBox_IP.Text = "192.168.1.1";
      this.comboBox_puerto_ETHERNET.Text = "8193";
      this.comboBox_tiempo_ETHERNET.Text = "2";
      this.comboBox_FTP_port.Text = "21";
      this.checkBox_FTP.Checked = false;
    }

    private void checkBox_FTP_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBox_FTP.Checked)
        this.groupBox_data_server.Visible = true;
      else
        this.groupBox_data_server.Visible = false;
    }

    private void button_probar_FTP_Click(object sender, EventArgs e)
    {
      this.textBox_FTP_result.Clear();
      this.textBox_FTP_result.Refresh();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(new Uri("ftp://" + this.comboBox_IP.Text + ":" + this.comboBox_FTP_port.Text + "/"));
        ftpWebRequest.Proxy = (IWebProxy) new WebProxy();
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.comboBox_FTP_user.Text, this.comboBox_FTP_password.Text);
        ftpWebRequest.Method = "NLST";
        WebResponse response = ftpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        while (true)
        {
          string str = streamReader.ReadLine();
          if (str != null)
          {
            TextBox textBoxFtpResult = this.textBox_FTP_result;
            textBoxFtpResult.Text = textBoxFtpResult.Text + str + "\r\n";
          }
          else
            break;
        }
        this.textBox_FTP_result.Text += "(Response: OK)\n";
        this.textBox_FTP_result.SelectionStart = this.textBox_FTP_result.Text.Length;
        this.textBox_FTP_result.ScrollToCaret();
        this.textBox_FTP_result.Refresh();
        streamReader.Close();
        response.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_config.String8 + ex.Message);
      }
    }

    private void comboBox_puerto_RS232C_Click(object sender, EventArgs e)
    {
      try
      {
        this.comboBox_puerto_RS232C.Items.Clear();
        foreach (string portName in SerialPort.GetPortNames())
        {
          this.comboBox_puerto_RS232C.Items.Add((object) portName);
          this.comboBox_puerto_RS232C.Text = portName;
        }
        this.comboBox_puerto_RS232C.Refresh();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_config.String9 + ex.Message);
        this.comboBox_puerto_RS232C.Text = "";
      }
    }

    private void button_probar_enviar_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.serialPort1.IsOpen)
          this.serialPort1.Close();
        this.serialPort1.PortName = this.comboBox_puerto_RS232C.Text;
        this.serialPort1.BaudRate = Convert.ToInt32(this.comboBox_baudios.Text);
        this.serialPort1.DataBits = (int) Convert.ToInt16(this.comboBox_bits_datos.Text);
        this.serialPort1.StopBits = !(this.comboBox_bits_stop.Text == "1") ? StopBits.Two : StopBits.One;
        this.serialPort1.Parity = Parity.None;
        string str1 = this.comboBox_paridad.Text.Substring(0, 1);
        if (str1 == nameof (e) || str1 == "E")
          this.serialPort1.Parity = Parity.Even;
        if (str1 == "o" || str1 == "O")
          this.serialPort1.Parity = Parity.Odd;
        this.serialPort1.Handshake = Handshake.None;
        if (this.comboBox_protocolo.Text == "Software")
          this.serialPort1.Handshake = Handshake.XOnXOff;
        if (this.comboBox_protocolo.Text == "Hardware")
          this.serialPort1.Handshake = Handshake.RequestToSend;
        if (this.comboBox_protocolo.Text == "Hardware-Software")
          this.serialPort1.Handshake = Handshake.RequestToSendXOnXOff;
        this.serialPort1.Open();
        this.serialPort1.DiscardOutBuffer();
        this.serialPort1.DiscardInBuffer();
        this.serialPort1.DtrEnable = true;
        string str2 = "%\r\nO5555\r\nG04X1.0\r\nM30\r\n%\r\n";
        char[] chArray = new char[50];
        char[] buffer = this.textBox_prueba_rs232c.Text.Length >= 10 ? this.textBox_prueba_rs232c.Text.ToCharArray() : str2.ToCharArray();
        this.serialPort1.Write(buffer, 0, buffer.Length);
        if (this.serialPort1.BytesToWrite != 0)
          return;
        this.textBox_prueba_rs232c.Text = new string(buffer);
        int num = (int) MessageBox.Show(Resource_Form_config.String14);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("ERROR :\n" + ex.Message);
      }
      finally
      {
        if (this.serialPort1.IsOpen)
          this.serialPort1.Close();
      }
    }

    private void button_probar_recibir_Click(object sender, EventArgs e)
    {
      try
      {
        this.textBox_prueba_rs232c.Text = Resource_Form_config.String15;
        this.button_cancelar_test.Enabled = true;
        this.button_probar_enviar.Enabled = false;
        this.button_probar_recibir.Enabled = false;
        this.Refresh();
        if (this.serialPort1.IsOpen)
          this.serialPort1.Close();
        this.serialPort1.PortName = this.comboBox_puerto_RS232C.Text;
        this.serialPort1.BaudRate = Convert.ToInt32(this.comboBox_baudios.Text);
        this.serialPort1.DataBits = (int) Convert.ToInt16(this.comboBox_bits_datos.Text);
        this.serialPort1.StopBits = !(this.comboBox_bits_stop.Text == "1") ? StopBits.Two : StopBits.One;
        this.serialPort1.Parity = Parity.None;
        string str = this.comboBox_paridad.Text.Substring(0, 1);
        if (str == nameof (e) || str == "E")
          this.serialPort1.Parity = Parity.Even;
        if (str == "o" || str == "O")
          this.serialPort1.Parity = Parity.Odd;
        this.serialPort1.Handshake = Handshake.None;
        if (this.comboBox_protocolo.Text == "Software")
          this.serialPort1.Handshake = Handshake.XOnXOff;
        if (this.comboBox_protocolo.Text == "Hardware")
          this.serialPort1.Handshake = Handshake.RequestToSend;
        if (this.comboBox_protocolo.Text == "Hardware-Software")
          this.serialPort1.Handshake = Handshake.RequestToSendXOnXOff;
        this.serialPort1.Open();
        this.serialPort1.DiscardOutBuffer();
        this.serialPort1.DiscardInBuffer();
        this.serialPort1.DtrEnable = true;
        this.cancelar_test = false;
        do
        {
          Application.DoEvents();
          if (this.serialPort1.BytesToRead > 10)
          {
            byte[] numArray = new byte[50];
            this.serialPort1.Read(numArray, 0, numArray.Length);
            this.textBox_prueba_rs232c.Text = Encoding.ASCII.GetString(numArray);
            int num = (int) MessageBox.Show(Resource_Form_config.String16);
            return;
          }
          Application.DoEvents();
        }
        while (!this.cancelar_test);
        this.cancelar_test = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("ERROR :\n" + ex.Message);
      }
      finally
      {
        if (this.serialPort1.IsOpen)
          this.serialPort1.Close();
        this.button_cancelar_test.Enabled = false;
        this.button_probar_enviar.Enabled = true;
        this.button_probar_recibir.Enabled = true;
        this.Refresh();
      }
    }

    private void button_cancelar_test_Click(object sender, EventArgs e)
    {
      this.cancelar_test = true;
      this.textBox_prueba_rs232c.Text = "";
    }

    private void button_help_serial_Click(object sender, EventArgs e)
    {
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_connect_setting_RS232"
      }.ShowDialog();
    }

    private void button_help_ethernet_Click(object sender, EventArgs e)
    {
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_connect_setting_Ethernet"
      }.ShowDialog();
    }

    private void button_help_DATASERVER_Click(object sender, EventArgs e)
    {
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_setting_DATASERVER"
      }.ShowDialog();
    }

    private void PC_IP_infobutton_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_IP_info().ShowDialog();
      this.Enabled = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_config));
      this.groupBox_ETHERNET = new GroupBox();
      this.PC_IP_infobutton = new Button();
      this.button_help_ethernet = new Button();
      this.groupBox1 = new GroupBox();
      this.textBox_ping = new TextBox();
      this.label19 = new Label();
      this.label15 = new Label();
      this.label4 = new Label();
      this.textBox_resultado_ETHERNET = new TextBox();
      this.checkBox_FTP = new CheckBox();
      this.groupBox_data_server = new GroupBox();
      this.button_help_DATASERVER = new Button();
      this.groupBox2 = new GroupBox();
      this.textBox_FTP_result = new TextBox();
      this.label14 = new Label();
      this.comboBox_FTP_port = new ComboBox();
      this.comboBox_FTP_password = new ComboBox();
      this.button_probar_FTP = new Button();
      this.comboBox_FTP_user = new ComboBox();
      this.label_port = new Label();
      this.label_password = new Label();
      this.label_user = new Label();
      this.button_default_ETHERNET = new Button();
      this.label3 = new Label();
      this.button_probar_ETHERNET = new Button();
      this.comboBox_tiempo_ETHERNET = new ComboBox();
      this.label2 = new Label();
      this.comboBox_puerto_ETHERNET = new ComboBox();
      this.comboBox_IP = new ComboBox();
      this.label1 = new Label();
      this.textBox_host_ip = new TextBox();
      this.button_cancelar = new Button();
      this.label8 = new Label();
      this.comboBox_maquina = new ComboBox();
      this.label9 = new Label();
      this.groupBox_maquina = new GroupBox();
      this.textBox_nombre_maquina = new TextBox();
      this.groupBox_tipo_com = new GroupBox();
      this.radioButton_ETHERNET = new RadioButton();
      this.radioButton_RS232C = new RadioButton();
      this.groupBox_RS232C = new GroupBox();
      this.button_help_serial = new Button();
      this.label18 = new Label();
      this.label17 = new Label();
      this.button_default_RS232C = new Button();
      this.label7 = new Label();
      this.comboBox_tiempo_fin = new ComboBox();
      this.label6 = new Label();
      this.comboBox_protocolo = new ComboBox();
      this.label5 = new Label();
      this.comboBox_paridad = new ComboBox();
      this.label10 = new Label();
      this.comboBox_bits_stop = new ComboBox();
      this.label11 = new Label();
      this.comboBox_bits_datos = new ComboBox();
      this.label12 = new Label();
      this.comboBox_baudios = new ComboBox();
      this.comboBox_puerto_RS232C = new ComboBox();
      this.label13 = new Label();
      this.label16 = new Label();
      this.button_cancelar_test = new Button();
      this.button_probar_recibir = new Button();
      this.textBox_prueba_rs232c = new TextBox();
      this.button_probar_enviar = new Button();
      this.button_salvar = new Button();
      this.serialPort1 = new SerialPort(this.components);
      this.groupBox_test_rs232c = new GroupBox();
      this.groupBox_ETHERNET.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox_data_server.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox_maquina.SuspendLayout();
      this.groupBox_tipo_com.SuspendLayout();
      this.groupBox_RS232C.SuspendLayout();
      this.groupBox_test_rs232c.SuspendLayout();
      this.SuspendLayout();
      this.groupBox_ETHERNET.Controls.Add((Control) this.PC_IP_infobutton);
      this.groupBox_ETHERNET.Controls.Add((Control) this.button_help_ethernet);
      this.groupBox_ETHERNET.Controls.Add((Control) this.groupBox1);
      this.groupBox_ETHERNET.Controls.Add((Control) this.checkBox_FTP);
      this.groupBox_ETHERNET.Controls.Add((Control) this.groupBox_data_server);
      this.groupBox_ETHERNET.Controls.Add((Control) this.button_default_ETHERNET);
      this.groupBox_ETHERNET.Controls.Add((Control) this.label3);
      this.groupBox_ETHERNET.Controls.Add((Control) this.button_probar_ETHERNET);
      this.groupBox_ETHERNET.Controls.Add((Control) this.comboBox_tiempo_ETHERNET);
      this.groupBox_ETHERNET.Controls.Add((Control) this.label2);
      this.groupBox_ETHERNET.Controls.Add((Control) this.comboBox_puerto_ETHERNET);
      this.groupBox_ETHERNET.Controls.Add((Control) this.comboBox_IP);
      this.groupBox_ETHERNET.Controls.Add((Control) this.label1);
      componentResourceManager.ApplyResources((object) this.groupBox_ETHERNET, "groupBox_ETHERNET");
      this.groupBox_ETHERNET.Name = "groupBox_ETHERNET";
      this.groupBox_ETHERNET.TabStop = false;
      this.PC_IP_infobutton.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.PC_IP_infobutton, "PC_IP_infobutton");
      this.PC_IP_infobutton.Name = "PC_IP_infobutton";
      this.PC_IP_infobutton.UseVisualStyleBackColor = false;
      this.PC_IP_infobutton.Click += new EventHandler(this.PC_IP_infobutton_Click);
      this.button_help_ethernet.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_help_ethernet, "button_help_ethernet");
      this.button_help_ethernet.ForeColor = System.Drawing.Color.Red;
      this.button_help_ethernet.Name = "button_help_ethernet";
      this.button_help_ethernet.UseVisualStyleBackColor = false;
      this.button_help_ethernet.Click += new EventHandler(this.button_help_ethernet_Click);
      this.groupBox1.Controls.Add((Control) this.textBox_ping);
      this.groupBox1.Controls.Add((Control) this.label19);
      this.groupBox1.Controls.Add((Control) this.label15);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.textBox_resultado_ETHERNET);
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      this.textBox_ping.BackColor = SystemColors.Window;
      componentResourceManager.ApplyResources((object) this.textBox_ping, "textBox_ping");
      this.textBox_ping.Name = "textBox_ping";
      this.textBox_ping.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label19, "label19");
      this.label19.Name = "label19";
      componentResourceManager.ApplyResources((object) this.label15, "label15");
      this.label15.Name = "label15";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      this.textBox_resultado_ETHERNET.BackColor = SystemColors.Window;
      componentResourceManager.ApplyResources((object) this.textBox_resultado_ETHERNET, "textBox_resultado_ETHERNET");
      this.textBox_resultado_ETHERNET.Name = "textBox_resultado_ETHERNET";
      this.textBox_resultado_ETHERNET.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.checkBox_FTP, "checkBox_FTP");
      this.checkBox_FTP.Name = "checkBox_FTP";
      this.checkBox_FTP.UseVisualStyleBackColor = true;
      this.checkBox_FTP.CheckedChanged += new EventHandler(this.checkBox_FTP_CheckedChanged);
      this.groupBox_data_server.Controls.Add((Control) this.button_help_DATASERVER);
      this.groupBox_data_server.Controls.Add((Control) this.groupBox2);
      this.groupBox_data_server.Controls.Add((Control) this.comboBox_FTP_port);
      this.groupBox_data_server.Controls.Add((Control) this.comboBox_FTP_password);
      this.groupBox_data_server.Controls.Add((Control) this.button_probar_FTP);
      this.groupBox_data_server.Controls.Add((Control) this.comboBox_FTP_user);
      this.groupBox_data_server.Controls.Add((Control) this.label_port);
      this.groupBox_data_server.Controls.Add((Control) this.label_password);
      this.groupBox_data_server.Controls.Add((Control) this.label_user);
      componentResourceManager.ApplyResources((object) this.groupBox_data_server, "groupBox_data_server");
      this.groupBox_data_server.Name = "groupBox_data_server";
      this.groupBox_data_server.TabStop = false;
      this.button_help_DATASERVER.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_help_DATASERVER, "button_help_DATASERVER");
      this.button_help_DATASERVER.ForeColor = System.Drawing.Color.Red;
      this.button_help_DATASERVER.Name = "button_help_DATASERVER";
      this.button_help_DATASERVER.UseVisualStyleBackColor = false;
      this.button_help_DATASERVER.Click += new EventHandler(this.button_help_DATASERVER_Click);
      this.groupBox2.Controls.Add((Control) this.textBox_FTP_result);
      this.groupBox2.Controls.Add((Control) this.label14);
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      this.textBox_FTP_result.BackColor = SystemColors.Window;
      componentResourceManager.ApplyResources((object) this.textBox_FTP_result, "textBox_FTP_result");
      this.textBox_FTP_result.Name = "textBox_FTP_result";
      componentResourceManager.ApplyResources((object) this.label14, "label14");
      this.label14.Name = "label14";
      this.comboBox_FTP_port.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_FTP_port.FormattingEnabled = true;
      this.comboBox_FTP_port.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("comboBox_FTP_port.Items"),
        (object) componentResourceManager.GetString("comboBox_FTP_port.Items1")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_FTP_port, "comboBox_FTP_port");
      this.comboBox_FTP_port.Name = "comboBox_FTP_port";
      this.comboBox_FTP_password.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_FTP_password.FormattingEnabled = true;
      this.comboBox_FTP_password.Items.AddRange(new object[1]
      {
        (object) componentResourceManager.GetString("comboBox_FTP_password.Items")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_FTP_password, "comboBox_FTP_password");
      this.comboBox_FTP_password.Name = "comboBox_FTP_password";
      this.button_probar_FTP.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_probar_FTP, "button_probar_FTP");
      this.button_probar_FTP.Name = "button_probar_FTP";
      this.button_probar_FTP.UseVisualStyleBackColor = false;
      this.button_probar_FTP.Click += new EventHandler(this.button_probar_FTP_Click);
      this.comboBox_FTP_user.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_FTP_user.FormattingEnabled = true;
      this.comboBox_FTP_user.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("comboBox_FTP_user.Items"),
        (object) componentResourceManager.GetString("comboBox_FTP_user.Items1")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_FTP_user, "comboBox_FTP_user");
      this.comboBox_FTP_user.Name = "comboBox_FTP_user";
      componentResourceManager.ApplyResources((object) this.label_port, "label_port");
      this.label_port.Name = "label_port";
      componentResourceManager.ApplyResources((object) this.label_password, "label_password");
      this.label_password.Name = "label_password";
      componentResourceManager.ApplyResources((object) this.label_user, "label_user");
      this.label_user.Name = "label_user";
      this.button_default_ETHERNET.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_default_ETHERNET, "button_default_ETHERNET");
      this.button_default_ETHERNET.Name = "button_default_ETHERNET";
      this.button_default_ETHERNET.UseVisualStyleBackColor = false;
      this.button_default_ETHERNET.Click += new EventHandler(this.button_default_ETHERNET_Click);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      this.button_probar_ETHERNET.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_probar_ETHERNET, "button_probar_ETHERNET");
      this.button_probar_ETHERNET.Name = "button_probar_ETHERNET";
      this.button_probar_ETHERNET.UseVisualStyleBackColor = false;
      this.button_probar_ETHERNET.Click += new EventHandler(this.button_probar_ETHERNET_Click);
      this.comboBox_tiempo_ETHERNET.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_tiempo_ETHERNET.FormattingEnabled = true;
      this.comboBox_tiempo_ETHERNET.Items.AddRange(new object[3]
      {
        (object) componentResourceManager.GetString("comboBox_tiempo_ETHERNET.Items"),
        (object) componentResourceManager.GetString("comboBox_tiempo_ETHERNET.Items1"),
        (object) componentResourceManager.GetString("comboBox_tiempo_ETHERNET.Items2")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_tiempo_ETHERNET, "comboBox_tiempo_ETHERNET");
      this.comboBox_tiempo_ETHERNET.Name = "comboBox_tiempo_ETHERNET";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.comboBox_puerto_ETHERNET.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_puerto_ETHERNET.FormattingEnabled = true;
      this.comboBox_puerto_ETHERNET.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("comboBox_puerto_ETHERNET.Items"),
        (object) componentResourceManager.GetString("comboBox_puerto_ETHERNET.Items1")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_puerto_ETHERNET, "comboBox_puerto_ETHERNET");
      this.comboBox_puerto_ETHERNET.Name = "comboBox_puerto_ETHERNET";
      this.comboBox_IP.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_IP.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBox_IP, "comboBox_IP");
      this.comboBox_IP.Name = "comboBox_IP";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.textBox_host_ip.BackColor = SystemColors.ControlLightLight;
      componentResourceManager.ApplyResources((object) this.textBox_host_ip, "textBox_host_ip");
      this.textBox_host_ip.Name = "textBox_host_ip";
      this.textBox_host_ip.ReadOnly = true;
      this.button_cancelar.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button_cancelar_Click);
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.Name = "label8";
      this.comboBox_maquina.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_maquina.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.comboBox_maquina, "comboBox_maquina");
      this.comboBox_maquina.Name = "comboBox_maquina";
      this.comboBox_maquina.SelectedIndexChanged += new EventHandler(this.comboBox_maquina_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label9, "label9");
      this.label9.Name = "label9";
      this.groupBox_maquina.Controls.Add((Control) this.textBox_nombre_maquina);
      this.groupBox_maquina.Controls.Add((Control) this.label9);
      this.groupBox_maquina.Controls.Add((Control) this.comboBox_maquina);
      this.groupBox_maquina.Controls.Add((Control) this.label8);
      componentResourceManager.ApplyResources((object) this.groupBox_maquina, "groupBox_maquina");
      this.groupBox_maquina.Name = "groupBox_maquina";
      this.groupBox_maquina.TabStop = false;
      this.textBox_nombre_maquina.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.textBox_nombre_maquina, "textBox_nombre_maquina");
      this.textBox_nombre_maquina.Name = "textBox_nombre_maquina";
      this.groupBox_tipo_com.Controls.Add((Control) this.radioButton_ETHERNET);
      this.groupBox_tipo_com.Controls.Add((Control) this.radioButton_RS232C);
      componentResourceManager.ApplyResources((object) this.groupBox_tipo_com, "groupBox_tipo_com");
      this.groupBox_tipo_com.Name = "groupBox_tipo_com";
      this.groupBox_tipo_com.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButton_ETHERNET, "radioButton_ETHERNET");
      this.radioButton_ETHERNET.Name = "radioButton_ETHERNET";
      this.radioButton_ETHERNET.TabStop = true;
      this.radioButton_ETHERNET.UseVisualStyleBackColor = true;
      this.radioButton_ETHERNET.CheckedChanged += new EventHandler(this.radioButton_ETHERNET_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.radioButton_RS232C, "radioButton_RS232C");
      this.radioButton_RS232C.Name = "radioButton_RS232C";
      this.radioButton_RS232C.UseVisualStyleBackColor = true;
      this.radioButton_RS232C.CheckedChanged += new EventHandler(this.radioButton_RS232C_CheckedChanged);
      this.groupBox_RS232C.BackColor = SystemColors.Control;
      this.groupBox_RS232C.Controls.Add((Control) this.button_help_serial);
      this.groupBox_RS232C.Controls.Add((Control) this.label18);
      this.groupBox_RS232C.Controls.Add((Control) this.label17);
      this.groupBox_RS232C.Controls.Add((Control) this.button_default_RS232C);
      this.groupBox_RS232C.Controls.Add((Control) this.label7);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_tiempo_fin);
      this.groupBox_RS232C.Controls.Add((Control) this.label6);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_protocolo);
      this.groupBox_RS232C.Controls.Add((Control) this.label5);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_paridad);
      this.groupBox_RS232C.Controls.Add((Control) this.label10);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_bits_stop);
      this.groupBox_RS232C.Controls.Add((Control) this.label11);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_bits_datos);
      this.groupBox_RS232C.Controls.Add((Control) this.label12);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_baudios);
      this.groupBox_RS232C.Controls.Add((Control) this.comboBox_puerto_RS232C);
      this.groupBox_RS232C.Controls.Add((Control) this.label13);
      componentResourceManager.ApplyResources((object) this.groupBox_RS232C, "groupBox_RS232C");
      this.groupBox_RS232C.Name = "groupBox_RS232C";
      this.groupBox_RS232C.TabStop = false;
      this.button_help_serial.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_help_serial, "button_help_serial");
      this.button_help_serial.ForeColor = System.Drawing.Color.Red;
      this.button_help_serial.Name = "button_help_serial";
      this.button_help_serial.UseVisualStyleBackColor = false;
      this.button_help_serial.Click += new EventHandler(this.button_help_serial_Click);
      componentResourceManager.ApplyResources((object) this.label18, "label18");
      this.label18.ForeColor = System.Drawing.Color.Blue;
      this.label18.Name = "label18";
      componentResourceManager.ApplyResources((object) this.label17, "label17");
      this.label17.ForeColor = System.Drawing.Color.Blue;
      this.label17.Name = "label17";
      this.button_default_RS232C.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_default_RS232C, "button_default_RS232C");
      this.button_default_RS232C.Name = "button_default_RS232C";
      this.button_default_RS232C.UseVisualStyleBackColor = false;
      this.button_default_RS232C.Click += new EventHandler(this.button_default_RS232C_Click);
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      this.comboBox_tiempo_fin.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_tiempo_fin.FormattingEnabled = true;
      this.comboBox_tiempo_fin.Items.AddRange(new object[5]
      {
        (object) componentResourceManager.GetString("comboBox_tiempo_fin.Items"),
        (object) componentResourceManager.GetString("comboBox_tiempo_fin.Items1"),
        (object) componentResourceManager.GetString("comboBox_tiempo_fin.Items2"),
        (object) componentResourceManager.GetString("comboBox_tiempo_fin.Items3"),
        (object) componentResourceManager.GetString("comboBox_tiempo_fin.Items4")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_tiempo_fin, "comboBox_tiempo_fin");
      this.comboBox_tiempo_fin.Name = "comboBox_tiempo_fin";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      this.comboBox_protocolo.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_protocolo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_protocolo.FormattingEnabled = true;
      this.comboBox_protocolo.Items.AddRange(new object[4]
      {
        (object) componentResourceManager.GetString("comboBox_protocolo.Items"),
        (object) componentResourceManager.GetString("comboBox_protocolo.Items1"),
        (object) componentResourceManager.GetString("comboBox_protocolo.Items2"),
        (object) componentResourceManager.GetString("comboBox_protocolo.Items3")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_protocolo, "comboBox_protocolo");
      this.comboBox_protocolo.Name = "comboBox_protocolo";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      this.comboBox_paridad.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_paridad.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_paridad.FormattingEnabled = true;
      this.comboBox_paridad.Items.AddRange(new object[3]
      {
        (object) componentResourceManager.GetString("comboBox_paridad.Items"),
        (object) componentResourceManager.GetString("comboBox_paridad.Items1"),
        (object) componentResourceManager.GetString("comboBox_paridad.Items2")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_paridad, "comboBox_paridad");
      this.comboBox_paridad.Name = "comboBox_paridad";
      componentResourceManager.ApplyResources((object) this.label10, "label10");
      this.label10.Name = "label10";
      this.comboBox_bits_stop.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_bits_stop.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_bits_stop.FormattingEnabled = true;
      this.comboBox_bits_stop.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("comboBox_bits_stop.Items"),
        (object) componentResourceManager.GetString("comboBox_bits_stop.Items1")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_bits_stop, "comboBox_bits_stop");
      this.comboBox_bits_stop.Name = "comboBox_bits_stop";
      componentResourceManager.ApplyResources((object) this.label11, "label11");
      this.label11.Name = "label11";
      this.comboBox_bits_datos.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_bits_datos.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_bits_datos.FormattingEnabled = true;
      this.comboBox_bits_datos.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("comboBox_bits_datos.Items"),
        (object) componentResourceManager.GetString("comboBox_bits_datos.Items1")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_bits_datos, "comboBox_bits_datos");
      this.comboBox_bits_datos.Name = "comboBox_bits_datos";
      componentResourceManager.ApplyResources((object) this.label12, "label12");
      this.label12.Name = "label12";
      this.comboBox_baudios.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_baudios.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_baudios.FormattingEnabled = true;
      this.comboBox_baudios.Items.AddRange(new object[8]
      {
        (object) componentResourceManager.GetString("comboBox_baudios.Items"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items1"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items2"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items3"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items4"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items5"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items6"),
        (object) componentResourceManager.GetString("comboBox_baudios.Items7")
      });
      componentResourceManager.ApplyResources((object) this.comboBox_baudios, "comboBox_baudios");
      this.comboBox_baudios.Name = "comboBox_baudios";
      this.comboBox_puerto_RS232C.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.comboBox_puerto_RS232C.FormattingEnabled = true;
      componentResourceManager.ApplyResources((object) this.comboBox_puerto_RS232C, "comboBox_puerto_RS232C");
      this.comboBox_puerto_RS232C.Name = "comboBox_puerto_RS232C";
      this.comboBox_puerto_RS232C.Click += new EventHandler(this.comboBox_puerto_RS232C_Click);
      componentResourceManager.ApplyResources((object) this.label13, "label13");
      this.label13.Name = "label13";
      componentResourceManager.ApplyResources((object) this.label16, "label16");
      this.label16.Name = "label16";
      this.button_cancelar_test.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      componentResourceManager.ApplyResources((object) this.button_cancelar_test, "button_cancelar_test");
      this.button_cancelar_test.Name = "button_cancelar_test";
      this.button_cancelar_test.UseVisualStyleBackColor = false;
      this.button_cancelar_test.Click += new EventHandler(this.button_cancelar_test_Click);
      this.button_probar_recibir.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_probar_recibir, "button_probar_recibir");
      this.button_probar_recibir.Name = "button_probar_recibir";
      this.button_probar_recibir.UseVisualStyleBackColor = false;
      this.button_probar_recibir.Click += new EventHandler(this.button_probar_recibir_Click);
      this.textBox_prueba_rs232c.BackColor = SystemColors.ControlLightLight;
      this.textBox_prueba_rs232c.ForeColor = System.Drawing.Color.DarkGreen;
      componentResourceManager.ApplyResources((object) this.textBox_prueba_rs232c, "textBox_prueba_rs232c");
      this.textBox_prueba_rs232c.Name = "textBox_prueba_rs232c";
      this.button_probar_enviar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_probar_enviar, "button_probar_enviar");
      this.button_probar_enviar.Name = "button_probar_enviar";
      this.button_probar_enviar.UseVisualStyleBackColor = false;
      this.button_probar_enviar.Click += new EventHandler(this.button_probar_enviar_Click);
      this.button_salvar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_salvar, "button_salvar");
      this.button_salvar.Name = "button_salvar";
      this.button_salvar.UseVisualStyleBackColor = false;
      this.button_salvar.Click += new EventHandler(this.button_salvar_Click);
      this.groupBox_test_rs232c.Controls.Add((Control) this.button_probar_enviar);
      this.groupBox_test_rs232c.Controls.Add((Control) this.label16);
      this.groupBox_test_rs232c.Controls.Add((Control) this.textBox_prueba_rs232c);
      this.groupBox_test_rs232c.Controls.Add((Control) this.button_probar_recibir);
      this.groupBox_test_rs232c.Controls.Add((Control) this.button_cancelar_test);
      componentResourceManager.ApplyResources((object) this.groupBox_test_rs232c, "groupBox_test_rs232c");
      this.groupBox_test_rs232c.Name = "groupBox_test_rs232c";
      this.groupBox_test_rs232c.TabStop = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox_test_rs232c);
      this.Controls.Add((Control) this.button_salvar);
      this.Controls.Add((Control) this.textBox_host_ip);
      this.Controls.Add((Control) this.groupBox_RS232C);
      this.Controls.Add((Control) this.groupBox_tipo_com);
      this.Controls.Add((Control) this.groupBox_ETHERNET);
      this.Controls.Add((Control) this.button_cancelar);
      this.Controls.Add((Control) this.groupBox_maquina);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Form_config);
      this.Load += new EventHandler(this.Form_configurar_Load);
      this.groupBox_ETHERNET.ResumeLayout(false);
      this.groupBox_ETHERNET.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox_data_server.ResumeLayout(false);
      this.groupBox_data_server.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox_maquina.ResumeLayout(false);
      this.groupBox_maquina.PerformLayout();
      this.groupBox_tipo_com.ResumeLayout(false);
      this.groupBox_tipo_com.PerformLayout();
      this.groupBox_RS232C.ResumeLayout(false);
      this.groupBox_RS232C.PerformLayout();
      this.groupBox_test_rs232c.ResumeLayout(false);
      this.groupBox_test_rs232c.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
