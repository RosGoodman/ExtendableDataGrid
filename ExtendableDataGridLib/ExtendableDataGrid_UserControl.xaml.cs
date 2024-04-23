#nullable disable

using ExtendableDataGridLib.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExtendableDataGridLib;

/// <summary>
/// Логика взаимодействия для ExtendableDataGrid_UserControl.xaml
/// </summary>
public partial class ExtendableDataGrid_UserControl : UserControl
{
    public static readonly DependencyProperty ItemsProperty;
    public static readonly DependencyProperty SelectedItemProperty;

    static ExtendableDataGrid_UserControl()
    {
        ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(ObservableCollection<IExtendableModel>), typeof(ExtendableDataGrid_UserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsChanged))
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        SelectedItemProperty = DependencyProperty.RegisterAttached("SelectedItem", typeof(IExtendableModel), typeof(ExtendableDataGrid_UserControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsChanged))
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
    }

    public ExtendableDataGrid_UserControl()
    {
        InitializeComponent();

        Binding itemsBinding = new Binding("Items");
        itemsBinding.Source = this;
        itemsBinding.Mode = BindingMode.TwoWay;

        ExtendableDataGrid.SetBinding(ItemsProperty, itemsBinding);

        Binding selectedItemBinding = new Binding("SelectedItem");
        selectedItemBinding.Source = this;
        selectedItemBinding.Mode = BindingMode.TwoWay;

        ExtendableDataGrid.SetBinding(SelectedItemProperty, selectedItemBinding);
    }

    /// <summary>
    /// Коллекция элементов.
    /// </summary>
    public ObservableCollection<IExtendableModel> Items
    {
        get => (ObservableCollection<IExtendableModel>)GetValue(ItemsProperty);
        set
        {
            SetValue(ItemsProperty, value);
            CreateTable(value);
        }
    }

    /// <summary>
    /// Выбранный элемент коллекции группируемых элементов.
    /// </summary>
    public IExtendableModel SelectedItem
    {
        get => (IExtendableModel)GetValue(SelectedItemProperty);
        set
        {
            SetValue(SelectedItemProperty, value);
        }
    }

    /// <summary>
    /// Создать таблицу.
    /// </summary>
    /// <param name="items"> Коллекция элементов. </param>
    private void CreateTable(ObservableCollection<IExtendableModel> items)
    {
        ExtendableDataGrid.Columns.Clear();
        if (items is null || items.Count == 0) return;

        var properties = items[0].GetAllPropertyNames();
        if(properties is null || properties.Length == 0) return;

        foreach (var item in properties)
        {
            ExtendableDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = item,
                Binding = new Binding()
                {
                    Path = new PropertyPath($"[{item}]"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                },
            });
        }

        ExtendableDataGrid.ItemsSource = items;
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedItem = (IExtendableModel)ExtendableDataGrid.SelectedItem;
    }

    private static void OnIsChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        try
        {
            ExtendableDataGrid_UserControl current = (ExtendableDataGrid_UserControl)sender;
            current.Items = (ObservableCollection<IExtendableModel>)e.NewValue;
        }
        catch (Exception ex) { }
    }
}
