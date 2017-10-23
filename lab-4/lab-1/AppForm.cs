using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace COM
{
    // Contains main logic of form
    public partial class AppForm : Form
    {
        private Thread receiverThread;

        private Thread senderThread;

        private bool isReceiverThreadActivated;

        private bool isSenderThreadActivated;

        private Sender sender;

        private Receiver receiver;

        private readonly static string THREAD_IS_ALREADY_ACTIVE_MESSAGE 
            = "Wait... previous sending hasn't completed";
        private readonly static int MAX_SEND_ATTEMPT_COUNT = 10;
        private readonly static char JAM = 'Z';
        private readonly static string COLLISION_SIGN = "X";

        public AppForm()
        {
            InitializeComponent();

            // get serial port connection api
            SerialPortConnectionApi connectionApi = SerialPortConnectionApi.GetInstance();

            // initialize receiver and sender objects
            this.receiver = new Receiver(connectionApi);
            this.sender = new Sender(connectionApi);

            receiverThread = new Thread(ReceiverThreadProcedure);
            isReceiverThreadActivated = true;
            isSenderThreadActivated = false;
            receiverThread.Start();
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control receiverTextBox.
        delegate void AddTextToReceiverTextBoxCallback(string text);

        // This method demonstrates a pattern for making thread-safe
        // calls on a Windows Forms control. 
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly. 
        private void AddTextToReceiverTextBox(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.receiverTextBox.InvokeRequired)
            {
                AddTextToReceiverTextBoxCallback d 
                    = new AddTextToReceiverTextBoxCallback(AddTextToReceiverTextBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.receiverTextBox.Text += text;
            }
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control collisionTextBox.
        delegate void AddTextToCollisionTextBoxCallback(string text);

        private void AddTextToCollisionTextBox(string text)
        {
            if (this.receiverTextBox.InvokeRequired)
            {
                AddTextToCollisionTextBoxCallback d
                    = new AddTextToCollisionTextBoxCallback(AddTextToCollisionTextBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.collisionTextBox.Text += text;
            }
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control receiverTextBox.
        delegate void CleanSenderTextBoxCallback();

        private void CleanSenderTextBox()
        {
            if (this.senderTextBox.InvokeRequired)
            {
                CleanSenderTextBoxCallback d
                    = new CleanSenderTextBoxCallback(CleanSenderTextBox);
                this.Invoke(d, new object[] {});
            }
            else
            {
                this.senderTextBox.Text = "";
            }
        }

        private void ReceiverThreadProcedure()
        {
            while (isReceiverThreadActivated)
            {
                string data = receiver.Receive();
                AddTextToReceiverTextBox(data);
            }
        }

        private void SenderThreadProcedure()
        {
            string data = senderTextBox.Text;
            CleanSenderTextBox();

            handleSendingWithCsmaCdAlgorithm(data);
            isSenderThreadActivated = false;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (isSenderThreadActivated)
            {
                MessageBox.Show(THREAD_IS_ALREADY_ACTIVE_MESSAGE);
            }
            else
            {
                isSenderThreadActivated = true;
                this.senderThread = new Thread(SenderThreadProcedure);
                this.senderThread.Start();
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.receiverTextBox.Text = "";
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (isReceiverThreadActivated)
            {
                receiverThread.Abort();
            }
            if (isSenderThreadActivated)
            {
                senderThread.Abort();
            }
            
            System.Windows.Forms.Application.Exit();
        }

        private void ReceptionSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(isSenderThreadActivated)
            {
                MessageBox.Show(THREAD_IS_ALREADY_ACTIVE_MESSAGE);
                return;
            }

            Int32 receptionSpeed = Convert.ToInt32(receptionSpeedComboBox.SelectedItem);
            receiver.SetReceptionSpeed(receptionSpeed);
        }
    
        private void BaudRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSenderThreadActivated)
            {
                MessageBox.Show(THREAD_IS_ALREADY_ACTIVE_MESSAGE);
                return;
            }

            Int32 baudRate = Convert.ToInt32(baudRateComboBox.SelectedItem);
            this.sender.SetBaudRate(baudRate);
        }

        private void handleSendingWithCsmaCdAlgorithm(string data)
        {
            char[] message = data.ToCharArray();
            // listen to a channel
            int numOfAttempt = 0;
            DateTime dateTime;
            int delay;
            for (int i = 0; i < message.Length; i++)
            {
                numOfAttempt = 0;
                while (numOfAttempt < MAX_SEND_ATTEMPT_COUNT)
                {
                    dateTime = DateTime.Now;
                    if (dateTime.Second % 2 != 0)
                    {
                        sender.Send(message[i]);
                        Thread.Sleep(200);
                        dateTime = DateTime.Now;
                        // if current second is even, then create random delay
                        if (dateTime.Second % 2 == 0)
                        {
                            // send jam signal after send data to identify bad frame
                            sender.Send(JAM);
                            delay = new Random().Next(0, (int)Math.Pow(2, MAX_SEND_ATTEMPT_COUNT));
                            numOfAttempt++;
                            // send to the debug window sign of collision
                            AddTextToCollisionTextBox(COLLISION_SIGN);
                            Thread.Sleep(delay);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
