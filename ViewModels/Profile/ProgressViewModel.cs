using DataAccess.Repositories;
using DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ViewModels
{
    public enum DiagramType
    {
        Weight,
        Waist,
        Hips,
        Chest,
        Photo
    }

    public class ProgressViewModel : ViewModel
    {
        private const int ItemCount = 20;
        private List<BodyState> bodyStates = new List<BodyState>();

        #region SelectedType

        private DiagramType selectedType;

        public DiagramType SelectedType
        {
            get { return selectedType; }
            set
            {
                if (value != selectedType)
                {
                    selectedType = value;
                    RaisePropertyChanged("SelectedType");
                    OnSelectedTypeChanged();
                }
            }
        }

        public Type DiagramType
        {
            get { return typeof(DiagramType); }
        }

        #endregion SelectedType

        #region Items

        private ObservableCollection<ChartItem> items = new ObservableCollection<ChartItem>();

        public ObservableCollection<ChartItem> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged("Items");
            }
        }

        #endregion Items

        #region IsPhotoVisible

        private bool isPhotoVisible = false;

        public bool IsPhotoVisible
        {
            get { return isPhotoVisible; }
            set
            {
                isPhotoVisible = value;
                RaisePropertyChanged("IsPhotoVisible");
            }
        }

        #endregion IsPhotoVisible

        #region Photos

        private ObservableCollection<ImageSource> photos = new ObservableCollection<ImageSource>();

        public ObservableCollection<ImageSource> Photos
        {
            get { return photos; }
            set
            {
                photos = value;
                RaisePropertyChanged("Photos");
            }
        }

        #endregion Photos

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            IsPhotoVisible = false;
            RunInBackground(() =>
            {
                using (var repo = new BodyStateRepository())
                {
                    bodyStates = repo.GetListForProgress(20);
                    selectedType = ViewModels.DiagramType.Weight;
                    InvokeInUIThread(() => OnSelectedTypeChanged());
                }
            });
        }

        private void OnSelectedTypeChanged()
        {
            this.Items.Clear();
            switch (selectedType)
            {
                case ViewModels.DiagramType.Weight:
                    foreach (var item in bodyStates)
                    {
                        this.Items.Add(new ChartItem { Date = item.Date.ToShortDateString(), Value = item.Weight });
                    }

                    break;
                case ViewModels.DiagramType.Waist:
                    foreach (var item in bodyStates)
                    {
                        this.Items.Add(new ChartItem { Date = item.Date.ToShortDateString(), Value = item.Waist });
                    }

                    break;
                case ViewModels.DiagramType.Hips:
                    foreach (var item in bodyStates)
                    {
                        this.Items.Add(new ChartItem { Date = item.Date.ToShortDateString(), Value = item.Hips });
                    }

                    break;
                case ViewModels.DiagramType.Chest:
                    foreach (var item in bodyStates)
                    {
                        this.Items.Add(new ChartItem { Date = item.Date.ToShortDateString(), Value = item.Chest });
                    }

                    break;
                case ViewModels.DiagramType.Photo:
                    IsPhotoVisible = true;
                    if (this.Photos.Count == 0)
                    {
                        foreach (var item in bodyStates)
                        {
                            this.Photos.Add(item.GetUserImage());
                        }
                    }

                    break;
            }
        }
    }

    public class ChartItem
    {
        public string Date { get; set; }

        public Object Value { get; set; }
    }

}
