using System;
using System.IO;

namespace _17011492_HW2
{

    class DeliveryVehicle { // 자동차 class

        private int vehicleId, priority; // 자동차ID, 우선순위 필드 변수 선언
        private string destination; // 배달 목적지 문자열 필드 변수 선언

        public DeliveryVehicle(int id, string destination, int priority) { // 생성자

            vehicleId = id;
            this.destination = destination;
            this.priority = priority;

        }

        public int GetId(){ // 자동차Id를 반환해주는 메소드
            return vehicleId;
        }

        public override string ToString() { // 자동차 정보를 문자열로 반환하는 메소드

            return $"FNUM: {vehicleId} DEST: {destination} PRIO: {priority}";

        }

    }

    class WaitPlace { // 대기장소 Class

        private int waitId, numVehicle; // 대기장소ID, 보관중인 자동차 개수
        private DeliveryVehicle[] vehicles; // 대기장소에 있는 자동차 배열

        public void SetWaitPlace(int Id, int num){ // 대기장소 객체 내부 정보 설정 메소드
            waitId = Id;
            numVehicle = num;
        }

        public int GetnumVehicle() { // 대기장소의 자동차 수 반환 메소드
            return numVehicle;
        }
        
        public string Parking(DeliveryVehicle vehicle) { // 대기장소에 자동차 정보 입력

            numVehicle++;
            Array.Resize(ref vehicles, numVehicle);

            // 인자로 받은 자동차 객체를 우선순위를 고려하여 배열에 삽입


            return $"Vehicle {vehicle.GetId()} assigned to waitPlace #{waitId}"; // 작동 결과 문자열로 반환

        }

        public void StatusWP(StreamWriter sw) { // Status 명령 시 해당 대기장소의 정보를 출력하는 메소드

            sw.WriteLine(this.ToString());

            foreach (DeliveryVehicle i in vehicles) {
                sw.WriteLine(i.ToString());
            }

        }

        public void ClearPlace() { // Clear 명령 시 해당 대기장소의 모든 자동차의 대기를 취소한다.
            Array.Resize(ref vehicles, 0);
            numVehicle = 0;
        }

        public override string ToString() // 대기장소의 정보를 문자열로 반환하는 메소드
        {
            return $"WaitPlace #{waitId} Number Vehicle: {numVehicle}";
        }

    }

    class DeliveryVehicleManager { // 배달 자동차 관리 시스템 Class

        int numWaitingPlaces; // 대기장소 총 개수
        WaitPlace[] waitPlaces; // 대기장소 배열

        public DeliveryVehicleManager(int num){ // 생성자

            numWaitingPlaces = num;
            waitPlaces = new WaitPlace[numWaitingPlaces]; // 대기장소의 개수만큼 배열 선언

            for (int i = 0; i < num; i++) {

                waitPlaces[i].SetWaitPlace(i + 1, 0); // 각 대기장소의 정보 초기화

            }

        }

        public void ReadyIn(StreamWriter sw, int numWaitPlace, int vehicleId, string destination, int priority) // ReadyIn 명령 시 입력된 대기장소에 자동차를 배정하는 메소드
        {

            DeliveryVehicle tmp = new DeliveryVehicle(vehicleId, destination, priority); // 자동차 객체 생성
            sw.WriteLine(waitPlaces[numWaitPlace - 1].Parking(tmp)); // 생성한 자동차 객체를 대기장소에 배정하고 반환받은 문자열 출력

        }

        public void Ready(StreamWriter sw, int vehicleId, string destination, int priority) { // Readt 명령 시 가장 적은 자동차가 대기중인 장소에 자동차를 배정하는 메소드

            int min, idx;

            min = waitPlaces[0].GetnumVehicle(); // 1번째 대기장소(index 0)의 값으로 정보 초기화
            idx = 0;

            for (int i = 0; i<waitPlaces.Length;i++) { // 가장 적은 수의 자동차가 있는 대기장소 탐색

                int now_num = waitPlaces[i].GetnumVehicle();

                if (now_num < min) {
                    min = now_num;
                    idx = i;
                }

            }

            DeliveryVehicle tmp = new DeliveryVehicle(vehicleId, destination, priority); // 자동차 객체 생성
            sw.WriteLine(waitPlaces[idx].Parking(tmp)); // 생성한 자동차 객체를 대기장소에 배정하고 반환받은 문자열 출력


        }

        public void Status(StreamWriter sw) { // Status 명령 메소드

            sw.WriteLine("************************ Delivery Vehicle Info ************************");

            sw.WriteLine($"Number of WaitPlaces: {numWaitingPlaces}"); // 대기장소의 수 출력

            sw.WriteLine("---------------------------------------------------");

            foreach (WaitPlace i in waitPlaces) { // 각 대기장소의 정보를 차례대로 출력
                i.StatusWP(sw);
                sw.WriteLine("---------------------------------------------------");
            }

            sw.WriteLine("************************ End Delivery Vehicle Info ************************");

        }

        public void Clear(StreamWriter sw, string wait_num) { // Clear 명령 메소드

            int clear_idx = wait_num[1] - '0'; // 입력된 문자열로부터 대기장소의 index 저장

            waitPlaces[clear_idx].ClearPlace(); // 해당 대기장소의 전체 자동차 대기 취소

            sw.WriteLine($"WaitPlace #{clear_idx} cleared."); // 작동결과 출력

        }

    }
    

    class Program
    {
        static void Main(string[] args)
        {

            StreamReader sr = new StreamReader("input.txt");
            StreamWriter sw = new StreamWriter("output.txt");

            string tmpreadLine;
            string[] readperline;

            tmpreadLine = sr.ReadLine();
            int numWaitingPlaces = Int32.Parse(tmpreadLine);

            DeliveryVehicleManager DVM = new DeliveryVehicleManager(numWaitingPlaces);

            while (sr.Peek() >= 0) { // input.txt 파일에 더 이상 읽을 문자가 없을 때까지 실행

                tmpreadLine = sr.ReadLine();
                readperline = tmpreadLine.Split(' ');

                if (readperline[0].Equals("ReadyIn"))  // 명령에 따라 메소드 호출
                {
                    int place, id, priority;

                    place = readperline[1][1] - '0';
                    id = Int32.Parse(readperline[2]);
                    priority = readperline[4][1] - '0';

                    DVM.ReadyIn(sw, place, id, readperline[3], priority);

                }

                else if (readperline[0].Equals("Ready")) {

                    int id, priority;

                    id = Int32.Parse(readperline[1]);
                    priority = readperline[3][1] - '0';

                    DVM.Ready(sw, id, readperline[2], priority);


                }

                else if (readperline[0].Equals("Status")) {
                    DVM.Status(sw);
                }

                else if (readperline[0].Equals("Cancel")) {

                    int id;

                    id = Int32.Parse(readperline[1]);



                }

                else if (readperline[0].Equals("Deliver")) { }

                else if (readperline[0].Equals("Clear")) { }

                else if (readperline[0].Equals("Quit")) break;

            }


            sr.Close();
            sw.Close();

        }
    }
}
