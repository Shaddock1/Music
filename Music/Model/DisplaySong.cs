using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Model
{
    public class DisplaySong
    {
        private int _Id;
        public int Id {
            set
            {
                this._Id = value;
            }
            get 
            {
                return this._Id;
            }
        }

        private string _name;
        public string Name
        {
            set
            {
                this._name = value;
            }
            get
            {
                return this._name;
            }
        }

        private string _playtimer;
        public string SongTime
        {
            set
            {
                this._playtimer = value;
            }
            get
            {
                return this._playtimer;
            }
        }
        
    }


}
