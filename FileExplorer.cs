// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.FileExplorer
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  internal class FileExplorer
  {
    public bool Crear_Tree(TreeView treeView)
    {
      bool flag = false;
      try
      {
        treeView.Nodes.Clear();
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
          TreeNode node = new TreeNode();
          node.ImageIndex = 0;
          node.SelectedImageIndex = 0;
          if (drive.DriveType == DriveType.Fixed)
          {
            node.ImageIndex = 4;
            node.SelectedImageIndex = 4;
          }
          if (drive.DriveType == DriveType.CDRom)
          {
            node.ImageIndex = 1;
            node.SelectedImageIndex = 1;
          }
          if (drive.DriveType == DriveType.Removable)
          {
            node.ImageIndex = 3;
            node.SelectedImageIndex = 3;
          }
          if (drive.DriveType == DriveType.Network)
          {
            node.ImageIndex = 5;
            node.SelectedImageIndex = 5;
          }
          node.Text = drive.Name;
          node.Name = node.Text;
          node.Nodes.Add("");
          treeView.Nodes.Add(node);
          flag = true;
        }
      }
      catch (Exception ex)
      {
        flag = false;
        int num = (int) MessageBox.Show("Error al leer el directorio: " + ex.Message);
      }
      return flag;
    }

    public void Visualizar_Subdirectorios_Treeview(TreeNode parentNode)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(parentNode.FullPath + "\\");
        parentNode.Nodes.Clear();
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        {
          TreeNode node = new TreeNode()
          {
            SelectedImageIndex = 0,
            ImageIndex = 0,
            Text = directory.Name
          };
          node.Name = node.Text;
          node.Nodes.Add("");
          parentNode.Nodes.Add(node);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error al listar el directorio: " + ex.Message);
      }
    }

    public void Listar_Ficheros(TreeNode parentNode, ListView listView)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(parentNode.FullPath + "\\");
        if (!directoryInfo.Exists)
          return;
        listView.Items.Clear();
        string str1 = "";
        foreach (FileInfo file in directoryInfo.GetFiles())
        {
          try
          {
            str1 = "";
            using (StreamReader streamReader = new StreamReader(file.FullName.ToString()))
            {
              for (int index = 1; index <= 5; ++index)
              {
                string str2 = streamReader.ReadLine();
                if (str2 != null && str2.Contains("(") && str2.Contains(")"))
                {
                  int num = str2.IndexOf("(");
                  int length = str2.IndexOf(")") - num - 1;
                  str1 = str2.Substring(num + 1, length);
                  break;
                }
              }
            }
          }
          catch
          {
          }
          listView.Items.Add(new ListViewItem()
          {
            Text = file.Name,
            SubItems = {
              file.Extension,
              str1,
              file.Length.ToString(),
              file.CreationTime.ToString()
            },
            ImageIndex = 2,
            StateImageIndex = 2
          });
        }
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey == null)
          return;
        string str3 = (string) registryKey.GetValue("Machine");
        Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str3, true).SetValue("Last_folder_PC", (object) parentNode.FullPath.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error al listar los ficheros: " + ex.Message);
      }
    }
  }
}
