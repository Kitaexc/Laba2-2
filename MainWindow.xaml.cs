using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Prakt_two
{
    public partial class MainWindow : Window
    {

        private readonly string _filePath = "notes.json";
        private List<Note> _notes = new List<Note>();

        public MainWindow()
        {
            InitializeComponent();
            data.SelectedDate = DateTime.Now;
        }
        private void LoadNotes(DateTime? selectedDate)
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _notes = JsonConvert.DeserializeObject<List<Note>>(json) ?? new List<Note>();
                if (selectedDate != null)
                {
                   
                    var filteredNotes = _notes.Where(n => n.Date.Date == selectedDate.Value.Date).ToList();
                    vse.ItemsSource = filteredNotes.Select(n => n.Name).ToList();
                }
                else
                {
                    vse.ItemsSource = null;
                }
            }
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            LoadNotes(datePicker.SelectedDate);
        }

        private void SaveNotes()
        {
            string json = JsonConvert.SerializeObject(_notes, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            var note = new Note
            {
                Name = name.Text,
                Description = opis.Text,
                Date = data.SelectedDate ?? DateTime.Now 
            };

            _notes.Add(note);
            SaveNotes();
            LoadNotes(data.SelectedDate); 

            name.Clear();
            opis.Clear();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (vse.SelectedItem == null) return;

            var selectedName = vse.SelectedItem.ToString();
            var note = _notes.FirstOrDefault(n => n.Name == selectedName);

            if (note != null)
            {
               
                note.Name = name.Text;
                note.Description = opis.Text;

                SaveNotes();
                LoadNotes(data.SelectedDate);
                
                vse.SelectedItem = null;
                name.Clear();
                opis.Clear();
            }
        }
        private void del_Click(object sender, RoutedEventArgs e)
        {
            if (vse.SelectedItem == null) return;

            var selectedName = vse.SelectedItem.ToString();
            var note = _notes.FirstOrDefault(n => n.Name == selectedName);

            if (note != null)
            {
                _notes.Remove(note); 

                SaveNotes();
                LoadNotes(data.SelectedDate);
                
                name.Clear();
                opis.Clear();
            }
        }

        private void vse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vse.SelectedItem != null)
            {
                var selectedName = vse.SelectedItem.ToString();
                var note = _notes.FirstOrDefault(n => n.Name == selectedName);
                if (note != null)
                {
                    name.Text = note.Name;
                    opis.Text = note.Description;
                }
            }
        }
        public class Note
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
        }

    }
}