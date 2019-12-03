﻿using System;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Linq;

namespace CareerCloud.ADODataAccessLayer
{
    public class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        private string _connStr;
        public SecurityLoginsLogRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                foreach (SecurityLoginsLogPoco poco in items)
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = connection;
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Logins_Log]
                                       ([Id]
                                       ,[Login]
                                       ,[Source_IP]
                                       ,[Logon_Date]
                                       ,[Is_Succesful])
                                 VALUES
                                       (@Id, 
                                        @Login, 
                                        @Source_IP, 
                                        @Logon_Date, 
                                        @Is_Succesful)";
                    comm.Parameters.AddWithValue("@Id", poco.Id);
                    comm.Parameters.AddWithValue("@Login", poco.Login);
                    comm.Parameters.AddWithValue("@Source_IP", poco.SourceIP);
                    comm.Parameters.AddWithValue("@Logon_Date", poco.LogonDate);
                    comm.Parameters.AddWithValue("@Is_Succesful", poco.IsSuccesful);
       
                 



                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT [Id]
                                  ,[Login]
                                  ,[Source_IP]
                                  ,[Logon_Date]
                                  ,[Is_Succesful]
                              FROM [dbo].[Security_Logins_Log]";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SecurityLoginsLogPoco[] pocos = new SecurityLoginsLogPoco[5000];
                int index = 0;
                while (reader.Read())
                {
                    SecurityLoginsLogPoco poco = new SecurityLoginsLogPoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Login = reader.GetGuid(1);
                    poco.SourceIP = (string)reader["Source_IP"];          
                    poco.LogonDate = reader.GetDateTime(3);
                    poco.IsSuccesful = reader.GetBoolean(4);
      
                   
                    pocos[index] = poco;
                    index++;
                }
                conn.Close();
                return pocos.Where(a => a != null).ToList();
            }
        }

        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsLogPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                foreach (SecurityLoginsLogPoco poco in items)
                {
                    cmd.CommandText = @"DELETE Security_Logins_Log   
                                        where ID = @id";
                    cmd.Parameters.AddWithValue("@Id", poco.Id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                foreach (var poco in items)
                {
                    cmd.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                           SET [Id] = @Id
                                              ,[Login] = @Login
                                              ,[Source_IP] = @Source_IP
                                              ,[Logon_Date] = @Logon_Date
                                              ,[Is_Succesful] = @Is_Succesful
                                         WHERE [Id] = @Id";

                    cmd.Parameters.AddWithValue("@Id", poco.Id);
                    cmd.Parameters.AddWithValue("@Login", poco.Login);
                    cmd.Parameters.AddWithValue("@Source_IP", poco.SourceIP);
                    cmd.Parameters.AddWithValue("@Logon_Date", poco.LogonDate);
                    cmd.Parameters.AddWithValue("@Is_Succesful", poco.IsSuccesful);

                    connection.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count <= 0)
                    {
                        throw new Exception();
                    }
                    connection.Close();
                }
            }
        }
    }
}
