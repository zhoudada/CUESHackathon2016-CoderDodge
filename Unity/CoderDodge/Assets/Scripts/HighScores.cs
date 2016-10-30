using System;
using System.Text;

public struct HighScores
{
    public int first;
    public int second;
    public int third;

    public override string ToString()
    {
        StringBuilder ss = new StringBuilder();
        ss.AppendLine("HighScores:");
        ss.AppendLine(string.Format("1.   {0}", first));
        ss.AppendLine(string.Format("2.   {0}", second));
        ss.AppendLine(string.Format("3.   {0}", third));
        return ss.ToString();
    }
}
