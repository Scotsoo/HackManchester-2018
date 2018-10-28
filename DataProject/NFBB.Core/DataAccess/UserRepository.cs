using Dapper;
using DapperExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NFBB.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFBB.Core.DataAccess
{
    public class UserRepository
    {
        IDbConnection connection;

        public UserRepository(IDbConnection cnn)
        {
            connection = cnn;
        }

        public User GetById(int userid)
        {
            connection.Open();
            var user = connection.Get<User>(userid);

            connection.Close();

            return user;
        }

        public void DeleteAll()
        {
            connection.Open();

            string sql = "delete from [User]";
            connection.Execute(sql);
            connection.Close();
        }


        public void Add(User u)
        {

            string sql = @"INSERT INTO[dbo].[User] ([UserId],[Name],[City],[State],[Image], Gender)
                                VALUES (@UserId,@Name,@City,@State,@Image, @gender)";
            connection.Execute(sql, new { u.UserId, u.Name, u.City, u.State, u.Image, u.Gender });
            connection.Close();
        }

        public List<int> GetMissingUsers()
        {
            connection.Open();

            string sql = "select distinct userid from Review where userid not in (select userid from [user])";
            var users = connection.Query<int>(sql).ToList<int>();
            connection.Close();

            return users;
        }

        public void SaveUsers(List<User> users)
        {
            foreach(var user in users)
            {
                Add(user);
            }
        }

        public List<User> CreateRandomUsers(List<int> missingUsers)
        {

            var users = new List<User>();
            int userstoFetch = missingUsers.Count;

            while (userstoFetch > 0)
            {
                var getUsers = userstoFetch;

                if (userstoFetch > 5000)
                {
                    getUsers = 5000;
                }

                

                RandomUser r = FetchRandomUsers(getUsers);

                if (r.results != null)
                {

                    foreach (var person in r.results)
                    {
                        var userid = missingUsers[0];

                        users.Add(
                            new User
                            {
                                UserId = userid,
                                Name = person.name.first + " " + person.name.last,
                                City = person.location.city,
                                State = person.location.state,
                                Gender = person.gender,
                                Image = person.picture.medium
                            });

                        missingUsers.Remove(userid);
                    }

                    userstoFetch -= getUsers;
                }

                Console.WriteLine(users.Count + " users created. " + userstoFetch + " left to create.");
                
            }
            Thread.Sleep(500);

            return users;
        }

        private static RandomUser FetchRandomUsers(int count)
        {
            string json;

            using (var client = new System.Net.Http.HttpClient())
            {
                var response = client.GetAsync("https://randomuser.me/api/?results=" + count).Result;
                using (HttpContent content = response.Content)
                {
                    Task<string> result = content.ReadAsStringAsync();
                    json = result.Result;
                }
            }

            var r = JsonConvert.DeserializeObject<RandomUser>(json);

            if (r.results == null)
            {
                Console.WriteLine(json);
            }
            return r;
        }
    }
}
