using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;
using HundredMilesSoftware.UltraID3Lib;
using libdb;


namespace libdb
{
    public partial class Album : libobj
    {
        public void Fill(DirectoryInfo dinfo, bool GetTagInfo)
        {
            if (!dinfo.Exists) throw new FileNotFoundException();


            UltraID3 u = new HundredMilesSoftware.UltraID3Lib.UltraID3();

            CueSharp.CueSheet c = new CueSharp.CueSheet("a");
            
        }

        
    }
}