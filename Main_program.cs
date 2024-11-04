// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Main_program
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using Microsoft.Win32;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  internal static class Main_program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Main_program.Seleccion_idioma();
      Application.Run((Form) new Form_main());
    }

    private static void Seleccion_idioma()
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (registryKey == null)
      {
        registryKey = Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config");
        if (registryKey == null)
        {
          int num = (int) MessageBox.Show("problemas al crear registro de idioma ");
          return;
        }
        registryKey.SetValue("Language", (object) "SPANISH");
      }
      switch ((string) registryKey.GetValue("Language"))
      {
        case null:
          int num1 = (int) MessageBox.Show("problema desconocido al seleccionar idioma");
          break;
        case "SPANISH":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
          break;
        case "ENGLISH":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
          break;
        case "FRENCH":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
          break;
        case "ITALIAN":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("it");
          break;
        case "RUSSIAN":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
          break;
        case "GERMAN":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
          break;
        case "PORTUGUESE":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt");
          break;
        case "CHINESE":
          Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-Hans");
          break;
      }
    }
  }
}
