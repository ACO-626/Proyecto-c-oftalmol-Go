#region BIBLIOTECAS
//BIBLIOTECAS BÁSICAS
using System;
using System.Windows.Forms;

//BIBLIOTECAS DE BD
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
#endregion

namespace Oftalmol_Go
{
    public partial class FormEliminar : Form
    {
        #region INICIALIZACION DE CONEXIÓN A LA BASE DATOS
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "608vd6SDwtR7uGhPNhd9j7bbgL0E2mR0xbhyOwSk",
            BasePath = "https://oftalmol-godb.firebaseio.com/"

        };
        IFirebaseClient client;
        #endregion

        #region VARIABLES
        int mesSelect = 0;
        int dias = 0;
        int añoSelect = 0;
        #endregion

        #region INICIALIZACIÓN DE VENTANA

        public FormEliminar()
        {
            InitializeComponent();
        }
        #endregion

        #region BOTONES
        private void btnCancelarEliminar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion  

        #region COMPORTAMIENTO DE COMBOBOX
        private void FormEliminar_Load(object sender, EventArgs e)
        {
            for (int i = 2020; i < 2026; i++)
            {
                comboAño.Items.Add(i);
            }

            for (int i = 0; i < 12; i++)
            {
                comboMes.Items.Add((i + 1).ToString("00"));
            }
            for (int i = 0; i < 24; i++)
            {
                comboHora.Items.Add((i + 1).ToString("00"));
            }
            for (int i = 0; i < 1; i++)
            {
                comboMin.Items.Add(0.ToString("00"));
                comboMin.Items.Add(30);
            }


        }

        private void comboAño_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboHora.Enabled = false;
            comboMin.Enabled = false;
            comboDia.Text = "Día";
            comboMes.Enabled = true;
            if (mesSelect == 2)
            {
                comboMes.SelectedIndex = 2;
                comboMes.SelectedIndex = 1;
            }
        }
        private void comboMes_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboDia.Items.Clear();
            comboHora.Enabled = false;
            comboMin.Enabled = false;
            comboDia.Text = "Día";
            mesSelect = int.Parse(comboMes.Text);
            añoSelect = int.Parse(comboAño.Text);

            switch (mesSelect)
            {
                case 1:
                    dias = 31;
                    break;
                case 3:
                    dias = 31;
                    break;
                case 7:
                    dias = 31;
                    break;
                case 8:
                    dias = 31;
                    break;
                case 10:
                    dias = 31;
                    break;
                case 12:
                    dias = 31;
                    break;
                case 2:
                    if (añoSelect % 4 == 0)
                    {
                        dias = 29;
                    }
                    else
                    {
                        dias = 28;
                    }
                    break;
                default:
                    dias = 30;
                    break;
            }

            for (int i = 0; i < dias; i++)
            {

                comboDia.Items.Add((i + 1).ToString("00"));
            }
            comboDia.Enabled = true;
        }

        private void comboDia_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboHora.Enabled = true;
        }

        private void comboHora_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMin.Enabled = true;
        }
        #endregion

        #region ELIMINAR
        private async void btnEliminarCita_Click(object sender, EventArgs e)
        {
                    
            this.Enabled = false;
            client = new FireSharp.FirebaseClient(config);
            var cita = new Cita
            {
                citaId = long.Parse(comboAño.Text + comboMes.Text + comboDia.Text + comboHora.Text + comboMin.Text)
            };
            FirebaseResponse response = await client.DeleteTaskAsync("PACIENTES/" +cita.citaId);
            MessageBox.Show("Se ha borrado la cita exitosamente de "+cita.citaId, "Base de datos");
            this.Close();
        }

        #endregion
        
    }
}
