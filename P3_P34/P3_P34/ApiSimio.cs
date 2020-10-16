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

                //CREACION MODELO
                SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
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


    }
}
