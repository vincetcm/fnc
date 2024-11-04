// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_ajustes_explorador
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_ajustes_explorador : Form
  {
    private IContainer components = (IContainer) null;
    private GroupBox groupBox1;
    private RadioButton radioButton_fijo_PC;
    private RadioButton radioButton_ultimo_PC;
    private TextBox textBox_directorio_PC;
    private Button button_OK;
    private Button button_buscar_PC;
    private FolderBrowserDialog folderBrowserDialog1;
    private GroupBox groupBox2;
    private GroupBox groupBox3;
    private RadioButton radioButton_fijo_CNC;
    private RadioButton radioButton_ultimo_CNC;
    private GroupBox groupBox4;
    private RadioButton radioButton_8_digit;
    private RadioButton radioButton_4_digit;
    private GroupBox groupBox5;
    private RadioButton radioButton_keep_program_number;
    private RadioButton radioButton_change_program_number;
    private GroupBox groupBox6;
    private TextBox textBox_extension;
    private CheckBox checkBox_add_extension;
    private CheckBox checkBox_change_O;
    private TextBox textBox_replace_O;

    public Form_ajustes_explorador() => this.InitializeComponent();

    private void radioButton_ultimo_PC_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButton_ultimo_PC.Enabled)
        return;
      this.groupBox2.Enabled = false;
    }

    private void radioButton_fijo_PC_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButton_fijo_PC.Enabled)
        return;
      this.groupBox2.Enabled = true;
    }

    private void button_buscar_PC_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox_directorio_PC.Text = this.folderBrowserDialog1.SelectedPath;
      this.textBox_directorio_PC.SelectionStart = this.textBox_directorio_PC.Text.Length;
      this.textBox_directorio_PC.Focus();
    }

    private void button_OK_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
        return;
      string str1 = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, true);
      if (this.radioButton_fijo_PC.Checked)
      {
        string str2 = this.textBox_directorio_PC.Text;
        int startIndex = str2.IndexOf("\\");
        if (startIndex > 0)
          str2 = str2.Insert(startIndex, "\\");
        registryKey2.SetValue("Selected_folder_PC", (object) str2);
        registryKey2.SetValue("Selected_folder_PC_Checked", (object) "YES");
      }
      else
        registryKey2.SetValue("Selected_folder_PC_Checked", (object) "NO");
      if (this.radioButton_fijo_CNC.Checked)
        registryKey2.SetValue("Selected_fijo_CNC_Checked", (object) "YES");
      else
        registryKey2.SetValue("Selected_fijo_CNC_Checked", (object) "NO");
      if (this.radioButton_4_digit.Checked)
        registryKey2.SetValue("Selected_4_digit", (object) "YES");
      else
        registryKey2.SetValue("Selected_4_digit", (object) "NO");
      if (this.radioButton_keep_program_number.Checked)
      {
        registryKey2.SetValue("Keep_program_number_checked", (object) "YES");
      }
      else
      {
        registryKey2.SetValue("Keep_program_number_checked", (object) "NO");
        if (this.checkBox_change_O.Checked)
        {
          registryKey2.SetValue("Change_O_checked", (object) "YES");
          registryKey2.SetValue("Replace_O", (object) this.textBox_replace_O.Text);
        }
        else
          registryKey2.SetValue("Change_O_checked", (object) "NO");
        if (this.checkBox_add_extension.Checked)
        {
          registryKey2.SetValue("Add_extension_checked", (object) "YES");
          registryKey2.SetValue("Extension", (object) this.textBox_extension.Text);
        }
        else
          registryKey2.SetValue("Add_extension_checked", (object) "NO");
      }
      this.Close();
    }

    private void Form_directorio_trabajo_Load(object sender, EventArgs e)
    {
      try
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
          return;
        string str1 = (string) registryKey1.GetValue("Machine");
        RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, true);
        string str2 = (string) registryKey2.GetValue("Name");
        this.Text = this.Text + str1 + "   (" + str2 + ")";
        string str3 = (string) registryKey2.GetValue("Selected_folder_PC_Checked");
        string str4 = (string) registryKey2.GetValue("Selected_folder_PC");
        if (str4 != null)
          this.textBox_directorio_PC.Text = str4.Replace("\\\\", "\\");
        this.radioButton_fijo_PC.Checked = str3 == "YES";
        this.radioButton_fijo_CNC.Checked = (string) registryKey2.GetValue("Selected_fijo_CNC_Checked") == "YES";
        if ((string) registryKey2.GetValue("Selected_4_digit") == "YES")
        {
          this.radioButton_4_digit.Checked = true;
          this.radioButton_8_digit.Checked = false;
        }
        else
        {
          this.radioButton_4_digit.Checked = false;
          this.radioButton_8_digit.Checked = true;
        }
        if ((string) registryKey2.GetValue("Keep_program_number_checked") == "YES")
        {
          this.radioButton_keep_program_number.Checked = true;
          this.radioButton_change_program_number.Checked = false;
          this.groupBox6.Enabled = false;
        }
        else
        {
          this.radioButton_keep_program_number.Checked = false;
          this.radioButton_change_program_number.Checked = true;
          this.groupBox6.Enabled = true;
          if ((string) registryKey2.GetValue("Change_O_Checked") == "YES")
          {
            this.checkBox_change_O.Checked = true;
            this.textBox_replace_O.Text = (string) registryKey2.GetValue("Replace_O");
          }
          else
            this.checkBox_change_O.Checked = false;
          if ((string) registryKey2.GetValue("Add_extension_Checked") == "YES")
          {
            this.checkBox_add_extension.Checked = true;
            this.textBox_extension.Text = (string) registryKey2.GetValue("Extension");
          }
          else
            this.checkBox_add_extension.Checked = false;
        }
      }
      catch
      {
      }
    }

    private void radioButton_keep_program_number_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioButton_keep_program_number.Checked)
        this.groupBox6.Enabled = true;
      else
        this.groupBox6.Enabled = false;
    }

    private void checkBox_change_O_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBox_change_O.Checked)
        this.textBox_replace_O.Enabled = true;
      else
        this.textBox_replace_O.Enabled = false;
    }

    private void checkBox_add_extension_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBox_add_extension.Checked)
        this.textBox_extension.Enabled = true;
      else
        this.textBox_extension.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_ajustes_explorador));
      this.groupBox1 = new GroupBox();
      this.radioButton_fijo_PC = new RadioButton();
      this.radioButton_ultimo_PC = new RadioButton();
      this.button_buscar_PC = new Button();
      this.textBox_directorio_PC = new TextBox();
      this.button_OK = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.groupBox2 = new GroupBox();
      this.groupBox3 = new GroupBox();
      this.radioButton_fijo_CNC = new RadioButton();
      this.radioButton_ultimo_CNC = new RadioButton();
      this.groupBox4 = new GroupBox();
      this.radioButton_8_digit = new RadioButton();
      this.radioButton_4_digit = new RadioButton();
      this.groupBox5 = new GroupBox();
      this.radioButton_keep_program_number = new RadioButton();
      this.radioButton_change_program_number = new RadioButton();
      this.groupBox6 = new GroupBox();
      this.textBox_extension = new TextBox();
      this.checkBox_add_extension = new CheckBox();
      this.checkBox_change_O = new CheckBox();
      this.textBox_replace_O = new TextBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.radioButton_fijo_PC);
      this.groupBox1.Controls.Add((Control) this.radioButton_ultimo_PC);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButton_fijo_PC, "radioButton_fijo_PC");
      this.radioButton_fijo_PC.Name = "radioButton_fijo_PC";
      this.radioButton_fijo_PC.UseVisualStyleBackColor = true;
      this.radioButton_fijo_PC.CheckedChanged += new EventHandler(this.radioButton_fijo_PC_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.radioButton_ultimo_PC, "radioButton_ultimo_PC");
      this.radioButton_ultimo_PC.Checked = true;
      this.radioButton_ultimo_PC.Name = "radioButton_ultimo_PC";
      this.radioButton_ultimo_PC.TabStop = true;
      this.radioButton_ultimo_PC.UseVisualStyleBackColor = true;
      this.radioButton_ultimo_PC.CheckedChanged += new EventHandler(this.radioButton_ultimo_PC_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.button_buscar_PC, "button_buscar_PC");
      this.button_buscar_PC.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_buscar_PC.Name = "button_buscar_PC";
      this.button_buscar_PC.UseVisualStyleBackColor = false;
      this.button_buscar_PC.Click += new EventHandler(this.button_buscar_PC_Click);
      componentResourceManager.ApplyResources((object) this.textBox_directorio_PC, "textBox_directorio_PC");
      this.textBox_directorio_PC.Name = "textBox_directorio_PC";
      componentResourceManager.ApplyResources((object) this.button_OK, "button_OK");
      this.button_OK.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_OK.Name = "button_OK";
      this.button_OK.UseVisualStyleBackColor = false;
      this.button_OK.Click += new EventHandler(this.button_OK_Click);
      componentResourceManager.ApplyResources((object) this.folderBrowserDialog1, "folderBrowserDialog1");
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.button_buscar_PC);
      this.groupBox2.Controls.Add((Control) this.textBox_directorio_PC);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.groupBox3, "groupBox3");
      this.groupBox3.Controls.Add((Control) this.radioButton_fijo_CNC);
      this.groupBox3.Controls.Add((Control) this.radioButton_ultimo_CNC);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButton_fijo_CNC, "radioButton_fijo_CNC");
      this.radioButton_fijo_CNC.Name = "radioButton_fijo_CNC";
      this.radioButton_fijo_CNC.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.radioButton_ultimo_CNC, "radioButton_ultimo_CNC");
      this.radioButton_ultimo_CNC.Checked = true;
      this.radioButton_ultimo_CNC.Name = "radioButton_ultimo_CNC";
      this.radioButton_ultimo_CNC.TabStop = true;
      this.radioButton_ultimo_CNC.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.groupBox4, "groupBox4");
      this.groupBox4.Controls.Add((Control) this.radioButton_8_digit);
      this.groupBox4.Controls.Add((Control) this.radioButton_4_digit);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButton_8_digit, "radioButton_8_digit");
      this.radioButton_8_digit.Checked = true;
      this.radioButton_8_digit.Name = "radioButton_8_digit";
      this.radioButton_8_digit.TabStop = true;
      this.radioButton_8_digit.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.radioButton_4_digit, "radioButton_4_digit");
      this.radioButton_4_digit.Name = "radioButton_4_digit";
      this.radioButton_4_digit.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.groupBox5, "groupBox5");
      this.groupBox5.Controls.Add((Control) this.radioButton_keep_program_number);
      this.groupBox5.Controls.Add((Control) this.radioButton_change_program_number);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButton_keep_program_number, "radioButton_keep_program_number");
      this.radioButton_keep_program_number.Checked = true;
      this.radioButton_keep_program_number.Name = "radioButton_keep_program_number";
      this.radioButton_keep_program_number.TabStop = true;
      this.radioButton_keep_program_number.UseVisualStyleBackColor = true;
      this.radioButton_keep_program_number.CheckedChanged += new EventHandler(this.radioButton_keep_program_number_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.radioButton_change_program_number, "radioButton_change_program_number");
      this.radioButton_change_program_number.Name = "radioButton_change_program_number";
      this.radioButton_change_program_number.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.groupBox6, "groupBox6");
      this.groupBox6.Controls.Add((Control) this.textBox_extension);
      this.groupBox6.Controls.Add((Control) this.checkBox_add_extension);
      this.groupBox6.Controls.Add((Control) this.checkBox_change_O);
      this.groupBox6.Controls.Add((Control) this.textBox_replace_O);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.TabStop = false;
      componentResourceManager.ApplyResources((object) this.textBox_extension, "textBox_extension");
      this.textBox_extension.Name = "textBox_extension";
      componentResourceManager.ApplyResources((object) this.checkBox_add_extension, "checkBox_add_extension");
      this.checkBox_add_extension.Name = "checkBox_add_extension";
      this.checkBox_add_extension.UseVisualStyleBackColor = true;
      this.checkBox_add_extension.CheckedChanged += new EventHandler(this.checkBox_add_extension_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.checkBox_change_O, "checkBox_change_O");
      this.checkBox_change_O.Name = "checkBox_change_O";
      this.checkBox_change_O.UseVisualStyleBackColor = true;
      this.checkBox_change_O.CheckedChanged += new EventHandler(this.checkBox_change_O_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.textBox_replace_O, "textBox_replace_O");
      this.textBox_replace_O.Name = "textBox_replace_O";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox6);
      this.Controls.Add((Control) this.groupBox5);
      this.Controls.Add((Control) this.groupBox4);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.button_OK);
      this.Name = nameof (Form_ajustes_explorador);
      this.ShowIcon = false;
      this.Load += new EventHandler(this.Form_directorio_trabajo_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
