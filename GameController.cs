using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Windows.Forms;

using DanmakuGraphics;
namespace shmup
{
    // бог всего и вся
    abstract class GameController
    {
        private static SaveManager<GameController> saveManager = new SaveManager<GameController>();
        protected static void Save() {
		var s = "Hello string";
	    float noTest = 2;
		var result = 10;
		result += 10;
	    int test = 3;
            saveManager.Save(Valid, Valid.ToString());
	    x = 5;
	    Console.WriteLine(x);
        }
        protected static void LoadNew<T>() where T : GameController, new()    // загружает контроллер
        {
                Valid = new T();
            Valid.Initialize();
        }
        protected static void Load<T>() where T : GameController, new()    // загружает контроллер
        {
            bool objektIsLoaded;
            Valid = saveManager.Load(typeof(T).ToString(), out objektIsLoaded);
            if (!objektIsLoaded) {
                Valid = new T();
                Valid.Initialize();
            }
        }
        protected static void QuickSave() {
            saveManager.QuickSave(Valid);
        } 
        protected static void QuickLoad() {
            Valid = saveManager.QuickLoad();
        }

        virtual public void GoToMenu() {
            throw new InvalidOperationException("функция LoadMenu не может быть вызвана из " + Valid);

            //if (Valid is GameControllers.PlayMode)
            //    throw new InvalidOperationException(typeof(GameControllers.MenuMode) + " нельзя вызвать из " + typeof(GameControllers.PlayMode));
            Load<GameControllers.MenuMode>();
        }
        static public void InitializeGame() {
            if (Valid != null)
                throw new InvalidOperationException("Игра уже начата!");
            Load<GameControllers.MenuMode>();
        }
        virtual public void StartGame() {
            throw new InvalidOperationException("функция PauseGame не может быть вызвана из " + Valid);

        }
        virtual public void PauseGame() {
            throw new InvalidOperationException("функция PauseGame не может быть вызвана из " + Valid);
            if (!(Valid is GameControllers.PlayMode))
                throw new InvalidOperationException(typeof(GameControllers.PauseMode) + " можно вызвать только из " + typeof(GameControllers.PlayMode));
            Load<GameControllers.PauseMode>();
        }
        virtual public void ContinueGame() {
            throw new InvalidOperationException("функция ContinueGame не может быть вызвана из " + Valid);
            //if (!(Valid is GameControllers.PauseMode))
            //    throw new InvalidOperationException("Продолжить игру можно только из меню паузы!");
            bool gameIsStarted = false;
            Valid = saveManager.Load("PlayMode", out gameIsStarted);
            if (!gameIsStarted)
                throw new InvalidOperationException("Игра не начата!");
        }
        virtual public void TurnBack() {
            throw new InvalidOperationException("функция TurnBack не может быть вызвана из " + Valid);

            Valid = saveManager.QuickLoad();
        }
        //работа с окном
        protected static Form1 ActiveWindow { get; set; }
        protected static bool windowIsLoaded = false;
        public static void Exit() {
            ActiveWindow.Close();
        }
        static public void LoadWindow(Form1 window) {
            if (window != null) {
                ActiveWindow = window;
                windowIsLoaded = true;
            }
        }
        public static bool FullScreenModeOn {
            get {
                if (windowIsLoaded)
                    return ActiveWindow.WindowState == FormWindowState.Maximized;
                else
                    throw new FormatException("Не загружено окно!");
            }

        }
        protected static int ScreenWidth {
            get {
                if (windowIsLoaded)
                    return ActiveWindow.Screen.Width;
                else
                    throw new FormatException("Не загружено окно!");
            }
        }
        protected static int ScreenHeight {
            get {
                if (windowIsLoaded)
                    return ActiveWindow.Screen.Height;
                else
                    throw new FormatException("Не загружено окно!");
            }
        }
        abstract protected void WindowInitialize();
        protected bool windowInitialized = false;

       
        protected static double centerX, centerY; //координаты центр экрана

        //Графика
        protected void InitializeGraphics() {
            if (!GraphicsInitialized) {
                Glut.glutInit();
                Glut.glutInitDisplayMode(Glut.GLUT_RGBA | Glut.GLUT_DOUBLE);
                Il.ilInit();
                Il.ilEnable(Il.IL_ORIGIN_SET);
                Gl.glClearColor(255, 0, 0, 1);
                GraphicsInitialized = true;
            }
            
        }
        static public bool GraphicsInitialized { get; protected set; }
        static protected void RefreshViewPort(Sprite UIsprite) {
            if (!GraphicsInitialized)
                throw new InvalidOperationException("Графика не инициализирована!");
            if (UIsprite == null)
                throw new FormatException("Не загружен спрайт UI!");
            Gl.glViewport(0, 0, ScreenWidth, ScreenHeight);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Glu.gluOrtho2D(0, UIsprite.Width, 0, UIsprite.Height);
            centerX = UIsprite.Width / 2;
            centerY = UIsprite.Height / 2;
        }
        protected void TranslateOrigin(double x, double y) {
            Gl.glTranslated(x, y, 0);
            centerX -= x;
            centerY -= y;
        }
        //параметры текущего gc
        public static GameController Valid { get; private set; }  // ссылка на действующий игровой контроллер
        public static int Ticks { get; protected set; }  // время, сколько игровой контроллер работает

