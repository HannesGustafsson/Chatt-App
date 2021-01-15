using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient; //added
using System.IO;
using Newtonsoft.Json;

namespace backend
{
    class Program
    {
        static GetMessagesResponse responseList;
        static NameList list;

        static void Main()
        {
            using (StreamReader r = new StreamReader("name_list.json"))
            {
                string json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<NameList>(json);
            }
            AsynchronousSocketListener.StartListening();
        }

        /// <summary>
        /// Insert a new message to the DB
        /// after checking user status in DB
        /// </summary>
        static public void AddMessage(BackendRequest message)
        {
            Console.WriteLine(message.Input.IpAddress);
            Console.WriteLine("Adding message to DB");
            responseList.MessageList.Add(message.Input.MessageToInput);
            string connString = "server=127.0.0.1;uid=root;" + "pwd=P@55w.rd;database=chattapp";
            MySqlConnection conn = new MySqlConnection(connString);
            try
            {
                //Retrives the users from the database
                List<DbUser> userList = new List<DbUser>();
                string sql = "SELECT * FROM users WHERE ip='" + message.Input.IpAddress + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();                
                while (reader.Read())
                {
                    DbUser user = new DbUser(reader.GetString("alias"), reader.GetInt32("iduser"), reader.GetString("ip"));
                    userList.Add(user);
                }
                reader.Close();

                // If multiple users on same id were found
                if (userList.Count() > 1)
                {
                    Console.WriteLine("More than one user was found");
                    foreach (var item in userList)
                    {
                        Console.WriteLine("id user: " + item.ToString());
                    }
                }
                // If user was found on id
                else if (userList.Count() > 0)
                {
                    int a = userList[0].Iduser;
                    Console.WriteLine("id user: " + a.ToString());

                    // Overlap the users and messages tables and 
                    // retrieve data linked to User ID, sorted by timestamp
                    sql = "INSERT INTO messages(message, timestamp, iduser) VALUES('" + message.Input.MessageToInput.MessageText + "','" + message.Input.MessageToInput.Timestamp + "','" + a + "')";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { }
                    reader.Close();
                }
                // If no users were found
                // Create new User
                else
                {
                    Console.WriteLine("No user was found");

                    // Randomize new Alias for user
                    DbUser user = new DbUser(RandomName(), message.Input.IpAddress);

                    // Save User to DB
                    sql = "INSERT INTO users(alias, ip) VALUES('" + user.Alias + "','" + user.Ip + "')";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { }
                    reader.Close();

                    // Get new User ID from DB
                    sql = "SELECT * FROM users WHERE ip='" + message.Input.IpAddress + "'";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new DbUser(reader.GetString("alias"), reader.GetInt32("iduser"), reader.GetString("ip"));
                        userList.Add(user);
                    }
                    reader.Close();

                    // Save message to DB with User ID
                    sql = "INSERT INTO messages(message, timestamp, iduser) VALUES('" + message.Input.MessageToInput.MessageText + "','" + message.Input.MessageToInput.Timestamp + "','" + userList[0].Iduser + "')";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { }
                    reader.Close();
                    responseList.MessageList.Add(new MessageObject { Alias = user.Alias, MessageText = message.Input.MessageToInput.MessageText, Timestamp = message.Input.MessageToInput.Timestamp });
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Retrives messages from DB
        /// </summary>
        static public GetMessagesResponse GetMessages(Int64 timestamp)
        {
            Console.WriteLine("Getting message from DB with timestamp: " + timestamp.ToString());
            string sql;
            responseList = new GetMessagesResponse();
            string connString = "server=127.0.0.1;uid=root;" + "pwd=P@55w.rd;database=chattapp";
            MySqlConnection conn = new MySqlConnection(connString);
            if (timestamp == 0)
            {
                sql = "SELECT users.alias, messages.message, messages.timestamp FROM users INNER JOIN messages ON users.iduser = messages.iduser ORDER BY messages.timestamp ASC";
            }
            else
            {
                sql = "SELECT users.alias, messages.message, messages.timestamp FROM users INNER JOIN messages ON users.iduser = messages.iduser WHERE messages.timestamp > " + timestamp + " ORDER BY messages.timestamp DESC";
            }


            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString("message") + reader.GetInt64("timestamp").ToString() + reader.GetString("alias"));
                responseList.MessageList.Add(new MessageObject { Alias = reader.GetString("alias"), MessageText = reader.GetString("message"), Timestamp = reader.GetInt64("timestamp") });
            }
            reader.Close();


            return responseList;
        }

        /// <summary>
        /// Returns randomized user alias after checking if it is available in database
        /// </summary>
        static public string RandomName()
        {
            bool unique = false;
            string name = "";
            while (!unique)
            {
                Random random = new Random();
                name = list.Names[random.Next(0, list.Names.Count - 1)] + " " + list.Names[random.Next(0, list.Names.Count - 1)];
                string user = "";

                string connString = "server=127.0.0.1;uid=root;" + "pwd=P@55w.rd;database=chattapp";
                MySqlConnection conn = new MySqlConnection(connString);

                try
                {
                    string sql = "SELECT * FROM users WHERE alias='" + name + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user += reader.GetString("alias");
                    }
                    reader.Close();
                    if (user.Length == 0)
                    {
                        unique = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return name;

        }
    }

    /// <summary>
    /// Class object to hold names from JSON file.
    /// </summary>
    public class NameList
    {
        public IList<string> Names { get; set; }
    }

    public class DbUser
    {
        private string alias;
        private readonly int iduser;
        private string ip;

        public DbUser(string alias, string ip)
        {
            this.alias = alias;
            this.ip = ip;
        }
        public DbUser(string alias, int iduser, string ip)
        {
            this.alias = alias;
            this.iduser = iduser;
            this.ip = ip;
        }
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        public int Iduser
        {
            get { return iduser; }
        }
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
    }
}
