// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_listado_programas
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
  public class Form_listado_programas : Form
  {
    private ushort hndl = 0;
    private string _numero_programa_start = "";
    private string _numero_programa_end = "";
    private string _origen_llamada = "";
    private int _selected_path = 0;
    private int modelo_cnc = 0;
    private Class_chequeo_errores_focas chequeo = new Class_chequeo_errores_focas();
    private Focas1.focas_ret ret_focas;
    private IContainer components = (IContainer) null;
    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fivherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private Button button_seleccionar;
    private Button button_borrar_programa;
    private Button button_renombrar;
    private Button button_copiar;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem borrarProgramaToolStripMenuItem;
    private ToolStripMenuItem renombrarProgramaToolStripMenuItem;
    private ToolStripMenuItem copiarProgramaToolStripMenuItem;
    private Label label1;
    private Label label_CNC;
    private Label label2;
    private Label label_numero_canales;
    private Label label4;
    private ComboBox comboBox_canal;
    private ToolStripMenuItem seleccionarTodosLosProgramasToolStripMenuItem;
    private Label label3;
    private Label label_directorio;
    private Button button_crear_directorio;

    public Form_listado_programas() => this.InitializeComponent();

    public string Numero_programa_start
    {
      get => this._numero_programa_start;
      set => this._numero_programa_start = value;
    }

    public string Numero_programa_end
    {
      get => this._numero_programa_end;
      set => this._numero_programa_end = value;
    }

    public string Origen_llamada
    {
      set => this._origen_llamada = value;
    }

    public int Canal_seleccionado
    {
      get => this._selected_path;
      set => this._selected_path = value;
    }

    private void Form_listado_programas_Load(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0)
      {
        if (informacion.leer_contador_uso(true) == -1)
          this.Close();
        if (informacion.leer_fecha_uso() == -1)
          this.Close();
      }
      if (this._origen_llamada == "recibir")
      {
        this.button_seleccionar.Visible = true;
        this.contextMenuStrip1.Items[3].Visible = true;
      }
      if (this.hndl == (ushort) 0)
      {
        int num = new Class_configurar_ethernet().obtener_handle();
        if (num <= 0)
          return;
        this.hndl = (ushort) num;
      }
      Focas1.ODBSYS a1 = new Focas1.ODBSYS();
      int num1 = (int) Focas1.cnc_sysinfo(this.hndl, a1);
      this.label_CNC.Text = new string(a1.cnc_type) + "-" + new string(a1.mt_type);
      this.comboBox_canal.Items.Clear();
      short a2;
      short b1;
      int num2 = (int) Focas1.cnc_getpath(this.hndl, out a2, out b1);
      for (int index = 1; index <= (int) b1; ++index)
        this.comboBox_canal.Items.Add((object) index.ToString());
      this.label_numero_canales.Text = b1.ToString();
      if (this._selected_path == 0)
        this._selected_path = (int) a2;
      if (this._selected_path != (int) a2 && Focas1.cnc_setpath(this.hndl, (short) this._selected_path) != (short) 0)
      {
        int num3 = (int) MessageBox.Show(Resource_Form_listado_programas.String1);
      }
      else
      {
        this.comboBox_canal.SelectedIndex = this._selected_path - 1;
        char[] b2 = new char[212];
        short num4 = Focas1.cnc_rdpdf_curdir(this.hndl, (short) 1, (object) b2);
        string str = new string(b2);
        if (num4 == (short) 0)
        {
          this.modelo_cnc = 1;
          Focas1.PRGDIR3 prgdiR3 = new Focas1.PRGDIR3();
          this.label_directorio.Text = str;
          this.button_crear_directorio.Visible = true;
        }
        else
        {
          this.modelo_cnc = 0;
          this.button_crear_directorio.Visible = false;
        }
        this.program_list();
      }
    }

    private void program_list()
    {
      short c1 = 1;
      int b1 = 0;
      short a1 = 2;
      this.listView1.Items.Clear();
      ListViewItem listViewItem1 = new ListViewItem();
      char[] chArray = new char[212];
      if (this.modelo_cnc == 1)
      {
        listViewItem1.Text = "<..>";
        listViewItem1.SubItems.Add("( Back Folder )");
        this.listView1.Items.Add(listViewItem1);
        Focas1.IDBPDFADIR b2 = new Focas1.IDBPDFADIR();
        b2.path = this.label_directorio.Text;
        b2.req_num = (short) 0;
        b2.type = (short) 1;
        b2.size_kind = (short) 1;
        while (true)
        {
          Focas1.ODBPDFADIR c2 = new Focas1.ODBPDFADIR();
          short a2 = 1;
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_rdpdf_alldir(this.hndl, ref a2, (object) b2, (object) c2);
          if (this.ret_focas == 0)
          {
            ++b2.req_num;
            if (a2 != (short) 0)
            {
              ListViewItem listViewItem2 = new ListViewItem();
              if (c2.data_kind == (short) 0)
              {
                listViewItem2.Text = "<" + c2.d_f.ToString() + ">";
                listViewItem2.SubItems.Add("( Folder )");
                this.listView1.Items.Add(listViewItem2);
              }
              else
              {
                listViewItem2.Text = c2.d_f.ToString();
                listViewItem2.SubItems.Add(c2.comment.ToString());
                listViewItem2.SubItems.Add(c2.size.ToString());
                listViewItem2.SubItems.Add(c2.day.ToString() + "-" + c2.mon.ToString() + "-" + c2.year.ToString());
                this.listView1.Items.Add(listViewItem2);
              }
            }
            else
              goto label_11;
          }
          else
            break;
        }
        return;
label_11:;
      }
      else
      {
        Focas1.PRGDIR3 d = new Focas1.PRGDIR3();
        while (true)
        {
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_rdprogdir3(this.hndl, a1, ref b1, ref c1, d);
          if (this.ret_focas == 0 && c1 != (short) 0)
          {
            this.listView1.Items.Add(new ListViewItem()
            {
              Text = "O" + d.dir1.number.ToString(),
              SubItems = {
                d.dir1.comment,
                d.dir1.length.ToString(),
                d.dir1.mdate.day.ToString() + "-" + d.dir1.mdate.month.ToString() + "-" + d.dir1.mdate.year.ToString()
              }
            });
            b1 = d.dir1.number + 1;
          }
          else
            break;
        }
      }
    }

    private void Form_listado_programas_FormClosing(object sender, FormClosingEventArgs e)
    {
      int num = (int) Focas1.cnc_freelibhndl(this.hndl);
    }

    private void salirToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      if (this.listView1.SelectedItems[0].Text.Substring(0, 1) != "<")
        return;
      if (this.listView1.SelectedItems[0].Text.Substring(0, 4) == "<..>")
      {
        if (this.label_directorio.Text == "//CNC_MEM/")
          return;
        string str = (string) null;
        if (this.label_directorio.Text[this.label_directorio.Text.Length - 1] == '/')
          str = this.label_directorio.Text.Substring(0, this.label_directorio.Text.Length - 1);
        int num = str.LastIndexOf("/");
        this.label_directorio.Text = str.Substring(0, num + 1);
        this.label_directorio.Refresh();
        this.program_list();
      }
      else
      {
        string text = this.listView1.SelectedItems[0].Text;
        string str = text.Substring(1, text.Length - 2);
        Label labelDirectorio = this.label_directorio;
        labelDirectorio.Text = labelDirectorio.Text + str + "/";
        this.label_directorio.Refresh();
        this.program_list();
      }
    }

    private void button_seleccionar_Click(object sender, EventArgs e)
    {
      try
      {
        string text1 = this.listView1.SelectedItems[0].Text;
        string text2 = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1].Text;
        if (text1.Substring(0, 1) == "<" || text2.Substring(0, 1) == "<")
        {
          int num = (int) MessageBox.Show(Resource_Form_listado_programas.String2);
        }
        else
        {
          this._numero_programa_start = text1;
          this._numero_programa_end = text2;
          this.Close();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_programas.String2);
      }
    }

    private void button_borrar_programa_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.modelo_cnc == 0)
        {
          if (MessageBox.Show(Resource_Form_listado_programas.String3, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            return;
          foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_delete(this.hndl, Convert.ToInt32(selectedItem.Text.Substring(1)));
            if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_delete", this.ret_focas) != 0)
              return;
          }
          this.Form_listado_programas_Load((object) null, (EventArgs) null);
        }
        if (this.modelo_cnc != 1 || MessageBox.Show(Resource_Form_listado_programas.String3, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
        {
          string str = selectedItem.Text;
          if (str == "<..>")
            return;
          if (str.Substring(0, 1) == "<")
            str = str.Substring(1, str.Length - 2) + "/";
          this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_del(this.hndl, (object) (this.label_directorio.Text + str));
          if (this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_del", this.ret_focas) != 0)
            return;
        }
        this.program_list();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_programas.String4 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void button_renombrar_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView1.FocusedItem == null)
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_programas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (this.modelo_cnc == 0)
          {
            this.Enabled = false;
            Form_renombrar_programa renombrarPrograma = new Form_renombrar_programa();
            renombrarPrograma.Programa_original = this.listView1.FocusedItem.Text;
            int num2 = (int) renombrarPrograma.ShowDialog();
            if (renombrarPrograma.Nuevo_programa != "")
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_renameprog(this.hndl, Convert.ToInt32(renombrarPrograma.Programa_original.Substring(1)), Convert.ToInt32(renombrarPrograma.Nuevo_programa.Substring(1)));
              this.chequeo.chequeo_errores_focas(this.hndl, "cnc_rename", this.ret_focas);
            }
            this.Enabled = true;
            this.Form_listado_programas_Load((object) null, (EventArgs) null);
          }
          if (this.modelo_cnc != 1)
            return;
          string str = this.listView1.SelectedItems[0].Text;
          if (str == "<..>")
            return;
          bool flag = false;
          if (str.Substring(0, 1) == "<")
          {
            str = str.Substring(1, str.Length - 2);
            flag = true;
          }
          this.Enabled = false;
          Form_renombrar_programa renombrarPrograma1 = new Form_renombrar_programa();
          renombrarPrograma1.Programa_original = str;
          renombrarPrograma1.Tipo_CNC = 1;
          int num3 = (int) renombrarPrograma1.ShowDialog();
          if (renombrarPrograma1.Nuevo_programa != "")
          {
            if (flag)
              str += "/";
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_rename(this.hndl, (object) (this.label_directorio.Text + str), (object) renombrarPrograma1.Nuevo_programa);
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_rename", this.ret_focas);
          }
          this.Enabled = true;
          this.program_list();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_programas.String4 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Close();
      }
    }

    private void button_copiar_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView1.FocusedItem == null || this.listView1.FocusedItem.Text.Substring(0, 1) == "<")
        {
          int num1 = (int) MessageBox.Show(Resource_Form_listado_programas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.Enabled = false;
          Form_copiar_programa formCopiarPrograma = new Form_copiar_programa();
          if (this.modelo_cnc == 0)
            formCopiarPrograma.Programa_original = this.listView1.FocusedItem.Text;
          if (this.modelo_cnc == 1)
            formCopiarPrograma.Programa_original = this.label_directorio.Text + this.listView1.SelectedItems[0].Text;
          formCopiarPrograma.Tipo_CNC = this.modelo_cnc;
          int num2 = (int) formCopiarPrograma.ShowDialog();
          if (formCopiarPrograma.Nuevo_programa != "")
          {
            if (this.modelo_cnc == 0)
            {
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_copyprog(this.hndl, Convert.ToInt32(formCopiarPrograma.Programa_original.Substring(1)), Convert.ToInt32(formCopiarPrograma.Nuevo_programa.Substring(1)));
              this.chequeo.chequeo_errores_focas(this.hndl, "cnc_copyprog", this.ret_focas);
            }
            if (this.modelo_cnc == 1)
            {
              string nuevoPrograma = formCopiarPrograma.Nuevo_programa;
              this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_copy(this.hndl, (object) formCopiarPrograma.Programa_original, (object) nuevoPrograma);
              this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_copy", this.ret_focas);
            }
          }
          this.Enabled = true;
          this.program_list();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_programas.String4 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Close();
      }
    }

    private void seleccionarProgramaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_seleccionar_Click((object) null, (EventArgs) null);
    }

    private void borrarProgramaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_borrar_programa_Click((object) null, (EventArgs) null);
    }

    private void renombrarProgramaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_renombrar_Click((object) null, (EventArgs) null);
    }

    private void copiarProgramaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.button_copiar_Click((object) null, (EventArgs) null);
    }

    private void comboBox_canal_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._selected_path = this.comboBox_canal.SelectedIndex + 1;
      if (Focas1.cnc_setpath(this.hndl, (short) this._selected_path) != (short) 0)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_programas.String5);
      }
      else
        this.program_list();
    }

    private void seleccionarTodosLosProgramasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.listView1.Items.Count <= 0)
          return;
        for (int index = 0; index < this.listView1.Items.Count; ++index)
          this.listView1.Items[index].Selected = true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_listado_programas.String4 + ex.Message);
      }
    }

    private void button_crear_directorio_Click(object sender, EventArgs e)
    {
      if (this.modelo_cnc == 0)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_listado_programas.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        try
        {
          this.Enabled = false;
          Form_cear_directorio formCearDirectorio = new Form_cear_directorio();
          int num2 = (int) formCearDirectorio.ShowDialog();
          if (formCearDirectorio.Nuevo_directorio != "")
          {
            this.ret_focas = (Focas1.focas_ret) Focas1.cnc_pdf_add(this.hndl, (object) (this.label_directorio.Text + formCearDirectorio.Nuevo_directorio + "/"));
            this.chequeo.chequeo_errores_focas(this.hndl, "cnc_pdf_add", this.ret_focas);
          }
          this.Enabled = true;
          this.program_list();
        }
        catch (Exception ex)
        {
          int num3 = (int) MessageBox.Show(Resource_Form_listado_programas.String4 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.Close();
        }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_listado_programas));
      this.listView1 = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.borrarProgramaToolStripMenuItem = new ToolStripMenuItem();
      this.renombrarProgramaToolStripMenuItem = new ToolStripMenuItem();
      this.copiarProgramaToolStripMenuItem = new ToolStripMenuItem();
      this.seleccionarTodosLosProgramasToolStripMenuItem = new ToolStripMenuItem();
      this.menuStrip1 = new MenuStrip();
      this.fivherosToolStripMenuItem = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.button_seleccionar = new Button();
      this.button_borrar_programa = new Button();
      this.button_renombrar = new Button();
      this.button_copiar = new Button();
      this.label1 = new Label();
      this.label_CNC = new Label();
      this.label2 = new Label();
      this.label_numero_canales = new Label();
      this.label4 = new Label();
      this.comboBox_canal = new ComboBox();
      this.label3 = new Label();
      this.label_directorio = new Label();
      this.button_crear_directorio = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.listView1, "listView1");
      this.listView1.Columns.AddRange(new ColumnHeader[4]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4
      });
      this.listView1.ContextMenuStrip = this.contextMenuStrip1;
      this.listView1.HideSelection = false;
      this.listView1.Name = "listView1";
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
      componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
      componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
      componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
      componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
      componentResourceManager.ApplyResources((object) this.contextMenuStrip1, "contextMenuStrip1");
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.borrarProgramaToolStripMenuItem,
        (ToolStripItem) this.renombrarProgramaToolStripMenuItem,
        (ToolStripItem) this.copiarProgramaToolStripMenuItem,
        (ToolStripItem) this.seleccionarTodosLosProgramasToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      componentResourceManager.ApplyResources((object) this.borrarProgramaToolStripMenuItem, "borrarProgramaToolStripMenuItem");
      this.borrarProgramaToolStripMenuItem.Name = "borrarProgramaToolStripMenuItem";
      this.borrarProgramaToolStripMenuItem.Click += new EventHandler(this.borrarProgramaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.renombrarProgramaToolStripMenuItem, "renombrarProgramaToolStripMenuItem");
      this.renombrarProgramaToolStripMenuItem.Name = "renombrarProgramaToolStripMenuItem";
      this.renombrarProgramaToolStripMenuItem.Click += new EventHandler(this.renombrarProgramaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.copiarProgramaToolStripMenuItem, "copiarProgramaToolStripMenuItem");
      this.copiarProgramaToolStripMenuItem.Name = "copiarProgramaToolStripMenuItem";
      this.copiarProgramaToolStripMenuItem.Click += new EventHandler(this.copiarProgramaToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.seleccionarTodosLosProgramasToolStripMenuItem, "seleccionarTodosLosProgramasToolStripMenuItem");
      this.seleccionarTodosLosProgramasToolStripMenuItem.Name = "seleccionarTodosLosProgramasToolStripMenuItem";
      this.seleccionarTodosLosProgramasToolStripMenuItem.Click += new EventHandler(this.seleccionarTodosLosProgramasToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.fivherosToolStripMenuItem
      });
      this.menuStrip1.Name = "menuStrip1";
      componentResourceManager.ApplyResources((object) this.fivherosToolStripMenuItem, "fivherosToolStripMenuItem");
      this.fivherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.salirToolStripMenuItem
      });
      this.fivherosToolStripMenuItem.Name = "fivherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.button_seleccionar, "button_seleccionar");
      this.button_seleccionar.AccessibleRole = AccessibleRole.None;
      this.button_seleccionar.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_seleccionar.Name = "button_seleccionar";
      this.button_seleccionar.UseVisualStyleBackColor = false;
      this.button_seleccionar.Click += new EventHandler(this.button_seleccionar_Click);
      componentResourceManager.ApplyResources((object) this.button_borrar_programa, "button_borrar_programa");
      this.button_borrar_programa.AccessibleRole = AccessibleRole.None;
      this.button_borrar_programa.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_borrar_programa.Name = "button_borrar_programa";
      this.button_borrar_programa.UseVisualStyleBackColor = false;
      this.button_borrar_programa.Click += new EventHandler(this.button_borrar_programa_Click);
      componentResourceManager.ApplyResources((object) this.button_renombrar, "button_renombrar");
      this.button_renombrar.AccessibleRole = AccessibleRole.None;
      this.button_renombrar.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_renombrar.Name = "button_renombrar";
      this.button_renombrar.UseVisualStyleBackColor = false;
      this.button_renombrar.Click += new EventHandler(this.button_renombrar_Click);
      componentResourceManager.ApplyResources((object) this.button_copiar, "button_copiar");
      this.button_copiar.AccessibleRole = AccessibleRole.None;
      this.button_copiar.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_copiar.Name = "button_copiar";
      this.button_copiar.UseVisualStyleBackColor = false;
      this.button_copiar.Click += new EventHandler(this.button_copiar_Click);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label_CNC, "label_CNC");
      this.label_CNC.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.label_CNC.BorderStyle = BorderStyle.Fixed3D;
      this.label_CNC.Name = "label_CNC";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label_numero_canales, "label_numero_canales");
      this.label_numero_canales.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.label_numero_canales.BorderStyle = BorderStyle.Fixed3D;
      this.label_numero_canales.Name = "label_numero_canales";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.comboBox_canal, "comboBox_canal");
      this.comboBox_canal.BackColor = Color.White;
      this.comboBox_canal.FormattingEnabled = true;
      this.comboBox_canal.Name = "comboBox_canal";
      this.comboBox_canal.SelectedIndexChanged += new EventHandler(this.comboBox_canal_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label_directorio, "label_directorio");
      this.label_directorio.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.label_directorio.BorderStyle = BorderStyle.Fixed3D;
      this.label_directorio.Name = "label_directorio";
      componentResourceManager.ApplyResources((object) this.button_crear_directorio, "button_crear_directorio");
      this.button_crear_directorio.AccessibleRole = AccessibleRole.None;
      this.button_crear_directorio.BackColor = Color.FromArgb(192, (int) byte.MaxValue, (int) byte.MaxValue);
      this.button_crear_directorio.Name = "button_crear_directorio";
      this.button_crear_directorio.UseVisualStyleBackColor = false;
      this.button_crear_directorio.Click += new EventHandler(this.button_crear_directorio_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.comboBox_canal);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label_CNC);
      this.Controls.Add((Control) this.button_crear_directorio);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label_directorio);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.listView1);
      this.Controls.Add((Control) this.button_copiar);
      this.Controls.Add((Control) this.label_numero_canales);
      this.Controls.Add((Control) this.button_renombrar);
      this.Controls.Add((Control) this.button_borrar_programa);
      this.Controls.Add((Control) this.button_seleccionar);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_listado_programas);
      this.FormClosing += new FormClosingEventHandler(this.Form_listado_programas_FormClosing);
      this.Load += new EventHandler(this.Form_listado_programas_Load);
      this.contextMenuStrip1.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
