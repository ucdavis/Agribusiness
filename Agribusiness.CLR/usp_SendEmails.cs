using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Mail;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void usp_SendEmails()
    {
        var client = new SmtpClient("smtp.ucdavis.edu");
        
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            var cmd = new SqlCommand("select id, subject, body, fromaddress, email from vEmailQueue", connection);
            var reader = cmd.ExecuteReader();

            var updateList = new List<int>();

            using (reader)
            {
                while (reader.Read())
                {
                    // deal with each individual object to send an email
                    var id = reader.GetInt32(0);
                    var subject = reader.GetString(1);
                    var body = reader.GetString(2);
                    var from = reader.GetString(3);
                    var email = reader.GetString(4);

                    var msg = new MailMessage();
                    msg.From = new MailAddress(from);
                    //var to = email.Split(';');
                    //foreach(var t in to) msg.To.Add(t);
                    msg.To.Add("anlai@ucdavis.edu");
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;

                    // send the message
                    client.Send(msg);

                    // execute the update
                    updateList.Add(id);
                }
            }

            foreach (var id in updateList)
            {
                var update = new SqlCommand(string.Format("update emailqueue set pending = 0, sentdatetime = getdate() where id = {0}", id), connection);
                update.ExecuteNonQuery();
            }

        }
    }
};
