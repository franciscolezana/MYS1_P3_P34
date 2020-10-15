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

        public ApiSimio()
        {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            model = proyectoApi.Models[1];
            intelligentObjects = model.Facility.IntelligentObjects;
        }


        public void crearModelo()
        {

            try
            {
                //Source
                intelligentObjects.CreateObject("Source", new FacilityLocation(5, 0, -5)); //x,z,y
                model.Facility.IntelligentObjects["Source" + ContadorSource].Properties["InterarrivalTime"].Value = "0.1";

                //Server
                intelligentObjects.CreateObject("Server", new FacilityLocation(10, 0, -5)); //x,z,y
                model.Facility.IntelligentObjects["Server" + ContadorServer].Properties["ProcessingTime"].Value = "2";

                //Sink
                intelligentObjects.CreateObject("Sink", new FacilityLocation(15, 0, -5)); //x,z,y

                //Source a Server
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Source1"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Server1"]).Nodes[0], null);


                //Server a Sink
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Server1"]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Sink1"]).Nodes[0], null);

                //Cambiar propiedades de path
                model.Facility.IntelligentObjects["Path" + ContadorPathSimple].Properties["DrawnToScale"].Value = "False";
                model.Facility.IntelligentObjects["Path" + ContadorPathSimple].Properties["LogicalLength"].Value = "0";

                ContadorPathSimple++;
                ContadorServer++;
                ContadorSink++;
                ContadorSource++;

                //Combiners y Separators
                intelligentObjects.CreateObject("Source", new FacilityLocation(5, 0, -20)); //x,z,y
                intelligentObjects.CreateObject("Sink", new FacilityLocation(30, 0, -20)); //x,z,y

                //combiner
                intelligentObjects.CreateObject("Combiner", new FacilityLocation(10, 0, -20)); //x,z,y

                model.Facility.IntelligentObjects["Combiner" + ContadorCombiner].Properties["FailureType"].Value = "Processing Count Based";
                model.Facility.IntelligentObjects["Combiner" + ContadorCombiner].Properties["CountBetweenFailures"].Value = "10";
                model.Facility.IntelligentObjects["Combiner" + ContadorCombiner].Properties["TimeToRepair"].Value = "1";
                model.Facility.IntelligentObjects["Combiner" + ContadorCombiner].Properties["InitialCapacity"].Value = "5";
                model.Facility.IntelligentObjects["Combiner" + ContadorCombiner].Properties["BatchQuantity"].Value = "10";
                model.Facility.IntelligentObjects["Combiner" + ContadorCombiner].Properties["ProcessingTime"].Value = "2";

                //separator
                intelligentObjects.CreateObject("Separator", new FacilityLocation(20, 0, -20)); //x,z,y

                //Source a Combiner
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Source1"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Combiner1"]).Nodes[0], null);
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Source2"]).Nodes[0], ((IFixedObject)model.Facility.IntelligentObjects["Combiner1"]).Nodes[1], null);

                //Combiner a Separator
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Combiner" + ContadorCombiner]).Nodes[2], ((IFixedObject)model.Facility.IntelligentObjects["Separator" + ContadorSeparator]).Nodes[0], null);

                //Separator a Sink
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Separator" + ContadorSeparator]).Nodes[1], ((IFixedObject)model.Facility.IntelligentObjects["Sink2"]).Nodes[0], null);
                intelligentObjects.CreateLink("Path", ((IFixedObject)model.Facility.IntelligentObjects["Separator" + ContadorSeparator]).Nodes[2], ((IFixedObject)model.Facility.IntelligentObjects["Sink2"]).Nodes[0], null);
                ContadorCombiner++;
                ContadorSeparator++;


                //CREACION MODELO
                SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
                Console.WriteLine("Modelo Creado");
            }
            catch
            {
                Console.WriteLine("Error al crear modelo");
            }
        }
    }
}
