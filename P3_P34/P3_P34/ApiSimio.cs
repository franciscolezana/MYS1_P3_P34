using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//IMPORTS
using SimioAPI;
using Simio.SimioEnums;
using SimioAPI.Extensions;
using SimioAPI.Graphics;
using Simio;
using System.Windows.Forms;

namespace P3_P34
{
    class ApiSimio
    {
        private ISimioProject proyectoApi;
        private string rutabase = "Modelobase.spfx";
        private string rutafinal = "ModeloFinal.spfx";
        private string[] warnings;
        private IModel model;
        private IIntelligentObjects intelligentObjects;
        int ContadorPath = 1, ContadorServer = 1, ContadorSource = 1, ContadorSink = 1, ContadorPathSimple = 1, ContadorTimepath = 1, ContadorConveyor = 1, ContadorSeparator = 1, ContadorCombiner = 1;
        int ContadorTransferNode = 1, contAux = 1, ContadorBasicN = 1;
        private SimioAPI.FacilitySize tamanioAvionMilitar;
        public ApiSimio()
        {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            model = proyectoApi.Models[1];
            intelligentObjects = model.Facility.IntelligentObjects;

        }


        public void crearModelo()
        {


            this.crearRegiones();
            this.crearMapa();
            this.unirMapa();
            this.tamanioFronteras();
            this.velocidadAvion();
            this.puntosCardinales();
            this.comportamientoTurista();

            //CREACION MODELO
            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
            MessageBox.Show("El proyecto Simio ha sido generado");
            Console.WriteLine("Modelo Creado");

            //Console.WriteLine("Error al crear modelo");

        }

