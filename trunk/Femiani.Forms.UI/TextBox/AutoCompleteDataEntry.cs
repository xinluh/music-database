using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomForm
{
    public class AutoCompleteDataEntry : IAutoCompleteEntry
    {
        IList list;
        int value_member;
        int display_member;
        int match_member;

        public AutoCompleteDataEntry(IList List,int ValueMember, int DisplayMember,int MatchMember)
        {
            list = List;
            
            if (ValueMember >= list.Count || DisplayMember >= list.Count || MatchMember >= list.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            value_member = ValueMember;
            display_member = DisplayMember;
            match_member = MatchMember;
        }
        public AutoCompleteDataEntry(int ValueMember, int DisplayMember, int MatchMember)
        {
            list = null;
            value_member = ValueMember;
            display_member = DisplayMember;
            match_member = MatchMember;
        }


        /// <summary>
        /// The match string can be optionally splitted to match strings using this separator
        /// </summary>
        public string[] SplitSeparator { get; set; }
        public IList Items { 
            get { return this.list; } 
            set 
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }
                if (value_member >= list.Count || display_member >= list.Count || match_member >= list.Count)
                {
                    throw new ArgumentOutOfRangeException();
                } 
                this.list = value;
            }
        }
        public object ValueMember { get { return list[value_member]; } }
        public string DisplayMember { get { return list[display_member].ToString(); } }
        public string[] MatchMember
        {
            get
            {
                if (SplitSeparator != null)
                {
                    return list[match_member].ToString().Split(SplitSeparator, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    return new string[] {list[match_member].ToString()};
                }
            }
        }

        public string[] MatchStrings
        {
            get { return MatchMember; }
        }

        public override string ToString()
        {
            return DisplayMember;
        }

        public AutoCompleteDataEntry Clone()
        {
            return new AutoCompleteDataEntry(this.list, value_member, display_member, match_member);
        }
    }
}
