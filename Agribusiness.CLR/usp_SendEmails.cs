using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net.Mail;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    // server
    protected const string SmtpServer = "smtp.ucdavis.edu";

    // queries
    protected const string EmailQueueQuery = "select id, subject, body, fromaddress, email from vEmailQueue";
    protected const string AttachmentQuery = "select contents, filename, contenttype from attachments where id in (select attachmentid from emailqueuexattachments where emailqueueid = {0})";
    protected const string UpdateQueueQuery = "update emailqueue set pending = 0, sentdatetime = getdate() where id = {0}";

    [SqlProcedure]
    public static void usp_SendEmails()
    {
        var client = new SmtpClient("smtp.ucdavis.edu");
        
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();

            var cmd = new SqlCommand(EmailQueueQuery, connection);
            var reader = cmd.ExecuteReader();

            var updateList = new List<int>();

            var emails = new List<Email>();

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

                    emails.Add(new Email(id, from, email, subject, body));
                }
            }

            byte[] buffer;

            // read out attachments
            foreach (var email in emails)
            {
                var cmd2 = new SqlCommand(string.Format(AttachmentQuery, email.Id), connection);
                var reader2 = cmd2.ExecuteReader();

                using (reader2)
                {
                    while (reader2.Read())
                    {
                        var length = (int)reader2.GetBytes(0, 0L, null, 0, 0);
                        buffer = new byte[length];
                        reader2.GetBytes(0, 0L, buffer, 0, length);

                        var fileName = reader2.GetString(1);
                        var contentType = reader2.GetString(2);

                        email.AddAttachment(buffer, fileName, contentType);
                    }
                }
            }

            foreach(var email in emails)
            {
                var msg = new MailMessage();
                msg.From = new MailAddress(email.From);
                //foreach(var to in email.To) msg.To.Add(to);
                msg.To.Add("anlai@ucdavis.edu");
                msg.Subject = email.Subject;
                msg.Body = email.Body;
                msg.IsBodyHtml = true;

                foreach (var a in email.Attachments)
                {
                    msg.Attachments.Add(new System.Net.Mail.Attachment(a.Data, a.FileName, a.ContentType));
                }

                client.Send(msg);

                var update = new SqlCommand(string.Format(UpdateQueueQuery, email.Id), connection);
                update.ExecuteNonQuery();
            }

        }
    }
};

public class Email
{
    public int Id { get; set; }
    public string From { get; set; }
    public IList<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public IList<Attachment> Attachments { get; set; }

    public Email(int id, string from, string to, string subject, string body)
    {
        Id = id;
        From = from;
        To = to.Split(';');
        Subject = subject;
        Body = body;

        Attachments = new List<Attachment>();
    }

    public void AddAttachment(byte[] contents, string fileName, string contentType)
    {
        Attachments.Add(new Attachment(fileName, contentType, contents));
    }
}

public class Attachment
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] Contents { get; set; }

    public Attachment(string fileName, string contentType, byte[] contents)
    {
        FileName = fileName;
        ContentType = contentType;
        Contents = contents;
    }

    public Stream Data
    {
        get { 
            var stream = new MemoryStream(Contents);
            return stream;
        }
    }
}