// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_informacion
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Menu_informacion;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_informacion : Form
  {
    private IContainer components = (IContainer) null;
    private GroupBox groupBox2;
    private Label label3;
    private Label label2;
    private Label label1;
    private GroupBox groupBox1;
    private Label label4;
    private Label label5;
    private Button button_clave;
    private TextBox textBox_num_serie;
    private Label label6;
    private TextBox textBox_operatividad;
    private GroupBox groupBox3;
    private RichTextBox richTextBox_descripcion;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem copiarToolStripMenuItem;
    private ToolStripMenuItem seleccionarTodoToolStripMenuItem;
    private TextBox textBox_CLR;
    private TextBox textBox_PC;
    private TextBox textBox_SO;
    private TextBox textBox_version;
    private Button button_email;

    public Form_informacion() => this.InitializeComponent();

    private void Form_informacion_Load(object sender, EventArgs e)
    {
      this.textBox_version.Text = "Vers.94_2";
      Informacion informacion = new Informacion();
      this.textBox_SO.Text = informacion.Str_os_version;
      this.textBox_PC.Text = informacion.Str_nombre_PC;
      this.textBox_CLR.Text = informacion.Str_version_clr;
      this.textBox_num_serie.Text = informacion.Numero_serie.ToString();
      this.textBox_SO.Select(0, 0);
      int num = informacion.leer_clave(false);
      if (num == 10)
      {
        this.textBox_operatividad.Text = Resource_Form_informacion.String1;
        this.button_clave.Visible = false;
      }
      if (num == 1)
      {
        this.textBox_operatividad.Text = Resource_Form_informacion.String2;
        this.button_clave.Visible = true;
      }
      if (num > 0)
        return;
      this.textBox_operatividad.Text = Resource_Form_informacion.String3;
      this.button_clave.Visible = true;
    }

    private void button_clave_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_clave().ShowDialog();
      this.Enabled = true;
      this.Form_informacion_Load((object) null, (EventArgs) null);
    }

    private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox_descripcion.Copy();
    }

    private void seleccionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.richTextBox_descripcion.Focus();
      this.richTextBox_descripcion.SelectionStart = 0;
      this.richTextBox_descripcion.SelectionLength = this.richTextBox_descripcion.Text.Length;
      this.richTextBox_descripcion.Copy();
    }

    private void button_email_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_email().ShowDialog();
      this.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_informacion));
      this.groupBox2 = new GroupBox();
      this.textBox_CLR = new TextBox();
      this.textBox_PC = new TextBox();
      this.textBox_SO = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.groupBox1 = new GroupBox();
      this.button_email = new Button();
      this.textBox_version = new TextBox();
      this.textBox_operatividad = new TextBox();
      this.label6 = new Label();
      this.textBox_num_serie = new TextBox();
      this.button_clave = new Button();
      this.label5 = new Label();
      this.label4 = new Label();
      this.groupBox3 = new GroupBox();
      this.richTextBox_descripcion = new RichTextBox();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.copiarToolStripMenuItem = new ToolStripMenuItem();
      this.seleccionarTodoToolStripMenuItem = new ToolStripMenuItem();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.textBox_CLR);
      this.groupBox2.Controls.Add((Control) this.textBox_PC);
      this.groupBox2.Controls.Add((Control) this.textBox_SO);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      this.groupBox2.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.textBox_CLR, "textBox_CLR");
      this.textBox_CLR.Name = "textBox_CLR";
      this.textBox_CLR.ReadOnly = true;
      this.textBox_CLR.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.textBox_PC, "textBox_PC");
      this.textBox_PC.Name = "textBox_PC";
      this.textBox_PC.ReadOnly = true;
      this.textBox_PC.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.textBox_SO, "textBox_SO");
      this.textBox_SO.Name = "textBox_SO";
      this.textBox_SO.ReadOnly = true;
      this.textBox_SO.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.button_email);
      this.groupBox1.Controls.Add((Control) this.textBox_version);
      this.groupBox1.Controls.Add((Control) this.textBox_operatividad);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.textBox_num_serie);
      this.groupBox1.Controls.Add((Control) this.button_clave);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      this.groupBox1.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.button_email, "button_email");
      this.button_email.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_email.Name = "button_email";
      this.button_email.UseVisualStyleBackColor = false;
      this.button_email.Click += new EventHandler(this.button_email_Click);
      componentResourceManager.ApplyResources((object) this.textBox_version, "textBox_version");
      this.textBox_version.Name = "textBox_version";
      this.textBox_version.ReadOnly = true;
      this.textBox_version.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.textBox_operatividad, "textBox_operatividad");
      this.textBox_operatividad.ForeColor = SystemColors.WindowText;
      this.textBox_operatividad.Name = "textBox_operatividad";
      this.textBox_operatividad.ReadOnly = true;
      this.textBox_operatividad.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.textBox_num_serie, "textBox_num_serie");
      this.textBox_num_serie.Name = "textBox_num_serie";
      this.textBox_num_serie.ReadOnly = true;
      this.textBox_num_serie.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.button_clave, "button_clave");
      this.button_clave.BackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.button_clave.Name = "button_clave";
      this.button_clave.UseVisualStyleBackColor = false;
      this.button_clave.Click += new EventHandler(this.button_clave_Click);
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.groupBox3, "groupBox3");
      this.groupBox3.Controls.Add((Control) this.richTextBox_descripcion);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.TabStop = false;
      this.groupBox3.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.richTextBox_descripcion, "richTextBox_descripcion");
      this.richTextBox_descripcion.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.richTextBox_descripcion.ContextMenuStrip = this.contextMenuStrip1;
      this.richTextBox_descripcion.EnableAutoDragDrop = true;
      this.richTextBox_descripcion.Name = "richTextBox_descripcion";
      this.richTextBox_descripcion.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.copiarToolStripMenuItem,
        (ToolStripItem) this.seleccionarTodoToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      componentResourceManager.ApplyResources((object) this.copiarToolStripMenuItem, "copiarToolStripMenuItem");
      this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
      this.copiarToolStripMenuItem.Click += new EventHandler(this.copiarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.seleccionarTodoToolStripMenuItem, "seleccionarTodoToolStripMenuItem");
      this.seleccionarTodoToolStripMenuItem.Name = "seleccionarTodoToolStripMenuItem";
      this.seleccionarTodoToolStripMenuItem.Click += new EventHandler(this.seleccionarTodoToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.groupBox2);
      this.Name = nameof (Form_informacion);
      this.Load += new EventHandler(this.Form_informacion_Load);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
