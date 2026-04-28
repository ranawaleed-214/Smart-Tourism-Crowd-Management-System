using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TourEgypt.Models;
using TourEgypt.Services;

namespace TourEgypt
{
    public partial class MainForm : Form
    {
        private UserManager _userManager;
        private BookingManager _bookingManager;
        private PlaceList _placeList;
        private IntelligenceEngine _intelligence;
        private User _currentUser;
        private Queue _currentQueue;

        private Panel pnlLogin;
        private Panel pnlRegister;
        private Panel pnlDashboard;

        public MainForm()
        {
            this.Text = "TourEgypt - Smart Tourism System";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 9.5f);

            LoadServices();
            BuildLoginPanel();
            BuildRegisterPanel();
            BuildDashboard();
            ShowPanel(pnlLogin);
        }

        private void LoadServices()
        {
            _placeList = DataSeeder.SeedPlaces();
            _userManager = DataSeeder.SeedUsers();
            _bookingManager = new BookingManager();
            _intelligence = new IntelligenceEngine(_placeList);
            _currentQueue = new Queue(1, 1, 5);
        }

        // LOGIN 
        private void BuildLoginPanel()
        {
            pnlLogin = new Panel();
            pnlLogin.Dock = DockStyle.Fill;
            pnlLogin.BackColor = Color.FromArgb(245, 245, 250);

            Panel card = new Panel();
            card.Size = new Size(420, 500);
            card.BackColor = Color.White;
            card.Location = new Point(340, 100);
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label();
            lblTitle.Text = "TourEgypt";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 90, 180);
            lblTitle.Location = new Point(30, 30);
            lblTitle.AutoSize = true;

            Label lblSub = new Label();
            lblSub.Text = "Smart Crowd Management System";
            lblSub.Font = new Font("Segoe UI", 10);
            lblSub.ForeColor = Color.Gray;
            lblSub.Location = new Point(30, 80);
            lblSub.AutoSize = true;

            Label lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Location = new Point(30, 130);
            lblEmail.AutoSize = true;

            TextBox txtEmail = new TextBox();
            txtEmail.Location = new Point(30, 153);
            txtEmail.Size = new Size(360, 28);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;

            Label lblPass = new Label();
            lblPass.Text = "Password";
            lblPass.Location = new Point(30, 195);
            lblPass.AutoSize = true;

            TextBox txtPass = new TextBox();
            txtPass.Location = new Point(30, 218);
            txtPass.Size = new Size(360, 28);
            txtPass.PasswordChar = '*';
            txtPass.BorderStyle = BorderStyle.FixedSingle;

            Label lblMsg = new Label();
            lblMsg.Location = new Point(30, 258);
            lblMsg.Size = new Size(360, 25);
            lblMsg.ForeColor = Color.Red;

            Button btnLogin = MakeButton("Login", new Point(30, 290), Color.FromArgb(30, 90, 180));
            btnLogin.Size = new Size(360, 42);
            btnLogin.Click += delegate
            {
                try
                {
                    _currentUser = _userManager.Login(txtEmail.Text.Trim(), txtPass.Text.Trim());
                    txtEmail.Clear();
                    txtPass.Clear();
                    lblMsg.Text = "";
                    ShowDashboard();
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error: " + ex.Message;
                }
            };

            LinkLabel lnkRegister = new LinkLabel();
            lnkRegister.Text = "Don't have an account? Register here";
            lnkRegister.Location = new Point(80, 348);
            lnkRegister.AutoSize = true;
            lnkRegister.LinkColor = Color.FromArgb(30, 90, 180);
            lnkRegister.Click += delegate { ShowPanel(pnlRegister); };

