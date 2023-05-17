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

namespace HCI_Projekat2_PacManGame
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        String level;
        int score;
        public CustomMessageBox(String message,int score2)
        {

            InitializeComponent();
            this.txtMessage.Text = message;
            this.level = "";
            this.score = score2;
          
        }

        public CustomMessageBox(String message,String level,int score2)
        {

            InitializeComponent();
            this.txtMessage.Text = message;
            this.level = level;
            this.score = score2;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            
            if(level.Equals("level2"))
            {
                Level2 level2 = new Level2(score);
                level2.Show();
                this.Close();
                
            }
            else
            if (level.Equals("level3"))
            {
                Level3 level3 = new Level3(score);
                level3.Show();
                this.Close();
            }else
                  if (level.Equals(""))
            {
                SaveScoreWindow saveScoreWindow = new SaveScoreWindow(score);
                saveScoreWindow.Show();
                this.Close();
            }
        }
    }
}
