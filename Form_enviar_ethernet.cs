// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_enviar_ethernet
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Enviar;
using FANUC_Open_Com.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_enviar_ethernet : Form
  {
    private bool parar_envio = false;
    private bool cancelar_envio = false;
    private long posicion_cursor = 0;
    private ushort hndl = 0;
    private Focas1.focas_ret ret_focas;
    private Class_chequeo_errores_focas chequeo = new Class_chequeo_errores_focas();
    private int selected_path = 0;
    private IContainer components = (IContainer) null;
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
    private GroupBox groupBox1;
    private Button button_enviar_desde_cursor;
    private Button button_salvar;
    private Button button_buscar;
    private Button button_enviar_texto;
    private GroupBox groupBox2;
    private GroupBox groupBox3;
    private Label label2;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem copiarToolStripMenuItem;
    private ToolStripMenuItem pegarToolStripMenuItem;
    private ToolStripMenuItem borrarToolStripMenuItem;
    private ToolStripMenuItem seleccionarTodoToolStripMenuItem;
    private ToolStripMenuItem pasarALetrasMayusculasToolStripMenuItem;
    private CheckBox checkBox_dnc;
    private Button button_reset;
    private Button button_stop;
    private ToolStripMenuItem deshacerToolStripMenuItem;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private ToolStripMenuItem listadoDeProgramasEnCNCToolStripMenuItem;
    private ToolStripMenuItem guardarTextoEnPantallaToolStripMenuItem;
    private Button button_visualizar_CNC;
    private ToolStripMenuItem configuracionToolStripMenuItem;
    private ComboBox comboBox_data_select;
    private GroupBox groupBox_datos_envio;
    private Label label3;
    private ComboBox comboBox_canal;
    private Label label5;
    private Label label4;
    private ToolStripMenuItem explorador_toolStripMenuItem1;

    public Form_enviar_ethernet() => this.InitializeComponent();

    private void button_enviar_texto_Click(object sender, EventArgs e)
    {
      if (this.richTextBox1.Text.Length > 10000 && new Informacion().leer_clave(true) <= 0)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          if (this.checkBox_dnc.Checked)
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dncstart2(this.hndl, (object) new char[16]);
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dncstart2", this.ret_focas) != 0)
              return;
          }
          else
          {
            short a;
            switch (this.comboBox_data_select.SelectedIndex)
            {
              case 0:
                a = (short) 0;
                break;
              case 1:
                a = (short) 1;
                break;
              case 2:
                a = (short) 5;
                break;
              case 3:
                a = (short) 4;
                break;
              case 4:
                a = (short) 2;
                break;
              case 5:
                a = (short) 3;
                break;
              case 6:
                a = (short) 18;
                break;
              default:
                int num2 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String3);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnstart3(this.hndl, a);
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dwnstart3", this.ret_focas) != 0)
              return;
          }
          long num3 = 256;
          long length = num3;
          char[] chArray = new char[length];
          char[] charArray = this.richTextBox1.Text.ToCharArray();
          long num4 = (long) charArray.Length - this.posicion_cursor;
          this.parar_envio = false;
          this.cancelar_envio = false;
          this.progressBar1.Maximum = (int) num4;
          this.progressBar1.Value = 0;
          this.label1.Text = Resource_Form_enviar_ethernet.String4;
          this.label1.Update();
          this.button_seleccionar_fichero.Enabled = false;
          this.button_enviar_texto.Enabled = false;
          this.button_enviar_fichero.Enabled = false;
          this.button_parar_envio.Enabled = true;
          this.button_continuar_envio.Enabled = false;
          this.button_cancelar_envio.Enabled = true;
          this.button_parar_envio.Focus();
          long num5 = 0;
          while (num5 < num4)
          {
            long sourceIndex = num5 + this.posicion_cursor;
            if (num4 - num5 < num3)
              length = num4 - num5;
            Array.Copy((Array) charArray, sourceIndex, (Array) chArray, 0L, length);
            int a = (int) length;
            bool flag = false;
            if (this.checkBox_dnc.Checked && !flag)
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dnc2(this.hndl, ref a, (object) chArray);
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dnc2", this.ret_focas) != 0)
                break;
            }
            else
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_download3(this.hndl, ref a, (object) chArray);
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_download3", this.ret_focas) != 0)
                break;
            }
            if (this.ret_focas == Focas1.focas_ret.EW_OK)
              num5 += (long) a;
            this.label1.Text = Resource_Form_enviar_ethernet.String1 + this.progressBar1.Value.ToString();
            this.progressBar1.Value = (int) num5;
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
                  goto label_32;
              }
              while (this.parar_envio);
              this.button_continuar_envio.Enabled = false;
              this.button_parar_envio.Enabled = true;
              this.button_parar_envio.Focus();
