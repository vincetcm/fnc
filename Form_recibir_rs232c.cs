// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_recibir_rs232c
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Recibir;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_recibir_rs232c : Form
  {
    private bool cancelar = false;
    private bool error_recibir = false;
    private int tiempo_fin = 4;
    private bool timer_end = false;
    private IContainer components = (IContainer) null;
    private SerialPort serialPort1;
    private TextBox textBox3;
    private Button button_recibir_fichero;
    private ProgressBar progressBar1;
    private Label label1;
    private Button button_cancelar;
    private RichTextBox richTextBox1;
    private Button button_seleccionar_fichero;
    private FolderBrowserDialog folderBrowserDialog1;
    private Button button_seleccionar_directorio;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private Timer timer1;
    private Label label3;
    private Label label4;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private ToolStripMenuItem guardarTextoEnPantallaToolStripMenuItem;
    private SaveFileDialog saveFileDialog1;
    private ToolStripMenuItem configurarToolStripMenuItem;
    private CheckBox checkBox_text;
    private CheckBox checkBox_spaces;
    private GroupBox groupBox3;

    public Form_recibir_rs232c() => this.InitializeComponent();

    private void button_configurar_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_config().ShowDialog();
      this.Enabled = true;
    }

    private int configurar()
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
      {
        int num = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      string str1 = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false);
      if (registryKey2 == null)
      {
        int num = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        this.serialPort1.Open();
        this.serialPort1.DiscardOutBuffer();
        this.serialPort1.DiscardInBuffer();
        this.serialPort1.DtrEnable = true;
        return 0;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String2 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
    }

    private void Form_recibir_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (!this.serialPort1.IsOpen)
        return;
      this.serialPort1.Close();
    }

    private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      this.error_recibir = true;
    }

    private void button_salir_Click(object sender, EventArgs e)
    {
      if (this.serialPort1.IsOpen)
        this.serialPort1.Close();
      this.Close();
    }

    private void button_seleccionar_fichero_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.textBox3.Text = openFileDialog.FileName;
      this.button_recibir_fichero.Enabled = true;
    }

    private void button_recibir_fichero_Click(object sender, EventArgs e)
    {
      if (this.textBox3.Text == "")
      {
        int num1 = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (File.Exists(this.textBox3.Text) && MessageBox.Show(Resource_Form_recibir_rs232c.String4, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
          return;
        FileStream fileStream;
        try
        {
          fileStream = new FileStream(this.textBox3.Text, FileMode.Create, FileAccess.Write, FileShare.None);
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        int count1 = 1000;
        byte[] numArray1 = new byte[count1];
        long num3 = 0;
        byte[] numArray2 = new byte[10000 + count1];
        try
        {
          this.progressBar1.Maximum = 10000;
          this.progressBar1.Value = 0;
          this.label1.Text = Resource_Form_recibir_rs232c.String6;
          this.richTextBox1.ResetText();
          this.button_cancelar.Enabled = true;
          this.button_recibir_fichero.Enabled = false;
          this.button_seleccionar_fichero.Enabled = false;
          this.button_seleccionar_directorio.Enabled = false;
          this.textBox3.Focus();
          if (this.configurar() == -1)
            return;
          this.serialPort1.DiscardInBuffer();
          this.timer1.Interval = this.tiempo_fin * 1000;
          this.timer1.Stop();
          int count2 = 0;
          this.cancelar = false;
          this.timer_end = false;
          this.error_recibir = false;
          bool flag = false;
          do
          {
            Array.Clear((Array) numArray1, 0, numArray1.Length);
            if (flag)
              this.timer1.Start();
            if (this.serialPort1.BytesToRead > 0)
            {
              flag = true;
              this.timer1.Stop();
              int count3 = this.serialPort1.Read(numArray1, 0, count1);
              Application.DoEvents();
              if (!this.error_recibir)
              {
                num3 += (long) count3;
                this.label1.Text = Resource_Form_recibir_rs232c.String7 + num3.ToString();
                count2 += count3;
                Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, count2 - count3, count3);
                if (count2 >= 10000)
                {
                  fileStream.Write(numArray2, 0, count2);
                  count2 = 0;
                }
                if (this.richTextBox1.Text.Length >= 10000)
                  this.richTextBox1.Text.Remove(0);
                this.richTextBox1.Text += Encoding.ASCII.GetString(numArray1);
                this.richTextBox1.SelectionStart += this.richTextBox1.Text.Length;
                this.richTextBox1.ScrollToCaret();
                this.progressBar1.Value = count2;
              }
              else
                goto label_41;
            }
            Application.DoEvents();
          }
          while (!this.cancelar && !this.timer_end);
          goto label_19;
label_41:
          return;
label_19:
          fileStream.Write(numArray2, 0, count2);
          fileStream.Close();
          string text = this.textBox3.Text;
          if (this.checkBox_text.Checked)
          {
            string end;
            using (StreamReader streamReader = File.OpenText(text))
              end = streamReader.ReadToEnd();
            string contents = Form_recibir_rs232c.convertir_texto(end);
            File.WriteAllText(text, contents);
          }
          if (this.checkBox_spaces.Checked)
          {
            string end;
            using (StreamReader streamReader = File.OpenText(text))
              end = streamReader.ReadToEnd();
            string contents = Form_recibir_rs232c.introducir_espacios(end);
            File.WriteAllText(text, contents);
          }
          this.cancelar = false;
          this.timer_end = false;
        }
        catch (Exception ex)
        {
          int num4 = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String8 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        finally
        {
          if (this.error_recibir)
          {
            int num5 = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String9, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.label1.Text = Resource_Form_recibir_rs232c.String10 + num3.ToString();
          fileStream.Close();
          if (this.serialPort1.IsOpen)
            this.serialPort1.Close();
          this.button_cancelar.Enabled = false;
          this.button_recibir_fichero.Enabled = true;
          this.button_seleccionar_fichero.Enabled = true;
          this.button_seleccionar_directorio.Enabled = true;
          this.button_recibir_fichero.Focus();
          this.progressBar1.Value = this.progressBar1.Maximum;
        }
      }
    }

    private static string introducir_espacios(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = 0;
      int num2 = 0;
      bool flag1 = false;
      foreach (char ch in input)
      {
        if (ch == '(')
          flag1 = true;
        if (ch == ')')
          flag1 = false;
        if (ch == '\r' || ch == '\n')
        {
          flag1 = false;
          num1 = 0;
          num2 = 0;
        }
        bool flag2;
        if (ch >= 'A' && ch <= 'Z' && !flag1)
        {
          flag2 = true;
          ++num1;
          ++num2;
        }
        else
        {
          flag2 = false;
          num2 = 0;
        }
        if (ch == '/' && !flag1)
          ++num1;
        if (flag2 && num1 > 1 && !flag1 && num2 == 1)
          stringBuilder.Append(' ');
        stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    private static string convertir_texto(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      foreach (char ch in input)
      {
        if (!flag1 || ch != '\n' && ch != '\r')
        {
          flag1 = false;
          switch (ch)
          {
            case '\n':
              stringBuilder.Append("\r\n");
              flag1 = true;
              break;
            case '\r':
              stringBuilder.Append("\r\n");
              flag1 = true;
              break;
            default:
              if (ch > '\u001F')
              {
                if (ch == '%')
                  flag3 = true;
                if (flag3)
                {
                  if (ch >= '0' && ch <= '9')
                    flag2 = true;
                  if (!flag2 && ch == ':')
                  {
                    stringBuilder.Append('O');
                    break;
                  }
                  stringBuilder.Append(ch);
                  break;
                }
                break;
              }
              break;
          }
        }
      }
      return stringBuilder.ToString();
    }

    private void button_cancelar_Click(object sender, EventArgs e) => this.cancelar = true;

    private void button_seleccionar_fichero_Click_1(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox3.Text = this.saveFileDialog1.FileName;
      this.button_recibir_fichero.Enabled = true;
      this.textBox3.SelectionStart = this.textBox3.Text.Length;
      this.textBox3.Focus();
    }

    private void button_seleccionar_directorio_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.textBox3.Text = folderBrowserDialog.SelectedPath + "\\";
      this.textBox3.SelectionStart = this.textBox3.Text.Length;
      this.textBox3.Focus();
      Environment.CurrentDirectory = this.textBox3.Text;
    }

    private void timer1_Tick(object sender, EventArgs e) => this.timer_end = true;

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void salirToolStripMenuItem_Click_1(object sender, EventArgs e) => this.Close();

    private void guardarTextoEnPantallaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.richTextBox1.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
    }

    private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num1 = (int) new Form_config().ShowDialog();
      this.Enabled = true;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String11);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "RS232C")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_recibir_rs232c.String12);
          this.Close();
        }
        this.Form_recibir_rs232c_Load((object) null, (EventArgs) null);
      }
    }

    private void Form_recibir_rs232c_Load(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0)
      {
        if (informacion.leer_contador_uso(true) == -1)
          this.Close();
        if (informacion.leer_fecha_uso() == -1)
          this.Close();
      }
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
        return;
      string str = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, true);
      this.checkBox_text.Checked = (string) registryKey2.GetValue("Text_mode") == "ON";
      this.checkBox_spaces.Checked = (string) registryKey2.GetValue("Space_mode") == "ON";
    }

    private void checkBox_text_CheckedChanged(object sender, EventArgs e)
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
        return;
      string str = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, true);
      if (this.checkBox_text.Checked)
        registryKey2.SetValue("Text_mode", (object) "ON");
      else
        registryKey2.SetValue("Text_mode", (object) "OFF");
    }

    private void checkBox_spaces_CheckedChanged(object sender, EventArgs e)
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
        return;
      string str = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, true);
      if (this.checkBox_spaces.Checked)
        registryKey2.SetValue("Space_mode", (object) "ON");
      else
        registryKey2.SetValue("Space_mode", (object) "OFF");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_recibir_rs232c));
      this.serialPort1 = new SerialPort(this.components);
      this.textBox3 = new TextBox();
      this.button_recibir_fichero = new Button();
      this.progressBar1 = new ProgressBar();
      this.label1 = new Label();
      this.button_cancelar = new Button();
      this.richTextBox1 = new RichTextBox();
      this.button_seleccionar_fichero = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.button_seleccionar_directorio = new Button();
      this.groupBox1 = new GroupBox();
      this.checkBox_spaces = new CheckBox();
      this.checkBox_text = new CheckBox();
      this.groupBox2 = new GroupBox();
      this.label3 = new Label();
      this.timer1 = new Timer(this.components);
      this.label4 = new Label();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.guardarTextoEnPantallaToolStripMenuItem = new ToolStripMenuItem();
      this.configurarToolStripMenuItem = new ToolStripMenuItem();
      this.saveFileDialog1 = new SaveFileDialog();
      this.groupBox3 = new GroupBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.serialPort1.DataBits = 7;
      this.serialPort1.DtrEnable = true;
      this.serialPort1.Handshake = Handshake.RequestToSendXOnXOff;
      this.serialPort1.Parity = Parity.Even;
      this.serialPort1.ReadTimeout = 5000;
      this.serialPort1.StopBits = StopBits.Two;
      this.serialPort1.WriteBufferSize = 4096;
      this.serialPort1.WriteTimeout = 5000;
      this.serialPort1.ErrorReceived += new SerialErrorReceivedEventHandler(this.serialPort1_ErrorReceived);
      componentResourceManager.ApplyResources((object) this.textBox3, "textBox3");
      this.textBox3.Name = "textBox3";
      componentResourceManager.ApplyResources((object) this.button_recibir_fichero, "button_recibir_fichero");
      this.button_recibir_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_recibir_fichero.Name = "button_recibir_fichero";
      this.button_recibir_fichero.UseVisualStyleBackColor = false;
      this.button_recibir_fichero.Click += new EventHandler(this.button_recibir_fichero_Click);
      componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
      this.progressBar1.Name = "progressBar1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.ForeColor = System.Drawing.Color.Red;
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.AccessibleRole = AccessibleRole.None;
      this.button_cancelar.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button_cancelar_Click);
      componentResourceManager.ApplyResources((object) this.richTextBox1, "richTextBox1");
      this.richTextBox1.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox1.Name = "richTextBox1";
      componentResourceManager.ApplyResources((object) this.button_seleccionar_fichero, "button_seleccionar_fichero");
      this.button_seleccionar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_seleccionar_fichero.Name = "button_seleccionar_fichero";
      this.button_seleccionar_fichero.UseVisualStyleBackColor = false;
      this.button_seleccionar_fichero.Click += new EventHandler(this.button_seleccionar_fichero_Click_1);
      componentResourceManager.ApplyResources((object) this.folderBrowserDialog1, "folderBrowserDialog1");
      this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
      componentResourceManager.ApplyResources((object) this.button_seleccionar_directorio, "button_seleccionar_directorio");
      this.button_seleccionar_directorio.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_seleccionar_directorio.Name = "button_seleccionar_directorio";
      this.button_seleccionar_directorio.UseVisualStyleBackColor = false;
      this.button_seleccionar_directorio.Click += new EventHandler(this.button_seleccionar_directorio_Click);
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.button_cancelar);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.progressBar1);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.checkBox_spaces, "checkBox_spaces");
      this.checkBox_spaces.Name = "checkBox_spaces";
      this.checkBox_spaces.UseVisualStyleBackColor = true;
      this.checkBox_spaces.CheckedChanged += new EventHandler(this.checkBox_spaces_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBox_text, "checkBox_text");
      this.checkBox_text.Name = "checkBox_text";
      this.checkBox_text.UseVisualStyleBackColor = true;
      this.checkBox_text.CheckedChanged += new EventHandler(this.checkBox_text_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.button_seleccionar_fichero);
      this.groupBox2.Controls.Add((Control) this.button_seleccionar_directorio);
      this.groupBox2.Controls.Add((Control) this.button_recibir_fichero);
      this.groupBox2.Controls.Add((Control) this.textBox3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configurarToolStripMenuItem
      });
      this.menuStrip1.Name = "menuStrip1";
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.salirToolStripMenuItem,
        (ToolStripItem) this.guardarTextoEnPantallaToolStripMenuItem
      });
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click_1);
      componentResourceManager.ApplyResources((object) this.guardarTextoEnPantallaToolStripMenuItem, "guardarTextoEnPantallaToolStripMenuItem");
      this.guardarTextoEnPantallaToolStripMenuItem.Name = "guardarTextoEnPantallaToolStripMenuItem";
      this.guardarTextoEnPantallaToolStripMenuItem.Click += new EventHandler(this.guardarTextoEnPantallaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.configurarToolStripMenuItem, "configurarToolStripMenuItem");
      this.configurarToolStripMenuItem.Name = "configurarToolStripMenuItem";
      this.configurarToolStripMenuItem.Click += new EventHandler(this.configurarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.saveFileDialog1, "saveFileDialog1");
      componentResourceManager.ApplyResources((object) this.groupBox3, "groupBox3");
      this.groupBox3.Controls.Add((Control) this.checkBox_text);
      this.groupBox3.Controls.Add((Control) this.checkBox_spaces);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.TabStop = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.richTextBox1);
      this.Name = nameof (Form_recibir_rs232c);
      this.Tag = (object) " ";
      this.FormClosed += new FormClosedEventHandler(this.Form_recibir_FormClosed);
      this.Load += new EventHandler(this.Form_recibir_rs232c_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
