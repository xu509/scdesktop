using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace scdesktop
{

    /// <summary>
    ///  Ball  Data
    /// </summary>
    public class BallData 
    {
        int _id;
        string _cover;
        string _detailCover;

        public int id { set { _id = value; } get { return _id; } }
        public string cover { set { _cover = value; } get { return _cover; } }
        public string detailCover { set { _detailCover = value; } get { return _detailCover; } }

        public override string ToString()
        {
            string str = "";

            str += "_cover : " + _cover + " |";
            str += "_detailCover : " + _detailCover;


            return str;
        }

    }
}