label_32:;
            }
            Application.DoEvents();
            if (this.cancelar_envio)
              break;
          }
        }
        catch (Exception ex)
        {
          int num6 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        finally
        {
          if (this.checkBox_dnc.Checked)
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dncend2(this.hndl, (short) -1);
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dncend2", this.ret_focas);
          }
          else
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnend3(this.hndl);
            Cursor.Current = Cursors.Default;
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dwnend3", this.ret_focas);
          }
          this.posicion_cursor = 0L;
          this.label1.Text = Resource_Form_enviar_ethernet.String6 + this.progressBar1.Value.ToString();
          this.label1.Update();
          this.button_seleccionar_fichero.Enabled = true;
          this.button_enviar_texto.Enabled = true;
          this.button_enviar_fichero.Enabled = true;
          this.button_parar_envio.Enabled = false;
          this.button_continuar_envio.Enabled = false;
          this.button_cancelar_envio.Enabled = false;
          this.button_editar.Enabled = true;
        }
      }
    }

    private void Form_enviar_FormClosed(object sender, FormClosedEventArgs e)
    {
      int num = (int) Focas1.cnc_freelibhndl(this.hndl);
      this.Close();
    }

    private void button_salir_Click(object sender, EventArgs e)
    {
      int num = (int) Focas1.cnc_freelibhndl(this.hndl);
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
      if (this.textBox_fichero.Text == "")
      {
        int num1 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
              int num2 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              fileStream.Close();
              return;
            }
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String8 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        int length1 = 1024;
        byte[] numArray1 = new byte[length1];
        long length2 = fileStream.Length;
        try
        {
          if (this.checkBox_dnc.Checked)
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dncstart2(this.hndl, (object) new char[16]);
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dncstart2", this.ret_focas) != 0)
              return;
          }
          else
          {
            short a;
            switch (this.comboBox_data_select.SelectedIndex)
            {
              case 0:
                a = (short) 0;
                break;
              case 1:
                a = (short) 1;
                break;
              case 2:
                a = (short) 5;
                break;
              case 3:
                a = (short) 4;
                break;
              case 4:
                a = (short) 2;
                break;
              case 5:
                a = (short) 3;
                break;
              default:
                int num4 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String3);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnstart3(this.hndl, a);
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dwnstart3", this.ret_focas) != 0)
              return;
          }
          byte[] numArray2 = new byte[1024];
          this.parar_envio = false;
          this.cancelar_envio = false;
          this.progressBar1.Maximum = (int) length2;
          this.progressBar1.Value = 0;
          this.label1.Text = "Intentando enviar";
          this.label1.Update();
          this.richTextBox1.ResetText();
          this.button_seleccionar_fichero.Enabled = false;
          this.button_enviar_texto.Enabled = false;
          this.button_enviar_fichero.Enabled = false;
          this.button_parar_envio.Enabled = true;
          this.button_continuar_envio.Enabled = false;
          this.button_cancelar_envio.Enabled = true;
          this.button_editar.Enabled = false;
          this.button_parar_envio.Focus();
          bool flag = false;
          while (!flag)
          {
            Array.Clear((Array) numArray1, 0, length1);
            int num5 = 1024;
            int num6 = fileStream.Read(numArray1, 0, length1);
            if (num6 != 0)
            {
              if (num6 < num5)
                num5 = num6;
              int num7 = 0;
              while (num5 > num7)
              {
                int srcOffset = num7;
                int count = num5 - num7;
                Buffer.BlockCopy((Array) numArray1, srcOffset, (Array) numArray2, 0, count);
                int a = count;
                if (this.checkBox_dnc.Checked)
                {
                  this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dnc2(this.hndl, ref a, (object) numArray2);
                  if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dnc2", this.ret_focas) != 0)
                  {
                    flag = true;
                    break;
                  }
                }
                else
                {
                  this.ret_focas = (Focas1.focas_ret) Focas1.cnc_download3(this.hndl, ref a, (object) numArray2);
                  if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_download3", this.ret_focas) != 0)
                  {
                    flag = true;
                    break;
                  }
                }
                if (this.ret_focas == Focas1.focas_ret.EW_OK)
                {
                  num7 += a;
                  this.progressBar1.Value += a;
                  this.label1.Text = Resource_Form_enviar_ethernet.String20 + this.progressBar1.Value.ToString();
                }
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
                      goto label_37;
                  }
                  while (this.parar_envio);
                  this.button_continuar_envio.Enabled = false;
                  this.button_parar_envio.Enabled = true;
                  this.button_parar_envio.Focus();
label_37:;
                }
                if (this.cancelar_envio)
                  break;
              }
              this.richTextBox1.Text += Encoding.ASCII.GetString(numArray1);
              this.richTextBox1.SelectionStart += this.richTextBox1.Text.Length;
              this.richTextBox1.ScrollToCaret();
              if (this.cancelar_envio)
              {
                this.button_enviar_fichero.Focus();
                break;
              }
            }
            else
              break;
          }
        }
        catch (Exception ex)
        {
          int num8 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        finally
        {
          if (this.checkBox_dnc.Checked)
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dncend2(this.hndl, (short) -1);
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dncend2", this.ret_focas);
          }
          else
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnend3(this.hndl);
            Cursor.Current = Cursors.Default;
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dwnend3", this.ret_focas);
          }
          this.label1.Text = Resource_Form_enviar_ethernet.String6 + this.progressBar1.Value.ToString();
          this.label1.Update();
          fileStream.Close();
        }
        this.button_seleccionar_fichero.Enabled = true;
        this.button_enviar_texto.Enabled = true;
        this.button_enviar_fichero.Enabled = true;
        this.button_parar_envio.Enabled = false;
        this.button_continuar_envio.Enabled = false;
        this.button_cancelar_envio.Enabled = false;
        this.button_editar.Enabled = true;
      }
    }

    private void button_parar_envio_Click(object sender, EventArgs e) => this.parar_envio = true;

    private void button_continuar_envio_Click(object sender, EventArgs e)
    {
      this.parar_envio = false;
    }

    private void button_cancelar_envio_Click(object sender, EventArgs e)
    {
      this.cancelar_envio = true;
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
        int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String9 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void button_salvar_Click(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      this.saveFileDialog1.Title = Resource_Form_enviar_ethernet.String10;
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
        int num2 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String11, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

    private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Paste();
    }

    private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.SelectedText = "";
    }

    private void seleccionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Focus();
      this.richTextBox1.SelectionStart = 0;
      this.richTextBox1.SelectionLength = this.richTextBox1.Text.Length;
      this.richTextBox1.Copy();
    }

    private void pasarALetrasMayusculasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.SelectedText = this.richTextBox1.SelectedText.ToUpper();
    }

    private void button_reset_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resource_Form_enviar_ethernet.String12, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      int num = 0;
      while (true)
      {
        bool flag = false;
        switch (this.focas_reset_16i())
        {
          case -1:
            goto label_6;
          case 0:
            flag = true;
            break;
        }
        if ((num < 5 || !flag) && (num < 10 || flag))
          ++num;
        else
          goto label_5;
      }
