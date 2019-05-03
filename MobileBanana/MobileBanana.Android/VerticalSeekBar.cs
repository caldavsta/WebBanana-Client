using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MobileBanana.Droid
{
    public class VerticalSeekBar : SeekBar
    {


        public VerticalSeekBar(Context context) :
            base(context)
        {

        }
        public VerticalSeekBar(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {

        }

        public VerticalSeekBar(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {

        }
        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            canvas.Rotate(-90);
            canvas.Translate(-Height, 0);

            base.OnDraw(canvas);
        }

        public void SetProgress(int progress)
        {
            base.Progress = progress;
        }
    }
}