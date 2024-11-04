// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_explorer
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Enviar;
using FANUC_Open_Com.Explorador;
using FANUC_Open_Com.Listado_programas;
using FANUC_Open_Com.Properties;
using FANUC_Open_Com.Recibir;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_explorer : Form
  {
    private FileExplorer file_e = new FileExplorer();
    private TreeNode lastNode_files = (TreeNode) null;
    private string[] fichero_copy = (string[]) null;
    private string[] programa_copy = (string[]) null;
    private string fichero_rename = (string) null;
    private string programa_rename = (string) null;
    private string directorio_PC_copiar = (string) null;
    private string directorio_CNC_copiar = (string) null;
    private string directorio_PC_pegar = (string) null;
    private string directorio_CNC_pegar = (string) null;
    private string directorio_PC_ultimo = "C:\\";
    private string directorio_CNC_ultimo = "//CNC_MEM/";
    private string origen_drag = (string) null;
    private short tipo_datos = 0;
    private string machine_name = (string) null;
    private DateTime old_time = DateTime.UtcNow;
    private ushort hndl = 0;
    private Class_chequeo_errores_focas chequeo = new Class_chequeo_errores_focas();
    private Focas1.focas_ret ret_focas;
    private CNC_Explorer cnc_e = new CNC_Explorer();
    private string origen_click = (string) null;
    private bool treeView_PC_visualizar_subfolders = false;
    private bool treeView_CNC_visualizar_subfolders = false;
    private bool load_terminado = false;
    private TreeNode ultimo_nodo_CNC = new TreeNode();
    private bool memoria_mensaje_sacado = false;
    private IContainer components = (IContainer) null;
    private ImageList imageList1;
    private Label label_tree_PC;
    private ListView listView_PC;
    private ColumnHeader columnHeaderPC0;
    private ColumnHeader columnHeaderPC1;
    private ColumnHeader columnHeaderPC2;
    private ColumnHeader columnHeaderPC3;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private SplitContainer splitContainer3;
    private TreeView treeView_CNC;
    private ListView listView_CNC;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private Label label_ficheros_PC;
    private Label label_tree_CNC;
    private Label label_listado_programas;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem toolStripMenuItem3;
    private ToolStripMenuItem toolStripMenuItem4;
    private ToolStripMenuItem toolStripMenuItem5;
    private ToolStripMenuItem toolStripMenuItem6;
    private ToolStripMenuItem toolStripMenuItem7;
    private ToolStripMenuItem toolStripMenuItem8;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem borrarToolStripMenuItem;
    private ToolStripMenuItem renombrarToolStripMenuItem;
    private ToolStripMenuItem copiarToolStripMenuItem;
    private ToolStripMenuItem pegarToolStripMenuItem;
    private ToolStripMenuItem nuevoToolStripMenuItem;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private TreeView treeView_PC;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;
    private Panel panel4;
    private ToolStripMenuItem ConfiguracionToolStripMenuItem;
    private ToolStripMenuItem reConectarCNCToolStripMenuItem;
    private Timer timer_refresco_conexion;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem ajustesToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem select_todo_toolStripMenuItem;
    private ToolStripSeparator toolStripSeparator4;
    private ColumnHeader columnHeaderPC4;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripMenuItem EditToolStripMenuItem;

    public Form_explorer() => this.InitializeComponent();

    private void Form_explorer_Load(object sender, EventArgs e)
    {
      if (Settings.Default.CNC_Explorer_size.Width != 0 && Settings.Default.CNC_Explorer_size.Height != 0)
      {
        this.WindowState = Settings.Default.CNC_Explorer_state;
        if (this.WindowState == FormWindowState.Minimized)
          this.WindowState = FormWindowState.Normal;
        this.Location = Settings.Default.CNC_Explorer_location;
        this.Size = Settings.Default.CNC_Explorer_size;
      }
      this.load_terminado = false;
      TreeNode parentNode = (TreeNode) null;
      try
      {
        this.treeView_PC.CollapseAll();
        this.treeView_CNC.CollapseAll();
        this.treeView_PC_visualizar_subfolders = false;
        this.treeView_CNC_visualizar_subfolders = false;
        this.listView_PC.Items.Clear();
        this.listView_CNC.Items.Clear();
        this.label_tree_CNC.Text = this.label_tree_CNC.Tag.ToString();
        Informacion informacion = new Informacion();
        if (informacion.leer_clave(true) <= 0)
        {
          if (informacion.leer_contador_uso(true) == -1)
            this.Close();
          if (informacion.leer_fecha_uso() == -1)
            this.Close();
        }
        this.file_e.Crear_Tree(this.treeView_PC);
        string str1 = (string) null;
        string str2 = (string) null;
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 != null)
        {
          try
          {
            string str3 = (string) registryKey1.GetValue("Machine");
            RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str3, false);
            this.machine_name = (string) registryKey2.GetValue("Name");
            if ((string) registryKey2.GetValue("Type") != "ETHERNET")
            {
              int num = (int) MessageBox.Show(Resource_Form_explorer.String12);
            }
            str1 = !((string) registryKey2.GetValue("selected_folder_PC_checked") == "YES") ? (string) registryKey2.GetValue("Last_folder_PC") : (string) registryKey2.GetValue("Selected_folder_PC");
            str2 = !((string) registryKey2.GetValue("Selected_fijo_CNC_Checked") == "YES") ? (string) registryKey2.GetValue("Last_folder_CNC") : "CNC_MEM";
          }
          catch
          {
          }
          if (str1 != null)
          {
            string[] strArray = Regex.Split(str1 + "\\", "\\\\");
            string key1 = "";
            string key2 = "";
            string key3 = "";
            string key4 = "";
            string key5 = "";
            string key6 = "";
            string key7 = "";
            string key8 = "";
            string key9 = "";
            string key10 = "";
            try
            {
              key1 = strArray[0] + "\\";
              key2 = strArray[2];
              key3 = strArray[3];
              key4 = strArray[4];
              key5 = strArray[5];
              key6 = strArray[6];
              key7 = strArray[7];
              key8 = strArray[8];
              key9 = strArray[9];
              key10 = strArray[10];
            }
            catch
            {
            }
            bool flag = false;
            if (key1 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1];
              if (parentNode == null)
              {
                key1 = "C:\\";
                parentNode = this.treeView_PC.Nodes[key1];
                flag = true;
              }
              this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_PC.SelectedNode = parentNode;
            }
            if (key2 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key3 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key4 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key5 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4].Nodes[key5];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key6 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4].Nodes[key5].Nodes[key6];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key7 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4].Nodes[key5].Nodes[key6].Nodes[key7];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key8 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4].Nodes[key5].Nodes[key6].Nodes[key7].Nodes[key8];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key9 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4].Nodes[key5].Nodes[key6].Nodes[key7].Nodes[key8].Nodes[key9];
              if (parentNode == null)
              {
                flag = true;
              }
              else
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
            if (key10 != "" && !flag)
            {
              parentNode = this.treeView_PC.Nodes[key1].Nodes[key2].Nodes[key3].Nodes[key4].Nodes[key5].Nodes[key6].Nodes[key7].Nodes[key8].Nodes[key9].Nodes[key10];
              if (parentNode != null)
              {
                this.file_e.Visualizar_Subdirectorios_Treeview(parentNode);
                this.treeView_PC.SelectedNode = parentNode;
              }
            }
          }
        }
        this.treeView_PC_visualizar_subfolders = true;
        Focas1.ODBSYS a = new Focas1.ODBSYS();
        if (this.hndl == (ushort) 0)
        {
          int num = new Class_configurar_ethernet().obtener_handle();
          if (num <= 0)
            return;
          this.hndl = (ushort) num;
        }
        this.cnc_e.Handle = this.hndl;
        this.cnc_e.Old_CNC = (ushort) 0;
        this.cnc_e.Crear_Tree(this.treeView_CNC);
        this.timer_refresco_conexion.Enabled = true;
        if (str2 != null)
        {
          string[] strArray = Regex.Split(str2 + "\\", "\\\\");
          string key11 = "";
          string key12 = "";
          string key13 = "";
          string key14 = "";
          string key15 = "";
          string key16 = "";
          string key17 = "";
          string key18 = "";
          string key19 = "";
          string key20 = "";
          try
          {
            key11 = strArray[0];
            key12 = strArray[1];
            key13 = strArray[2];
            key14 = strArray[3];
            key15 = strArray[4];
            key16 = strArray[5];
            key17 = strArray[6];
            key18 = strArray[7];
            key19 = strArray[8];
            key20 = strArray[9];
          }
          catch
          {
          }
          bool flag = false;
          if (key11 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11];
            if (parentNode == null)
            {
              key11 = "CNC_MEM";
              parentNode = this.treeView_CNC.Nodes[key11];
              flag = true;
            }
            this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
            this.treeView_CNC.SelectedNode = parentNode;
          }
          if (key12 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key13 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key14 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key15 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14].Nodes[key15];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key16 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14].Nodes[key15].Nodes[key16];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key17 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14].Nodes[key15].Nodes[key16].Nodes[key17];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key18 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14].Nodes[key15].Nodes[key16].Nodes[key17].Nodes[key18];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key19 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14].Nodes[key15].Nodes[key16].Nodes[key17].Nodes[key18].Nodes[key19];
            if (parentNode == null)
            {
              flag = true;
            }
            else
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
          if (key20 != "" && !flag)
          {
            parentNode = this.treeView_CNC.Nodes[key11].Nodes[key12].Nodes[key13].Nodes[key14].Nodes[key15].Nodes[key16].Nodes[key17].Nodes[key18].Nodes[key19].Nodes[key20];
            if (parentNode != null)
            {
              this.cnc_e.Visualizar_Subdirectorios_Treeview(parentNode);
              this.treeView_CNC.SelectedNode = parentNode;
            }
          }
        }
        this.treeView_CNC.TreeViewNodeSorter = (IComparer) new Form_explorer.NodeSorter();
        this.treeView_CNC_visualizar_subfolders = true;
        int num1 = (int) Focas1.cnc_sysinfo(this.hndl, a);
        string str4 = new string(a.cnc_type);
        string str5 = new string(a.mt_type);
        byte[] bytes = BitConverter.GetBytes(a.addinfo);
        string str6 = "";
        if (((int) bytes[0] & 2) == 2)
          str6 = "i";
        string str7 = "";
        if (bytes[1] == (byte) 0)
          str7 = "";
        if (bytes[1] == (byte) 1)
          str7 = "A";
        if (bytes[1] == (byte) 2)
          str7 = "B";
        if (bytes[1] == (byte) 3)
          str7 = "C";
        if (bytes[1] == (byte) 4)
          str7 = "D";
        if (bytes[1] == (byte) 5)
          str7 = "E";
        if (bytes[1] == (byte) 6)
          str7 = "F";
        this.label_tree_CNC.Text = "CNC : " + (str4 + " " + str6 + " - " + str5 + str7) + " (" + this.machine_name + ")";
        this.load_terminado = true;
        this.treeView_CNC.SelectedNode = parentNode;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed" + ex.Message);
      }
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void treeView_PC_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      if (!this.treeView_PC_visualizar_subfolders)
        return;
      this.file_e.Visualizar_Subdirectorios_Treeview(e.Node);
    }

    private void treeView_PC_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      this.file_e.Listar_Ficheros(e.Node, this.listView_PC);
      this.lastNode_files = e.Node;
      if (e.Node.FullPath == null)
        return;
      this.directorio_PC_ultimo = e.Node.FullPath.ToString();
      if (!this.directorio_PC_ultimo.EndsWith("\\"))
        this.directorio_PC_ultimo += "\\";
    }

    private void treeView_CNC_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      if (!this.treeView_CNC_visualizar_subfolders)
        return;
      this.cnc_e.Visualizar_Subdirectorios_Treeview(e.Node);
    }

    private void treeView_CNC_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      this.ultimo_nodo_CNC = e.Node;
      if (!this.load_terminado)
        return;
      this.cnc_e.Listar_Programas(e.Node, this.listView_CNC);
      if (e.Node.FullPath == null)
        return;
      this.directorio_CNC_ultimo = "//" + e.Node.FullPath.ToString().Replace("\\", "/") + "/";
    }

    private void treeView_CNC_AfterCollapse(object sender, TreeViewEventArgs e)
    {
      this.cnc_e.Visualizar_Subdirectorios_Treeview(e.Node);
    }

    private void listView_PC_ItemDrag(object sender, ItemDragEventArgs e)
    {
      try
      {
        this.origen_drag = "PC";
        this.directorio_PC_copiar = this.directorio_PC_ultimo;
        string[] selectionPc = this.GetSelection_PC();
        if (selectionPc == null)
          return;
        int num = (int) this.DoDragDrop((object) new DataObject(DataFormats.FileDrop, (object) selectionPc), DragDropEffects.Copy);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed" + ex.Message);
      }
    }

    private string[] GetSelection_PC()
    {
      if (this.listView_PC.SelectedItems.Count == 0)
        return (string[]) null;
      string[] selectionPc = new string[this.listView_PC.SelectedItems.Count];
      int num = 0;
      foreach (ListViewItem selectedItem in this.listView_PC.SelectedItems)
        selectionPc[num++] = selectedItem.Text;
      return selectionPc;
    }

    private void listView_PC_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
        return;
      e.Effect = DragDropEffects.All;
    }

    private void listView_PC_DragOver(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
        return;
      e.Effect = DragDropEffects.All;
    }

    private void listView_PC_DragDrop(object sender, DragEventArgs e)
    {
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      this.fichero_copy = (string[]) null;
      this.programa_copy = (string[]) null;
      if (this.origen_drag == "CNC")
        this.programa_copy = data;
      else
        this.fichero_copy = data;
      this.directorio_PC_pegar = this.directorio_PC_ultimo;
      this.origen_click = "listView_PC";
      this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void treeView_PC_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
        return;
      e.Effect = DragDropEffects.All;
    }

    private void treeView_PC_DragDrop(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.treeView_PC.GetNodeAt(this.treeView_PC.PointToClient(new Point(e.X, e.Y)));
      if (nodeAt == null || !(nodeAt.Text != ""))
        return;
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      this.fichero_copy = (string[]) null;
      this.programa_copy = (string[]) null;
      if (this.origen_drag == "PC")
        this.fichero_copy = data;
      if (this.origen_drag == "CNC")
        this.programa_copy = data;
      this.directorio_PC_pegar = nodeAt.FullPath.ToString();
      if (!this.directorio_PC_pegar.EndsWith("\\"))
        this.directorio_PC_pegar += "\\";
      this.origen_click = "treeView_PC";
      this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void listView_CNC_ItemDrag(object sender, ItemDragEventArgs e)
    {
      try
      {
        this.origen_drag = "CNC";
        this.directorio_CNC_copiar = this.directorio_CNC_ultimo;
        string[] selectionCnc = this.GetSelection_CNC();
        if (selectionCnc == null)
          return;
        int num = (int) this.DoDragDrop((object) new DataObject(DataFormats.FileDrop, (object) selectionCnc), DragDropEffects.Copy);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed" + ex.Message);
      }
    }

    private string[] GetSelection_CNC()
    {
      if (this.listView_CNC.SelectedItems.Count == 0)
        return (string[]) null;
      string[] selectionCnc = new string[this.listView_CNC.SelectedItems.Count];
      int num = 0;
      foreach (ListViewItem selectedItem in this.listView_CNC.SelectedItems)
        selectionCnc[num++] = selectedItem.Text;
      return selectionCnc;
    }

    private void listView_CNC_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
        return;
      e.Effect = DragDropEffects.All;
    }

    private void listView_CNC_DragOver(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
        return;
      e.Effect = DragDropEffects.All;
    }

    private void listView_CNC_DragDrop(object sender, DragEventArgs e)
    {
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      this.fichero_copy = (string[]) null;
      this.programa_copy = (string[]) null;
      if (this.origen_drag == "CNC")
        this.programa_copy = data;
      else
        this.fichero_copy = data;
      this.origen_click = "listView_CNC";
      this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void treeView_CNC_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
        return;
      e.Effect = DragDropEffects.All;
    }

    private void treeView_CNC_DragDrop(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.treeView_CNC.GetNodeAt(this.treeView_CNC.PointToClient(new Point(e.X, e.Y)));
      if (nodeAt == null || !(nodeAt.Text != ""))
        return;
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      this.fichero_copy = (string[]) null;
      this.programa_copy = (string[]) null;
      if (this.origen_drag == "PC")
        this.fichero_copy = data;
      if (this.origen_drag == "CNC")
        this.programa_copy = data;
      this.directorio_CNC_pegar = "//" + nodeAt.FullPath.ToString().Replace("\\", "/") + "/";
      this.origen_click = "treeView_CNC";
      this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void listView_PC_MouseDown(object sender, MouseEventArgs e)
    {
      this.origen_click = "listView_PC";
    }

    private void listView_CNC_MouseDown(object sender, MouseEventArgs e)
    {
      this.origen_click = "listView_CNC";
    }

    private void treeView_PC_MouseDown(object sender, MouseEventArgs e)
    {
      this.origen_click = "treeView_PC";
    }

    private void treeView_CNC_MouseDown(object sender, MouseEventArgs e)
    {
      this.origen_click = "treeView_CNC";
    }

    private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        bool flag = false;
        switch (this.origen_click)
        {
          case "listView_PC":
            for (int index = 0; index < this.listView_PC.SelectedItems.Count; ++index)
            {
              string text = this.listView_PC.SelectedItems[index].Text;
              if (!flag)
              {
                DialogResult dialogResult = MessageBox.Show(Resource_Form_explorer.String2, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                flag = true;
                if (dialogResult == DialogResult.No)
                  break;
              }
              File.Delete(this.treeView_PC.SelectedNode.FullPath.ToString() + "\\" + text);
            }
            this.file_e.Listar_Ficheros(this.treeView_PC.SelectedNode, this.listView_PC);
            break;
          case "treeView_PC":
            if (this.treeView_PC.SelectedNode == null)
            {
              int num = (int) MessageBox.Show(Resource_Form_explorer.String1);
              break;
            }
            string path = this.treeView_PC.SelectedNode.FullPath.ToString();
            if (MessageBox.Show(Resource_Form_explorer.String3, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
              break;
            Directory.Delete(path);
            TreeNode selectedNode1 = this.treeView_PC.SelectedNode;
            TreeNode parent1 = selectedNode1.Parent;
            selectedNode1.Remove();
            this.treeView_PC.SelectedNode = parent1;
            this.treeView_PC.Refresh();
            break;
          case "listView_CNC":
            for (int index = 0; index < this.listView_CNC.SelectedItems.Count; ++index)
            {
              if (this.cnc_e.Old_CNC == (ushort) 0)
              {
                string text = this.listView_CNC.SelectedItems[index].Text;
                if (!flag)
                {
                  DialogResult dialogResult = MessageBox.Show(Resource_Form_explorer.String2, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  flag = true;
                  if (dialogResult == DialogResult.No)
                    break;
                }
                this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_del(this.hndl, (object) ("//" + this.treeView_CNC.SelectedNode.FullPath.ToString().Replace("\\", "/") + "/" + text));
                if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_del", this.ret_focas) != 0)
                  break;
              }
              else
              {
                if (!flag)
                {
                  DialogResult dialogResult = MessageBox.Show(Resource_Form_explorer.String2, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  flag = true;
                  if (dialogResult == DialogResult.No)
                    break;
                }
                this.ret_focas = (Focas1.focas_ret) Focas1.cnc_delete(this.hndl, Convert.ToInt32(this.listView_CNC.SelectedItems[index].Text.Substring(1)));
                if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_delete", this.ret_focas) != 0)
                  break;
              }
            }
            this.cnc_e.Listar_Programas(this.treeView_CNC.SelectedNode, this.listView_CNC);
            break;
          case "treeView_CNC":
            if (this.treeView_CNC.SelectedNode == null)
            {
              int num = (int) MessageBox.Show(Resource_Form_explorer.String1);
              break;
            }
            if (this.cnc_e.Old_CNC != (ushort) 0 || MessageBox.Show(Resource_Form_explorer.String4, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
              break;
            string str = this.treeView_CNC.SelectedNode.FullPath.ToString().Replace("\\", "/");
            string a = "//" + str + "/";
            if (a.Contains("//DATA_SV/"))
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dsrmdir(this.hndl, (object) "DATA_SV", (object) ("//" + str));
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dsrmdir", this.ret_focas) != 0)
                break;
            }
            else
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_del(this.hndl, (object) a);
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_del", this.ret_focas) != 0)
                break;
            }
            TreeNode selectedNode2 = this.treeView_CNC.SelectedNode;
            TreeNode parent2 = selectedNode2.Parent;
            selectedNode2.Remove();
            this.treeView_CNC.SelectedNode = parent2;
            this.treeView_CNC.Refresh();
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void renombrarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.fichero_copy = (string[]) null;
        this.programa_copy = (string[]) null;
        switch (this.origen_click)
        {
          case "listView_PC":
            if (this.listView_PC.SelectedItems.Count != 1)
            {
              int num = (int) MessageBox.Show(Resource_Form_explorer.String1);
              break;
            }
            ListViewItem listViewItem1 = new ListViewItem();
            ListViewItem selectedItem1 = this.listView_PC.SelectedItems[0];
            this.fichero_rename = selectedItem1.Text;
            this.listView_PC.LabelEdit = true;
            selectedItem1.BeginEdit();
            break;
          case "listView_CNC":
            if (this.listView_CNC.FocusedItem == null)
            {
              int num = (int) MessageBox.Show(Resource_Form_listado_programas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            }
            if (this.listView_CNC.SelectedItems.Count != 1)
            {
              int num = (int) MessageBox.Show(Resource_Form_explorer.String1);
              break;
            }
            ListViewItem listViewItem2 = new ListViewItem();
            ListViewItem selectedItem2 = this.listView_CNC.SelectedItems[0];
            this.programa_rename = selectedItem2.Text;
            this.listView_CNC.LabelEdit = true;
            selectedItem2.BeginEdit();
            break;
          case "treeView_PC":
            if (this.treeView_PC.SelectedNode == null || this.treeView_PC.SelectedNode.Parent == null)
              break;
            TreeNode selectedNode1 = this.treeView_PC.SelectedNode;
            this.treeView_PC.LabelEdit = true;
            if (!selectedNode1.IsEditing)
              selectedNode1.BeginEdit();
            this.treeView_PC.Refresh();
            break;
          case "treeView_CNC":
            if (this.treeView_CNC.SelectedNode == null || this.treeView_CNC.SelectedNode.Parent == null)
              break;
            TreeNode selectedNode2 = this.treeView_CNC.SelectedNode;
            this.treeView_CNC.LabelEdit = true;
            if (selectedNode2.IsEditing)
              break;
            selectedNode2.BeginEdit();
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void listView_PC_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      try
      {
        string label = e.Label;
        if (this.fichero_rename == label)
          return;
        e.CancelEdit = true;
        string str = this.treeView_PC.SelectedNode.FullPath.ToString();
        File.Move(str + "\\" + this.fichero_rename, str + "\\" + label);
        this.listView_PC.SelectedItems[0].Text = label;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void listView_CNC_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      try
      {
        string label = e.Label;
        if (this.programa_rename == label)
          return;
        e.CancelEdit = true;
        if (this.cnc_e.Old_CNC == (ushort) 0)
        {
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_rename(this.hndl, (object) ("//" + this.treeView_CNC.SelectedNode.FullPath.ToString().Replace("\\", "/") + "/" + this.programa_rename), (object) label);
          this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_rename", this.ret_focas);
        }
        else
        {
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_renameprog(this.hndl, Convert.ToInt32(this.programa_rename.Substring(1)), Convert.ToInt32(label.Substring(1)));
          this.chequeo.chequeo_errores_focas(this.hndl, "cnc_renameprog", this.ret_focas);
        }
        this.cnc_e.Listar_Programas(this.treeView_CNC.SelectedNode, this.listView_CNC);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.fichero_copy = (string[]) null;
        this.programa_copy = (string[]) null;
        this.directorio_PC_copiar = (string) null;
        this.directorio_CNC_copiar = (string) null;
        this.directorio_PC_pegar = (string) null;
        this.directorio_CNC_pegar = (string) null;
        switch (this.origen_click)
        {
          case "listView_PC":
            this.fichero_copy = new string[this.listView_PC.SelectedItems.Count];
            for (int index = 0; index < this.listView_PC.SelectedItems.Count; ++index)
              this.fichero_copy[index] = this.listView_PC.SelectedItems[index].Text;
            this.directorio_PC_copiar = this.directorio_PC_ultimo;
            break;
          case "listView_CNC":
            this.programa_copy = new string[this.listView_CNC.SelectedItems.Count];
            for (int index = 0; index < this.listView_CNC.SelectedItems.Count; ++index)
              this.programa_copy[index] = this.listView_CNC.SelectedItems[index].Text;
            this.directorio_CNC_copiar = this.directorio_CNC_ultimo;
            break;
          default:
            int num = (int) MessageBox.Show(Resource_Form_explorer.String1);
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.tipo_datos = (short) 0;
        this.memoria_mensaje_sacado = false;
        if (this.fichero_copy == null && this.programa_copy == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_explorer.String5);
        }
        else if (this.fichero_copy != null && this.programa_copy != null)
        {
          int num2 = (int) MessageBox.Show("Error desconocido en COPY - PASTE");
        }
        else
        {
          switch (this.origen_click)
          {
            case "listView_PC":
            case "treeView_PC":
              if (this.directorio_PC_pegar == null)
                this.directorio_PC_pegar = this.directorio_PC_ultimo;
              if (this.fichero_copy != null && this.programa_copy == null)
              {
                for (int index = 0; index < this.fichero_copy.Length; ++index)
                {
                  string path;
                  if (this.directorio_PC_copiar == this.directorio_PC_pegar)
                  {
                    int num3 = this.fichero_copy[index].IndexOf(".");
                    path = num3 != -1 ? this.fichero_copy[index].Substring(0, num3) + "_COPY" + this.fichero_copy[index].Substring(num3, this.fichero_copy[index].Length - num3) : this.fichero_copy[index] + "_COPY";
                  }
                  else
                    path = this.fichero_copy[index];
                  if (path.Contains("\\"))
                  {
                    string fileName = Path.GetFileName(path);
                    File.Copy(this.fichero_copy[index], this.directorio_PC_pegar + fileName);
                  }
                  else
                    File.Copy(this.directorio_PC_copiar + this.fichero_copy[index], this.directorio_PC_pegar + path);
                }
              }
              if (this.fichero_copy != null || this.programa_copy == null)
                break;
              this.old_time = DateTime.UtcNow;
              int num4 = 0;
              for (int index = 0; index < this.programa_copy.Length; ++index)
              {
                num4 = this.transferir_datos_CNC_PC(this.programa_copy[index]);
                if (num4 != 0)
                  break;
              }
              if (num4 == 0)
              {
                int num5 = (int) MessageBox.Show(Resource_Form_explorer.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }
              break;
            case "listView_CNC":
            case "treeView_CNC":
              if (this.directorio_CNC_pegar == null)
                this.directorio_CNC_pegar = this.directorio_CNC_ultimo;
              if (this.fichero_copy != null && this.programa_copy == null)
              {
                this.old_time = DateTime.UtcNow;
                int num6 = 0;
                for (int index = 0; index < this.fichero_copy.Length; ++index)
                {
                  num6 = this.transferir_datos_PC_CNC(this.fichero_copy[index]);
                  if (num6 != 0)
                    break;
                }
                if (num6 != 0)
                  break;
                int num7 = (int) MessageBox.Show(Resource_Form_explorer.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                break;
              }
              if (this.fichero_copy != null || this.programa_copy == null)
                break;
              for (int index = 0; index < this.programa_copy.Length; ++index)
              {
                if (this.cnc_e.Old_CNC == (ushort) 0)
                {
                  string[] destinationArray = new string[this.programa_copy.Length];
                  Array.Copy((Array) this.programa_copy, (Array) destinationArray, this.programa_copy.Length);
                  if (this.directorio_CNC_copiar == this.directorio_CNC_pegar)
                  {
                    // ISSUE: explicit reference operation
                    ^ref destinationArray[index] += "_COPY";
                  }
                  this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_copy(this.hndl, (object) (this.directorio_CNC_copiar + this.programa_copy[index]), (object) (this.directorio_CNC_pegar + destinationArray[index]));
                  this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_copy", this.ret_focas);
                  if (this.ret_focas != 0)
                    break;
                }
                else
                {
                  Form_copiar_programa formCopiarPrograma = new Form_copiar_programa();
                  formCopiarPrograma.Programa_original = this.listView_CNC.FocusedItem.Text;
                  int num8 = (int) formCopiarPrograma.ShowDialog();
                  if (formCopiarPrograma.Nuevo_programa != "")
                  {
                    this.ret_focas = (Focas1.focas_ret) Focas1.cnc_copyprog(this.hndl, Convert.ToInt32(formCopiarPrograma.Programa_original.Substring(1)), Convert.ToInt32(formCopiarPrograma.Nuevo_programa.Substring(1)));
                    this.chequeo.chequeo_errores_focas(this.hndl, "cnc_copyprog", this.ret_focas);
                    if (this.ret_focas != 0)
                      break;
                  }
                }
              }
              break;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.fichero_copy = (string[]) null;
        this.programa_copy = (string[]) null;
        this.file_e.Listar_Ficheros(this.treeView_PC.SelectedNode, this.listView_PC);
        if (this.treeView_CNC.SelectedNode != null)
          this.cnc_e.Listar_Programas(this.treeView_CNC.SelectedNode, this.listView_CNC);
      }
    }

    private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.treeView_PC_visualizar_subfolders = false;
        this.treeView_CNC_visualizar_subfolders = false;
        switch (this.origen_click)
        {
          case "treeView_PC":
          case "listView_PC":
            if (this.treeView_PC.SelectedNode == null)
              break;
            string string13_1 = Resource_Form_explorer.String13;
            string path = this.treeView_PC.SelectedNode.FullPath.ToString();
            TreeNode selectedNode1 = this.treeView_PC.SelectedNode;
            int num1 = 1;
            while (true)
            {
              string13_1 += num1.ToString();
              if (Directory.Exists(path + "\\" + string13_1))
              {
                if (num1 <= 10)
                  ++num1;
                else
                  break;
              }
              else
                goto label_9;
            }
            int num2 = (int) MessageBox.Show(Resource_Form_explorer.String11);
            break;
label_9:
            new DirectoryInfo(path).CreateSubdirectory(string13_1);
            this.treeView_PC_visualizar_subfolders = false;
            TreeNode treeNode1 = selectedNode1.Nodes.Add(string13_1);
            treeNode1.EnsureVisible();
            this.treeView_PC.SelectedNode = treeNode1;
            this.treeView_PC.LabelEdit = true;
            if (treeNode1.IsEditing)
              break;
            treeNode1.BeginEdit();
            break;
          case "treeView_CNC":
          case "listView_CNC":
            if (this.treeView_CNC.SelectedNode == null)
              break;
            string string13_2 = Resource_Form_explorer.String13;
            string str1 = this.treeView_CNC.SelectedNode.FullPath.ToString();
            TreeNode selectedNode2 = this.treeView_CNC.SelectedNode;
            string str2 = "//" + str1.Replace("\\", "/") + "/";
            string a = str2 + string13_2 + "/";
            if (a.Contains("//DATA_SV/"))
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dsmkdir(this.hndl, (object) "DATA_SV", (object) (str2 + string13_2));
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dsmkdir", this.ret_focas) != 0)
                break;
            }
            else
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_add(this.hndl, (object) a);
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_add", this.ret_focas) != 0)
                break;
            }
            this.treeView_CNC_visualizar_subfolders = false;
            TreeNode treeNode2 = selectedNode2.Nodes.Add(string13_2);
            treeNode2.EnsureVisible();
            this.treeView_CNC.SelectedNode = treeNode2;
            this.treeView_CNC.LabelEdit = true;
            if (treeNode2.IsEditing)
              break;
            treeNode2.BeginEdit();
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.treeView_PC_visualizar_subfolders = true;
        this.treeView_CNC_visualizar_subfolders = true;
      }
    }

    private void treeView_PC_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      try
      {
        if (e.Label == null || e.Label.Length <= 0)
          e.CancelEdit = true;
        else if (e.Label.IndexOfAny(new char[4]
        {
          '@',
          '.',
          ',',
          '!'
        }) != -1)
        {
          int num = (int) MessageBox.Show(Resource_Form_explorer.String6);
          e.CancelEdit = true;
          this.treeView_PC.SelectedNode.BeginEdit();
        }
        else
        {
          string path = this.treeView_PC.SelectedNode.FullPath.ToString();
          int num1 = path.LastIndexOf("\\");
          string str = path.Substring(0, num1 + 1) + e.Label;
          DirectoryInfo directoryInfo = new DirectoryInfo(path);
          if (new DirectoryInfo(str).Exists)
          {
            int num2 = (int) MessageBox.Show(Resource_Form_explorer.String11);
            e.CancelEdit = true;
          }
          else
          {
            directoryInfo.MoveTo(str);
            this.directorio_PC_ultimo = str;
            if (this.directorio_PC_ultimo.EndsWith("\\"))
              return;
            this.directorio_PC_ultimo += "\\";
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void treeView_CNC_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      try
      {
        if (e.Label == null || e.Label.Length <= 0)
          e.CancelEdit = true;
        else if (e.Label.IndexOfAny(new char[4]
        {
          '@',
          '.',
          ',',
          '!'
        }) != -1)
        {
          int num = (int) MessageBox.Show(Resource_Form_explorer.String6);
          e.CancelEdit = true;
          this.treeView_CNC.SelectedNode.BeginEdit();
        }
        else
        {
          string str1 = this.treeView_CNC.SelectedNode.FullPath.ToString().Replace("\\", "/");
          string str2 = "//" + str1 + "/";
          string a = str2;
          string label = e.Label;
          if (str2.Contains("//DATA_SV/"))
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dsrename(this.hndl, (object) "DATA_SV", (object) ("//" + str1), (object) label);
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dsrename", this.ret_focas) != 0)
              return;
          }
          else
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_rename(this.hndl, (object) a, (object) label);
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_rename", this.ret_focas);
          }
          if (this.ret_focas != 0)
            e.CancelEdit = true;
          else
            this.directorio_CNC_ultimo = label;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void listView_PC_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.C)
      {
        this.origen_click = "listView_PC";
        this.copiarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.Control && e.KeyCode == Keys.V)
      {
        this.origen_click = "listView_PC";
        this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.KeyCode != Keys.Delete)
        return;
      this.origen_click = "listView_PC";
      this.borrarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void listView_CNC_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.C)
      {
        this.origen_click = "listView_CNC";
        this.copiarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.Control && e.KeyCode == Keys.V)
      {
        this.origen_click = "listView_CNC";
        this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.KeyCode != Keys.Delete)
        return;
      this.origen_click = "listView_CNC";
      this.borrarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void treeView_PC_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.C)
      {
        this.origen_click = "listView_PC";
        this.copiarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.Control && e.KeyCode == Keys.V)
      {
        this.origen_click = "listView_PC";
        this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.KeyCode != Keys.Delete)
        return;
      this.origen_click = "listView_PC";
      this.borrarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void treeView_CNC_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.V)
      {
        this.origen_click = "treeView_CNC";
        this.pegarToolStripMenuItem_Click((object) null, (EventArgs) null);
      }
      if (e.KeyCode != Keys.Delete)
        return;
      this.origen_click = "treeView_CNC";
      this.borrarToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void listView_PC_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.Enabled = false;
      Form_TextEditor formTextEditor = new Form_TextEditor();
      ListViewItem selectedItem = this.listView_PC.SelectedItems[0];
      formTextEditor.Fichero_editar = this.directorio_PC_ultimo + selectedItem.Text;
      int num = (int) formTextEditor.ShowDialog();
      this.Enabled = true;
      this.file_e.Listar_Ficheros(this.treeView_PC.SelectedNode, this.listView_PC);
    }

    private void listView_CNC_DoubleClick(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show(Resource_Form_explorer.String10);
    }

    private int transferir_datos_CNC_PC(string programa_recibir)
    {
      string str1 = programa_recibir;
      this.tipo_datos = (short) 0;
      if (this.directorio_CNC_copiar == "//CNC_DATA/")
      {
        programa_recibir = programa_recibir.ToUpper();
        switch (programa_recibir)
        {
          case "MACRO_VARIABLES.TXT":
            this.tipo_datos = (short) 4;
            break;
          case "MAINTENANCE INFO.TXT":
            this.tipo_datos = (short) 8;
            break;
          case "OPERAT_HISTORY.TXT":
            this.tipo_datos = (short) 7;
            break;
          case "PARAMETER.TXT":
            this.tipo_datos = (short) 2;
            break;
          case "PITCH_ERROR.TXT":
            this.tipo_datos = (short) 3;
            break;
          case "SYSTEM_INFO.TXT":
            this.tipo_datos = (short) 9;
            break;
          case "TOOL_OFFSET.TXT":
            this.tipo_datos = (short) 1;
            break;
          case "WORKPIECE OFFSET.TXT":
            this.tipo_datos = (short) 5;
            break;
          default:
            int num = (int) MessageBox.Show("Error desconocido en COPY - PASTE");
            return 1;
        }
      }
      if (this.tipo_datos == (short) 0)
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 != null)
        {
          string str2 = (string) registryKey1.GetValue("Machine");
          RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str2, true);
          if ((string) registryKey2.GetValue("Keep_program_number_checked") == "YES")
          {
            str1 = programa_recibir;
          }
          else
          {
            if ((string) registryKey2.GetValue("Change_O_Checked") == "YES")
            {
              string newValue = (string) registryKey2.GetValue("Replace_O");
              if (newValue != null && programa_recibir[0] == 'O')
                str1 = programa_recibir.Replace("O", newValue);
            }
            if ((string) registryKey2.GetValue("Add_extension_Checked") == "YES")
            {
              string str3 = (string) registryKey2.GetValue("Extension");
              if (str3 != null)
                str1 = str1 + "." + str3;
            }
          }
        }
      }
      string path = this.directorio_PC_pegar + str1;
      if (File.Exists(path) && !this.memoria_mensaje_sacado)
      {
        DialogResult dialogResult = MessageBox.Show(Resource_Form_recibir_ethernet.String2, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
        this.memoria_mensaje_sacado = true;
        if (dialogResult == DialogResult.Cancel)
          return 0;
      }
      FileStream fileStream;
      try
      {
        fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String3 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return 2;
      }
      if (this.cnc_e.Old_CNC == (ushort) 0)
      {
        try
        {
          if (programa_recibir == "")
          {
            int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String4);
            fileStream.Close();
            File.Delete(path);
            return 3;
          }
          Class_chequeo_errores_focas chequeoErroresFocas = new Class_chequeo_errores_focas();
          Cursor.Current = Cursors.WaitCursor;
          Focas1.focas_ret ret_focas = (Focas1.focas_ret) Focas1.cnc_upstart4(this.hndl, this.tipo_datos, (object) (this.directorio_CNC_copiar + programa_recibir));
          if (chequeoErroresFocas.chequeo_errores_focas(this.hndl, "cnc_upstart3", ret_focas) != 0)
          {
            fileStream.Close();
            File.Delete(path);
            return 10;
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          fileStream.Close();
          File.Delete(path);
          return 4;
        }
      }
      else
      {
        try
        {
          if (programa_recibir == "")
          {
            int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String4);
            fileStream.Close();
            File.Delete(path);
            return 3;
          }
          string str4 = "Oo";
          int b = 0;
          int c = 0;
          if (this.tipo_datos == (short) 0)
          {
            string str5 = programa_recibir;
            if ((int) str5[0] == (int) str4[0] || (int) str5[0] == (int) str4[1])
              str5 = programa_recibir.Substring(1);
            string str6 = str5;
            b = Convert.ToInt32(str5);
            c = Convert.ToInt32(str6);
          }
          Class_chequeo_errores_focas chequeoErroresFocas = new Class_chequeo_errores_focas();
          Cursor.Current = Cursors.WaitCursor;
          Focas1.focas_ret ret_focas = (Focas1.focas_ret) Focas1.cnc_upstart3(this.hndl, this.tipo_datos, b, c);
          if (chequeoErroresFocas.chequeo_errores_focas(this.hndl, "cnc_upstart3", ret_focas) != 0)
          {
            fileStream.Close();
            File.Delete(path);
            return 10;
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          fileStream.Close();
          File.Delete(path);
          return 4;
        }
      }
      int length = 1024;
      byte[] numArray1 = new byte[length];
      long num1 = 0;
      byte[] numArray2 = new byte[10240 + length];
      try
      {
        int count1 = 0;
        int count2;
        string str7;
        do
        {
          int a;
          Focas1.focas_ret ret_focas;
          do
          {
            Array.Clear((Array) numArray1, 0, numArray1.Length);
            a = length;
            ret_focas = (Focas1.focas_ret) Focas1.cnc_upload4(this.hndl, ref a, (object) numArray1);
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_upload3", ret_focas) != 0)
              goto label_60;
          }
          while (ret_focas == Focas1.focas_ret.EW_BUFFER);
          count2 = a;
          num1 += (long) count2;
          count1 += count2;
          Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, count1 - count2, count2);
          if (count1 >= 10240)
          {
            fileStream.Write(numArray2, 0, count1);
            count1 = 0;
          }
          str7 = "%";
        }
        while ((int) numArray1[count2 - 1] != (int) (byte) str7[0]);
        fileStream.Write(numArray2, 0, count1);
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show(Resource_Form_recibir_ethernet.String9 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        fileStream.Close();
        if (this.cnc_e.Old_CNC == (ushort) 0)
        {
          int num3 = (int) Focas1.cnc_upend4(this.hndl);
        }
        else
        {
          int num4 = (int) Focas1.cnc_upend3(this.hndl);
        }
        Cursor.Current = Cursors.Default;
        return 5;
      }
label_60:
      fileStream.Close();
      if (this.cnc_e.Old_CNC == (ushort) 0)
      {
        int num5 = (int) Focas1.cnc_upend4(this.hndl);
      }
      else
      {
        int num6 = (int) Focas1.cnc_upend3(this.hndl);
      }
      Cursor.Current = Cursors.Default;
      return 0;
    }

    private int transferir_datos_PC_CNC(string fichero_enviar)
    {
      string path = this.directorio_PC_copiar + fichero_enviar;
      if (fichero_enviar.Contains("\\"))
        path = fichero_enviar;
      FileStream fileStream;
      try
      {
        fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        if (fileStream.Length > 10000L)
        {
          if (new Informacion().leer_clave(true) <= 0)
          {
            int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            fileStream.Close();
            File.Delete(path);
            return 1;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String8 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return 2;
      }
      int length1 = 1024;
      byte[] numArray1 = new byte[length1];
      long length2 = fileStream.Length;
      try
      {
        if (this.directorio_CNC_pegar == "//CNC_DATA/")
        {
          fichero_enviar = fichero_enviar.ToUpper();
          if (fichero_enviar.Contains("TOOL"))
            fichero_enviar = "TOOL_OFFSET.TXT";
          if (fichero_enviar.Contains("WORK"))
            fichero_enviar = "WORKPIECE OFFSET.TXT";
          if (fichero_enviar.Contains("MACRO"))
            fichero_enviar = "MACRO_VARIABLES.TXT";
          if (fichero_enviar.Contains("PARAM"))
            fichero_enviar = "PARAMETER.TXT";
          if (fichero_enviar.Contains("PITCH"))
            fichero_enviar = "PITCH_ERROR.TXT";
          switch (fichero_enviar)
          {
            case "TOOL_OFFSET.TXT":
              this.tipo_datos = (short) 1;
              break;
            case "WORKPIECE OFFSET.TXT":
              this.tipo_datos = (short) 5;
              break;
            case "MACRO_VARIABLES.TXT":
              this.tipo_datos = (short) 4;
              break;
            case "PARAMETER.TXT":
              this.tipo_datos = (short) 2;
              break;
            case "PITCH_ERROR.TXT":
              this.tipo_datos = (short) 3;
              break;
            default:
              int num = (int) MessageBox.Show(Resource_Form_explorer.String9);
              return 3;
          }
        }
        if (this.cnc_e.Old_CNC == (ushort) 0)
        {
          Cursor.Current = Cursors.WaitCursor;
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnstart4(this.hndl, this.tipo_datos, (object) this.directorio_CNC_pegar);
          if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dwnstart3", this.ret_focas) != 0)
            return 10;
        }
        else
        {
          Cursor.Current = Cursors.WaitCursor;
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnstart3(this.hndl, this.tipo_datos);
          if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_dwnstart3", this.ret_focas) != 0)
            return 10;
        }
        byte[] numArray2 = new byte[1024];
label_33:
        Array.Clear((Array) numArray1, 0, length1);
        int num1 = 1024;
        int num2 = fileStream.Read(numArray1, 0, length1);
        if (num2 != 0)
        {
          if (num2 < num1)
            num1 = num2;
          int num3 = 0;
          while (num1 > num3)
          {
            int srcOffset = num3;
            int count = num1 - num3;
            Buffer.BlockCopy((Array) numArray1, srcOffset, (Array) numArray2, 0, count);
            int a = count;
            if (this.cnc_e.Old_CNC == (ushort) 0)
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_download4(this.hndl, ref a, (object) numArray2);
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_download3", this.ret_focas) != 0)
              {
                this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnend4(this.hndl);
                return 10;
              }
            }
            else
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_download3(this.hndl, ref a, (object) numArray2);
              if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_download3", this.ret_focas) != 0)
              {
                this.ret_focas = (Focas1.focas_ret) Focas1.cnc_dwnend3(this.hndl);
                return 10;
              }
            }
            if (this.ret_focas == Focas1.focas_ret.EW_OK)
              num3 += a;
          }
          goto label_33;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_enviar_ethernet.String5 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.ret_focas = this.cnc_e.Old_CNC != (ushort) 0 ? (Focas1.focas_ret) Focas1.cnc_dwnend3(this.hndl) : (Focas1.focas_ret) Focas1.cnc_dwnend4(this.hndl);
        fileStream.Close();
        return 5;
      }
      this.ret_focas = this.cnc_e.Old_CNC != (ushort) 0 ? (Focas1.focas_ret) Focas1.cnc_dwnend3(this.hndl) : (Focas1.focas_ret) Focas1.cnc_dwnend4(this.hndl);
      fileStream.Close();
      return 0;
    }

    private void Form_explorer_FormClosed(object sender, FormClosedEventArgs e)
    {
      int num = (int) Focas1.cnc_freelibhndl(this.hndl);
      this.Close();
    }

    private void listView_PC_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (this.listView_PC.Sorting != SortOrder.Ascending)
        this.listView_PC.Sorting = SortOrder.Ascending;
      else
        this.listView_PC.Sorting = SortOrder.Descending;
    }

    private void listView_CNC_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (this.listView_CNC.Sorting != SortOrder.Ascending)
        this.listView_CNC.Sorting = SortOrder.Ascending;
      else
        this.listView_CNC.Sorting = SortOrder.Descending;
    }

    private void reConectarCNCToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int num = (int) Focas1.cnc_freelibhndl(this.hndl);
      this.hndl = (ushort) 0;
      this.Form_explorer_Load((object) null, (EventArgs) null);
    }

    private void ConfiguracionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_config().ShowDialog();
      this.Enabled = true;
    }

    private void timer_reconexion_Tick(object sender, EventArgs e)
    {
      try
      {
        DateTime utcNow = DateTime.UtcNow;
        if ((int) (utcNow - this.old_time).TotalMinutes < 4)
          return;
        Focas1.ODBSYS a = new Focas1.ODBSYS();
        short num1 = Focas1.cnc_sysinfo(this.hndl, a);
        short num2 = Focas1.cnc_upstart3(this.hndl, (short) 1, 0, 0);
        short num3 = Focas1.cnc_upend3(this.hndl);
        if (num1 == (short) 0 && num2 == (short) 0 && num3 == (short) 0)
        {
          this.label_tree_CNC.Text = "CNC : " + (new string(a.cnc_type) + new string(a.mt_type)) + " (" + this.machine_name + ")";
          this.old_time = utcNow;
        }
        else
        {
          if (num1 != (short) -16 && num2 != (short) -16 && num3 != (short) -16)
            return;
          int num4 = (int) Focas1.cnc_freelibhndl(this.hndl);
          this.hndl = (ushort) 0;
          this.timer_refresco_conexion.Enabled = false;
          int num5 = new Class_configurar_ethernet().obtener_handle();
          if (num5 <= 0)
          {
            this.label_tree_CNC.Text = this.label_tree_CNC.Tag.ToString();
          }
          else
          {
            this.hndl = (ushort) num5;
            this.timer_refresco_conexion.Enabled = true;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Problem : " + ex.Message);
      }
    }

    private void ajustesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_ajustes_explorador().ShowDialog();
      this.Enabled = true;
    }

    private void select_todo_toolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        switch (this.origen_click)
        {
          case "listView_PC":
            IEnumerator enumerator1 = this.listView_PC.Items.GetEnumerator();
            try
            {
              while (enumerator1.MoveNext())
                ((ListViewItem) enumerator1.Current).Selected = true;
              break;
            }
            finally
            {
              if (enumerator1 is IDisposable disposable)
                disposable.Dispose();
            }
          case "treeView_PC":
            this.listView_PC.Focus();
            foreach (ListViewItem listViewItem in this.listView_PC.Items)
              listViewItem.Selected = true;
            this.origen_click = "listView_PC";
            break;
          case "listView_CNC":
            IEnumerator enumerator2 = this.listView_CNC.Items.GetEnumerator();
            try
            {
              while (enumerator2.MoveNext())
                ((ListViewItem) enumerator2.Current).Selected = true;
              break;
            }
            finally
            {
              if (enumerator2 is IDisposable disposable)
                disposable.Dispose();
            }
          case "treeView_CNC":
            this.listView_CNC.Focus();
            foreach (ListViewItem listViewItem in this.listView_CNC.Items)
              listViewItem.Selected = true;
            this.origen_click = "listView_CNC";
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void Form_explorer_FormClosing(object sender, FormClosingEventArgs e)
    {
      Settings.Default.CNC_Explorer_state = this.WindowState;
      if (this.WindowState == FormWindowState.Normal)
      {
        Settings.Default.CNC_Explorer_location = this.Location;
        Settings.Default.CNC_Explorer_size = this.Size;
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        Settings settings1 = Settings.Default;
        Rectangle restoreBounds = this.RestoreBounds;
        Point location = restoreBounds.Location;
        settings1.CNC_Explorer_location = location;
        // ISSUE: variable of a compiler-generated type
        Settings settings2 = Settings.Default;
        restoreBounds = this.RestoreBounds;
        Size size = restoreBounds.Size;
        settings2.CNC_Explorer_size = size;
      }
      Settings.Default.Save();
    }

    private void EditToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(this.origen_click == "listView_PC") || this.listView_PC.SelectedItems.Count <= 0)
        return;
      this.Enabled = false;
      Form_TextEditor formTextEditor = new Form_TextEditor();
      ListViewItem selectedItem = this.listView_PC.SelectedItems[0];
      formTextEditor.Fichero_editar = this.directorio_PC_ultimo + selectedItem.Text;
      int num = (int) formTextEditor.ShowDialog();
      this.Enabled = true;
      this.file_e.Listar_Ficheros(this.treeView_PC.SelectedNode, this.listView_PC);
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      if (this.origen_click == "listView_PC")
      {
        this.contextMenuStrip1.Items[10].Visible = true;
        this.contextMenuStrip1.Refresh();
      }
      else
      {
        this.contextMenuStrip1.Items[10].Visible = false;
        this.contextMenuStrip1.Refresh();
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
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_explorer));
      this.splitContainer1 = new SplitContainer();
      this.splitContainer2 = new SplitContainer();
      this.treeView_PC = new TreeView();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.borrarToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.renombrarToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.copiarToolStripMenuItem = new ToolStripMenuItem();
      this.pegarToolStripMenuItem = new ToolStripMenuItem();
      this.select_todo_toolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.nuevoToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator5 = new ToolStripSeparator();
      this.EditToolStripMenuItem = new ToolStripMenuItem();
      this.imageList1 = new ImageList(this.components);
      this.panel1 = new Panel();
      this.label_tree_PC = new Label();
      this.listView_PC = new ListView();
      this.columnHeaderPC0 = new ColumnHeader();
      this.columnHeaderPC1 = new ColumnHeader();
      this.columnHeaderPC4 = new ColumnHeader();
      this.columnHeaderPC2 = new ColumnHeader();
      this.columnHeaderPC3 = new ColumnHeader();
      this.panel2 = new Panel();
      this.label_ficheros_PC = new Label();
      this.splitContainer3 = new SplitContainer();
      this.treeView_CNC = new TreeView();
      this.panel3 = new Panel();
      this.label_tree_CNC = new Label();
      this.listView_CNC = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.panel4 = new Panel();
      this.label_listado_programas = new Label();
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.ajustesToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.ConfiguracionToolStripMenuItem = new ToolStripMenuItem();
      this.reConectarCNCToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripMenuItem();
      this.toolStripMenuItem3 = new ToolStripMenuItem();
      this.toolStripMenuItem4 = new ToolStripMenuItem();
      this.toolStripMenuItem5 = new ToolStripMenuItem();
      this.toolStripMenuItem6 = new ToolStripMenuItem();
      this.toolStripMenuItem7 = new ToolStripMenuItem();
      this.toolStripMenuItem8 = new ToolStripMenuItem();
      this.timer_refresco_conexion = new Timer(this.components);
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.splitContainer3.BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
      this.splitContainer1.BackColor = SystemColors.Control;
      this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer3);
      this.splitContainer2.BorderStyle = BorderStyle.Fixed3D;
      componentResourceManager.ApplyResources((object) this.splitContainer2, "splitContainer2");
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Panel1.Controls.Add((Control) this.treeView_PC);
      this.splitContainer2.Panel1.Controls.Add((Control) this.panel1);
      this.splitContainer2.Panel2.Controls.Add((Control) this.listView_PC);
      this.splitContainer2.Panel2.Controls.Add((Control) this.panel2);
      this.treeView_PC.AllowDrop = true;
      this.treeView_PC.ContextMenuStrip = this.contextMenuStrip1;
      componentResourceManager.ApplyResources((object) this.treeView_PC, "treeView_PC");
      this.treeView_PC.HideSelection = false;
      this.treeView_PC.ImageList = this.imageList1;
      this.treeView_PC.Name = "treeView_PC";
      this.treeView_PC.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_PC_AfterLabelEdit);
      this.treeView_PC.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_PC_BeforeExpand);
      this.treeView_PC.BeforeSelect += new TreeViewCancelEventHandler(this.treeView_PC_BeforeSelect);
      this.treeView_PC.DragDrop += new DragEventHandler(this.treeView_PC_DragDrop);
      this.treeView_PC.DragEnter += new DragEventHandler(this.treeView_PC_DragEnter);
      this.treeView_PC.KeyUp += new KeyEventHandler(this.treeView_PC_KeyUp);
      this.treeView_PC.MouseDown += new MouseEventHandler(this.treeView_PC_MouseDown);
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.borrarToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.renombrarToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.copiarToolStripMenuItem,
        (ToolStripItem) this.pegarToolStripMenuItem,
        (ToolStripItem) this.select_todo_toolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.nuevoToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator5,
        (ToolStripItem) this.EditToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.borrarToolStripMenuItem.Image = (Image) Resources.DSK3_36F;
      this.borrarToolStripMenuItem.Name = "borrarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.borrarToolStripMenuItem, "borrarToolStripMenuItem");
      this.borrarToolStripMenuItem.Click += new EventHandler(this.borrarToolStripMenuItem_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator2, "toolStripSeparator2");
      this.renombrarToolStripMenuItem.Image = (Image) Resources.CLIPB14D;
      this.renombrarToolStripMenuItem.Name = "renombrarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.renombrarToolStripMenuItem, "renombrarToolStripMenuItem");
      this.renombrarToolStripMenuItem.Click += new EventHandler(this.renombrarToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator3, "toolStripSeparator3");
      this.copiarToolStripMenuItem.Image = (Image) Resources.POINT18A;
      this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.copiarToolStripMenuItem, "copiarToolStripMenuItem");
      this.copiarToolStripMenuItem.Click += new EventHandler(this.copiarToolStripMenuItem_Click);
      this.pegarToolStripMenuItem.Image = (Image) Resources.POINT18B;
      this.pegarToolStripMenuItem.Name = "pegarToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.pegarToolStripMenuItem, "pegarToolStripMenuItem");
      this.pegarToolStripMenuItem.Click += new EventHandler(this.pegarToolStripMenuItem_Click);
      this.select_todo_toolStripMenuItem.Name = "select_todo_toolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.select_todo_toolStripMenuItem, "select_todo_toolStripMenuItem");
      this.select_todo_toolStripMenuItem.Click += new EventHandler(this.select_todo_toolStripMenuItem_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator4, "toolStripSeparator4");
      this.nuevoToolStripMenuItem.Image = (Image) Resources.FOLDER10;
      this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.nuevoToolStripMenuItem, "nuevoToolStripMenuItem");
      this.nuevoToolStripMenuItem.Click += new EventHandler(this.nuevoToolStripMenuItem_Click);
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator5, "toolStripSeparator5");
      componentResourceManager.ApplyResources((object) this.EditToolStripMenuItem, "EditToolStripMenuItem");
      this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
      this.EditToolStripMenuItem.Click += new EventHandler(this.EditToolStripMenuItem_Click);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = System.Drawing.Color.White;
      this.imageList1.Images.SetKeyName(0, "FOLDER.ICO");
      this.imageList1.Images.SetKeyName(1, "DVDFolderXP.ico");
      this.imageList1.Images.SetKeyName(2, "DOCL.ICO");
      this.imageList1.Images.SetKeyName(3, "Floppy.ico");
      this.imageList1.Images.SetKeyName(4, "HD.ICO");
      this.imageList1.Images.SetKeyName(5, "Net_drive.ico");
      this.imageList1.Images.SetKeyName(6, "CNC.ico");
      this.panel1.Controls.Add((Control) this.label_tree_PC);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      this.label_tree_PC.BackColor = SystemColors.GradientActiveCaption;
      this.label_tree_PC.BorderStyle = BorderStyle.Fixed3D;
      componentResourceManager.ApplyResources((object) this.label_tree_PC, "label_tree_PC");
      this.label_tree_PC.FlatStyle = FlatStyle.Flat;
      this.label_tree_PC.Name = "label_tree_PC";
      this.listView_PC.AllowDrop = true;
      this.listView_PC.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeaderPC0,
        this.columnHeaderPC1,
        this.columnHeaderPC4,
        this.columnHeaderPC2,
        this.columnHeaderPC3
      });
      this.listView_PC.ContextMenuStrip = this.contextMenuStrip1;
      componentResourceManager.ApplyResources((object) this.listView_PC, "listView_PC");
      this.listView_PC.HideSelection = false;
      this.listView_PC.Name = "listView_PC";
      this.listView_PC.SmallImageList = this.imageList1;
      this.listView_PC.Sorting = SortOrder.Ascending;
      this.listView_PC.UseCompatibleStateImageBehavior = false;
      this.listView_PC.View = View.Details;
      this.listView_PC.AfterLabelEdit += new LabelEditEventHandler(this.listView_PC_AfterLabelEdit);
      this.listView_PC.ColumnClick += new ColumnClickEventHandler(this.listView_PC_ColumnClick);
      this.listView_PC.ItemDrag += new ItemDragEventHandler(this.listView_PC_ItemDrag);
      this.listView_PC.DragDrop += new DragEventHandler(this.listView_PC_DragDrop);
      this.listView_PC.DragEnter += new DragEventHandler(this.listView_PC_DragEnter);
      this.listView_PC.DragOver += new DragEventHandler(this.listView_PC_DragOver);
      this.listView_PC.KeyUp += new KeyEventHandler(this.listView_PC_KeyUp);
      this.listView_PC.MouseDoubleClick += new MouseEventHandler(this.listView_PC_MouseDoubleClick);
      this.listView_PC.MouseDown += new MouseEventHandler(this.listView_PC_MouseDown);
      componentResourceManager.ApplyResources((object) this.columnHeaderPC0, "columnHeaderPC0");
      componentResourceManager.ApplyResources((object) this.columnHeaderPC1, "columnHeaderPC1");
      componentResourceManager.ApplyResources((object) this.columnHeaderPC4, "columnHeaderPC4");
      componentResourceManager.ApplyResources((object) this.columnHeaderPC2, "columnHeaderPC2");
      componentResourceManager.ApplyResources((object) this.columnHeaderPC3, "columnHeaderPC3");
      this.panel2.Controls.Add((Control) this.label_ficheros_PC);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      this.label_ficheros_PC.BackColor = SystemColors.GradientActiveCaption;
      this.label_ficheros_PC.BorderStyle = BorderStyle.Fixed3D;
      this.label_ficheros_PC.ContextMenuStrip = this.contextMenuStrip1;
      componentResourceManager.ApplyResources((object) this.label_ficheros_PC, "label_ficheros_PC");
      this.label_ficheros_PC.FlatStyle = FlatStyle.Flat;
      this.label_ficheros_PC.Name = "label_ficheros_PC";
      this.splitContainer3.BorderStyle = BorderStyle.Fixed3D;
      componentResourceManager.ApplyResources((object) this.splitContainer3, "splitContainer3");
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Panel1.Controls.Add((Control) this.treeView_CNC);
      this.splitContainer3.Panel1.Controls.Add((Control) this.panel3);
      this.splitContainer3.Panel2.Controls.Add((Control) this.listView_CNC);
      this.splitContainer3.Panel2.Controls.Add((Control) this.panel4);
      this.treeView_CNC.AllowDrop = true;
      this.treeView_CNC.ContextMenuStrip = this.contextMenuStrip1;
      componentResourceManager.ApplyResources((object) this.treeView_CNC, "treeView_CNC");
      this.treeView_CNC.HideSelection = false;
      this.treeView_CNC.ImageList = this.imageList1;
      this.treeView_CNC.Name = "treeView_CNC";
      this.treeView_CNC.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_CNC_AfterLabelEdit);
      this.treeView_CNC.AfterCollapse += new TreeViewEventHandler(this.treeView_CNC_AfterCollapse);
      this.treeView_CNC.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_CNC_BeforeExpand);
      this.treeView_CNC.BeforeSelect += new TreeViewCancelEventHandler(this.treeView_CNC_BeforeSelect);
      this.treeView_CNC.DragDrop += new DragEventHandler(this.treeView_CNC_DragDrop);
      this.treeView_CNC.DragEnter += new DragEventHandler(this.treeView_CNC_DragEnter);
      this.treeView_CNC.KeyUp += new KeyEventHandler(this.treeView_CNC_KeyUp);
      this.treeView_CNC.MouseDown += new MouseEventHandler(this.treeView_CNC_MouseDown);
      this.panel3.Controls.Add((Control) this.label_tree_CNC);
      componentResourceManager.ApplyResources((object) this.panel3, "panel3");
      this.panel3.Name = "panel3";
      this.label_tree_CNC.BackColor = System.Drawing.Color.Lime;
      this.label_tree_CNC.BorderStyle = BorderStyle.Fixed3D;
      componentResourceManager.ApplyResources((object) this.label_tree_CNC, "label_tree_CNC");
      this.label_tree_CNC.FlatStyle = FlatStyle.Flat;
      this.label_tree_CNC.Name = "label_tree_CNC";
      this.label_tree_CNC.Tag = (object) "CNC : No Connection";
      this.listView_CNC.AllowDrop = true;
      this.listView_CNC.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.listView_CNC.ContextMenuStrip = this.contextMenuStrip1;
      componentResourceManager.ApplyResources((object) this.listView_CNC, "listView_CNC");
      this.listView_CNC.HideSelection = false;
      this.listView_CNC.Name = "listView_CNC";
      this.listView_CNC.SmallImageList = this.imageList1;
      this.listView_CNC.Sorting = SortOrder.Ascending;
      this.listView_CNC.UseCompatibleStateImageBehavior = false;
      this.listView_CNC.View = View.Details;
      this.listView_CNC.AfterLabelEdit += new LabelEditEventHandler(this.listView_CNC_AfterLabelEdit);
      this.listView_CNC.ColumnClick += new ColumnClickEventHandler(this.listView_CNC_ColumnClick);
      this.listView_CNC.ItemDrag += new ItemDragEventHandler(this.listView_CNC_ItemDrag);
      this.listView_CNC.DragDrop += new DragEventHandler(this.listView_CNC_DragDrop);
      this.listView_CNC.DragEnter += new DragEventHandler(this.listView_CNC_DragEnter);
      this.listView_CNC.DragOver += new DragEventHandler(this.listView_CNC_DragOver);
      this.listView_CNC.DoubleClick += new EventHandler(this.listView_CNC_DoubleClick);
      this.listView_CNC.KeyUp += new KeyEventHandler(this.listView_CNC_KeyUp);
      this.listView_CNC.MouseDown += new MouseEventHandler(this.listView_CNC_MouseDown);
      componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
      componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
      componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
      componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
      componentResourceManager.ApplyResources((object) this.columnHeader5, "columnHeader5");
      this.panel4.Controls.Add((Control) this.label_listado_programas);
      componentResourceManager.ApplyResources((object) this.panel4, "panel4");
      this.panel4.Name = "panel4";
      this.label_listado_programas.BackColor = System.Drawing.Color.Lime;
      this.label_listado_programas.BorderStyle = BorderStyle.Fixed3D;
      componentResourceManager.ApplyResources((object) this.label_listado_programas, "label_listado_programas");
      this.label_listado_programas.FlatStyle = FlatStyle.Flat;
      this.label_listado_programas.Name = "label_listado_programas";
      this.label_listado_programas.Tag = (object) "";
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.ConfiguracionToolStripMenuItem,
        (ToolStripItem) this.reConectarCNCToolStripMenuItem
      });
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.Name = "menuStrip1";
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.ajustesToolStripMenuItem,
        (ToolStripItem) this.salirToolStripMenuItem
      });
      this.ficherosToolStripMenuItem.Image = (Image) Resources.Explorer_2;
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator1, "toolStripSeparator1");
      componentResourceManager.ApplyResources((object) this.ajustesToolStripMenuItem, "ajustesToolStripMenuItem");
      this.ajustesToolStripMenuItem.Name = "ajustesToolStripMenuItem";
      this.ajustesToolStripMenuItem.Click += new EventHandler(this.ajustesToolStripMenuItem_Click);
      this.salirToolStripMenuItem.Image = (Image) Resources.ARROW05L;
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.ConfiguracionToolStripMenuItem, "ConfiguracionToolStripMenuItem");
      this.ConfiguracionToolStripMenuItem.Name = "ConfiguracionToolStripMenuItem";
      this.ConfiguracionToolStripMenuItem.Click += new EventHandler(this.ConfiguracionToolStripMenuItem_Click);
      this.reConectarCNCToolStripMenuItem.Image = (Image) Resources.CONNECT;
      this.reConectarCNCToolStripMenuItem.Name = "reConectarCNCToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.reConectarCNCToolStripMenuItem, "reConectarCNCToolStripMenuItem");
      this.reConectarCNCToolStripMenuItem.Click += new EventHandler(this.reConectarCNCToolStripMenuItem_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem1, "toolStripMenuItem1");
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem2, "toolStripMenuItem2");
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem3, "toolStripMenuItem3");
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem4, "toolStripMenuItem4");
      this.toolStripMenuItem5.Name = "toolStripMenuItem5";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem5, "toolStripMenuItem5");
      this.toolStripMenuItem6.Name = "toolStripMenuItem6";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem6, "toolStripMenuItem6");
      this.toolStripMenuItem7.Name = "toolStripMenuItem7";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem7, "toolStripMenuItem7");
      this.toolStripMenuItem8.Name = "toolStripMenuItem8";
      componentResourceManager.ApplyResources((object) this.toolStripMenuItem8, "toolStripMenuItem8");
      this.timer_refresco_conexion.Interval = 60000;
      this.timer_refresco_conexion.Tick += new EventHandler(this.timer_reconexion_Tick);
      this.AllowDrop = true;
      this.AutoScaleMode = AutoScaleMode.None;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.menuStrip1);
      this.KeyPreview = true;
      this.Name = nameof (Form_explorer);
      this.FormClosing += new FormClosingEventHandler(this.Form_explorer_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.Form_explorer_FormClosed);
      this.Load += new EventHandler(this.Form_explorer_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      this.splitContainer3.EndInit();
      this.splitContainer3.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public class NodeSorter : IComparer
    {
      public int Compare(object x, object y)
      {
        return string.Compare((x as TreeNode).Text, (y as TreeNode).Text);
      }
    }
  }
}
