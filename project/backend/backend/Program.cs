using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using MySql.Data.MySqlClient; //added
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;
using Newtonsoft;
using Newtonsoft.Json;

namespace backend
{
    class Program
    {
        static GetMessagesResponse responseList;
        static NameList list;

        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("name_list.json"))
            {
                string json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<NameList>(json);
            }

            //string tempName = RandomName();

            //ListenTCP();
            //DummyThiccArray();
            //AsynchronousSocketListener.StartListening();

            //GetMessages(0);
            //GetMessages(1);
            //Console.WriteLine(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            //Console.WriteLine(DateTimeOffset.Now.ToUnixTimeSeconds());

            //AddMessage(new BackendRequest { Input = new InputMessage { MessageToInput = new MessageObject { MessageText = "Hellow db!", Timestamp = DateTime.Now.Ticks }, IpAddress = "192.168.1.69" } });

        }



        static void DummyThiccArray()
        {
            responseList = new GetMessagesResponse();

            for (int i = 0; i < 5; i++)
            {
                responseList.MessageList.Add(new MessageObject
                {
                    MessageText = i + " message YOOO!!",
                    Alias = "James den store",
                    Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()

                });
            }

        }

        static public void AddMessage(BackendRequest message)
        {
            Console.WriteLine(message.Input.IpAddress);
            Console.WriteLine("Adding message to DB");
            responseList.MessageList.Add(message.Input.MessageToInput);
            string connString = "server=127.0.0.1;uid=root;" + "pwd=P@55w.rd;database=chattapp";
            MySqlConnection conn = new MySqlConnection(connString);
            try
            {
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


                if (userList.Count() > 1)
                {
                    Console.WriteLine("more than one user was found");
                    foreach (var item in userList)
                    {
                        Console.WriteLine("id user: " + item.ToString());
                    }
                }
                else if (userList.Count() > 0)
                {
                    int a = userList[0].Iduser;
                    Console.WriteLine("id user: " + a.ToString());

                    sql = "INSERT INTO messages(message, timestamp, iduser) VALUES('" + message.Input.MessageToInput.MessageText + "','" + message.Timestamp + "','" + a + "')";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { }
                    reader.Close();

                }
                else
                {
                    Console.WriteLine("No user was found");

                    DbUser user = new DbUser(RandomName(), message.Input.IpAddress);

                    sql = "INSERT INTO users(alias, ip) VALUES('" + user.Alias + "','" + user.Ip + "')";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { }
                    reader.Close();

                    sql = "SELECT * FROM users WHERE ip='" + message.Input.IpAddress + "'";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new DbUser(reader.GetString("alias"), reader.GetInt32("iduser"), reader.GetString("ip"));
                        userList.Add(user);
                    }
                    reader.Close();

                    sql = "INSERT INTO messages(message, timestamp, iduser) VALUES('" + message.Input.MessageToInput.MessageText + "','" + message.Timestamp + "','" + userList[0].Iduser + "')";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) { }
                    reader.Close();
                    responseList.MessageList.Add(new MessageObject { Alias = user.Alias, MessageText = message.Input.MessageToInput.MessageText, Timestamp = message.Timestamp });


                }


                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //Random random = new Random(Encoding.IPAddress.Parse(message.Input.IpAddress).Address);
            // adds message to DB
            //if (true)
            //{

            //}
            //else
            //{
            //    string connString = "server=127.0.0.1;uid=root;" + "pwd=P@55w.rd;database=messages";
            //    MySqlConnection conn = new MySqlConnection(connString);
            //    try
            //    {
            //        //string sql = "INSERT INTO `iotunits`.`iotunit` (`DeviceID`, `IPAdress`, `Timestamp`, `DeviceType`, `DeviceCategory`, `Value`, `Unit`) VALUES ('" + unit.DeviceID + "', '" + unit.IPadress + "', '" + DateTime.Now + "', '" + unit.DeviceType + "', '" + unit.DeviceCategory + "', '" + unit.Value + "', '" + unit.Unit + "')";
            //        string sql = "";
            //        MySqlCommand cmd = new MySqlCommand(sql, conn);
            //        conn.Open();
            //        DataTable table = conn.GetSchema("Databases");
            //        string tName = table.TableName;
            //        MySqlDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read()) { }
            //        conn.Close();
            //    }
            //    catch (MySqlException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
        }

        static public GetMessagesResponse GetMessages(Int64 timestamp)
        {
            Console.WriteLine("Getting message from DB");
            string sql = "";
            responseList = new GetMessagesResponse();
            // Retrives messages from DB
            string connString = "server=127.0.0.1;uid=root;" + "pwd=P@55w.rd;database=chattapp";
            MySqlConnection conn = new MySqlConnection(connString);
            if (timestamp == 0)
            {
                sql = "SELECT users.alias, messages.message, messages.timestamp FROM users INNER JOIN messages ON users.iduser = messages.iduser ORDER BY messages.timestamp";
            }
            else
            {
                sql = "SELECT users.alias, messages.message, messages.timestamp FROM users INNER JOIN messages ON users.iduser = messages.iduser WHERE messages.timestamp > " + timestamp + " ORDER BY messages.timestamp";
            }


            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString("message") + reader.GetInt32("timestamp").ToString() + reader.GetString("alias"));
                responseList.MessageList.Add(new MessageObject { Alias = reader.GetString("alias"), MessageText = reader.GetString("message"), Timestamp = reader.GetInt32("timestamp") });
            }
            reader.Close();


            return responseList;
        }

        // Returns randomized user alias after checking if it is available in database
        static public string RandomName()
        {
            bool unique = false;
            string name = "";
            while (!unique)
            {
                Random random = new Random();
                name = list.names[random.Next(0, list.names.Count - 1)] + " " + list.names[random.Next(0, list.names.Count - 1)];
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
                        user = user + reader.GetString("alias");
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
    public class NameList
    {
        public IList<string> names { get; set; }
    }

    public class DbUser
    {
        private string alias;
        private int iduser;
        private string ip;

        public DbUser(string alias, string ip)
        {
            this.alias = alias;
            this.ip = ip;
        }
        public DbUser(string alias, int idUser, string ip)
        {
            this.alias = alias;
            this.iduser = idUser;
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
