﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FreePIE.Core.Contracts;
using FreePIE.Plugin_Keyboard.ScriptAuto;
using SharpDX.DirectInput;
using static FreePIE.CommonTools.GlobalTools;
using FreePIE.CommonStrategy;

//using SlimDX.DirectInput;
//using AutoIt;
// ajouter free.core  dans refererence pour NeedIndexer
//using FreePIE.Core.ScriptEngine.Globals.ScriptHelpers;   

namespace FreePIE.Plugin_Keyboard
{

    [GlobalType(Type = typeof(KeyboardGlobal))]
    public class KeyboardPlugin : Plugin
    {

        // Maps SharpDX key codes to scan codes
        private int[] ScanCodeMap =
            {
            0x0B, //D0 = 0,
            0x02, //D1 = 1,
            0x03, //D2 = 2,
            0x04, //D3 = 3,
            0x05, //D4 = 4,
            0x06, //D5 = 5,
            0x07, //D6 = 6,
            0x08, //D7 = 7,
            0x09, //D8 = 8,
            0x0A, //D9 = 9,
            0x1E, //A = 10,
            0x30, //B = 11,
            0x2E, //C = 12,
            0x20, //D = 13,
            0x12, //E = 14,
            0x21, //F = 15,
            0x22, //G = 16,
            0x23, //H = 17,
            0x17, //I = 18,
            0x24, //J = 19,
            0x25, //K = 20,
            0x26, //L = 21,
            0x32, //M = 22,
            0x31, //N = 23,
            0x18, //O = 24,
            0x19, //P = 25,
            0x10, //Q = 26,
            0x13, //R = 27,
            0x1F, //S = 28,
            0x14, //T = 29,
            0x16, //U = 30,
            0x2F, //V = 31,
            0x11, //W = 32,
            0x2D, //X = 33,
            0x15, //Y = 34,
            0x2C, //Z = 35,
            0x73, //AbntC1 = 36,
            0x7E, //AbntC2 = 37,
            0x28, //Apostrophe = 38,
            0xDD, //Applications = 39,
            0x91, //AT = 40,
            0x96, //AX = 41,
            0x0E, //Backspace = 42,
            0x2B, //Backslash = 43,
            0xA1, //Calculator = 44,
            0x3A, //CapsLock = 45,
            0x92, //Colon = 46,
            0x33, //Comma = 47,
            0x79, //Convert = 48,
            0xD3, //Delete = 49,
            0xD0, //DownArrow = 50,
            0xCF, //End = 51,
            0x0D, //Equals = 52,
            0x01, //Escape = 53,
            0x3B, //F1 = 54,
            0x3C, //F2 = 55,
            0x3D, //F3 = 56,
            0x3E, //F4 = 57,
            0x3F, //F5 = 58,
            0x40, //F6 = 59,
            0x41, //F7 = 60,
            0x42, //F8 = 61,
            0x43, //F9 = 62,
            0x44, //F10 = 63,
            0x57, //F11 = 64,
            0x58, //F12 = 65,
            0x64, //F13 = 66,
            0x65, //F14 = 67,
            0x66, //F15 = 68,
            0x29, //Grave = 69,
            0xC7, //Home = 70,
            0xD2, //Insert = 71,
            0x70, //Kana = 72,
            0x94, //Kanji = 73,
            0x1A, //LeftBracket = 74,
            0x1D, //LeftControl = 75,
            0xCB, //LeftArrow = 76,
            0x38, //LeftAlt = 77,
            0x2A, //LeftShift = 78,
            0xDB, //LeftWindowsKey = 79,
            0xEC, //Mail = 80,
            0xED, //MediaSelect = 81,
            0xA4, //MediaStop = 82,
            0x0C, //Minus = 83,
            0xA0, //Mute = 84,
            0xEB, //MyComputer = 85,
            0x99, //NextTrack = 86,
            0x7B, //NoConvert = 87,
            0x45, //NumberLock = 88,
            0x52, //NumberPad0 = 89,
            0x4F, //NumberPad1 = 90,
            0x50, //NumberPad2 = 91,
            0x51, //NumberPad3 = 92,
            0x4B, //NumberPad4 = 93,
            0x4C, //NumberPad5 = 94,
            0x4D, //NumberPad6 = 95,
            0x47, //NumberPad7 = 96,
            0x48, //NumberPad8 = 97,
            0x49, //NumberPad9 = 98,
            0xB3, //NumberPadComma = 99,
            0x9C, //NumberPadEnter = 100,
            0x8D, //NumberPadEquals = 101,
            0x4A, //NumberPadMinus = 102,
            0x53, //NumberPadPeriod = 103,
            0x4E, //NumberPadPlus = 104,
            0xB5, //NumberPadSlash = 105,
            0x37, //NumberPadStar = 106,
            0x56, //Oem102 = 107,
            0xD1, //PageDown = 108,
            0xC9, //PageUp = 109,
            0xC5, //Pause/break = 110,              DIK_PAUSE
            0x34, //Period = 111,
            0xA2, //PlayPause = 112,
            0xDE, //Power = 113,
            0x90, //PreviousTrack = 114,
            0x1B, //RightBracket = 115,
            0x9D, //RightControl = 116,
            0x1C, //Return = 117,
            0xCD, //RightArrow = 118,
            0xB8, //RightAlt = 119,
            0x36, //RightShift = 120,
            0xDC, //RightWindowsKey = 121,
            0x46, //ScrollLock = 122,
            0x27, //Semicolon = 123,
            0x35, //Slash = 124,
            0xDF, //Sleep = 125,
            0x39, //Space = 126,
            0x95, //Stop = 127,                     DIK_STOP
            0xB7, //PrintScreen = 128,              DIK_SYSRQ
            0x0F, //Tab = 129,
            0x93, //Underline = 130,
            0x97, //Unlabeled = 131,
            0xC8, //UpArrow = 132,
            0xAE, //VolumeDown = 133,
            0xB0, //VolumeUp = 134,
            0xE3, //Wake = 135,
            0xEA, //WebBack = 136,
            0xE6, //WebFavorites = 137,
            0xE9, //WebForward = 138,
            0xB2, //WebHome = 139,
            0xE7, //WebRefresh = 140,
            0xE5, //WebSearch = 141,
            0xE8, //WebStop = 142,
            0x7D, //Yen = 143,
            0, //Unknown = 144,
        };

