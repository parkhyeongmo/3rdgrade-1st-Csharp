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

        public int GetPrio() { // 우선순위를 반환해주는 메소드
            return priority;
        }

        public override string ToString() { // 자동차 정보를 문자열로 반환하는 메소드

            return $"FNUM: {vehicleId} DEST: {destination} PRIO: {priority}";

        }

    }

    class WaitPlace { // 대기장소 Class

        private int waitId, numVehicle; // 대기장소ID, 보관중인 자동차 개수
        private DeliveryVehicle[] vehicles; // 대기장소에 있는 자동차 배열

        public WaitPlace (int Id){ // 생성자, ID를 인자로 받는다.
            waitId = Id;
            numVehicle = 0; // 보관중인 자동차는 0대로 설정한다.
        }

        public int GetnumVehicle() { // 대기장소의 자동차 수 반환 메소드
            return numVehicle;
        }
        
        public string Parking(DeliveryVehicle vehicle) { // 대기장소에 자동차 정보 입력

            numVehicle++;
            Array.Resize(ref vehicles, numVehicle);

            int new_prio = vehicle.GetPrio();

            if (numVehicle == 1) { // 최초 배정된 자동차는 바로 배열에 투입한다.
                vehicles[0] = vehicle;
                return $"Vehicle {vehicle.GetId()} assigned to WaitPlace #{waitId}"; // 작동 결과 문자열로 반환
            }

            int i;

            for (i = 0; i < vehicles.Length - 1; i++) { // 우선순위를 고려하여 투입될 배열의 위치를 탐색한다.
                if (vehicles[i].GetId() < vehicle.GetId())
                    break;
            }

            for (int j = vehicles.Length - 2; j >= i; j--) { // 해당 위치에 삽입하기 위해 기존 자동차들의 위치를 옮겨준다.

                vehicles[j + 1] = vehicles[j];

            }

            vehicles[i] = vehicle; // 알맞은 순서의 위치에 자동차 객체 삽입     

            return $"Vehicle {vehicle.GetId()} assigned to WaitPlace #{waitId}"; // 작동 결과 문자열로 반환

        }

        public int Deliver() { // 대기장소의 자동차들 중 우선순위가 가장 높은 자동차를 배달 보내는 메소드

            int tmp = vehicles[0].GetId(); // 우선순위가 가장 높은 배열 맨 앞의 자동차Id를 미리 저장한다. 

            for (int i = 0; i < vehicles.Length - 1; i++) { // 배열의 두 번째 자동차 객체부터 마지막 자동차 객체까지 모두 한 칸씩 당겨준다.
                vehicles[i] = vehicles[i + 1];
            }

            Array.Resize(ref vehicles, numVehicle - 1); // 배열의 크기를 하나 줄여주고 자동차의 수도 하나 줄여준다.
            numVehicle--;

            return tmp; // 배달된 자동차의 ID를 반환한다.

        }

        public bool FindId(int search_id) { // 대기장소에 주어진 Id를 가진 자동차가 있는지 확인하는 메소드

            if (numVehicle == 0) { // 대기중인 자동차가 없을 경우 false 반환
                return false;
            }

            foreach (DeliveryVehicle i in vehicles) { // 자동차 배열을 탐색하며 해당 Id를 가진 자동차가 있다면 true 반환

                if (i.GetId() == search_id)
                    return true;
            
            }

            return false; // 탐색 결과 발견되지 않으면 false 반환

        }

        public void Cancelation(int id) { // 대기장소에 주어진 Id를 가진 자동차를 삭제하는 메소드

            int i;

            for (i = 0; i < vehicles.Length; i++) { // 배열 내에서 해당 Id의 자동차 위치를 탐색
                if (vehicles[i].GetId() == id) {
                    break;
                }
            }

            for (; i < vehicles.Length - 1; i++) { // 발견한 위치의 자동차를 제외하여 배열을 빈칸이 없도록 정렬해준다.

                vehicles[i] = vehicles[i + 1];

            }

            Array.Resize(ref vehicles, numVehicle - 1); // 배열의 크기를 하나 줄여주고 자동차의 수도 하나 줄여준다.
            numVehicle--;

        }

        public void StatusWP(StreamWriter sw) { // Status 명령 시 해당 대기장소의 정보를 출력하는 메소드

            sw.WriteLine(this.ToString());

            if (numVehicle != 0) { // 대기중인 자동차가 있을 경우 정보 출력
                for (int i = 0; i < vehicles.Length; i++) {
                    sw.WriteLine(vehicles[i].ToString());
                }
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

                waitPlaces[i] = new WaitPlace(i + 1); // 각 대기장소의 객체 생성

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

            foreach (WaitPlace i in waitPlaces) { // 각 대기장소의 정보를 차례대로 출력
                i.StatusWP(sw);
                sw.WriteLine("---------------------------------------------------");
            }

            sw.WriteLine("********************** End Delivery Vehicle Info **********************");

        }

        public void Cancel(StreamWriter sw, int cancel_id) { // Cancel 명령 메소드

            WaitPlace tmp;

            for (int i = 0; i < waitPlaces.Length; i++) { // 해당 Id를 가진 자동차를 찾아 각 대기장소를 탐색
                
                tmp = waitPlaces[i];

                if (tmp.FindId(cancel_id)) { // 발견 시 삭제 메소드 호출 후 결과 출력
                    tmp.Cancelation(cancel_id); // 삭제 메소드
                    sw.WriteLine($"Cancelation of Vehicle {cancel_id} completed.");
                    return;
                }

            }           

            sw.WriteLine($"Can't Find Vehicle {cancel_id}. Cancelation failed."); // 발견 실패 시 문구 출력

        }

        public void Deliver(StreamWriter sw, int waitId) { // Deliver 명령 메소드

            int vehicleId = waitPlaces[waitId - 1].Deliver(); // 해당 대기장소에 Deliver 메소드 호출 후 배달 완료한 자동차ID 반환받음.

            sw.WriteLine($"Vehicle {vehicleId} used to deliver."); // 결과 출력

        }

        public void Clear(StreamWriter sw, int waitId) { // Clear 명령 메소드

            waitPlaces[waitId - 1].ClearPlace(); // 해당 대기장소의 전체 자동차 대기 취소

            sw.WriteLine($"WaitPlace #{waitId} cleared."); // 작동결과 출력

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
                    
                    int place = Int32.Parse(readperline[1].Substring(1));
                    int id = Int32.Parse(readperline[2]);
                    int priority = Int32.Parse(readperline[4].Substring(1));

                    DVM.ReadyIn(sw, place, id, readperline[3], priority);

                }

                else if (readperline[0].Equals("Ready")) {

                    int id = Int32.Parse(readperline[1]);
                    int priority = Int32.Parse(readperline[3].Substring(1));

                    DVM.Ready(sw, id, readperline[2], priority);

                }

                else if (readperline[0].Equals("Status")) {

                     DVM.Status(sw);

                }

                else if (readperline[0].Equals("Cancel")) {

                    int id = Int32.Parse(readperline[1]);                    
                    DVM.Cancel(sw, id);

                }

                else if (readperline[0].Equals("Deliver")) {

                    int id = Int32.Parse(readperline[1].Substring(1));
                    DVM.Deliver(sw, id);

                }

                else if (readperline[0].Equals("Clear")) {

                    int waitId = Int32.Parse(readperline[1].Substring(1));
                    DVM.Clear(sw, waitId);

                }

                else if (readperline[0].Equals("Quit")) break;

            }

            sr.Close();
            sw.Close();

        }
    }
}
