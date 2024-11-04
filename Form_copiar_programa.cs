// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_copiar_programa
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Listado_programas;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_copiar_programa : Form
  {
    private string _programa_original = "";
    private string _nuevo_programa = "";
    private int _tipo_CNC = 0;
    private IContainer components = (IContainer) null;
    private Label label_original;
    private Label label_nuevo;
    private TextBox textBox_original;
    private TextBox textBox_nuevo;
    private Button button_copiar;
    private Button button_cancelar;
    private Label label_directorio;

    public Form_copiar_programa() => this.InitializeComponent();

    public string Programa_original
    {
      set => this._programa_original = value;
      get => this._programa_original;
    }

    public string Nuevo_programa => this._nuevo_programa;

    public int Tipo_CNC
    {
      set => this._tipo_CNC = value;
    }

    private void Form_rename_Load(object sender, EventArgs e)
    {
      this.textBox_original.Text = this._programa_original;
    }

    private void button_cancelar_Click(object sender, EventArgs e) => this.Close();

    private void button_copiar_Click(object sender, EventArgs e)
    {
      if (this._tipo_CNC == 0)
      {
        string str1 = this.textBox_original.Text.Substring(0, 1);
        string str2 = this.textBox_nuevo.Text.Substring(0, 1);
        if (str1 != "O" || str2 != "O")
        {
          int num = (int) MessageBox.Show(Resource_Form_copiar.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        try
        {
          int int32_1 = Convert.ToInt32(this.textBox_nuevo.Text.Substring(1));
          int int32_2 = Convert.ToInt32(this.textBox_nuevo.Text.Substring(1));
          if (int32_1 < 0 || int32_2 < 0)
          {
            int num = (int) MessageBox.Show(Resource_Form_copiar.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          this._nuevo_programa = this.textBox_nuevo.Text;
          this._programa_original = this.textBox_original.Text;
          this.Close();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(Resource_Form_copiar.String3 + " :" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      if (this._tipo_CNC != 1)
        return;
      try
      {
        this._nuevo_programa = this.textBox_nuevo.Text;
        this._programa_original = this.textBox_original.Text;
        this.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_renombrar.String3 + " :" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_copiar_programa));
      this.label_original = new Label();
      this.label_nuevo = new Label();
      this.textBox_original = new TextBox();
      this.textBox_nuevo = new TextBox();
      this.button_copiar = new Button();
      this.button_cancelar = new Button();
      this.label_directorio = new Label();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label_original, "label_original");
      this.label_original.Name = "label_original";
      componentResourceManager.ApplyResources((object) this.label_nuevo, "label_nuevo");
      this.label_nuevo.Name = "label_nuevo";
      componentResourceManager.ApplyResources((object) this.textBox_original, "textBox_original");
      this.textBox_original.CharacterCasing = CharacterCasing.Upper;
      this.textBox_original.Name = "textBox_original";
      componentResourceManager.ApplyResources((object) this.textBox_nuevo, "textBox_nuevo");
      this.textBox_nuevo.CharacterCasing = CharacterCasing.Upper;
      this.textBox_nuevo.Name = "textBox_nuevo";
      componentResourceManager.ApplyResources((object) this.button_copiar, "button_copiar");
      this.button_copiar.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_copiar.Name = "button_copiar";
      this.button_copiar.UseVisualStyleBackColor = false;
      this.button_copiar.Click += new EventHandler(this.button_copiar_Click);
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.BackColor = Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button_cancelar_Click);
      componentResourceManager.ApplyResources((object) this.label_directorio, "label_directorio");
      this.label_directorio.Name = "label_directorio";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.label_directorio);
      this.Controls.Add((Control) this.button_cancelar);
      this.Controls.Add((Control) this.button_copiar);
      this.Controls.Add((Control) this.textBox_nuevo);
      this.Controls.Add((Control) this.textBox_original);
      this.Controls.Add((Control) this.label_nuevo);
      this.Controls.Add((Control) this.label_original);
      this.Name = nameof (Form_copiar_programa);
      this.Load += new EventHandler(this.Form_rename_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