        private HashSet<int> extendedKeyMap = new HashSet<int>() { 39, 44, 46, 48, 49, 50, 51, 70, 71, 76, 79, 80, 81, 82, 84, 85, 86, 100, 105, 108, 109, 110, 112, 113, 114, 116, 118, 119, 121, 125, 127, 128, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143 };
        private HashSet<int> extKeyMap = new HashSet<int>() { 121, 125, 144, 146, 149, 153, 156, 157, 160, 161, 162, 164, 174, 176, 178, 181, 183, 184, 197, 199, 200, 201, 203, 205, 207, 208, 209, 210, 211, 219, 220, 221, 222, 223, 227, 229, 230, 231, 232, 233, 234, 235, 236, 237 };

        private ScriptKeyboard SF;

        //private Dictionary<int, int> azerty = new Dictionary<int, int>()
        //{
        //    { (int)Key.A, (int)Key.Q},
        //    { (int)Key.Q, (int)Key.A},
        //    { (int)Key.Z, (int)Key.W},
        //    { (int)Key.W, (int)Key.Z},
        //    { (int)Key.Semicolon, (int)Key.M},
        //    { (int)Key.Comma, (int)Key.Semicolon},
        //    { (int)Key.M, (int)Key.Comma}
        //};

        private DirectInput DirectInputInstance = new DirectInput();
        private Keyboard KeyboardDevice;
        private KeyboardState KeyState = new KeyboardState();
 
        private bool[] MyKeyDown = new bool[150];
        public SetPressedStrategy<int> setPressedStrategy;
        public GetPressedStrategy<int> getPressedStrategy;

        public override object CreateGlobal()
        {
            return new KeyboardGlobal(this);
        }

        public override string FriendlyName
        {
            get { return "Keyboard"; }
        }

