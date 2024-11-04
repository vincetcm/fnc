// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Class_chequeo_errores_focas
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Focas_gj;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  internal class Class_chequeo_errores_focas
  {
    public int chequeo_errores_focas(ushort handle, string funcion, Focas1.focas_ret ret_focas)
    {
      if (ret_focas == Focas1.focas_ret.EW_SOCKET || ret_focas == Focas1.focas_ret.EW_HANDLE)
      {
        int num = (int) MessageBox.Show(Resource_errores_focas.String37, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      if (handle <= (ushort) 0)
      {
        int num = (int) MessageBox.Show(Resource_errores_focas.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return -1;
      }
      switch (funcion)
      {
        case "cnc_dwnstart3":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num1 = (int) MessageBox.Show(Resource_errores_focas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num2 = (int) Focas1.cnc_dwnend3(handle);
              Cursor.Current = Cursors.Default;
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_ATTRIB:
              int num3 = (int) MessageBox.Show(Resource_errores_focas.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_NOOPT:
              int num4 = (int) MessageBox.Show(Resource_errores_focas.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PARAM:
              int num5 = (int) MessageBox.Show(Resource_errores_focas.String5, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_MODE:
              int num6 = (int) MessageBox.Show(Resource_errores_focas.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num7 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num8 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PASSWD:
              int num9 = (int) MessageBox.Show(Resource_errores_focas.String9, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num10 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_download3":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_RESET:
              int num11 = (int) MessageBox.Show(Resource_errores_focas.String10, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num12 = (int) Focas1.cnc_dwnend3(handle);
              Cursor.Current = Cursors.Default;
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a1 = new Focas1.ODBERR();
              int num13 = (int) Focas1.cnc_getdtailerr(handle, a1);
              switch (a1.err_no)
              {
                case 1:
                  int num14 = (int) MessageBox.Show(Resource_errores_focas.String11, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 3:
                  int num15 = (int) MessageBox.Show(Resource_errores_focas.String12, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 4:
                  int num16 = (int) MessageBox.Show(Resource_errores_focas.String13, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 5:
                  int num17 = (int) MessageBox.Show(Resource_errores_focas.String14, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num18 = (int) MessageBox.Show(Resource_errores_focas.String15 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num19 = (int) MessageBox.Show(Resource_errores_focas.String16 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OVRFLOW:
              int num20 = (int) MessageBox.Show(Resource_errores_focas.String17 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_BUFFER:
              return 0;
            case Focas1.focas_ret.EW_REJECT:
              int num21 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num22 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num23 = (int) MessageBox.Show(Resource_errores_focas.String19 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dwnend3":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a2 = new Focas1.ODBERR();
              int num24 = (int) Focas1.cnc_getdtailerr(handle, a2);
              switch (a2.err_no)
              {
                case 1:
                  int num25 = (int) MessageBox.Show(Resource_errores_focas.String11, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 3:
                  int num26 = (int) MessageBox.Show(Resource_errores_focas.String12, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 4:
                  int num27 = (int) MessageBox.Show(Resource_errores_focas.String13, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 5:
                  int num28 = (int) MessageBox.Show(Resource_errores_focas.String14, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num29 = (int) MessageBox.Show(Resource_errores_focas.String15 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num30 = (int) MessageBox.Show(Resource_errores_focas.String16 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OVRFLOW:
              int num31 = (int) MessageBox.Show(Resource_errores_focas.String17 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num32 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num33 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num34 = (int) MessageBox.Show("ERROR: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dncstart2":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num35 = (int) MessageBox.Show(Resource_errores_focas.String20, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num36 = (int) Focas1.cnc_dncend2(handle, (short) -1);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_NOOPT:
              int num37 = (int) MessageBox.Show(Resource_errores_focas.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PARAM:
              int num38 = (int) MessageBox.Show(Resource_errores_focas.String21, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num39 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num40 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dnc2":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_RESET:
              int num41 = (int) MessageBox.Show(Resource_errores_focas.String10, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num42 = (int) Focas1.cnc_dncend2(handle, short.MinValue);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_BUFFER:
              return 0;
            default:
              int num43 = (int) MessageBox.Show("ERROR : " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dncend2":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_NOOPT:
              int num44 = (int) MessageBox.Show(Resource_errores_focas.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PARAM:
              int num45 = (int) MessageBox.Show(Resource_errores_focas.String21, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num46 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_delete":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num47 = (int) MessageBox.Show(Resource_errores_focas.String23, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_FUNC:
              int num48 = (int) MessageBox.Show(Resource_errores_focas.String22, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_DATA:
              int num49 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num50 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PASSWD:
              int num51 = (int) MessageBox.Show(Resource_errores_focas.String9, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num52 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_renameprog":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num53 = (int) MessageBox.Show(Resource_errores_focas.String23, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a3 = new Focas1.ODBERR();
              int num54 = (int) Focas1.cnc_getdtailerr(handle, a3);
              switch (a3.err_no)
              {
                case 1:
                  int num55 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num56 = (int) MessageBox.Show(Resource_errores_focas.String25, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num57 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num58 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num59 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num60 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_copyprog":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num61 = (int) MessageBox.Show(Resource_errores_focas.String23, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a4 = new Focas1.ODBERR();
              int num62 = (int) Focas1.cnc_getdtailerr(handle, a4);
              switch (a4.err_no)
              {
                case 1:
                  int num63 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num64 = (int) MessageBox.Show(Resource_errores_focas.String25, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num65 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num66 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OVRFLOW:
              int num67 = (int) MessageBox.Show(Resource_errores_focas.String17 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_MODE:
              int num68 = (int) MessageBox.Show(Resource_errores_focas.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num69 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num70 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num71 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_upstart3":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num72 = (int) MessageBox.Show(Resource_errores_focas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num73 = (int) Focas1.cnc_upend3(handle);
              Cursor.Current = Cursors.Default;
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_ATTRIB:
              int num74 = (int) MessageBox.Show(Resource_errores_focas.String26, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_DATA:
              int num75 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_NOOPT:
              int num76 = (int) MessageBox.Show(Resource_errores_focas.String4, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PARAM:
              int num77 = (int) MessageBox.Show(Resource_errores_focas.String21, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_MODE:
              int num78 = (int) MessageBox.Show(Resource_errores_focas.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num79 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num80 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PASSWD:
              int num81 = (int) MessageBox.Show(Resource_errores_focas.String9, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num82 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num83 = (int) Focas1.cnc_upend3(handle);
              Cursor.Current = Cursors.Default;
              return -1;
          }
        case "cnc_upload3":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_RESET:
              int num84 = (int) MessageBox.Show(Resource_errores_focas.String10, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num85 = (int) Focas1.cnc_upend3(handle);
              Cursor.Current = Cursors.Default;
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_FUNC:
              int num86 = (int) MessageBox.Show(Resource_errores_focas.String27, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_LENGTH:
              int num87 = (int) MessageBox.Show(Resource_errores_focas.String28, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_DATA:
              int num88 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num89 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_BUFFER:
              return 0;
            case Focas1.focas_ret.EW_REJECT:
              int num90 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num91 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num92 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              int num93 = (int) Focas1.cnc_upend3(handle);
              Cursor.Current = Cursors.Default;
              return -1;
          }
        case "cnc_pdf_add":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num94 = (int) MessageBox.Show(Resource_errores_focas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a5 = new Focas1.ODBERR();
              int num95 = (int) Focas1.cnc_getdtailerr(handle, a5);
              switch (a5.err_no)
              {
                case 1:
                  int num96 = (int) MessageBox.Show(Resource_errores_focas.String30, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num97 = (int) MessageBox.Show(Resource_errores_focas.String31, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 3:
                  int num98 = (int) MessageBox.Show(Resource_errores_focas.String34, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 4:
                  int num99 = (int) MessageBox.Show(Resource_errores_focas.String32, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 5:
                  int num100 = (int) MessageBox.Show(Resource_errores_focas.String33, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num101 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num102 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num103 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num104 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_rdpdf_alldir":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num105 = (int) MessageBox.Show(Resource_errores_focas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_ATTRIB:
              int num106 = (int) MessageBox.Show(Resource_errores_focas.String3, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num107 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_pdf_rename":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num108 = (int) MessageBox.Show(Resource_errores_focas.String23, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a6 = new Focas1.ODBERR();
              int num109 = (int) Focas1.cnc_getdtailerr(handle, a6);
              switch (a6.err_no)
              {
                case 1:
                  int num110 = (int) MessageBox.Show(Resource_errores_focas.String30, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num111 = (int) MessageBox.Show(Resource_errores_focas.String31, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 4:
                  int num112 = (int) MessageBox.Show(Resource_errores_focas.String32, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 5:
                  int num113 = (int) MessageBox.Show(Resource_errores_focas.String33, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num114 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num115 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num116 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num117 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_copy":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num118 = (int) MessageBox.Show(Resource_errores_focas.String23, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a7 = new Focas1.ODBERR();
              int num119 = (int) Focas1.cnc_getdtailerr(handle, a7);
              switch (a7.err_no)
              {
                case 1:
                  int num120 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num121 = (int) MessageBox.Show(Resource_errores_focas.String25, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num122 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num123 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OVRFLOW:
              int num124 = (int) MessageBox.Show(Resource_errores_focas.String17 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_MODE:
              int num125 = (int) MessageBox.Show(Resource_errores_focas.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num126 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num127 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num128 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_pdf_copy":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num129 = (int) MessageBox.Show(Resource_errores_focas.String23, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a8 = new Focas1.ODBERR();
              int num130 = (int) Focas1.cnc_getdtailerr(handle, a8);
              switch (a8.err_no)
              {
                case 1:
                  int num131 = (int) MessageBox.Show(Resource_errores_focas.String24, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num132 = (int) MessageBox.Show(Resource_errores_focas.String25, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num133 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num134 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OVRFLOW:
              int num135 = (int) MessageBox.Show(Resource_errores_focas.String17 + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_MODE:
              int num136 = (int) MessageBox.Show(Resource_errores_focas.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num137 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_ALARM:
              int num138 = (int) MessageBox.Show(Resource_errores_focas.String8, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num139 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_pdf_del":
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num140 = (int) MessageBox.Show(Resource_errores_focas.String2, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_DATA:
              Focas1.ODBERR a9 = new Focas1.ODBERR();
              int num141 = (int) Focas1.cnc_getdtailerr(handle, a9);
              switch (a9.err_no)
              {
                case 1:
                  int num142 = (int) MessageBox.Show(Resource_errores_focas.String30, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 2:
                  int num143 = (int) MessageBox.Show(Resource_errores_focas.String31, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 3:
                  int num144 = (int) MessageBox.Show(Resource_errores_focas.String34, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 4:
                  int num145 = (int) MessageBox.Show(Resource_errores_focas.String32, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 5:
                  int num146 = (int) MessageBox.Show(Resource_errores_focas.String33, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num147 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_PROT:
              int num148 = (int) MessageBox.Show(Resource_errores_focas.String16, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_REJECT:
              int num149 = (int) MessageBox.Show(Resource_errores_focas.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            default:
              int num150 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dsrmdir":
          Focas1.ODBERR a10 = new Focas1.ODBERR();
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num151 = (int) MessageBox.Show(Resource_errores_focas.String38, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_NOOPT:
              int num152 = (int) Focas1.cnc_getdtailerr(handle, a10);
              switch (a10.err_no)
              {
                case 22:
                  int num153 = (int) MessageBox.Show(Resource_errores_focas.String42, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 24:
                  int num154 = (int) MessageBox.Show(Resource_errores_focas.String42, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 26:
                  int num155 = (int) MessageBox.Show(Resource_errores_focas.String41, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num156 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_DTSRVR:
              int num157 = (int) Focas1.cnc_getdtailerr(handle, a10);
              switch (a10.err_no)
              {
                case 464:
                  int num158 = (int) MessageBox.Show(Resource_errores_focas.String39, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 487:
                  int num159 = (int) MessageBox.Show(Resource_errores_focas.String40, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num160 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            default:
              int num161 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dsmkdir":
          Focas1.ODBERR a11 = new Focas1.ODBERR();
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num162 = (int) MessageBox.Show(Resource_errores_focas.String38, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_NOOPT:
              int num163 = (int) Focas1.cnc_getdtailerr(handle, a11);
              switch (a11.err_no)
              {
                case 22:
                  int num164 = (int) MessageBox.Show(Resource_errores_focas.String42, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 24:
                  int num165 = (int) MessageBox.Show(Resource_errores_focas.String42, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 26:
                  int num166 = (int) MessageBox.Show(Resource_errores_focas.String41, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num167 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_DTSRVR:
              int num168 = (int) Focas1.cnc_getdtailerr(handle, a11);
              switch (a11.err_no)
              {
                case 464:
                  int num169 = (int) MessageBox.Show(Resource_errores_focas.String39, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 489:
                  int num170 = (int) MessageBox.Show(Resource_errores_focas.String43, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num171 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            default:
              int num172 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        case "cnc_dsrename":
          Focas1.ODBERR a12 = new Focas1.ODBERR();
          switch (ret_focas)
          {
            case Focas1.focas_ret.EW_BUSY:
              int num173 = (int) MessageBox.Show(Resource_errores_focas.String38, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
            case Focas1.focas_ret.EW_OK:
              return 0;
            case Focas1.focas_ret.EW_NOOPT:
              int num174 = (int) Focas1.cnc_getdtailerr(handle, a12);
              switch (a12.err_no)
              {
                case 22:
                  int num175 = (int) MessageBox.Show(Resource_errores_focas.String42, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 24:
                  int num176 = (int) MessageBox.Show(Resource_errores_focas.String42, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 26:
                  int num177 = (int) MessageBox.Show(Resource_errores_focas.String41, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num178 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            case Focas1.focas_ret.EW_DTSRVR:
              int num179 = (int) Focas1.cnc_getdtailerr(handle, a12);
              switch (a12.err_no)
              {
                case 464:
                  int num180 = (int) MessageBox.Show(Resource_errores_focas.String39, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 491:
                  int num181 = (int) MessageBox.Show(Resource_errores_focas.String44, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                case 497:
                  int num182 = (int) MessageBox.Show(Resource_errores_focas.String45, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
                default:
                  int num183 = (int) MessageBox.Show("ERROR :" + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  break;
              }
              return -1;
            default:
              int num184 = (int) MessageBox.Show("Error: " + ret_focas.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return -1;
          }
        default:
          int num185 = (int) MessageBox.Show(Resource_errores_focas.String29, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return -1;
      }
    }
  }
}
