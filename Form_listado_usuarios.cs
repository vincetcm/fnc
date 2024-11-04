// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_listado_usuarios
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Servidor_CNC.FTP_Servidor;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_listado_usuarios : Form
  {
    private IContainer components = (IContainer) null;
    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private Button button_editar;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private Button button_salir;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;

    public Form_listado_usuarios() => this.InitializeComponent();

    private void button_añadir_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      Form_editar_usuario formEditarUsuario = new Form_editar_usuario();
      int num = (int) formEditarUsuario.ShowDialog();
      this.listView1.Items.Add(new ListViewItem(formEditarUsuario.Nombre_usuario)
      {
        SubItems = {
          formEditarUsuario.Clave_usuario,
          formEditarUsuario.Directorio_trabajo,
          formEditarUsuario.Puerto_FTP,
          formEditarUsuario.Auto_start
        }
      });
      this.Enabled = true;
      this.listView1.Show();
    }

    private void Form_listado_usuarios_Load(object sender, EventArgs e)
    {
      try
      {
        this.listView1.Items.Clear();
        for (int index = 1; index <= 20; ++index)
        {
          string text1 = index.ToString();
          RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + text1, false);
          if (registryKey != null && !((string) registryKey.GetValue("Type") != "ETHERNET"))
          {
            ListViewItem listViewItem = new ListViewItem(text1);
            listViewItem.SubItems.Add((string) registryKey.GetValue("Name"));
            listViewItem.SubItems.Add((string) registryKey.GetValue("User_FTP_Server"));
            listViewItem.SubItems.Add((string) registryKey.GetValue("Password_FTP_Server"));
            listViewItem.SubItems.Add((string) registryKey.GetValue("Directory_FTP_Server"));
            listViewItem.SubItems.Add((string) registryKey.GetValue("Port_FTP_Server"));
            listViewItem.UseItemStyleForSubItems = false;
            string text2 = (string) registryKey.GetValue("Auto_start_FTP_Server");
            listViewItem.SubItems.Add(text2).ForeColor = System.Drawing.Color.Red;
            this.listView1.Items.Add(listViewItem);
            this.listView1.Items[0].Selected = true;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error: " + ex.Message);
      }
    }

    private void button_editar_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView1.FocusedItem == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_usuarios.String1);
        }
        else
        {
          string text = this.listView1.FocusedItem.Text;
          RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + text, true);
          if (registryKey == null)
          {
            int num2 = (int) MessageBox.Show(Resource_Form_listado_usuarios.String2);
          }
          else
          {
            Form_editar_usuario formEditarUsuario = new Form_editar_usuario();
            formEditarUsuario.Numero_maquina = text;
            formEditarUsuario.Nombre_usuario = (string) registryKey.GetValue("User_FTP_Server");
            formEditarUsuario.Clave_usuario = (string) registryKey.GetValue("Password_FTP_Server");
            formEditarUsuario.Directorio_trabajo = (string) registryKey.GetValue("Directory_FTP_Server");
            formEditarUsuario.Puerto_FTP = (string) registryKey.GetValue("Port_FTP_Server");
            formEditarUsuario.Auto_start = (string) registryKey.GetValue("Auto_start_FTP_Server");
            this.Enabled = false;
            int num3 = (int) formEditarUsuario.ShowDialog();
            if (formEditarUsuario.Aceptado)
            {
              registryKey.SetValue("User_FTP_Server", (object) formEditarUsuario.Nombre_usuario);
              registryKey.SetValue("Password_FTP_Server", (object) formEditarUsuario.Clave_usuario);
              registryKey.SetValue("Directory_FTP_Server", (object) formEditarUsuario.Directorio_trabajo);
              registryKey.SetValue("Port_FTP_Server", (object) formEditarUsuario.Puerto_FTP);
              registryKey.SetValue("Auto_start_FTP_Server", (object) formEditarUsuario.Auto_start);
            }
            this.Enabled = true;
            this.Form_listado_usuarios_Load((object) null, (EventArgs) null);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error: " + ex.Message);
      }
    }

    private void button_salir_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_listado_usuarios));
      this.listView1 = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.button_editar = new Button();
      this.button_salir = new Button();
      this.SuspendLayout();
      this.listView1.Columns.AddRange(new ColumnHeader[7]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5,
        this.columnHeader6,
        this.columnHeader7
      });
      this.listView1.HideSelection = false;
      componentResourceManager.ApplyResources((object) this.listView1, "listView1");
      this.listView1.Name = "listView1";
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
      componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
      componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
      componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
      componentResourceManager.ApplyResources((object) this.columnHeader5, "columnHeader5");
      componentResourceManager.ApplyResources((object) this.columnHeader6, "columnHeader6");
      componentResourceManager.ApplyResources((object) this.columnHeader7, "columnHeader7");
      this.button_editar.AccessibleRole = AccessibleRole.None;
      this.button_editar.BackColor = System.Drawing.Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      componentResourceManager.ApplyResources((object) this.button_editar, "button_editar");
      this.button_editar.Name = "button_editar";
      this.button_editar.UseVisualStyleBackColor = false;
      this.button_editar.Click += new EventHandler(this.button_editar_Click);
      this.button_salir.AccessibleRole = AccessibleRole.None;
      this.button_salir.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 128, 128);
      componentResourceManager.ApplyResources((object) this.button_salir, "button_salir");
      this.button_salir.Name = "button_salir";
      this.button_salir.UseVisualStyleBackColor = false;
      this.button_salir.Click += new EventHandler(this.button_salir_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.button_salir);
      this.Controls.Add((Control) this.button_editar);
      this.Controls.Add((Control) this.listView1);
      this.Name = nameof (Form_listado_usuarios);
      this.Load += new EventHandler(this.Form_listado_usuarios_Load);
      this.ResumeLayout(false);
    }
  }
}
