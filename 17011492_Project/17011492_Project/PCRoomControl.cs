using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace _17011492
{
    class PCRoomControl
    {
        public static List<PC> PCs = new List<PC>(); // 프로그램에 사용되는 객체를 관리할 List
        public static List<User> Users = new List<User>();
        public static List<Product> Products = new List<Product>();
        public static int sales; // 매출 금액 변수

        static PCRoomControl()
        {
            Load();
        }

        public static void Load() // xml 파일 읽기 메소드
        {
            try // PC, 회원, 상품은 xml 파일로 읽기와 저장한다.
            {
                XmlDocument readSales = new XmlDocument(); // sales.xml 파일 읽기
                readSales.Load("sales.xml");
                XmlNodeList sale = readSales.GetElementsByTagName("sale");
                foreach (XmlNode item in sale) {
                    sales = int.Parse(item["cash"].InnerText);
                }

                XmlDocument readPCXml = new XmlDocument(); // PCs.xml 파일을 읽어 객체로 변환 후 List에 저장
                readPCXml.Load("PCs.xml");
                XmlNodeList pcList = readPCXml.GetElementsByTagName("PC");
                foreach (XmlNode item in pcList)
                {
                    PCs.Add(new PC()
                    {
                        Id = int.Parse(item["Id"].InnerText),
                        power = item["power"].InnerText != "0" ? true : false,
                        inUse = item["inUse"].InnerText != "0" ? true : false,
                        Payment = item["Payment"].InnerText,
                        UserId = int.Parse(item["UserId"].InnerText),
                        UserName = item["UserName"].InnerText,
                        ChargeTime = item["ChargeTime"].InnerText
                    });

                }

                XmlDocument readUserXml = new XmlDocument();// Users.xml 파일을 읽어 객체로 변환 후 List에 저장
                readUserXml.Load("Users.xml");
                XmlNodeList userList = readUserXml.GetElementsByTagName("User");
                foreach (XmlNode item in userList)
                {
                    Users.Add(new User()
                    {
                        Id = int.Parse(item["Id"].InnerText),
                        Name = item["Name"].InnerText,
                        Charge = int.Parse(item["Charge"].InnerText)
                    });
                }

                XmlDocument readProductXml = new XmlDocument(); // Products.xml 파일을 읽어 객체로 변환 후 List에 저장
                readProductXml.Load("Products.xml");
                XmlNodeList productList = readProductXml.GetElementsByTagName("Product");
                foreach (XmlNode item in productList)
                {
                    Products.Add(new Product()
                    {
                        Name = item["Name"].InnerText,
                        Price = int.Parse(item["Price"].InnerText)
                    });
                }
            }
            catch (FileLoadException exception) // 읽기 실패시 xml 파일 생성 및 저장
            {
                Save();
            }
        }

        public static void Save() // xml 파일 저장 메소드
        {
            string PCsOutput = "";
            PCsOutput += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\n";
            PCsOutput += "<PCs>\n";
            foreach (var item in PCs)  // List 내부의 객체들의 정보를 문자열로 변환
            {
                PCsOutput += "<PC>\n";
                PCsOutput += "<Id>" + item.Id + "</Id>\n";
                PCsOutput += "<power>" + (item.power ? 1 : 0) + "</power>\n";
                PCsOutput += "<inUse>" + (item.inUse ? 1 : 0) + "</inUse>\n";
                PCsOutput += "<Payment>" + item.Payment + "</Payment>\n";
                PCsOutput += "<UserId>" + item.UserId + "</UserId>\n";
                PCsOutput += "<UserName>" + item.UserName + "</UserName>\n";
                PCsOutput += "<ChargeTime>" + item.ChargeTime + "</ChargeTime>\n";
                PCsOutput += "</PC>\n";
            }
            PCsOutput += "</PCs>";

            string UsersOutput = "";
            UsersOutput += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\n";
            UsersOutput += "<Users>\n";
            foreach (var item in Users) 
            {
                UsersOutput += "<User>\n";
                UsersOutput += "<Id>" + item.Id + "</Id>\n";
                UsersOutput += "<Name>" + item.Name + "</Name>\n";
                UsersOutput += "<Charge>" + item.Charge + "</Charge>\n";
                UsersOutput += "</User>\n";
            }
            UsersOutput += "</Users>";

            string SalesOutput = "";
            SalesOutput += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\n";
            SalesOutput += "<sales>\n";
            SalesOutput += "<sale>\n";
            SalesOutput += "<cash>" + sales + "</cash>\n";
            SalesOutput += "</sale>\n";
            SalesOutput += "</sales>";

            File.WriteAllText(@"./PCs.xml", PCsOutput); // List 내부의 객체 정보들을 xml 파일에 저장
            File.WriteAllText(@"./Users.xml", UsersOutput);
            File.WriteAllText(@"./sales.xml", SalesOutput);

        }

    }
}
