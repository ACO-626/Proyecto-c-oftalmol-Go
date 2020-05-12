#region BIBLIOTECAS
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Media;

//BIBLIOTECAS PARA BD
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

#endregion

namespace Oftalmol_Go
{
    public partial class FormMenu : Form
    {

        #region VARIABLES Y COLECCIONES
        string nnombre;
        string año;
        string mes;
        string dia;
        string min;
        string hora;
        int registros;
        int iReg;
        int contadorAtras = 0;
        int contadorAdelante = 0;
        int navegante;
        int aux;
        int estado;
        long idcomparer;

        bool rootMode;
        List<long> direcciones = new List<long>();
        Dictionary<string, Cita> citas = new Dictionary<string, Cita>();

        #endregion

        #region INICIALIZACIÓN DE VENTANA
        public FormMenu(string nombre, bool rootmode)
        {
            InitializeComponent();            
            SoundPlayer player = new SoundPlayer();
            try
            {
                player.SoundLocation = @"..\..\SOUND\1.wav";
                player.Play();
            }
            catch(Exception)
            {
                MessageBox.Show("Faltan Archivos de sonido, error: 1001");
            }            
            nnombre = nombre;
            rootMode = rootmode;
            if(rootmode==true)
            {
                btnMenu.Visible = true;
            }

        }
        private void FormMenu_Load(object sender, EventArgs e)
        {
            lbSimpleNombre.Text = nnombre; 
            this.ActualizarAll();
            
        }
        #endregion

        #region INSTANCIAS PARA BD
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "608vd6SDwtR7uGhPNhd9j7bbgL0E2mR0xbhyOwSk",
            BasePath = "https://oftalmol-godb.firebaseio.com/"

        };
        IFirebaseClient client;
        #endregion

        #region BOTONES
        private void btnAgendarCita_Click(object sender, EventArgs e)
        {
            Form ventanaAgendar = new FormCrearCita();
            ventanaAgendar.Show();
        }

        public void btnReagendarCita_Click(object sender, EventArgs e)
        {
            Form VentanaModificarCita = new FormModificarCita();
            VentanaModificarCita.Show();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            Form VentanaConsulta = new FormConsulta();
            VentanaConsulta.Show();
        }
        private void btnAgenda_Click(object sender, EventArgs e)
        {
            Form VentanaAgenda = new FormAgenda();
            VentanaAgenda.Show();
        }
        private async void btnAfter_Click(object sender, EventArgs e)
        {
            this.Actualizar();
            MessageBox.Show("Boton adelante");
            if(contadorAdelante == registros-iReg-1)
            {
                MessageBox.Show("No hago nada");
            }
            else
            {               
                contadorAdelante++;
                contadorAtras--;
                navegante = iReg + contadorAdelante;
                try
                {
                    MessageBox.Show("navegante:"+navegante.ToString()+" contador ad"+contadorAdelante.ToString()+"cont atra:"+contadorAtras);
                    FirebaseResponse response = await client.GetTaskAsync("PACIENTES/"+direcciones[navegante]);
                    if(response.Body!=null)
                    {
                        Cita obj = response.ResultAs<Cita>();
                        lbPaciente.Text = obj.nombrePaciente;
                        aux = navegante - iReg;
                        lbSimpleEstado.Text = ("Lugar " + aux.ToString());
                        lbTel.Text = obj.telefonoPciente;
                        lbCorreo.Text = obj.correoPaciente;
                        lbDia.Text = obj.day;
                        lbAño.Text = obj.year;
                        lbMes.Text = obj.mounth;
                        estado--;
                    }
                    else
                    {
                        ActualizarAll();
                        contadorAdelante = 0;
                        contadorAtras = 0;
                    }
                }
                catch
                {
                    ActualizarAll();
                    contadorAdelante = 0;
                }
            }
            
        }

