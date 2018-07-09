using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFlipView.Controls
{
    public class FlipView : ContentView
    {
        public FlipView()
        {
            Flip += FlipView_Flip;
        }

        #region Bindable properties
        public static readonly BindableProperty FrontViewProperty =
            BindableProperty.Create("FrontView", typeof(View), typeof(FlipView), default(View),
                propertyChanged:
                (bindable, oldValue, newValue) =>
                {
                    FlipView fv = bindable as FlipView;
                    fv.Content = newValue as View;
                });

        public View FrontView
        {
            get { return (View)GetValue(FrontViewProperty); }
            set { SetValue(FrontViewProperty, value); }
        }


        public static readonly BindableProperty BackViewProperty =
            BindableProperty.Create("BackView", typeof(View), typeof(FlipView), default(View),
                propertyChanged:
                (bindable, oldValue, newValue) =>
                {
                    FlipView fv = bindable as FlipView;
                    if (fv.FrontView == null && newValue != null)
                    {
                        fv.Content = newValue as View;
                        fv.IsFlipped = true;
                    }
                });

        public View BackView
        {
            get { return (View)GetValue(BackViewProperty); }
            set { SetValue(BackViewProperty, value); }
        }

        public static readonly BindableProperty IsFlippedProperty =
            BindableProperty.Create("IsFlipped", typeof(bool), typeof(FlipView), default(bool),
                propertyChanged:
                (bindable, oldValue, newValue) =>
                {
                    FlipView fv = bindable as FlipView;
                    fv.OnFlip((bool)newValue);
                });

        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }
        #endregion

        //Prevent problems with animation while it is in action
        private bool FlipAnimationStopped = true;

        //Flip event rise on IsFlipped value changed
        private event EventHandler<bool> Flip;
        private void OnFlip(bool flipped)
        {
            Flip?.Invoke(this, flipped);
        }

        /// <summary>
        /// Flip card to another side 
        /// </summary>
        public void FlipCard()
        {
            if (!FlipAnimationStopped)
                return;

            //Turn animation on IsFlipped changed. See IsFlippedProperty BindableProperty
            IsFlipped = !IsFlipped;
        }

        /// <summary>
        /// Rotation animation and changing content of the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Is Flipped value</param>
        private async void FlipView_Flip(object sender, bool e)
        {
            if (e)
            {
                FlipAnimationStopped = false;
                await this.RotateYTo(90, 250);
                RotationY = 270;

                Content = BackView;

                await this.RotateYTo(360, 250);
                RotationY = 0;

                FlipAnimationStopped = true;
            }
            else
            {
                FlipAnimationStopped = false;

                await this.RotateYTo(90, 250);
                RotationY = 270;

                Content = FrontView;

                await this.RotateYTo(360, 250);
                RotationY = 0;

                FlipAnimationStopped = true;
            }
        }
    }
}