        public override Action Start()
        {

            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

            KeyboardDevice = new Keyboard(DirectInputInstance);
            if (KeyboardDevice == null)
                throw new Exception("Failed to create keyboard device");

            KeyboardDevice.SetCooperativeLevel(handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
            KeyboardDevice.Acquire();

            KeyboardDevice.GetCurrentState(ref KeyState);

            setPressedStrategy = new SetPressedStrategy<int>(KeyDown, KeyUp);
            getPressedStrategy = new GetPressedStrategy<int>(IsDown);

            OnStarted(this, new EventArgs());
            return null;
        }

        public override void Stop()
        {
            SF = null;
            // Don't leave any keys pressed
            for (int i = 0; i < MyKeyDown.Length; i++)
            {
                if (MyKeyDown[i])
                    KeyUp(i);
            }

            if (KeyboardDevice != null)
            {
                KeyboardDevice.Unacquire();
                KeyboardDevice.Dispose();
                KeyboardDevice = null;
            }

            if (DirectInputInstance != null)
            {
                DirectInputInstance.Dispose();
                DirectInputInstance = null;
            }
        }

        public override bool GetProperty(int index, IPluginProperty property)
        {
            return false;
        }

        public override bool SetProperties(Dictionary<string, object> properties)
        {
            return false;
        }

        public override void DoBeforeNextExecute()
        {
            KeyboardDevice.GetCurrentState(ref KeyState);

            setPressedStrategy.Do();
            CheckScriptTimer();

            if (cmd == 'K' && setPressedStrategy.IsListEmpty())
            {
                if (SF == null)
                {
                    SF = new ScriptKeyboard(this, ref ScanCodeMap, ref KeyState);
                }
                SF.Keyboard();
            }
        }

        public bool isLedScrollLockOn() => System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.Scroll);

        public bool IsSingleClicked(int keycode) => getPressedStrategy.IsSingleClicked(keycode);
        public bool IsDoubleClicked(int keycode) => getPressedStrategy.IsDoubleClicked(keycode);

        public int HeldDown(int keycode, int nbvalue, int lapse) => getPressedStrategy.HeldDown(keycode, IsDown(keycode), nbvalue, lapse);
        public void HeldDownStop(int keycode) => getPressedStrategy.HeldDownStop(keycode);

        public bool IsDown(int keycode, bool value = false)
        {
            // Returns true if the key is currently being pressed
            var key = (SharpDX.DirectInput.Key)ScanCodeMap[keycode];
            return KeyState.IsPressed(key) || MyKeyDown[keycode];
        }

        public bool IsUp(int keycode) => !IsDown(keycode) && !MyKeyDown[keycode];

        public bool IsPressed(int key) => getPressedStrategy.IsPressed(key);
        public bool IsReleased(int key) => getPressedStrategy.IsReleased(key);

        private MouseKeyIO.KEYBDINPUT KeyInput(ushort code, uint flag)
        {
            var i = new MouseKeyIO.KEYBDINPUT();
            i.wVk = 0;
            i.wScan = code;
            i.time = 0;
            i.dwExtraInfo = IntPtr.Zero;
            i.dwFlags = flag | MouseKeyIO.KEYEVENTF_SCANCODE;
            return i;
        }

        public void KeyDown(int code)
        {
            if (code < 0) return;
            if (!MyKeyDown[code])
            {
                //System.Console.Out.WriteLine("keydown");
                MyKeyDown[code] = true;
                int scancode = ScanCodeMap[code]; // convert the keycode for SendInput

                var input = new MouseKeyIO.INPUT[1];
                input[0].type = MouseKeyIO.INPUT_KEYBOARD;
                //if (ExtendedKeyMap[code])
                //    input[0].ki = KeyInput((ushort)scancode, MouseKeyIO.KEYEVENTF_EXTENDEDKEY);
                //else
                //    input[0].ki = KeyInput((ushort)scancode, 0);
                input[0].ki = KeyInput((ushort)scancode,
                    extendedKeyMap.Contains(code) ? MouseKeyIO.KEYEVENTF_EXTENDEDKEY : 0);
                MouseKeyIO.NativeMethods.SendInput(1, input, Marshal.SizeOf(input[0].GetType()));
            }
        }