label_6:
      return;
label_5:;
    }

    private void button_start_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resource_Form_enviar_ethernet.String13, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      int num = 0;
      while (true)
      {
        bool flag = false;
        switch (this.focas_start_16i())
        {
          case -1:
            goto label_6;
          case 0:
            flag = true;
            break;
        }
        if ((num < 5 || !flag) && (num < 10 || flag))
          ++num;
        else
          goto label_5;
      }
label_6:
      return;
label_5:;
    }

    private void button_stop_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(Resource_Form_enviar_ethernet.String14, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      int num = 0;
      while (true)
      {
        bool flag = false;
        switch (this.focas_stop_16i())
        {
          case -1:
            goto label_6;
          case 0:
            flag = true;
            break;
        }
        if ((num < 5 || !flag) && (num < 10 || flag))
          ++num;
        else
          goto label_5;
      }
label_6:
      return;
label_5:;
    }

    private int focas_reset_16i()
    {
      Class_configurar_ethernet configurarEthernet = new Class_configurar_ethernet();
      if (this.hndl == (ushort) 0)
      {
        int num = configurarEthernet.obtener_handle();
        if (num == -1)
          return -1;
        this.hndl = (ushort) num;
      }
      Focas1.IODBPMC0 iodbpmC0 = new Focas1.IODBPMC0();
      short a = 0;
      short b = 0;
      ushort c = 8;
      ushort d = 8;
      ushort num1 = 9;
      int num2 = (int) Focas1.pmc_rdpmcrng(this.hndl, a, b, c, d, num1, iodbpmC0);
      iodbpmC0.cdata[0] |= (byte) 64;
      int num3 = (int) Focas1.pmc_wrpmcrng(this.hndl, num1, iodbpmC0);
      int num4 = (int) Focas1.pmc_rdpmcrng(this.hndl, a, b, c, d, num1, iodbpmC0);
      return ((int) Convert.ToInt16(iodbpmC0.cdata[0]) & 64) == 64 ? 0 : 1;
    }

    private int focas_start_16i()
    {
      Class_configurar_ethernet configurarEthernet = new Class_configurar_ethernet();
      if (this.hndl == (ushort) 0)
      {
        int num = configurarEthernet.obtener_handle();
        if (num == -1)
          return -1;
        this.hndl = (ushort) num;
      }
      Focas1.IODBPMC0 iodbpmC0 = new Focas1.IODBPMC0();
      short a = 0;
      short b = 0;
      ushort c = 7;
      ushort d = 7;
      ushort num1 = 9;
      int num2 = (int) Focas1.pmc_rdpmcrng(this.hndl, a, b, c, d, num1, iodbpmC0);
      iodbpmC0.cdata[0] |= (byte) 4;
      int num3 = (int) Focas1.pmc_wrpmcrng(this.hndl, num1, iodbpmC0);
      int num4 = (int) Focas1.pmc_rdpmcrng(this.hndl, a, b, c, d, num1, iodbpmC0);
      return ((int) Convert.ToInt16(iodbpmC0.cdata[0]) & 4) == 4 ? 0 : 1;
    }

    private int focas_stop_16i()
    {
      Class_configurar_ethernet configurarEthernet = new Class_configurar_ethernet();
      if (this.hndl == (ushort) 0)
      {
        int num = configurarEthernet.obtener_handle();
        if (num == -1)
          return -1;
        this.hndl = (ushort) num;
      }
      Focas1.IODBPMC0 iodbpmC0 = new Focas1.IODBPMC0();
      short a = 0;
      short b = 0;
      ushort c = 8;
      ushort d = 8;
      ushort num1 = 9;
      int num2 = (int) Focas1.pmc_rdpmcrng(this.hndl, a, b, c, d, num1, iodbpmC0);
      iodbpmC0.cdata[0] &= (byte) 223;
      int num3 = (int) Focas1.pmc_wrpmcrng(this.hndl, num1, iodbpmC0);
      int num4 = (int) Focas1.pmc_rdpmcrng(this.hndl, a, b, c, d, num1, iodbpmC0);
      return ((int) Convert.ToInt16(iodbpmC0.cdata[0]) & 32) == 0 ? 0 : 1;
    }

    private void checkBox_dnc_CheckedChanged(object sender, EventArgs e)
    {
      if (this.comboBox_data_select.SelectedIndex != 0 && this.checkBox_dnc.Checked)
      {
        this.checkBox_dnc.Checked = false;
        int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String16);
      }
      else if (this.checkBox_dnc.Checked)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String15);
        this.button_stop.Enabled = true;
      }
      else
        this.button_stop.Enabled = false;
    }

    private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Undo();
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void listadoDeProgramasEnCNCToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_listado_programas().ShowDialog();
      this.Enabled = true;
    }

    private void guardarTextoEnPantallaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.richTextBox1.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
    }

    private void button_visualizar_CNC_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_listado_programas().ShowDialog();
      this.Enabled = true;
    }

    private void configuracionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num1 = (int) new Form_config().ShowDialog();
      this.Enabled = true;
      this.hndl = (ushort) 0;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String17);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "ETHERNET")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String18);
          this.Close();
        }
        this.Form_enviar_ethernet_Load((object) null, (EventArgs) null);
      }
    }

    private void comboBox_data_select_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.comboBox_data_select.SelectedIndex != 0)
        this.checkBox_dnc.Checked = false;
      this.textBox_fichero.Focus();
    }

    private void Form_enviar_ethernet_Load(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0)
      {
        if (informacion.leer_contador_uso(true) == -1)
          this.Close();
        if (informacion.leer_fecha_uso() == -1)
          this.Close();
      }
      this.comboBox_data_select.SelectedIndex = 0;
      Class_configurar_ethernet configurarEthernet = new Class_configurar_ethernet();
      if (this.hndl == (ushort) 0)
      {
        int num = configurarEthernet.obtener_handle();
        if (num == -1)
          return;
        this.hndl = (ushort) num;
      }
      if (this.selected_path != 0)
        return;
      this.comboBox_canal.Items.Clear();
      short a;
      short b;
      int num1 = (int) Focas1.cnc_getpath(this.hndl, out a, out b);
      for (int index = 1; index <= (int) b; ++index)
        this.comboBox_canal.Items.Add((object) index.ToString());
      this.comboBox_canal.SelectedIndex = (int) a - 1;
      this.selected_path = (int) a;
    }

    private void comboBox_canal_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selected_path = this.comboBox_canal.SelectedIndex + 1;
      if (Focas1.cnc_setpath(this.hndl, (short) this.selected_path) == (short) 0)
        return;
      int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String19);
    }

    private void label4_Click(object sender, EventArgs e)
    {
    }

    private void explorador_toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_explorer().ShowDialog();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_enviar_ethernet));
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
      this.groupBox1 = new GroupBox();
      this.button_stop = new Button();
      this.checkBox_dnc = new CheckBox();
      this.button_reset = new Button();
      this.button_enviar_desde_cursor = new Button();
      this.button_salvar = new Button();
      this.button_buscar = new Button();
      this.button_enviar_texto = new Button();
      this.groupBox2 = new GroupBox();
      this.label2 = new Label();
      this.groupBox3 = new GroupBox();
      this.label3 = new Label();
      this.button_visualizar_CNC = new Button();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.listadoDeProgramasEnCNCToolStripMenuItem = new ToolStripMenuItem();
      this.guardarTextoEnPantallaToolStripMenuItem = new ToolStripMenuItem();
      this.configuracionToolStripMenuItem = new ToolStripMenuItem();
      this.explorador_toolStripMenuItem1 = new ToolStripMenuItem();
      this.comboBox_data_select = new ComboBox();
      this.groupBox_datos_envio = new GroupBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.comboBox_canal = new ComboBox();
      this.contextMenuStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.groupBox_datos_envio.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.textBox2, "textBox2");
      this.textBox2.Name = "textBox2";
      this.textBox2.TextChanged += new EventHandler(this.textBox2_TextChanged);
      componentResourceManager.ApplyResources((object) this.button_seleccionar_fichero, "button_seleccionar_fichero");
      this.button_seleccionar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_seleccionar_fichero.Name = "button_seleccionar_fichero";
      this.button_seleccionar_fichero.UseVisualStyleBackColor = false;
      this.button_seleccionar_fichero.Click += new EventHandler(this.button_seleccionar_fichero_Click);
      this.openFileDialog1.FileName = "openFileDialog1";
      componentResourceManager.ApplyResources((object) this.openFileDialog1, "openFileDialog1");
      componentResourceManager.ApplyResources((object) this.textBox_fichero, "textBox_fichero");
      this.textBox_fichero.Name = "textBox_fichero";
      componentResourceManager.ApplyResources((object) this.button_enviar_fichero, "button_enviar_fichero");
      this.button_enviar_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_enviar_fichero.Name = "button_enviar_fichero";
      this.button_enviar_fichero.UseVisualStyleBackColor = false;
      this.button_enviar_fichero.Click += new EventHandler(this.button_enviar_fichero_Click);
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
      componentResourceManager.ApplyResources((object) this.button_editar, "button_editar");
      this.button_editar.AccessibleRole = AccessibleRole.None;
      this.button_editar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_editar.Name = "button_editar";
      this.button_editar.UseVisualStyleBackColor = false;
      this.button_editar.Click += new EventHandler(this.button_editar_Click);
      componentResourceManager.ApplyResources((object) this.richTextBox1, "richTextBox1");
      this.richTextBox1.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.TextChanged += new EventHandler(this.richTextBox1_TextChanged);
      componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
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
      componentResourceManager.ApplyResources((object) this.copiarToolStripMenuItem, "copiarToolStripMenuItem");
      this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
      this.copiarToolStripMenuItem.Click += new EventHandler(this.copiarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.pegarToolStripMenuItem, "pegarToolStripMenuItem");
      this.pegarToolStripMenuItem.Name = "pegarToolStripMenuItem";
      this.pegarToolStripMenuItem.Click += new EventHandler(this.pegarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.borrarToolStripMenuItem, "borrarToolStripMenuItem");
      this.borrarToolStripMenuItem.Name = "borrarToolStripMenuItem";
      this.borrarToolStripMenuItem.Click += new EventHandler(this.borrarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.seleccionarTodoToolStripMenuItem, "seleccionarTodoToolStripMenuItem");
      this.seleccionarTodoToolStripMenuItem.Name = "seleccionarTodoToolStripMenuItem";
      this.seleccionarTodoToolStripMenuItem.Click += new EventHandler(this.seleccionarTodoToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.pasarALetrasMayusculasToolStripMenuItem, "pasarALetrasMayusculasToolStripMenuItem");
      this.pasarALetrasMayusculasToolStripMenuItem.Name = "pasarALetrasMayusculasToolStripMenuItem";
      this.pasarALetrasMayusculasToolStripMenuItem.Click += new EventHandler(this.pasarALetrasMayusculasToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.deshacerToolStripMenuItem, "deshacerToolStripMenuItem");
      this.deshacerToolStripMenuItem.Name = "deshacerToolStripMenuItem";
      this.deshacerToolStripMenuItem.Click += new EventHandler(this.deshacerToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.saveFileDialog1, "saveFileDialog1");
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.button_stop);
      this.groupBox1.Controls.Add((Control) this.checkBox_dnc);
      this.groupBox1.Controls.Add((Control) this.button_parar_envio);
      this.groupBox1.Controls.Add((Control) this.button_continuar_envio);
      this.groupBox1.Controls.Add((Control) this.button_cancelar_envio);
      this.groupBox1.Controls.Add((Control) this.progressBar1);
      this.groupBox1.Controls.Add((Control) this.button_reset);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.button_stop, "button_stop");
      this.button_stop.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_stop.Name = "button_stop";
      this.button_stop.UseVisualStyleBackColor = false;
      this.button_stop.Click += new EventHandler(this.button_stop_Click);
      componentResourceManager.ApplyResources((object) this.checkBox_dnc, "checkBox_dnc");
      this.checkBox_dnc.ForeColor = System.Drawing.Color.Red;
      this.checkBox_dnc.Name = "checkBox_dnc";
      this.checkBox_dnc.UseVisualStyleBackColor = true;
      this.checkBox_dnc.CheckedChanged += new EventHandler(this.checkBox_dnc_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.button_reset, "button_reset");
      this.button_reset.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_reset.Name = "button_reset";
      this.button_reset.UseVisualStyleBackColor = false;
      this.button_reset.Click += new EventHandler(this.button_reset_Click);
      componentResourceManager.ApplyResources((object) this.button_enviar_desde_cursor, "button_enviar_desde_cursor");
      this.button_enviar_desde_cursor.AccessibleRole = AccessibleRole.None;
      this.button_enviar_desde_cursor.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_enviar_desde_cursor.Name = "button_enviar_desde_cursor";
      this.button_enviar_desde_cursor.UseVisualStyleBackColor = false;
      this.button_enviar_desde_cursor.Click += new EventHandler(this.button_enviar_desde_cursor_Click);
      componentResourceManager.ApplyResources((object) this.button_salvar, "button_salvar");
      this.button_salvar.AccessibleRole = AccessibleRole.None;
      this.button_salvar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_salvar.Name = "button_salvar";
      this.button_salvar.UseVisualStyleBackColor = false;
      this.button_salvar.Click += new EventHandler(this.button_salvar_Click);
      componentResourceManager.ApplyResources((object) this.button_buscar, "button_buscar");
      this.button_buscar.AccessibleRole = AccessibleRole.None;
      this.button_buscar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_buscar.Name = "button_buscar";
      this.button_buscar.UseVisualStyleBackColor = false;
      this.button_buscar.Click += new EventHandler(this.button_buscar_Click);
      componentResourceManager.ApplyResources((object) this.button_enviar_texto, "button_enviar_texto");
      this.button_enviar_texto.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_enviar_texto.Name = "button_enviar_texto";
      this.button_enviar_texto.UseVisualStyleBackColor = false;
      this.button_enviar_texto.Click += new EventHandler(this.button_enviar_texto_Click);
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.button_enviar_texto);
      this.groupBox2.Controls.Add((Control) this.button_buscar);
      this.groupBox2.Controls.Add((Control) this.button_salvar);
      this.groupBox2.Controls.Add((Control) this.button_enviar_desde_cursor);
      this.groupBox2.Controls.Add((Control) this.textBox2);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.groupBox3, "groupBox3");
      this.groupBox3.Controls.Add((Control) this.label3);
      this.groupBox3.Controls.Add((Control) this.button_visualizar_CNC);
      this.groupBox3.Controls.Add((Control) this.button_seleccionar_fichero);
      this.groupBox3.Controls.Add((Control) this.button_enviar_fichero);
      this.groupBox3.Controls.Add((Control) this.button_editar);
      this.groupBox3.Controls.Add((Control) this.textBox_fichero);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.button_visualizar_CNC, "button_visualizar_CNC");
      this.button_visualizar_CNC.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_visualizar_CNC.Name = "button_visualizar_CNC";
      this.button_visualizar_CNC.UseVisualStyleBackColor = false;
      this.button_visualizar_CNC.Click += new EventHandler(this.button_visualizar_CNC_Click);
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configuracionToolStripMenuItem,
        (ToolStripItem) this.explorador_toolStripMenuItem1
      });
      this.menuStrip1.Name = "menuStrip1";
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.salirToolStripMenuItem,
        (ToolStripItem) this.listadoDeProgramasEnCNCToolStripMenuItem,
        (ToolStripItem) this.guardarTextoEnPantallaToolStripMenuItem
      });
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.listadoDeProgramasEnCNCToolStripMenuItem, "listadoDeProgramasEnCNCToolStripMenuItem");
      this.listadoDeProgramasEnCNCToolStripMenuItem.Name = "listadoDeProgramasEnCNCToolStripMenuItem";
      this.listadoDeProgramasEnCNCToolStripMenuItem.Click += new EventHandler(this.listadoDeProgramasEnCNCToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.guardarTextoEnPantallaToolStripMenuItem, "guardarTextoEnPantallaToolStripMenuItem");
      this.guardarTextoEnPantallaToolStripMenuItem.Name = "guardarTextoEnPantallaToolStripMenuItem";
      this.guardarTextoEnPantallaToolStripMenuItem.Click += new EventHandler(this.guardarTextoEnPantallaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.configuracionToolStripMenuItem, "configuracionToolStripMenuItem");
      this.configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
      this.configuracionToolStripMenuItem.Click += new EventHandler(this.configuracionToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.explorador_toolStripMenuItem1, "explorador_toolStripMenuItem1");
      this.explorador_toolStripMenuItem1.Image = (Image) Resources.DIR10A;
      this.explorador_toolStripMenuItem1.Name = "explorador_toolStripMenuItem1";
      this.explorador_toolStripMenuItem1.Click += new EventHandler(this.explorador_toolStripMenuItem1_Click);
      componentResourceManager.ApplyResources((object) this.comboBox_data_select, "comboBox_data_select");
      this.comboBox_data_select.BackColor = System.Drawing.Color.White;
      this.comboBox_data_select.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_data_select.FormattingEnabled = true;
      this.comboBox_data_select.Items.AddRange(new object[6]
      {
        (object) componentResourceManager.GetString("comboBox_data_select.Items"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items1"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items2"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items3"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items4"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items5")
      });
      this.comboBox_data_select.Name = "comboBox_data_select";
      this.comboBox_data_select.SelectedIndexChanged += new EventHandler(this.comboBox_data_select_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.groupBox_datos_envio, "groupBox_datos_envio");
      this.groupBox_datos_envio.Controls.Add((Control) this.label5);
      this.groupBox_datos_envio.Controls.Add((Control) this.label4);
      this.groupBox_datos_envio.Controls.Add((Control) this.comboBox_canal);
      this.groupBox_datos_envio.Controls.Add((Control) this.comboBox_data_select);
      this.groupBox_datos_envio.Name = "groupBox_datos_envio";
      this.groupBox_datos_envio.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      this.label4.Click += new EventHandler(this.label4_Click);
      componentResourceManager.ApplyResources((object) this.comboBox_canal, "comboBox_canal");
      this.comboBox_canal.BackColor = System.Drawing.Color.White;
      this.comboBox_canal.FormattingEnabled = true;
      this.comboBox_canal.Name = "comboBox_canal";
      this.comboBox_canal.SelectedIndexChanged += new EventHandler(this.comboBox_canal_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox_datos_envio);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.richTextBox1);
      this.Controls.Add((Control) this.groupBox3);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_enviar_ethernet);
      this.Tag = (object) "  ";
      this.FormClosed += new FormClosedEventHandler(this.Form_enviar_FormClosed);
      this.Load += new EventHandler(this.Form_enviar_ethernet_Load);
      this.contextMenuStrip1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBox_datos_envio.ResumeLayout(false);
      this.groupBox_datos_envio.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
