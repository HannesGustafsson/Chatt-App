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

        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("name_list.json"))
            {
                string json = r.ReadToEnd();
                NameList list = JsonConvert.DeserializeObject<NameList>(json);
                Console.WriteLine(json);
                
            }
            //ListenTCP();
            DummyThiccArray();
            //AsynchronousSocketListener.StartListening();

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
                    Timestamp = DateTime.Now.Ticks

                });
            }

        }

        static void ListenTCP()
        {
            try
            {
                TcpListener tcpListener = new TcpListener(GetLocalIP(), 11000);
                tcpListener.Start();

                while (true)
                {
                    BackendRequest request = new BackendRequest();
                    Console.Write("Waiting for a connection... ");
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    NetworkStream stream = tcpClient.GetStream();



                    try
                    {
                        request = BackendRequest.Parser.ParseFrom(stream);
                        if (request.IsInput)
                        {
                            Console.WriteLine("Message recived adding to DB");
                            Console.WriteLine(request.Input);
                            AddMessage(request);

                            tcpClient.Close();
                        }
                        else
                        {
                            GetMessagesResponse response = GetMessages(request.Timestamp);
                            Console.WriteLine("Waiting for a connection to transfer... ");
                            tcpClient = tcpListener.AcceptTcpClient();

                            response.WriteTo(tcpClient.GetStream());
                            Console.WriteLine("Transfer Complete closing stream");
                            tcpClient.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static private IPAddress GetLocalIP()
        {
            //Retrives the local computers IP from the DNS
            IPAddress[] iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            return iPAddresses[3];
        }
        static public void AddMessage(BackendRequest message)
        {
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
                }
                else
                {
                    Console.WriteLine("No user was found");

                    Random random = new Random();
                    random.Next(0, 4945);

                    

                    DbUser user = new DbUser("Saaaaanic", message.Input.IpAddress);

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
            Console.WriteLine("Writing message to DB");
            // Retrives messages from DB
            return responseList;
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