        public void crearRegiones()
        {
            #region Regiones
            intelligentObjects.CreateObject("Server", new FacilityLocation(0, 0, 0));  //Metropolitana
            intelligentObjects.CreateObject("Server", new FacilityLocation(5, 0, -20));  //Norte
            intelligentObjects.CreateObject("Server", new FacilityLocation(30, 0, -10));  //Nor-Oriente
            intelligentObjects.CreateObject("Server", new FacilityLocation(15, 0, 20));  //Sur-Oriente
            intelligentObjects.CreateObject("Server", new FacilityLocation(-15, 0, 20));  //Central
            intelligentObjects.CreateObject("Server", new FacilityLocation(-40, 0, 10));  //Sur-Occidente   
            intelligentObjects.CreateObject("Server", new FacilityLocation(-40, 0, -20));  //Nor-Occidente         
            intelligentObjects.CreateObject("Server", new FacilityLocation(10, 0, -50));  //Peten

            model.Facility.IntelligentObjects["Server1"].ObjectName = "Metropolitana";
            model.Facility.IntelligentObjects["Server2"].ObjectName = "Norte";
            model.Facility.IntelligentObjects["Server3"].ObjectName = "NorOriente";
            model.Facility.IntelligentObjects["Server4"].ObjectName = "SurOriente";
            model.Facility.IntelligentObjects["Server5"].ObjectName = "Central";
            model.Facility.IntelligentObjects["Server6"].ObjectName = "SurOccidente";
            model.Facility.IntelligentObjects["Server7"].ObjectName = "NorOccidente";
            model.Facility.IntelligentObjects["Server8"].ObjectName = "Peten";
            #endregion

            #region ENTIDAD
            intelligentObjects.CreateObject("ModelEntity", new FacilityLocation(-8, 0, -64));
            intelligentObjects.CreateObject("ModelEntity", new FacilityLocation(0, 0, 0));
            #endregion

            //FUERZAS ARMADAS
            intelligentObjects.CreateObject("Source", new FacilityLocation(-8, 0, -66));
            intelligentObjects.CreateObject("Sink", new FacilityLocation(-8, 0, -66));

            #region Sink&SourceCrear
            //Metropolitana
            intelligentObjects.CreateObject("Source", new FacilityLocation(0, 0, 0));
            intelligentObjects.CreateObject("Sink", new FacilityLocation(0, 0, 0));
            //NORTE
            intelligentObjects.CreateObject("Source", new FacilityLocation(5, 0, -20));  //Norte
            intelligentObjects.CreateObject("Sink", new FacilityLocation(5, 0, -20));  //Norte
            //NOR ORIENTE
            intelligentObjects.CreateObject("Source", new FacilityLocation(30, 0, -10));  //Nor-Oriente
            intelligentObjects.CreateObject("Sink", new FacilityLocation(30, 0, -10));  //Nor-Oriente
            //SUR ORIENTE
            intelligentObjects.CreateObject("Source", new FacilityLocation(15, 0, 20));  //Sur-Oriente
            intelligentObjects.CreateObject("Sink", new FacilityLocation(15, 0, 20));  //Sur-Oriente
            //CENTRAL
            intelligentObjects.CreateObject("Source", new FacilityLocation(-15, 0, 20));  //Central
            intelligentObjects.CreateObject("Sink", new FacilityLocation(-15, 0, 20));  //Central
            //SUR OCCIDENTE
            intelligentObjects.CreateObject("Source", new FacilityLocation(-40, 0, 10));  //Sur-Occidente   
            intelligentObjects.CreateObject("Sink", new FacilityLocation(-40, 0, 10));  //Sur-Occidente   
            //NOR OCCIDENTE
            intelligentObjects.CreateObject("Source", new FacilityLocation(-40, 0, -20));  //Nor-Occidente         
            intelligentObjects.CreateObject("Sink", new FacilityLocation(-40, 0, -20));  //Nor-Occidente         
            //PETEN
            intelligentObjects.CreateObject("Source", new FacilityLocation(10, 0, -50));  //Peten
            intelligentObjects.CreateObject("Sink", new FacilityLocation(10, 0, -50));  //Peten
            #endregion

            //ENTIDAD
            model.Facility.IntelligentObjects["ModelEntity1"].ObjectName = "AvionMilitar";
            model.Facility.IntelligentObjects["ModelEntity2"].ObjectName = "Turista";

            #region NombreSink&Source
            //FUERZAS ARMADAS
            model.Facility.IntelligentObjects["Source1"].ObjectName = "SalidaFuerzaArmada";
            //model.Facility.IntelligentObjects["Sink1"].ObjectName = "FuerzaArmada";
            //METROPOLITANA
            model.Facility.IntelligentObjects["Source2"].ObjectName = "MetropolitanaE";
            model.Facility.IntelligentObjects["Sink2"].ObjectName = "MetropolitanaI";
            //NORTE
            model.Facility.IntelligentObjects["Source3"].ObjectName = "NorteE";
            model.Facility.IntelligentObjects["Sink3"].ObjectName = "NorteI";
            //NOR ORIENTE
            model.Facility.IntelligentObjects["Source4"].ObjectName = "NorOrienteE";
            model.Facility.IntelligentObjects["Sink4"].ObjectName = "NorOrienteI";
            //SUR ORIENTE
            model.Facility.IntelligentObjects["Source5"].ObjectName = "SurOrienteE";
            model.Facility.IntelligentObjects["Sink5"].ObjectName = "SurOrienteI";
            //CENTRAL
            model.Facility.IntelligentObjects["Source6"].ObjectName = "CentralE";
            model.Facility.IntelligentObjects["Sink6"].ObjectName = "CentralI";
            //SUR OCCIDENTE
            model.Facility.IntelligentObjects["Source7"].ObjectName = "SurOccidenteE";
            model.Facility.IntelligentObjects["Sink7"].ObjectName = "SurOccidenteI";
            //NOR OCCIDENTE
            model.Facility.IntelligentObjects["Source8"].ObjectName = "NorOccidenteE";
            model.Facility.IntelligentObjects["Sink8"].ObjectName = "NorOccidenteI";
            //PETEN
            model.Facility.IntelligentObjects["Source9"].ObjectName = "PetenE";
            model.Facility.IntelligentObjects["Sink9"].ObjectName = "PetenI";
            #endregion
        }

