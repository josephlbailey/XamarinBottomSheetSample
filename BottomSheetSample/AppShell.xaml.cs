using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BottomSheetSample
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(BottomSheetPage), typeof(BottomSheetPage));
        }

    }
}