        public void KeyUp(int code)
        {
            if (code < 0) return;
            if (MyKeyDown[code])
            {
                //System.Console.Out.WriteLine("keyup");
                MyKeyDown[code] = false;

                int scancode = ScanCodeMap[code]; // convert the keycode for SendInput

                var input = new MouseKeyIO.INPUT[1];
                input[0].type = MouseKeyIO.INPUT_KEYBOARD;
                //                if (ExtendedKeyMap[code])
                if (extendedKeyMap.Contains(code))
                    input[0].ki = KeyInput((ushort)scancode,
                        MouseKeyIO.KEYEVENTF_EXTENDEDKEY | MouseKeyIO.KEYEVENTF_KEYUP);
                else
                    input[0].ki = KeyInput((ushort)scancode, MouseKeyIO.KEYEVENTF_KEYUP);

                MouseKeyIO.NativeMethods.SendInput(1, input, Marshal.SizeOf(input[0].GetType()));
            }
        }
        public int Keyspressed => KeyState.PressedKeys.Count;
        public List<Key> ListKeyspressed
        {
            get
            {
                if (Keyspressed == 0) return null;
                List<Key> ks = new List<Key>();
                foreach(var k in KeyState.PressedKeys)
                {
                    var a = Array.FindIndex(ScanCodeMap, t => t == (int)k);
                    ks.Add((Key)a);
                }
                return ks;
            }
    }
        public List<int> ListKeyspressed1 => KeyState.PressedKeys.Cast<int>().ToList();
        public void PressAndRelease(int keycode) => setPressedStrategy.Add(keycode);
        public void PressAndRelease(int keycode, bool state) => setPressedStrategy.Add(keycode, state);

        public List<KeySharpDx1> getKeys()
        {
            List<KeySharpDx1> kk = new List<KeySharpDx1>();
            foreach (var k in extendedKeyMap)
            {
                kk.Add( (KeySharpDx1)ScanCodeMap[k]);
            }
            return kk;
        }
    }

    [Global(Name = "keyboard")]
    public class KeyboardGlobal
    {
        //private bool azerty_t, azerty_s;
        private readonly KeyboardPlugin plugin;

        public KeyboardGlobal(KeyboardPlugin plugin)
        {
            this.plugin = plugin;
        }

        //public bool azerty_test
        //{
        //    get { return azerty_t; }
        //    set { azerty_t = value; }
        //}
        //public bool azerty_send
        //{
        //    get { return azerty_s; }
        //    set { azerty_s = value; }
        //}

        //----------------------- special key -----------------------------------------------------------
        public bool xShift => plugin.IsDown((int) Key.LeftShift) || plugin.IsDown((int) Key.RightShift);
        public bool xControl => plugin.IsDown((int) Key.LeftControl) || plugin.IsDown((int) Key.RightControl);
        public bool xAlt => plugin.IsDown((int) Key.LeftAlt) || plugin.IsDown((int) Key.RightAlt);
        public bool xWin => plugin.IsDown((int) Key.LeftWindowsKey) || plugin.IsDown((int) Key.RightWindowsKey);
        //------------------------------------------------------------------------------------------------
        public Key intTOkey(int key)
        {
            //string e = ((Key)key).ToString();
            //string t = Enum.GetName(typeof(Key), key);
            return (Key) key;
        }

        //// ****************** key single or double clicked ************************************
        public bool getClicked<T>(T key, bool dblclick = false)
        {
            return dblclick ? plugin.IsDoubleClicked(Convert.ToInt32(key)) : plugin.IsSingleClicked(Convert.ToInt32(key));
        }
        // ****************** button Helddown ************************************
        public int getHeldDown<T>(T key, int nbvalue, int duration) => plugin.HeldDown(Convert.ToInt32(key), nbvalue, duration);
        public void getHeldDownStop<T>(T key) => plugin.HeldDownStop(Convert.ToInt32(key));

        public string getNamekey(int i) => Enum.GetName(typeof(Key), i);
        public Key getNamekey1(string k)
        {
            //Enum.TryParse(k, out Key mykey);
            return (Key)Enum.Parse(typeof(Key), k, true);
        }
        public bool getDown<T>(T key) where T : struct => plugin.IsDown(Convert.ToInt32(key));

