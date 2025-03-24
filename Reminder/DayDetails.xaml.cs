using Reminder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for DayDetails.xaml
    /// </summary>
    public partial class DayDetails : Window
    {
        CalendarDayModel model;
        Action<CalendarDayModel> applyCallback;

        public DayDetails(CalendarDayModel dayModel, Action<CalendarDayModel> applyCallback)
        {
            InitializeComponent();

            this.model = dayModel;
            this.applyCallback = applyCallback;

            TitleTextBlox.Text = dayModel.Title;
            NotesTextBlox.Text = dayModel.Notes;
            DateTextBlock.Text = "TODO Month/Day/Year";
            //TODO add alarm data
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            model.Title = TitleTextBlox.Text;
            model.Notes = NotesTextBlox.Text;
            //model.Alarms = TODO

            //TODO save to db

            applyCallback(model);
            this.Close();
        }
    }
}
