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
        private string rutabase = "[MYS1]ModeloBase_P34.spfx";
        private string rutafinal = "[MYS1]ModeloFinal_P34.spfx";
        private string[] warnings;
        private IModel model;
        private IIntelligentObjects intelligentObjects;
        int ContadorPath = 1, ContadorServer = 1, ContadorSource = 1, ContadorSink = 1, ContadorPathSimple = 1, ContadorTimepath = 1, ContadorConveyor = 1, ContadorSeparator = 1, ContadorCombiner = 1;
        int ContadorTransferNode = 1, contAux = 1, ContadorBasicN = 1;
        private SimioAPI.FacilitySize tamanioAvionMilitar;
        //CARNETS
        private string rutacarnets = "[MYS1]ModeloFinalCarnets_P34.spfx";
        private ISimioProject proyectoApiCarnets;
        private string[] warningsC;
        private IModel modelC;
        private IIntelligentObjects intelligentObjectsC;
        private SimioAPI.FacilitySize tamanio;
        private SimioAPI.FacilitySize tamSource;
        private SimioAPI.FacilitySize tamSink;


        public ApiSimio()
        {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            model = proyectoApi.Models[1];
            intelligentObjects = model.Facility.IntelligentObjects;
            //Carnets
            proyectoApiCarnets = SimioProjectFactory.LoadProject(rutabase, out warningsC);
            modelC = proyectoApiCarnets.Models[1];
            intelligentObjectsC = modelC.Facility.IntelligentObjects;
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
            this.configAvion();
            this.crearCarnets();

            //CREACION MODELO
            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
            MessageBox.Show("El proyecto Simio ha sido generado");
            Console.WriteLine("Modelo Creado");

            //Console.WriteLine("Error al crear modelo");
            SimioProjectFactory.SaveProject(proyectoApiCarnets, rutacarnets, out warningsC);
            MessageBox.Show("El proyecto de Carnets ha sido generado");
            Console.WriteLine("Modelo Creado");
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
            intelligentObjects.CreateObject("ModelEntity", new FacilityLocation(-93, 0, -10));
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

            //---Aeropuerto 
            intelligentObjects.CreateObject("Source", new FacilityLocation(-10, 0, -10));  //Metropolitana
            intelligentObjects.CreateObject("Source", new FacilityLocation(10, 0, -60));  //Peten
            intelligentObjects.CreateObject("Source", new FacilityLocation(-53, 0, 14));  //Sur-Occidente 
            #endregion

            //ENTIDAD
            model.Facility.IntelligentObjects["ModelEntity1"].ObjectName = "AvionMilitar";
            model.Facility.IntelligentObjects["ModelEntity2"].ObjectName = "Turista";
            model.Facility.IntelligentObjects["ModelEntity3"].ObjectName = "Avion";


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

            //AEROPUERTO
            model.Facility.IntelligentObjects["Source10"].ObjectName = "MetropolitanaA";
            model.Facility.IntelligentObjects["Source11"].ObjectName = "PetenA";
            model.Facility.IntelligentObjects["Source12"].ObjectName = "SurOccidenteA";
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
            //Frontera con Mexico 962km 
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
            #region Source32-39
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["CentralE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["PetenE"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            #endregion
            //---Salidas desde los server
            #region Metropolitana40-43
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            #endregion
            #region Norte44-47
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            #endregion
            #region Nor-Oriente48-52
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOrienteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            #endregion
            #region Sur-Oriente53-56
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOrienteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            #endregion
            #region Central57-61
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["CentralI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOriente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            #endregion
            #region Sur-Occidente62-64
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            #endregion
            #region Nor-Occidente65-68
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Central"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["NorOccidente"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            #endregion
            #region Peten69-71
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["PetenI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Norte"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["NorOriente"]).Nodes[0], null);
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
            for (int i = 32; i < 72; i++)
            {
                con = "Conveyor" + i;
                model.Facility.IntelligentObjects[con].Properties["DrawnToScale"].Value = "False";
            }
            #region SalidasSource
            //--Server32-29
            llenarPath("Conveyor32", "1", "0");
            llenarPath("Conveyor33", "1", "0");
            llenarPath("Conveyor34", "1", "0");
            llenarPath("Conveyor35", "1", "0");
            llenarPath("Conveyor36", "1", "0");
            llenarPath("Conveyor37", "1", "0");
            llenarPath("Conveyor38", "1", "0");
            llenarPath("Conveyor39", "1", "0");
            #endregion

            #region SalidasServer
            //--Source64-95
            llenarPath("Conveyor40", "0.35", "0");
            llenarPath("Conveyor41", "0.30", "63");
            llenarPath("Conveyor42", "0.15", "124");
            llenarPath("Conveyor43", "0.20", "241");
            llenarPath("Conveyor44", "0.40", "0");
            llenarPath("Conveyor45", "0.40", "147");
            llenarPath("Conveyor46", "0.10", "138");
            llenarPath("Conveyor47", "0.10", "145");
            llenarPath("Conveyor48", "0.20", "0");
            llenarPath("Conveyor49", "0.30", "241");
            llenarPath("Conveyor50", "0.15", "138");
            llenarPath("Conveyor51", "0.05", "231");
            llenarPath("Conveyor52", "0.30", "282");
            llenarPath("Conveyor53", "0.40", "0");
            llenarPath("Conveyor54", "0.20", "231");
            llenarPath("Conveyor55", "0.25", "124");
            llenarPath("Conveyor56", "0.15", "154");
            llenarPath("Conveyor57", "0.35", "0");
            llenarPath("Conveyor58", "0.35", "63");
            llenarPath("Conveyor59", "0.05", "154");
            llenarPath("Conveyor60", "0.15", "155");
            llenarPath("Conveyor61", "0.10", "269");
            llenarPath("Conveyor62", "0.35", "0");
            llenarPath("Conveyor63", "0.30", "87");
            llenarPath("Conveyor64", "0.35", "155");
            llenarPath("Conveyor65", "0.40", "0");
            llenarPath("Conveyor66", "0.30", "87");
            llenarPath("Conveyor67", "0.10", "269");
            llenarPath("Conveyor68", "0.20", "145");
            llenarPath("Conveyor69", "0.50", "0");
            llenarPath("Conveyor70", "0.25", "147");
            llenarPath("Conveyor71", "0.25", "282");
            #endregion

        }

        public void colocarNombre()
        {
            #region SalidasSource
            //--Server32-39
            setNameObject("Conveyor32", "MetropolitanaE", "Metropolitana");
            setNameObject("Conveyor33", "NorteE", "Norte");
            setNameObject("Conveyor34", "NorOrienteE", "NorOriente");
            setNameObject("Conveyor35", "SurOrienteE", "SurOriente");
            setNameObject("Conveyor36", "CentralE", "Central");
            setNameObject("Conveyor37", "SurOccidenteE", "SurOccidente");
            setNameObject("Conveyor38", "NorOccidenteE", "NorOccidente");
            setNameObject("Conveyor39", "PetenE", "Peten");
            #endregion

            #region SalidasServer
            //--Source64-95
            setNameObject("Conveyor40", "Metropolitana", "MetropolitanaI");
            setNameObject("Conveyor41", "Metropolitana", "Central");
            setNameObject("Conveyor42", "Metropolitana", "NorOriente");
            setNameObject("Conveyor43", "Metropolitana", "SurOriente");
            setNameObject("Conveyor44", "Norte", "NorteI");
            setNameObject("Conveyor45", "Norte", "Peten");
            setNameObject("Conveyor46", "Norte", "NorOriente");
            setNameObject("Conveyor47", "Norte", "NorOccidente");
            setNameObject("Conveyor48", "NorOriente", "NorOrienteI");
            setNameObject("Conveyor49", "NorOriente", "Metropolitana");
            setNameObject("Conveyor50", "NorOriente", "Norte");
            setNameObject("Conveyor51", "NorOriente", "SurOriente");
            setNameObject("Conveyor52", "NorOriente", "Peten");
            setNameObject("Conveyor53", "SurOriente", "SurOrienteI");
            setNameObject("Conveyor54", "SurOriente", "NorOriente");
            setNameObject("Conveyor55", "SurOriente", "Metropolitana");
            setNameObject("Conveyor56", "SurOriente", "Central");
            setNameObject("Conveyor57", "Central", "CentralI");
            setNameObject("Conveyor58", "Central", "Metropolitana");
            setNameObject("Conveyor59", "Central", "SurOriente");
            setNameObject("Conveyor60", "Central", "SurOccidente");
            setNameObject("Conveyor61", "Central", "NorOccidente");
            setNameObject("Conveyor62", "SurOccidente", "SurOccidenteI");
            setNameObject("Conveyor63", "SurOccidente", "NorOccidente");
            setNameObject("Conveyor64", "SurOccidente", "Central");
            setNameObject("Conveyor65", "NorOccidente", "NorOccidenteI");
            setNameObject("Conveyor66", "NorOccidente", "SurOccidente");
            setNameObject("Conveyor67", "NorOccidente", "Central");
            setNameObject("Conveyor68", "NorOccidente", "Norte");
            setNameObject("Conveyor69", "Peten", "PetenI");
            setNameObject("Conveyor70", "Peten", "Norte");
            setNameObject("Conveyor71", "Peten", "NorOriente");
            #endregion

            setNameObject("Conveyor72", "Aurora", "QUEDA");
            setNameObject("Conveyor73", "Aurora", "Metropolitana");
            setNameObject("Conveyor74", "MundoMaya", "QUEDA");
            setNameObject("Conveyor75", "MundoMaya", "Peten");
            setNameObject("Conveyor76", "Quetzaltenango", "QUEDA");
            setNameObject("Conveyor77", "Quetzaltenango", "SurOccidente");
        }

        public void setNameObject(string obj, string nameO, string nameD)
        {
            string name = "ruta" + nameO + nameD;
            model.Facility.IntelligentObjects[obj].ObjectName = name;
        }

        public void llenarPath(String nombre,String Peso, String distancia)
        {
            int dist = Int32.Parse(distancia);
            int mult = dist * 1000;
            model.Facility.IntelligentObjects[nombre].Properties["SelectionWeight"].Value = Peso;
            model.Facility.IntelligentObjects[nombre].Properties["LogicalLength"].Value = mult.ToString();
            model.Facility.IntelligentObjects[nombre].Properties["InitialDesiredSpeed"].Value = "19.4444"; //"MetersPerSecond"

        }

        public void configAvion() {
            //---Asociar entidades a los aeropuertos
            model.Facility.IntelligentObjects["MetropolitanaA"].Properties["EntityType"].Value = "Avion";
            model.Facility.IntelligentObjects["PetenA"].Properties["EntityType"].Value = "Avion";
            model.Facility.IntelligentObjects["SurOccidenteA"].Properties["EntityType"].Value = "Avion";
            //--Tasa Llegada
            model.Facility.IntelligentObjects["MetropolitanaA"].Properties["InterarrivalTime"].Value = "Random.Exponential(35)";
            model.Facility.IntelligentObjects["PetenA"].Properties["InterarrivalTime"].Value = "Random.Exponential(50)";
            model.Facility.IntelligentObjects["SurOccidenteA"].Properties["InterarrivalTime"].Value = "Random.Exponential(70)";
            //--Llegadas por arribo
            model.Facility.IntelligentObjects["MetropolitanaA"].Properties["EntitiesPerArrival"].Value = "70";
            model.Facility.IntelligentObjects["PetenA"].Properties["EntitiesPerArrival"].Value = "40";
            model.Facility.IntelligentObjects["SurOccidenteA"].Properties["EntitiesPerArrival"].Value = "30";

            //--Crear RUTAS
            //72-73
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaA"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaA"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Metropolitana"]).Nodes[0], null);
            //74-75
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["PetenA"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["PetenI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["PetenA"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Peten"]).Nodes[0], null);
            //76-77
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteA"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteI"]).Nodes[0], null);
            intelligentObjects.CreateLink("Conveyor", ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteA"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["SurOccidente"]).Nodes[0], null);
            
            //--Por peso source
            ((IFixedObject)model.Facility.IntelligentObjects["MetropolitanaA"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["PetenA"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";
            ((IFixedObject)model.Facility.IntelligentObjects["SurOccidenteA"]).Nodes[0].Properties["OutboundLinkRule"].Value = "By Link Weight";

            //--Colocar Peso
            model.Facility.IntelligentObjects["Conveyor72"].Properties["SelectionWeight"].Value = "0.5";//se queda en la region
            model.Facility.IntelligentObjects["Conveyor73"].Properties["SelectionWeight"].Value = "0.5";//se va a otra region

            model.Facility.IntelligentObjects["Conveyor74"].Properties["SelectionWeight"].Value = "0.3";//se queda 
            model.Facility.IntelligentObjects["Conveyor75"].Properties["SelectionWeight"].Value = "0.7";//se va

            model.Facility.IntelligentObjects["Conveyor76"].Properties["SelectionWeight"].Value = "0.4";//se queda
            model.Facility.IntelligentObjects["Conveyor77"].Properties["SelectionWeight"].Value = "0.6";//se va

            this.colocarNombre();

        }

        //CARNETS

        private void crearCarnets()
        {            

            //-------------------201503777
            //2
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-23, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-20, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-20, 0, -3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-23, 0, -3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-23, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-20, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode1"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode2"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode2"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode3"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode3"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode4"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode4"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode5"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode5"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode6"]), null);

            //0
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-18, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-15, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-15, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-18, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode7"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode8"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode8"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode9"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode9"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode10"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode10"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode7"]), null);
            
            //1
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-13, 0, -4));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-12, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-12, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-11, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-13, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode11"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode12"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode12"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode13"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode13"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode14"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode14"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode15"]), null);
            
            //5
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-6, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-9, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-9, 0, -3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-6, 0, -3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-6, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-9, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode16"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode17"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode17"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode18"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode18"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode19"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode19"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode20"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode20"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode21"]), null);

            //0
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-4, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-1, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-1, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-4, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode22"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode23"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode23"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode24"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode24"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode25"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode25"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode22"]), null);
           
            //3
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(1, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(4, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(4, 0, -3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(2, 0, -3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(4, 0, -1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(1, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode26"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode27"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode27"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode30"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode30"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode31"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode28"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode29"]), null);
            //7
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(6, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(9, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(7, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode32"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode33"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode33"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode34"]), null);
            //7
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(11, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(14, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(12, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode35"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode36"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode36"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode37"]), null);
            //7
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(16, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(19, 0, -5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(17, 0, -1));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode38"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode39"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode39"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode40"]), null);

            //-------------------201503609

            //2
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-23, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-20, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-20, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-23, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-23, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-20, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode41"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode42"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode42"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode43"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode43"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode44"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode44"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode45"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode45"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode46"]), null);

            //0
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-18, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-15, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-15, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-18, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode47"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode48"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode48"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode49"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode49"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode50"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode50"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode47"]), null);

            //1
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-13, 0, 2));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-12, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-12, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-11, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-13, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode51"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode52"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode52"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode53"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode53"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode54"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode54"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode55"]), null);

            //5
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-6, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-9, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-9, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-6, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-6, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-9, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode56"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode57"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode57"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode58"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode58"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode59"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode59"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode60"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode60"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode61"]), null);

            //0
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-4, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-1, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-1, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(-4, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode62"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode63"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode63"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode64"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode64"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode65"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode65"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode62"]), null);

            //3
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(1, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(4, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(4, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(2, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(4, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(1, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode66"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode67"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode67"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode70"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode70"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode71"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode68"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode69"]), null);
            //6
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(9, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(6, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(6, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(9, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(9, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(6, 0, 3));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode72"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode73"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode73"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode74"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode74"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode75"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode75"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode76"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode76"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode77"]), null);
            //0
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(11, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(14, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(14, 0, 5));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(11, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode78"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode79"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode79"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode80"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode80"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode81"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode81"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode78"]), null);
            //9
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(19, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(16, 0, 3));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(16, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(19, 0, 1));
            intelligentObjectsC.CreateObject("TransferNode", new FacilityLocation(19, 0, 5));

            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode82"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode83"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode83"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode84"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode84"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode85"]), null);
            intelligentObjectsC.CreateLink("Path", ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode85"]), ((INodeObject)modelC.Facility.IntelligentObjects["TransferNode86"]), null);

            for (int i = 1; i < 87; i++)
            {
                modelC.Facility.IntelligentObjects["TransferNode" + i].ObjectName = "T" + i;
            }

        }

    }
}
