using System;
using System.IO;

namespace _17011492
{

    class Computer { // Computer class

        protected int id, custid = 0; // Computer id, 대여중인 사용자 id
        protected int DR, DL, DU;

        public static int total_cost = 0; // 결제된 총 금액

        public Computer(int id)
        {
            this.id = id;
        }

        public void Rent(int custid, int days) // 대여 등록
        {

            this.custid = custid;
            this.DR = days;
            this.DL = days;
            this.DU = 0;

        }

    }

    class Desktop:Computer { // Desktop Class, Computer 상속

        int charge = 0, deskid;
        static int costperday = 13000, have_val = 1; // 하루 대여 요금, value 값 1로 설정

        public Desktop(int id, int deskid) : base(id) {

            this.deskid = deskid;

        }

        public void PassAday() { // 하루 경과 시 정보 최신화

            if (custid == 0) return; // 대여중이지 않음

            DL--; // 잔여 일수 하루 감소
            DU++;
            charge += costperday; // 미수금 갱신

            if (DL == 0) { // 반환일 됐을 시 정산 후 반환

                Computer.total_cost += charge;
                charge = 0;
                custid = 0;

            }

        }

    }

    class Notebook : Computer // Desktop Class, Computer 상속
    {

        int charge = 0, noteid;
        static int costperday = 10000, have_val = 0; // 하루 대여 요금, value 값 0으로 설정

        public Notebook(int id, int noteid) : base(id) {

            this.noteid = noteid;

        }

        public void PassAday() // 하루 경과 시 정보 최신화 
        {

            if (custid == 0) return; // 대여중이지 않음 

            DL--; // 잔여 일수 하루 감소
            DU++;
            charge += costperday; // 마수금 갱신

            if (DL == 0) // 반환일 됐을 시 정산 후 반환
            {

                Computer.total_cost += charge;
                charge = 0;
                custid = 0;

            }


        }
    }

    class Netbook : Computer // Netbook Class, Computer 상속
    {

        int charge = 0, netid;
        static int costperday = 7000, have_val = 2; // 하루 대여 요금, value 값 2로 설정

        public Netbook(int id, int netid) : base(id) {

            this.netid = netid;

        }

        public void PassAday() // 하루 경과 시 정보 최신화
        {

            if (custid == 0) return; // 대여중이지 않음

            DL--; // 잔여 일수 하루 감소
            DU++;
            charge += costperday; // 미수금 갱신

            if (DL == 0) // 반환일 됐을 시 정산 후 반
            {

                Computer.total_cost += charge;
                charge = 0;
                custid = 0;

            }


        }

    }


    class User { // User Class

        public static int user_num; // User의 총 수
        int id; // 각 User id
        String name; // 각 User 이름

        public User(int id, String name) { // User 생성자

            this.id = id;
            this.name = name;

        }

    }

    class Student : User {

        static int need_val = 0; // notebook 대여 필요

        public Student(int id, String name) : base(id, name){ }

    }

    class Worker : User {

        static int need_val = 2; // netbook 대여 필요

        public Worker(int id, String name) : base(id, name) { }

    }

    class Gamer : User {

        static int need_val = 1; // desktop 대여 필요

        public Gamer(int id, String name) : base(id, name) { }

    }

    class ComputerManager {

        private Computer[] arrComp;
        private User[] arrUser;

        public void SetComputer(int num, int notebook_num, int desktop_num, int netbook_num) {

            arrComp = new Computer[num];
            int i;

            for (i = 0; i < notebook_num; i++)
            {

                arrComp[i] = new Notebook(i, i + 1);

            }

            for (int j = 1; j <= notebook_num; j++, i++)
            {

                arrComp[i] = new Notebook(i, j);

            }

            for (int k = 1; i <= notebook_num; k++, i++)
            {

                arrComp[i] = new Notebook(i, k);

            }


        }

        public void setUser() { }

        public static void A() {



        }

        public static void T() {



        }

        public static void S() { }

    }

    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            StreamWriter sw = new StreamWriter("output.txt");

            String tmpreadline;
            String[] readperkey;

            tmpreadline = sr.ReadLine(); // 컴퓨터 총 개수

            int num = Int32.Parse(tmpreadline);

            tmpreadline = sr.ReadLine(); // 노트북, 데스크톱, 넷북의 수
            readperkey = tmpreadline.Split(' '); // 문자열 Split으로 분할

            int notebook_num, desktop_num, netbook_num;

            notebook_num = Int32.Parse(readperkey[0]); // 종류별 컴퓨터의 개수
            desktop_num = Int32.Parse(readperkey[1]);
            netbook_num = Int32.Parse(readperkey[2]);


            tmpreadline = sr.ReadLine(); // 유저의 총 수
            User.user_num = Int32.Parse(tmpreadline);




            while (sr.Peek() >= 0) // 입력 파일에 더 이상 읽을 문자가 없을 때 까지 실행 
            {
                tmpreadline = sr.ReadLine(); // 입력파일에 한 줄의 문자열을 읽어와 string 변수에 tmpreadline 할당

                readperkey = tmpreadline.Split(' ');




            }


            sr.Close();
            sw.Close();

        }
    }
}
