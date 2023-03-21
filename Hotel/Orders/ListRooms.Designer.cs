namespace Hotel.Orders
{
    partial class ListRooms
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoomNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoomIsBusy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountRooms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(527, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 31);
            this.button1.TabIndex = 5;
            this.button1.Text = "Выбрать";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeight = 34;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.RoomNumber,
            this.RoomIsBusy,
            this.CountRooms,
            this.TypeRoom});
            this.dataGridView1.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridView1.Location = new System.Drawing.Point(0, 1);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(644, 407);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Visible = false;
            this.ID.Width = 5;
            // 
            // RoomNumber
            // 
            this.RoomNumber.HeaderText = "Номер номера";
            this.RoomNumber.Name = "RoomNumber";
            this.RoomNumber.ReadOnly = true;
            this.RoomNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RoomNumber.Width = 150;
            // 
            // RoomIsBusy
            // 
            this.RoomIsBusy.HeaderText = "Статус комнаты";
            this.RoomIsBusy.Name = "RoomIsBusy";
            this.RoomIsBusy.ReadOnly = true;
            this.RoomIsBusy.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RoomIsBusy.Width = 125;
            // 
            // CountRooms
            // 
            this.CountRooms.HeaderText = "Число комнат";
            this.CountRooms.Name = "CountRooms";
            this.CountRooms.ReadOnly = true;
            this.CountRooms.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CountRooms.Width = 125;
            // 
            // TypeRoom
            // 
            this.TypeRoom.HeaderText = "Тип комнаты";
            this.TypeRoom.Name = "TypeRoom";
            this.TypeRoom.ReadOnly = true;
            this.TypeRoom.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TypeRoom.Width = 200;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(0, 407);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(644, 44);
            this.panel1.TabIndex = 6;
            // 
            // ListRooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(644, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(660, 491);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(660, 491);
            this.Name = "ListRooms";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список комнат";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListRooms_FormClosing);
            this.Load += new System.EventHandler(this.ListRooms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoomNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoomIsBusy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountRooms;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeRoom;
        private System.Windows.Forms.Panel panel1;
    }
}