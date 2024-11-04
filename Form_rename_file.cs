// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_rename_file
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.DATASERVER.FTP_Cliente;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_rename_file : Form
  {
    private string _programa_original = "";
    private string _nuevo_programa = "";
    private bool _boton_aceptar = false;
    private IContainer components = (IContainer) null;
    private Label label_original;
    private Label label_nuevo;
    private TextBox textBox_original;
    private TextBox textBox_nuevo;
    private Button button_cambiar;
    private Button button_cancelar;

    public Form_rename_file() => this.InitializeComponent();

    public string Programa_original
    {
      set => this._programa_original = value;
      get => this._programa_original;
    }

    public string Nuevo_programa => this._nuevo_programa;

    public bool Boton_aceptar => this._boton_aceptar;

    private void button_cambiar_Click(object sender, EventArgs e)
    {
      try
      {
        if (!new Regex("\\w+[.]?\\w?").Match(this.textBox_nuevo.Text).Success)
        {
          int num = (int) MessageBox.Show(Resource_Form_renombrar_fichero.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this._boton_aceptar = true;
          this._nuevo_programa = this.textBox_nuevo.Text;
          this._programa_original = this.textBox_original.Text;
          this.Close();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_renombrar_fichero.String2 + ex.Message);
      }
    }

    private void Form_rename_Load(object sender, EventArgs e)
    {
      this.textBox_original.Text = this._programa_original;
    }

    private void button_cancelar_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_rename_file));
      this.label_original = new Label();
      this.label_nuevo = new Label();
      this.textBox_original = new TextBox();
      this.textBox_nuevo = new TextBox();
      this.button_cambiar = new Button();
      this.button_cancelar = new Button();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label_original, "label_original");
      this.label_original.Name = "label_original";
      componentResourceManager.ApplyResources((object) this.label_nuevo, "label_nuevo");
      this.label_nuevo.Name = "label_nuevo";
      componentResourceManager.ApplyResources((object) this.textBox_original, "textBox_original");
      this.textBox_original.Name = "textBox_original";
      this.textBox_original.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBox_nuevo, "textBox_nuevo");
      this.textBox_nuevo.Name = "textBox_nuevo";
      componentResourceManager.ApplyResources((object) this.button_cambiar, "button_cambiar");
      this.button_cambiar.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_cambiar.Name = "button_cambiar";
      this.button_cambiar.UseVisualStyleBackColor = false;
      this.button_cambiar.Click += new EventHandler(this.button_cambiar_Click);
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.BackColor = Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button_cancelar_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.button_cancelar);
      this.Controls.Add((Control) this.button_cambiar);
      this.Controls.Add((Control) this.textBox_nuevo);
      this.Controls.Add((Control) this.textBox_original);
      this.Controls.Add((Control) this.label_nuevo);
      this.Controls.Add((Control) this.label_original);
      this.Name = nameof (Form_rename_file);
      this.Load += new EventHandler(this.Form_rename_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
