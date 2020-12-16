// Check tutorial on youtube https://www.youtube.com/watch?v=m8wsxb6PaNs


# M850-C-Driver
Driver for AllenBradley Micro850 with Visual Basic C#

//Import files in your projets in visual Basic. 

//Exemple with a Android application :

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Micro800Connection.DriverM800;
using System.Timers;

namespace ControlM850Android2
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        M800Driver m800Driver;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            txtTemp = (TextView)FindViewById(Resource.Id.txtTemp);
            txtSetpoint = (TextView)FindViewById(Resource.Id.txtSetpoint);
            txtSeekBar = (TextView)FindViewById(Resource.Id.txtSeekBar);
            txtOuput = (TextView)FindViewById(Resource.Id.txtOutput);
            
            
            m800Driver = new M800Driver("PLC adresse IP", 44818);
            
            //Used to connect with plc
            m800Driver.connection();
            
            createVariable();

            timer1 = new Timer();
            timer1.Interval = 500;
            timer1.Elapsed += refreshScreen;
            timer1.Start();

        }

        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        private void refreshScreen(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                txtTemp.Text = "Temperature : " + m800Driver.readFloat("varTemp",2)+"°C";
                txtSetpoint.Text = "Setpoint : " + m800Driver.read("varSetpoint")+"°C";
                txtOuput.Text = "Output : " + m800Driver.read("bOutput");
            });
        }




        private void createVariable()
        {
        
            //m800Driver.createVariable(Variable name,PLC VariableName, reading intervale in Millisecondes);
            m800Driver.createVariable("varTemp", "Temp", 500);
            m800Driver.createVariable("varSetpoint", "Setpoint", 500);
            m800Driver.createVariable("bAutoMan", "AutoMan", 500);
            m800Driver.createVariable("bJog", "BpManuel", 500);
            m800Driver.createVariable("bOutput", "_IO_EM_DO_00", 500);
        }
    }
}

