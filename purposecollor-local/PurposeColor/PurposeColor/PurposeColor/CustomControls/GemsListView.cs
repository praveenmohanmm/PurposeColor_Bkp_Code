﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    public class GemsListView : ListView
    {
        public Action<int> Scroll { get; set; }
    }
}
