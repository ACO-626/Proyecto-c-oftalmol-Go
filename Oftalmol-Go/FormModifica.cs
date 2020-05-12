#region BIBLIOTECAS
using System;
using System.Windows.Forms;

//BIBLIOTECAS PARA BD
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
#endregion

namespace Oftalmol_Go
{
    public partial class FormModifica : Form
    {
        #region VARIABLES
        int tipoCambio = 0;
        int mesSelect = 0;
        int dias = 0;
        int añoSelect = 0;
        int mesSelect2 = 0;
        int añoSelect2 = 0;
        #endregion

        #region INSTANCIAS PRINCIPALES PARA LA BD
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "608vd6SDwtR7uGhPNhd9j7bbgL0E2mR0xbhyOwSk",
            BasePath = "https://oftalmol-godb.firebaseio.com/"

        };
        IFirebaseClient client;
        #endregion

        #region INICIALIZACIÓN DE INTERFAZ
        public FormModifica(int indicador)
        {
            InitializeComponent();
            tipoCambio = indicador;
            if(indicador==1)
            {
                lbSimpleCambio.Text = "Nueva fecha y hora";
                comboAño2.Visible = true;
                comboMes2.Visible = true;
                comboHora2.Visible = true;
                comboDia2.Visible = true;
                comboMin2.Visible = true;
            }
            else
            {
                txtbModificacion.Visible = true;

                switch(indicador)
                {
                    case 2:
                        lbSimpleCambio.Text = "Ingrese el nuevo nombre";
                    break;
                    case 3:
                        lbSimpleCambio.Text = "Ingrese el nuevo número de teléfono";
                    break;
                    case 4:
                        lbSimpleCambio.Text = "Ingrese el nuevo correo electrónico";
                    break;                    

                }
            }

        }
        #endregion

        #region COMPORTAMIENTO DE TXTBOX
        private void txtbModificacion_Enter(object sender, EventArgs e)
        {
            if(txtbModificacion.Text=="Modificacion")
            {
                txtbModificacion.Text = "";
            }
        }

        private void txtbModificacion_Leave(object sender, EventArgs e)
        {
            if(txtbModificacion.Text=="")
            {
                txtbModificacion.Text = "Modificacion";
            }
        }
        #endregion

        #region COMPORTAMIENTO DE COMBOBOXES
        private void FormModifica_Load(object sender, EventArgs e)
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

