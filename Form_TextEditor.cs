// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.Form_TextEditor
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using FANUC_Open_Com.Edit2;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace FANUC_Open_Com
{
  public class Form_TextEditor : Form
  {
    private string _fichero_editar = "";
    private bool texto_salvado = true;
    private IContainer components = (IContainer) null;
    private ToolStrip Tools;
    private ToolStripButton tb_New;
    private ToolStripButton tb_Open;
    private ToolStripButton tb_Save;
    private ToolStripSeparator toolStripSeparator;
    private ToolStripButton tb_Cut;
    private ToolStripButton tb_Copy;
    private ToolStripButton tb_Paste;
    private ToolStripSeparator toolStripSeparator3;
    private StatusStrip Status;
    private ToolStripStatusLabel charCount;
    private ToolStripStatusLabel toolStripStatusLabel2;
    private ToolStripStatusLabel status_current_line;
    private RichTextBox Document;
    private ToolStripSeparator toolStripSeparator7;
    private ToolStripButton tb_UpperCase;
    private ToolStripSeparator toolStripSeparator8;
    private ToolStripButton tb_ZoomIn;
    private ToolStripButton tb_ZoomOut;
    private ToolStripSeparator toolStripSeparator9;
    private ContextMenuStrip rcMenu;
    private ToolStripMenuItem rc_Undo;
    private ToolStripMenuItem rc_Redo;
    private ToolStripSeparator toolStripSeparator10;
    private ToolStripMenuItem rc_Cut;
    private ToolStripMenuItem rc_Copy;
    private ToolStripMenuItem rc_Paste;
    private Timer Timer;
    private OpenFileDialog openWork;
    private SaveFileDialog saveWork;
    private ToolStripButton toolStripButton1;
    private ToolStripTextBox toolStripTextBox_busqueda;
    private ToolStripButton tb_busqueda_inicio;
    private ToolStripButton toolStripButton_busqueda_desde_cursor;
    private ToolStripMenuItem mM_File;
    private ToolStripMenuItem file_New;
    private ToolStripMenuItem file_Open;
    private ToolStripSeparator toolStripSeparator11;
    private ToolStripMenuItem file_Save;
    private ToolStripMenuItem save_As;
    private ToolStripSeparator toolStripSeparator13;
    private ToolStripMenuItem file_Print;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem file_Exit;
    private ToolStripMenuItem mM_Edit;
    private ToolStripMenuItem edit_Undo;
    private ToolStripMenuItem edit_Redo;
    private ToolStripSeparator toolStripSeparator14;
    private ToolStripMenuItem edit_Cut;
    private ToolStripMenuItem edit_Copy;
    private ToolStripMenuItem edit_Paste;
    private ToolStripSeparator toolStripSeparator15;
    private ToolStripMenuItem edit_SelectAll;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripMenuItem toolStrip_introducir_espacios;
    private ToolStripMenuItem toolStrip_eliminar_espacios;
    private ToolStripMenuItem eliminarLineasVaciasToolStripMenuItem;
    private ToolStripMenuItem mM_Tools;
    private ToolStripMenuItem tools_Customise;
    private MenuStrip mainMenu;
    private ToolStripMenuItem toolStrip_text_color;
    private ToolStripMenuItem textInBlackToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator6;
    private ToolStripTextBox toolStripTextBox_sustituir;
    private ToolStripButton toolStripButton_sustituir;
    private ToolStripButton toolStripButton_sustituir_todos;
    private ToolStripSeparator toolStripSeparator16;
    private ToolStripSeparator toolStripSeparator12;

    public Form_TextEditor() => this.InitializeComponent();

    public string Fichero_editar
    {
      set => this._fichero_editar = value;
    }

    private void TextEditor_Load(object sender, EventArgs e)
    {
      try
      {
        if (this._fichero_editar == "")
          return;
        this.Document.LoadFile(this._fichero_editar, RichTextBoxStreamType.PlainText);
        this.Text = this._fichero_editar;
        this.Document.Focus();
        this.texto_salvado = true;
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Edit", false);
        if (registryKey == null)
          return;
        try
        {
          if ((string) registryKey.GetValue("Color") == "COLOR" && this.Document.TextLength < 2000)
            Form_TextEditor.cambiar_color(this.Document);
          string s = (string) registryKey.GetValue("Zoom_Factor");
          if (s != null)
            this.Document.ZoomFactor = (float) int.Parse(s);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Failed" + ex.Message);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("PROBLEM AT EDITING : " + ex.Message);
      }
    }

    private void Timer_Tick_1(object sender, EventArgs e)
    {
      this.charCount.Text = Resource_Form_TextEditor.String2 + this.Document.TextLength.ToString();
      ToolStripStatusLabel charCount = this.charCount;
      charCount.Text = charCount.Text + " , " + Resource_Form_TextEditor.String3 + this.Document.GetLineFromCharIndex(this.Document.TextLength).ToString();
      this.status_current_line.Text = Resource_Form_TextEditor.String4 + (this.Document.GetLineFromCharIndex(this.Document.SelectionStart) + 1).ToString();
    }

    private void file_New_Click(object sender, EventArgs e) => this.New();

    private void file_Open_Click(object sender, EventArgs e) => this.Open();

    private void file_Save_Click(object sender, EventArgs e)
    {
      this.tb_Save_Click((object) null, (EventArgs) null);
    }

    private void file_Exit_Click(object sender, EventArgs e) => this.Close();

    private void edit_Undo_Click(object sender, EventArgs e) => this.Undo();

    private void edit_Redo_Click(object sender, EventArgs e) => this.Redo();

    private void edit_Cut_Click(object sender, EventArgs e) => this.Cut();

    private void edit_Copy_Click(object sender, EventArgs e) => this.Copy();

    private void edit_Paste_Click(object sender, EventArgs e) => this.Paste();

    private void edit_SelectAll_Click(object sender, EventArgs e) => this.SelectAll();

    private void clearAllToolStripMenuItem_Click(object sender, EventArgs e) => this.ClearAll();

    private void tools_Customise_Click(object sender, EventArgs e) => this.customise();

    private void tb_New_Click(object sender, EventArgs e) => this.New();

    private void tb_Open_Click(object sender, EventArgs e) => this.Open();

    private void tb_Save_Click(object sender, EventArgs e)
    {
      try
      {
        this.Document.SaveFile(this._fichero_editar, RichTextBoxStreamType.PlainText);
        this.texto_salvado = true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void tb_Cut_Click(object sender, EventArgs e) => this.Cut();

    private void tb_Copy_Click(object sender, EventArgs e) => this.Copy();

    private void tb_Paste_Click(object sender, EventArgs e) => this.Paste();

    private void tb_ZoomIn_Click(object sender, EventArgs e)
    {
      if ((double) this.Document.ZoomFactor == 63.0)
        return;
      ++this.Document.ZoomFactor;
      this.memorizar_zoom((int) this.Document.ZoomFactor);
    }

    private void tb_ZoomOut_Click(object sender, EventArgs e)
    {
      if ((double) this.Document.ZoomFactor == 1.0)
        return;
      --this.Document.ZoomFactor;
      this.memorizar_zoom((int) this.Document.ZoomFactor);
    }

    private void memorizar_zoom(int valor_zoom)
    {
      try
      {
        (Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Edit", true) ?? Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config\\Edit")).SetValue("Zoom_Factor", (object) valor_zoom.ToString());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed" + ex.Message);
      }
    }

    private void tb_Bold_Click(object sender, EventArgs e)
    {
      Font font1 = new Font(this.Document.Font, FontStyle.Bold);
      Font font2 = new Font(this.Document.Font, FontStyle.Regular);
      if (this.Document.SelectedText.Length == 0)
        return;
      if (this.Document.SelectionFont.Bold)
        this.Document.SelectionFont = font2;
      else
        this.Document.SelectionFont = font1;
    }

    private void tb_Italic_Click(object sender, EventArgs e)
    {
      Font font1 = new Font(this.Document.Font, FontStyle.Italic);
      Font font2 = new Font(this.Document.Font, FontStyle.Regular);
      if (this.Document.SelectedText.Length == 0)
        return;
      if (this.Document.SelectionFont.Italic)
        this.Document.SelectionFont = font2;
      else
        this.Document.SelectionFont = font1;
    }

    private void tb_UnderLine_Click(object sender, EventArgs e)
    {
      Font font1 = new Font(this.Document.Font, FontStyle.Underline);
      Font font2 = new Font(this.Document.Font, FontStyle.Regular);
      if (this.Document.SelectedText.Length == 0)
        return;
      if (this.Document.SelectionFont.Underline)
        this.Document.SelectionFont = font2;
      else
        this.Document.SelectionFont = font1;
    }

    private void tb_Strike_Click(object sender, EventArgs e)
    {
      Font font1 = new Font(this.Document.Font, FontStyle.Strikeout);
      Font font2 = new Font(this.Document.Font, FontStyle.Regular);
      if (this.Document.SelectedText.Length == 0)
        return;
      if (this.Document.SelectionFont.Strikeout)
        this.Document.SelectionFont = font2;
      else
        this.Document.SelectionFont = font1;
    }

    private void tb_AlignLeft_Click(object sender, EventArgs e)
    {
      this.Document.SelectionAlignment = HorizontalAlignment.Left;
    }

    private void tb_AlignCenter_Click(object sender, EventArgs e)
    {
      this.Document.SelectionAlignment = HorizontalAlignment.Center;
    }

    private void tb_AlignRight_Click(object sender, EventArgs e)
    {
      this.Document.SelectionAlignment = HorizontalAlignment.Right;
    }

    private void tb_UpperCase_Click(object sender, EventArgs e)
    {
      this.Document.SelectedText = this.Document.SelectedText.ToUpper();
    }

    private void tb_LowerCase_Click(object sender, EventArgs e)
    {
      this.Document.SelectedText = this.Document.SelectedText.ToLower();
    }

    private void rc_Undo_Click(object sender, EventArgs e) => this.Undo();

    private void rc_Redo_Click(object sender, EventArgs e) => this.Redo();

    private void rc_Cut_Click(object sender, EventArgs e) => this.Cut();

    private void rc_Copy_Click(object sender, EventArgs e) => this.Copy();

    private void rc_Paste_Click(object sender, EventArgs e) => this.Paste();

    private void New() => this.Document.Clear();

    private void Open()
    {
      if (this.openWork.ShowDialog() != DialogResult.OK)
        return;
      this.Document.LoadFile(this.openWork.FileName, RichTextBoxStreamType.PlainText);
      this._fichero_editar = this.openWork.FileName;
      this.Text = this._fichero_editar;
      this.texto_salvado = true;
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Edit", false);
      if (registryKey != null)
      {
        try
        {
          string s = (string) registryKey.GetValue("Zoom_Factor");
          int num = 1;
          if (s != null)
            num = int.Parse(s);
          if ((string) registryKey.GetValue("Color") == "COLOR" && this.Document.TextLength < 2000)
            Form_TextEditor.cambiar_color(this.Document);
          else
            Form_TextEditor.cambiar_negro(this.Document);
          this.Document.ZoomFactor = 1f;
          this.Document.ZoomFactor = (float) num;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Failed" + ex.Message);
        }
      }
    }

    private void Save_as()
    {
      this.saveWork.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
      this.saveWork.ValidateNames = true;
      this.saveWork.FileName = Path.GetFileName(this._fichero_editar);
      if (this.saveWork.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        this.Document.SaveFile(this.saveWork.FileName, RichTextBoxStreamType.PlainText);
        this._fichero_editar = this.saveWork.FileName;
        this.Text = this._fichero_editar;
        this.texto_salvado = true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void Exit() => Application.Exit();

    private void Undo() => this.Document.Undo();

    private void Redo() => this.Document.Redo();

    private void Cut() => this.Document.Cut();

    private void Copy() => this.Document.Copy();

    private void Paste() => this.Document.Paste();

    private void SelectAll() => this.Document.SelectAll();

    private void ClearAll() => this.Document.Clear();

    private void customise()
    {
      ColorDialog colorDialog = new ColorDialog();
      if (colorDialog.ShowDialog() != DialogResult.OK)
        return;
      this.mainMenu.BackColor = colorDialog.Color;
      this.Status.BackColor = colorDialog.Color;
      this.Tools.BackColor = colorDialog.Color;
    }

    private void tb_busqueda_inicio_Click(object sender, EventArgs e)
    {
      this.toolStripTextBox_busqueda.Select(0, 0);
      string str = this.toolStripTextBox_busqueda.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
      if (str.Length > this.Document.TextLength)
        return;
      int num1 = this.Document.Text.IndexOf(str, 0, this.Document.Text.Length);
      if (num1 > 0)
      {
        this.Document.SelectionStart = num1;
        this.Document.SelectionLength = this.toolStripTextBox_busqueda.Text.Length;
        this.Document.ScrollToCaret();
        this.Document.Focus();
      }
      else
      {
        int num2 = (int) MessageBox.Show(Resource_Form_TextEditor.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.toolStripTextBox_busqueda.Text = str;
    }

    private void toolStripTextBox_busqueda_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.tb_busqueda_inicio_Click((object) null, (EventArgs) null);
      this.toolStripTextBox_busqueda.Select(0, 0);
    }

    private void toolStripButton_busqueda_desde_cursor_Click(object sender, EventArgs e)
    {
      this.toolStripTextBox_busqueda.Select(0, 0);
      string str = this.toolStripTextBox_busqueda.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
      if (str.Length > this.Document.TextLength)
        return;
      int selectionStart = this.Document.SelectionStart;
      int num1 = this.Document.Text.IndexOf(str, selectionStart + str.Length, this.Document.Text.Length - selectionStart - str.Length);
      if (num1 > 0)
      {
        this.Document.SelectionStart = num1;
        this.Document.SelectionLength = this.toolStripTextBox_busqueda.Text.Length;
        this.Document.ScrollToCaret();
        this.Document.Focus();
      }
      else
      {
        int num2 = (int) MessageBox.Show(Resource_Form_TextEditor.String1, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void toolStripTextBox_busqueda_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F3)
        return;
      this.toolStripButton_busqueda_desde_cursor_Click((object) null, (EventArgs) null);
      this.toolStripTextBox_busqueda.Select(0, 0);
    }

    private void Document_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F3)
        return;
      this.toolStripButton_busqueda_desde_cursor_Click((object) null, (EventArgs) null);
      this.toolStripTextBox_busqueda.Select(0, 0);
    }

    private void file_Print_Click(object sender, EventArgs e)
    {
      PrintDialog printDialog = new PrintDialog();
      PrintDocument printDocument = new PrintDocument();
      printDocument.DocumentName = "Print Document";
      printDocument.PrintPage += new PrintPageEventHandler(this.printDoc_PrintPage);
      printDialog.Document = printDocument;
      if (printDialog.ShowDialog() != DialogResult.OK)
        return;
      printDocument.Print();
    }

    private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
    {
      e.Graphics.DrawString(this.Document.Text, this.Document.Font, Brushes.Black, 10f, 25f);
    }

    private void toolStrip_introducir_espacios_Click(object sender, EventArgs e)
    {
      if (this.Document.SelectedText.Length > 0)
      {
        this.Document.SelectionColor = System.Drawing.Color.Black;
        this.Document.SelectedText = Form_TextEditor.introducir_espacios(this.Document.SelectedText);
      }
      else
      {
        this.Document.SelectionStart = 0;
        this.Document.SelectionLength = this.Document.Text.Length;
        this.Document.SelectionColor = System.Drawing.Color.Black;
        this.Document.Text = Form_TextEditor.introducir_espacios(this.Document.Text);
      }
    }

    private static string introducir_espacios(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = 0;
      int num2 = 0;
      bool flag1 = false;
      foreach (char ch in input)
      {
        if (ch == '(')
          flag1 = true;
        if (ch == ')')
          flag1 = false;
        if (ch == '\r' || ch == '\n')
        {
          flag1 = false;
          num1 = 0;
          num2 = 0;
        }
        bool flag2;
        if (ch >= 'A' && ch <= 'Z' && !flag1)
        {
          flag2 = true;
          ++num1;
          ++num2;
        }
        else
        {
          flag2 = false;
          num2 = 0;
        }
        if (ch == '/' && !flag1)
          ++num1;
        if (flag2 && num1 > 1 && !flag1 && num2 == 1)
          stringBuilder.Append(' ');
        stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    private void toolStrip_eliminar_espacios_Click(object sender, EventArgs e)
    {
      if (this.Document.SelectedText.Length > 0)
      {
        this.Document.SelectionColor = System.Drawing.Color.Black;
        this.Document.SelectedText = Form_TextEditor.eliminar_espacios(this.Document.SelectedText);
      }
      else
      {
        this.Document.SelectionStart = 0;
        this.Document.SelectionLength = this.Document.Text.Length;
        this.Document.SelectionColor = System.Drawing.Color.Black;
        this.Document.Text = Form_TextEditor.eliminar_espacios(this.Document.Text);
      }
    }

    private static string eliminar_espacios(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (char ch in input)
      {
        if (ch == '(')
          flag = true;
        if (ch == ')')
          flag = false;
        if (ch == '\r' || ch == '\n')
          flag = false;
        if (ch != ' ' || flag)
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    private void eliminarLineasVaciasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.Document.SelectedText.Length > 0)
        this.Document.SelectedText = Form_TextEditor.eliminar_lineas_vacias(this.Document.SelectedText);
      else
        this.Document.Text = Form_TextEditor.eliminar_lineas_vacias(this.Document.Text);
    }

    private static string eliminar_lineas_vacias(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      foreach (char ch in input)
      {
        if (ch == '\r' || ch == '\n')
        {
          if (num != 0)
            num = 0;
          else
            continue;
        }
        else
          ++num;
        stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    private void save_As_Click(object sender, EventArgs e) => this.Save_as();

    private void Document_TextChanged(object sender, EventArgs e) => this.texto_salvado = false;

    private void Form_TextEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.texto_salvado || MessageBox.Show(Resource_Form_TextEditor.String5, "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.Save_as();
    }

    private static void cambiar_color(RichTextBox rtb)
    {
      Cursor.Current = Cursors.WaitCursor;
      rtb.HideSelection = false;
      string str = rtb.Text.ToString();
      rtb.Text = "";
      bool flag = false;
      foreach (char ch in str)
      {
        rtb.SelectionColor = System.Drawing.Color.Red;
        if (ch == '(')
          flag = true;
        if (flag)
          rtb.SelectionColor = System.Drawing.Color.Green;
        if (ch == ')' || ch == '\r' || ch == '\n')
          flag = false;
        if (ch >= 'A' && ch <= 'Z' && !flag)
        {
          rtb.SelectionColor = System.Drawing.Color.Blue;
          if (ch == 'M' || ch == 'S' || ch == 'T')
            rtb.SelectionColor = System.Drawing.Color.DarkViolet;
        }
        if (ch >= '0' && ch <= '9' && !flag || ch == '.')
          rtb.SelectionColor = System.Drawing.Color.Black;
        rtb.AppendText(ch.ToString());
      }
      rtb.SelectionStart = 0;
      rtb.HideSelection = true;
      Cursor.Current = Cursors.Default;
    }

    private void toolStrip_text_color_Click(object sender, EventArgs e)
    {
      if (this.Document.TextLength < 2000)
      {
        Form_TextEditor.cambiar_color(this.Document);
        this.memorizar_color("COLOR");
      }
      else
      {
        int num = (int) MessageBox.Show(Resource_Form_TextEditor.String6, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void textInBlackToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Form_TextEditor.cambiar_negro(this.Document);
      this.memorizar_color("BLACK");
    }

    private static void cambiar_negro(RichTextBox rtb)
    {
      RichTextBox richTextBox = new RichTextBox();
      richTextBox.SelectionColor = System.Drawing.Color.Black;
      richTextBox.Text = rtb.Text;
      rtb.SelectionColor = System.Drawing.Color.Black;
      rtb.Text = richTextBox.Text;
    }

    private void memorizar_color(string color)
    {
      try
      {
        (Registry.CurrentUser.OpenSubKey("Software\\FANUC_Open_Com\\Config\\Edit", true) ?? Registry.CurrentUser.CreateSubKey("Software\\FANUC_Open_Com\\Config\\Edit")).SetValue("Color", (object) color);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed" + ex.Message);
      }
    }

    private void toolStripButton_sustituir_Click(object sender, EventArgs e)
    {
      if (this.Document.SelectedText == "")
      {
        int num1 = (int) MessageBox.Show(Resource_Form_TextEditor.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.Document.SelectedText = this.toolStripTextBox_sustituir.Text;
        int num2 = (int) MessageBox.Show(Resource_Form_TextEditor.String8);
      }
    }

    private void toolStripButton_sustituir_todos_Click(object sender, EventArgs e)
    {
      this.ReplaceAll(this.Document, this.toolStripTextBox_busqueda.Text, this.toolStripTextBox_sustituir.Text);
    }

    public void ReplaceAll(RichTextBox myRtb, string word, string replacement)
    {
      if (word == "" || replacement == "")
      {
        int num1 = (int) MessageBox.Show(Resource_Form_TextEditor.String7, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int num2 = 0;
        int num3 = 0;
        int num4 = replacement.Length - word.Length;
        foreach (Match match in Regex.Matches(myRtb.Text, word))
        {
          myRtb.Select(match.Index + num2, word.Length);
          num2 += num4;
          myRtb.SelectedText = replacement;
          ++num3;
        }
        int num5 = (int) MessageBox.Show(Resource_Form_TextEditor.String9 + num3.ToString());
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form_TextEditor));
      this.Tools = new ToolStrip();
      this.tb_New = new ToolStripButton();
      this.tb_Open = new ToolStripButton();
      this.tb_Save = new ToolStripButton();
      this.toolStripSeparator = new ToolStripSeparator();
      this.tb_Cut = new ToolStripButton();
      this.tb_Copy = new ToolStripButton();
      this.tb_Paste = new ToolStripButton();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.toolStripSeparator7 = new ToolStripSeparator();
      this.tb_UpperCase = new ToolStripButton();
      this.toolStripSeparator8 = new ToolStripSeparator();
      this.tb_ZoomIn = new ToolStripButton();
      this.tb_ZoomOut = new ToolStripButton();
      this.toolStripSeparator9 = new ToolStripSeparator();
      this.toolStripSeparator6 = new ToolStripSeparator();
      this.toolStripTextBox_busqueda = new ToolStripTextBox();
      this.tb_busqueda_inicio = new ToolStripButton();
      this.toolStripButton_busqueda_desde_cursor = new ToolStripButton();
      this.toolStripSeparator16 = new ToolStripSeparator();
      this.toolStripSeparator12 = new ToolStripSeparator();
      this.toolStripTextBox_sustituir = new ToolStripTextBox();
      this.toolStripButton_sustituir = new ToolStripButton();
      this.toolStripButton_sustituir_todos = new ToolStripButton();
      this.Status = new StatusStrip();
      this.charCount = new ToolStripStatusLabel();
      this.toolStripStatusLabel2 = new ToolStripStatusLabel();
      this.status_current_line = new ToolStripStatusLabel();
      this.Document = new RichTextBox();
      this.rcMenu = new ContextMenuStrip(this.components);
      this.rc_Undo = new ToolStripMenuItem();
      this.rc_Redo = new ToolStripMenuItem();
      this.toolStripSeparator10 = new ToolStripSeparator();
      this.rc_Cut = new ToolStripMenuItem();
      this.rc_Copy = new ToolStripMenuItem();
      this.rc_Paste = new ToolStripMenuItem();
      this.Timer = new Timer(this.components);
      this.openWork = new OpenFileDialog();
      this.saveWork = new SaveFileDialog();
      this.mM_File = new ToolStripMenuItem();
      this.file_New = new ToolStripMenuItem();
      this.file_Open = new ToolStripMenuItem();
      this.toolStripSeparator11 = new ToolStripSeparator();
      this.file_Save = new ToolStripMenuItem();
      this.save_As = new ToolStripMenuItem();
      this.toolStripSeparator13 = new ToolStripSeparator();
      this.file_Print = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.file_Exit = new ToolStripMenuItem();
      this.mM_Edit = new ToolStripMenuItem();
      this.edit_Undo = new ToolStripMenuItem();
      this.edit_Redo = new ToolStripMenuItem();
      this.toolStripSeparator14 = new ToolStripSeparator();
      this.edit_Cut = new ToolStripMenuItem();
      this.edit_Copy = new ToolStripMenuItem();
      this.edit_Paste = new ToolStripMenuItem();
      this.toolStripSeparator15 = new ToolStripSeparator();
      this.edit_SelectAll = new ToolStripMenuItem();
      this.toolStripSeparator5 = new ToolStripSeparator();
      this.toolStrip_introducir_espacios = new ToolStripMenuItem();
      this.toolStrip_eliminar_espacios = new ToolStripMenuItem();
      this.eliminarLineasVaciasToolStripMenuItem = new ToolStripMenuItem();
      this.toolStrip_text_color = new ToolStripMenuItem();
      this.textInBlackToolStripMenuItem = new ToolStripMenuItem();
      this.mM_Tools = new ToolStripMenuItem();
      this.tools_Customise = new ToolStripMenuItem();
      this.mainMenu = new MenuStrip();
      this.Tools.SuspendLayout();
      this.Status.SuspendLayout();
      this.rcMenu.SuspendLayout();
      this.mainMenu.SuspendLayout();
      this.SuspendLayout();
      this.Tools.ImageScalingSize = new Size(20, 20);
      this.Tools.Items.AddRange(new ToolStripItem[23]
      {
        (ToolStripItem) this.tb_New,
        (ToolStripItem) this.tb_Open,
        (ToolStripItem) this.tb_Save,
        (ToolStripItem) this.toolStripSeparator,
        (ToolStripItem) this.tb_Cut,
        (ToolStripItem) this.tb_Copy,
        (ToolStripItem) this.tb_Paste,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.toolStripSeparator7,
        (ToolStripItem) this.tb_UpperCase,
        (ToolStripItem) this.toolStripSeparator8,
        (ToolStripItem) this.tb_ZoomIn,
        (ToolStripItem) this.tb_ZoomOut,
        (ToolStripItem) this.toolStripSeparator9,
        (ToolStripItem) this.toolStripSeparator6,
        (ToolStripItem) this.toolStripTextBox_busqueda,
        (ToolStripItem) this.tb_busqueda_inicio,
        (ToolStripItem) this.toolStripButton_busqueda_desde_cursor,
        (ToolStripItem) this.toolStripSeparator16,
        (ToolStripItem) this.toolStripSeparator12,
        (ToolStripItem) this.toolStripTextBox_sustituir,
        (ToolStripItem) this.toolStripButton_sustituir,
        (ToolStripItem) this.toolStripButton_sustituir_todos
      });
      componentResourceManager.ApplyResources((object) this.Tools, "Tools");
      this.Tools.Name = "Tools";
      this.tb_New.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_New, "tb_New");
      this.tb_New.Name = "tb_New";
      this.tb_New.Click += new EventHandler(this.tb_New_Click);
      this.tb_Open.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_Open, "tb_Open");
      this.tb_Open.Name = "tb_Open";
      this.tb_Open.Click += new EventHandler(this.tb_Open_Click);
      this.tb_Save.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_Save, "tb_Save");
      this.tb_Save.Name = "tb_Save";
      this.tb_Save.Click += new EventHandler(this.tb_Save_Click);
      this.toolStripSeparator.Name = "toolStripSeparator";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator, "toolStripSeparator");
      this.tb_Cut.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_Cut, "tb_Cut");
      this.tb_Cut.Name = "tb_Cut";
      this.tb_Cut.Click += new EventHandler(this.tb_Cut_Click);
      this.tb_Copy.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_Copy, "tb_Copy");
      this.tb_Copy.Name = "tb_Copy";
      this.tb_Copy.Click += new EventHandler(this.tb_Copy_Click);
      this.tb_Paste.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_Paste, "tb_Paste");
      this.tb_Paste.Name = "tb_Paste";
      this.tb_Paste.Click += new EventHandler(this.tb_Paste_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator3, "toolStripSeparator3");
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator7, "toolStripSeparator7");
      this.tb_UpperCase.DisplayStyle = ToolStripItemDisplayStyle.Text;
      componentResourceManager.ApplyResources((object) this.tb_UpperCase, "tb_UpperCase");
      this.tb_UpperCase.Name = "tb_UpperCase";
      this.tb_UpperCase.Click += new EventHandler(this.tb_UpperCase_Click);
      this.toolStripSeparator8.Name = "toolStripSeparator8";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator8, "toolStripSeparator8");
      this.tb_ZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Text;
      componentResourceManager.ApplyResources((object) this.tb_ZoomIn, "tb_ZoomIn");
      this.tb_ZoomIn.Name = "tb_ZoomIn";
      this.tb_ZoomIn.Click += new EventHandler(this.tb_ZoomIn_Click);
      this.tb_ZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Text;
      componentResourceManager.ApplyResources((object) this.tb_ZoomOut, "tb_ZoomOut");
      this.tb_ZoomOut.Name = "tb_ZoomOut";
      this.tb_ZoomOut.Click += new EventHandler(this.tb_ZoomOut_Click);
      this.toolStripSeparator9.Name = "toolStripSeparator9";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator9, "toolStripSeparator9");
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator6, "toolStripSeparator6");
      this.toolStripTextBox_busqueda.BackColor = System.Drawing.Color.White;
      this.toolStripTextBox_busqueda.BorderStyle = BorderStyle.FixedSingle;
      componentResourceManager.ApplyResources((object) this.toolStripTextBox_busqueda, "toolStripTextBox_busqueda");
      this.toolStripTextBox_busqueda.Name = "toolStripTextBox_busqueda";
      this.toolStripTextBox_busqueda.KeyDown += new KeyEventHandler(this.toolStripTextBox_busqueda_KeyDown);
      this.toolStripTextBox_busqueda.KeyUp += new KeyEventHandler(this.toolStripTextBox_busqueda_KeyUp);
      this.tb_busqueda_inicio.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.tb_busqueda_inicio, "tb_busqueda_inicio");
      this.tb_busqueda_inicio.Name = "tb_busqueda_inicio";
      this.tb_busqueda_inicio.Click += new EventHandler(this.tb_busqueda_inicio_Click);
      this.toolStripButton_busqueda_desde_cursor.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.toolStripButton_busqueda_desde_cursor, "toolStripButton_busqueda_desde_cursor");
      this.toolStripButton_busqueda_desde_cursor.Name = "toolStripButton_busqueda_desde_cursor";
      this.toolStripButton_busqueda_desde_cursor.Click += new EventHandler(this.toolStripButton_busqueda_desde_cursor_Click);
      this.toolStripSeparator16.Name = "toolStripSeparator16";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator16, "toolStripSeparator16");
      this.toolStripSeparator12.Name = "toolStripSeparator12";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator12, "toolStripSeparator12");
      this.toolStripTextBox_sustituir.BackColor = System.Drawing.Color.White;
      this.toolStripTextBox_sustituir.BorderStyle = BorderStyle.FixedSingle;
      componentResourceManager.ApplyResources((object) this.toolStripTextBox_sustituir, "toolStripTextBox_sustituir");
      this.toolStripTextBox_sustituir.Name = "toolStripTextBox_sustituir";
      this.toolStripButton_sustituir.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.toolStripButton_sustituir, "toolStripButton_sustituir");
      this.toolStripButton_sustituir.Name = "toolStripButton_sustituir";
      this.toolStripButton_sustituir.Click += new EventHandler(this.toolStripButton_sustituir_Click);
      this.toolStripButton_sustituir_todos.DisplayStyle = ToolStripItemDisplayStyle.Image;
      componentResourceManager.ApplyResources((object) this.toolStripButton_sustituir_todos, "toolStripButton_sustituir_todos");
      this.toolStripButton_sustituir_todos.Name = "toolStripButton_sustituir_todos";
      this.toolStripButton_sustituir_todos.Click += new EventHandler(this.toolStripButton_sustituir_todos_Click);
      this.Status.ImageScalingSize = new Size(20, 20);
      this.Status.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.charCount,
        (ToolStripItem) this.toolStripStatusLabel2,
        (ToolStripItem) this.status_current_line
      });
      componentResourceManager.ApplyResources((object) this.Status, "Status");
      this.Status.Name = "Status";
      this.charCount.Name = "charCount";
      componentResourceManager.ApplyResources((object) this.charCount, "charCount");
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      componentResourceManager.ApplyResources((object) this.toolStripStatusLabel2, "toolStripStatusLabel2");
      this.toolStripStatusLabel2.Spring = true;
      this.status_current_line.Name = "status_current_line";
      componentResourceManager.ApplyResources((object) this.status_current_line, "status_current_line");
      this.Document.BackColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.Document.ContextMenuStrip = this.rcMenu;
      componentResourceManager.ApplyResources((object) this.Document, "Document");
      this.Document.Name = "Document";
      this.Document.TextChanged += new EventHandler(this.Document_TextChanged);
      this.Document.KeyUp += new KeyEventHandler(this.Document_KeyUp);
      this.rcMenu.ImageScalingSize = new Size(20, 20);
      this.rcMenu.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.rc_Undo,
        (ToolStripItem) this.rc_Redo,
        (ToolStripItem) this.toolStripSeparator10,
        (ToolStripItem) this.rc_Cut,
        (ToolStripItem) this.rc_Copy,
        (ToolStripItem) this.rc_Paste
      });
      this.rcMenu.Name = "rcMenu";
      componentResourceManager.ApplyResources((object) this.rcMenu, "rcMenu");
      this.rc_Undo.Name = "rc_Undo";
      componentResourceManager.ApplyResources((object) this.rc_Undo, "rc_Undo");
      this.rc_Undo.Click += new EventHandler(this.rc_Undo_Click);
      this.rc_Redo.Name = "rc_Redo";
      componentResourceManager.ApplyResources((object) this.rc_Redo, "rc_Redo");
      this.rc_Redo.Click += new EventHandler(this.rc_Redo_Click);
      this.toolStripSeparator10.Name = "toolStripSeparator10";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator10, "toolStripSeparator10");
      this.rc_Cut.Name = "rc_Cut";
      componentResourceManager.ApplyResources((object) this.rc_Cut, "rc_Cut");
      this.rc_Cut.Click += new EventHandler(this.rc_Cut_Click);
      this.rc_Copy.Name = "rc_Copy";
      componentResourceManager.ApplyResources((object) this.rc_Copy, "rc_Copy");
      this.rc_Copy.Click += new EventHandler(this.rc_Copy_Click);
      this.rc_Paste.Name = "rc_Paste";
      componentResourceManager.ApplyResources((object) this.rc_Paste, "rc_Paste");
      this.rc_Paste.Click += new EventHandler(this.rc_Paste_Click);
      this.Timer.Enabled = true;
      this.Timer.Interval = 500;
      this.Timer.Tick += new EventHandler(this.Timer_Tick_1);
      componentResourceManager.ApplyResources((object) this.openWork, "openWork");
      this.saveWork.AddExtension = false;
      componentResourceManager.ApplyResources((object) this.saveWork, "saveWork");
      this.mM_File.DropDownItems.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.file_New,
        (ToolStripItem) this.file_Open,
        (ToolStripItem) this.toolStripSeparator11,
        (ToolStripItem) this.file_Save,
        (ToolStripItem) this.save_As,
        (ToolStripItem) this.toolStripSeparator13,
        (ToolStripItem) this.file_Print,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.file_Exit
      });
      this.mM_File.Name = "mM_File";
      componentResourceManager.ApplyResources((object) this.mM_File, "mM_File");
      componentResourceManager.ApplyResources((object) this.file_New, "file_New");
      this.file_New.Name = "file_New";
      this.file_New.Click += new EventHandler(this.file_New_Click);
      componentResourceManager.ApplyResources((object) this.file_Open, "file_Open");
      this.file_Open.Name = "file_Open";
      this.file_Open.Click += new EventHandler(this.file_Open_Click);
      this.toolStripSeparator11.Name = "toolStripSeparator11";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator11, "toolStripSeparator11");
      componentResourceManager.ApplyResources((object) this.file_Save, "file_Save");
      this.file_Save.Name = "file_Save";
      this.file_Save.Click += new EventHandler(this.file_Save_Click);
      this.save_As.Name = "save_As";
      componentResourceManager.ApplyResources((object) this.save_As, "save_As");
      this.save_As.Click += new EventHandler(this.save_As_Click);
      this.toolStripSeparator13.Name = "toolStripSeparator13";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator13, "toolStripSeparator13");
      componentResourceManager.ApplyResources((object) this.file_Print, "file_Print");
      this.file_Print.Name = "file_Print";
      this.file_Print.Click += new EventHandler(this.file_Print_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator4, "toolStripSeparator4");
      this.file_Exit.Name = "file_Exit";
      componentResourceManager.ApplyResources((object) this.file_Exit, "file_Exit");
      this.file_Exit.Click += new EventHandler(this.file_Exit_Click);
      this.mM_Edit.DropDownItems.AddRange(new ToolStripItem[14]
      {
        (ToolStripItem) this.edit_Undo,
        (ToolStripItem) this.edit_Redo,
        (ToolStripItem) this.toolStripSeparator14,
        (ToolStripItem) this.edit_Cut,
        (ToolStripItem) this.edit_Copy,
        (ToolStripItem) this.edit_Paste,
        (ToolStripItem) this.toolStripSeparator15,
        (ToolStripItem) this.edit_SelectAll,
        (ToolStripItem) this.toolStripSeparator5,
        (ToolStripItem) this.toolStrip_introducir_espacios,
        (ToolStripItem) this.toolStrip_eliminar_espacios,
        (ToolStripItem) this.eliminarLineasVaciasToolStripMenuItem,
        (ToolStripItem) this.toolStrip_text_color,
        (ToolStripItem) this.textInBlackToolStripMenuItem
      });
      this.mM_Edit.Name = "mM_Edit";
      componentResourceManager.ApplyResources((object) this.mM_Edit, "mM_Edit");
      this.edit_Undo.Name = "edit_Undo";
      componentResourceManager.ApplyResources((object) this.edit_Undo, "edit_Undo");
      this.edit_Undo.Click += new EventHandler(this.edit_Undo_Click);
      this.edit_Redo.Name = "edit_Redo";
      componentResourceManager.ApplyResources((object) this.edit_Redo, "edit_Redo");
      this.edit_Redo.Click += new EventHandler(this.edit_Redo_Click);
      this.toolStripSeparator14.Name = "toolStripSeparator14";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator14, "toolStripSeparator14");
      componentResourceManager.ApplyResources((object) this.edit_Cut, "edit_Cut");
      this.edit_Cut.Name = "edit_Cut";
      this.edit_Cut.Click += new EventHandler(this.edit_Cut_Click);
      componentResourceManager.ApplyResources((object) this.edit_Copy, "edit_Copy");
      this.edit_Copy.Name = "edit_Copy";
      this.edit_Copy.Click += new EventHandler(this.edit_Copy_Click);
      componentResourceManager.ApplyResources((object) this.edit_Paste, "edit_Paste");
      this.edit_Paste.Name = "edit_Paste";
      this.edit_Paste.Click += new EventHandler(this.edit_Paste_Click);
      this.toolStripSeparator15.Name = "toolStripSeparator15";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator15, "toolStripSeparator15");
      this.edit_SelectAll.Name = "edit_SelectAll";
      componentResourceManager.ApplyResources((object) this.edit_SelectAll, "edit_SelectAll");
      this.edit_SelectAll.Click += new EventHandler(this.edit_SelectAll_Click);
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      componentResourceManager.ApplyResources((object) this.toolStripSeparator5, "toolStripSeparator5");
      componentResourceManager.ApplyResources((object) this.toolStrip_introducir_espacios, "toolStrip_introducir_espacios");
      this.toolStrip_introducir_espacios.Name = "toolStrip_introducir_espacios";
      this.toolStrip_introducir_espacios.Click += new EventHandler(this.toolStrip_introducir_espacios_Click);
      this.toolStrip_eliminar_espacios.Name = "toolStrip_eliminar_espacios";
      componentResourceManager.ApplyResources((object) this.toolStrip_eliminar_espacios, "toolStrip_eliminar_espacios");
      this.toolStrip_eliminar_espacios.Click += new EventHandler(this.toolStrip_eliminar_espacios_Click);
      this.eliminarLineasVaciasToolStripMenuItem.Name = "eliminarLineasVaciasToolStripMenuItem";
      componentResourceManager.ApplyResources((object) this.eliminarLineasVaciasToolStripMenuItem, "eliminarLineasVaciasToolStripMenuItem");
      this.eliminarLineasVaciasToolStripMenuItem.Click += new EventHandler(this.eliminarLineasVaciasToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) this.toolStrip_text_color, "toolStrip_text_color");
      this.toolStrip_text_color.Name = "toolStrip_text_color";
      this.toolStrip_text_color.Click += new EventHandler(this.toolStrip_text_color_Click);
      componentResourceManager.ApplyResources((object) this.textInBlackToolStripMenuItem, "textInBlackToolStripMenuItem");
      this.textInBlackToolStripMenuItem.Name = "textInBlackToolStripMenuItem";
      this.textInBlackToolStripMenuItem.Click += new EventHandler(this.textInBlackToolStripMenuItem_Click);
      this.mM_Tools.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tools_Customise
      });
      this.mM_Tools.Name = "mM_Tools";
      componentResourceManager.ApplyResources((object) this.mM_Tools, "mM_Tools");
      this.tools_Customise.Name = "tools_Customise";
      componentResourceManager.ApplyResources((object) this.tools_Customise, "tools_Customise");
      this.tools_Customise.Click += new EventHandler(this.tools_Customise_Click);
      this.mainMenu.ImageScalingSize = new Size(20, 20);
      this.mainMenu.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mM_File,
        (ToolStripItem) this.mM_Edit,
        (ToolStripItem) this.mM_Tools
      });
      componentResourceManager.ApplyResources((object) this.mainMenu, "mainMenu");
      this.mainMenu.Name = "mainMenu";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.Document);
      this.Controls.Add((Control) this.Status);
      this.Controls.Add((Control) this.Tools);
      this.Controls.Add((Control) this.mainMenu);
      this.MainMenuStrip = this.mainMenu;
      this.Name = nameof (Form_TextEditor);
      this.ShowIcon = false;
      this.FormClosing += new FormClosingEventHandler(this.Form_TextEditor_FormClosing);
      this.Load += new EventHandler(this.TextEditor_Load);
      this.Tools.ResumeLayout(false);
      this.Tools.PerformLayout();
      this.Status.ResumeLayout(false);
      this.Status.PerformLayout();
      this.rcMenu.ResumeLayout(false);
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
