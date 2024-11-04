// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_recibir_ethernet
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Properties;
using FANUC_Open_Com.Recibir;
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
  public class Form_recibir_ethernet : Form
  {
    private bool cancelar = false;
    private ushort hndl = 0;
    private Focas1.focas_ret ret;
    private int selected_path = 0;
    private IContainer components = (IContainer) null;
    private TextBox textBox_fichero;
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
    private TextBox textBox_numero_programa_start;
    private Label label2;
    private SaveFileDialog saveFileDialog1;
    private Button button_programas_cnc;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private ToolStripMenuItem listadoProgramasCNCToolStripMenuItem;
    private ToolStripMenuItem guardarTextoEnPantallaToolStripMenuItem;
    private Label label4;
    private GroupBox groupBox_seleccion_datos;
    private ComboBox comboBox_data_select;
    private GroupBox groupBox_seleccion_programa;
    private ToolStripMenuItem configurarToolStripMenuItem;
    private Label label5;
    private ComboBox comboBox_canal;
    private Label label3;
    private Label label6;
    private Label label7;
    private TextBox textBox_numero_programa_end;
    private ToolStripMenuItem exploradorToolStripMenuItem1;

    public Form_recibir_ethernet() => this.InitializeComponent();

    private void Form_recibir_FormClosed(object sender, FormClosedEventArgs e)
    {
      int num = (int) Focas1.cnc_freelibhndl(this.hndl);
      this.Close();
    }

    private void button_salir_Click(object sender, EventArgs e) => this.Close();

    private void button_seleccionar_fichero_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.textBox_fichero.Text = openFileDialog.FileName;
      this.button_recibir_fichero.Enabled = true;
    }

    private void button_recibir_fichero_Click(object sender, EventArgs e)
    {
      if (this.textBox_fichero.Text == "")
      {
        int num1 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (File.Exists(this.textBox_fichero.Text) && MessageBox.Show(Resource_Form_recibir_ethernet.String2, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
          return;
        FileStream fileStream;
        try
        {
          fileStream = new FileStream(this.textBox_fichero.Text, FileMode.Create, FileAccess.Write, FileShare.None);
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String3 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        string str1 = "Oo";
        int b = 0;
        int c = 0;
        short a1;
        switch (this.comboBox_data_select.SelectedIndex)
        {
          case 0:
            a1 = (short) 0;
            try
            {
              if (this.textBox_numero_programa_start.Text == "")
              {
                int num3 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String4);
                fileStream.Close();
                return;
              }
              string str2 = this.textBox_numero_programa_start.Text;
              if ((int) str2[0] == (int) str1[0] || (int) str2[0] == (int) str1[1])
                str2 = this.textBox_numero_programa_start.Text.Substring(1);
              if (this.textBox_numero_programa_end.Text == "")
                this.textBox_numero_programa_end.Text = this.textBox_numero_programa_start.Text;
              string str3 = this.textBox_numero_programa_end.Text;
              if ((int) str3[0] == (int) str1[0] || (int) str3[0] == (int) str1[1])
                str3 = this.textBox_numero_programa_end.Text.Substring(1);
              b = Convert.ToInt32(str2);
              c = Convert.ToInt32(str3);
              break;
            }
            catch (Exception ex)
            {
              int num4 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              fileStream.Close();
              return;
            }
          case 1:
            a1 = (short) 1;
            break;
          case 2:
            a1 = (short) 5;
            break;
          case 3:
            a1 = (short) 4;
            break;
          case 4:
            a1 = (short) 2;
            break;
          case 5:
            a1 = (short) 3;
            break;
          case 6:
            a1 = (short) 8;
            break;
          case 7:
            a1 = (short) 9;
            break;
          case 8:
            a1 = (short) 7;
            break;
          default:
            int num5 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String6);
            fileStream.Close();
            return;
        }
        Class_chequeo_errores_focas chequeoErroresFocas = new Class_chequeo_errores_focas();
        Cursor.Current = Cursors.WaitCursor;
        this.ret = (Focas1.focas_ret) Focas1.cnc_upstart3(this.hndl, a1, b, c);
        if (chequeoErroresFocas.chequeo_errores_focas(this.hndl, "cnc_upstart3", this.ret) != 0)
        {
          fileStream.Close();
        }
        else
        {
          int length = 1024;
          byte[] numArray1 = new byte[length];
          long num6 = 0;
          byte[] numArray2 = new byte[10240 + length];
          try
          {
            int count1 = 0;
            this.cancelar = false;
            this.progressBar1.Maximum = 10240;
            this.progressBar1.Value = 0;
            this.label1.Text = Resource_Form_recibir_ethernet.String7;
            this.richTextBox1.ResetText();
            this.button_cancelar.Enabled = true;
            this.button_recibir_fichero.Enabled = false;
            this.button_seleccionar_fichero.Enabled = false;
            this.button_seleccionar_directorio.Enabled = false;
            this.textBox_fichero.Focus();
            int count2;
            string str4;
            do
            {
              int a2;
              do
              {
                Array.Clear((Array) numArray1, 0, numArray1.Length);
                a2 = length;
                this.ret = (Focas1.focas_ret) Focas1.cnc_upload3(this.hndl, ref a2, (object) numArray1);
                if (chequeoErroresFocas.chequeo_errores_focas(this.hndl, "cnc_upload3", this.ret) != 0)
                  goto label_43;
              }
              while (this.ret == Focas1.focas_ret.EW_BUFFER);
              count2 = a2;
              num6 += (long) count2;
              this.label1.Text = Resource_Form_recibir_ethernet.String8 + num6.ToString();
              count1 += count2;
              Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, count1 - count2, count2);
              if (count1 >= 10240)
              {
                fileStream.Write(numArray2, 0, count1);
                count1 = 0;
              }
              if (this.richTextBox1.Text.Length >= 10240)
                this.richTextBox1.Text.Remove(0);
              this.richTextBox1.Text += Encoding.ASCII.GetString(numArray1);
              this.richTextBox1.SelectionStart += this.richTextBox1.Text.Length;
              this.richTextBox1.ScrollToCaret();
              this.progressBar1.Value = count1;
              Application.DoEvents();
              if (this.cancelar)
              {
                fileStream.Write(numArray2, 0, count1);
                this.cancelar = false;
                return;
              }
              str4 = "%";
            }
            while ((int) numArray1[count2 - 1] != (int) (byte) str4[0]);
            goto label_40;
label_43:
            return;
label_40:
            fileStream.Write(numArray2, 0, count1);
          }
          catch (Exception ex)
          {
            int num7 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String9 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          finally
          {
            this.label1.Text = Resource_Form_recibir_ethernet.String10 + num6.ToString();
            fileStream.Close();
            this.ret = (Focas1.focas_ret) Focas1.cnc_upend3(this.hndl);
            Cursor.Current = Cursors.Default;
            this.button_cancelar.Enabled = false;
            this.button_recibir_fichero.Enabled = true;
            this.button_seleccionar_fichero.Enabled = true;
            this.button_seleccionar_directorio.Enabled = true;
            this.button_recibir_fichero.Focus();
            this.progressBar1.Value = 10240;
          }
        }
      }
    }

    private void button_cancelar_Click(object sender, EventArgs e) => this.cancelar = true;

    private void button_seleccionar_fichero_Click_1(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      this.saveFileDialog1.FileName = this.textBox_numero_programa_start.Text;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox_fichero.Text = this.saveFileDialog1.FileName;
      this.button_recibir_fichero.Enabled = true;
      this.textBox_fichero.SelectionStart = this.textBox_fichero.Text.Length;
      this.textBox_fichero.Focus();
      this.button_recibir_fichero_Click((object) null, (EventArgs) null);
    }

    private void button_seleccionar_directorio_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox_fichero.Text = this.folderBrowserDialog1.SelectedPath + "\\";
      this.textBox_fichero.SelectionStart = this.textBox_fichero.Text.Length;
      this.textBox_fichero.Focus();
      int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String11, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void button_programas_cnc_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      Form_listado_programas listadoProgramas = new Form_listado_programas();
      listadoProgramas.Origen_llamada = "recibir";
      listadoProgramas.Numero_programa_start = "";
      listadoProgramas.Numero_programa_end = "";
      listadoProgramas.Canal_seleccionado = this.selected_path;
      int num = (int) listadoProgramas.ShowDialog();
      if (listadoProgramas.Numero_programa_start != "")
      {
        this.textBox_numero_programa_start.Text = listadoProgramas.Numero_programa_start;
        this.textBox_numero_programa_end.Text = listadoProgramas.Numero_programa_end;
      }
      this.selected_path = listadoProgramas.Canal_seleccionado;
      this.comboBox_canal.SelectedIndex = this.selected_path - 1;
      this.Enabled = true;
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void listadoProgramasCNCToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_programas_cnc_Click((object) null, (EventArgs) null);
    }

    private void guardarTextoEnPantallaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveFileDialog1.ValidateNames = true;
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.richTextBox1.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
    }

    private void comboBox_data_select_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.comboBox_data_select.SelectedIndex == 0)
        this.groupBox_seleccion_programa.Visible = true;
      else
        this.groupBox_seleccion_programa.Visible = false;
      this.textBox_fichero.Focus();
    }

    private void Form_recibir_ethernet_Load(object sender, EventArgs e)
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

    private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num1 = (int) new Form_config().ShowDialog();
      this.Enabled = true;
      this.hndl = (ushort) 0;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String12);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "ETHERNET")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String13);
          this.Close();
        }
        this.Form_recibir_ethernet_Load((object) null, (EventArgs) null);
      }
    }

    private void comboBox_canal_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selected_path = this.comboBox_canal.SelectedIndex + 1;
      if (Focas1.cnc_setpath(this.hndl, (short) this.selected_path) == (short) 0)
        return;
      int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String14);
    }

    private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }

    private void exploradorToolStripMenuItem1_Click(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_recibir_ethernet));
      this.textBox_fichero = new TextBox();
      this.button_recibir_fichero = new Button();
      this.progressBar1 = new ProgressBar();
      this.label1 = new Label();
      this.button_cancelar = new Button();
      this.richTextBox1 = new RichTextBox();
      this.button_seleccionar_fichero = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.button_seleccionar_directorio = new Button();
      this.groupBox1 = new GroupBox();
      this.groupBox2 = new GroupBox();
      this.label3 = new Label();
      this.textBox_numero_programa_start = new TextBox();
      this.label2 = new Label();
      this.saveFileDialog1 = new SaveFileDialog();
      this.button_programas_cnc = new Button();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.listadoProgramasCNCToolStripMenuItem = new ToolStripMenuItem();
      this.guardarTextoEnPantallaToolStripMenuItem = new ToolStripMenuItem();
      this.configurarToolStripMenuItem = new ToolStripMenuItem();
      this.exploradorToolStripMenuItem1 = new ToolStripMenuItem();
      this.label4 = new Label();
      this.groupBox_seleccion_datos = new GroupBox();
      this.label5 = new Label();
      this.comboBox_canal = new ComboBox();
      this.comboBox_data_select = new ComboBox();
      this.groupBox_seleccion_programa = new GroupBox();
      this.textBox_numero_programa_end = new TextBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.groupBox_seleccion_datos.SuspendLayout();
      this.groupBox_seleccion_programa.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.textBox_fichero, "textBox_fichero");
      this.textBox_fichero.Name = "textBox_fichero";
      componentResourceManager.ApplyResources((object) this.button_recibir_fichero, "button_recibir_fichero");
      this.button_recibir_fichero.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_recibir_fichero.Name = "button_recibir_fichero";
      this.button_recibir_fichero.UseVisualStyleBackColor = false;
      this.button_recibir_fichero.Click += new EventHandler(this.button_recibir_fichero_Click);
      componentResourceManager.ApplyResources((object) this.progressBar1, "progressBar1");
      this.progressBar1.Name = "progressBar1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
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
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.button_seleccionar_fichero);
      this.groupBox2.Controls.Add((Control) this.button_seleccionar_directorio);
      this.groupBox2.Controls.Add((Control) this.textBox_fichero);
      this.groupBox2.Controls.Add((Control) this.button_recibir_fichero);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.textBox_numero_programa_start, "textBox_numero_programa_start");
      this.textBox_numero_programa_start.Name = "textBox_numero_programa_start";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.saveFileDialog1, "saveFileDialog1");
      componentResourceManager.ApplyResources((object) this.button_programas_cnc, "button_programas_cnc");
      this.button_programas_cnc.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_programas_cnc.Name = "button_programas_cnc";
      this.button_programas_cnc.UseVisualStyleBackColor = false;
      this.button_programas_cnc.Click += new EventHandler(this.button_programas_cnc_Click);
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configurarToolStripMenuItem,
        (ToolStripItem) this.exploradorToolStripMenuItem1
      });
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.salirToolStripMenuItem,
        (ToolStripItem) this.listadoProgramasCNCToolStripMenuItem,
        (ToolStripItem) this.guardarTextoEnPantallaToolStripMenuItem
      });
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.listadoProgramasCNCToolStripMenuItem, "listadoProgramasCNCToolStripMenuItem");
      this.listadoProgramasCNCToolStripMenuItem.Name = "listadoProgramasCNCToolStripMenuItem";
      this.listadoProgramasCNCToolStripMenuItem.Click += new EventHandler(this.listadoProgramasCNCToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.guardarTextoEnPantallaToolStripMenuItem, "guardarTextoEnPantallaToolStripMenuItem");
      this.guardarTextoEnPantallaToolStripMenuItem.Name = "guardarTextoEnPantallaToolStripMenuItem";
      this.guardarTextoEnPantallaToolStripMenuItem.Click += new EventHandler(this.guardarTextoEnPantallaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.configurarToolStripMenuItem, "configurarToolStripMenuItem");
      this.configurarToolStripMenuItem.Name = "configurarToolStripMenuItem";
      this.configurarToolStripMenuItem.Click += new EventHandler(this.configurarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.exploradorToolStripMenuItem1, "exploradorToolStripMenuItem1");
      this.exploradorToolStripMenuItem1.Image = (Image) Resources.DIR10A;
      this.exploradorToolStripMenuItem1.Name = "exploradorToolStripMenuItem1";
      this.exploradorToolStripMenuItem1.Click += new EventHandler(this.exploradorToolStripMenuItem1_Click);
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.groupBox_seleccion_datos, "groupBox_seleccion_datos");
      this.groupBox_seleccion_datos.Controls.Add((Control) this.label5);
      this.groupBox_seleccion_datos.Controls.Add((Control) this.comboBox_canal);
      this.groupBox_seleccion_datos.Controls.Add((Control) this.comboBox_data_select);
      this.groupBox_seleccion_datos.Name = "groupBox_seleccion_datos";
      this.groupBox_seleccion_datos.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.comboBox_canal, "comboBox_canal");
      this.comboBox_canal.BackColor = System.Drawing.Color.White;
      this.comboBox_canal.FormattingEnabled = true;
      this.comboBox_canal.Name = "comboBox_canal";
      this.comboBox_canal.SelectedIndexChanged += new EventHandler(this.comboBox_canal_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.comboBox_data_select, "comboBox_data_select");
      this.comboBox_data_select.BackColor = System.Drawing.Color.White;
      this.comboBox_data_select.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox_data_select.FormattingEnabled = true;
      this.comboBox_data_select.Items.AddRange(new object[9]
      {
        (object) componentResourceManager.GetString("comboBox_data_select.Items"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items1"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items2"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items3"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items4"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items5"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items6"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items7"),
        (object) componentResourceManager.GetString("comboBox_data_select.Items8")
      });
      this.comboBox_data_select.Name = "comboBox_data_select";
      this.comboBox_data_select.SelectedIndexChanged += new EventHandler(this.comboBox_data_select_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.groupBox_seleccion_programa, "groupBox_seleccion_programa");
      this.groupBox_seleccion_programa.Controls.Add((Control) this.textBox_numero_programa_end);
      this.groupBox_seleccion_programa.Controls.Add((Control) this.label7);
      this.groupBox_seleccion_programa.Controls.Add((Control) this.label6);
      this.groupBox_seleccion_programa.Controls.Add((Control) this.button_programas_cnc);
      this.groupBox_seleccion_programa.Controls.Add((Control) this.label2);
      this.groupBox_seleccion_programa.Controls.Add((Control) this.textBox_numero_programa_start);
      this.groupBox_seleccion_programa.Name = "groupBox_seleccion_programa";
      this.groupBox_seleccion_programa.TabStop = false;
      componentResourceManager.ApplyResources((object) this.textBox_numero_programa_end, "textBox_numero_programa_end");
      this.textBox_numero_programa_end.Name = "textBox_numero_programa_end";
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox_seleccion_programa);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.richTextBox1);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.groupBox_seleccion_datos);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_recibir_ethernet);
      this.Tag = (object) " ";
      this.FormClosed += new FormClosedEventHandler(this.Form_recibir_FormClosed);
      this.Load += new EventHandler(this.Form_recibir_ethernet_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBox_seleccion_datos.ResumeLayout(false);
      this.groupBox_seleccion_datos.PerformLayout();
      this.groupBox_seleccion_programa.ResumeLayout(false);
      this.groupBox_seleccion_programa.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