        //функции действия
        abstract public void ChangeScreenMode();
        public abstract void Initialize();
        public abstract void Action();  // вызывается каждый кадр
        protected static void AllObjectsAct()    // функция для упрощения кода; просто вызывает Action() у всех объектов
        {
            Player.Instance.Action();
            foreach (Bullet bullet in Bullet.Container)
                bullet.Action();
            foreach (var handler in Handler.Container)
                handler.Action();
        }
        protected static void DrawAllObjects() {
            foreach (Priority priority in Enum.GetValues(typeof(Priority))) {
                foreach (Bullet bullet in Bullet.Container[priority])
                    bullet.Draw();
        }
        }
    }

    // gc - GameController
    namespace GameControllers
    {
        class PlayMode : GameController
        {
            override public void PauseGame() {
                Save();
                LoadNew<PauseMode>();
            }
            // работа с UI и активным окном
            Sprite HUD;
            const int topBorderHUD = 20, bottomBorderHUD = 20, leftBorderHUD = 40, rightBorderHUD = 240;
            public void DrawHUD() {
                HUD.Draw(centerX, centerY);
               
            }
            override public void ChangeScreenMode() {
                if (FullScreenModeOn == false) {
                    ActiveWindow.FormBorderStyle = FormBorderStyle.None;
                    ActiveWindow.WindowState = FormWindowState.Maximized;
                    RefreshViewPort(HUD);
                    TranslateOrigin(leftBorderHUD, bottomBorderHUD);
                }
                else if (FullScreenModeOn == true) {
                    ActiveWindow.WindowState = FormWindowState.Normal;
                    ActiveWindow.FormBorderStyle = FormBorderStyle.Fixed3D;
                    RefreshViewPort(HUD);

                    TranslateOrigin(leftBorderHUD, bottomBorderHUD);
                }
            }
            override protected void WindowInitialize() {
                HUD = new Sprite("HUD");
                if (windowIsLoaded) {
                    if (!windowInitialized) {
                        int horizontalBorders = (ActiveWindow.Size.Width - ActiveWindow.Screen.Width);
                        int verticalBorders = ActiveWindow.Size.Height - ActiveWindow.Screen.Height;
                        ActiveWindow.Size = new System.Drawing.Size(HUD.Width + horizontalBorders, HUD.Height + verticalBorders);
                        centerX = ScreenWidth / 2;
                        centerY = ScreenHeight / 2;
                    }
                }
                else throw new FormatException("Не загружено окно!");
            }


            //инициализация работа и функция таймера
            public override void Initialize() {
                InputManager.Load<PlayerController>();
                InitializeGraphics();
                WindowInitialize();
                RefreshViewPort(HUD);
                TranslateOrigin(leftBorderHUD, bottomBorderHUD);

                Player.Instance.LoadCharacter("Clownpiece");
                Ticks = 0;
                Player.Instance.Put(Metric.Pct(50), Metric.Pct(50));    // ставит игрока в центр экрана
                Cleaner.DeleteAllObjects();
				CommandList.Instance.Clear();
                chapters.TestChapter.Load();    // проверка этого чаптера
                ActiveWindow.MainTimer.Start();

            }
            public override void Action() {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glClearColor(0, 0, 0, 1);

                CommandList.Instance.Perform(); // проверка списка команд
                AllObjectsAct();    // передвижение всех объектов, включая игрока
                Cleaner.CleanUp();
                //здесь будет прорисовка поля
                Player.Instance.DrawSprite();
                DrawAllObjects();
                Player.Instance.DrawHitbox();
                DrawHUD();
				Sprite.PrintText(0, 30, "Graze: " + Player.Instance.Graze);
				Sprite.PrintText(0, 15, "Handlers: " + Handler.Container.Count);
				Sprite.PrintText(0, 0, "Bullets: " + Bullet.Container.Count);
                ++Ticks;
            }
        }

