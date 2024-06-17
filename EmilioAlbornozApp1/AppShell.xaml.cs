namespace EmilioAlbornozApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.EA_NotePage), typeof(Views.EA_NotePage));
        }
    }
}
