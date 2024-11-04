// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_main
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.CNC_BACKUP_RESTORE;
using FANUC_Open_Com.CNC_MACHINE_INFO;
using FANUC_Open_Com.CNC_MACRO_LIST;
using FANUC_Open_Com.CNC_SCREEN;
using FANUC_Open_Com.Focas_gj;
using FANUC_Open_Com.Principal;
using FANUC_Open_Com.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_main : Form
  {
    private IContainer components = (IContainer) null;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem enviarToolStripMenuItem1;
    private ToolStripMenuItem recibirToolStripMenuItem;
    private ToolStripMenuItem configurarToolStripMenuItem;
    private ToolStripMenuItem servidorToolStripMenuItem;
    private ToolStripMenuItem informacionToolStripMenuItem;
    private ToolStripMenuItem ficherosToolStripMenuItem;
    private ToolStripMenuItem salirToolStripMenuItem;
    private ToolStripMenuItem editarToolStripMenuItem;
    private ToolStripMenuItem directorioProgramasToolStripMenuItem;
    private Label label1;
    private ToolStripMenuItem programasEnDATASERVERToolStripMenuItem;
    private ToolStripMenuItem fTPDataServerToolStripMenuItem;
    private GroupBox groupBox_language;
    private RadioButton radioButton_english;
    private RadioButton radioButton_spanish;
    private ToolStripMenuItem exploradorToolStripMenuItem1;
    private PictureBox pictureBox1;
    private RadioButton radioButton_french;
    private ToolStripMenuItem Tools_toolStripMenuItem;
    private ToolStripMenuItem pantallasCNCToolStripMenuItem;
    private Button button_Explorador_CNC;
    private RadioButton radioButton_italian;
    private RadioButton radioButton_russian;
    private RadioButton radioButton_german;
    private RadioButton radioButton_portugal;
    private RadioButton radioButton_china;
    private ToolStripMenuItem CNC_BACKUP_ToolStripMenuItem;
    private ToolStripMenuItem macroVariableListToolStripMenuItem;
    private ToolStripMenuItem cNCMachineInfoToolStripMenuItem;

    public Form_main() => this.InitializeComponent();

    private void salirToolStripMenuItem_Click_1(object sender, EventArgs e) => this.Close();

    private void enviarToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_main.String1);
      }
      else
      {
        string str1 = (string) registryKey.GetValue("Machine");
        string str2 = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false).GetValue("Type");
        if (str2 == "ETHERNET")
        {
          this.Enabled = false;
          int num2 = (int) new Form_enviar_ethernet().ShowDialog();
          this.Enabled = true;
        }
        if (!(str2 == "RS232C"))
          return;
        this.Enabled = false;
        int num3 = (int) new Form_enviar_rs232c().ShowDialog();
        this.Enabled = true;
      }
    }

    private void recibirToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_main.String1);
      }
      else
      {
        string str1 = (string) registryKey.GetValue("Machine");
        string str2 = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false).GetValue("Type");
        if (str2 == "ETHERNET")
        {
          this.Enabled = false;
          int num2 = (int) new Form_recibir_ethernet().ShowDialog();
          this.Enabled = true;
        }
        if (!(str2 == "RS232C"))
          return;
        this.Enabled = false;
        int num3 = (int) new Form_recibir_rs232c().ShowDialog();
        this.Enabled = true;
      }
    }

    private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_config().ShowDialog();
      this.Enabled = true;
    }

    private void servidorToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_main.String1);
      }
      else
      {
        string str1 = (string) registryKey.GetValue("Machine");
        string str2 = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false).GetValue("Type");
        if (str2 != "RS232C" && str2 != "ETHERNET")
        {
          int num2 = (int) MessageBox.Show(Resource_Form_main.String1);
        }
        else
        {
          DialogResult dialogResult = MessageBox.Show(Resource_Form_main.String2, "", MessageBoxButtons.YesNo);
          switch (str2)
          {
            case "RS232C":
              if (dialogResult == DialogResult.No)
              {
                this.Enabled = false;
                int num3 = (int) new Form_servidor_rs232c().ShowDialog();
                this.Enabled = true;
                break;
              }
              Thread thread1 = new Thread(new ThreadStart(this.crear_servidor_RS232C));
              thread1.SetApartmentState(ApartmentState.STA);
              thread1.Start();
              break;
            case "ETHERNET":
              if (dialogResult == DialogResult.No)
              {
                this.Enabled = false;
                int num4 = (int) new Form_FTP_server().ShowDialog();
                this.Enabled = true;
                break;
              }
              Thread thread2 = new Thread(new ThreadStart(this.crear_servidor_FTP));
              thread2.SetApartmentState(ApartmentState.STA);
              thread2.Start();
              break;
          }
        }
      }
    }

    private void crear_servidor_FTP()
    {
      string str = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", false).GetValue("Language");
      if (str == "SPANISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
      if (str == "ENGLISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
      if (str == "FRENCH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
      if (str == "ITALIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("it");
      if (str == "RUSSIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
      if (str == "GERMAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
      if (str == "PORTUGUESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt");
      if (str == "CHINESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-Hans");
      int num = (int) new Form_FTP_server().ShowDialog();
    }

    private void crear_servidor_RS232C()
    {
      string str = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", false).GetValue("Language");
      if (str == "SPANISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
      if (str == "ENGLISH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
      if (str == "FRENCH")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
      if (str == "ITALIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("it");
      if (str == "RUSSIAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
      if (str == "GERMAN")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
      if (str == "PORTUGUESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt");
      if (str == "CHINESE")
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-Hans");
      int num = (int) new Form_servidor_rs232c().ShowDialog();
    }

    private void editarToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_TextEditor().ShowDialog();
      this.Enabled = true;
    }

    private void informacionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      int num = (int) new Form_informacion().ShowDialog();
      this.Enabled = true;
    }

    private void directorioProgramasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey == null)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_main.String1);
      }
      else
      {
        string str = (string) registryKey.GetValue("Machine");
        if ((string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false).GetValue("Type") != "ETHERNET")
        {
          int num2 = (int) MessageBox.Show(Resource_Form_main.String3);
        }
        else
        {
          this.Enabled = false;
          int num3 = (int) new Form_listado_programas().ShowDialog();
          this.Enabled = true;
        }
      }
    }

    private void programasEnDATASERVERToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
      if (registryKey1 == null)
      {
        int num1 = (int) MessageBox.Show(Resource_Form_main.String1);
      }
      else
      {
        string str = (string) registryKey1.GetValue("Machine");
        RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str, false);
        if ((string) registryKey2.GetValue("Type") != "ETHERNET")
        {
          int num2 = (int) MessageBox.Show(Resource_Form_main.String3);
        }
        else if ((string) registryKey2.GetValue("DATASERVER") != "YES")
        {
          int num3 = (int) MessageBox.Show(Resource_Form_main.String4);
        }
        else
        {
          this.Enabled = false;
          int num4 = (int) new Form_listado_FTP().ShowDialog();
          this.Enabled = true;
        }
      }
    }

    private void fTPDataServerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.programasEnDATASERVERToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    private void radioButton_spanish_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "SPANISH"))
        return;
      registryKey.SetValue("Language", (object) "SPANISH");
      Application.Restart();
    }

    private void radioButton_english_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "ENGLISH"))
        return;
      registryKey.SetValue("Language", (object) "ENGLISH");
      Application.Restart();
    }

    private void radioButton_french_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "FRENCH"))
        return;
      registryKey.SetValue("Language", (object) "FRENCH");
      Application.Restart();
    }

    private void radioButton_italian_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "ITALIAN"))
        return;
      registryKey.SetValue("Language", (object) "ITALIAN");
      Application.Restart();
    }

    private void radioButton_russian_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "RUSSIAN"))
        return;
      registryKey.SetValue("Language", (object) "RUSSIAN");
      Application.Restart();
    }

    private void radioButton_german_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "GERMAN"))
        return;
      registryKey.SetValue("Language", (object) "GERMAN");
      Application.Restart();
    }

    private void radioButton_portugal_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "PORTUGUESE"))
        return;
      registryKey.SetValue("Language", (object) "PORTUGUESE");
      Application.Restart();
    }

    private void radioButton_china_Click(object sender, EventArgs e)
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true);
      if (!((string) registryKey.GetValue("Language") != "CHINESE"))
        return;
      registryKey.SetValue("Language", (object) "CHINESE");
      Application.Restart();
    }

    private void Form_main_Load(object sender, EventArgs e)
    {
      string str = (string) Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config", true).GetValue("Language");
      if (str == "SPANISH")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = true;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = false;
        this.Text = "   Software para comunicaciones CNC - PC";
      }
      if (str == "ENGLISH")
      {
        this.radioButton_english.Checked = true;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = false;
        this.Text = "   Software for communications CNC - PC";
      }
      if (str == "FRENCH")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = true;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = false;
        this.Text = "   Software pour communications CNC - PC";
      }
      if (str == "ITALIAN")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = true;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = false;
        this.Text = "   Software di comunicazione CNC - PC";
      }
      if (str == "RUSSIAN")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = true;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = false;
        this.Text = "   Программное обеспечение для связи с ЧПУ - ПК";
      }
      if (str == "GERMAN")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = true;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = false;
        this.Text = "   Software für die Kommunikation CNC - PC";
      }
      if (str == "PORTUGUESE")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = true;
        this.radioButton_china.Checked = false;
        this.Text = "   Software para comunicações CNC - PC";
      }
      if (str == "CHINESE")
      {
        this.radioButton_english.Checked = false;
        this.radioButton_spanish.Checked = false;
        this.radioButton_french.Checked = false;
        this.radioButton_italian.Checked = false;
        this.radioButton_russian.Checked = false;
        this.radioButton_german.Checked = false;
        this.radioButton_portugal.Checked = false;
        this.radioButton_china.Checked = true;
        this.Text = "   通信软件 CNC  -  PC";
      }
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Server_FTP", false);
      if (registryKey == null || !((string) registryKey.GetValue("User_list") == "ON"))
        return;
      this.autostart_FTP_servers();
    }

    private void exploradorToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.Visible = false;
      this.Enabled = false;
      int num = (int) new Form_explorer().ShowDialog();
      this.Enabled = true;
      this.Visible = true;
    }

    private void pantallasCNCToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0 && (informacion.leer_contador_uso(true) == -1 || informacion.leer_fecha_uso() == -1))
        return;
      int num1 = (int) MessageBox.Show(Resource_errores_CNC_SCREEN.String1);
      try
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
        {
          int num2 = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          string str1 = (string) registryKey1.GetValue("Machine");
          RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false);
          if (registryKey2 == null)
          {
            int num3 = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if ((string) registryKey2.GetValue("Type") != "ETHERNET")
          {
            int num4 = (int) MessageBox.Show(Resource_Form_main.String3);
          }
          else
          {
            string str2 = (string) registryKey2.GetValue("IP_ETHERNET");
            string str3 = (string) registryKey2.GetValue("Port_ETHERNET");
            string str4 = (string) registryKey2.GetValue("Time_ETHERNET");
            string path = "CNC_SCREEN";
            string str5 = Path.GetPathRoot(Environment.CurrentDirectory) + "\\Display_C";
            Directory.CreateDirectory(str5);
            foreach (string file in Directory.GetFiles(path, "*.*"))
              File.Copy(file, Path.Combine(str5, Path.GetFileName(file)), true);
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = str5 + "\\CNCSCRNE.EXE";
            process.StartInfo.Arguments = "/H=" + str2 + ":" + str3 + "/T=" + str4;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            if (!Directory.Exists(str5))
              return;
            foreach (string file in Directory.GetFiles(path, "*.*"))
              File.Delete(Path.Combine(str5, Path.GetFileName(file)));
            Directory.Delete(str5);
          }
        }
      }
      catch (Exception ex)
      {
        int num5 = (int) MessageBox.Show(Resource_errores_CNC_SCREEN.String2 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void exploradorStripMenuItem_Click(object sender, EventArgs e)
    {
      this.exploradorToolStripMenuItem1_Click((object) null, (EventArgs) null);
    }

    private void button_Explorador_CNC_Click(object sender, EventArgs e)
    {
      this.exploradorToolStripMenuItem1_Click((object) null, (EventArgs) null);
    }

    private void CNC_BACKUP_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0 && (informacion.leer_contador_uso(true) == -1 || informacion.leer_fecha_uso() == -1))
        return;
      DialogResult dialogResult = MessageBox.Show(Resource_mensajes_CNC_BACKUP_RESTORE.String1, "WARNING", MessageBoxButtons.YesNoCancel);
      if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
        return;
      if (dialogResult == DialogResult.Yes)
      {
        int num1 = (int) new Form_informacion_general()
        {
          Fichero_mostrar = "Help_CNC_DATA_MANAGEMENT_TOOL"
        }.ShowDialog();
      }
      try
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
        {
          int num2 = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          string str1 = (string) registryKey1.GetValue("Machine");
          RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false);
          if (registryKey2 == null)
          {
            int num3 = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if ((string) registryKey2.GetValue("Type") != "ETHERNET")
          {
            int num4 = (int) MessageBox.Show(Resource_Form_main.String3);
          }
          else
          {
            string str2 = (string) registryKey2.GetValue("IP_ETHERNET");
            string str3 = (string) registryKey2.GetValue("Port_ETHERNET");
            string str4 = (string) registryKey2.GetValue("Time_ETHERNET");
            string path1_1 = "CNC_BACKUP_RESTORE";
            string path2_1 = "Language.TXT";
            string str5 = Path.Combine(path1_1, "System");
            string path2_2 = "Language.XML";
            Directory.CreateDirectory(str5);
            if (!File.Exists(Path.Combine(str5, path2_2)))
              File.Copy(Path.Combine(path1_1, path2_1), Path.Combine(str5, path2_2), true);
            string path1_2 = "CNC_BACKUP_RESTORE";
            string path2_3 = "CDMT.ini";
            string str6 = "C:\\ProgramData\\FANUC\\CNCDataManagementTool\\Ini";
            Directory.CreateDirectory(str6);
            string path2_4 = path2_3;
            if (!File.Exists(Path.Combine(str6, path2_4)))
              File.Copy(Path.Combine(path1_2, path2_3), Path.Combine(str6, path2_4), true);
            string[] contents = File.ReadAllLines(Path.Combine(str6, path2_4));
            string str7 = "IP";
            string str8 = "IP=" + str2;
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str7))
              {
                contents[index] = str8;
                break;
              }
            }
            string str9 = "PORT";
            string str10 = "PORT=" + str3;
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str9))
              {
                contents[index] = str10;
                break;
              }
            }
            string str11 = "TIMEOUT";
            string str12 = "TIMEOUT=" + str4;
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str11))
              {
                contents[index] = str12;
                break;
              }
            }
            string str13 = "KEY_UP_APP";
            string str14 = "KEY_UP_APP=0";
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str13))
              {
                contents[index] = str14;
                break;
              }
            }
            string str15 = "KEY_UP_SRAM";
            string str16 = "KEY_UP_SRAM=0";
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str15))
              {
                contents[index] = str16;
                break;
              }
            }
            File.WriteAllLines(Path.Combine(str6, path2_4), contents);
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "CNC_BACKUP_RESTORE\\CDMT.exe";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
          }
        }
      }
      catch (Exception ex)
      {
        int num5 = (int) MessageBox.Show(Resource_mensajes_CNC_BACKUP_RESTORE.String2 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void macroVariableListToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0 && (informacion.leer_contador_uso(true) == -1 || informacion.leer_fecha_uso() == -1))
        return;
      DialogResult dialogResult = MessageBox.Show(Resource_mensajes_CNC_MACRO_LIST.String1, "INFORMATION", MessageBoxButtons.YesNoCancel);
      if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
        return;
      if (dialogResult == DialogResult.Yes)
      {
        int num1 = (int) new Form_informacion_general()
        {
          Fichero_mostrar = "Help_CNC_MACRO_LIST"
        }.ShowDialog();
      }
      try
      {
        RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Last_machine", false);
        if (registryKey1 == null)
        {
          int num2 = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          string str1 = (string) registryKey1.GetValue("Machine");
          RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + str1, false);
          if (registryKey2 == null)
          {
            int num3 = (int) MessageBox.Show(Resource_configurar_ethernet.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if ((string) registryKey2.GetValue("Type") != "ETHERNET")
          {
            int num4 = (int) MessageBox.Show(Resource_Form_main.String3);
          }
          else
          {
            string str2 = (string) registryKey2.GetValue("IP_ETHERNET");
            string str3 = (string) registryKey2.GetValue("Port_ETHERNET");
            string str4 = (string) registryKey2.GetValue("Time_ETHERNET");
            string path1_1 = "CNC_MACRO_LIST";
            string path2_1 = "Language.TXT";
            string path2_2 = "style.TXT";
            string str5 = Path.Combine(path1_1, "System");
            string path2_3 = "Language.XML";
            string path2_4 = "style.css";
            Directory.CreateDirectory(str5);
            if (!File.Exists(Path.Combine(str5, path2_3)))
              File.Copy(Path.Combine(path1_1, path2_1), Path.Combine(str5, path2_3), true);
            if (!File.Exists(Path.Combine(str5, path2_4)))
              File.Copy(Path.Combine(path1_1, path2_2), Path.Combine(str5, path2_4), true);
            string path1_2 = "CNC_MACRO_LIST";
            string path2_5 = "CML.ini";
            string str6 = "C:\\ProgramData\\FANUC\\Tools\\CML";
            Directory.CreateDirectory(str6);
            string path2_6 = path2_5;
            if (!File.Exists(Path.Combine(str6, path2_6)))
              File.Copy(Path.Combine(path1_2, path2_5), Path.Combine(str6, path2_6), true);
            string[] contents = File.ReadAllLines(Path.Combine(str6, path2_6));
            string str7 = "ETHER_IPADDR";
            string str8 = "ETHER_IPADDR=" + str2;
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str7))
              {
                contents[index] = str8;
                break;
              }
            }
            string str9 = "ETHER_PORT";
            string str10 = "ETHER_PORT=" + str3;
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str9))
              {
                contents[index] = str10;
                break;
              }
            }
            string str11 = "ETHER_TIMEOUT";
            string str12 = "ETHER_TIMEOUT=" + str4;
            for (int index = 0; index < contents.Length; ++index)
            {
              if (contents[index].Contains(str11))
              {
                contents[index] = str12;
                break;
              }
            }
            File.WriteAllLines(Path.Combine(str6, path2_6), contents);
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "CNC_MACRO_LIST\\CML.exe";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
          }
        }
      }
      catch (Exception ex)
      {
        int num5 = (int) MessageBox.Show(Resource_mensajes_CNC_MACRO_LIST.String2 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void cNCMachineInfoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Informacion informacion = new Informacion();
      if (informacion.leer_clave(true) <= 0 && (informacion.leer_contador_uso(true) == -1 || informacion.leer_fecha_uso() == -1))
        return;
      DialogResult dialogResult = MessageBox.Show(Resource_mensajes_CNC_MACHINE_INFO.String1, "INFORMATION", MessageBoxButtons.YesNoCancel);
      if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
        return;
      if (dialogResult == DialogResult.Yes)
      {
        int num1 = (int) new Form_informacion_general()
        {
          Fichero_mostrar = "Help_CNC_MACHINE_INFO"
        }.ShowDialog();
      }
      try
      {
        Process process = new Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = "CNC_MACHINE_INFO\\ActiveG-codeChecker.exe";
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show(Resource_mensajes_CNC_MACHINE_INFO.String2 + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void autostart_FTP_servers()
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string[] source = new string[20];
      string str4 = "";
      RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Server_FTP", true);
      for (int index = 1; index <= 20; ++index)
      {
        try
        {
          RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Machine" + index.ToString(), false);
          if (registryKey2 != null)
          {
            str1 = (string) registryKey2.GetValue("Type");
            str2 = (string) registryKey2.GetValue("Port_FTP_Server");
            str3 = (string) registryKey2.GetValue("Directory_FTP_Server");
            str4 = (string) registryKey2.GetValue("Auto_start_FTP_Server");
            if (!((IEnumerable<string>) source).Contains<string>(str2))
              source[index] = str2;
            else
              continue;
          }
          else
            continue;
        }
        catch
        {
        }
        if (str4 == "ON" && str1 == "ETHERNET" && str2 != "" && str3 != "")
        {
          registryKey1.SetValue("Activate_FTP_Server", (object) "YES");
          registryKey1.SetValue("Directory", (object) str3);
          registryKey1.SetValue("FTP_port", (object) str2);
          Thread thread = new Thread(new ThreadStart(this.crear_servidor_FTP));
          thread.SetApartmentState(ApartmentState.STA);
          thread.Start();
          Thread.Sleep(2000);
        }
      }
      registryKey1.SetValue("Activate_FTP_Server", (object) "NO");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_main));
      this.menuStrip1 = new MenuStrip();
      this.ficherosToolStripMenuItem = new ToolStripMenuItem();
      this.editarToolStripMenuItem = new ToolStripMenuItem();
      this.directorioProgramasToolStripMenuItem = new ToolStripMenuItem();
      this.programasEnDATASERVERToolStripMenuItem = new ToolStripMenuItem();
      this.exploradorToolStripMenuItem1 = new ToolStripMenuItem();
      this.salirToolStripMenuItem = new ToolStripMenuItem();
      this.configurarToolStripMenuItem = new ToolStripMenuItem();
      this.enviarToolStripMenuItem1 = new ToolStripMenuItem();
      this.recibirToolStripMenuItem = new ToolStripMenuItem();
      this.servidorToolStripMenuItem = new ToolStripMenuItem();
      this.fTPDataServerToolStripMenuItem = new ToolStripMenuItem();
      this.informacionToolStripMenuItem = new ToolStripMenuItem();
      this.Tools_toolStripMenuItem = new ToolStripMenuItem();
      this.pantallasCNCToolStripMenuItem = new ToolStripMenuItem();
      this.CNC_BACKUP_ToolStripMenuItem = new ToolStripMenuItem();
      this.macroVariableListToolStripMenuItem = new ToolStripMenuItem();
      this.cNCMachineInfoToolStripMenuItem = new ToolStripMenuItem();
      this.label1 = new Label();
      this.groupBox_language = new GroupBox();
      this.radioButton_china = new RadioButton();
      this.radioButton_portugal = new RadioButton();
      this.radioButton_german = new RadioButton();
      this.radioButton_russian = new RadioButton();
      this.radioButton_italian = new RadioButton();
      this.radioButton_french = new RadioButton();
      this.radioButton_english = new RadioButton();
      this.radioButton_spanish = new RadioButton();
      this.button_Explorador_CNC = new Button();
      this.pictureBox1 = new PictureBox();
      this.menuStrip1.SuspendLayout();
      this.groupBox_language.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.menuStrip1, "menuStrip1");
      this.menuStrip1.BackColor = System.Drawing.Color.Yellow;
      this.menuStrip1.ImageScalingSize = new Size(20, 20);
      this.menuStrip1.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.ficherosToolStripMenuItem,
        (ToolStripItem) this.configurarToolStripMenuItem,
        (ToolStripItem) this.enviarToolStripMenuItem1,
        (ToolStripItem) this.recibirToolStripMenuItem,
        (ToolStripItem) this.servidorToolStripMenuItem,
        (ToolStripItem) this.fTPDataServerToolStripMenuItem,
        (ToolStripItem) this.informacionToolStripMenuItem,
        (ToolStripItem) this.Tools_toolStripMenuItem
      });
      this.menuStrip1.Name = "menuStrip1";
      componentResourceManager.ApplyResources((object) this.ficherosToolStripMenuItem, "ficherosToolStripMenuItem");
      this.ficherosToolStripMenuItem.AutoToolTip = true;
      this.ficherosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.editarToolStripMenuItem,
        (ToolStripItem) this.directorioProgramasToolStripMenuItem,
        (ToolStripItem) this.programasEnDATASERVERToolStripMenuItem,
        (ToolStripItem) this.exploradorToolStripMenuItem1,
        (ToolStripItem) this.salirToolStripMenuItem
      });
      this.ficherosToolStripMenuItem.Name = "ficherosToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.editarToolStripMenuItem, "editarToolStripMenuItem");
      this.editarToolStripMenuItem.Name = "editarToolStripMenuItem";
      this.editarToolStripMenuItem.Click += new EventHandler(this.editarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.directorioProgramasToolStripMenuItem, "directorioProgramasToolStripMenuItem");
      this.directorioProgramasToolStripMenuItem.Name = "directorioProgramasToolStripMenuItem";
      this.directorioProgramasToolStripMenuItem.Click += new EventHandler(this.directorioProgramasToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.programasEnDATASERVERToolStripMenuItem, "programasEnDATASERVERToolStripMenuItem");
      this.programasEnDATASERVERToolStripMenuItem.Name = "programasEnDATASERVERToolStripMenuItem";
      this.programasEnDATASERVERToolStripMenuItem.Click += new EventHandler(this.programasEnDATASERVERToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.exploradorToolStripMenuItem1, "exploradorToolStripMenuItem1");
      this.exploradorToolStripMenuItem1.ForeColor = System.Drawing.Color.ForestGreen;
      this.exploradorToolStripMenuItem1.Image = (Image) Resources.DIR10A;
      this.exploradorToolStripMenuItem1.Name = "exploradorToolStripMenuItem1";
      this.exploradorToolStripMenuItem1.Click += new EventHandler(this.exploradorToolStripMenuItem1_Click);
      componentResourceManager.ApplyResources((object) this.salirToolStripMenuItem, "salirToolStripMenuItem");
      this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
      this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click_1);
      componentResourceManager.ApplyResources((object) this.configurarToolStripMenuItem, "configurarToolStripMenuItem");
      this.configurarToolStripMenuItem.Name = "configurarToolStripMenuItem";
      this.configurarToolStripMenuItem.Click += new EventHandler(this.configurarToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.enviarToolStripMenuItem1, "enviarToolStripMenuItem1");
      this.enviarToolStripMenuItem1.AutoToolTip = true;
      this.enviarToolStripMenuItem1.Name = "enviarToolStripMenuItem1";
      this.enviarToolStripMenuItem1.Click += new EventHandler(this.enviarToolStripMenuItem1_Click);
      componentResourceManager.ApplyResources((object) this.recibirToolStripMenuItem, "recibirToolStripMenuItem");
      this.recibirToolStripMenuItem.Name = "recibirToolStripMenuItem";
      this.recibirToolStripMenuItem.Click += new EventHandler(this.recibirToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.servidorToolStripMenuItem, "servidorToolStripMenuItem");
      this.servidorToolStripMenuItem.Name = "servidorToolStripMenuItem";
      this.servidorToolStripMenuItem.Click += new EventHandler(this.servidorToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.fTPDataServerToolStripMenuItem, "fTPDataServerToolStripMenuItem");
      this.fTPDataServerToolStripMenuItem.Name = "fTPDataServerToolStripMenuItem";
      this.fTPDataServerToolStripMenuItem.Click += new EventHandler(this.fTPDataServerToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.informacionToolStripMenuItem, "informacionToolStripMenuItem");
      this.informacionToolStripMenuItem.Name = "informacionToolStripMenuItem";
      this.informacionToolStripMenuItem.Click += new EventHandler(this.informacionToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.Tools_toolStripMenuItem, "Tools_toolStripMenuItem");
      this.Tools_toolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.pantallasCNCToolStripMenuItem,
        (ToolStripItem) this.CNC_BACKUP_ToolStripMenuItem,
        (ToolStripItem) this.macroVariableListToolStripMenuItem,
        (ToolStripItem) this.cNCMachineInfoToolStripMenuItem
      });
      this.Tools_toolStripMenuItem.Image = (Image) Resources.tools;
      this.Tools_toolStripMenuItem.Name = "Tools_toolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.pantallasCNCToolStripMenuItem, "pantallasCNCToolStripMenuItem");
      this.pantallasCNCToolStripMenuItem.Image = (Image) Resources.SimBase;
      this.pantallasCNCToolStripMenuItem.Name = "pantallasCNCToolStripMenuItem";
      this.pantallasCNCToolStripMenuItem.Click += new EventHandler(this.pantallasCNCToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.CNC_BACKUP_ToolStripMenuItem, "CNC_BACKUP_ToolStripMenuItem");
      this.CNC_BACKUP_ToolStripMenuItem.Name = "CNC_BACKUP_ToolStripMenuItem";
      this.CNC_BACKUP_ToolStripMenuItem.Click += new EventHandler(this.CNC_BACKUP_ToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.macroVariableListToolStripMenuItem, "macroVariableListToolStripMenuItem");
      this.macroVariableListToolStripMenuItem.Image = (Image) Resources.Capture;
      this.macroVariableListToolStripMenuItem.Name = "macroVariableListToolStripMenuItem";
      this.macroVariableListToolStripMenuItem.Click += new EventHandler(this.macroVariableListToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.cNCMachineInfoToolStripMenuItem, "cNCMachineInfoToolStripMenuItem");
      this.cNCMachineInfoToolStripMenuItem.Image = (Image) Resources.Capture_v06;
      this.cNCMachineInfoToolStripMenuItem.Name = "cNCMachineInfoToolStripMenuItem";
      this.cNCMachineInfoToolStripMenuItem.Click += new EventHandler(this.cNCMachineInfoToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.BackColor = System.Drawing.Color.White;
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.groupBox_language, "groupBox_language");
      this.groupBox_language.Controls.Add((Control) this.radioButton_china);
      this.groupBox_language.Controls.Add((Control) this.radioButton_portugal);
      this.groupBox_language.Controls.Add((Control) this.radioButton_german);
      this.groupBox_language.Controls.Add((Control) this.radioButton_russian);
      this.groupBox_language.Controls.Add((Control) this.radioButton_italian);
      this.groupBox_language.Controls.Add((Control) this.radioButton_french);
      this.groupBox_language.Controls.Add((Control) this.radioButton_english);
      this.groupBox_language.Controls.Add((Control) this.radioButton_spanish);
      this.groupBox_language.Name = "groupBox_language";
      this.groupBox_language.TabStop = false;
      componentResourceManager.ApplyResources((object) this.radioButton_china, "radioButton_china");
      this.radioButton_china.Image = (Image) Resources.CHINA;
      this.radioButton_china.Name = "radioButton_china";
      this.radioButton_china.TabStop = true;
      this.radioButton_china.UseVisualStyleBackColor = true;
      this.radioButton_china.Click += new EventHandler(this.radioButton_china_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_portugal, "radioButton_portugal");
      this.radioButton_portugal.Image = (Image) Resources.PORTGL0A;
      this.radioButton_portugal.Name = "radioButton_portugal";
      this.radioButton_portugal.TabStop = true;
      this.radioButton_portugal.UseVisualStyleBackColor = true;
      this.radioButton_portugal.Click += new EventHandler(this.radioButton_portugal_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_german, "radioButton_german");
      this.radioButton_german.Name = "radioButton_german";
      this.radioButton_german.TabStop = true;
      this.radioButton_german.UseVisualStyleBackColor = true;
      this.radioButton_german.Click += new EventHandler(this.radioButton_german_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_russian, "radioButton_russian");
      this.radioButton_russian.Name = "radioButton_russian";
      this.radioButton_russian.TabStop = true;
      this.radioButton_russian.UseVisualStyleBackColor = true;
      this.radioButton_russian.Click += new EventHandler(this.radioButton_russian_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_italian, "radioButton_italian");
      this.radioButton_italian.Image = (Image) Resources.ITALY0C;
      this.radioButton_italian.Name = "radioButton_italian";
      this.radioButton_italian.TabStop = true;
      this.radioButton_italian.UseVisualStyleBackColor = true;
      this.radioButton_italian.Click += new EventHandler(this.radioButton_italian_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_french, "radioButton_french");
      this.radioButton_french.Image = (Image) Resources.FRENCH1A;
      this.radioButton_french.Name = "radioButton_french";
      this.radioButton_french.TabStop = true;
      this.radioButton_french.UseVisualStyleBackColor = true;
      this.radioButton_french.Click += new EventHandler(this.radioButton_french_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_english, "radioButton_english");
      this.radioButton_english.Image = (Image) Resources.English_flag_32pix_8bit;
      this.radioButton_english.Name = "radioButton_english";
      this.radioButton_english.TabStop = true;
      this.radioButton_english.UseVisualStyleBackColor = true;
      this.radioButton_english.Click += new EventHandler(this.radioButton_english_Click);
      componentResourceManager.ApplyResources((object) this.radioButton_spanish, "radioButton_spanish");
      this.radioButton_spanish.Image = (Image) Resources.Spain_flag_32pix_8bit;
      this.radioButton_spanish.Name = "radioButton_spanish";
      this.radioButton_spanish.TabStop = true;
      this.radioButton_spanish.UseVisualStyleBackColor = true;
      this.radioButton_spanish.Click += new EventHandler(this.radioButton_spanish_Click);
      componentResourceManager.ApplyResources((object) this.button_Explorador_CNC, "button_Explorador_CNC");
      this.button_Explorador_CNC.ForeColor = System.Drawing.Color.DarkGreen;
      this.button_Explorador_CNC.Image = (Image) Resources.Explorer_2;
      this.button_Explorador_CNC.Name = "button_Explorador_CNC";
      this.button_Explorador_CNC.UseVisualStyleBackColor = true;
      this.button_Explorador_CNC.Click += new EventHandler(this.button_Explorador_CNC_Click);
      componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
      this.pictureBox1.Image = (Image) Resources.DIBUJO_CNC_PEQUEÑO__V3;
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = SystemColors.ControlLightLight;
      this.Controls.Add((Control) this.button_Explorador_CNC);
      this.Controls.Add((Control) this.groupBox_language);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (Form_main);
      this.Load += new EventHandler(this.Form_main_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.groupBox_language.ResumeLayout(false);
      this.groupBox_language.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
