// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_editar_usuario
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Servidor_CNC.FTP_Servidor;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_editar_usuario : Form
  {
    private string _numero_maquina = "";
    private string _nombre_usuario = "";
    private string _clave_usuario = "";
    private string _directorio_trabajo = "";
    private bool _aceptado = false;
    private string _puerto_FTP = "";
    private string _auto_start = "";
    private IContainer components = (IContainer) null;
    private Label label_nombre;
    private Label label_clave;
    private TextBox textBox_nombre;
    private TextBox textBox_clave;
    private Button button_aceptar;
    private Button button_cancelar;
    private GroupBox groupBox2;
    private Label label_directorio_seleccionado;
    private TextBox textBox_directorio;
    private Button button_visualizar_ficheros;
    private Button button_seleccionar_directorio;
    private Label label1;
    private TextBox textBox_numero;
    private TextBox textBox_Port_FTP;
    private Label label2;
    private Label label3;
    private CheckBox checkBox1;
    private Button button_help;

    public Form_editar_usuario() => this.InitializeComponent();

    public string Numero_maquina
    {
      set => this._numero_maquina = value;
    }

    public string Nombre_usuario
    {
      set => this._nombre_usuario = value;
      get => this._nombre_usuario;
    }

    public string Clave_usuario
    {
      set => this._clave_usuario = value;
      get => this._clave_usuario;
    }

    public string Directorio_trabajo
    {
      set => this._directorio_trabajo = value;
      get => this._directorio_trabajo;
    }

    public bool Aceptado => this._aceptado;

    public string Puerto_FTP
    {
      set => this._puerto_FTP = value;
      get => this._puerto_FTP;
    }

    public string Auto_start
    {
      set => this._auto_start = value;
      get => this._auto_start;
    }

    private void button_aceptar_Click(object sender, EventArgs e)
    {
      this._nombre_usuario = Regex.Replace(this.textBox_nombre.Text, "\\s+", "");
      this._clave_usuario = Regex.Replace(this.textBox_clave.Text, "\\s+", "");
      this._directorio_trabajo = Regex.Replace(this.textBox_directorio.Text, "\\s+", "");
      this._puerto_FTP = Regex.Match(this.textBox_Port_FTP.Text, "\\d+").Value;
      if (this.checkBox1.Checked)
      {
        if (this._puerto_FTP != "" && this._directorio_trabajo != "")
        {
          this._auto_start = "ON";
        }
        else
        {
          int num = (int) MessageBox.Show(Resource_Form_editar_usuario.String4);
          this._auto_start = "";
          return;
        }
      }
      else
        this._auto_start = "";
      this._aceptado = true;
      this.Close();
    }

    private void button_cancelar_Click(object sender, EventArgs e) => this.Close();

    private void Form_editar_usuario_Load(object sender, EventArgs e)
    {
      this.textBox_numero.Text = this._numero_maquina;
      this.textBox_nombre.Text = this._nombre_usuario;
      this.textBox_clave.Text = this._clave_usuario;
      this.textBox_directorio.Text = this._directorio_trabajo;
      this.textBox_Port_FTP.Text = this._puerto_FTP;
      if (!(this._auto_start == "ON"))
        return;
      this.checkBox1.Checked = true;
    }

    private void button_seleccionar_directorio_Click(object sender, EventArgs e)
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

    private void button_seleccionar_fichero_Click(object sender, EventArgs e)
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
      if (MessageBox.Show(Resource_Form_editar_usuario.String3 + str + "   ", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        this.textBox_directorio.Text = str + "\\";
        this.textBox_directorio.SelectionStart = this.textBox_directorio.Text.Length;
        this.textBox_directorio.Focus();
      }
    }

    private void button_help_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show(Resource_Form_editar_usuario.String5);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_editar_usuario));
      this.label_nombre = new Label();
      this.label_clave = new Label();
      this.textBox_nombre = new TextBox();
      this.textBox_clave = new TextBox();
      this.button_aceptar = new Button();
      this.button_cancelar = new Button();
      this.groupBox2 = new GroupBox();
      this.label_directorio_seleccionado = new Label();
      this.textBox_directorio = new TextBox();
      this.button_visualizar_ficheros = new Button();
      this.button_seleccionar_directorio = new Button();
      this.label1 = new Label();
      this.textBox_numero = new TextBox();
      this.textBox_Port_FTP = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.checkBox1 = new CheckBox();
      this.button_help = new Button();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label_nombre, "label_nombre");
      this.label_nombre.Name = "label_nombre";
      componentResourceManager.ApplyResources((object) this.label_clave, "label_clave");
      this.label_clave.Name = "label_clave";
      componentResourceManager.ApplyResources((object) this.textBox_nombre, "textBox_nombre");
      this.textBox_nombre.CharacterCasing = CharacterCasing.Upper;
      this.textBox_nombre.Name = "textBox_nombre";
      componentResourceManager.ApplyResources((object) this.textBox_clave, "textBox_clave");
      this.textBox_clave.CharacterCasing = CharacterCasing.Upper;
      this.textBox_clave.Name = "textBox_clave";
      componentResourceManager.ApplyResources((object) this.button_aceptar, "button_aceptar");
      this.button_aceptar.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_aceptar.Name = "button_aceptar";
      this.button_aceptar.UseVisualStyleBackColor = false;
      this.button_aceptar.Click += new EventHandler(this.button_aceptar_Click);
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.BackColor = Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button_cancelar_Click);
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.label_directorio_seleccionado);
      this.groupBox2.Controls.Add((Control) this.textBox_directorio);
      this.groupBox2.Controls.Add((Control) this.button_visualizar_ficheros);
      this.groupBox2.Controls.Add((Control) this.button_seleccionar_directorio);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label_directorio_seleccionado, "label_directorio_seleccionado");
      this.label_directorio_seleccionado.Name = "label_directorio_seleccionado";
      componentResourceManager.ApplyResources((object) this.textBox_directorio, "textBox_directorio");
      this.textBox_directorio.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.textBox_directorio.Name = "textBox_directorio";
      this.textBox_directorio.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.button_visualizar_ficheros, "button_visualizar_ficheros");
      this.button_visualizar_ficheros.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_visualizar_ficheros.Name = "button_visualizar_ficheros";
      this.button_visualizar_ficheros.UseVisualStyleBackColor = false;
      this.button_visualizar_ficheros.Click += new EventHandler(this.button_seleccionar_fichero_Click);
      componentResourceManager.ApplyResources((object) this.button_seleccionar_directorio, "button_seleccionar_directorio");
      this.button_seleccionar_directorio.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_seleccionar_directorio.Name = "button_seleccionar_directorio";
      this.button_seleccionar_directorio.UseVisualStyleBackColor = false;
      this.button_seleccionar_directorio.Click += new EventHandler(this.button_seleccionar_directorio_Click);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.textBox_numero, "textBox_numero");
      this.textBox_numero.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.textBox_numero.CharacterCasing = CharacterCasing.Upper;
      this.textBox_numero.Name = "textBox_numero";
      this.textBox_numero.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBox_Port_FTP, "textBox_Port_FTP");
      this.textBox_Port_FTP.CharacterCasing = CharacterCasing.Upper;
      this.textBox_Port_FTP.Name = "textBox_Port_FTP";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.ForeColor = Color.Red;
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.checkBox1, "checkBox1");
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.button_help, "button_help");
      this.button_help.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.button_help.ForeColor = Color.Red;
      this.button_help.Name = "button_help";
      this.button_help.UseVisualStyleBackColor = false;
      this.button_help.Click += new EventHandler(this.button_help_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.button_help);
      this.Controls.Add((Control) this.checkBox1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textBox_Port_FTP);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textBox_numero);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button_cancelar);
      this.Controls.Add((Control) this.button_aceptar);
      this.Controls.Add((Control) this.textBox_clave);
      this.Controls.Add((Control) this.textBox_nombre);
      this.Controls.Add((Control) this.label_clave);
      this.Controls.Add((Control) this.label_nombre);
      this.Controls.Add((Control) this.groupBox2);
      this.Name = nameof (Form_editar_usuario);
      this.Load += new EventHandler(this.Form_editar_usuario_Load);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
