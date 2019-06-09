//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using Xamarin.Forms;

//namespace XamarinHelperLib.Controls
//{
//    public class CarouselViewManagerExperiment : CarouselView
//    {
//        bool waitingForBindingContext = false;

//        private int realItemIndex = 0;
//        private int viewIndex = 0;
//        private int previousViewIndex;

//        public int InNextItemCount = 10;
//        public int InPreviousItemCount = 10;
//        public int ViewCacheCount = 10;
//        public int DataCacheCount = 10;

//        public object CurrentItem;

//        public Func<object, int, int, object> NextItemFactory;
//        public Func<object, int, int, object> PreviousItemFactory;

//        public CarouselViewManagerExperiment()
//        {
//            Dictionary<Element, IDisposable> activatedViews
//                = new Dictionary<Element, IDisposable>();
//        }

//        public static readonly BindableProperty ItemsSourceProperty =
//            BindableProperty.Create(
//                "ItemsSource",
//                typeof(IEnumerable),
//                typeof(CarouselViewManagerExperiment),
//                defaultValue: null,
//                defaultBindingMode: BindingMode.OneWay,
//                propertyChanged: ItemsChanged);


//        public static readonly BindableProperty ItemTemplateProperty =
//            BindableProperty.Create(
//                "ViewModel",
//                typeof(DataTemplate),
//                typeof(CarouselViewManagerExperiment),
//                defaultValue: null,
//                defaultBindingMode: BindingMode.OneWay);


//        public IEnumerable ItemsSource
//        {
//            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
//            set { SetValue(ItemsSourceProperty, value); }
//        }

//        public DataTemplate ItemTemplate
//        {
//            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
//            set { SetValue(ItemTemplateProperty, value); }
//        }


//        protected override void OnBindingContextChanged()
//        {
//            base.OnBindingContextChanged();

//            if (BindingContext != null && waitingForBindingContext && ItemsSource != null)
//            {
//                ItemsChanged(this, null, ItemsSource);
//            }
//        }

//        private static void ItemsChanged(BindableObject bindable, object old, object newVal)
//        {
//            IEnumerable oldValue = old as IEnumerable;
//            IEnumerable newValue = newVal as IEnumerable;

//            var control = (CarouselViewManagerExperiment)bindable;

//            var oldObservableCollection = oldValue as INotifyCollectionChanged;

//            if (oldObservableCollection != null)
//            {
//                oldObservableCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
//            }

//            //HACK:SHANE
//            if (control.BindingContext == null)
//            {
//                control.waitingForBindingContext = true;
//                //this means this control has been removed from the visual tree
//                //so don't update it other wise you get random null reference exceptions
//                return;
//            }

//            control.waitingForBindingContext = false;

//            var newObservableCollection = newValue as INotifyCollectionChanged;

//            if (newObservableCollection != null)
//            {
//                newObservableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
//            }

//            try
//            {
//                control.Children.Clear();

//                if (newValue != null)
//                {
//                    foreach (var item in newValue)
//                    {
//                        var view = control.CreateChildViewFor(item);
//                        control.Children.Add(view);
//                        control.OnItemCreated(view);
//                    }
//                }

//                control.UpdateChildrenLayout();
//                control.InvalidateLayout();
//            }
//            catch (NullReferenceException)
//            {
//                try
//                {
//                    Debug.WriteLine(
//                        String.Format($"RepeaterView: NullReferenceException Parent:{control.Parent} ParentView:{control.Parent} IsVisible:{control.IsVisible}")
//                    );
//                }
//                catch (Exception exc)
//                {
//                    Debug.WriteLine($"NullReferenceException Logging Failed {exc}");
//                }
//            }
//        }



//        protected virtual void OnItemCreated(View view)
//        {
//            //if (this.ItemCreated != null)
//            //{
//            //    ItemCreated.Invoke(this, new RepeaterViewItemAddedEventArgs(view, view.BindingContext));
//            //}
//        }

//        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            try
//            {
//                var invalidate = false;

//                List<View> createdViews = new List<View>();
//                if (e.Action == NotifyCollectionChangedAction.Reset)
//                {
//                    var list = sender as IEnumerable;


//                    this.Children.SyncList(
//                        list,
//                        (item) =>
//                        {
//                            var view = this.CreateChildViewFor(item);
//                            createdViews.Add(view);
//                            return view;
//                        }, (item, view) => view.BindingContext == item,
//                        null);

//                    foreach (View view in createdViews)
//                    {
//                        OnItemCreated(view);
//                    }

//                    invalidate = true;
//                }

//                if (e.OldItems != null)
//                {
//                    this.Children.RemoveAt(e.OldStartingIndex);
//                    invalidate = true;
//                }

//                if (e.NewItems != null)
//                {
//                    for (var i = 0; i < e.NewItems.Count; ++i)
//                    {
//                        var item = e.NewItems[i];
//                        var view = this.CreateChildViewFor(item);

//                        this.Children.Insert(i + e.NewStartingIndex, view);
//                        OnItemCreated(view);
//                    }

//                    invalidate = true;
//                }

//                if (invalidate)
//                {
//                    this.UpdateChildrenLayout();
//                    this.InvalidateLayout();
//                }
//            }
//            catch (NullReferenceException)
//            {
//                try
//                {
//                    Debug.WriteLine(
//                        $"RepeaterView.OnItemsSourceCollectionChanged: NullReferenceException Parent:{Parent} ParentView:{Parent} IsVisible:{IsVisible} BindingContext: {BindingContext}"
//                    );
//                }
//                catch (Exception exc)
//                {
//                    Debug.WriteLine($"OnItemsSourceCollectionChanged: NullReferenceException Logging Failed {exc}");
//                }
//            }
//        }

//        private View CreateChildViewFor(object item)
//        {
//            this.ItemTemplate.SetValue(BindableObject.BindingContextProperty, item);

//            if (this.ItemTemplate is DataTemplateSelector)
//            {
//                var dts = (DataTemplateSelector)this.ItemTemplate;
//                return (View)dts.SelectTemplate(item, null).CreateContent();
//            }
//            else
//            {
//                return (View)this.ItemTemplate.CreateContent();
//            }
//        }

//        protected override void LayoutChildren(double x, double y, double width, double height)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

