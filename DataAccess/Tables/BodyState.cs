﻿using Models;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DataAccess.Tables
{
    [Table]
    public class BodyState : RaisableObject
    {
        #region Fields

        private DateTime date;
        private float height;
        private float weight;
        private float waist;
        private float hips;
        private float chest;
        private float wrist;
        private float neck;
        private byte[] image;
        private BitmapImage userImage;

        #endregion Fields

        #region Columns

        [Column(Storage = "Date", DbType = "DateTime NOT NULL", IsPrimaryKey = true)]
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        [Column(Storage = "Height", DbType = "Float")]
        public float Height
        {
            get { return height; }
            set
            {
                height = value;
                RaisePropertyChanged("Height");
            }
        }

        [Column(Storage = "Weight", DbType = "Float")]
        public float Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                RaisePropertyChanged("Weight");
            }
        }

        [Column(Storage = "Waist", DbType = "Float")]
        public float Waist
        {
            get { return waist; }
            set
            {
                waist = value;
                RaisePropertyChanged("Waist");
            }
        }

        [Column(Storage = "Hips", DbType = "Float")]
        public float Hips
        {
            get { return hips; }
            set
            {
                hips = value;
                RaisePropertyChanged("Hips");
            }
        }

        [Column(Storage = "Chest", DbType = "Float")]
        public float Chest
        {
            get { return chest; }
            set
            {
                chest = value;
                RaisePropertyChanged("Chest");
            }
        }

        [Column(Storage = "Wrist", DbType = "Float")]
        public float Wrist
        {
            get { return wrist; }
            set
            {
                wrist = value;
                RaisePropertyChanged("Wrist");
            }
        }

        [Column(Storage = "Neck", DbType = "Float")]
        public float Neck
        {
            get { return neck; }
            set
            {
                neck = value;
                RaisePropertyChanged("Neck");
            }
        }

        [Column(Storage = "Image", DbType = "image")]
        public byte[] Image
        {
            get { return image; }
            set
            {
                image = value;
                userImage = null;
                RaisePropertyChanged("Image");
            }
        }

        #endregion Columns

        public BitmapImage GetUserImage()
        {
            if (userImage != null)
            {
                return userImage;
            }

            if (image != null)
            {
                MemoryStream stream = new MemoryStream(image);
                userImage = new BitmapImage();
                userImage.SetSource(stream);
            }

            return userImage;
        }

        public BodyState CreateCopy()
        {
            return new BodyState
            {
                Height = this.Height,
                Hips = this.Hips,
                Waist = this.Waist,
                Weight = this.Weight,
                Image = this.Image,
                Neck = this.Neck,
                Wrist = this.Wrist,
                Chest = this.Chest
            };
        }
    }
}
