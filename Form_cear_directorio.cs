// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_cear_directorio
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_cear_directorio : Form
  {
    private string _nuevo_directorio = "";
    private IContainer components = (IContainer) null;
    private TextBox textBox_nuevo;
    private Button button_create;
    private Button button_cancelar;
    private Label label1;

    public Form_cear_directorio() => this.InitializeComponent();

    public string Nuevo_directorio => this._nuevo_directorio;

    private void button_cancelar_Click(object sender, EventArgs e) => this.Close();

    private void button_crear_Click(object sender, EventArgs e)
    {
      this._nuevo_directorio = this.textBox_nuevo.Text;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_cear_directorio));
      this.textBox_nuevo = new TextBox();
      this.button_create = new Button();
      this.button_cancelar = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.textBox_nuevo, "textBox_nuevo");
      this.textBox_nuevo.CharacterCasing = CharacterCasing.Upper;
      this.textBox_nuevo.Name = "textBox_nuevo";
      componentResourceManager.ApplyResources((object) this.button_create, "button_create");
      this.button_create.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_create.Name = "button_create";
      this.button_create.UseVisualStyleBackColor = false;
      this.button_create.Click += new EventHandler(this.button_crear_Click);
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.BackColor = Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button_cancelar_Click);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button_cancelar);
      this.Controls.Add((Control) this.button_create);
      this.Controls.Add((Control) this.textBox_nuevo);
      this.Name = nameof (Form_cear_directorio);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
