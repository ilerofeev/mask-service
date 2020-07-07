using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MaskService
{
    // Реализация методов сервиса по его интерфейсу
    public class Service1 : IService1
    {
        // Создание подключения к локальной БД
        // ВНИМАНИЕ! Замените "C:\Users\Ilya\source\repos\MaskService\Client\App_Data\Database.mdf" на
        // свой путь до БД. (Можно посмотреть в Обозреватель сервисов, Database.mdf, ПКМ, свойства,
        // строка подключения. Для появления БД в обозревателе двойной клик по "Database.mdf") 
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Ilya\source\repos\MaskService\Client\App_Data\Database.mdf;Integrated Security=True");

        // Метод получения логов
        public List<UserDetails> GetLogs(string UserID)
        {
            // Создаем список типа объекта сервиса
            List<UserDetails> UserDetails = new List<UserDetails>();
            // Открываем подключение к БД
            con.Open();
            // Задаем команду хранимую процедуру, используемую в данном методе
            SqlCommand cmd = new SqlCommand("GetLogsByID", con);
            // Задаем тип комманды (предосторожность для избежания возможных ошибок)
            cmd.CommandType = CommandType.StoredProcedure;
            // Добавление необходимых для процедуры параметров
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            // Адаптер заполняет данными, полученными из БД, DataTable, объявленный ниже
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            // Заполнение dt
            da.Fill(dt);
            // Проход по всем строкам полученных данных
            if (dt.Rows.Count > 0)
            {
                // Для каждой строки
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // Создаем новый объект сервиса и заполняем его свойства полученными данными
                    UserDetails userInfo = new UserDetails();
                    userInfo.UserID = dt.Rows[i]["UserID"].ToString();
                    userInfo.MaskUserID = dt.Rows[i]["MaskUserID"].ToString();
                    UserDetails.Add(userInfo);
                }
            }
            // Закрываем подключение
            con.Close();
            // Возвращаем полученный в итоге объект
            return UserDetails;
        }

        // Метод обновления маски
        public string UpdateMask(string UserID, string MaskUserID, string newValue)
        {
            // Задаем возвращаемую строку
            string strMessage = string.Empty;
            // Далее по аналогии, что и в прошлом методе
            con.Open();
            SqlCommand cmd = new SqlCommand("UpdateMaskByID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            cmd.Parameters.Add("@MaskUserID", SqlDbType.Int).Value = MaskUserID;
            cmd.Parameters.Add("@newValue", SqlDbType.Int).Value = newValue;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // Выполняем команду, не возвращающую данные и обновляем строку ответа
            if (cmd.ExecuteNonQuery() == 1)
            {
                strMessage = "updated successfully";
            }
            else
            {
                strMessage = "not updated successfully";
            }
            con.Close();
            // Возвращаем строку ответа
            return strMessage;
        }

        // Метод удаления маски
        public string DeleteMask(string UserID)
        {
            // Далее по аналогии, что и в прошлом методе
            string strMessage = string.Empty;
            con.Open();
            SqlCommand cmd = new SqlCommand("DeleteMaskByID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            if (cmd.ExecuteNonQuery() == 1)
            {
                strMessage = "deleted successfully";
            }
            else
            {
                strMessage = "not deleted successfully";
            }
            con.Close();
            return strMessage;
        }

        // Метод получения всех масок
        public List<UserDetails> GetUserMasks()
        {
            // По аналогии, что и в первом методе, только данные берутся из другой таблицы
            // т.е выполняется др. процедура()
            List<UserDetails> UserDetails = new List<UserDetails>();
            con.Open();
            SqlCommand cmd = new SqlCommand("GetAllMasks", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserDetails userInfo = new UserDetails();
                    userInfo.UserID = dt.Rows[i]["UserID"].ToString();
                    userInfo.UserName = dt.Rows[i]["UserName"].ToString();
                    userInfo.MaskUserID = dt.Rows[i]["MaskUserID"].ToString();
                    UserDetails.Add(userInfo);
                }
            }
            con.Close();
            return UserDetails;
        }
    }
}