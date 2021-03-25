using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BottomSheetSample
{
    public partial class BottomSheetPage : ContentPage
    {
        private double previewSheetY,
                       panDelta;

        public BottomSheetPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Normally don't do this. If you know the height of your content
            // beforehand just set the layout bounds in your xaml. If your content
            // height is dynamic, subscribe to an event that you can emit from your
            // viewmodel when the content changes and then set the layout bounds.
            // I set my layout bounds to way outside the view beforehand and then
            // update the bounds based on the content by using weakeventmanager
            // from xamarin community toolkit.
            await Task.Delay(300);
            AbsoluteLayout.SetLayoutFlags(BottomSheetPanView, AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(BottomSheetPanView, new Rectangle(0, OuterLayout.Height, 1, AbsoluteLayout.AutoSize));
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            SheetBack.IsVisible = true;

            await Task.Delay(50);

            await Task.WhenAll(
                SheetBack.FadeTo(0.7, 300, Easing.CubicOut),
                BottomSheetPanView.TranslateTo(0, -PreviewSheet.Height, 300, Easing.CubicOut)
            );
        }

        private async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            await ChangeSheetPosition(BottomSheetPosition.Closed);
        }

        private async void PanGestureRecognizer_PanUpdated(Object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    panDelta = Math.Max(Math.Min(PreviewSheet.Height, previewSheetY + e.TotalY), 0) -
                               PreviewSheet.TranslationY;

                    PreviewSheet.TranslationY = Math.Max(Math.Min(PreviewSheet.Height, previewSheetY + e.TotalY), 0);
                    SheetBack.Opacity = (1 - Math.Abs(PreviewSheet.TranslationY) / PreviewSheet.Height) * 0.7;
                    break;
                case GestureStatus.Completed:
                    if (panDelta > 5 || PreviewSheet.TranslationY >= PreviewSheet.Height * 0.65)
                    {
                        await ChangeSheetPosition(BottomSheetPosition.Closed);
                    }
                    else
                    {
                        await ChangeSheetPosition(BottomSheetPosition.FullOpen);
                    }
                    break;

            }
        }

        private async Task ChangeSheetPosition(BottomSheetPosition position)
        {
            switch (position)
            {
                case BottomSheetPosition.Closed:
                    await Task.WhenAll(
                        SheetBack.FadeTo(0, 300, Easing.CubicOut),
                        PreviewSheet.TranslateTo(0, PreviewSheet.Height, 300, Easing.CubicOut)
                    );
                    previewSheetY = 0;
                    SheetBack.IsVisible = false;
                    BottomSheetPanView.TranslationY = 0;
                    PreviewSheet.TranslationY = 0;
                    break;
                case BottomSheetPosition.FullOpen:
                    await Task.WhenAll(
                        SheetBack.FadeTo(0.7, 300, Easing.CubicOut),
                        PreviewSheet.TranslateTo(0, 0, 300, Easing.CubicOut)
                    );
                    previewSheetY = PreviewSheet.TranslationY;
                    break;
            }
        }
    }
}
