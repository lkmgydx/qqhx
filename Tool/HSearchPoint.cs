namespace Tool
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class HSearchPoint
    {
        private System.Drawing.Point _findPoint;
        private bool _isSuccess;
        private Size _searchSize;
        private bool centerInit;
        private System.Drawing.Point centerPoint;

        public HSearchPoint(bool success, System.Drawing.Point point, Size size, int times)
        {
            this._isSuccess = success;
            this._searchSize = size;
            this._findPoint = point;
            this.Times = times;
        }

        public System.Drawing.Point CenterPoint
        {
            get
            {
                if (!this.centerInit)
                {
                    this.centerInit = true;
                    this.centerPoint = new System.Drawing.Point(this.Point.X + (this.SearchSize.Width / 2), this.Point.Y + (this.SearchSize.Height / 2));
                }
                return this.centerPoint;
            }
            private set
            {
                this.centerPoint = value;
            }
        }

        public System.Drawing.Point Point
        {
            get
            {
                return this._findPoint;
            }
        }

        public Size SearchSize
        {
            get
            {
                return this._searchSize;
            }
        }

        public bool Success
        {
            get
            {
                return this._isSuccess;
            }
        }

        public int Times { get; private set; }
    }
}

