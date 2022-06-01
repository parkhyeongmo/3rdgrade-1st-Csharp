using _17011492;
using _17011492_Project;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Text = "회원 관리";

            dataGridView1.ReadOnly = true; // 회원 정보를 dataGridView 폼에서 직접 수정이 불가능하도록 설정

            dataGridView1.DataSource = PCRoomControl.Users; // 회원 List 정보 출력
            dataGridView1.CurrentCellChanged += DataGridView1_CurrentCellChanged;

        }

        private void DataGridView1_CurrentCellChanged(object sender, EventArgs e) // 회원 정보를 선택할 경우 textBox에 입력
        {
            try
            {
                User selected_user;
                selected_user = dataGridView1.CurrentRow.DataBoundItem as User;
                textBox1.Text = selected_user.Id.ToString();
                textBox2.Text = selected_user.Name;
                textBox3.Text = selected_user.Charge.ToString();
            }

            catch (Exception exception) { 
            }
        }

        private void button3_Click(object sender, EventArgs e) // 요금 충전 버튼 메소드
        {

            if (textBox3.Text == "") // 선택된 회원이 없을 경우 안내문구 출력
            {
                MessageBox.Show("회원을 선택해주세요.");
            }

            else if (textBox4.Text == "") // 충전할 금액이 입력되지 않은 경우 안내문구 출력
            {
                MessageBox.Show("충전할 금액을 입력해주세요.");
            }

            else
            {
                try // 충전한 금액만큼 회원의 잔액을 증가시키고 매출액도 증가시켜준다.
                {
                    User user = PCRoomControl.Users.Single((x) => x.Id.ToString() == textBox1.Text);
                    user.Charge += int.Parse(textBox4.Text);
                    PCRoomControl.sales += int.Parse(textBox4.Text);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = PCRoomControl.Users;
                    
                    PCRoomControl.Save();
                }

                catch (Exception exception) // 예외처리 안내문구 출력
                {
                    MessageBox.Show("선택된 회원을 확인해주세요.\n충전할 금액은 정수로 입력해주세요.");
                }

            }
        }

        private void button1_Click(object sender, EventArgs e) // 회원 등록 버튼 메소드
        {
            if (PCRoomControl.Users.Exists((x) => x.Id == int.Parse(textBox1.Text))) // 회원 등록 시 이미 사용중인 Id일 경우 안내문구 출력
            {
                MessageBox.Show("이미 사용중인 ID입니다.");
            }

            else 
            {
                User user = new User() { // 입력 받은 정보로 회원 객체 생성
                    Id = int.Parse(textBox1.Text),
                    Name = textBox2.Text,
                    Charge = 0
                };

                PCRoomControl.Users.Add(user); // 회원 List에 추가

                MessageBox.Show("가입이 완료되었습니다."); // 안내문구 출력

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = PCRoomControl.Users;

                PCRoomControl.Save();
            }
        }

        private void button4_Click(object sender, EventArgs e) // 회원 탈퇴 버튼 메소드
        {
            if (PCRoomControl.Users.Exists((x) => x.Id == int.Parse(textBox1.Text)))
            {
                User user = PCRoomControl.Users.Single((x) => x.Id == int.Parse(textBox1.Text)); // 탈퇴할 회원의 객체 저장
                PCRoomControl.Users.Remove(user); // 회원 List에서 제거

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = PCRoomControl.Users;

                MessageBox.Show("탈퇴가 완료되었습니다."); // 안내문구 출력

                PCRoomControl.Save();
            }

            else { // 예외처리 안내문구 출력
                MessageBox.Show("존재하지 않는 회원입니다.");
            }

        }
    }
}
