using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EmilioAlbornozApp1.ViewModels
{
    internal class EA_AllNotesPageViewModel : IQueryAttributable
    {
        public ObservableCollection<ViewModels.EA_NotePageViewModel> AllNotes { get; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }

        public EA_AllNotesPageViewModel()
        {
            AllNotes = new ObservableCollection<ViewModels.EA_NotePageViewModel>(Models.EA_Note.LoadAll().Select(n => new EA_NotePageViewModel(n)));
            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<ViewModels.EA_NotePageViewModel>(SelectNoteAsync);
        }

        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.EA_NotePage));
        }

        private async Task SelectNoteAsync(ViewModels.EA_NotePageViewModel note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(Views.EA_NotePage)}?load={note.Identifier}");
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                EA_NotePageViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note exists, delete it
                if (matchedNote != null)
                    AllNotes.Remove(matchedNote);
            }
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                EA_NotePageViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                // If note is found, update it
                if (matchedNote != null)
                    matchedNote.Reload();

                // If note isn't found, it's new; add it.
                else
                    AllNotes.Add(new EA_NotePageViewModel(Models.EA_Note.Load(noteId)));
            }
        }
    }
}
