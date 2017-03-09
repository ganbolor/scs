using System;
using System.Text;
using System.Windows;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using Hik.Samples.Scs.IrcChat.Contracts;
using DevExpress.Xpo;
using Hik.Samples.Scs.IrcChat.Database;
using Hik.Samples.Scs.IrcChat.Database.SolutionSafetyMain;
using DevExpress.Data.Filtering;
using System.Collections;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Generators;

namespace Hik.Samples.Scs.IrcChat.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This object is used to host Chat Service on a SCS server.
        /// </summary>
        private IScsServiceApplication _serviceApplication;

        /// <summary>
        /// Chat Service object that serves clients.
        /// </summary>
        private ChatService _chatService;
        Session session;
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            lblUsers.Text = "";
        }

        /// <summary>
        /// Handles Client event of 'Start Server' button.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            //Check Database connection

            session = XPOUtils.GetNewSession();

            //VisibleCount p = new VisibleCount(session);
            //p.Date = DateTime.Now;
            //p.ViewIP = myIP;
            //p.Department = GetDepartment(myIP);
            //p.FileName = filename;
            //p.FilePath = filepath;
            //p.IPAddressName = GetIPAddressName(myIP);
            //p.IPDescription = GetIPDescription(myIP);
            //// p.Save(); when working with a Session 
            //p.Save();


            //IPList myIPList1 = session1.FindObject<IPList>(new BinaryOperator("ClientIP", myIP));
            //if (myIPList1 != null)
            //{
            //    return myIPList1.Department;
            //}
            //else
            //{
            //    return null;
            //}

            //CriteriaOperator criteria = new BinaryOperator("Oid", new JoinOperand("P_SafeMySecuritySystemUser", null, Aggregate.Max, new OperandProperty("Oid")));


            // Үндсэн шалгалт
            //XPClassInfo productClass = session.GetClassInfo(typeof(P_SafeMainDepartment));
            //SortingCollection sortProps = new SortingCollection(null);
            //CollectionCriteriaPatcher patcher = new DevExpress.Xpo.Generators.CollectionCriteriaPatcher(false, session.TypesManager);
            //ICollection results = session.GetObjects(productClass, null, sortProps, 0, patcher, true);
            //foreach (P_SafeMainDepartment result in results)
            //{
            //    Console.WriteLine( result.Oid.ToString());
            //}

            CriteriaOperator criteria = null;
            P_SafeMainDepartment a = session.FindObject<P_SafeMainDepartment>(criteria);

            //P_SafeMainDepartment myIPList1 = session.FindObject<P_SafeMainDepartment>(new BinaryOperator("Oid", 1));
            
            
            if (a != null)
            {
                Console.WriteLine("Test1");
            }
            else
            {
                Console.WriteLine("Test2");
            }


            //Get TCP port number from textbox
            int port;
            try
            {
                port = Convert.ToInt32(txtPort.Text);
                if (port <= 0 || port > 65536)
                {
                    throw new Exception(port + " is not a valid TCP port number.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TCP port must be a positive number. Exception detail: " + ex.Message);
                return;
            }

            try
            {
                _serviceApplication = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(port));
                _chatService = new ChatService();
                _chatService.session = session;
                _serviceApplication.AddService<IChatService, ChatService>(_chatService);
                _chatService.UserListChanged += chatService_UserListChanged;
                _serviceApplication.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Service can not be started. Exception detail: " + ex.Message);
                return;
            }

            btnStartServer.IsEnabled = false;
            btnStopServer.IsEnabled = true;
            txtPort.IsEnabled = false;
        }

        private void chatService_UserListChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(UpdateUserList));
        }

        /// <summary>
        /// Handles Client event of 'Stop Server' button.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {
            if (_serviceApplication == null)
            {
                return;
            }

            _serviceApplication.Stop();

            btnStartServer.IsEnabled = true;
            btnStopServer.IsEnabled = false;
            txtPort.IsEnabled = true;
        }

        /// <summary>
        /// Updates user list on GUI.
        /// </summary>
        private void UpdateUserList()
        {
            var users = new StringBuilder();
            foreach (var user in _chatService.UserList)
            {
                if (users.Length > 0)
                {
                    users.Append(", ");
                }

                users.Append(user.Nick);
            }

            lblUsers.Text = users.ToString();
        }

        #region Database

        public bool IsServerConnected()
        {
            //using (var l_oConnection = new SqlConnection(DBConnection.ConnectionString))
            //{
            //    try
            //    {
            //        l_oConnection.Open();
            //        return true;
            //    }
            //    catch (SqlException)
            //    {
            //        return false;
            //    }
            //}

            return false;
        }

        #endregion
    }
}