            Label lblHint = new Label();
            lblHint.Text = "Demo accounts:\nTourist: alice@email.com / pass123\nAdmin:   admin@email.com / admin123";
            lblHint.Location = new Point(30, 385);
            lblHint.Size = new Size(360, 65);
            lblHint.ForeColor = Color.Gray;
            lblHint.Font = new Font("Segoe UI", 8.5f);

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblSub);
            card.Controls.Add(lblEmail);
            card.Controls.Add(txtEmail);
            card.Controls.Add(lblPass);
            card.Controls.Add(txtPass);
            card.Controls.Add(lblMsg);
            card.Controls.Add(btnLogin);
            card.Controls.Add(lnkRegister);
            card.Controls.Add(lblHint);

            pnlLogin.Controls.Add(card);
            this.Controls.Add(pnlLogin);
        }

        //  REGISTER 
        private void BuildRegisterPanel()
        {
            pnlRegister = new Panel();
            pnlRegister.Dock = DockStyle.Fill;
            pnlRegister.BackColor = Color.FromArgb(245, 245, 250);

            Panel card = new Panel();
            card.Size = new Size(450, 420);
            card.BackColor = Color.White;
            card.Location = new Point(320, 120);
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label();
            lblTitle.Text = "Create Account";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 90, 180);
            lblTitle.Location = new Point(30, 20);
            lblTitle.AutoSize = true;

            Label lblName = new Label(); lblName.Text = "Full Name"; lblName.Location = new Point(30, 70); lblName.AutoSize = true;
            TextBox txtName = new TextBox(); txtName.Location = new Point(30, 93); txtName.Size = new Size(390, 28); txtName.BorderStyle = BorderStyle.FixedSingle;

            Label lblEmail = new Label(); lblEmail.Text = "Email"; lblEmail.Location = new Point(30, 133); lblEmail.AutoSize = true;
            TextBox txtEmail = new TextBox(); txtEmail.Location = new Point(30, 156); txtEmail.Size = new Size(390, 28); txtEmail.BorderStyle = BorderStyle.FixedSingle;

            Label lblPass = new Label(); lblPass.Text = "Password"; lblPass.Location = new Point(30, 196); lblPass.AutoSize = true;
            TextBox txtPass = new TextBox(); txtPass.Location = new Point(30, 219); txtPass.Size = new Size(390, 28); txtPass.PasswordChar = '*'; txtPass.BorderStyle = BorderStyle.FixedSingle;

            Label lblMsg = new Label(); lblMsg.Location = new Point(30, 258); lblMsg.Size = new Size(390, 25); lblMsg.ForeColor = Color.Red;

            Button btnReg = MakeButton("Register as Tourist", new Point(30, 285), Color.FromArgb(30, 130, 80));
            btnReg.Size = new Size(390, 42);
            btnReg.Click += delegate
            {
                try
                {
                    _currentUser = _userManager.RegisterTourist(txtName.Text.Trim(), txtEmail.Text.Trim(), txtPass.Text.Trim());
                    ShowDashboard();
                }
                catch (Exception ex) { lblMsg.Text = "Error: " + ex.Message; }
            };

            LinkLabel lnkBack = new LinkLabel(); lnkBack.Text = "Back to Login"; lnkBack.Location = new Point(30, 345); lnkBack.AutoSize = true;
            lnkBack.Click += delegate { ShowPanel(pnlLogin); };

            card.Controls.Add(lblTitle); card.Controls.Add(lblName); card.Controls.Add(txtName);
            card.Controls.Add(lblEmail); card.Controls.Add(txtEmail);
            card.Controls.Add(lblPass); card.Controls.Add(txtPass);
            card.Controls.Add(lblMsg); card.Controls.Add(btnReg); card.Controls.Add(lnkBack);
            pnlRegister.Controls.Add(card);
            this.Controls.Add(pnlRegister);
        }

        //  DASHBOARD 
        private void BuildDashboard()
        {
            pnlDashboard = new Panel();
            pnlDashboard.Dock = DockStyle.Fill;
            pnlDashboard.BackColor = Color.FromArgb(245, 245, 250);

            // Header
            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 58;
            header.BackColor = Color.FromArgb(20, 70, 160);

            Label lblHeader = new Label();
            lblHeader.Name = "lblHeader";
            lblHeader.Text = "TourEgypt";
            lblHeader.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(20, 14);
            lblHeader.AutoSize = true;

            Button btnLogout = MakeButton("Logout", new Point(980, 12), Color.FromArgb(200, 60, 60));
            btnLogout.Size = new Size(90, 34);
            btnLogout.Click += delegate
            {
                _userManager.Logout();
                _currentUser = null;
                ShowPanel(pnlLogin);
            };

            header.Controls.Add(lblHeader);
            header.Controls.Add(btnLogout);

            // Tabs
            TabControl tabs = new TabControl();
            tabs.Location = new Point(10, 68);
            tabs.Size = new Size(1070, 650);
            tabs.Font = new Font("Segoe UI", 10);

            tabs.TabPages.Add(BuildCrowdTab());
            tabs.TabPages.Add(BuildQueueTab());
            tabs.TabPages.Add(BuildTripTab());
            tabs.TabPages.Add(BuildBookingTab());
            tabs.TabPages.Add(BuildProfileTab());
            tabs.TabPages.Add(BuildAdminTab());

            pnlDashboard.Controls.Add(header);
            pnlDashboard.Controls.Add(tabs);
            this.Controls.Add(pnlDashboard);
        }

        //  TAB: CROWD
        private TabPage BuildCrowdTab()
        {
            TabPage tab = new TabPage("  Crowd Monitor  ");
            tab.BackColor = Color.FromArgb(248, 248, 252);

            Label lblTitle = new Label();
            lblTitle.Text = "Live Crowd Levels & Waiting Times";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 70, 160);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            DataGridView dgv = new DataGridView();
            dgv.Location = new Point(15, 50);
            dgv.Size = new Size(740, 320);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Columns.Add("Name", "Place");
            dgv.Columns.Add("Location", "Location");
            dgv.Columns.Add("Crowd", "Crowd Level");
            dgv.Columns.Add("People", "People Inside");
            dgv.Columns.Add("Wait", "Est. Wait (min)");
            dgv.Columns.Add("Rating", "Rating");

            Button btnRefresh = MakeButton("Refresh", new Point(15, 385), Color.FromArgb(30, 90, 180));
            btnRefresh.Click += delegate { RefreshCrowdGrid(dgv); };

            // What-If section
            Label lblWhatIf = new Label();
            lblWhatIf.Text = "What-If Simulator";
            lblWhatIf.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblWhatIf.ForeColor = Color.FromArgb(120, 60, 180);
            lblWhatIf.Location = new Point(775, 50);
            lblWhatIf.AutoSize = true;

            Label lblPlaceLbl = new Label(); lblPlaceLbl.Text = "Select Place:"; lblPlaceLbl.Location = new Point(775, 85); lblPlaceLbl.AutoSize = true;
            ComboBox cmbPlace = new ComboBox();
            cmbPlace.Location = new Point(775, 108);
            cmbPlace.Size = new Size(280, 28);
            cmbPlace.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var p in _placeList.GetAll()) cmbPlace.Items.Add(p.Name);
            if (cmbPlace.Items.Count > 0) cmbPlace.SelectedIndex = 0;

            Label lblHourLbl = new Label(); lblHourLbl.Text = "Select Hour:"; lblHourLbl.Location = new Point(775, 148); lblHourLbl.AutoSize = true;
            ComboBox cmbHour = new ComboBox();
            cmbHour.Location = new Point(775, 171);
            cmbHour.Size = new Size(120, 28);
            cmbHour.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int h = 8; h <= 18; h++) cmbHour.Items.Add(h + ":00");
            cmbHour.SelectedIndex = 0;

            RichTextBox rtbResult = new RichTextBox();
            rtbResult.Location = new Point(775, 215);
            rtbResult.Size = new Size(280, 180);
            rtbResult.ReadOnly = true;
            rtbResult.BackColor = Color.FromArgb(245, 245, 255);
            rtbResult.BorderStyle = BorderStyle.FixedSingle;

            Button btnSimulate = MakeButton("Simulate", new Point(775, 408), Color.FromArgb(120, 60, 180));
            btnSimulate.Size = new Size(280, 38);
            btnSimulate.Click += delegate
            {
                if (cmbPlace.SelectedIndex < 0) return;
                var place = _placeList.GetAll()[cmbPlace.SelectedIndex];
                int hour = 8 + cmbHour.SelectedIndex;
                string result = _intelligence.WhatIfSimulate(place.PlaceId, hour);
                rtbResult.Clear();
                rtbResult.AppendText("What-If for: " + place.Name + "\n\n");
                rtbResult.AppendText(result);
            };

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(dgv);
            tab.Controls.Add(btnRefresh);
            tab.Controls.Add(lblWhatIf);
            tab.Controls.Add(lblPlaceLbl);
            tab.Controls.Add(cmbPlace);
            tab.Controls.Add(lblHourLbl);
            tab.Controls.Add(cmbHour);
            tab.Controls.Add(rtbResult);
            tab.Controls.Add(btnSimulate);

            RefreshCrowdGrid(dgv);
            return tab;
        }

        private void RefreshCrowdGrid(DataGridView dgv)
        {
            dgv.Rows.Clear();
            foreach (var p in _placeList.GetAll())
            {
                int rowIdx = dgv.Rows.Add();
                dgv.Rows[rowIdx].Cells["Name"].Value = p.Name;
                dgv.Rows[rowIdx].Cells["Location"].Value = p.Location;
                dgv.Rows[rowIdx].Cells["Crowd"].Value = p.Counter.CrowdLevel.ToString();
                dgv.Rows[rowIdx].Cells["People"].Value = p.Counter.CurrentPeople;
                dgv.Rows[rowIdx].Cells["Wait"].Value = p.GetEstimatedWaitingTime();
                dgv.Rows[rowIdx].Cells["Rating"].Value = p.GetAverageRating();

                Color rowColor;
                if (p.Counter.CrowdLevel == CrowdLevel.Low) rowColor = Color.FromArgb(220, 255, 220);
                else if (p.Counter.CrowdLevel == CrowdLevel.Medium) rowColor = Color.FromArgb(255, 255, 210);
                else if (p.Counter.CrowdLevel == CrowdLevel.High) rowColor = Color.FromArgb(255, 225, 200);
                else rowColor = Color.FromArgb(255, 200, 200);

                dgv.Rows[rowIdx].DefaultCellStyle.BackColor = rowColor;
            }
        }

        // TAB: QUEUE 
        private TabPage BuildQueueTab()
        {
            TabPage tab = new TabPage("  Queue System  ");
            tab.BackColor = Color.FromArgb(248, 248, 252);

            Label lblTitle = new Label();
            lblTitle.Text = "Join & Manage Queue";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 70, 160);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            Label lblPlace = new Label(); lblPlace.Text = "Select Place:"; lblPlace.Location = new Point(15, 55); lblPlace.AutoSize = true;
            ComboBox cmbPlace = new ComboBox();
            cmbPlace.Location = new Point(15, 78);
            cmbPlace.Size = new Size(280, 28);
            cmbPlace.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var p in _placeList.GetAll()) cmbPlace.Items.Add(p.PlaceId + ". " + p.Name);
            if (cmbPlace.Items.Count > 0) cmbPlace.SelectedIndex = 0;

            Label lblInfo = new Label();
            lblInfo.Location = new Point(15, 115);
            lblInfo.Size = new Size(350, 40);
            lblInfo.ForeColor = Color.FromArgb(80, 80, 120);

            cmbPlace.SelectedIndexChanged += delegate
            {
                var place = _placeList.GetAll()[cmbPlace.SelectedIndex];
                lblInfo.Text = "Current wait: ~" + place.GetEstimatedWaitingTime() + " min | Crowd: " + place.Counter.CrowdLevel;
            };

            if (_placeList.GetAll().Length > 0)
            {
                var p0 = _placeList.GetAll()[0];
                lblInfo.Text = "Current wait: ~" + p0.GetEstimatedWaitingTime() + " min | Crowd: " + p0.Counter.CrowdLevel;
            }

            ListBox lstQueue = new ListBox();
            lstQueue.Location = new Point(15, 210);
            lstQueue.Size = new Size(600, 330);
            lstQueue.Font = new Font("Courier New", 9);
            lstQueue.BackColor = Color.White;

            Button btnJoin = MakeButton("Join Queue", new Point(15, 163), Color.FromArgb(30, 130, 80));
            btnJoin.Click += delegate
            {
                if (_currentUser == null) { MessageBox.Show("Please login first."); return; }
                var place = _placeList.GetAll()[cmbPlace.SelectedIndex];
                _currentQueue.PlaceId = place.PlaceId;
                var ticket = _currentQueue.JoinQueue(_currentUser.Name, place.AverageServiceTime);
                FileHandler.SaveQueueEntry(ticket);
                RefreshQueueList(lstQueue);
                MessageBox.Show("Joined Queue!\nTicket: #" + ticket.TicketNumber +
                                "\nPosition: " + ticket.Position +
                                "\nEst. Time: " + ticket.EstTimeSlot.ToString("HH:mm"), "Queue Confirmation");
            };

            Button btnCallNext = MakeButton("Call Next", new Point(175, 163), Color.FromArgb(180, 100, 20));
            btnCallNext.Click += delegate
            {
                var ticket = _currentQueue.DequeueNext();
                if (ticket != null)
                    MessageBox.Show("Now serving: " + ticket.TouristName + "\nTicket #" + ticket.TicketNumber, "Next in Queue");
                else
                    MessageBox.Show("Queue is empty.");
                RefreshQueueList(lstQueue);
            };

            Label lblQueueTitle = new Label();
            lblQueueTitle.Text = "Current Queue:";
            lblQueueTitle.Location = new Point(15, 190);
            lblQueueTitle.AutoSize = true;
            lblQueueTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(lblPlace);
            tab.Controls.Add(cmbPlace);
            tab.Controls.Add(lblInfo);
            tab.Controls.Add(btnJoin);
            tab.Controls.Add(btnCallNext);
            tab.Controls.Add(lblQueueTitle);
            tab.Controls.Add(lstQueue);

            RefreshQueueList(lstQueue);
            return tab;
        }

        private void RefreshQueueList(ListBox lst)
        {
            lst.Items.Clear();
            lst.Items.Add("Waiting in queue: " + _currentQueue.WaitingCount + " person(s)");
            foreach (var t in _currentQueue.GetWaitingList())
                lst.Items.Add(t.ToString());
        }

        //  TAB: TRIP PLANNER 
        private TabPage BuildTripTab()
        {
            TabPage tab = new TabPage("  Trip Planner  ");
            tab.BackColor = Color.FromArgb(248, 248, 252);

            Label lblTitle = new Label();
            lblTitle.Text = "Smart Trip Planner";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 70, 160);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            Label lblInstr = new Label(); lblInstr.Text = "Select places to visit:"; lblInstr.Location = new Point(15, 50); lblInstr.AutoSize = true;

            CheckedListBox clbPlaces = new CheckedListBox();
            clbPlaces.Location = new Point(15, 73);
            clbPlaces.Size = new Size(300, 180);
            clbPlaces.BorderStyle = BorderStyle.FixedSingle;
            foreach (var p in _placeList.GetAll()) clbPlaces.Items.Add(p.Name);

            Button btnPlan = MakeButton("Plan My Trip", new Point(15, 268), Color.FromArgb(30, 90, 180));
            btnPlan.Size = new Size(180, 40);

            RichTextBox rtbRoute = new RichTextBox();
            rtbRoute.Location = new Point(15, 325);
            rtbRoute.Size = new Size(660, 270);
            rtbRoute.ReadOnly = true;
            rtbRoute.BackColor = Color.White;
            rtbRoute.BorderStyle = BorderStyle.FixedSingle;
            rtbRoute.Font = new Font("Segoe UI", 10);

            // Forecast section
            Label lblBest = new Label();
            lblBest.Text = "Best Visiting Times:";
            lblBest.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblBest.Location = new Point(345, 50);
            lblBest.AutoSize = true;

            ComboBox cmbForPlace = new ComboBox();
            cmbForPlace.Location = new Point(345, 78);
            cmbForPlace.Size = new Size(250, 28);
            cmbForPlace.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var p in _placeList.GetAll()) cmbForPlace.Items.Add(p.Name);
            if (cmbForPlace.Items.Count > 0) cmbForPlace.SelectedIndex = 0;

            RichTextBox rtbForecast = new RichTextBox();
            rtbForecast.Location = new Point(345, 115);
            rtbForecast.Size = new Size(700, 140);
            rtbForecast.ReadOnly = true;
            rtbForecast.BackColor = Color.White;
            rtbForecast.Font = new Font("Segoe UI", 9.5f);

            Button btnForecast = MakeButton("Show Forecast", new Point(345, 268), Color.FromArgb(100, 60, 180));
            btnForecast.Click += delegate
            {
                if (cmbForPlace.SelectedIndex < 0) return;
                rtbForecast.Clear();
                var place = _placeList.GetAll()[cmbForPlace.SelectedIndex];
                rtbForecast.AppendText("Forecast for " + place.Name + ":\n\n");
                foreach (var kv in place.HourlyCrowdForecast)
                    rtbForecast.AppendText("  " + kv.Key + ":00  ->  " + kv.Value + "\n");
                rtbForecast.AppendText("\nBest time: " + place.GetBestVisitingTimeToday());
            };

            btnPlan.Click += delegate
            {
                rtbRoute.Clear();
                List<TouristAttraction> selected = new List<TouristAttraction>();
                foreach (var item in clbPlaces.CheckedItems)
                {
                    var matches = _placeList.GetByName(item.ToString());
                    if (matches.Length > 0) selected.Add(matches[0]);
                }
                if (selected.Count == 0) { rtbRoute.AppendText("Please select at least one place."); return; }

                TripPlanner planner = new TripPlanner(_currentUser != null ? _currentUser.UserId : 0, DateTime.Today);
                planner.PlanDayBasedOnCrowd(selected);

                rtbRoute.AppendText("RECOMMENDED ROUTE (sorted by crowd level)\n");
                rtbRoute.AppendText("==========================================\n\n");
                string[] labels = { "Morning", "Mid-Morning", "Afternoon", "Late Afternoon", "Evening" };
                for (int i = 0; i < planner.OrderedAttractionsList.Count; i++)
                {
                    var p = planner.OrderedAttractionsList[i];
                    string time = i < labels.Length ? labels[i] : "Stop " + (i + 1);
                    rtbRoute.AppendText((i + 1) + ". " + p.Name + "  (" + time + ")\n");
                    rtbRoute.AppendText("   Location : " + p.Location + "\n");
                    rtbRoute.AppendText("   Crowd    : " + p.Counter.CrowdLevel + "\n");
                    rtbRoute.AppendText("   Wait     : ~" + p.GetEstimatedWaitingTime() + " min\n\n");
                }

                // Alternative suggestion
                bool hasCrowded = false;
                TouristAttraction crowdedPlace = null;
                foreach (var p in selected)
                {
                    if (p.Counter.CrowdLevel == CrowdLevel.High || p.Counter.CrowdLevel == CrowdLevel.VeryHigh)
                    {
                        hasCrowded = true;
                        crowdedPlace = p;
                        break;
                    }
                }
                if (hasCrowded && crowdedPlace != null)
                {
                    var alt = _intelligence.SuggestAlternativePlaces(crowdedPlace.PlaceId);
                    if (alt != null)
                        rtbRoute.AppendText("Tip: " + crowdedPlace.Name + " is crowded -> Consider " + alt.Name + " (Crowd: " + alt.Counter.CrowdLevel + ")\n");
                }
            };

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(lblInstr);
            tab.Controls.Add(clbPlaces);
            tab.Controls.Add(btnPlan);
            tab.Controls.Add(rtbRoute);
            tab.Controls.Add(lblBest);
            tab.Controls.Add(cmbForPlace);
            tab.Controls.Add(rtbForecast);
            tab.Controls.Add(btnForecast);
            return tab;
        }

        // ===================== TAB: BOOKING =====================
        private TabPage BuildBookingTab()
        {
            TabPage tab = new TabPage("  Booking  ");
            tab.BackColor = Color.FromArgb(248, 248, 252);

            Label lblTitle = new Label();
            lblTitle.Text = "Premium Booking System";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 70, 160);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            RadioButton rdbTour = new RadioButton(); rdbTour.Text = "Book a Tour"; rdbTour.Location = new Point(15, 52); rdbTour.AutoSize = true; rdbTour.Checked = true;
            RadioButton rdbTicket = new RadioButton(); rdbTicket.Text = "Book Tickets"; rdbTicket.Location = new Point(160, 52); rdbTicket.AutoSize = true;
            RadioButton rdbGuide = new RadioButton(); rdbGuide.Text = "Book a Guide"; rdbGuide.Location = new Point(305, 52); rdbGuide.AutoSize = true;

            Label lblName = new Label(); lblName.Text = "Tour / Attraction / Guide Name:"; lblName.Location = new Point(15, 90); lblName.AutoSize = true;
            TextBox txtName = new TextBox(); txtName.Location = new Point(15, 113); txtName.Size = new Size(300, 28); txtName.BorderStyle = BorderStyle.FixedSingle;

            Label lblPrice = new Label(); lblPrice.Text = "Price ($):"; lblPrice.Location = new Point(15, 153); lblPrice.AutoSize = true;
            NumericUpDown nudPrice = new NumericUpDown(); nudPrice.Location = new Point(15, 176); nudPrice.Size = new Size(120, 28); nudPrice.Minimum = 1; nudPrice.Maximum = 10000; nudPrice.Value = 50;

            Label lblDur = new Label(); lblDur.Text = "Duration (hours):"; lblDur.Location = new Point(155, 153); lblDur.AutoSize = true;
            NumericUpDown nudDur = new NumericUpDown(); nudDur.Location = new Point(155, 176); nudDur.Size = new Size(80, 28); nudDur.Minimum = 1; nudDur.Maximum = 24; nudDur.Value = 4;

            Label lblDate = new Label(); lblDate.Text = "Date:"; lblDate.Location = new Point(15, 216); lblDate.AutoSize = true;
            DateTimePicker dtPicker = new DateTimePicker(); dtPicker.Location = new Point(15, 239); dtPicker.Size = new Size(200, 28); dtPicker.Format = DateTimePickerFormat.Short;

            Label lblLang = new Label(); lblLang.Text = "Language (for Guide):"; lblLang.Location = new Point(235, 216); lblLang.AutoSize = true;
            TextBox txtLang = new TextBox(); txtLang.Location = new Point(235, 239); txtLang.Size = new Size(150, 28); txtLang.Text = "English"; txtLang.BorderStyle = BorderStyle.FixedSingle;

            Button btnBook = MakeButton("Confirm Booking", new Point(15, 285), Color.FromArgb(30, 130, 80));
            btnBook.Size = new Size(200, 42);

            RichTextBox rtbBookings = new RichTextBox();
            rtbBookings.Location = new Point(15, 345);
            rtbBookings.Size = new Size(1020, 255);
            rtbBookings.ReadOnly = true;
            rtbBookings.BackColor = Color.White;
            rtbBookings.Font = new Font("Segoe UI", 9);

            btnBook.Click += delegate
            {
                if (_currentUser == null) { MessageBox.Show("Please login first."); return; }
                try
                {
                    Booking booking;
                    if (rdbTour.Checked)
                        booking = _bookingManager.BookTour(_currentUser.Name, dtPicker.Value, (double)nudPrice.Value, txtName.Text.Trim(), (int)nudDur.Value);
                    else if (rdbTicket.Checked)
                        booking = _bookingManager.BookTicket(_currentUser.Name, dtPicker.Value, (double)nudPrice.Value, txtName.Text.Trim(), 1);
                    else
                        booking = _bookingManager.BookGuide(_currentUser.Name, dtPicker.Value, (double)nudPrice.Value, txtName.Text.Trim(), txtLang.Text.Trim());

                    _bookingManager.ProcessPayment(booking, "Credit Card");

                    if (_currentUser is Tourist)
                        ((Tourist)_currentUser).Profile.AddPoints(50);

                    rtbBookings.Clear();
                    foreach (var b in _bookingManager.GetBookingsByTourist(_currentUser.Name))
                    {
                        rtbBookings.AppendText(b.ToString() + "\n");
                        rtbBookings.AppendText("   " + b.GetBookingDetails() + "\n\n");
                    }
                    MessageBox.Show("Booking confirmed!\nCommission: $" + booking.CalculateCommission().ToString("F2"), "Success");
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            };

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(rdbTour); tab.Controls.Add(rdbTicket); tab.Controls.Add(rdbGuide);
            tab.Controls.Add(lblName); tab.Controls.Add(txtName);
            tab.Controls.Add(lblPrice); tab.Controls.Add(nudPrice);
            tab.Controls.Add(lblDur); tab.Controls.Add(nudDur);
            tab.Controls.Add(lblDate); tab.Controls.Add(dtPicker);
            tab.Controls.Add(lblLang); tab.Controls.Add(txtLang);
            tab.Controls.Add(btnBook);
            tab.Controls.Add(rtbBookings);
            return tab;
        }

        // ===================== TAB: PROFILE =====================
        private TabPage BuildProfileTab()
        {
            TabPage tab = new TabPage("  My Profile  ");
            tab.BackColor = Color.FromArgb(248, 248, 252);

            Label lblTitle = new Label();
            lblTitle.Text = "Tourist Profile & Gamification";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 70, 160);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            RichTextBox rtbProfile = new RichTextBox();
            rtbProfile.Location = new Point(15, 50);
            rtbProfile.Size = new Size(680, 500);
            rtbProfile.ReadOnly = true;
            rtbProfile.BackColor = Color.White;
            rtbProfile.Font = new Font("Segoe UI", 10.5f);
            rtbProfile.BorderStyle = BorderStyle.FixedSingle;

            Button btnRefresh = MakeButton("Refresh Profile", new Point(15, 565), Color.FromArgb(30, 90, 180));
            btnRefresh.Click += delegate { LoadProfile(rtbProfile); };

            tab.Enter += delegate { LoadProfile(rtbProfile); };
            tab.Controls.Add(lblTitle);
            tab.Controls.Add(rtbProfile);
            tab.Controls.Add(btnRefresh);
            return tab;
        }

        private void LoadProfile(RichTextBox rtb)
        {
            rtb.Clear();
            if (_currentUser == null) { rtb.AppendText("Please login to view your profile."); return; }

            rtb.AppendText("Name  : " + _currentUser.Name + "\n");
            rtb.AppendText("Email : " + _currentUser.Email + "\n");
            rtb.AppendText("Role  : " + _currentUser.Role + "\n\n");

            if (_currentUser is Tourist)
            {
                Tourist t = (Tourist)_currentUser;
                TouristProfile p = t.Profile;
                rtb.AppendText("======= Gamification Stats =======\n\n");
                rtb.AppendText("Efficiency Score : " + p.EfficiencyScore + "%\n");
                rtb.AppendText("Time Saved       : " + p.TimeSaved + " minutes\n");
                rtb.AppendText("Points           : " + p.Points + "\n");
                rtb.AppendText("Badges           : " + (p.Badges.Count > 0 ? string.Join(", ", p.Badges) : "None yet") + "\n\n");

                rtb.AppendText("======= Wishlist =======\n");
                if (t.Wishlist.Count == 0) rtb.AppendText("Empty\n");
                else foreach (var w in t.Wishlist) rtb.AppendText("  - " + w + "\n");

                rtb.AppendText("\n======= My Bookings =======\n");
                List<Booking> bookings = _bookingManager.GetBookingsByTourist(t.Name);
                if (bookings.Count == 0) rtb.AppendText("No bookings yet.\n");
                else foreach (var b in bookings)
                    rtb.AppendText("  " + b + "\n  " + b.GetBookingDetails() + "\n\n");
            }
        }

        // ===================== TAB: ADMIN =====================
        private TabPage BuildAdminTab()
        {
            TabPage tab = new TabPage("  Admin  ");
            tab.BackColor = Color.FromArgb(248, 248, 252);

            Label lblTitle = new Label();
            lblTitle.Text = "Admin Dashboard";
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 70, 160);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            Label lblNote = new Label();
            lblNote.Text = "(Admin access only)";
            lblNote.ForeColor = Color.Red;
            lblNote.Location = new Point(230, 18);
            lblNote.AutoSize = true;

            RichTextBox rtbReport = new RichTextBox();
            rtbReport.Location = new Point(15, 55);
            rtbReport.Size = new Size(1020, 490);
            rtbReport.ReadOnly = true;
            rtbReport.BackColor = Color.White;
            rtbReport.Font = new Font("Segoe UI", 10);
            rtbReport.BorderStyle = BorderStyle.FixedSingle;

            Button btnReport = MakeButton("View Full Report", new Point(15, 560), Color.FromArgb(30, 90, 180));
            btnReport.Click += delegate
            {
                if (!(_currentUser is Admin)) { MessageBox.Show("Admin access required."); return; }
                rtbReport.Clear();
                rtbReport.AppendText("============ ADMIN SYSTEM REPORT ============\n\n");
                foreach (var place in _placeList.GetAll())
                {
                    rtbReport.AppendText("Place        : " + place.Name + "\n");
                    rtbReport.AppendText("Location     : " + place.Location + "\n");
                    rtbReport.AppendText("Crowd Level  : " + place.Counter.CrowdLevel + "\n");
                    rtbReport.AppendText("People Inside: " + place.Counter.CurrentPeople + "\n");
                    rtbReport.AppendText("Est. Wait    : " + place.GetEstimatedWaitingTime() + " min\n");
                    rtbReport.AppendText("Avg Rating   : " + place.GetAverageRating() + "\n");
                    rtbReport.AppendText("---------------------------------------------\n");
                }
                rtbReport.AppendText("\nTotal Revenue   : $" + _bookingManager.GetTotalRevenue().ToString("F2") + "\n");
                rtbReport.AppendText("Total Commission: $" + _bookingManager.GetTotalCommission().ToString("F2") + "\n");
                rtbReport.AppendText("Total Users     : " + _userManager.GetAllUsers().Count + "\n");
                FileHandler.LogActivity("Admin viewed full report.");
            };

            Button btnLog = MakeButton("View Logs", new Point(175, 560), Color.FromArgb(100, 100, 100));
            btnLog.Click += delegate
            {
                if (!(_currentUser is Admin)) { MessageBox.Show("Admin access required."); return; }
                rtbReport.Clear();
                rtbReport.AppendText("============ ACTIVITY LOG ============\n\n");
                rtbReport.AppendText(FileHandler.ReadLog());
            };

            tab.Controls.Add(lblTitle);
            tab.Controls.Add(lblNote);
            tab.Controls.Add(rtbReport);
            tab.Controls.Add(btnReport);
            tab.Controls.Add(btnLog);
            return tab;
        }

        // ===================== HELPERS =====================
        private Button MakeButton(string text, Point location, Color backColor)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = location;
            btn.Size = new Size(145, 38);
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void ShowPanel(Panel panel)
        {
            pnlLogin.Visible = false;
            pnlRegister.Visible = false;
            pnlDashboard.Visible = false;
            panel.Visible = true;
            panel.BringToFront();
        }

        private void ShowDashboard()
        {
            ShowPanel(pnlDashboard);
            Panel header = (Panel)pnlDashboard.Controls[0];
            foreach (Control c in header.Controls)
            {
                if (c.Name == "lblHeader")
                {
                    c.Text = "TourEgypt  |  Welcome, " + (_currentUser != null ? _currentUser.Name : "");
                    break;
                }
            }
        }
    }
}
