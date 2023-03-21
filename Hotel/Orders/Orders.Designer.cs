namespace Hotel.Orders
{
    partial class Orders
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID_order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNameClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateArrival = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateDeparture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNameEmployee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ListServicesRendered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Highlight;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(238, 615);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Highlight;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 23.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(-1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 58);
            this.label1.TabIndex = 8;
            this.label1.Text = "ЗАКАЗЫ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(11, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(216, 47);
            this.button2.TabIndex = 13;
            this.button2.Text = "ИЗМЕНИТЬ ЗАКАЗ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 47);
            this.button1.TabIndex = 12;
            this.button1.Text = "УДАЛИТЬ ЗАКАЗ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(11, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(216, 47);
            this.button4.TabIndex = 11;
            this.button4.Text = "ДОБАВИТЬ НОВЫЙ ЗАКАЗ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 36;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_order,
            this.FullNameClient,
            this.DateArrival,
            this.DateDeparture,
            this.NumRoom,
            this.FullNameEmployee,
            this.ListServicesRendered});
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView1.Location = new System.Drawing.Point(238, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(762, 615);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ID_order
            // 
            this.ID_order.HeaderText = "ID";
            this.ID_order.Name = "ID_order";
            this.ID_order.ReadOnly = true;
            this.ID_order.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID_order.Visible = false;
            // 
            // FullNameClient
            // 
            this.FullNameClient.HeaderText = "ФИО клиента";
            this.FullNameClient.Name = "FullNameClient";
            this.FullNameClient.ReadOnly = true;
            this.FullNameClient.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FullNameClient.Width = 185;
            // 
            // DateArrival
            // 
            this.DateArrival.HeaderText = "Дата заселения";
            this.DateArrival.Name = "DateArrival";
            this.DateArrival.ReadOnly = true;
            this.DateArrival.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DateArrival.Width = 95;
            // 
            // DateDeparture
            // 
            this.DateDeparture.HeaderText = "Дата выселения";
            this.DateDeparture.Name = "DateDeparture";
            this.DateDeparture.ReadOnly = true;
            this.DateDeparture.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DateDeparture.Width = 95;
            // 
            // NumRoom
            // 
            this.NumRoom.HeaderText = "Номер комнаты";
            this.NumRoom.Name = "NumRoom";
            this.NumRoom.ReadOnly = true;
            this.NumRoom.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NumRoom.Width = 60;
            // 
            // FullNameEmployee
            // 
            this.FullNameEmployee.HeaderText = "ФИО сотрудника";
            this.FullNameEmployee.Name = "FullNameEmployee";
            this.FullNameEmployee.ReadOnly = true;
            this.FullNameEmployee.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FullNameEmployee.Width = 184;
            // 
            // ListServicesRendered
            // 
            this.ListServicesRendered.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ListServicesRendered.FillWeight = 1F;
            this.ListServicesRendered.HeaderText = "Список оказанных услуг";
            this.ListServicesRendered.Name = "ListServicesRendered";
            this.ListServicesRendered.ReadOnly = true;
            this.ListServicesRendered.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ListServicesRendered.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 505);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(216, 47);
            this.button3.TabIndex = 38;
            this.button3.Text = "ПОИСК";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Highlight;
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(12, 419);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 80);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Критерии поиска";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "Дата заселения";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 41);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(203, 20);
            this.dateTimePicker1.TabIndex = 33;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(11, 177);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(216, 47);
            this.button5.TabIndex = 41;
            this.button5.Text = "ОТЧЁТ О ЗАКАЗАХ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(11, 558);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(216, 47);
            this.button6.TabIndex = 42;
            this.button6.Text = "СБРОСИТЬ";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 61);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(8, 15, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(238, 352);
            this.flowLayoutPanel1.TabIndex = 43;
            // 
            // Orders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 615);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1016, 654);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1016, 654);
            this.Name = "Orders";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Orders_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_order;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNameClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateArrival;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateDeparture;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNameEmployee;
        private System.Windows.Forms.DataGridViewTextBoxColumn ListServicesRendered;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}