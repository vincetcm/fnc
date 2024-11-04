// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Class_configurar_ethernet
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Focas_gj;
using Microsoft.Win32;
using System;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  internal class Class_configurar_ethernet
  {
    public int obtener_handle()
    {
      ushort FlibHndl = 0;
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
      {
        int num = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      string str1 = (string) registryKey1.GetValue("Machine");
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false);
      if (registryKey2 == null)
      {
        int num = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      try
      {
        string ip = (string) registryKey2.GetValue("IP_ETHERNET");
        string str2 = (string) registryKey2.GetValue("Port_ETHERNET");
        string str3 = (string) registryKey2.GetValue("Time_ETHERNET");
        ushort int16 = (ushort) Convert.ToInt16(str2);
        int int32 = Convert.ToInt32(str3);
        short num1 = Focas1.cnc_allclibhndl3((object) ip, int16, int32, out FlibHndl);
        if (num1 == (short) 0)
          return (int) FlibHndl;
        int num2 = (int) MessageBox.Show(Resource_configurar_ethernet.String2 + num1.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_configurar_ethernet.String3 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
    }
  }
}
