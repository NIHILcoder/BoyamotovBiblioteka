using System;
using System.Windows.Forms;

namespace boyBibl
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true) // Запускаем цикл для управления входом и выходом
            {
                // Показываем LoginForm
                LoginForm loginForm = new LoginForm();
                Application.Run(loginForm);

                // Проверяем, установлена ли роль
                if (!string.IsNullOrEmpty(loginForm.UserRole))
                {
                    // Показываем MainForm
                    MainForm mainForm = new MainForm(loginForm.UserRole);
                    Application.Run(mainForm);

                    // Если MainForm закрыта через выход из системы, повторяем вход
                    if (mainForm.LogoutRequested)
                    {
                        continue;
                    }
                }

                // Если нет роли или пользователь полностью вышел, выходим из приложения
                break;
            }
        }
    }
}
