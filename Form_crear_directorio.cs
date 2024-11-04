// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_crear_directorio
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
  public class Form_crear_directorio : Form
  {
    private string _nuevo_directorio = "";
    private bool _boton_aceptar = false;
    private IContainer components = (IContainer) null;
    private Label label_nuevo_directorio;
    private TextBox textBox_nuevo_directorio;
    private Button button_aceptar;
    private Button button_cancelar;

    public Form_crear_directorio() => this.InitializeComponent();

    public string Nuevo_directorio => this._nuevo_directorio;

    public bool Boton_aceptar => this._boton_aceptar;

    private void button_cancelar_Click(object sender, EventArgs e) => this.Close();

    private void button_aceptar_Click(object sender, EventArgs e)
    {
      try
      {
        if (!new Regex("\\w+").Match(this.textBox_nuevo_directorio.Text).Success)
        {
          int num = (int) MessageBox.Show(Resource_Form_nuevo_directorio.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this._boton_aceptar = true;
          this._nuevo_directorio = this.textBox_nuevo_directorio.Text;
          this.Close();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_nuevo_directorio.String2 + ex.Message);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_crear_directorio));
      this.label_nuevo_directorio = new Label();
      this.textBox_nuevo_directorio = new TextBox();
      this.button_aceptar = new Button();
      this.button_cancelar = new Button();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label_nuevo_directorio, "label_nuevo_directorio");
      this.label_nuevo_directorio.Name = "label_nuevo_directorio";
      componentResourceManager.ApplyResources((object) this.textBox_nuevo_directorio, "textBox_nuevo_directorio");
      this.textBox_nuevo_directorio.CharacterCasing = CharacterCasing.Upper;
      this.textBox_nuevo_directorio.Name = "textBox_nuevo_directorio";
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
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.button_cancelar);
      this.Controls.Add((Control) this.button_aceptar);
      this.Controls.Add((Control) this.textBox_nuevo_directorio);
      this.Controls.Add((Control) this.label_nuevo_directorio);
      this.Name = nameof (Form_crear_directorio);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
