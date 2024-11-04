// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Informacion
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Menu_informacion;
using Microsoft.Win32;
using System;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  internal class Informacion
  {
    public const int CONTADOR_TEMPORAL = -60;
    public const int CONTADOR_USO = -200;
    public const int CONTADOR_AVISO = -30;
    public const int DIAS_AVISO = 150;
    public const int DIAS_USO = 180;
    public const int LIMITE_DATOS_ENVIADOS = 10000;
    public const int MAXIMO_MAQUINAS = 20;
    public const string DISTRIBUIDO_POR = "";
    public const string VERSION = "Vers.94_2";
    private string str_os_version = "V11";
    private string str_nombre_PC = "V22";
    private string str_version_clr = "V33";
    private long numero_serie = 0;
    private string str_version_programa = "Vers.94_2";
    private string str_operatividad_programa = "";

    public string Str_os_version => this.str_os_version;

    public string Str_nombre_PC => this.str_nombre_PC;

    public string Str_version_clr => this.str_version_clr;

    public string Str_version_programa => this.str_version_programa;

    public long Numero_serie => this.numero_serie;

    public string Str_operatividad_programa => this.str_operatividad_programa;

    public Informacion()
    {
      try
      {
        this.str_os_version = Environment.OSVersion.ToString();
      }
      catch (Exception ex)
      {
        this.str_os_version = "V11";
      }
      try
      {
        this.str_nombre_PC = Environment.MachineName.ToString();
      }
      catch (Exception ex)
      {
        this.str_nombre_PC = "V22";
      }
      try
      {
        this.str_version_clr = Environment.Version.ToString();
      }
      catch (Exception ex)
      {
        this.str_version_clr = "V33";
      }
      Regex regex = new Regex("[0-9]");
      string str1 = "";
      string str2 = "";
      string str3 = "";
      int startat1 = 0;
      Match match1;
      do
      {
        match1 = regex.Match(this.str_os_version, startat1);
        str1 += match1.ToString();
        startat1 = match1.Index + 1;
      }
      while (match1.Length != 0);
      int startat2 = 0;
      Match match2;
      do
      {
        match2 = regex.Match(this.str_nombre_PC, startat2);
        str2 += match2.ToString();
        startat2 = match2.Index + 1;
      }
      while (match2.Length != 0);
      int startat3 = 0;
      Match match3;
      do
      {
        match3 = regex.Match(this.str_version_clr, startat3);
        str3 += match3.ToString();
        startat3 = match3.Index + 1;
      }
      while (match3.Length != 0);
      if (str1 == "")
        str1 = "11";
      if (str2 == "")
        str2 = "22";
      if (this.str_version_clr == "")
        this.str_version_clr = "33";
      try
      {
        Convert.ToInt64(str1);
        long num1 = Convert.ToInt64(str2);
        Convert.ToInt64(str3);
        long length1 = (long) this.str_os_version.Length;
        long num2 = (long) this.str_nombre_PC.Length;
        long length2 = (long) this.str_version_clr.Length;
        long num3 = 0;
        for (int index = 0; index < this.str_nombre_PC.Length; ++index)
        {
          int num4 = (int) this.str_nombre_PC[index];
          num3 += (long) num4;
        }
        string input = string.Empty;
        try
        {
          foreach (ManagementObject instance in new ManagementClass("win32_processor").GetInstances())
          {
            if (input == "")
            {
              input = instance.Properties["processorID"].Value.ToString();
              break;
            }
          }
        }
        catch
        {
          input = "1234";
        }
        string str4 = (string) null;
        int startat4 = 0;
        Match match4;
        do
        {
          match4 = regex.Match(input, startat4);
          str4 += match4.ToString();
          startat4 = match4.Index + 1;
        }
        while (match4.Length != 0);
        long num5 = Convert.ToInt64(str4);
        long num6 = (long) this.str_nombre_PC.Length;
        long num7 = 0;
        for (int index = 0; index < input.Length; ++index)
        {
          int num8 = (int) input[index];
          num7 += (long) num8;
        }
        if (num2 == 0L)
          num2 = 11L;
        if (num1 == 0L)
          num1 = 1234L;
        if (num3 == 0L)
          num3 = 13L;
        if (num6 == 0L)
          num6 = 11L;
        if (num5 == 0L)
          num5 = 1234L;
        if (num7 == 0L)
          num7 = 13L;
        this.numero_serie = num2 * num1 * num3 + num6 * num5 * num7;
        if (this.numero_serie < 9999L)
          this.numero_serie *= 123456L;
        if (this.numero_serie > 999999999999L)
          this.numero_serie /= 123456L;
        int num9 = this.leer_clave(false);
        if (num9 == 10)
          this.str_operatividad_programa = Resource_Informacion.String1;
        if (num9 == 1)
          this.str_operatividad_programa = Resource_Informacion.String2;
        if (num9 > 0)
          return;
        this.str_operatividad_programa = Resource_Informacion.String3;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Informacion.String4 + ex.Message, " ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public int leer_clave(bool incremento)
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\P", true);
      RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\GJKE", true);
      if (registryKey1 == null)
        return 0;
      try
      {
        long int64 = Convert.ToInt64(registryKey1.GetValue("PW"));
        if (int64 == this.numero_serie * 5L / 3L + 10101960L)
          return 10;
        if (int64 == this.numero_serie * 3L / 2L + 19601010L)
        {
          int num1 = (int) Convert.ToInt16(registryKey1.GetValue("CC1"));
          int num2 = (int) Convert.ToInt16(registryKey2.GetValue("ZENBAT1"));
          if (num1 == num2 && num1 < -60)
          {
            registryKey1.SetValue("CC1", (object) -60);
            registryKey2.SetValue("ZENBAT1", (object) -60);
            num1 = -60;
            num2 = -60;
          }
          if (num1 < -60 || num1 >= 0 || num1 != num2)
            return -1;
          if (incremento)
          {
            int num3 = num1 + 1;
            registryKey1.SetValue("CC1", (object) num3);
            registryKey2.SetValue("ZENBAT1", (object) num3);
            if (num3 > -30)
            {
              int num4 = (int) MessageBox.Show(Resource_Form_clave.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          return 1;
        }
        if (int64 == 0L)
          return 0;
        return int64 != 0L ? -4 : -3;
      }
      catch (Exception ex)
      {
        return -3;
      }
    }

    public int leer_contador_uso(bool incremento)
    {
      try
      {
        RegistryKey subKey1 = Registry.CurrentUser.CreateSubKey("Software\\P");
        RegistryKey subKey2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\GJKE");
        if (subKey1.GetValue("CC2") == null && subKey2.GetValue("ZENBAT2") == null)
        {
          int num = -200;
          subKey1.SetValue("CC2", (object) num);
          subKey2.SetValue("ZENBAT2", (object) num);
          return 0;
        }
        int num1 = (int) Convert.ToInt16(subKey1.GetValue("CC2"));
        int num2 = (int) Convert.ToInt16(subKey2.GetValue("ZENBAT2"));
        if (num1 == num2 && num1 < -200)
        {
          subKey1.SetValue("CC2", (object) -200);
          subKey2.SetValue("ZENBAT2", (object) -200);
          num1 = -200;
          num2 = -200;
        }
        if (num1 >= -200 && num1 < 0 && num1 == num2)
        {
          if (incremento)
          {
            int num3 = num1 + 1;
            subKey1.SetValue("CC2", (object) num3);
            subKey2.SetValue("ZENBAT2", (object) num3);
            if (num3 > -30)
            {
              int num4 = (int) MessageBox.Show(Resource_Form_clave.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          return 1;
        }
        int num5 = (int) MessageBox.Show(Resource_Form_clave.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_clave.String3 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
    }

    public int leer_fecha_uso()
    {
      try
      {
        DateTime now = DateTime.Now;
        RegistryKey subKey1 = Registry.CurrentUser.CreateSubKey("Software\\P");
        RegistryKey subKey2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\GJKE");
        string str1 = now.ToString("dd");
        string str2 = now.ToString("MM");
        string str3 = now.ToString("yyyy");
        if (subKey1.GetValue("FECH_I_D") == null && subKey2.GetValue("FECH_U_D") == null)
        {
          subKey1.SetValue("FECH_I_D", (object) str1);
          subKey1.SetValue("FECH_I_M", (object) str2);
          subKey1.SetValue("FECH_I_A", (object) str3);
          subKey2.SetValue("FECH_U_D", (object) str1);
          subKey2.SetValue("FECH_U_M", (object) str2);
          subKey2.SetValue("FECH_U_A", (object) str3);
          return 0;
        }
        int int16_1 = (int) Convert.ToInt16(subKey1.GetValue("FECH_I_D"));
        int int16_2 = (int) Convert.ToInt16(subKey1.GetValue("FECH_I_M"));
        DateTime dateTime1 = new DateTime((int) Convert.ToInt16(subKey1.GetValue("FECH_I_A")), int16_2, int16_1);
        int int16_3 = (int) Convert.ToInt16(subKey2.GetValue("FECH_U_D"));
        int int16_4 = (int) Convert.ToInt16(subKey2.GetValue("FECH_U_M"));
        DateTime dateTime2 = new DateTime((int) Convert.ToInt16(subKey2.GetValue("FECH_U_A")), int16_4, int16_3);
        if (int16_1 == 0 || int16_3 == 0)
        {
          int num = (int) MessageBox.Show(Resource_Form_clave.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return -1;
        }
        if (now > dateTime2)
        {
          subKey2.SetValue("FECH_U_D", (object) str1);
          subKey2.SetValue("FECH_U_M", (object) str2);
          subKey2.SetValue("FECH_U_A", (object) str3);
          dateTime2 = now;
        }
        int days = (dateTime2 - dateTime1).Days;
        if (days < 180)
        {
          if (days > 150)
          {
            int num = (int) MessageBox.Show(Resource_Form_clave.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          return 1;
        }
        int num1 = (int) MessageBox.Show(Resource_Form_clave.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(Resource_Form_clave.String3 + "\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
    }
  }
}