        class MenuMode : GameController
        {
            public override void StartGame() {
                LoadNew<PlayMode>();
            }
            Sprite MenuBackground;
            public void DrawMenu() {
                MenuBackground.Draw(centerX, centerY);
                ButtonManager.Valid.DrawButtons();
            }
            public override void Initialize() {
                InputManager.Load<MenuController>();
                InitializeGraphics();
                WindowInitialize();
                RefreshViewPort(MenuBackground);

                ButtonManager.Load(SortingMode.Vertical);
                ButtonManager.Valid.AddButton(20, 700, StartGame, "New Game");
                ButtonManager.Valid.AddButton(20, 600, Exit, "Quit");

                Ticks = 0;
                ActiveWindow.MainTimer.Start();

            }
            override protected void WindowInitialize() {
                    MenuBackground = new Sprite("Menu");
                if (windowIsLoaded) {
                    if (!windowInitialized) {
                        int horizontalBorders = (ActiveWindow.Size.Width - ActiveWindow.Screen.Width);
                        int verticalBorders = ActiveWindow.Size.Height - ActiveWindow.Screen.Height;
                        ActiveWindow.Size = new System.Drawing.Size(MenuBackground.Width + horizontalBorders, MenuBackground.Height + verticalBorders);
                        centerX = ScreenWidth / 2;
                        centerY = ScreenHeight / 2;
                    }
                }
                else throw new FormatException("Не загружено окно!");
            }
            public override void Action() {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glClearColor(255, 0, 0, 1);
                //здесь будет прорисовка поля
                DrawMenu();
                ++Ticks;
            }
            override public void ChangeScreenMode() {
                if (FullScreenModeOn == false) {
                    ActiveWindow.FormBorderStyle = FormBorderStyle.None;
                    ActiveWindow.WindowState = FormWindowState.Maximized;
                    RefreshViewPort(MenuBackground);
                    //Gl.glViewport(0, 0, ScreenWidth, ScreenHeight);
                    //Gl.glMatrixMode(Gl.GL_MODELVIEW);
                    //Gl.glLoadIdentity();

                    //Glu.gluOrtho2D(0, MenuBackground.Width, 0, MenuBackground.Height);
                    //centerX = MenuBackground.Width / 2;
                    //centerY = MenuBackground.Height / 2;
                    //Gl.glMatrixMode(Gl.GL_MODELVIEW);
                }
                else if (FullScreenModeOn == true) {
                    ActiveWindow.WindowState = FormWindowState.Normal;
                    ActiveWindow.FormBorderStyle = FormBorderStyle.Fixed3D;
                    centerX = ScreenWidth / 2;
                    centerY = ScreenHeight / 2;
                    RefreshViewPort(MenuBackground);
                    //Gl.glViewport(0, 0, MenuBackground.Width, MenuBackground.Height);
                    //Gl.glMatrixMode(Gl.GL_MODELVIEW);
                    //Gl.glLoadIdentity();

                    //Glu.gluOrtho2D(0, MenuBackground.Width, 0, MenuBackground.Height);
                    //Gl.glMatrixMode(Gl.GL_MODELVIEW);
                }
            }
        }
        class PauseMode : GameController
        {
            const int topBorderHUD = 20, bottomBorderHUD = 20, leftBorderHUD = 40, rightBorderHUD = 240;

            public override void ContinueGame() {
                Load<PlayMode>();
                InputManager.Load<PlayerController>();
            }
            public override void GoToMenu() {
                LoadNew<MenuMode>();
            }
            Sprite pauseBackground;
            override protected void WindowInitialize() {
                pauseBackground = new Sprite("Pause");
                if (windowIsLoaded) {
                    if (!windowInitialized) {
                        int horizontalBorders = (ActiveWindow.Size.Width - ActiveWindow.Screen.Width);
                        int verticalBorders = ActiveWindow.Size.Height - ActiveWindow.Screen.Height;
                        ActiveWindow.Size = new System.Drawing.Size(pauseBackground.Width + horizontalBorders, pauseBackground.Height + verticalBorders);
                        centerX = ScreenWidth / 2;
                        centerY = ScreenHeight / 2;
                    }
                }
                else throw new FormatException("Не загружено окно!");
            }
            public override void Initialize() {
                InputManager.Load<MenuController>();
                InitializeGraphics();
                WindowInitialize();
                RefreshViewPort(pauseBackground);
                    TranslateOrigin(leftBorderHUD, bottomBorderHUD);

                ButtonManager.Load(SortingMode.Vertical);
                ButtonManager.Valid.AddButton(20, 700, ContinueGame, "Continue Game");
                ButtonManager.Valid.AddButton(20, 600, GoToMenu, "Quit1");

                Ticks = 0;
            }
            override public void ChangeScreenMode() {
                if (!FullScreenModeOn) {
                    ActiveWindow.FormBorderStyle = FormBorderStyle.None;
                    ActiveWindow.WindowState = FormWindowState.Maximized;
                    RefreshViewPort(pauseBackground);
                }
                else {
                    ActiveWindow.WindowState = FormWindowState.Normal;
                    ActiveWindow.FormBorderStyle = FormBorderStyle.Fixed3D;
                    RefreshViewPort(pauseBackground);
                }
            }
            public override void Action() {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glClearColor(0, 0, 0, 1);
                Player.Instance.DrawSprite();
                DrawAllObjects();
                pauseBackground.Draw(centerX, centerY);
                ButtonManager.Valid.DrawButtons();

            }

        }
    }
}
