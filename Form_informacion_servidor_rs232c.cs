// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_informacion_servidor_rs232c
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_informacion_servidor_rs232c : Form
  {
    private IContainer components = (IContainer) null;
    private RichTextBox richTextBox1;

    public Form_informacion_servidor_rs232c() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_informacion_servidor_rs232c));
      this.richTextBox1 = new RichTextBox();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.richTextBox1, "richTextBox1");
      this.richTextBox1.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox1.Name = "richTextBox1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.richTextBox1);
      this.Name = nameof (Form_informacion_servidor_rs232c);
      this.ResumeLayout(false);
    }
  }
}
