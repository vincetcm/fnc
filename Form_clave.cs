// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_clave
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Menu_informacion;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_clave : Form
  {
    private IContainer components = (IContainer) null;
    private TextBox textBox_clave;
    private GroupBox groupBox1;
    private Button button_ok;
    private Button button_cancelar;
    private Label label1;

    public Form_clave() => this.InitializeComponent();

    private void button2_Click(object sender, EventArgs e) => Application.Exit();

    private void button1_Click(object sender, EventArgs e)
    {
      string lower = this.textBox_clave.Text.ToLower();
      try
      {
        Informacion informacion = new Informacion();
        long int64 = Convert.ToInt64(lower);
        if (int64 == informacion.Numero_serie * 5L / 3L + 10101960L)
        {
          Registry.CurrentUser.CreateSubKey("Software\\P").SetValue("PW", (object) int64);
          int num = (int) MessageBox.Show(Resource_Form_clave.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.Close();
        }
        else
        {
          if (int64 == informacion.Numero_serie * 3L / 2L + 19601010L)
          {
            RegistryKey subKey1 = Registry.CurrentUser.CreateSubKey("Software\\P");
            RegistryKey subKey2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\GJKE");
            if (subKey1.GetValue("CC1") == null && subKey2.GetValue("ZENBAT1") == null)
            {
              subKey1.SetValue("PW", (object) int64);
              int num1 = -60;
              subKey1.SetValue("CC1", (object) num1);
              subKey2.SetValue("ZENBAT1", (object) num1);
              int num2 = (int) MessageBox.Show(Resource_Form_clave.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.Close();
              return;
            }
            int int16_1 = (int) Convert.ToInt16(subKey1.GetValue("CC1"));
            int int16_2 = (int) Convert.ToInt16(subKey2.GetValue("ZENBAT1"));
            if (int16_1 < -60 || int16_1 >= 0 || int16_1 != int16_2)
            {
              int num = (int) MessageBox.Show(Resource_Form_clave.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              Application.Exit();
            }
          }
          int num3 = (int) MessageBox.Show(Resource_Form_clave.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          Application.Exit();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_clave.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Application.Exit();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_clave));
      this.textBox_clave = new TextBox();
      this.groupBox1 = new GroupBox();
      this.label1 = new Label();
      this.button_cancelar = new Button();
      this.button_ok = new Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.textBox_clave, "textBox_clave");
      this.textBox_clave.Name = "textBox_clave";
      this.textBox_clave.Tag = (object) "gj";
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.textBox_clave);
      this.groupBox1.Controls.Add((Control) this.button_cancelar);
      this.groupBox1.Controls.Add((Control) this.button_ok);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      this.groupBox1.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.button_cancelar, "button_cancelar");
      this.button_cancelar.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      this.button_cancelar.Name = "button_cancelar";
      this.button_cancelar.UseVisualStyleBackColor = false;
      this.button_cancelar.Click += new EventHandler(this.button2_Click);
      componentResourceManager.ApplyResources((object) this.button_ok, "button_ok");
      this.button_ok.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_ok.Name = "button_ok";
      this.button_ok.UseVisualStyleBackColor = false;
      this.button_ok.Click += new EventHandler(this.button1_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Form_clave);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
