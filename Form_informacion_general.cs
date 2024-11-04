// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_informacion_general
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_informacion_general : Form
  {
    private string _fichero_mostrar = "";
    private string _fichero_mostrado = "";
    private IContainer components = (IContainer) null;
    private RichTextBox richTextBox_informacion_general;

    public Form_informacion_general() => this.InitializeComponent();

    public string Fichero_mostrar
    {
      set => this._fichero_mostrar = value;
      get => this._fichero_mostrado;
    }

    private void Form_informacion_general_Load(object sender, EventArgs e)
    {
      try
      {
        string str = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true).GetValue("Language");
        this._fichero_mostrado = this._fichero_mostrar + "_en.txt";
        if (str == "SPANISH")
          this._fichero_mostrado = this._fichero_mostrar + "_sp.txt";
        if (str == "ENGLISH")
          this._fichero_mostrado = this._fichero_mostrar + "_en.txt";
        if (str == "FRENCH")
          this._fichero_mostrado = this._fichero_mostrar + "_fr.txt";
        if (str == "ITALIAN")
          this._fichero_mostrado = this._fichero_mostrar + "_it.txt";
        if (str == "RUSSIAN")
          this._fichero_mostrado = this._fichero_mostrar + "_ru.txt";
        if (str == "GERMAN")
          this._fichero_mostrado = this._fichero_mostrar + "_de.txt";
        if (str == "PORTUGUESE")
          this._fichero_mostrado = this._fichero_mostrar + "_pt.txt";
        if (str == "CHINESE")
          this._fichero_mostrado = this._fichero_mostrar + "_zh-Hans.txt";
        this.richTextBox_informacion_general.LoadFile(this._fichero_mostrado);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Problem to read the Information file: " + ex.Message);
        this._fichero_mostrado = (string) null;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_informacion_general));
      this.richTextBox_informacion_general = new RichTextBox();
      this.SuspendLayout();
      this.richTextBox_informacion_general.BackColor = SystemColors.Info;
      this.richTextBox_informacion_general.Dock = DockStyle.Fill;
      this.richTextBox_informacion_general.Location = new Point(0, 0);
      this.richTextBox_informacion_general.Name = "richTextBox_informacion_general";
      this.richTextBox_informacion_general.Size = new Size(846, 737);
      this.richTextBox_informacion_general.TabIndex = 0;
      this.richTextBox_informacion_general.Text = "";
      this.ClientSize = new Size(846, 737);
      this.Controls.Add((Control) this.richTextBox_informacion_general);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Form_informacion_general);
      this.Text = "Instructions";
      this.Load += new EventHandler(this.Form_informacion_general_Load);
      this.ResumeLayout(false);
    }
  }
}
