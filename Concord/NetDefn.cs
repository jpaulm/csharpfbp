using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBPLib;
using Components;
using System.Windows.Forms;

namespace Concord
{
    class NetDefn : Network
    {
       public override void Define()
        {
            // StringWriter SwOut = new StringWriter();
            //string Filename = @"..\..\..\concord.txt";
            Component("Dir1", typeof(DirList));
            Component("Match", typeof(Match));
            Component("Show3", typeof(WriteTextBox));
            Component("Read", typeof(ReadStreamBlob));
            Component("Clean", typeof(CleanBlob));
            Component("Words", typeof(TextToWords));
            Component("DisplayWords", typeof(DisplayWords));
            Component("Sort", typeof(Sort));
            Component("Format", typeof(FormatConcord));
            Component("Show1", typeof(WriteTextBox));
            Component("Show2", typeof(WriteTextBox));

            Initialize(@"..\..\..\", "Dir1.IN");
            Initialize(@"fake_cy", "Match.CONFIG");
            Initialize(@"5,50", "Format.CONFIG");
            TextBox tb = parms[0] as TextBox;
            TextBox tb2 = parms[1] as TextBox;
            TextBox tb3 = parms[2] as TextBox;
            Initialize(tb, "Show1.DESTINATION");
            Initialize(tb2, "Show2.DESTINATION");
            Initialize(tb3, "Show3.DESTINATION");

            Connect("Dir1.OUTF", "Match.IN");
            Connect("Match.OUT", "Show3.IN");
            Connect("Show3.OUT", "Read.IN");
            Connect("Read.OUT", "Clean.IN");
            Connect("Clean.OUT", "Words.IN");
            Connect("Words.OUT", "DisplayWords.IN");
            Connect("DisplayWords.OUT", "Sort.IN");
            Connect("DisplayWords.OUTD", "Show1.IN");
            Connect("Sort.OUT", "Format.IN");
            Connect("Format.OUT", "Show2.IN");

            Component("Discard1", typeof(Discard));
            Component("Discard2", typeof(Discard));
            Component("Discard3", typeof(Discard));
            Component("Discard4", typeof(Discard));
            Connect("Dir1.OUT", "Discard1.IN");
            Connect("Dir1.OUTD", "Discard2.IN");
            Connect("Match.OUTN", "Discard3.IN");
            Connect("Words.OUTN", "Discard4.IN");
            Connect("Show1.OUT", "Discard4.IN");
            Connect("Show2.OUT", "Discard4.IN");


        }
    }
    }