            //FECHA DE MODIFICACIÓN
            if(tipoCambio==1)
            {
                for (int i = 2020; i < 2026; i++)
                {
                    comboAño2.Items.Add(i);
                }
                for (int i = 0; i < 12; i++)
                {
                    comboMes2.Items.Add((i + 1).ToString("00"));
                }
                for (int i = 0; i < 24; i++)
                {
                    comboHora2.Items.Add((i + 1).ToString("00"));
                }
                for (int i = 0; i < 1; i++)
                {
                    comboMin2.Items.Add(0.ToString("00"));
                    comboMin2.Items.Add(30);
                }
            }
        }
        private void comboAño_SelectedIndexChanged(object sender, EventArgs e)
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

        private void comboMes_SelectedIndexChanged(object sender, EventArgs e)
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

        private void comboMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboAño2.Enabled = true;
        }

        //COMBOBOX DE FECHA DE MODIFICACIÓN
        private void comboAño2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboHora2.Enabled = false;
            comboMin2.Enabled = false;
            comboDia2.Text = "Día";
            comboMes2.Enabled = true;
            if (mesSelect2 == 2)
            {
                comboMes2.SelectedIndex = 2;
                comboMes2.SelectedIndex = 1;
            }
        }

        private void comboMes2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboDia2.Items.Clear();
            comboHora2.Enabled = false;
            comboMin2.Enabled = false;
            comboDia2.Text = "Día";
            mesSelect2 = int.Parse(comboMes2.Text);
            añoSelect2 = int.Parse(comboAño2.Text);

            switch (mesSelect2)
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
                    if (añoSelect2 % 4 == 0)
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

                comboDia2.Items.Add((i + 1).ToString("00"));
            }
            comboDia2.Enabled = true;
        }

        private void comboDia2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboHora2.Enabled = true;
        }

        private void comboHora2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMin2.Enabled = true;
        }


        #endregion

        #region MODIFICACIÓN 
        private async void btnConfirmarCambio_Click(object sender, EventArgs e)
        {

            client = new FireSharp.FirebaseClient(config);
            var cita = new Cita
            {
                citaId = long.Parse(comboAño.Text + comboMes.Text + comboDia.Text + comboHora.Text + comboMin.Text)
            };
            //Sección de recuperación
            FirebaseResponse recuperacion = await client.GetTaskAsync("PACIENTES/" +cita.citaId);
            Cita obj = recuperacion.ResultAs<Cita>();
            switch (tipoCambio)
            {
                case 1: //CAMBIO DE FECHA
                    //Sección de insersión 
                    var citaModificada = new Cita
                    {
                        day = comboDia2.Text,
                        mounth = comboMes2.Text,
                        year = comboAño2.Text,
                        hora = comboHora2.Text,
                        minuto = comboMin2.Text,
                        citaId = long.Parse(comboAño2.Text + comboMes2.Text + comboDia2.Text + comboHora2.Text + comboMin2.Text),
                        nombrePaciente = obj.nombrePaciente,
                        telefonoPciente = obj.telefonoPciente,
                        correoPaciente = obj.correoPaciente                      
                    };
                    SetResponse response = await client.SetTaskAsync("PACIENTES/"+citaModificada.citaId,citaModificada);
                    FirebaseResponse eliminar = await client.DeleteTaskAsync("PACIENTES/" + obj.citaId);
                    this.Close();
                    break;

                case 2: //CAMBIO DE NOMBRE
                    var citaModificada2 = new Cita
                    {
                        day = obj.day,
                        mounth = obj.mounth,
                        year = obj.year,
                        hora = obj.hora,
                        minuto = obj.minuto,
                        citaId = obj.citaId,
                        nombrePaciente = txtbModificacion.Text,
                        telefonoPciente = obj.telefonoPciente,
                        correoPaciente = obj.correoPaciente
                    };
                    FirebaseResponse response2 = await client.UpdateTaskAsync("PACIENTES/" + cita.citaId, citaModificada2);
                    this.Close();
                    break;

                case 3:  //CAMBIO DE TELÉFONO
                    var citaModificada3 = new Cita
                    {
                        day = obj.day,
                        mounth = obj.mounth,
                        year = obj.year,
                        hora = obj.hora,
                        minuto = obj.minuto,
                        citaId = obj.citaId,
                        nombrePaciente = obj.nombrePaciente,
                        telefonoPciente = txtbModificacion.Text,
                        correoPaciente = obj.correoPaciente
                    };
                    FirebaseResponse response3 = await client.UpdateTaskAsync("PACIENTES/" + cita.citaId, citaModificada3);
                    Cita resultado3 = response3.ResultAs<Cita>();
                    MessageBox.Show("Modificación realizada en" + resultado3.citaId, "BASE DE DATOS", MessageBoxButtons.OK);
                    this.Close();
                    break;

                case 4:  //CAMBIO DE CORREO
                    var citaModificada4 = new Cita
                    {
                        day = obj.day,
                        mounth = obj.mounth,
                        year = obj.year,
                        hora = obj.hora,
                        minuto = obj.minuto,
                        citaId = obj.citaId,
                        nombrePaciente = obj.nombrePaciente,
                        telefonoPciente = obj.telefonoPciente,
                        correoPaciente = txtbModificacion.Text
                    };
                    FirebaseResponse response4 = await client.UpdateTaskAsync("PACIENTES/" + cita.citaId, citaModificada4);
                    Cita resultado4 = response4.ResultAs<Cita>();
                    this.Close();
                    break;
            }


        }
        #endregion

        #region BOTONES DE REGRESO
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
        #endregion

    }
}