        public bool getDown<T>(IList<T> keys)
        {
            foreach (var key in keys)
                if (!plugin.IsDown(Convert.ToInt32(key))) return false;
            return true;
        }

        public bool getUp<T>(T key) => plugin.IsUp(Convert.ToInt32(key));

        public bool getPressed<T>(T key) => plugin.IsPressed(Convert.ToInt32(key));

        public bool getReleased<T>(T key) => plugin.IsReleased(Convert.ToInt32(key));

        public void setKeyDown(Key key) => plugin.KeyDown((int) key);

        //Multi keys to Keydown, return is list reversed
        public List<Key> setKeyDown(IList<Key> keys)
        {
            List<Key> k = new List<Key>();
            for (int i = keys.Count - 1; i >= 0; i--)
                k.Add(keys[i]);

            setKey(keys, true);
            return k;
        }

        // direction Pov to list of key(s)
        public List<Key> getKeyFromPov(int direction, params IList<Key>[] keys)
        {
            if (direction < 0 || direction > 3) return null;

            List<Key> keycursor = new List<Key>();

            switch (keys.Length)
            {
                case 1:
                    keycursor.Add(keys[0][direction]);
                    break;
                case 4:
                    foreach (var ky in keys[direction])
                        keycursor.Add(ky);
                    break;
                default:
                    throw new Exception($"Number of list: {keys.Length}. Just only 1 or 4 lists of Keys.");
            }
            return keycursor;
        }

        public void setKeyUp(Key key) => plugin.KeyUp((int) key);

        public void setKeyUp(IList<Key> keys, bool reverse = false) => setKey(keys, false, reverse);
        public void setKey(Key key, bool down)
        {
            if (down)
                plugin.KeyDown((int) key);
            else
                plugin.KeyUp((int) key);
        }

        public void setKey(IList<Key> keys, bool down, bool reverse = false)
        {
            if (reverse)
                foreach (var k in keys.Reverse())
                    setKey(k, down);
            else
                foreach (var k in keys)
                    setKey(k, down);
        }

        // One Key to press
        public void setPressed<T>(T key) where T : struct => plugin.PressAndRelease(Convert.ToInt32(key));

        // Key(s) to press one time only
        public void setPressed<T>(IList<T> keys, bool state)
        {
            foreach (var k in keys)
                plugin.PressAndRelease(Convert.ToInt32(k), state);
        }

        public void setPressed<T>(T key, bool state) where T : struct => plugin.PressAndRelease((Convert.ToInt32(key)), state);

        // list of Keys to press

        public void setPressed(int[] directionXY, params IList<Key>[] keys)
        {
            if (keys.Length < 4) return;
            for (int i = 0; i < 4; i++)
                if (i == directionXY[0] || i == directionXY[1]) setPressed(keys[i], true);
        }

        //public void setPressedNow(IList<Key> keys, int time = 20, bool sw = true)
        //{
        //    if (keys == null || !sw) return;
        //    var b = setKeyDown(keys);
        //    Thread.Sleep(time);
        //    setKeyUp(b);
        //}
        public void setPressedBip(IList<Key> keys, int frequency, int duration = 300)
        {
            foreach (var k in keys)
                plugin.PressAndRelease((int) k);
            true.Beep(frequency, duration);
        }

        public bool isScrollLedOn() => plugin.isLedScrollLockOn();
        public int getNbrKeysDown => plugin.Keyspressed;
        public List<Key> getListOfKeysDown => plugin.ListKeyspressed;
        public string getBuffer() => buffer;

        //private BeepPlugin beepplg;
        //public BeepPlugin BeepPlg
        //{
        //    get { return beepplg; }
        //    set { beepplg = value; }
        //}

        //public void beep(int f, int d)
        //{
        //    BeepPlg.Beep(f, d);
        //}

        public List<KeySharpDx1> getListOfx => plugin.getKeys();
        //private List<string> test1(List<int> l)
        //{
        //    List<string> t = new List<string>();
        //    foreach (var z in l)
        //        t.Add(z.ToString());
        //    return t;
        //}
        //public List<string> test(List<int> tu)
        //{
        //    return test1(tu);
        //}
        //public KeyboardGlobal save()
        //{
        //    return this;
        //}
    }
}
