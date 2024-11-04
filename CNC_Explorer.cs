// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.CNC_Explorer
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using Microsoft.Win32;
using System;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  internal class CNC_Explorer
  {
    private ushort _handle;
    private ushort _old_CNC;

    public ushort Handle
    {
      set => this._handle = value;
    }

    public ushort Old_CNC
    {
      get => this._old_CNC;
      set => value = this._old_CNC;
    }

    public bool Crear_Tree(TreeView treeView_CNC)
    {
      bool flag;
      try
      {
        treeView_CNC.Nodes.Clear();
        ushort handle = this._handle;
        Focas1.ODBPDFDRV a = new Focas1.ODBPDFDRV();
        TreeNode node1 = new TreeNode();
        TreeNode node2 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();
        TreeNode node5 = new TreeNode();
        TreeNode node6 = new TreeNode();
        TreeNode node7 = new TreeNode();
        node1.ImageIndex = 6;
        node1.SelectedImageIndex = 6;
        node2.ImageIndex = 6;
        node2.SelectedImageIndex = 6;
        node3.ImageIndex = 6;
        node3.SelectedImageIndex = 6;
        node4.ImageIndex = 6;
        node4.SelectedImageIndex = 6;
        node5.ImageIndex = 6;
        node5.SelectedImageIndex = 6;
        node6.ImageIndex = 6;
        node6.SelectedImageIndex = 6;
        node7.ImageIndex = 6;
        node7.SelectedImageIndex = 6;
        if (Focas1.cnc_rdpdf_drive(handle, (object) a) != (short) 0)
        {
          this._old_CNC = (ushort) 1;
          node2.Text = "CNC_MEM ";
          node2.Name = node2.Text;
          node2.Nodes.Add("");
          treeView_CNC.Nodes.Add(node2);
        }
        else
        {
          if (a.drive1 != "" && a.max_num >= (short) 1)
          {
            node2.Text = a.drive1;
            node2.Name = node2.Text;
            node2.Nodes.Add("");
            treeView_CNC.Nodes.Add(node2);
          }
          if (a.drive2 != "" && a.max_num >= (short) 2)
          {
            node3.Text = a.drive2;
            node3.Name = node3.Text;
            node3.Nodes.Add("");
            treeView_CNC.Nodes.Add(node3);
          }
          if (a.drive3 != "" && a.max_num >= (short) 3)
          {
            node4.Text = a.drive3;
            node4.Name = node4.Text;
            node4.Nodes.Add("");
            treeView_CNC.Nodes.Add(node4);
          }
          if (a.drive4 != "" && a.max_num >= (short) 4)
          {
            node5.Text = a.drive4;
            node5.Name = node5.Text;
            node5.Nodes.Add("");
            treeView_CNC.Nodes.Add(node5);
          }
          if (a.drive5 != "" && a.max_num >= (short) 5)
          {
            node6.Text = a.drive5;
            node6.Name = node6.Text;
            node6.Nodes.Add("");
            treeView_CNC.Nodes.Add(node6);
          }
          if (a.drive6 != "" && a.max_num >= (short) 6)
          {
            node7.Text = a.drive6;
            node7.Name = node7.Text;
            node7.Nodes.Add("");
            treeView_CNC.Nodes.Add(node7);
          }
        }
        node1.Text = "CNC_DATA";
        node1.Name = node1.Text;
        treeView_CNC.Nodes.Add(node1);
        flag = true;
      }
      catch (Exception ex)
      {
        flag = false;
        int num = (int) MessageBox.Show("Error at reading CNC sub-directories : " + ex.Message);
      }
      return flag;
    }

    public void Visualizar_Subdirectorios_Treeview(TreeNode parentNode)
    {
      if (parentNode == null)
        return;
      if (this._old_CNC == (ushort) 0)
      {
        try
        {
          parentNode.Nodes.Clear();
          ushort handle = this._handle;
          Focas1.IDBPDFSDIR b = new Focas1.IDBPDFSDIR();
          Focas1.ODBPDFSDIR c = new Focas1.ODBPDFSDIR();
          string str = "//" + parentNode.FullPath.ToString().Replace("\\", "/") + "/";
          b.path = str;
          b.req_num = (short) 0;
          while (true)
          {
            TreeNode node = new TreeNode();
            short a = 1;
            if (Focas1.cnc_rdpdf_subdir(handle, ref a, b, c) == (short) 0 && a != (short) 0)
            {
              node.Text = c.d_f;
              node.Name = node.Text;
              node.Nodes.Add("");
              parentNode.Nodes.Add(node);
              ++b.req_num;
            }
            else
              break;
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error at adding CNC sub-directory: " + ex.Message);
        }
      }
      else
      {
        try
        {
          parentNode.Nodes.Clear();
          short b;
          int num = (int) Focas1.cnc_getpath(this._handle, out short _, out b);
          if (b <= (short) 1)
            return;
          for (int index = 1; index <= (int) b; ++index)
            parentNode.Nodes.Add("PATH " + index.ToString());
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error at adding CNC sub-directory: " + ex.Message);
        }
      }
    }

    public void Listar_Programas(TreeNode parentNode, ListView listView)
    {
      if (parentNode.Text == "CNC_DATA")
      {
        listView.Items.Clear();
        ListViewItem listViewItem1 = new ListViewItem("TOOL_OFFSET.TXT", 2);
        ListViewItem listViewItem2 = new ListViewItem("WORKPIECE OFFSET.TXT", 2);
        ListViewItem listViewItem3 = new ListViewItem("MACRO_VARIABLES.TXT", 2);
        ListViewItem listViewItem4 = new ListViewItem("PARAMETER.TXT", 2);
        ListViewItem listViewItem5 = new ListViewItem("PITCH_ERROR.TXT", 2);
        ListViewItem listViewItem6 = new ListViewItem("SYSTEM_INFO.TXT", 2);
        ListViewItem listViewItem7 = new ListViewItem("OPERAT_HISTORY.TXT", 2);
        listView.Items.Add(listViewItem1);
        listView.Items.Add(listViewItem2);
        listView.Items.Add(listViewItem3);
        listView.Items.Add(listViewItem4);
        listView.Items.Add(listViewItem5);
        listView.Items.Add(listViewItem6);
        listView.Items.Add(listViewItem7);
      }
      else
      {
        int totalWidth = 8;
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
          return;
        string str1 = (string) registryKey1.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, true).GetValue("Selected_4_digit") == "YES")
          totalWidth = 4;
        if (this._old_CNC == (ushort) 0)
        {
          try
          {
            ushort handle = this._handle;
            Class_chequeo_errores_focas chequeoErroresFocas = new Class_chequeo_errores_focas();
            Focas1.focas_ret focasRet1 = Focas1.focas_ret.EW_OK;
            Focas1.IDBPDFADIR b = new Focas1.IDBPDFADIR();
            Focas1.ODBPDFADIR c = new Focas1.ODBPDFADIR();
            string str2 = parentNode.FullPath.ToString().Replace("\\", "/");
            string str3 = "//" + str2;
            if (!str2.EndsWith("/"))
              str3 += "/";
            b.path = str3;
            b.size_kind = (short) 1;
            b.type = (short) 1;
            b.req_num = (short) 0;
            listView.Items.Clear();
            StringBuilder a1 = new StringBuilder(244);
            focasRet1 = (Focas1.focas_ret) Focas1.cnc_pdf_rdmain(handle, (object) a1);
            while (true)
            {
              do
              {
                short a2 = 1;
                Focas1.focas_ret focasRet2 = (Focas1.focas_ret) Focas1.cnc_rdpdf_alldir(handle, ref a2, (object) b, (object) c);
                ++b.req_num;
                if (focasRet2 != Focas1.focas_ret.EW_OK && focasRet2 != Focas1.focas_ret.EW_NUMBER)
                {
                  int num = (int) MessageBox.Show("FOCAS Error: Communication Error?, Old CNC?");
                  goto label_24;
                }
                else if (a2 == (short) 0 || focasRet2 == Focas1.focas_ret.EW_NUMBER)
                  goto label_24;
              }
              while (c.data_kind == (short) 0);
              string str4 = c.d_f;
              if (str4[0] == 'O')
              {
                string s = str4.Substring(1);
                if (int.TryParse(s, out int _))
                  str4 = "O" + s.PadLeft(totalWidth, '0');
              }
              ListViewItem listViewItem = new ListViewItem("", 2);
              if (str3 + c.d_f == a1.ToString())
                listViewItem.ForeColor = System.Drawing.Color.Red;
              listViewItem.Text = str4;
              if ((c.attr & 8) == 8)
                listViewItem.SubItems.Add("(BIN)");
              else
                listViewItem.SubItems.Add("(PRG)");
              listViewItem.SubItems.Add(c.comment);
              listViewItem.SubItems.Add(c.size.ToString());
              string str5 = c.day.ToString() + "-" + c.mon.ToString() + "-" + c.year.ToString();
              string str6 = c.hour.ToString() + ":" + c.min.ToString() + ":" + c.sec.ToString();
              listViewItem.SubItems.Add(str5 + " (" + str6 + ")");
              listView.Items.Add(listViewItem);
            }
label_24:
            RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
            if (registryKey2 == null)
              return;
            string str7 = (string) registryKey2.GetValue("Machine");
            Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str7, true).SetValue("Last_folder_CNC", (object) parentNode.FullPath.ToString());
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Error at listing CNC programs: " + ex.Message);
          }
        }
        else
        {
          try
          {
            ushort handle = this._handle;
            if (parentNode.Text.StartsWith("PATH"))
            {
              int int16 = (int) Convert.ToInt16(parentNode.Text.Substring(4));
              Focas1.focas_ret focasRet = (Focas1.focas_ret) Focas1.cnc_setpath(handle, (short) int16);
              if (focasRet != 0)
              {
                int num = (int) MessageBox.Show("Focas1.cnc_setpath Error: " + focasRet.ToString());
                listView.Items.Clear();
                return;
              }
            }
            short c = 1;
            int b = 0;
            short a = 2;
            Focas1.PRGDIR3 d = new Focas1.PRGDIR3();
            listView.Items.Clear();
            for (; Focas1.cnc_rdprogdir3(handle, a, ref b, ref c, d) == (short) 0 && c != (short) 0; b = d.dir1.number + 1)
              listView.Items.Add(new ListViewItem()
              {
                Text = "O" + d.dir1.number.ToString(),
                SubItems = {
                  "PRG",
                  d.dir1.comment,
                  d.dir1.length.ToString(),
                  d.dir1.mdate.day.ToString() + "-" + d.dir1.mdate.month.ToString() + "-" + d.dir1.mdate.year.ToString()
                }
              });
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Error at listing CNC programs: " + ex.Message);
          }
        }
      }
    }
  }
}
