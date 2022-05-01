using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _17011492_HW1
{
    class Computer
    { // Computer class

        protected int id, custid = 0; // Computer id, 대여중인 사용자 id
        protected int DR, DL, DU;

        public Computer(int id) // Computer 객체 생성자
        {
            this.id = id;
        }

        public void RentPC(int custid, int days) // 대여 등록 메소드, 대여자의 id와 대여 일수를 인자로 받는다
        {

            this.custid = custid; // 인자로 받은 정보들을 객체에 입력한다.
            this.DR = days;
            this.DL = days;
            this.DU = 0;

        }

        public virtual int PassAday(StreamWriter sw) { return 0; } // 하루가 지나는 메소드
        public virtual int returnCom(StreamWriter sw) { return 0; } // 대여 중인 컴퓨터를 반납하는 메소드

    }

    class Desktop : Computer
    { // Desktop Class, Computer 상속

        int charge = 0, deskid;
        static int costperday = 13000; // 하루 대여 요금을 13000원으로 설정

        public Desktop(int id, int deskid) : base(id)
        { // Desktop 객체 생성자

            this.deskid = deskid;

        }

        public override int PassAday(StreamWriter sw)
        { // 하루 경과 시 정보 최신화 메소드

            if (custid == 0) return 0; // 대여중이지 않음

            DL--; // 잔여 일수 하루 감소
            DU++; // 사용 일수 하루 증가
            charge += costperday; // 미수금 갱신

            if (DL == 0)
            { // 반환일 됐을 시 정산 후 반환

                int tmp_id = custid; // 대여자 Id 저장      

                sw.Write($"Time for Computer #{id} has expired. "); // 대여 기간 종료 문구 출력

                this.returnCom(sw); // 컴퓨터 반납 메소드 호출

                return tmp_id; // 반납된 컴퓨터의 대여자 Id 반환

            }

            return 0;

        }

        public override int returnCom(StreamWriter sw) // 컴퓨터 반납 메소드
        {

            ComputerManager.total_cost += charge; // 총 수납 요금에 대여료를 더해준다.      

            sw.WriteLine($"User #{custid} has returned Computer #{id} and paid {charge} won."); // 반납 문구 출력

            charge = 0; // 반납 처리를 위한 정보 최신화
            custid = 0;

            return 2; // Desktop이 반납되었음을 뜻하는 2 반환

        }

        public override String ToString()
        { // 해당 객체의 정보를 문자열로 반환하는 메소드

            String ret_str;

            if (custid == 0)
            {
                ret_str = $"type: Desktop, ComId: {id}, DeskId: {deskid}, Used for: internet, scientific, game, Avail: Y";
            }

            else
            {
                ret_str = $"type: Desktop, ComId: {id}, DeskId: {deskid}, Used for: internet, scientific, game, Avail: N (UserId: {custid}, DR: {DR}, DL: {DL}, DU: {DU})";
            }

            return ret_str;

        }

    }



    class Notebook : Computer // Desktop Class, Computer 상속
    {

        int charge = 0, noteid;
        static int costperday = 10000; // 하루 대여 요금을 10000원으로 설정

        public Notebook(int id, int noteid) : base(id)
        { // Notebook 객체 생성자

            this.noteid = noteid;

        }

        public override int PassAday(StreamWriter sw) // 하루 경과 시 정보 최신화 메소드
        {

            if (custid == 0) return 0; // 대여중이지 않음

            DL--; // 잔여 일수 하루 감소
            DU++; // 사용 일수 하루 증가
            charge += costperday; // 미수금 갱신

            if (DL == 0) // 반환일 됐을 시 정산 후 반환
            {

                int tmp_id = custid; // 대여자 Id 저장

                sw.Write($"Time for Computer #{id} has expired. "); // 사용 기간 종료 알림 문구 출력

                this.returnCom(sw); // 반납 메소드 호출

                return tmp_id; // 대여자 Id 반환

            }

            return 0;

        }

        public override int returnCom(StreamWriter sw) // 컴퓨터 반납 메소드
        {

            ComputerManager.total_cost += charge; // 총 수납 요금에 대여료를 더해준다.

            sw.WriteLine($"User #{custid} has returned Computer #{id} and paid {charge} won."); // 반납 문구 출력

            charge = 0; // 반납 후 정보 최신화
            custid = 0;

            return 1; // Notebook이 반환되었다는 뜻의 1 반납

        }

        public override String ToString() // 해당 객체의 정보를 문자열로 반환하는 메소드
        {

            String ret_str;

            if (custid == 0)
            {
                ret_str = $"type: Notebook, ComId: {id}, NoteId: {noteid}, Used for: internet, scientific, Avail: Y";
            }

            else
            {
                ret_str = $"type: Notebook, ComId: {id}, NoteId: {noteid}, Used for: internet, scientific, Avail: N (UserId: {custid}, DR: {DR}, DL: {DL}, DU: {DU})";
            }

            return ret_str;

        }

    }

    class Netbook : Computer // Netbook Class, Computer 상속
    {

        int charge = 0, netid;
        static int costperday = 7000; // 하루 대여 요금을 7000원으로 설정

        public Netbook(int id, int netid) : base(id) // Netbook 객체 생성자
        {
            this.netid = netid;
        }

        public override int PassAday(StreamWriter sw) // 하루 경과 시 정보 최신화 메소드
        {

            if (custid == 0) return 0; // 대여중이지 않음

            DL--; // 잔여 일수 하루 감소
            DU++; // 사용 일수 하루 증가
            charge += costperday; // 미수금 갱신

            if (DL == 0) // 반환일 됐을 시 정산 후 반환
            {

                int tmp_id = custid; // 대여자 Id 저장

                sw.Write($"Time for Computer #{id} has expired. "); // 사용 기간 종료 문구 출력

                this.returnCom(sw); // 반환 메소드 호출

                return tmp_id; // 대여자 Id 반환

            }

            return 0;

        }

        public override int returnCom(StreamWriter sw) // 컴퓨터 반납 메소드
        {

            ComputerManager.total_cost += charge; // 총 수납 요금에 대여료를 더해준다. 

            sw.WriteLine($"User #{custid} has returned Computer #{id} and paid {charge} won."); // 반납 문구 출력

            charge = 0; // 반납 후 정보 초기화
            custid = 0;

            return 3; // Netbook이 반납되었다는 뜻의 3 반환

        }

        public override String ToString() // 해당 객체의 정보를 문자열로 반환하는 메소드
        {

            String ret_str;

            if (custid == 0)
            {
                ret_str = $"type: Netbook, ComId: {id}, NetId: {netid}, Used for: internet, Avail: Y";
            }

            else
            {
                ret_str = $"type: Netbook, ComId: {id}, NetId: {netid}, Used for: internet, Avail: N (UserId: {custid}, DR: {DR}, DL: {DL}, DU: {DU})";
            }

            return ret_str;

        }

    }


    class User
    { // User Class

        protected int id; // 각 User id
        protected String name; // 각 User 이름
        public int rent_id = 0;
        public int need_val;

        public User(int id, String name)
        { // User 생성자

            this.id = id;
            this.name = name;

        }

        public void RentUser(int ComId)
        { // 컴퓨터 대여 정보 입력 메소드

            rent_id = ComId; // 대여받은 컴퓨터의 Id 초기화

        }

        public void return_Comp()
        { // 컴퓨터 반납 메소드

            rent_id = 0; // 대여중인 컴퓨터가 없다는 뜻으로 rend_id를 0으로 초기화

        }

    }

    class Student : User
    { // Student Class, User 상속

        int st_id; // 학생 Id

        public Student(int id, String name, int st_id) : base(id, name)
        { // Student 객체 생성자
            this.st_id = st_id;
            need_val = 1; // Notebook 대여 필요
        }

        public override String ToString()
        { // 객체 정보를 문자열로 반환하는 메소드

            String ret_str;

            if (rent_id == 0)
                ret_str = $"type: Students, Name: {name}, UserId: {id}, StudId: {st_id}, Used for: internet, scientific, Rent: N";

            else
                ret_str = $"type: Students, Name: {name}, UserId: {id}, StudId: {st_id}, Used for: internet, scientific, Rent: Y (RentCompId: {rent_id})";

            return ret_str;

        }

    }

    class Worker : User
    { // Worker Class, User 상속


        int wk_id;

        public Worker(int id, String name, int wk_id) : base(id, name)
        { // Worker 객체 생성자
            this.wk_id = wk_id;
            need_val = 3; // netbook 대여 필요
        }

        public override String ToString() // 객체 정보를 문자열로 반환하는 메소드
        {

            String ret_str;

            if (rent_id == 0)
                ret_str = $"type: OfficeWorkers, Name: {name}, UserId: {id}, WorkerId: {wk_id}, Used for: internet, Rent: N";

            else
                ret_str = $"type: OfficeWorkers, Name: {name}, UserId: {id}, WorkerId: {wk_id}, Used for: internet, Rent: Y (RentCompId: {rent_id})";

            return ret_str;

        }

    }

    class Gamer : User
    { // Gamer Class, User 상속

        int gm_id;

        public Gamer(int id, String name, int gm_id) : base(id, name)
        { // Gamer 객체 생성자
            this.gm_id = gm_id;
            need_val = 2; // Desktop 대여 필요
        }

        public override String ToString() // 객체 정보를 문자열로 반환하는 메소드
        {

            String ret_str;

            if (rent_id == 0)
                ret_str = $"type: Gamers, Name: {name}, UserId: {id}, GamerId: {gm_id}, Used for: internet, game, Rent: N";

            else
                ret_str = $"type: Gamers, Name: {name}, UserId: {id}, GamerId: {gm_id}, Used for: internet, game, Rent: Y (RentCompId: {rent_id})";

            return ret_str;

        }

    }

    class ComputerManager
    { // ComputerManager Class, 컴퓨터 대여 시스템을 관리하는 Class이다.

        private Computer[] arrComp; // 시스템 내의 컴퓨터들을 객체로 관리하는 Computer 배열
        private User[] arrUser; // 시스템 이용자들을 객체로 관리하는 User 배열

        public int com_num; // Computer의 총 수
        public int user_num; // User의 총 수

        private int u; // User 객체 생성 시 사용할 변수 u
        int st = 0, wk = 0, gm = 0; // User type에 따른 id 부여에 사용할 변수들

        int desk_top, net_top, note_top; // 배열 내의 컴퓨터들을 종류별로 분류하는 index로 사용될 변수 (Stack의 top 역할) 
        int desk_max, net_max, note_max; // 배열 내의 컴퓨터들을 종류별로 분할하는 index의 경계로 사용될 변수 (Stack의 상한선 역할)

        public static int total_cost = 0; // 결제된 총 금액

        public void SetComputer(int notebook_num, int desktop_num, int netbook_num)
        { // Computer 객체 배열 초기화 및 인자 입력 메소드

            arrComp = new Computer[com_num]; // 입력된 컴퓨터의 총 개수 만큼 배열 초기화
            int i;

            net_max = netbook_num; // 배열 내의 index로 사용될 변수들 초기화
            note_max = net_max + notebook_num;
            desk_max = note_max + desktop_num;
            net_top = 0;
            note_top = net_max;
            desk_top = note_max;

            for (i = 0; i < netbook_num; i++) // Netbook 객체를 배열 내에 차례대로 생성
            {

                arrComp[i] = new Netbook(i + 1, i + 1);

            }

            for (int j = 1; j <= notebook_num; j++, i++) // Notebook 객체를 배열 내에 차례대로 생성
            {

                arrComp[i] = new Notebook(i + 1, j);

            }

            for (int k = 1; k <= desktop_num; k++, i++) // Desktop 객체를 배열 내에 차례대로 생성
            {

                arrComp[i] = new Desktop(i + 1, k);

            }


        }

        public void setUser(int num)
        { // User 객체 배열 초기화

            arrUser = new User[num];
            u = 0;

        }

        public void insertUser(String type, String name)
        { // User 객체 배열 인자 입력 메소드, 각 이용자들의 type에 맞게 객체를 생성하여 이름과 Id 입력

            if (type.Equals("Student"))
            {
                st++;
                arrUser[u] = new Student(u + 1, name, st);
                u++;
            }

            else if (type.Equals("Gamer"))
            {
                gm++;
                arrUser[u] = new Gamer(u + 1, name, gm);
                u++;
            }

            else if (type.Equals("Worker"))
            {
                wk++;
                arrUser[u] = new Worker(u + 1, name, wk);
                u++;
            }

        }

        public void A(StreamWriter sw, int cust_id, int days) // 사용자에게 컴퓨터를 할당해주는 메소드
        {

            int need_type = arrUser[cust_id - 1].need_val; // 이용자가 어떤 타입의 컴퓨터를 원하는지 확인

            if (need_type == 1)
            { // Notebook을 원하는 경우

                note_top++; // top을 1 증가

                if (note_top > note_max)
                { // 대여 가능한 Notebook이 없을 경우 안내 문구 출력 후 종료
                    sw.WriteLine($"User #{cust_id} can't rent a Computer.");
                    return;
                }

                arrComp[note_top - 1].RentPC(cust_id, days); // 대여 가능할 경우 대여 처리
                arrUser[cust_id - 1].RentUser(note_top);

                sw.WriteLine($"Computer #{note_top} has been assigned to User #{cust_id}"); // 대여 처리 문구 출력

            }

            else if (need_type == 2) // Desktop을 원하는 경우
            {

                desk_top++; // top을 1 증가

                if (desk_top > desk_max) // 대여 가능한 Desktop이 없을 경우 안내 문구 출력 후 종료
                {
                    sw.WriteLine($"User #{cust_id} can't rent a Computer.");
                    return;
                }

                arrComp[desk_top - 1].RentPC(cust_id, days); // 대여 가능할 경우 대여 처리
                arrUser[cust_id - 1].RentUser(desk_top);

                sw.WriteLine($"Computer #{desk_top} has been assigned to User #{cust_id}"); // 대여 처리 문구 출력

            }

            else if (need_type == 3) // Netbook을 원하는 경우
            {

                net_top++; // top을 1 증가

                if (net_top > net_max) // 대여 가능한 Netbook이 없을 경우 안내 문구 출력 후 종료
                {
                    sw.WriteLine($"User #{cust_id} can't rent a Computer.");
                    return;
                }

                arrComp[net_top - 1].RentPC(cust_id, days); // 대여 가능할 경우 대여 처리
                arrUser[cust_id - 1].RentUser(net_top);

                sw.WriteLine($"Computer #{net_top} has been assigned to User #{cust_id}"); // 대여 처리 문구 출력

            }

        }

        public void R(StreamWriter sw, int cust_id)
        { // 사용자의 컴퓨터 반납을 처리하는 메소드

            int tmp_comid, tmp;

            tmp_comid = arrUser[cust_id - 1].rent_id; // 사용자가 대여했던 컴퓨터의 Id 저장

            if (tmp_comid == 0) return; // 만약 대여 중이지 않다면 종료

            tmp = arrComp[tmp_comid - 1].returnCom(sw); // 대여 중이었을 경우 해당 컴퓨터의 반납 메소드 호출

            if (tmp == 1) note_top--; // 반납 후 top을 1 감소
            else if (tmp == 2) desk_top--;
            else if (tmp == 3) net_top--;

            arrUser[cust_id - 1].return_Comp(); // 사용자 객체에서도 반납 처리

        }

        public void T(StreamWriter sw)
        { // 사용 시간 하루 경과 처리 메소드

            sw.WriteLine("It has passed one day"); // 하루가 경과됐다는 문구 출력

            int tmp_custid;
            int tmp;

            for (int i = 0; i < com_num; i++)
            { // 컴퓨터 배열 내 모든 컴퓨터 객체의 대여 일수 갱신

                tmp_custid = arrComp[i].PassAday(sw); // 각 컴퓨터 객체의 하루 경과를 알리는 메소드 호출, 갱신된 날짜가 반납일이면 컴퓨터 반납 처리 후 사용자 Id 반환

                if (tmp_custid != 0)
                { // 반납일인 사용자일 경우

                    arrUser[tmp_custid - 1].return_Comp(); // 사용자 객체 반납 처리

                    tmp = arrUser[tmp_custid - 1].need_val; // 반납된 컴퓨터의 타입 확인

                    if (tmp == 1) note_top--; // top 1 감소
                    else if (tmp == 2) desk_top--;
                    else if (tmp == 3) net_top--;

                }

            }

        }

        public void S(StreamWriter sw) // 현재 상태를 표시하는 메소드
        {

            int i;

            sw.WriteLine($"Total Cost: {total_cost}"); // 현재 지불된 총 금액 출력

            sw.WriteLine("Computer List:");
            for (i = 0; i < com_num; i++)
            { // 컴퓨터들의 대여 상황 출력
                sw.WriteLine($"({i + 1}) " + arrComp[i].ToString());
            }

            sw.WriteLine("User List:");
            for (i = 0; i < user_num; i++)
            { // 사용자의 현재 상황 정보 출력
                sw.WriteLine($"({i + 1}) " + arrUser[i].ToString());
            }


        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            StreamWriter sw = new StreamWriter("output.txt");

            ComputerManager manager = new ComputerManager();

            String tmpreadline;
            String[] readperkey;

            tmpreadline = sr.ReadLine(); // 컴퓨터 총 개수
            manager.com_num = Int32.Parse(tmpreadline);

            tmpreadline = sr.ReadLine(); // 노트북, 데스크톱, 넷북의 수
            readperkey = tmpreadline.Split(' '); // 문자열 Split으로 분할

            int notebook_num, desktop_num, netbook_num;

            notebook_num = Int32.Parse(readperkey[0]); // 종류별 컴퓨터의 개수
            desktop_num = Int32.Parse(readperkey[1]);
            netbook_num = Int32.Parse(readperkey[2]);

            manager.SetComputer(notebook_num, desktop_num, netbook_num); // manager 객체 내 Computer 배열 정보 초기화

            tmpreadline = sr.ReadLine(); // 유저의 총 수
            manager.user_num = Int32.Parse(tmpreadline);

            manager.setUser(manager.user_num); // manager 객체 내 User 배열에 정보 입력
            for (int i = 0; i < manager.user_num; i++)
            {

                tmpreadline = sr.ReadLine();
                readperkey = tmpreadline.Split(' ');

                manager.insertUser(readperkey[0], readperkey[1]);

            }

            while (sr.Peek() >= 0) // 입력 파일에 더 이상 읽을 문자가 없을 때 까지 실행 
            {
                tmpreadline = sr.ReadLine(); // 입력파일에 한 줄의 문자열을 읽어와 string 변수에 tmpreadline 할당

                readperkey = tmpreadline.Split(' ');

                if (readperkey[0].Equals("Q")) // Q 입력 시 프로그램 종료  
                {                   

                    break;
                }

                else if (readperkey[0].Equals("A")) // A 입력 시 같이 입력받은 정보들로 컴퓨터 할당 메소드 호출
                { 
                    manager.A(sw, Int32.Parse(readperkey[1]), Int32.Parse(readperkey[2]));
                }

                else if (readperkey[0].Equals("R")) // R 입력 시 같이 입력받은 사용자의 컴퓨터 반납 메소드 호출
                { 
                    manager.R(sw, Int32.Parse(readperkey[1]));
                }

                else if (readperkey[0].Equals("T")) // T 입력 시 하루 시간 경과 메소드 호출
                { 
                    manager.T(sw);
                }

                else if (readperkey[0].Equals("S")) // S 입력 시 현재 상태를 표시하는 메소드 호출
                {
                    manager.S(sw);
                }

                sw.WriteLine("===========================================================");

            }

            sr.Close();
            sw.Close();

        }
    }
}
