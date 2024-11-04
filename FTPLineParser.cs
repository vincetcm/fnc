// Decompiled with JetBrains decompiler
// Type: FANUC_Open_Com.FTPLineParser
// Assembly: Communication Software for FANUC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C808FFA-3D86-4A9B-B076-4FAA6D5E761B
// Assembly location: C:\Users\Superuser\Downloads\Communication_Software_for_FANUC_CNC_V94_2\Application Files\Communication Software for FANUC_94_0_0_2\Communication Software for FANUC.exe

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace FANUC_Open_Com
{
  public class FTPLineParser
  {
    private Regex regex_data_server = new Regex("^(?<dir>[-dl])(?<ownerSec>[-r][-w][-x])(?<groupSec>[-r][-w][-x])(?<everyoneSec>[-r][-w][-x])\\s+(?:\\d)\\s+(?<owner>\\w+)\\s+(?<group>\\w+)\\s+(?<size>\\d+)\\s+(?<month>\\w+)\\s+(?<day>\\d{1,2})\\s+(?<year_time>[0-9]+[:]?[0-9]*)\\s+(?<name>.*)$");

    public FTPLineResult Parse(string line)
    {
      Match match = this.regex_data_server.Match(line);
      return match.Success ? this.ParseMatch(match.Groups) : throw new Exception("Invalid FTP list directory details format");
    }

    private FTPLineResult ParseMatch(GroupCollection matchGroups)
    {
      return new FTPLineResult()
      {
        IsDirectory = matchGroups["dir"].Value.Equals("d", StringComparison.InvariantCultureIgnoreCase),
        Name = matchGroups["name"].Value,
        Size = matchGroups["size"].Value,
        Day = matchGroups["day"].Value,
        Month = matchGroups["month"].Value,
        Year_time = matchGroups["year_time"].Value
      };
    }
  }
}