        public void crearMapa()
        {
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-20, 0, -73));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(33, 0, -73));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(33, 0, -20));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(49, 0, -20));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(40, 0, -10));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(48, 0, -11));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(50, 0, -20));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(56, 0, -16));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(58, 0, -18));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(56, 0, -22)); //10
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(64, 0, -14));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(38, 0, 12));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(40, 0, 19));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(30, 0, 25));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(21, 0, 40));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(1, 0, 34));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-23, 0, 36));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-57, 0, 21));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-58, 0, 4));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-53, 0, 2));//20
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-59, 0, -5));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-50, 0, -26));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-9, 0, -26));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-8, 0, -32));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-13, 0, -35));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-14, 0, -41));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-21, 0, -46));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-28, 0, -48));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-35, 0, -54));
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-20, 0, -54));
            //ULTIMO NODO
            //intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-20, 0, -73));
            //ContadorTransferNode++;
        }

        public void RenombrarTransfer()
        {
            for (int i = 1; i < 31; i++)
            {
                model.Facility.IntelligentObjects["TransferNode" + i].ObjectName = "T" + i;
            }
        }

        public void unirMapa()
        {
            this.RenombrarTransfer();
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T1"]), ((INodeObject)model.Facility.IntelligentObjects["T2"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T2"]), ((INodeObject)model.Facility.IntelligentObjects["T3"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T3"]), ((INodeObject)model.Facility.IntelligentObjects["T4"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T4"]), ((INodeObject)model.Facility.IntelligentObjects["T5"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T5"]), ((INodeObject)model.Facility.IntelligentObjects["T6"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T6"]), ((INodeObject)model.Facility.IntelligentObjects["T7"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T7"]), ((INodeObject)model.Facility.IntelligentObjects["T8"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T8"]), ((INodeObject)model.Facility.IntelligentObjects["T9"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T9"]), ((INodeObject)model.Facility.IntelligentObjects["T10"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T10"]), ((INodeObject)model.Facility.IntelligentObjects["T11"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T11"]), ((INodeObject)model.Facility.IntelligentObjects["T12"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T12"]), ((INodeObject)model.Facility.IntelligentObjects["T13"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T13"]), ((INodeObject)model.Facility.IntelligentObjects["T14"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T14"]), ((INodeObject)model.Facility.IntelligentObjects["T15"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T15"]), ((INodeObject)model.Facility.IntelligentObjects["T16"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T16"]), ((INodeObject)model.Facility.IntelligentObjects["T17"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T17"]), ((INodeObject)model.Facility.IntelligentObjects["T18"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T18"]), ((INodeObject)model.Facility.IntelligentObjects["T19"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T19"]), ((INodeObject)model.Facility.IntelligentObjects["T20"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T20"]), ((INodeObject)model.Facility.IntelligentObjects["T21"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T21"]), ((INodeObject)model.Facility.IntelligentObjects["T22"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T22"]), ((INodeObject)model.Facility.IntelligentObjects["T23"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T23"]), ((INodeObject)model.Facility.IntelligentObjects["T24"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T24"]), ((INodeObject)model.Facility.IntelligentObjects["T25"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T25"]), ((INodeObject)model.Facility.IntelligentObjects["T26"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T26"]), ((INodeObject)model.Facility.IntelligentObjects["T27"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T27"]), ((INodeObject)model.Facility.IntelligentObjects["T28"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T28"]), ((INodeObject)model.Facility.IntelligentObjects["T29"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T29"]), ((INodeObject)model.Facility.IntelligentObjects["T30"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["T30"]), ((INodeObject)model.Facility.IntelligentObjects["T1"]), null);

            //FUERZAS ARMADAS
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SalidaFuerzaArmada"]).Nodes[0], (INodeObject)model.Facility.IntelligentObjects["T1"], null);
            //intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode31"]), ((IFixedObject)model.Facility.IntelligentObjects["FuerzaArmada"]).Nodes[0], null);
            //SOURCE FUERZA ARMADA
            model.Facility.IntelligentObjects["SalidaFuerzaArmada"].Properties["InterarrivalTime"].Value = "Random.Exponential(15)";
            model.Facility.IntelligentObjects["SalidaFuerzaArmada"].Properties["MaximumArrivals"].Value = "15";
            model.Facility.IntelligentObjects["SalidaFuerzaArmada"].Properties["EntityType"].Value = "AvionMilitar";
            // SET TAMAÑO ENTIDAD
            tamanioAvionMilitar.Length = 3;
            tamanioAvionMilitar.Height = 3;
            tamanioAvionMilitar.Width = 3;
            model.Facility.IntelligentObjects["AvionMilitar"].Size = tamanioAvionMilitar;

        }

        public void tamanioFronteras()
        {
            String con = "Conveyor";
            for (int i = 1; i < 31; i++)
            {
                con = "Conveyor" + i;
                model.Facility.IntelligentObjects[con].Properties["DrawnToScale"].Value = "False";
            }

            // Frontera con BELICE 266km
            model.Facility.IntelligentObjects["Conveyor2"].Properties["LogicalLength"].Value = "250000";
            model.Facility.IntelligentObjects["Conveyor3"].Properties["LogicalLength"].Value = "16000";
            // Frontera con el caribe 148km
            model.Facility.IntelligentObjects["Conveyor4"].Properties["LogicalLength"].Value = "28000";
            model.Facility.IntelligentObjects["Conveyor5"].Properties["LogicalLength"].Value = "28000";
            model.Facility.IntelligentObjects["Conveyor6"].Properties["LogicalLength"].Value = "28000";
            model.Facility.IntelligentObjects["Conveyor7"].Properties["LogicalLength"].Value = "16000";
            model.Facility.IntelligentObjects["Conveyor8"].Properties["LogicalLength"].Value = "14000";
            model.Facility.IntelligentObjects["Conveyor9"].Properties["LogicalLength"].Value = "6000";
            model.Facility.IntelligentObjects["Conveyor10"].Properties["LogicalLength"].Value = "28000";
            // Frontera con Honduras 256km
            model.Facility.IntelligentObjects["Conveyor11"].Properties["LogicalLength"].Value = "240000";
            model.Facility.IntelligentObjects["Conveyor12"].Properties["LogicalLength"].Value = "16000";
            // Frontera con El Salvador 203km
            model.Facility.IntelligentObjects["Conveyor13"].Properties["LogicalLength"].Value = "53000";
            model.Facility.IntelligentObjects["Conveyor14"].Properties["LogicalLength"].Value = "150000";
            // Frontera con el Pacifico 254km
            model.Facility.IntelligentObjects["Conveyor15"].Properties["LogicalLength"].Value = "88000";
            model.Facility.IntelligentObjects["Conveyor16"].Properties["LogicalLength"].Value = "84000";
            model.Facility.IntelligentObjects["Conveyor17"].Properties["LogicalLength"].Value = "82000";
            //Frontera con Mexico 962km 240 240 241 241
            model.Facility.IntelligentObjects["Conveyor1"].Properties["LogicalLength"].Value = "252000"; //252
            model.Facility.IntelligentObjects["Conveyor18"].Properties["LogicalLength"].Value = "84000"; //220
            model.Facility.IntelligentObjects["Conveyor19"].Properties["LogicalLength"].Value = "22000";
            model.Facility.IntelligentObjects["Conveyor20"].Properties["LogicalLength"].Value = "28000";
            model.Facility.IntelligentObjects["Conveyor21"].Properties["LogicalLength"].Value = "86000";
            model.Facility.IntelligentObjects["Conveyor22"].Properties["LogicalLength"].Value = "230000"; //230
            model.Facility.IntelligentObjects["Conveyor23"].Properties["LogicalLength"].Value = "25000";  //260
            model.Facility.IntelligentObjects["Conveyor24"].Properties["LogicalLength"].Value = "25000";
            model.Facility.IntelligentObjects["Conveyor25"].Properties["LogicalLength"].Value = "25000";
            model.Facility.IntelligentObjects["Conveyor26"].Properties["LogicalLength"].Value = "25000";
            model.Facility.IntelligentObjects["Conveyor27"].Properties["LogicalLength"].Value = "25000";
            model.Facility.IntelligentObjects["Conveyor28"].Properties["LogicalLength"].Value = "25000";
            model.Facility.IntelligentObjects["Conveyor29"].Properties["LogicalLength"].Value = "40000";
            model.Facility.IntelligentObjects["Conveyor30"].Properties["LogicalLength"].Value = "70000";
        }

        public void velocidadAvion()
        {
            String con = "";
            for (int i = 1; i < 31; i++)
            {
                con = "Conveyor" + i;
                model.Facility.IntelligentObjects[con].Properties["InitialDesiredSpeed"].Value = "16.6667"; //"MetersPerSecond"
            }
        }

        public void puntosCardinales()
        {
            intelligentObjects.CreateObject("BasicNode", new FacilityLocation(0, 0, -100));
            intelligentObjects.CreateObject("BasicNode", new FacilityLocation(0, 0, 70));
            intelligentObjects.CreateObject("BasicNode", new FacilityLocation(-93, 0, 0));
            intelligentObjects.CreateObject("BasicNode", new FacilityLocation(90, 0, 0));

            model.Facility.IntelligentObjects["BasicNode1"].ObjectName = "Norte1";
            model.Facility.IntelligentObjects["BasicNode2"].ObjectName = "Sur";
            model.Facility.IntelligentObjects["BasicNode3"].ObjectName = "Oeste";
            model.Facility.IntelligentObjects["BasicNode4"].ObjectName = "Este";


        }

        public void comportamientoTurista()
        {
            #region salidaSource
            model.Facility.IntelligentObjects["MetropolitanaE"].Properties["InterarrivalTime"].Value = "Random.Poisson(2)";
            model.Facility.IntelligentObjects["NorteE"].Properties["InterarrivalTime"].Value = "Random.Poisson(8)";
            model.Facility.IntelligentObjects["NorOrienteE"].Properties["InterarrivalTime"].Value = "Random.Poisson(6)";
            model.Facility.IntelligentObjects["SurOrienteE"].Properties["InterarrivalTime"].Value = "Random.Poisson(10)";
            model.Facility.IntelligentObjects["CentralE"].Properties["InterarrivalTime"].Value = "Random.Poisson(3)";
            model.Facility.IntelligentObjects["SurOccidenteE"].Properties["InterarrivalTime"].Value = "Random.Poisson(4)";
            model.Facility.IntelligentObjects["NorOccidenteE"].Properties["InterarrivalTime"].Value = "Random.Poisson(12)";
            model.Facility.IntelligentObjects["PetenE"].Properties["InterarrivalTime"].Value = "Random.Poisson(4)";

            model.Facility.IntelligentObjects["MetropolitanaE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["NorteE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["NorOrienteE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["SurOrienteE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["CentralE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["SurOccidenteE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["NorOccidenteE"].Properties["EntityType"].Value = "Turista";
            model.Facility.IntelligentObjects["PetenE"].Properties["EntityType"].Value = "Turista";
            #endregion

            #region atencionServidor
            model.Facility.IntelligentObjects["Metropolitana"].Properties["ProcessingTime"].Value = "Random.Exponential(4)";
            model.Facility.IntelligentObjects["Metropolitana"].Properties["InitialCapacity"].Value = "200";

            model.Facility.IntelligentObjects["Norte"].Properties["ProcessingTime"].Value = "Random.Exponential(5)";
            model.Facility.IntelligentObjects["Norte"].Properties["InitialCapacity"].Value = "50";

            model.Facility.IntelligentObjects["NorOriente"].Properties["ProcessingTime"].Value = "Random.Exponential(3)";
            model.Facility.IntelligentObjects["NorOriente"].Properties["InitialCapacity"].Value = "40";

            model.Facility.IntelligentObjects["SurOriente"].Properties["ProcessingTime"].Value = "Random.Exponential(4)";
            model.Facility.IntelligentObjects["SurOriente"].Properties["InitialCapacity"].Value = "30";

            model.Facility.IntelligentObjects["Central"].Properties["ProcessingTime"].Value = "Random.Exponential(5)";
            model.Facility.IntelligentObjects["Central"].Properties["InitialCapacity"].Value = "100";

            model.Facility.IntelligentObjects["SurOccidente"].Properties["ProcessingTime"].Value = "Random.Exponential(3)";
            model.Facility.IntelligentObjects["SurOccidente"].Properties["InitialCapacity"].Value = "120";

            model.Facility.IntelligentObjects["NorOccidente"].Properties["ProcessingTime"].Value = "Random.Exponential(6)";
            model.Facility.IntelligentObjects["NorOccidente"].Properties["InitialCapacity"].Value = "30";

            model.Facility.IntelligentObjects["Peten"].Properties["ProcessingTime"].Value = "Random.Exponential(4)";
            model.Facility.IntelligentObjects["Peten"].Properties["InitialCapacity"].Value = "150";
            #endregion

            this.rutasTurista();
        }

        public void rutasTurista()
        {
            //---Salidas desde los source
            #region Metropolitana32-35
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);


            #endregion
            #region Norte36-39
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            #endregion
            #region Nor-Oriente40-44
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            #endregion
            #region Sur-Oriente45-48
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            #endregion
            #region Central49-53
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["CentralI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            #endregion
            #region Sur-Occidente54-56
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            #endregion
            #region Nor-Occidente57-60
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            #endregion
            #region Peten61-63
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["PetenE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["PetenI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["PetenE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["PetenE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            #endregion
            //---Salidas desde los server
            #region Metropolitana64-67
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            #endregion
            #region Norte68-71
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            #endregion
            #region Nor-Oriente72-76
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            #endregion
            #region Sur-Oriente77-80
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            #endregion
            #region Central81-85
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["CentralI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            #endregion
            #region Sur-Occidente86-88
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            #endregion
            #region Nor-Occidente89-92
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            #endregion
            #region Peten93-95
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["PetenI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            #endregion
            //---TODOS LOS CONVEYOR POR PESO
            this.porPeso();
            this.colocarDistanciaPeso();
        }

        public void porPeso()
        {
            //--Servidores
            ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[1].Properties["OutboundLinkRule"].Value = "By Link Weight";
            //--Source
            ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["NorteE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["PetenE"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
        }

        public void colocarDistanciaPeso()
        {
            String con = "Conveyor";
            for (int i = 32; i < 96; i++)
            {
                con = "Conveyor" + i;
                model.Facility.IntelligentObjects[con].Properties["DrawnToScale"].Value = "False";
            }
            #region SalidasSource
            //--Server32-63
            llenarPath("Conveyor32", "0.35", "0");
            llenarPath("Conveyor33", "0.30", "63");
            llenarPath("Conveyor34", "0.15", "124");
            llenarPath("Conveyor35", "0.20", "241");
            llenarPath("Conveyor36", "0.40", "0");
            llenarPath("Conveyor37", "0.40", "147");
            llenarPath("Conveyor38", "0.10", "138");
            llenarPath("Conveyor39", "0.10", "145");
            llenarPath("Conveyor40", "0.20", "0");
            llenarPath("Conveyor41", "0.30", "241");
            llenarPath("Conveyor42", "0.15", "138");
            llenarPath("Conveyor43", "0.05", "231");
            llenarPath("Conveyor44", "0.30", "282");
            llenarPath("Conveyor45", "0.40", "0");
            llenarPath("Conveyor46", "0.20", "231");
            llenarPath("Conveyor47", "0.25", "124");
            llenarPath("Conveyor48", "0.15", "154");
            llenarPath("Conveyor49", "0.35", "0");
            llenarPath("Conveyor50", "0.35", "63");
            llenarPath("Conveyor51", "0.05", "154");
            llenarPath("Conveyor52", "0.15", "155");
            llenarPath("Conveyor53", "0.10", "269");
            llenarPath("Conveyor54", "0.35", "0");
            llenarPath("Conveyor55", "0.30", "87");
            llenarPath("Conveyor56", "0.35", "155");
            llenarPath("Conveyor57", "0.40", "0");
            llenarPath("Conveyor58", "0.30", "87");
            llenarPath("Conveyor59", "0.10", "269");
            llenarPath("Conveyor60", "0.20", "145");
            llenarPath("Conveyor61", "0.50", "0");
            llenarPath("Conveyor62", "0.25", "147");
            llenarPath("Conveyor63", "0.25", "282");
            #endregion

            #region SalidasServer
            //--Source64-95
            llenarPath("Conveyor64", "0.35", "0");
            llenarPath("Conveyor65", "0.30", "63");
            llenarPath("Conveyor66", "0.15", "124");
            llenarPath("Conveyor67", "0.20", "241");
            llenarPath("Conveyor68", "0.40", "0");
            llenarPath("Conveyor69", "0.40", "147");
            llenarPath("Conveyor70", "0.10", "138");
            llenarPath("Conveyor71", "0.10", "145");
            llenarPath("Conveyor72", "0.20", "0");
            llenarPath("Conveyor73", "0.30", "241");
            llenarPath("Conveyor74", "0.15", "138");
            llenarPath("Conveyor75", "0.05", "231");
            llenarPath("Conveyor76", "0.30", "282");
            llenarPath("Conveyor77", "0.40", "0");
            llenarPath("Conveyor78", "0.20", "231");
            llenarPath("Conveyor79", "0.25", "124");
            llenarPath("Conveyor80", "0.15", "154");
            llenarPath("Conveyor81", "0.35", "0");
            llenarPath("Conveyor82", "0.35", "63");
            llenarPath("Conveyor83", "0.05", "154");
            llenarPath("Conveyor84", "0.15", "155");
            llenarPath("Conveyor85", "0.10", "269");
            llenarPath("Conveyor86", "0.35", "0");
            llenarPath("Conveyor87", "0.30", "87");
            llenarPath("Conveyor88", "0.35", "155");
            llenarPath("Conveyor89", "0.40", "0");
            llenarPath("Conveyor90", "0.30", "87");
            llenarPath("Conveyor91", "0.10", "269");
            llenarPath("Conveyor92", "0.20", "145");
            llenarPath("Conveyor93", "0.50", "0");
            llenarPath("Conveyor94", "0.25", "147");
            llenarPath("Conveyor95", "0.25", "282");
            #endregion

        }

        public void llenarPath(String nombre,String Peso, String distancia)
        {
            int dist = Int32.Parse(distancia);
            int mult = dist * 1000;
            model.Facility.IntelligentObjects[nombre].Properties["SelectionWeight"].Value = Peso;
            model.Facility.IntelligentObjects[nombre].Properties["LogicalLength"].Value = mult.ToString();
            model.Facility.IntelligentObjects[nombre].Properties["InitialDesiredSpeed"].Value = "19.4444"; //"MetersPerSecond"

        }

    }
}
