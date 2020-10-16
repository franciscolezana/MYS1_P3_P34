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
        int ContadorTransferNode = 1, contAux = 1;
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

            //CREACION MODELO
            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
            MessageBox.Show("El proyecto Simio ha sido generado");
            Console.WriteLine("Modelo Creado");
          
            //Console.WriteLine("Error al crear modelo");

        }

        public void crearRegiones()
        {
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
        }

        public void crearMapa()
        {
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-20, 0, -73));
            ContadorTransferNode++;

            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(33, 0, -73));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(33, 0, -20));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(49, 0, -20));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(40, 0, -10));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(48, 0, -11));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(50, 0, -20));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(56, 0, -16));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(58, 0, -18));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(56, 0, -22)); //10
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(64, 0, -14));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(38, 0, 12));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(40, 0, 19));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(30, 0, 25));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(21, 0, 40));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(1, 0, 34));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-23, 0, 36));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-57, 0, 21));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-58, 0, 4));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-53, 0, 2));//20
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-59, 0, -5));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-50, 0, -26));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-9, 0, -26));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-8, 0, -32));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-13, 0, -35));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-14, 0, -41));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-21, 0, -46));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-28, 0, -48));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-35, 0, -54));
            ContadorTransferNode++;
            
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(-20, 0, -54));
            ContadorTransferNode++;
        }

        public void unirMapa()
        {
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode1"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode2"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode2"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode3"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode3"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode4"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode4"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode5"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode5"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode6"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode6"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode7"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode7"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode8"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode8"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode9"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode9"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode10"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode10"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode11"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode11"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode12"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode12"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode13"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode13"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode14"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode14"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode15"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode15"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode16"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode16"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode17"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode17"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode18"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode18"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode19"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode19"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode20"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode20"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode21"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode21"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode22"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode22"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode23"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode23"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode24"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode24"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode25"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode25"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode26"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode26"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode27"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode27"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode28"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode28"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode29"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode29"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode30"]), null);
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)model.Facility.IntelligentObjects["TransferNode30"]), ((INodeObject)model.Facility.IntelligentObjects["TransferNode1"]), null);

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

    }
}
