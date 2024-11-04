// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_enviar_rs232c
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Enviar;
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
  public class Form_enviar_rs232c : Form
  {
    private bool parar_envio = false;
    private bool cancelar_envio = false;
    private long posicion_cursor = 0;
    private const int PAQUETE_MAX_LECTURA = 1024;
    private const int PAQUETE_MAX_ENVIO = 8;
    private int paquete_enviar = 0;
    private byte[] array_lectura = new byte[1024];
    private long datos_enviados_totales = 0;
    private long aux_datos_enviados_totales = 0;
    private IContainer components = (IContainer) null;
    private SerialPort serialPort1;
    private TextBox textBox2;
    private Button button_seleccionar_fichero;
    private OpenFileDialog openFileDialog1;
    private TextBox textBox_fichero;
    private Button button_enviar_fichero;
    private ProgressBar progressBar1;
    private Label label1;
    private Button button_parar_envio;
    private Button button_continuar_envio;
    private Button button_cancelar_envio;
    private Button button_editar;
    private RichTextBox richTextBox1;
    private SaveFileDialog saveFileDialog1;
    private GroupBox groupBox_control_envio;
    private Button button_enviar_desde_cursor;
    private Button button_salvar;
    private Button button_buscar;
    private Button button_enviar_texto;
    private GroupBox groupBox_control_texto;
    private GroupBox groupBox_control_ficheros;
    private Label label2;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem copiarToolStripMenuItem;
    private ToolStripMenuItem pegarToolStripMenuItem;
    private ToolStripMenuItem borrarToolStripMenuItem;
    private ToolStripMenuItem seleccionarTodoToolStripMenuItem;
    private ToolStripMenuItem pasarALetrasMayusculasToolStripMenuItem;
    private ToolStripMenuItem deshacerToolStripMenuItem;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private ToolStripMenuItem guardarTextoEnPantallaToolStripMenuItem;
    private Label label3;
    private ToolStripMenuItem configurarToolStripMenuItem;
    private Label label4;
    private System.Timers.Timer timer1;
    private Button button_help_send_RS232;
    private ToolStripMenuItem toolStripMenuItem1;

    public Form_enviar_rs232c() => this.InitializeComponent();

    private void button_configurar_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_config().ShowDialog();
      this.Enabled = true;
    }

    private void button_enviar_texto_Click(object sender, EventArgs e)
    {
      if (this.richTextBox1.Text.Length > 10000 && new Informacion().leer_clave(true) <= 0)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String1);
      }
      else
        new Thread(new ThreadStart(this.enviar_texto)).Start();
    }

    private void enviar_texto()
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
      Control.CheckForIllegalCrossThreadCalls = false;
      try
      {
        if (this.configurar() == -1)
          return;
        this.serialPort1.DiscardOutBuffer();
        this.timer1.Stop();
        long length = 8;
        char[] chArray = new char[length];
        char[] charArray = this.richTextBox1.Text.ToCharArray();
        long num1 = (long) charArray.Length - this.posicion_cursor;
        long num2 = num1 / length;
        long num3 = num1 % length;
        this.parar_envio = false;
        this.cancelar_envio = false;
        this.progressBar1.Maximum = (int) num1;
        this.progressBar1.Value = 0;
        this.label1.Text = Resource_Form_enviar_rs232c.String2;
        this.label1.Update();
        this.textBox2.ResetText();
        this.groupBox_control_texto.Enabled = false;
        this.groupBox_control_ficheros.Enabled = false;
        this.groupBox_control_envio.Enabled = true;
        this.button_parar_envio.Enabled = true;
        this.button_continuar_envio.Enabled = false;
        this.button_cancelar_envio.Enabled = true;
        this.button_parar_envio.Focus();
        for (int index = 0; (long) index <= num2; ++index)
        {
          long sourceIndex = (long) index * length + this.posicion_cursor;
          if ((long) index == num2)
          {
            length = num3;
            if (num3 == 0L)
              break;
          }
          Array.Copy((Array) charArray, sourceIndex, (Array) chArray, 0L, length);
          this.serialPort1.Write(chArray, 0, (int) length);
          this.label1.Text = Resource_Form_enviar_rs232c.String3 + this.progressBar1.Value.ToString();
          this.progressBar1.Value += (int) length;
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
                goto label_26;
            }
            while (this.parar_envio);
            this.button_continuar_envio.Enabled = false;
            this.button_parar_envio.Enabled = true;
            this.button_parar_envio.Focus();
label_26:;
          }
          if (this.cancelar_envio)
            break;
        }
        do
        {
          Application.DoEvents();
        }
        while (!this.cancelar_envio && this.serialPort1.BytesToWrite > 0);
        Thread.Sleep(2000);
      }
      catch (TimeoutException ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String4 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        if (this.cancelar_envio)
          return;
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        if (this.serialPort1.IsOpen)
          this.serialPort1.Close();
        this.posicion_cursor = 0L;
        this.label1.Text = Resource_Form_enviar_rs232c.String6 + this.progressBar1.Value.ToString();
        this.label1.Update();
        this.groupBox_control_texto.Enabled = true;
        this.groupBox_control_ficheros.Enabled = true;
        this.groupBox_control_envio.Enabled = false;
        Control.CheckForIllegalCrossThreadCalls = true;
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

    private void Form_enviar_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.serialPort1.IsOpen)
        this.serialPort1.Close();
      this.Close();
    }

    private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String9, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.serialPort1.Close();
    }

    private void button_salir_Click(object sender, EventArgs e)
    {
      if (this.serialPort1.IsOpen)
        this.serialPort1.Close();
      this.Close();
    }

    private void button_seleccionar_fichero_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox_fichero.Text = this.openFileDialog1.FileName;
      this.button_enviar_fichero.Enabled = true;
      this.button_editar.Enabled = true;
    }

    private void button_enviar_fichero_Click(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(this.enviar_fichero)).Start();
    }

    private void enviar_fichero()
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
      Control.CheckForIllegalCrossThreadCalls = false;
      if (this.textBox_fichero.Text == "")
      {
        int num1 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String10, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        FileStream fileStream;
        try
        {
          fileStream = new FileStream(this.textBox_fichero.Text, FileMode.Open, FileAccess.Read, FileShare.None);
          if (fileStream.Length > 10000L)
          {
            if (new Informacion().leer_clave(true) <= 0)
            {
              int num2 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              fileStream.Close();
              return;
            }
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String11 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        int num4 = 1024;
        long length = fileStream.Length;
        byte[] numArray1 = new byte[1024];
        try
        {
          if (this.configurar() == -1)
            return;
          this.serialPort1.DiscardOutBuffer();
          int num5 = num4;
          this.paquete_enviar = 8;
          byte[] numArray2 = new byte[this.paquete_enviar];
          int num6 = num5 / this.paquete_enviar;
          int num7 = num5 % this.paquete_enviar;
          this.parar_envio = false;
          this.cancelar_envio = false;
          this.progressBar1.Maximum = (int) length;
          this.progressBar1.Value = 0;
          this.label1.Text = Resource_Form_enviar_rs232c.String2;
          this.label1.Update();
          this.richTextBox1.ResetText();
          this.groupBox_control_texto.Enabled = false;
          this.groupBox_control_ficheros.Enabled = false;
          this.groupBox_control_envio.Enabled = true;
          this.button_parar_envio.Enabled = true;
          this.button_continuar_envio.Enabled = false;
          this.button_cancelar_envio.Enabled = true;
          this.button_parar_envio.Focus();
          this.datos_enviados_totales = 0L;
          this.timer1.Enabled = true;
          this.timer1.Start();
          do
          {
            Array.Clear((Array) this.array_lectura, 0, num4);
            this.paquete_enviar = 8;
            int num8 = fileStream.Read(this.array_lectura, 0, num4);
            if (num8 == 0)
            {
              do
              {
                Application.DoEvents();
              }
              while (!this.cancelar_envio && this.serialPort1.BytesToWrite > 0);
              Thread.Sleep(2000);
              return;
            }
            Array.Copy((Array) this.array_lectura, (Array) numArray1, this.array_lectura.Length);
            if (num8 < num4)
            {
              int num9 = num8;
              num6 = num9 / this.paquete_enviar;
              num7 = num9 % this.paquete_enviar;
            }
            for (int index = 0; index <= num6; ++index)
            {
              int srcOffset = index * this.paquete_enviar;
              if (index == num6)
              {
                this.paquete_enviar = num7;
                if (num7 == 0)
                  break;
              }
              Buffer.BlockCopy((Array) this.array_lectura, srcOffset, (Array) numArray2, 0, this.paquete_enviar);
              this.serialPort1.Write(numArray2, 0, this.paquete_enviar);
              this.datos_enviados_totales += (long) this.paquete_enviar;
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
                    goto label_39;
                }
                while (this.parar_envio);
                this.button_continuar_envio.Enabled = false;
                this.button_parar_envio.Enabled = true;
                this.button_parar_envio.Focus();
label_39:;
              }
              if (this.cancelar_envio)
                break;
            }
          }
          while (!this.cancelar_envio);
          Array.Copy((Array) this.array_lectura, (Array) numArray1, this.array_lectura.Length);
          this.button_enviar_fichero.Focus();
        }
        catch (TimeoutException ex)
        {
          int num10 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String12 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        catch (Exception ex)
        {
          if (this.cancelar_envio)
            return;
          int num11 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String18 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        finally
        {
          this.label1.Text = Resource_Form_enviar_rs232c.String19 + this.progressBar1.Value.ToString();
          this.label1.Update();
          fileStream.Close();
          if (this.serialPort1.IsOpen)
            this.serialPort1.Close();
          this.groupBox_control_texto.Enabled = true;
          this.groupBox_control_ficheros.Enabled = true;
          this.groupBox_control_envio.Enabled = false;
          this.timer1.Stop();
          this.richTextBox1.Text = Encoding.ASCII.GetString(numArray1);
          this.richTextBox1.SelectionStart += this.richTextBox1.Text.Length;
          this.richTextBox1.ScrollToCaret();
          Control.CheckForIllegalCrossThreadCalls = true;
        }
      }
    }

    private void button_parar_envio_Click(object sender, EventArgs e) => this.parar_envio = true;

    private void button_continuar_envio_Click(object sender, EventArgs e)
    {
      this.parar_envio = false;
    }

    private void button_cancelar_envio_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resource_Form_enviar_rs232c.String13, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.cancelar_envio = true;
      this.serialPort1.DiscardOutBuffer();
      this.timer1.Stop();
    }

    private void button_editar_Click(object sender, EventArgs e)
    {
      try
      {
        this.richTextBox1.LoadFile(this.textBox_fichero.Text, RichTextBoxStreamType.PlainText);
        this.richTextBox1.Focus();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String14 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void button_salvar_Click(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      this.saveFileDialog1.FileName = this.textBox_fichero.Text;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.richTextBox1.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
      this.textBox_fichero.Text = this.saveFileDialog1.FileName;
    }

    private void button_buscar_Click(object sender, EventArgs e)
    {
      int selectionStart = this.richTextBox1.SelectionStart;
      int num1 = this.richTextBox1.Text.IndexOf(this.textBox2.Text, selectionStart + this.textBox2.Text.Length, this.richTextBox1.Text.Length - selectionStart - this.textBox2.Text.Length);
      if (num1 > 0)
      {
        this.richTextBox1.SelectionStart = num1;
        this.richTextBox1.SelectionLength = this.textBox2.Text.Length;
        this.richTextBox1.ScrollToCaret();
        this.richTextBox1.Focus();
      }
      else
      {
        int num2 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String15, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void button_enviar_desde_cursor_Click(object sender, EventArgs e)
    {
      this.posicion_cursor = (long) this.richTextBox1.SelectionStart;
      this.button_enviar_texto_Click((object) null, (EventArgs) null);
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
      if (this.textBox2.Text == "")
        this.button_buscar.Enabled = false;
      else
        this.button_buscar.Enabled = true;
    }

    private void richTextBox1_TextChanged(object sender, EventArgs e)
    {
      if (this.richTextBox1.Text == "")
      {
        this.button_enviar_desde_cursor.Enabled = false;
        this.button_enviar_texto.Enabled = false;
        this.button_salvar.Enabled = false;
      }
      else
      {
        this.button_enviar_desde_cursor.Enabled = true;
        this.button_enviar_texto.Enabled = true;
        this.button_salvar.Enabled = true;
      }
    }

    private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Copy();
    }

    private void pasarALetrasMayusculasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.SelectedText = this.richTextBox1.SelectedText.ToUpper();
    }

    private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Undo();
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void guardarTextoEnPantallaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.richTextBox1.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
    }

    private void seleccionarTodoToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      this.richTextBox1.Focus();
      this.richTextBox1.SelectionStart = 0;
      this.richTextBox1.SelectionLength = this.richTextBox1.Text.Length;
      this.richTextBox1.Copy();
    }

    private void pegarToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      this.richTextBox1.Paste();
    }

    private void borrarToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      this.richTextBox1.SelectedText = "";
    }

    private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num1 = (int) new Form_config().ShowDialog();
      this.Enabled = true;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String16);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "RS232C")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_enviar_rs232c.String17);
          this.Close();
        }
        this.Form_enviar_rs232c_Load((object) null, (EventArgs) null);
      }
    }

    private void Form_enviar_rs232c_Load(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) > 0)
        return;
      if (informacion.leer_contador_uso(true) == -1)
        this.Close();
      if (informacion.leer_fecha_uso() == -1)
        this.Close();
    }

    private void timer1_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.label1.Text = Resource_Form_enviar_rs232c.String20 + this.progressBar1.Value.ToString();
      this.progressBar1.Value = (int) this.datos_enviados_totales;
      if (this.datos_enviados_totales > this.aux_datos_enviados_totales)
      {
        this.richTextBox1.Text = Encoding.ASCII.GetString(this.array_lectura);
        this.richTextBox1.SelectionStart += this.richTextBox1.Text.Length;
        this.richTextBox1.ScrollToCaret();
      }
      this.aux_datos_enviados_totales = this.datos_enviados_totales;
    }

    private void button_help_send_RS232_Click(object sender, EventArgs e)
    {
      int num = (int) new Form_informacion_general()
      {
        Fichero_mostrar = "help_send_data_RS232"
      }.ShowDialog();
    }

    private void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.button_help_send_RS232_Click((object) null, (EventArgs) null);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_enviar_rs232c));
      this.serialPort1 = new SerialPort(this.components);
      this.textBox2 = new TextBox();
      this.button_seleccionar_fichero = new Button();
      this.openFileDialog1 = new OpenFileDialog();
      this.textBox_fichero = new TextBox();
      this.button_enviar_fichero = new Button();
      this.progressBar1 = new ProgressBar();
      this.label1 = new Label();
      this.button_parar_envio = new Button();
      this.button_continuar_envio = new Button();
      this.button_cancelar_envio = new Button();
      this.button_editar = new Button();
      this.richTextBox1 = new RichTextBox();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.copiarToolStripMenuItem = new ToolStripMenuItem();
      this.pegarToolStripMenuItem = new ToolStripMenuItem();
      this.borrarToolStripMenuItem = new ToolStripMenuItem();
      this.seleccionarTodoToolStripMenuItem = new ToolStripMenuItem();
      this.pasarALetrasMayusculasToolStripMenuItem = new ToolStripMenuItem();
      this.deshacerToolStripMenuItem = new ToolStripMenuItem();
      this.saveFileDialog1 = new SaveFileDialog();
      this.groupBox_control_envio = new GroupBox();
      this.button_enviar_desde_cursor = new Button();
      this.button_salvar = new Button();
      this.button_buscar = new Button();
      this.button_enviar_texto = new Button();
      this.groupBox_control_texto = new GroupBox();
      this.label2 = new Label();
      this.groupBox_control_ficheros = new GroupBox();
      this.label3 = new Label();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.guardarTextoEnPantallaToolStripMenuItem = new ToolStripMenuItem();
      this.configurarToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripMenuItem();
      this.label4 = new Label();
      this.timer1 = new System.Timers.Timer();
      this.button_help_send_RS232 = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.groupBox_control_envio.SuspendLayout();
      this.groupBox_control_texto.SuspendLayout();
      this.groupBox_control_ficheros.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.timer1.BeginInit();
      this.SuspendLayout();
      this.serialPort1.WriteTimeout = 600000;
      this.serialPort1.ErrorReceived += new SerialErrorReceivedEventHandler(this.serialPort1_ErrorReceived);
      componentResourceManager.ApplyResources((object) this.textBox2, "textBox2");
      this.textBox2.Name = "textBox2";
      this.textBox2.TextChanged += new EventHandler(this.textBox2_TextChanged);
      this.button_seleccionar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_seleccionar_fichero, "button_seleccionar_fichero");
      this.button_seleccionar_fichero.Name = "button_seleccionar_fichero";
      this.button_seleccionar_fichero.UseVisualStyleBackColor = false;
      this.button_seleccionar_fichero.Click += new EventHandler(this.button_seleccionar_fichero_Click);
      this.openFileDialog1.FileName = "openFileDialog1";
      componentResourceManager.ApplyResources((object) this.openFileDialog1, "openFileDialog1");
      componentResourceManager.ApplyResources((object) this.textBox_fichero, "textBox_fichero");
      this.textBox_fichero.Name = "textBox_fichero";
      this.button_enviar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_enviar_fichero, "button_enviar_fichero");
      this.button_enviar_fichero.Name = "button_enviar_fichero";
      this.button_enviar_fichero.UseVisualStyleBackColor = false;
      this.button_enviar_fichero.Click += new EventHandler(this.button_enviar_fichero_Click);
      componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
      this.progressBar1.Name = "progressBar1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.button_parar_envio.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      componentResourceManager.ApplyResources((object) this.button_parar_envio, "button_parar_envio");
      this.button_parar_envio.Name = "button_parar_envio";
      this.button_parar_envio.UseVisualStyleBackColor = false;
      this.button_parar_envio.Click += new EventHandler(this.button_parar_envio_Click);
      this.button_continuar_envio.AccessibleRole = AccessibleRole.None;
      this.button_continuar_envio.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_continuar_envio, "button_continuar_envio");
      this.button_continuar_envio.Name = "button_continuar_envio";
      this.button_continuar_envio.UseVisualStyleBackColor = false;
      this.button_continuar_envio.Click += new EventHandler(this.button_continuar_envio_Click);
      this.button_cancelar_envio.AccessibleRole = AccessibleRole.None;
      this.button_cancelar_envio.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      componentResourceManager.ApplyResources((object) this.button_cancelar_envio, "button_cancelar_envio");
      this.button_cancelar_envio.Name = "button_cancelar_envio";
      this.button_cancelar_envio.UseVisualStyleBackColor = false;
      this.button_cancelar_envio.Click += new EventHandler(this.button_cancelar_envio_Click);
      this.button_editar.AccessibleRole = AccessibleRole.None;
      this.button_editar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_editar, "button_editar");
      this.button_editar.Name = "button_editar";
      this.button_editar.UseVisualStyleBackColor = false;
      this.button_editar.Click += new EventHandler(this.button_editar_Click);
      this.richTextBox1.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
      componentResourceManager.ApplyResources((object) this.richTextBox1, "richTextBox1");
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.TextChanged += new EventHandler(this.richTextBox1_TextChanged);
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.copiarToolStripMenuItem,
        (ToolStripItem) this.pegarToolStripMenuItem,
        (ToolStripItem) this.borrarToolStripMenuItem,
        (ToolStripItem) this.seleccionarTodoToolStripMenuItem,
        (ToolStripItem) this.pasarALetrasMayusculasToolStripMenuItem,
        (ToolStripItem) this.deshacerToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
      this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.copiarToolStripMenuItem, "copiarToolStripMenuItem");
      this.copiarToolStripMenuItem.Click += new EventHandler(this.copiarToolStripMenuItem_Click);
      this.pegarToolStripMenuItem.Name = "pegarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.pegarToolStripMenuItem, "pegarToolStripMenuItem");
      this.pegarToolStripMenuItem.Click += new EventHandler(this.pegarToolStripMenuItem_Click_1);
      this.borrarToolStripMenuItem.Name = "borrarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.borrarToolStripMenuItem, "borrarToolStripMenuItem");
      this.borrarToolStripMenuItem.Click += new EventHandler(this.borrarToolStripMenuItem_Click_1);
      this.seleccionarTodoToolStripMenuItem.Name = "seleccionarTodoToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.seleccionarTodoToolStripMenuItem, "seleccionarTodoToolStripMenuItem");
      this.seleccionarTodoToolStripMenuItem.Click += new EventHandler(this.seleccionarTodoToolStripMenuItem_Click_1);
      this.pasarALetrasMayusculasToolStripMenuItem.Name = "pasarALetrasMayusculasToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.pasarALetrasMayusculasToolStripMenuItem, "pasarALetrasMayusculasToolStripMenuItem");
      this.pasarALetrasMayusculasToolStripMenuItem.Click += new EventHandler(this.pasarALetrasMayusculasToolStripMenuItem_Click);
      this.deshacerToolStripMenuItem.Name = "deshacerToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.deshacerToolStripMenuItem, "deshacerToolStripMenuItem");
      this.deshacerToolStripMenuItem.Click += new EventHandler(this.deshacerToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.saveFileDialog1, "saveFileDialog1");
      this.groupBox_control_envio.Controls.Add((Control) this.button_parar_envio);
      this.groupBox_control_envio.Controls.Add((Control) this.button_continuar_envio);
      this.groupBox_control_envio.Controls.Add((Control) this.button_cancelar_envio);
      componentResourceManager.ApplyResources((object) this.groupBox_control_envio, "groupBox_control_envio");
      this.groupBox_control_envio.Name = "groupBox_control_envio";
      this.groupBox_control_envio.TabStop = false;
      this.button_enviar_desde_cursor.AccessibleRole = AccessibleRole.None;
      this.button_enviar_desde_cursor.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_enviar_desde_cursor, "button_enviar_desde_cursor");
      this.button_enviar_desde_cursor.Name = "button_enviar_desde_cursor";
      this.button_enviar_desde_cursor.UseVisualStyleBackColor = false;
      this.button_enviar_desde_cursor.Click += new EventHandler(this.button_enviar_desde_cursor_Click);
      this.button_salvar.AccessibleRole = AccessibleRole.None;
      this.button_salvar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_salvar, "button_salvar");
      this.button_salvar.Name = "button_salvar";
      this.button_salvar.UseVisualStyleBackColor = false;
      this.button_salvar.Click += new EventHandler(this.button_salvar_Click);
      this.button_buscar.AccessibleRole = AccessibleRole.None;
      this.button_buscar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_buscar, "button_buscar");
      this.button_buscar.Name = "button_buscar";
      this.button_buscar.UseVisualStyleBackColor = false;
      this.button_buscar.Click += new EventHandler(this.button_buscar_Click);
      this.button_enviar_texto.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_enviar_texto, "button_enviar_texto");
      this.button_enviar_texto.Name = "button_enviar_texto";
      this.button_enviar_texto.UseVisualStyleBackColor = false;
      this.button_enviar_texto.Click += new EventHandler(this.button_enviar_texto_Click);
      this.groupBox_control_texto.Controls.Add((Control) this.label2);
      this.groupBox_control_texto.Controls.Add((Control) this.button_enviar_texto);
      this.groupBox_control_texto.Controls.Add((Control) this.button_buscar);
      this.groupBox_control_texto.Controls.Add((Control) this.button_salvar);
      this.groupBox_control_texto.Controls.Add((Control) this.button_enviar_desde_cursor);
      this.groupBox_control_texto.Controls.Add((Control) this.textBox2);
      componentResourceManager.ApplyResources((object) this.groupBox_control_texto, "groupBox_control_texto");
      this.groupBox_control_texto.Name = "groupBox_control_texto";
      this.groupBox_control_texto.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.groupBox_control_ficheros.Controls.Add((Control) this.label3);
      this.groupBox_control_ficheros.Controls.Add((Control) this.button_seleccionar_fichero);
      this.groupBox_control_ficheros.Controls.Add((Control) this.button_enviar_fichero);
      this.groupBox_control_ficheros.Controls.Add((Control) this.button_editar);
      this.groupBox_control_ficheros.Controls.Add((Control) this.textBox_fichero);
      componentResourceManager.ApplyResources((object) this.groupBox_control_ficheros, "groupBox_control_ficheros");
      this.groupBox_control_ficheros.Name = "groupBox_control_ficheros";
      this.groupBox_control_ficheros.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configurarToolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem1
      });
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.Name = "menuStrip1";
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.salirToolStripMenuItem,
        (ToolStripItem) this.guardarTextoEnPantallaToolStripMenuItem
      });
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.guardarTextoEnPantallaToolStripMenuItem, "guardarTextoEnPantallaToolStripMenuItem");
      this.guardarTextoEnPantallaToolStripMenuItem.Name = "guardarTextoEnPantallaToolStripMenuItem";
      this.guardarTextoEnPantallaToolStripMenuItem.Click += new EventHandler(this.guardarTextoEnPantallaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.configurarToolStripMenuItem, "configurarToolStripMenuItem");
      this.configurarToolStripMenuItem.Name = "configurarToolStripMenuItem";
      this.configurarToolStripMenuItem.Click += new EventHandler(this.configurarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem1, "toolStripMenuItem1");
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItem1_Click);
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.ForeColor = System.Drawing.Color.Red;
      this.label4.Name = "label4";
      this.timer1.Interval = 1000.0;
      this.timer1.SynchronizingObject = (ISynchronizeInvoke) this;
      this.timer1.Elapsed += new ElapsedEventHandler(this.timer1_Elapsed);
      this.button_help_send_RS232.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      componentResourceManager.ApplyResources((object) this.button_help_send_RS232, "button_help_send_RS232");
      this.button_help_send_RS232.ForeColor = System.Drawing.Color.Red;
      this.button_help_send_RS232.Name = "button_help_send_RS232";
      this.button_help_send_RS232.UseVisualStyleBackColor = false;
      this.button_help_send_RS232.Click += new EventHandler(this.button_help_send_RS232_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.button_help_send_RS232);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.groupBox_control_ficheros);
      this.Controls.Add((Control) this.groupBox_control_texto);
      this.Controls.Add((Control) this.groupBox_control_envio);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.progressBar1);
      this.Controls.Add((Control) this.richTextBox1);
      this.Name = nameof (Form_enviar_rs232c);
      this.Tag = (object) "  ";
      this.FormClosed += new FormClosedEventHandler(this.Form_enviar_FormClosed);
      this.Load += new EventHandler(this.Form_enviar_rs232c_Load);
      this.contextMenuStrip1.ResumeLayout(false);
      this.groupBox_control_envio.ResumeLayout(false);
      this.groupBox_control_texto.ResumeLayout(false);
      this.groupBox_control_texto.PerformLayout();
      this.groupBox_control_ficheros.ResumeLayout(false);
      this.groupBox_control_ficheros.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.timer1.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
