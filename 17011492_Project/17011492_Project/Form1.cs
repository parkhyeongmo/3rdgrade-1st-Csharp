using _17011492;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _17011492_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "세종 PC방";
                         
            dataGridView2.ReadOnly = true; // 각 정보를 dataGridView 폼에서 직접 수정이 불가능하도록 설정
            dataGridView3.ReadOnly = true;
            timer1.Start();

            try // PCRoomControl의 객체 List 정보들을 프로그램에 출력
            {
                sales_label.Text = PCRoomControl.sales.ToString();

                dataGridView1.DataSource = PCRoomControl.PCs;
                dataGridView2.DataSource = PCRoomControl.Products;
                dataGridView3.DataSource = PCRoomControl.Users;

                dataGridView1.Columns[0].ReadOnly = true; // PC 정보를 dataGridView 폼에서 직접 수정이 불가능하도록 설정
                dataGridView1.Columns[2].ReadOnly = true; // 전원 조작(power) 컬럼은 조작이 가능하도록 설정
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = true;

            }
            catch (Exception exception) { }

            dataGridView1.CurrentCellChanged += DataGridView1_CurrentCellChanged;
            dataGridView2.CurrentCellChanged += DataGridView2_CurrentCellChanged;
            dataGridView3.CurrentCellChanged += DataGridView3_CurrentCellChanged;

        }

        private void DataGridView1_CurrentCellChanged(object sender, EventArgs e) // PC 정보 출력창의 Cell을 선택할 경우
        {
            try
            {
                PC selected_PC = dataGridView1.CurrentRow.DataBoundItem as PC;
                textBox1.Text = selected_PC.Id.ToString(); // 선택된 PC Id를 textBox에 입력
            }

            catch (Exception exception)
            {
            }
        }

        private void DataGridView2_CurrentCellChanged(object sender, EventArgs e) // 상품 정보 출력창의 Cell을 선택할 경우
        {
            try
            {
                Product selected_Product = dataGridView2.CurrentRow.DataBoundItem as Product;
                textBox3.Text = selected_Product.Name; // 선택된 상품의 정보를 textBox에 입력
                textBox4.Text = selected_Product.Price.ToString();
                
            }

            catch (Exception exception)
            {
            }
        }

        private void DataGridView3_CurrentCellChanged(object sender, EventArgs e) // 회원 정보 출력창의 Cell을 선택할 경우
        {
            try
            {
                User selected_User = dataGridView3.CurrentRow.DataBoundItem as User;
                textBox2.Text = selected_User.Id.ToString(); // 선택된 회원의 Id를 textBox에 입력
            }

            catch (Exception exception)
            {
            }
        }

        private void member_button_Click(object sender, EventArgs e) // 회원 관리 창 호출
        {
            new Form2().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) // 총 매출 새로고침
        {
            sales_label.Text = PCRoomControl.sales.ToString();
            PCRoomControl.Save();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = PCRoomControl.PCs;

            dataGridView3.DataSource = null;
            dataGridView3.DataSource = PCRoomControl.Users;
        }

        private void button2_Click(object sender, EventArgs e) // 제품 판매
        {
            PCRoomControl.sales += int.Parse(textBox4.Text);
            sales_label.Text = PCRoomControl.sales.ToString();
            PCRoomControl.Save();
        }

        private void button1_Click(object sender, EventArgs e) // PC 사용 시작
        {
            int cost = 1000;
            int time;

            try
            {
                time = int.Parse(textBox5.Text); // 입력 받은 시간 저장 및 결제 금액 환산

                if (textBox2.Text == "0") // 비회원의 경우 요금을 시간당 1200원으로 계산
                    cost = 1200;

                cost *= time; // 요금은 시간당 1000원으로 계산


            }

            catch (Exception exception) { // 예외 처리
                MessageBox.Show("시간 단위로 정수를 입력해주세요.");
                return;
            }


            if (textBox1.Text == "") // PC가 선택되지 않은 경우 안내문구 출력
                MessageBox.Show("PC를 선택해주세요.");

            else
            {
                PC pc = PCRoomControl.PCs.Single((x) => x.Id.ToString() == textBox1.Text);
                User user;

                if (!pc.power) // 전원이 꺼져있는 경우 power의 체크박스를 활성화하여 전원을 켜주어야한다.
                {
                    MessageBox.Show("전원을 켜주세요.");
                    return;
                }

                if (pc.inUse) // 이미 사용중인 PC일 경우 안내문구 출력
                {
                    MessageBox.Show("다른 PC를 선택해주세요.");
                    return;
                }

                if (textBox2.Text == "0") { // 회원 Id가 0으로 입력된 경우 비회원으로 시작
                    user = new User() { Id = 0, Name = "비회원", Charge = 0 };
                    PCRoomControl.sales += cost;
                    sales_label.Text = PCRoomControl.sales.ToString();                                       
                }

                else {
                    user = PCRoomControl.Users.Single((x) => x.Id.ToString() == textBox2.Text);
                }                               

                if (user.Id != 0 && user.Charge < cost) { // 회원의 경우 잔액이 부족하면 충전 안내문구 출력
                    MessageBox.Show("요금을 충전해주세요.");
                    return;
                }

                pc.UserId = user.Id;
                pc.UserName = user.Name;
                pc.inUse = true;
                pc.Payment = "선불";
                pc.ChargeTime = time * 3600;

                if (user.Id != 0) {
                    user.Charge -= cost;
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = PCRoomControl.PCs;

                dataGridView3.DataSource = null;
                dataGridView3.DataSource = PCRoomControl.Users;

                sales_label.Text = PCRoomControl.sales.ToString(); 

                PCRoomControl.Save();

            }

        }

        private void button3_Click(object sender, EventArgs e) // 사용 종료
        {

            if (textBox1.Text == "") // PC가 선택되지 않은 경우 안내 문구 출력
                MessageBox.Show("PC를 선택해주세요.");
            else 
            {
                PC pc = PCRoomControl.PCs.Single((x) => x.Id.ToString() == textBox1.Text);

                if (!pc.inUse) // 사용중인 PC가 아닐 경우 안내 문구 출력
                {
                    MessageBox.Show("사용중인 PC가 아닙니다.");
                    return;
                }

                pc.power = false;
                pc.inUse = false;
                pc.Payment = "";
                pc.UserId = 0;
                pc.UserName = "";
                pc.ChargeTime = 0;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = PCRoomControl.PCs;

                PCRoomControl.Save();

            }
        }

        private void button5_Click(object sender, EventArgs e) // 회원 Id를 0으로 설정해 비회원으로 시작할 수 있도록 함.
        {
            textBox2.Text = "0";
        }

        private void timer1_Tick(object sender, EventArgs e) // 타이머가 1초마다 수행하는 작업
        {
            foreach (PC pc in PCRoomControl.PCs) { // List의 모든 PC를 각각 제어

                if (pc.ChargeTime > 0) // 잔여 시간이 있는 경우, 즉 PC가 사용중인 경우 1초마다 시간이 차감됨.
                {
                    pc.ChargeTime -= 1;

                    if (pc.ChargeTime == 0) // 잔여 시간이 0이 되면 사용 종료 처리
                    {
                        pc.power = false;
                        pc.inUse = false;
                        pc.Payment = "";
                        pc.UserId = 0;
                        pc.UserName = "";
                        pc.ChargeTime = 0;

                        PCRoomControl.Save();

                    }

                }               

            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = PCRoomControl.PCs;
            
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) // dataGridView1의 ChargeTime 부분을 시간 형식으로 출력
        {
            if (e.RowIndex != this.dataGridView1.NewRowIndex) {
                if (e.ColumnIndex == 6) {
                    int time = Int32.Parse(e.Value.ToString());
                    string timeToString;

                    timeToString = string.Format("{0:D2}:{1:D2}:{2:D2}", time / 3600, (time % 3600 / 60), (time % 3600 % 60));

                    try
                    {
                        if (timeToString == "00:00:00") {
                            e.Value = "";
                        }
                        else
                            e.Value = timeToString;
                    }
                    catch 
                    {
                        e.Value = time;
                    }
                }
            }
        }
    }   
}