        private async void btnBefore_Click(object sender, EventArgs e)
        {
            this.Actualizar();
            MessageBox.Show("Botón atrás");
            if (contadorAtras >= iReg)
            {

                MessageBox.Show("NO HAGO NADA");
            }
            else
            {
                //MessageBox.Show("yA");
                contadorAtras++;
                contadorAdelante--;
                //MessageBox.Show(iReg.ToString());
                navegante = iReg - contadorAtras;                
                try
                {
                    //MessageBox.Show(navegante.ToString());
                    FirebaseResponse response = await client.GetTaskAsync("PACIENTES/" + direcciones[navegante]);
                    if (response.Body != null)
                    {
                        Cita obj = response.ResultAs<Cita>();
                        lbPaciente.Text = obj.nombrePaciente;
                        aux = navegante - iReg;
                        lbSimpleEstado.Text = ("Lugar " + aux.ToString());
                        lbTel.Text = obj.telefonoPciente;
                        lbCorreo.Text = obj.correoPaciente;
                        lbDia.Text = obj.day;
                        lbAño.Text = obj.year;
                        lbMes.Text = obj.mounth;
                        estado--;
                    }
                    else
                    {
                        ActualizarAll();
                        contadorAtras = 0;
                    }
                }
                catch(Exception)
                {
                    ActualizarAll();
                    contadorAtras = 0;

                };
                
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            this.ActualizarAll();
            contadorAtras = 0;
        }
        #endregion

        #region TIEMPO
        private void TmHoraR_Tick(object sender, EventArgs e)
        {
            lbRealHora.Text = DateTime.Now.ToString("HH:mm:ss");
            lbFechaReal.Text = DateTime.Now.ToString("d/M/yyyy");
        }
        #endregion

        #region MÉTODO ACTUALIZAR TODO
        public async void ActualizarAll()
        {
            client = new FireSharp.FirebaseClient(config);

            año = DateTime.Now.ToString("yyyy");
            mes = DateTime.Now.ToString("MM");
            dia = DateTime.Now.ToString("dd");
            hora = DateTime.Now.ToString("HH");
            min = DateTime.Now.ToString("mm");
            idcomparer = long.Parse(año + mes + dia + hora + min);
            citas.Clear();
            direcciones.Clear();

            var data = await client.GetTaskAsync("PACIENTES");

            if (data.Body != "null")
            {
                btnBefore.Enabled = true;
                btnAfter.Enabled = true;

                citas = data.ResultAs<Dictionary<string, Cita>>();

                foreach (var Cita in citas)
                {
                    direcciones.Add(Cita.Value.citaId);
                }
                registros = direcciones.Count;
                iReg = registros - 1;
                for (int i = 0; i < registros; i++)
                {
                    if (direcciones[iReg] < idcomparer)
                    {
                        break;
                    }
                    else if (direcciones[iReg] > idcomparer)
                    {
                        iReg--;
                    }
                    else
                    {
                        iReg--;
                        break;
                    }
                }
                if (iReg + 1 != registros)
                {
                    iReg++;//Ya tienes el paciente a mostrar
                    FirebaseResponse recuperacion = await client.GetTaskAsync("PACIENTES/" + direcciones[iReg]);
                    Cita obj = recuperacion.ResultAs<Cita>();
                    #region ETIQUETAS INCIIALES
                    lbSimpleEstado.Text = "PROXIMO PACIENTE";
                    lbPaciente.Text = obj.nombrePaciente;
                    lbAño.Text = obj.year;
                    lbMes.Text = obj.mounth;
                    lbDia.Text = obj.day;
                    lbEdad.Text = obj.edad;
                    lbHora.Text = obj.hora;
                    lbMin.Text = obj.minuto;
                    lbTel.Text = obj.telefonoPciente;
                    lbCorreo.Text = obj.correoPaciente;
                    #endregion
                    /*if (iReg + 1 == registros)
                    {
                        
                        btnAfter.Enabled = false;
                        if (navegante == 0)
                        {
                            
                            btnBefore.Enabled = false;
                        }
                        else
                        {
                            btnBefore.Enabled = true;
                        }
                    }
                    else if (navegante != 0)
                    {
                        
                        btnBefore.Enabled = true;
                        btnAfter.Enabled = true;
                    }
                    else
                    {
                        
                        btnBefore.Enabled = false;
                    }
                    */

                }
                else
                {
                    iReg++;
                    //btnAfter.Enabled = true;
                    //btnBefore.Enabled = true;

                    

                    lbSimpleEstado.Text = "SIN PRÓXIMA CONSULTA";
                    lbAño.Text = "--";
                    lbMes.Text = "--";
                    lbDia.Text = "--";
                    lbHora.Text = "--";
                    lbMin.Text = "--";
                    lbCorreo.Text = "";
                    lbTel.Text = "";
                    lbEdad.Text = "";
                    lbPaciente.Text = "Hola, todo listo para registrar";
                }


            }
            else //EN EL CASO DE QUE NO HAYA REGISTROS EN LA BD
            {                
                iReg = 0;
                contadorAdelante = 10;
                btnAfter.Enabled = false;
                btnBefore.Enabled = false;
            }
            //El paciente a mostrar será
        }

        #endregion

        #region ACTUALIZAR
        public async void Actualizar()
        {
            client = new FireSharp.FirebaseClient(config);

            año = DateTime.Now.ToString("yyyy");
            mes = DateTime.Now.ToString("MM");
            dia = DateTime.Now.ToString("dd");
            hora = DateTime.Now.ToString("HH");
            min = DateTime.Now.ToString("mm");
            idcomparer = long.Parse(año + mes + dia + hora + min);
            citas.Clear();
            direcciones.Clear();

            var data = await client.GetTaskAsync("PACIENTES");

            if (data.Body != "null")
            {
                btnAfter.Enabled = true;
                btnBefore.Enabled = true;
                citas = data.ResultAs<Dictionary<string, Cita>>();

                foreach (var Cita in citas)
                {
                    direcciones.Add(Cita.Value.citaId);
                }
                registros = direcciones.Count;
                iReg = registros - 1;
                for (int i = 0; i < registros; i++)
                {
                    if (direcciones[iReg] < idcomparer)
                    {
                        break;
                    }
                    else if (direcciones[iReg] > idcomparer)
                    {
                        iReg--;
                    }
                    else
                    {
                        iReg--;
                        break;
                    }
                }
                if (iReg + 1 != registros)
                {
                    iReg++;//Ya tienes el paciente a mostrar
                    
                    //MARCA
                    /*if (navegante + 1 == registros)
                    {

                        btnAfter.Enabled = false;
                        if (navegante <= 0)
                        {

                            btnBefore.Enabled = false;
                        }
                        else
                        {
                            btnBefore.Enabled = true;
                        }
                    }
                    else if (navegante != 0)
                    {

                        btnBefore.Enabled = true;
                        btnAfter.Enabled = true;
                    }
                    else
                    {

                        btnBefore.Enabled = false;
                    }*/


                }
                else
                {
                    
                    //btnAfter.Enabled = false;
                    //btnBefore.Enabled = true;
                    iReg++;
                }


            }
            else //EN EL CASO DE QUE NO HAYA REGISTROS EN LA BD
            {
                iReg = 0;
                
                btnAfter.Enabled = false;
                btnBefore.Enabled = false;
            }
            //El paciente a mostrar será
        }
        #endregion

        #region CERRADO DE VENTANA
        private void FormMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }



        #endregion

        private void btnCerrarSesión_Click(object sender, EventArgs e)
        {         
            Form VentanaMensaje = new FormMensaje("Confirmación para cerrar sesión","MENSAJE","Confirmar",true,1);
            VentanaMensaje.Show();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Puedes encontrar el manual de usuario aquí: https://www.youtube.com/watch?v=CUdRuml6efA o contactarnos nuestro correro oftalmolgo@oftal.com para soporte técnico", "AYUDA", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Close();
            Form ventanaAdminMenu = new FormAdminMenu(nnombre,rootMode);
            ventanaAdminMenu.Show();
        }
    }

}

