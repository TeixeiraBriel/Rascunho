using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rascunho
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient();

            logarSSW(client);
        }

        static void logarSSW(RestClient client)
        {
            var request = new RestRequest("https://sistema.ssw.inf.br/bin/ssw0422", Method.Post);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Referer", "https://sistema.ssw.inf.br/bin/ssw0422");
            
            request.AddParameter("act", "L");
            request.AddParameter("f1", "X04");
            request.AddParameter("f2", "1046422300");
            request.AddParameter("f3", "backpri");
            request.AddParameter("f4", "123456");
            request.AddParameter("f6", "TRUE");
            request.AddParameter("backimg", "ssw07.jpg?dummy=230802");
            request.AddParameter("dummy", "1691010257660");

            RestResponse response = client.Execute(request);

            //request = new RestRequest("https://safebrowsing.google.com/safebrowsing/clientreport/realtime", Method.Post);
            //request.AddHeader("Content-Type", "application/octet-stream");
            //response = client.Execute(request);

            request = new RestRequest("https://sistema.ssw.inf.br/bin/menu01", Method.Post);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("Referer", "https://sistema.ssw.inf.br/bin/ssw0422");
            request.AddHeader("Origin","https://sistema.ssw.inf.br");
            request.AddHeader("sec-ch-ua", "Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"115\", \"Chromium\";v=\"115\"");
            request.AddHeader("Content-Type" ,"application/x-www-form-urlencoded");

            request.AddParameter("act", "");
            response = client.Execute(request);

            request = new RestRequest("https://sistema.ssw.inf.br/bin/menu01", Method.Post);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Referer", "https://sistema.ssw.inf.br/bin/menu01");

            request.AddParameter("act", "TRO");
            request.AddParameter("f2", "");
            request.AddParameter("f3", "101");
            request.AddParameter("dummy", "1691010996643");

            response = client.Execute(request);

            //request = new RestRequest("https://sistema.ssw.inf.br/bin/ssw0053", Method.Post);
            //request.AddHeader("Accept", "*/*");
            //request.AddHeader("Referer", "https://sistema.ssw.inf.br/bin/menu01");

            //request.AddParameter("sequencia", "101");
            //request.AddParameter("dummy", "1691010996769");

            //response = client.Execute(request);

        }


        static void abrirSistemaTsv(RestClient client)
        {
            var request = new RestRequest("https://portal.tsvtransportes.com.br/login.aspx/", Method.Get);
            RestResponse response = client.Execute(request);
        }
        static void LogarTsv(RestClient client)
        {
            var request = new RestRequest("https://portal.tsvtransportes.com.br/crypto.aspx/enviar", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody("{\"usuario\":\"PRIVALIA\"}");
            RestResponse response = client.Execute(request);

            //var chave = "UFJJVkExMzI1MVNPTElEMg==";
            var chave = response.Content.Split('"')[3];
            var senha = obtemSenhaCrypt(chave);


            request = new RestRequest("https://portal.tsvtransportes.com.br/login.aspx", Method.Post);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");

            request.AddParameter("txtLogin", "PRIVALIA");
            request.AddParameter("txtSenha", senha);
            request.AddParameter("__VIEWSTATE", "/wEPDwUKMTY4MDcyMjQwMWQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFCGJ0bkxvZ2FyyP8GohChkmlLXEgIX4afOGsjZWC3dcKdWmlCIAnd4RY=");
            request.AddParameter("__VIEWSTATEGENERATOR", "C2EE9ABB");
            request.AddParameter("__EVENTVALIDATION", "/wEdAAQMmTSiXTkGJ/t5PHLbknvPxcn6oIDdbNQI5AQUIIyv4iLn1AyeUZQZF14sXrB8lOFq/A0Pbonn73bBXDMYTbIwxnBHigR9+TcWgueqQOOT52lUlUX54aimQleMzshrPFo=");

            response = client.Execute(request);
        }

        static string obtemSenhaCrypt(string chave)
        {
            using (var aeslg = Aes.Create())
            {
                aeslg.Key = Convert.FromBase64String(chave);
                aeslg.IV = Convert.FromBase64String(chave);

                ICryptoTransform cryptor = aeslg.CreateEncryptor(aeslg.Key, aeslg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncryp = new CryptoStream(msEncrypt, cryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncryp))
                        {
                            swEncrypt.Write("tsv2022");
                        }

                        var encripted = msEncrypt.ToArray();
                        return Convert.ToBase64String(encripted);
                    }
                }
            }
        }

        static void SegundoMenuTSV(RestClient client)
        {
            var request = new RestRequest("https://portal.tsvtransportes.com.br/home.aspx?id_menu=2", Method.Get);
            RestResponse response = client.Execute(request);
        }

        static void abrirSistema(RestClient client)
        {
            var request = new RestRequest("https://edi.voegol.com.br/", Method.Get);
            RestResponse response = client.Execute(request);
        }

        static void logar(RestClient client)
        {
            try
            {
                var cook = client.CookieContainer;
                var request = new RestRequest("https://edi.voegol.com.br/default.aspx", Method.Post);
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");

                request.AddParameter("txtUsuario", "backoffice.cex@privalia.com");
                request.AddParameter("txtSenha", "privalia321@");
                request.AddParameter("__VIEWSTATE", "/wEPDwULLTE4NTQzNjQ5NjZkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQxJbWFnZUJ1dHRvbjFCDGXOk85Of+yuglcVq1sjMHZL5Q==");
                request.AddParameter("__VIEWSTATEGENERATOR", "CA0B0334");
                request.AddParameter("__EVENTVALIDATION", "/wEWBAKp1YnFDgKpwKPFCALxiYiEAQLSwpnTCGBciQbQyIGjqkffHzF9zWC+5UvT");
                request.AddParameter("ImageButton1.x", "71");
                request.AddParameter("ImageButton1.y", "10");

                RestResponse response = client.Execute(request);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        static void abrirMenu(RestClient client)
        {
            var request = new RestRequest("https://edi.voegol.com.br/menu.aspx", Method.Get);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");

            RestResponse response = client.Execute(request);
        }

        static void FazerPesquisa(RestClient client)
        {
            var request = new RestRequest("https://edi.voegol.com.br/int_rastreamento_list.aspx", Method.Post);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");

            request.AddParameter("txtUsuario", "backoffice.cex@privalia.com");
            request.AddParameter("txtSenha", "privalia321@");
            request.AddParameter("__VIEWSTATE", "kyZ21HMIVDQJsHQMXH5jQz0iBOhu/Vt0ajnGWWaf/6Bs4vlO6T2Ahk8t/CQ+JEcRcBnRWnnW8YnboYP6gdjQpyaIoiz3fcbjUs1KEeQb9lEwIJI+Ks9kMi8QVlddppP0TvW5X0bxIjTA102J4695t/JINq59H7RWfcQjA9u55zXUtyEOAMNqY3PrGoN30qAcZc3XjOW5inrZBtOTYWIRTjDTpsHG89qaM15GbiaZYJHjFA/o57l5TRWXVpJsEUvE2Ho4bxXhgL999g9dqGBx0kFag4jbNoKmK3sKCjKFfUKsmxN12xo7BVPZfZ6KNcX0LZ4ZfxjMiZLKCgX9cb7hU4Xpm+SOoG18Vx3kjLGak62eDfZxE7viOPjLLn4u569DrTWCv21N9ptHpXueDB1+NhsGPUXPppKr0NNmYjT9GLQfxRhUC6J69Rw/uuHjk+9WLfeUfgS6pgBloUDNF8DMJSBcVH9DENvhQLVymaek03YqvFkScODYhAXJXwtRdbIEedCNMA+3ad2uvify54g6mmNN7PgFvfs+Ou8gf1ctjHPAEENK4YQ/EcphAyVyZ/C6VNjZTQyEccVEMal4i7JJTw9ftNAHKyiC+/UHGYDDkRhq1InTM1R6Hxt0vyzJn33dwdCtArEv6625XTvZoZ9OtDXjNqhpYx4mqYwbP/0er1GfufkBOQSyivVLrFiX1dsFN3zgNnjtvHx6Tlp3gDxVr0EctXiI3Hhy6eZy910i0orQnQ7qrlr56T9dLSViPVRrGfZMynkL9y9/QgvSYIy1SZZ4mCNr7WIi7ItL78vJwC5z9LlWEwTpDKBU5TnO/zFDD4tQeg==");
            request.AddParameter("__VIEWSTATEGENERATOR", "93540689");
            request.AddParameter("__EVENTVALIDATION", "7lcniSAsLY9mTV9Znum2z5tI2nTk64dtEPvzgmyYgZkkUzqSav5zsIJAeCGvoIh5psZUPk3EU0so7x8FbrKbWaVa7mCkjXPOZC/yme/ud1b47Pf8g2b3anZmtD0LS0Zuxpc5rV/t1ksZ5sumOqQkNjaw2y30vNrloPePbXMz875SLvcH4QIT8TylxtI487SsvKq3AwcqGjj9bw07+bYNM3/+5mJiKikgb/v4mKnkDqqBdzhRLTCc4RkF6fMtJkeF0govBbyadokAvFbOIfFhcRfQEJqlUcA6mpKKILgVzOKCgoOJ3XlulJvM4Hx910myDIKFfa4U76h4/kUHcRrQIOj2r1g1k7YcuyiPltjgwfJ3+/CUFpurT4tiI4NODK/toN/sPZkfmpDNGAd2vecQR4jQCTX7eHyxQ4FbGNxIxEKQxTFaUrEDiCugaPKDMpH+KZKhmA==");
            request.AddParameter("ctl00$ContentPlaceHolder1$txtSearchValue", "128243408");
            request.AddParameter("ImageButton1.y", "10");

            request.AddParameter("ctl00$ContentPlaceHolder1$ddlSearchField", "pedido");
            request.AddParameter("ctl00$ContentPlaceHolder1$DropDownList2", ">=");
            request.AddParameter("ctl00$ContentPlaceHolder1$btnSearch", "Buscar");
            request.AddParameter("ctl00$ContentPlaceHolder1$ddlSearchOperation", "Contains");
            request.AddParameter("ctl00$ContentPlaceHolder1$TextBox1", "");
            request.AddParameter("ctl00$ContentPlaceHolder1$txtSearchValue", "128243408");
            request.AddParameter("ctl00$ContentPlaceHolder1$ddlPagerCount", "20");
            request.AddParameter("ctl00$ContentPlaceHolder1$ddlVisao", "0");
            request.AddParameter("ctl00$ContentPlaceHolder1$DropDownList1", "all");

            RestResponse response = client.Execute(request);
        }

        static void oldRascunho()
        {
            string tes = "321";
            char[] car = tes.ToCharArray();

            int aaa = int.Parse(car[0].ToString());
            int bbb = int.Parse(car[1].ToString());
            int ccc = int.Parse(car[2].ToString());




            Console.WriteLine("Pesquisa feita no https://www.adorocinema.com/ através de requisições. Material de estudo.");

            do
            {
                Console.Write("\nPesquisar por: ");
                string nomeFilmePesquisa = Console.ReadLine();
                List<Tuple<string, string, string>> locais = new List<Tuple<string, string, string>>
                {
                    new Tuple<string, string, string>("movie","movies-results","Filmes"),
                    new Tuple<string, string, string>("series","series-results","Series")
                };

                foreach (var local in locais)
                {
                    realizarRequisicao(nomeFilmePesquisa, local);
                }
            } while (true);
        }

        static void realizarRequisicao(string nomeFilmePesquisa, Tuple<string, string, string> dados)
        {
            var client = new RestClient("https://www.adorocinema.com");
            var request = new RestRequest($"/pesquisar/{dados.Item1}/?q={nomeFilmePesquisa.Replace(" ", "+")}", Method.Get);
            var response = client.Execute(request);

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                Console.WriteLine($"Falha na requisição. \n {response.StatusCode} - {response.ErrorMessage}");
                return;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response.Content);

            var seccao = htmlDoc.DocumentNode.SelectSingleNode($"//*[contains(@class,'{dados.Item2}')]");
            if (seccao != null)
            {
                var lista = seccao.ChildNodes.FirstOrDefault(x => x.Name == "ul").DescendantNodes().Where(x => x.Name == "li").ToList();
                Console.WriteLine($"\n{dados.Item3}: {lista.Count}");

                foreach (var item in lista)
                {
                    var dadosItem = item.DescendantNodes().Where(x => x.Name == "span");

                    Func<HtmlNode, string> buscaDado = (HtmlNode dado) =>
                    {
                        return dado == null ? "Sem Dados" : dado.InnerText.Equals("/") ? "Sem Dados" : dado.InnerText;
                    };
                    Console.WriteLine($"{buscaDado(dadosItem.ElementAtOrDefault(1))} || {buscaDado(dadosItem.ElementAtOrDefault(2))}");
                }
            }
        }
    }
}
