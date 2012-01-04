using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web;
using System.Diagnostics;
using System.IO;

using Nohros;
using Nohros.Data;
using Nohros.Configuration;
//using Nohros.Desktop.Security;
using Nohros.Security.Auth;
using Nohros.Toolkit.Messaging;

namespace desktdd
{
    class Program
    {
        static void DictTest()
        {
            /*#region init

            int num = 100;
            Node[] guids = new Node[num];
            StreamWriter w = new StreamWriter("c:\\dict.bck", false, Encoding.Default);

            Stopwatch s = new Stopwatch();

            s.Start();

            for (int i = 0; i < num; i++)
            {
                guids[i] = new Node(i.ToString(), i);
            }

            s.Stop();

            Console.WriteLine("Data Creation = " + s.Elapsed);

            s.Reset();

            #endregion

            int k = 0;
            while (k < num)
            {
                #region hash

                Hashtable hash = new Hashtable();

                s.Start();

                for (int i = 0; i < k; i++)
                {
                    Node node = guids[i];
                    hash.Add(node.Name, node);
                }

                s.Stop();

                w.WriteLine(k.ToString() + ":inserthash:" + s.ElapsedTicks.ToString());

                s.Reset();
                s.Start();

                for (int i = 0; i < k; i++)
                {
                    Node node = hash[guids[i].Name] as Node;
                }

                w.WriteLine(k.ToString() + ":extracthash:" + s.ElapsedTicks.ToString());

                s.Reset();

                s.Start();

                for (int i = 0; i < k; i++)
                {
                    hash.Remove(guids[i]);
                }

                w.WriteLine(k.ToString() + ":removehash:" + s.ElapsedTicks.ToString());

                s.Reset();

                #endregion

                #region dict

                Dictionary<string, Node> dict = new Dictionary<string, Node>();

                s.Start();

                for (int i = 0; i < k; i++)
                {
                    Node node = guids[i];
                    dict.Add(node.Name, node);
                }

                s.Stop();

                w.WriteLine(k.ToString() + ":insertdict:" + s.ElapsedTicks.ToString());

                s.Reset();

                s.Start();

                for (int i = 0; i < k; i++)
                {
                    Node node;
                     dict.TryGetValue(guids[i].Name, out node);
                }

                w.WriteLine(k.ToString() + ":extractsdict:" + s.ElapsedTicks.ToString());

                s.Reset();

                s.Start();

                for (int i = 0; i < k; i++)
                {
                    dict.Remove(guids[i].Name);
                }

                w.WriteLine(k.ToString() + ":removesdict:" + s.ElapsedTicks.ToString());

                s.Reset();

                #endregion

                k += 1;
            }

            w.Flush();
            w.Close();

            Console.WriteLine("Finished");
            Console.ReadLine();*/
        }

        static void SendHtml() {
            EmailMessage message = new EmailMessage("somemessage");
            message.Sender = new EmailAgent("comunicacao@somedomain.com", "Comunicacao Acao");
            message.Subject = "Pesquisa Perfil";
            message.IsBodyHtml = true;
            message.Message =
@"<html>
	<head>
		<title>Pesquisa Perfil Acao</title>
	</head>
	<body>
		<a href=""$link$"">
			<img alt=""Pesquisa Perfil Acao"" src=""http://192.168.203.8:8080/images/pesquisa_adm.jpg""/>
		</a>
	</body>
</html>";
            message.AddRecipient(new EmailAgent("comunicacao@acaoassessoria.com.br", "Comunicacao"));

            Dictionary<string, string> options = new Dictionary<string, string>();
            options["smtp-host"] = "smtp.acao.net.br";
            options["smtp-port"] = "25";
            SmtpMessenger messenger = new SmtpMessenger("somename", options);
            ResponseMessage response = messenger.Send(message);
        }

        static void Main(string[] args) {
            SendHtml();
        }
    }
}