using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using FileUpload;
using Hotkeys;
using Hotkeys.Constants;
using Uppy.Properties;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Uppy
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MemberWindow : Window
    {
        public static System.Drawing.Point p1;
        public static System.Drawing.Point p2;
        private readonly Dictionary<Button, bool> hkButton_pressed = new Dictionary<Button, bool>();
        private readonly Dictionary<String, List<string>> registeredKeys = new Dictionary<string, List<string>>();
        public static String SAVEFOLDER;

        private GlobalHotKeys ghk;

        private String password;
        private ScreenShotHandler scrHandler;
        private String username;
        private HwndSource source;
        private IntPtr handle;

        public MemberWindow()
        {   }

        public MemberWindow(String username, String password)
        {
            //Settings.Default.Reset();
            

            InitializeComponent();
            this.username = username;
            this.password = password;


            //Properties.Settings.Default.Reset();
            label_username.Content = login(username, password);
            setupSaveFolder();

            //ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;

        }


        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            textBox_wm.Text = msg.message.ToString();
            if (msg.message == Constants.WM_HOTKEY_MSG_ID)
            {
                MessageBox.Show("PRESSED");
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            handle = new WindowInteropHelper(this).Handle;
            setupHotkeys();

            ghk.testReg(handle);
            ghk = new GlobalHotKeys(Constants.SHIFT, Keys.S, handle);
            ghk.Register();

            var helper = new WindowInteropHelper(this);
            source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }


        public void setupHotkeys()
        {
            String[] keys =
            {
                Settings.Default.button_hk1, Settings.Default.button_hk2,
                Settings.Default.button_hk3, Settings.Default.button_hk4,
                Settings.Default.button_hk5, Settings.Default.button_hk6
            };

            String[] mods =
            {
                Settings.Default.button_hk1_mod, Settings.Default.button_hk2_mod,
                Settings.Default.button_hk3_mod, Settings.Default.button_hk4_mod,
                Settings.Default.button_hk5_mod, Settings.Default.button_hk6_mod
            };


            if (Settings.Default.button_hk1 == "" && Settings.Default.button_hk1_mod == "")
            {
                button_hk1.Content = "Control+1";
                ghk = new GlobalHotKeys(Constants.CONTROL, Keys.D1, handle);
                ghk.Register();
                var list = new List<string>();
                list.Add("Control");
                list.Add("D1");
                registeredKeys.Add("button_hk1", list);
            }
            else
            {
                button_hk1.Content = mods[0] + "+" + keys[0];
            }
            if (Settings.Default.button_hk2 == "" && Settings.Default.button_hk2_mod == "")
            {
                button_hk2.Content = "Control+2";
                ghk = new GlobalHotKeys(Constants.CTRL, Keys.D2, handle);
                ghk.Register();
                var list = new List<string>();
                list.Add("Control");
                list.Add("D2");
                registeredKeys.Add("button_hk2", list);
            }
            else
            {
                button_hk2.Content = mods[1] + "+" + keys[1];
            }
            if (Settings.Default.button_hk3 == "" && Settings.Default.button_hk3_mod == "")
            {
                button_hk3.Content = "Control+3";
                ghk = new GlobalHotKeys(Constants.CTRL, Keys.D3, handle);
                ghk.Register();
                var list = new List<string>();
                list.Add("Control");
                list.Add("D3");
                registeredKeys.Add("button_hk3", list);
            }
            else
            {
                button_hk3.Content = mods[2] + "+" + keys[2];
            }
            if (Settings.Default.button_hk4 == "" && Settings.Default.button_hk4_mod == "")
            {
                button_hk4.Content = "Control+4";
                ghk = new GlobalHotKeys(Constants.CTRL, Keys.D4, handle);
                ghk.Register();
                var list = new List<string>();
                list.Add("Control");
                list.Add("D4");
                registeredKeys.Add("button_hk4", list);
            }
            else
            {
                button_hk4.Content = mods[3] + "+" + keys[3];
            }
            if (Settings.Default.button_hk5 == "" && Settings.Default.button_hk5_mod == "")
            {
                button_hk5.Content = "Control+5";
                ghk = new GlobalHotKeys(Constants.CTRL, Keys.D5, handle);
                ghk.Register();
                var list = new List<string>();
                list.Add("Control");
                list.Add("D5");
                registeredKeys.Add("button_hk5", list);
            }
            else
            {
                button_hk5.Content = mods[4] + "+" + keys[4];
            }
            if (Settings.Default.button_hk6 == "" && Settings.Default.button_hk6_mod == "")
            {
                button_hk6.Content = "Control+6";
                ghk = new GlobalHotKeys(Constants.CTRL, Keys.D6, handle);
                ghk.Register();
                var list = new List<string>();
                list.Add("Control");
                list.Add("D6");
                registeredKeys.Add("button_hk6", list);
            }
            else
            {
                button_hk6.Content = mods[5] + "+" + keys[5];
            }


            textBox1.Text += "Keys length = " + keys.Length + "\r\n";

            foreach (UIElement control in grid_hotkeys.Children)
            {
                Button btn = null;

                if (control.GetType().Name == "Button")
                {
                    btn = (Button) control;

                    if (btn.Content.ToString().Contains("None+"))
                    {
                        btn.Content = btn.Content.ToString().Replace("None+", "");
                    }
                }
            }

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] != "" && mods[i] != "")
                {
                    var list = new List<string>();

                    ghk = new GlobalHotKeys(Constants.getConstantInt(mods[i]), stringToKey(keys[i]), handle);

                    bool reg = ghk.Register();

                    list.Add(mods[i]);
                    list.Add(keys[i]);
                    int j = i + 1;
                    if (registeredKeys.ContainsKey("button_hk" + j))
                    {
                        registeredKeys.Remove("button_hk" + j);
                    }

                    registeredKeys.Add("button_hk" + j, list);
                    textBox1.Text += Constants.getConstantInt(mods[i]) + "+" + stringToKey(keys[i]) +
                                     "     REGISTERED: " +
                                     reg + "\r\n";
                }
            }
            int k = 0;

            foreach (String str in registeredKeys.Keys)
            {
                List<string> list = registeredKeys[str];
                textBox1.Text += k + ": " + list[0] + ":" + list[1] + "\r\n";
                k += 1;
            }
        }

        public void setupSaveFolder()
        {
            if (Settings.Default.localFolder != "")
            {
                textBox_saveFolder.Text = Settings.Default.localFolder;
                SAVEFOLDER = Settings.Default.localFolder;
            }
        }

        public Keys stringToKey(String str)
        {
            try
            {
                return (Keys) Enum.Parse(typeof (Keys), str);
            }
            catch (Exception)
            {
                return new Keys();
            }
        }


        public void HotkeyMessage()
        {
            MessageBox.Show("Key Pressed");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            textBox_wm.Text = msg.ToString();

            if (msg == Constants.WM_HOTKEY_MSG_ID)
            {
                int mod = ((int) lParam & 0xFFFF);

                string mod_ = Constants.getConstant(mod);
                var key = (Keys) ((lParam.ToInt32() >> 16) & 0xffff);

                textBox1.Text = "pressed: " + mod_ + " " + key + "\r\n";

                foreach (String button in registeredKeys.Keys)
                {
                    if (button == "button_hk1")
                    {
                        String regMod = registeredKeys[button][0];
                        String regKey = registeredKeys[button][1];

                        if (Constants.getConstantInt(regMod) == mod && regKey == key.ToString())
                        {
                            textBox1.Text += "Pressed Full Screenshot Key!";
                            scrHandler = new ScreenShotHandler();
                            Bitmap img = scrHandler.GetFullScreenShot();
                            img.Save(SAVEFOLDER+"test.jpg", ImageFormat.Jpeg);
                        }
                    }
                    if (button == "button_hk2")
                    {
                        String regMod = registeredKeys[button][0];
                        String regKey = registeredKeys[button][1];

                        if (Constants.getConstantInt(regMod) == mod && regKey == key.ToString())
                        {
                            textBox1.Text += "Pressed Current Window Screenshot Key!";
                            scrHandler = new ScreenShotHandler();
                            Bitmap img = scrHandler.GetCurrentWindowScreenShot();
                            img.Save(SAVEFOLDER + "test.jpg", ImageFormat.Jpeg);
                        }
                    }
                    if (button == "button_hk3")
                    {
                        String regMod = registeredKeys[button][0];
                        String regKey = registeredKeys[button][1];

                        if (Constants.getConstantInt(regMod) == mod && regKey == key.ToString())
                        {
                            textBox1.Text += "Pressed Region Screenshot Key!";
                            Window w = new DrawForm();
                            w.Show();
                        }
                    }
                    if (button == "button_hk4")
                    {
                        String regMod = registeredKeys[button][0];
                        String regKey = registeredKeys[button][1];

                        if (Constants.getConstantInt(regMod) == mod && regKey == key.ToString())
                        {
                            textBox1.Text += "Pressed Upload Clipboard Key!";
                            ClipboardHandler ch = new ClipboardHandler();
                            ch.saveClipboard();
                        }
                    }
                    if (button == "button_hk5")
                    {
                        String regMod = registeredKeys[button][0];
                        String regKey = registeredKeys[button][1];

                        if (Constants.getConstantInt(regMod) == mod && regKey == key.ToString())
                        {
                            textBox1.Text += "Pressed File Upload Key!";
                        }
                    }
                    if (button == "button_hk6")
                    {
                        String regMod = registeredKeys[button][0];
                        String regKey = registeredKeys[button][1];

                        if (Constants.getConstantInt(regMod) == mod && regKey == key.ToString())
                        {
                            textBox1.Text += "Pressed Unassigned Key!";
                        }
                    }

                }

            } return IntPtr.Zero;
        }

        public string login(String name, String password)
        {
            using (var client = new WebClientEx())
            {
                var values = new NameValueCollection
                {
                    {"username", name},
                    {"password", password},
                };
                // Login
                client.UploadValues("http://ravand.org/logintest/login.php", values);
                // getPage
                return client.DownloadString("http://ravand.org/logintest/memberarea.php");
            }
        }

        private void MemberArea_Load(object sender, EventArgs e)
        {
        }


        private void button_hk1_Click(object sender, EventArgs e)
        {
            label_keyassign.Visibility = Visibility.Visible;

            if (hkButton_pressed.Count == 0)
            {
                label_keyassign.Visibility = Visibility.Visible;
                hkButton_pressed.Add(button_hk1, true);
            }
        }


        private void button_hk2_Click(object sender, EventArgs e)
        {
            if (hkButton_pressed.Count == 0)
            {
                hkButton_pressed.Add(button_hk2, true);
                label_keyassign.Visibility = Visibility.Visible;

            }
        }

        private void button_hk3_Click(object sender, EventArgs e)
        {
            if (hkButton_pressed.Count == 0)
            {
                hkButton_pressed.Add(button_hk3, true);
                label_keyassign.Visibility = Visibility.Visible;

            }
        }

        private void button_hk4_Click(object sender, EventArgs e)
        {
            if (hkButton_pressed.Count == 0)
            {
                hkButton_pressed.Add(button_hk4, true);
                label_keyassign.Visibility = Visibility.Visible;

            }
        }

        private void button_hk5_Click(object sender, EventArgs e)
        {
            if (hkButton_pressed.Count == 0)
            {
                hkButton_pressed.Add(button_hk5, true);
                label_keyassign.Visibility = Visibility.Visible;

            }
        }

        private void button_hk6_Click(object sender, EventArgs e)
        {
            if (hkButton_pressed.Count == 0)
            {
                hkButton_pressed.Add(button_hk6, true);
                label_keyassign.Visibility = Visibility.Visible;

            }
        }

        public void box(string txt)
        {
            MessageBox.Show(txt);
        }





        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = true;
            folderDialog.Description = "Select where you want your local copies to be saved!";

            DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBox_saveFolder.Text = folderDialog.SelectedPath;
                SAVEFOLDER = folderDialog.SelectedPath + "/";
                Settings.Default.localFolder = SAVEFOLDER;
                Settings.Default.Save();
            }
        }

        private void grid_hotkeys_KeyUp(object sender, KeyEventArgs e)
        {
            TabItem ti = tabControl.SelectedItem as TabItem;


            if (ti.Header.ToString() == "HotKeys")
            {
                foreach (Button key in hkButton_pressed.Keys)
                {
                    if (hkButton_pressed[key])
                    {
                        if (e.Key != Key.Escape)
                        {
                            if (registeredKeys.ContainsKey(key.Name))
                            {
                                ghk = new GlobalHotKeys(Constants.getConstantInt(registeredKeys[key.Name][0]),
                                    stringToKey(registeredKeys[key.Name][1]), handle);
                                bool b = ghk.Unregister();
                                //box(registeredKeys[key.Name][0] + "+" + registeredKeys[key.Name][1] + " UNREGISTERED: " + b);
                                registeredKeys.Remove(key.Name);
                            }

                            

                            Keys modKey = Keys.None;


                            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) 
                            {
                                modKey = Keys.Alt;
                            }
                            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                            {
                                modKey = Keys.Control;
                            }
                            if (Keyboard.IsKeyDown(Key.LeftShift))
                            {
                                modKey = Keys.Shift;
                            }

                            Key Key_ = e.Key;
                            String str = "";
                            String keys = Key_.ToString();
                            String mod = "";

                            ghk = new GlobalHotKeys(Constants.getConstantInt(modKey.ToString()), stringToKey(e.Key.ToString()), handle);
                            bool b2 = ghk.Register();
                            //box(modKey + "+" + keys + " REGISTERED: " + b2);

                            registeredKeys.Remove(key.Name);
                            var list = new List<string>();
                            list.Add(modKey.ToString());
                            list.Add(Key_.ToString());
                            registeredKeys.Add(key.Name, list);

                            if (modKey != Keys.None)
                            {
                                mod = modKey + "+";
                            }
                            else
                            {
                                mod = "";
                            }

                            str = mod + keys;
                            key.Content = str;


                            Settings.Default[key.Name] = Key_.ToString();
                            Settings.Default[key.Name + "_mod"] = modKey.ToString();
                            Settings.Default.Save();


                            hkButton_pressed.Clear();
                            label_keyassign.Visibility = Visibility.Visible;

                            break;
                        }
                        hkButton_pressed.Clear();
                        label_keyassign.Visibility = Visibility.Hidden;

                        break;
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClipboardHandler ch = new ClipboardHandler(this);
            ch.dumpClipboard();
        }
    
    }
}
