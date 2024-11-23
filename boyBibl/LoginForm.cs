using System;
using System.Windows.Forms;

namespace boyBibl
{
    public partial class LoginForm : Form
    {
        public string UserRole { get; private set; } // Свойство для хранения роли пользователя

        public LoginForm()
        {
            InitializeComponent();
        }

        [Obsolete]
        private void bunifuButton21_Click(object sender, EventArgs e) // Кнопка "Войти"
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            // Проверка роли на основе введённых данных
            if (username == "admin" && password == "admin123")
            {
                UserRole = "Admin"; // Устанавливаем роль
                MainForm mainForm = new MainForm(UserRole); // Передаём роль в MainForm
                mainForm.Show();
                this.Hide();
            }
            else if (username == "user" && password == "user123")
            {
                UserRole = "User"; // Устанавливаем роль
                MainForm mainForm = new MainForm(UserRole); // Передаём роль в MainForm
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
