// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_email
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_email : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label5;
    private TextBox textBox_comentarios;
    private Label label4;
    private Label label6;
    private Label label7;
    private Label label8;

    public Form_email() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_email));
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label5 = new Label();
      this.textBox_comentarios = new TextBox();
      this.label4 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.ForeColor = Color.Green;
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.ForeColor = Color.Green;
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.ForeColor = Color.Green;
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.ForeColor = Color.Green;
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.textBox_comentarios, "textBox_comentarios");
      this.textBox_comentarios.ForeColor = Color.Blue;
      this.textBox_comentarios.Name = "textBox_comentarios";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.ForeColor = Color.Green;
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.label8, "label8");
      this.label8.ForeColor = Color.Green;
      this.label8.Name = "label8";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.textBox_comentarios);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (Form_email);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
